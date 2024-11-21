#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Utils;
using MiNET.Utils.Collections;

namespace MiNET.Net.RakNet
{
	public class RakSession : INetworkHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(RakSession));

		private readonly IPacketSender _packetSender;

		private long _lastOrderingIndex = -1; // That's the first message with wrapper
		private readonly SemaphoreSlim _packetProcessingSemaphore = new SemaphoreSlim(0, int.MaxValue);

		private object _eventSync = new object();

		private readonly ConcurrentPriorityQueue<int, Packet> _orderingBufferQueue = new ConcurrentPriorityQueue<int, Packet>();
		private CancellationTokenSource _cancellationToken;
		private Thread _orderedQueueProcessingThread;


		public object EncryptionSyncRoot { get; } = new object();

		public ConnectionInfo ConnectionInfo { get; }

		public ICustomMessageHandler CustomMessageHandler { get; set; }

		public bool EnableCompression { get; set; } = false;

		public string Username { get; set; }
		public IPEndPoint EndPoint { get; private set; }
		public short MtuSize { get; set; }
		public long NetworkIdentifier { get; set; }

		public int DatagramSequenceNumber = -1;
		public int ReliableMessageNumber = -1;
		public int SplitPartId = 0;
		public int OrderingIndex = -1;
		public int ErrorCount { get; set; }

		public bool Evicted { get; set; }

		public ConnectionState State { get; set; } = ConnectionState.Unconnected;

		public DateTime LastUpdatedTime { get; set; }
		public bool WaitForAck { get; set; }
		public int ResendCount { get; set; }

		public long Ping { get; set; }

		/// <summary>
		/// </summary>
		public long Syn { get; set; } = 300;

		/// <summary>
		///     Round Trip Time.
		///     <code>RTT = RTT * 0.875 + rtt * 0.125</code>
		/// </summary>
		public long Rtt { get; set; } = 300;

		/// <summary>
		///     Round Trip Time Variance.
		///     <code>RTTVar = RTTVar * 0.875 + abs(RTT - rtt)) * 0.125</code>
		/// </summary>
		public long RttVar { get; set; }

		/// <summary>
		///     Retransmission Time Out.
		///     <code>RTO = RTT + 4 * RTTVar</code>
		/// </summary>
		public long Rto { get; set; }

		public long InactivityTimeout { get; }
		public int ResendThreshold { get; }

		public ConcurrentDictionary<int, SplitPartPacket[]> Splits { get; } = new ConcurrentDictionary<int, SplitPartPacket[]>();
		public ConcurrentQueue<int> OutgoingAckQueue { get; } = new ConcurrentQueue<int>();
		public ConcurrentDictionary<int, Datagram> WaitingForAckQueue { get; } = new ConcurrentDictionary<int, Datagram>();


		public RakSession(ConnectionInfo connectionInfo, IPacketSender packetSender, IPEndPoint endPoint, short mtuSize, ICustomMessageHandler messageHandler = null)
		{
			_reliabilityHandlers = new Dictionary<Reliability, Action<Packet>>
			{
				{ Reliability.ReliableOrdered, AddToOrderedChannel },
				{ Reliability.ReliableOrderedWithAckReceipt, AddToOrderedChannel },
				{ Reliability.UnreliableSequenced, AddToSequencedChannel },
				{ Reliability.ReliableSequenced, AddToSequencedChannel },
				{ Reliability.Unreliable, HandlePacket },
				{ Reliability.UnreliableWithAckReceipt, HandlePacket },
				{ Reliability.Reliable, HandlePacket },
				{ Reliability.ReliableWithAckReceipt, HandlePacket }
			};
			Log.Debug($"Create session for {endPoint}");

			_packetSender = packetSender;
			ConnectionInfo = connectionInfo;
			CustomMessageHandler = messageHandler ?? new DefaultMessageHandler();
			EndPoint = endPoint;
			MtuSize = mtuSize;

			InactivityTimeout = Config.GetProperty("InactivityTimeout", 8500);
			ResendThreshold = Config.GetProperty("ResendThreshold", 10);

			_cancellationToken = new CancellationTokenSource();
		}

		private readonly Dictionary<Reliability, Action<Packet>> _reliabilityHandlers;

		internal void HandleRakMessage(Packet message)
		{
			if (message == null)
				return;

			if (_reliabilityHandlers.TryGetValue(message.ReliabilityHeader.Reliability, out var handler))
			{
				handler(message);
			}
			else
			{
				Log.Warn($"Unhandled reliability: {message.ReliabilityHeader.Reliability}");
			}
		}

		public void AddToSequencedChannel(Packet message)
		{
			AddToOrderedChannel(message);
		}

		public void AddToOrderedChannel(Packet message)
		{
			try
			{
				if (_cancellationToken.Token.IsCancellationRequested)
					return;

				if (message.ReliabilityHeader.OrderingIndex <= _lastOrderingIndex)
					return;

				lock (_eventSync)
				{
					if (_orderedQueueProcessingThread == null)
					{
						_orderedQueueProcessingThread = new Thread(async () => await ProcessOrderedQueueAsync())
						{
							IsBackground = true,
							Name = $"Ordering Thread [{EndPoint}]"
						};
						_orderedQueueProcessingThread.Start();
						if (Log.IsDebugEnabled)
							Log.Debug($"Started processing thread for {Username}");
					}

					_orderingBufferQueue.Enqueue(message.ReliabilityHeader.OrderingIndex, message);
					
					_packetProcessingSemaphore.Release();
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Error in AddToOrderedChannel for packet {message?.Id}", ex);
			}
		}


		private async Task ProcessOrderedQueueAsync()
		{
			try
			{
				while (!_cancellationToken.IsCancellationRequested && !ConnectionInfo.IsEmulator)
				{
					await _packetProcessingSemaphore.WaitAsync(_cancellationToken.Token);

					if (_orderingBufferQueue.TryPeek(out var pair))
					{
						if (pair.Key == _lastOrderingIndex + 1)
						{
							if (_orderingBufferQueue.TryDequeue(out var dequeuedPair))
							{
								_lastOrderingIndex = dequeuedPair.Key;
								HandlePacket(dequeuedPair.Value);
							}
						}
						else if (pair.Key <= _lastOrderingIndex)
						{
							if (_orderingBufferQueue.TryDequeue(out var outdatedPair))
							{
								Log.Debug($"{Username} - Skipping outdated packet with index {outdatedPair.Key}");
								outdatedPair.Value.PutPool();
							}
						}
						else
						{
							Log.Debug($"{Username} - Packet sequence gap. Expected {_lastOrderingIndex + 1}, but got {pair.Key}");
						}
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				Log.Error($"Error in ProcessOrderedQueueAsync for {Username}", ex);
			}
		}

		private void HandlePacket(Packet message)
		{
			if (message == null) return;

			try
			{
				RakOfflineHandler.TraceReceive(Log, message);

				if (message.Id < (int) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
				{
					switch (message)
					{
						case ConnectedPing connectedPing:
							HandleConnectedPing(connectedPing);
							break;
						case ConnectedPong connectedPong:
							HandleConnectedPong(connectedPong);
							break;
						case DetectLostConnections _:
							break;
						case ConnectionRequest connectionRequest:
							HandleConnectionRequest(connectionRequest);
							break;
						case ConnectionRequestAccepted connectionRequestAccepted:
							HandleConnectionRequestAccepted(connectionRequestAccepted);
							break;
						case NewIncomingConnection newIncomingConnection:
							HandleNewIncomingConnection(newIncomingConnection);
							break;
						case DisconnectionNotification _:
							HandleDisconnectionNotification();
							break;
						default:
							Log.Error($"Unhandled packet: {message.GetType().Name} 0x{message.Id:X2} for user: {Username}, IP {EndPoint.Address}");
							if (Log.IsDebugEnabled) Log.Warn($"Unknown packet 0x{message.Id:X2}\n{Packet.HexDump(message.Bytes)}");
							break;
					}
				}
				else
				{
					try
					{
						CustomMessageHandler.HandlePacket(message);
					}
					catch (Exception e)
					{
						// ignore
						Log.Warn($"Custom message handler error", e);
					}
				}

				if (message.Timer.IsRunning)
				{
					long elapsedMilliseconds = message.Timer.ElapsedMilliseconds;
					if (elapsedMilliseconds > 1000)
					{
						Log.WarnFormat("Packet (0x{1:x2}) handling too long {0}ms for {2}", elapsedMilliseconds, message.Id, Username);
					}
				}
				else
				{
					Log.WarnFormat("Packet (0x{0:x2}) timer not started for {1}.", message.Id, Username);
				}
			}
			catch (Exception e)
			{
				Log.Error("Packet handling", e);
				throw;
			}
			finally
			{
				message?.PutPool();
			}
		}

		private void HandleConnectedPong(ConnectedPong connectedPong)
		{
			// Ignore
		}

		protected virtual void HandleConnectedPing(ConnectedPing message)
		{
			var packet = ConnectedPong.CreateObject();
			packet.sendpingtime = message.sendpingtime;
			packet.sendpongtime = Stopwatch.GetTimestamp() / (Stopwatch.Frequency / 1000);

			Ping = packet.sendpongtime - packet.sendpingtime;

			if (!string.IsNullOrEmpty(Username))
			{
				lock (Player.Pings)
				{
					Player.Pings[Username] = Ping;
				}
			}

			SendPacket(packet);
		}

		protected virtual void HandleConnectionRequest(ConnectionRequest message)
		{
			Log.DebugFormat("Connection request from: {0}", EndPoint.Address);

			var response = ConnectionRequestAccepted.CreateObject();
			response.NoBatch = true;
			response.systemAddress = new IPEndPoint(IPAddress.Loopback, 19132);
			response.systemAddresses = new IPEndPoint[20];
			response.systemAddresses[0] = new IPEndPoint(IPAddress.Loopback, 19132);
			response.incomingTimestamp = message.timestamp;
			response.serverTimestamp = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

			for (int i = 1; i < 20; i++)
			{
				response.systemAddresses[i] = new IPEndPoint(IPAddress.Any, 19132);
			}

			SendPacket(response);
		}

		protected virtual void HandleNewIncomingConnection(NewIncomingConnection message)
		{
			Log.Debug($"New incoming connection from {EndPoint.Address} {EndPoint.Port}");

			State = ConnectionState.Connected;
		}

		private void HandleConnectionRequestAccepted(ConnectionRequestAccepted message)
		{
			SendNewIncomingConnection();

			State = ConnectionState.Connected;

			CustomMessageHandler?.Connected();
		}

		public void SendNewIncomingConnection()
		{
			var packet = NewIncomingConnection.CreateObject();
			packet.clientendpoint = EndPoint;
			packet.systemAddresses = new IPEndPoint[20];
			for (int i = 0; i < 20; i++)
			{
				packet.systemAddresses[i] = new IPEndPoint(IPAddress.Any, 0);
			}

			SendPacket(packet);
		}


		protected virtual void HandleDisconnectionNotification()
		{
			Disconnect("Client requested disconnected", false);
		}

		public virtual void Disconnect(string reason, bool sendDisconnect = true)
		{
			CustomMessageHandler?.Disconnect("RakSession: " + reason, sendDisconnect);
			Close();
		}

		public void DetectLostConnection()
		{
			var ping = DetectLostConnections.CreateObject();
			SendPacket(ping);
		}

		// MCPE Login handling


		private ConcurrentQueue<Packet> _sendQueue = new ConcurrentQueue<Packet>();

		public void SendPacket(Packet packet)
		{
			if (packet == null)
				return;

			if (State == ConnectionState.Unconnected)
			{
				if (Log.IsDebugEnabled)
				{
					Log.Debug($"Ignoring send of packet {packet.GetType().Name} because session is not connected");
				}
				packet.PutPool();
				return;
			}

			RakOfflineHandler.TraceSend(packet);
			
			_sendQueue.Enqueue(packet);
		}

		private int _tickCounter;

		public async Task SendTickAsync(RakConnection connection)
		{
			try
			{
				if (_tickCounter++ >= 5)
				{
					await Task.WhenAll(SendAckQueueAsync(), UpdateAsync(), SendQueueAsync(), connection.UpdateAsync(this));
					_tickCounter = 0;
				}
				else
				{
					await Task.WhenAll(SendAckQueueAsync(), SendQueueAsync());
				}
			}
			catch (Exception e)
			{
				Log.Warn(e);
			}
		}


		//private object _updateSync = new object();
		private SemaphoreSlim _updateSync = new SemaphoreSlim(1, 1);

		private async Task UpdateAsync()
		{
			if (Evicted) return;

			if (MiNetServer.FastThreadPool == null) return;

			if (!await _updateSync.WaitAsync(0)) return;

			try
			{
				if (Evicted) return;

				long now = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
				long lastUpdate = LastUpdatedTime.Ticks / TimeSpan.TicksPerMillisecond;

				if (lastUpdate + InactivityTimeout + 3000 < now)
				{
					Evicted = true;
					// Disconnect user
					MiNetServer.FastThreadPool.QueueUserWorkItem(() =>
					{
						Disconnect("You've been kicked with reason: Network timeout.");
						Close();
					});

					return;
				}

				if (State != ConnectionState.Connected && CustomMessageHandler != null && lastUpdate + 3000 < now)
				{
					MiNetServer.FastThreadPool.QueueUserWorkItem(() => { Disconnect("You've been kicked with reason: Lost connection."); });
					return;
				}
			}
			finally
			{
				_updateSync.Release();
			}
		}

		private async Task SendAckQueueAsync()
		{
			RakSession session = this;
			var queue = session.OutgoingAckQueue;
			int queueCount = queue.Count;

			if (queueCount == 0) return;

			var acks = Acks.CreateObject();
			for (int i = 0; i < queueCount; i++)
			{
				if (!queue.TryDequeue(out int ack)) break;

				Interlocked.Increment(ref ConnectionInfo.NumberOfAckSent);
				acks.acks.Add(ack);
			}

			if (acks.acks.Count > 0)
			{
				byte[] data = acks.Encode();
				await _packetSender.SendDataAsync(data, session.EndPoint);
			}

			acks.PutPool();
		}

		private SemaphoreSlim _syncHack = new SemaphoreSlim(1, 1);

		[SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
		public async Task SendQueueAsync(int millisecondsWait = 0)
		{
			if (_sendQueue.IsEmpty)
				return;
			
			if (!await _syncHack.WaitAsync(millisecondsWait))
				return;

			try
			{
				var sendList = new List<Packet>();
				
				while (_sendQueue.TryDequeue(out var packet))
				{
					if (packet == null)
						continue;

					if (State == ConnectionState.Unconnected)
					{
						packet.PutPool();
						continue;
					}

					sendList.Add(packet);
				}

				if (sendList.Count == 0)
					return;
				
				var prepareSend = CustomMessageHandler.PrepareSend(sendList);
				var preppedSendList = new List<Packet>();

				foreach (var packet in prepareSend)
				{
					var message = packet;

					if (CustomMessageHandler != null)
					{
						message = CustomMessageHandler.HandleOrderedSend(message);
					}

					var reliability = message.ReliabilityHeader.Reliability;
					if (reliability == Reliability.Undefined)
					{
						reliability = Reliability.Reliable;
					}

					preppedSendList.Add(message);
				}
				
				await _packetSender.SendPacketAsync(this, preppedSendList);
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
			finally
			{
				_syncHack.Release();
			}
		}


		public void SendDirectPacket(Packet packet)
		{
			if (packet.ReliabilityHeader.Reliability == Reliability.ReliableOrdered)
				throw new Exception($"Can't send direct messages with ordering. The offending packet was {packet.GetType().Name}");

			if (packet.ReliabilityHeader.Reliability == Reliability.Undefined)
				packet.ReliabilityHeader.Reliability = Reliability.Reliable; // Questionable practice

			_packetSender.SendPacketAsync(this, packet).Wait();
		}

		public void SendPrepareDirectPacket(Packet packet)
		{
			List<Packet> prepareSend = CustomMessageHandler.PrepareSend(new List<Packet> { packet });
			var preppedSendList = new List<Packet>();
			foreach (Packet preparePacket in prepareSend)
			{
				Packet message = preparePacket;

				if (CustomMessageHandler != null)
					message = CustomMessageHandler.HandleOrderedSend(message);

				Reliability reliability = message.ReliabilityHeader.Reliability;
				if (reliability == Reliability.Undefined)
					reliability = Reliability.Reliable; // Questionable practice

				_packetSender.SendPacketAsync(this, message).Wait();
			}
		}

		public IPEndPoint GetClientEndPoint()
		{
			return EndPoint;
		}

		public long GetNetworkNetworkIdentifier()
		{
			return NetworkIdentifier;
		}

		public void Close()
		{
			if (!ConnectionInfo.RakSessions.TryRemove(EndPoint, out _))
			{
				return;
			}

			try
			{
				SendDirectPacket(DisconnectionNotification.CreateObject());
				SendQueueAsync(500).Wait();
				
				State = ConnectionState.Unconnected;
				Evicted = true;
				CustomMessageHandler = null;
				
				_cancellationToken.Cancel();
				
				_packetProcessingSemaphore.Release(); 
				
				_orderingBufferQueue.Clear();
				_packetSender.Close(this);
				_orderedQueueProcessingThread?.Join(100);
				_orderedQueueProcessingThread = null;

				_cancellationToken.Dispose();
				_packetProcessingSemaphore.Dispose();

				if (Log.IsDebugEnabled)
				{
					Log.Info($"Closed network session for player {Username}");
				}
			}
			catch (Exception ex)
			{
				Log.Warn($"Error during session closure for {Username}", ex);
			}
		}
	}
}