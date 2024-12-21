using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Utils;
using MiNET.Utils.IO;

namespace MiNET.Net.RakNet;

public class RakConnection : IPacketSender
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(RakConnection));

	private UdpClient _listener;
	private readonly IPEndPoint _endpoint;
	private readonly DedicatedThreadPool _receiveThreadPool;
	private CancellationTokenSource _cts;
	private HighPrecisionTimer _tickerHighPrecisionTimer;
	private readonly ConcurrentDictionary<IPEndPoint, RakSession> _rakSessions = new();
	private readonly GreyListManager _greyListManager;
	public readonly RakOfflineHandler _rakOfflineHandler;
	public ConnectionInfo ConnectionInfo { get; }

	public bool FoundServer => _rakOfflineHandler.HaveServer;
	private bool isListening = true;

	public bool AutoConnect
	{
		get => _rakOfflineHandler.AutoConnect;
		set => _rakOfflineHandler.AutoConnect = value;
	}

	public IPEndPoint RemoteEndpoint { get; set; }
	public string RemoteServerName { get; set; }

	public Func<RakSession, ICustomMessageHandler> CustomMessageHandlerFactory { get; set; }

	public RakConnection(IPEndPoint endpoint, GreyListManager greyListManager, MotdProvider motdProvider, DedicatedThreadPool threadPool = null)
	{
		_endpoint = endpoint ?? new IPEndPoint(IPAddress.Any, 0);
		_greyListManager = greyListManager;
		_receiveThreadPool = threadPool ?? new DedicatedThreadPool(new DedicatedThreadPoolSettings(100, "Datagram_Rcv_Thread"));
		ConnectionInfo = new ConnectionInfo(_rakSessions);
		_rakOfflineHandler = new RakOfflineHandler(this, this, _greyListManager, motdProvider, ConnectionInfo);
	}
	
	public void Start()
	{
		if (_listener != null) return;

		Log.Debug($"Creating listener for packets on {_endpoint}");
		_listener = CreateListener(_endpoint);
		_cts = new CancellationTokenSource();
		Task.Run(() => ReceiveDatagramAsync(_cts.Token));
		_tickerHighPrecisionTimer = new HighPrecisionTimer(10, SendTick, true);
	}
	
	private async Task ReceiveDatagramAsync(CancellationToken token)
	{
		while (isListening && !token.IsCancellationRequested)
		{
			try
			{
				UdpReceiveResult received = await _listener.ReceiveAsync(token).ConfigureAwait(false);
				_receiveThreadPool.QueueUserWorkItem(() => HandleReceivedData(received));
			}
			catch (SocketException e) when (e.SocketErrorCode == SocketError.Interrupted)
			{
				Log.Warn("Socket interrupted", e);
				break;
			}
			catch (Exception e)
			{
				Log.Error("Error receiving datagram", e);
			}
		}
		_listener.Close();
	}
	
	private void HandleReceivedData(UdpReceiveResult received)
	{
		byte[] receiveBytes = received.Buffer;
		IPEndPoint senderEndpoint = received.RemoteEndPoint;
		Interlocked.Increment(ref ConnectionInfo.NumberOfPacketsInPerSecond);
		Interlocked.Add(ref ConnectionInfo.TotalPacketSizeInPerSecond, receiveBytes.Length);

		if (_greyListManager != null && !_greyListManager.IsWhitelisted(senderEndpoint.Address))
		{
			if (_greyListManager.IsBlacklisted(senderEndpoint.Address) || _greyListManager.IsGreylisted(senderEndpoint.Address)) return;
		}
		ReceiveDatagram(receiveBytes, senderEndpoint);
	}

	public bool TryLocate(out (IPEndPoint serverEndPoint, string serverName) serverInfo, int numberOfAttempts = int.MaxValue)
	{
		return TryLocate(null, out serverInfo, numberOfAttempts);
	}

	public bool TryLocate(IPEndPoint targetEndPoint, out (IPEndPoint serverEndPoint, string serverName) serverInfo, int numberOfAttempts = int.MaxValue)
	{
		Start(); // Make sure we have started.

		bool oldAutoConnect = AutoConnect;
		AutoConnect = false;

		while (!FoundServer && numberOfAttempts-- > 0)
		{
			SendUnconnectedPingInternal(targetEndPoint);
			Task.Delay(100).Wait();
		}

		serverInfo = (RemoteEndpoint, RemoteServerName);

		AutoConnect = oldAutoConnect;

		return FoundServer;
	}


	public bool TryConnect(IPEndPoint targetEndPoint, int numberOfAttempts = int.MaxValue, short mtuSize = 1500)
	{
		Start(); // Make sure we have started the listener

		RakSession session;
		do
		{
			_rakOfflineHandler.SendOpenConnectionRequest1(targetEndPoint, mtuSize);
			Task.Delay(300).Wait();
		} while (!_rakSessions.TryGetValue(targetEndPoint, out session) && numberOfAttempts-- > 0);

		if (session == null) return false;
		while (session.State != ConnectionState.Connected && numberOfAttempts-- > 0) Task.Delay(100).Wait();
		return session.State == ConnectionState.Connected;
	}

	private void SendUnconnectedPingInternal(IPEndPoint targetEndPoint)
	{
		var packet = new UnconnectedPing
		{
			pingId = Stopwatch.GetTimestamp() /*incoming.pingId*/,
			guid = _rakOfflineHandler.ClientGuid
		};

		byte[] data = packet.Encode();

		SendData(data, targetEndPoint ?? new IPEndPoint(IPAddress.Broadcast, 19132));
	}

	public void Stop()
	{
		Log.Info("Shutting down...");
		_cts?.Cancel();
		_listener?.Close();
		isListening = false;
		_tickerHighPrecisionTimer?.Dispose();
	}


	private static UdpClient CreateListener(IPEndPoint endpoint)
	{
		var listener = new UdpClient();

		if (Environment.OSVersion.Platform != PlatformID.MacOSX)
		{
			listener.Client.ReceiveBufferSize = int.MaxValue;
			listener.Client.SendBufferSize = int.MaxValue;
		}

		listener.DontFragment = false;
		listener.EnableBroadcast = true;

		if (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX)
		{
			uint IOC_IN = 0x80000000;
			uint IOC_VENDOR = 0x18000000;
			uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
			listener.Client.IOControl((int) SIO_UDP_CONNRESET, [Convert.ToByte(false)], null);
		}

		listener.Client.Bind(endpoint);
		return listener;
	}

	public void Close(RakSession session)
	{
		ConcurrentDictionary<int, Datagram> ackQueue = session.WaitingForAckQueue;
		foreach (KeyValuePair<int, Datagram> kvp in ackQueue)
			if (ackQueue.TryRemove(kvp.Key, out Datagram datagram)) datagram.PutPool();

		ConcurrentDictionary<int, SplitPartPacket[]> splits = session.Splits;
		foreach (KeyValuePair<int, SplitPartPacket[]> kvp in splits)
		{
			if (!splits.TryRemove(kvp.Key, out SplitPartPacket[] splitPartPackets)) continue;
			if (splitPartPackets == null) continue;

			foreach (SplitPartPacket packet in splitPartPackets) packet?.PutPool();
		}

		ackQueue.Clear();
		splits.Clear();
	}

	private void ReceiveDatagram(ReadOnlyMemory<byte> receivedBytes, IPEndPoint clientEndpoint)
	{
		var header = new DatagramHeader(receivedBytes.Span[0]);

		if (!header.IsValid)
		{
			byte messageId = receivedBytes.Span[0];

			if (messageId <= (byte) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
				_rakOfflineHandler.HandleOfflineRakMessage(receivedBytes, clientEndpoint);
			else
				Log.Warn($"Receive invalid message, but not a RakNet message. Message ID={messageId}. Ignoring.");

			return;
		}
		if (!_rakSessions.TryGetValue(clientEndpoint, out RakSession rakSession)) return;
		if (rakSession.CustomMessageHandler == null) Log.ErrorFormat("Receive online message without message handler for IP={0}. Session removed.", clientEndpoint.Address);
		if (rakSession.Evicted) return;
		rakSession.LastUpdatedTime = DateTime.UtcNow;
		if (header.IsAck)
		{
			if (ConnectionInfo.IsEmulator) return;

			var ack = new Ack();
			ack.Decode(receivedBytes);

			HandleAck(rakSession, ack, ConnectionInfo);
			return;
		}

		if (header.IsNak)
		{
			if (ConnectionInfo.IsEmulator) return;

			var nak = new Nak();
			nak.Decode(receivedBytes);

			HandleNak(rakSession, nak, ConnectionInfo);
			return;
		}

		if (ConnectionInfo.IsEmulator)
		{
			if (_tickerHighPrecisionTimer != null)
			{
				HighPrecisionTimer timer = _tickerHighPrecisionTimer;
				_tickerHighPrecisionTimer = null;
				timer?.Dispose();
			}

			var datagramSequenceNumber = new Int24(receivedBytes.Span.Slice(1, 3));

			Acks acks = Acks.CreateObject();
			acks.acks.Add(datagramSequenceNumber);
			byte[] data = acks.Encode();

			SendData(data, clientEndpoint);

			return;
		}

		Datagram datagram = Datagram.CreateObject();
		try
		{
			datagram.Decode(receivedBytes);
		}
		catch (Exception e)
		{
			rakSession.Disconnect("Bad packet received from client.");
			Log.Warn($"Bad packet {receivedBytes.Span[0]}\n{Packet.HexDump(receivedBytes)}", e);
			_greyListManager.Blacklist(clientEndpoint.Address);

			return;
		}

		EnqueueAck(rakSession, datagram.Header.DatagramSequenceNumber);
		HandleDatagram(rakSession, datagram);
		datagram.PutPool();
	}

	private void HandleDatagram(RakSession session, Datagram datagram)
	{
		foreach (Packet packet in datagram.Messages)
		{
			Packet message = packet;
			if (message is SplitPartPacket splitPartPacket)
			{
				message = HandleSplitMessage(session, splitPartPacket);
				if (message == null) continue;
			}

			message.Timer.Restart();
			session.HandleRakMessage(message);
		}
	}

	private Packet HandleSplitMessage(RakSession session, SplitPartPacket splitPart)
	{
		int spId = splitPart.ReliabilityHeader.PartId;
		int spIdx = splitPart.ReliabilityHeader.PartIndex;
		int spCount = splitPart.ReliabilityHeader.PartCount;

		SplitPartPacket[] splitPartList = session.Splits.GetOrAdd(spId, new SplitPartPacket[spCount]);
		bool haveAllParts = true;

		lock (splitPartList)
		{
			if (splitPartList[spIdx] != null) return null;

			splitPartList[spIdx] = splitPart;

			if (splitPartList.Any(spp => spp == null)) haveAllParts = false;
		}

		if (!haveAllParts) return null;

		if (Log.IsVerboseEnabled()) Log.Verbose($"Got all {spCount} split packets for split ID: {spId}");

		session.Splits.TryRemove(spId, out SplitPartPacket[] _);

		int contiguousLength = splitPartList.Sum(spp => spp.Message.Length);

		var buffer = new Memory<byte>(new byte[contiguousLength]);

		Reliability headerReliability = splitPart.ReliabilityHeader.Reliability;
		Int24 headerReliableMessageNumber = splitPart.ReliabilityHeader.ReliableMessageNumber;
		byte headerOrderingChannel = splitPart.ReliabilityHeader.OrderingChannel;
		Int24 headerOrderingIndex = splitPart.ReliabilityHeader.OrderingIndex;

		int position = 0;
		foreach (SplitPartPacket spp in splitPartList)
		{
			spp.Message.CopyTo(buffer.Slice(position));
			position += spp.Message.Length;
			spp.PutPool();
		}

		try
		{
			Packet fullMessage = PacketFactory.Create(buffer.Span[0], buffer, "raknet") ??
								new UnknownPacket(buffer.Span[0], buffer.ToArray());

			fullMessage.ReliabilityHeader = new ReliabilityHeader()
			{
				Reliability = headerReliability,
				ReliableMessageNumber = headerReliableMessageNumber,
				OrderingChannel = headerOrderingChannel,
				OrderingIndex = headerOrderingIndex,
			};

			if (Log.IsVerboseEnabled()) Log.Verbose($"Assembled split packet {fullMessage.ReliabilityHeader.Reliability} message #{fullMessage.ReliabilityHeader.ReliableMessageNumber}, OrdIdx: #{fullMessage.ReliabilityHeader.OrderingIndex}");

			return fullMessage;
		}
		catch (Exception e)
		{
			Log.Error("Error during split message parsing", e);
			if (Log.IsDebugEnabled) Log.Debug($"0x{buffer.Span[0]:x2}\n{Packet.HexDump(buffer)}");
			session.Disconnect("Bad packet received from client.", false);
		}

		return null;
	}


	private void EnqueueAck(RakSession session, Int24 datagramSequenceNumber)
	{
		session.OutgoingAckQueue.Enqueue(datagramSequenceNumber);
	}

	private void HandleAck(RakSession session, Ack ack, ConnectionInfo connectionInfo)
	{
		ConcurrentDictionary<int, Datagram> queue = session.WaitingForAckQueue;

		foreach ((int start, int end) range in ack.ranges)
		{
			Interlocked.Increment(ref connectionInfo.NumberOfAckReceive);

			for (int i = range.start; i <= range.end; i++)
			{
				if (queue.TryRemove(i, out Datagram datagram))
				{
					CalculateRto(session, datagram);

					datagram.PutPool();
				}
				else
				{
					if (Log.IsDebugEnabled) Log.Warn($"ACK, Failed to remove datagram #{i} for {session.Username}. Queue size={queue.Count}");
				}
			}
		}

		session.ResendCount = 0;
		session.WaitForAck = false;
	}

	private void HandleNak(RakSession session, Nak nak, ConnectionInfo connectionInfo)
	{
		ConcurrentDictionary<int, Datagram> queue = session.WaitingForAckQueue;

		foreach ((int start, int end) in nak.ranges)
		{
			Interlocked.Increment(ref connectionInfo.NumberOfNakReceive);

			for (int i = start; i <= end; i++)
			{
				if (queue.TryGetValue(i, out Datagram datagram))
				{
					CalculateRto(session, datagram);

					datagram.RetransmitImmediate = true;
				}
				else
				{
					if (Log.IsDebugEnabled)
						Log.WarnFormat("NAK, no datagram #{0} for {1}", i, session.Username);
				}
			}
		}
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CalculateRto(RakSession session, Datagram datagram)
	{
		// RTT = RTT * 0.875 + rtt * 0.125
		// RTTVar = RTTVar * 0.875 + abs(RTT - rtt)) * 0.125
		// RTO = RTT + 4 * RTTVar
		long rtt = datagram.Timer.ElapsedMilliseconds;
		long RTT = session.Rtt;
		long RTTVar = session.RttVar;

		session.Rtt = (long) (RTT * 0.875 + rtt * 0.125);
		session.RttVar = (long) (RTTVar * 0.875 + Math.Abs(RTT - rtt) * 0.125);
		session.Rto = session.Rtt + 4 * session.RttVar + 100; // SYNC time in the end
	}

	private async void SendTick(object obj)
	{
		var tasks = _rakSessions.Select(session => session.Value.SendTickAsync(this)).ToList();
		await Task.WhenAll(tasks);

		//long duration = watch.ElapsedMilliseconds;
		//if (duration > 10) Log.Warn($"Ticker thread exceeded max time. Took {watch.ElapsedMilliseconds}ms for {_playerSessions.Count} sessions.");
	}

	internal async Task UpdateAsync(RakSession session)
	{
		if (session.Evicted) return;

		if (MiNetServer.FastThreadPool == null) return;

		try
		{
			long now = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
			long lastUpdate = session.LastUpdatedTime.Ticks / TimeSpan.TicksPerMillisecond;

			if (!session.WaitForAck && (session.ResendCount > session.ResendThreshold || lastUpdate + session.InactivityTimeout < now))
			{
				session.DetectLostConnection();
				session.WaitForAck = true;
			}

			if (session.WaitingForAckQueue.IsEmpty) return;
			if (session.WaitForAck) return;
			if (session.Rto == 0) return;

			long rto = Math.Max(100, session.Rto);
			ConcurrentDictionary<int, Datagram> queue = session.WaitingForAckQueue;

			foreach (KeyValuePair<int, Datagram> datagramPair in queue)
			{
				if (session.Evicted) return;

				Datagram datagram = datagramPair.Value;

				if (!datagram.Timer.IsRunning)
				{
					Log.Error($"Timer not running for #{datagram.Header.DatagramSequenceNumber}");
					datagram.Timer.Restart();
					continue;
				}

				if (session.Rtt == -1) return;

				long elapsedTime = datagram.Timer.ElapsedMilliseconds;
				long datagramTimeout = rto * (datagram.TransmissionCount + session.ResendCount + 1);
				datagramTimeout = Math.Min(datagramTimeout, 3000);
				datagramTimeout = Math.Max(datagramTimeout, 100);

				if (!datagram.RetransmitImmediate && elapsedTime < datagramTimeout / 2) continue;
				if (session.Evicted || !session.WaitingForAckQueue.TryRemove(datagram.Header.DatagramSequenceNumber, out _)) continue;
				session.ErrorCount++;
				session.ResendCount++;

				if (Log.IsDebugEnabled) Log.Warn($"{(datagram.RetransmitImmediate ? "NAK RSND" : "TIMEOUT")}, Resent #{datagram.Header.DatagramSequenceNumber.IntValue()} Type: {datagram.FirstMessageId} (0x{datagram.FirstMessageId:x2}) for {session.Username} ({elapsedTime} > {datagramTimeout}) RTO {session.Rto}");

				Interlocked.Increment(ref ConnectionInfo.NumberOfResends);
				await SendDatagramAsync(session, datagram);
			}
		}
		catch (Exception e)
		{
			Log.Warn(e);
		}
	}

	public async Task SendPacketAsync(RakSession session, Packet message)
	{
		foreach (Datagram datagram in Datagram.CreateDatagrams(message, session.MtuSize, session)) await SendDatagramAsync(session, datagram);
		message.PutPool();
	}

	public async Task SendPacketAsync(RakSession session, List<Packet> messages)
	{
		foreach (Datagram datagram in Datagram.CreateDatagrams(messages, session.MtuSize, session)) await SendDatagramAsync(session, datagram);
		foreach (Packet message in messages) message.PutPool();
	}


	private async Task SendDatagramAsync(RakSession session, Datagram datagram)
	{
		if (datagram.MessageParts.Count == 0)
		{
			Log.Warn($"Failed to send #{datagram.Header.DatagramSequenceNumber.IntValue()}");
			datagram.PutPool();
			return;
		}

		if (datagram.TransmissionCount > 10)
		{
			if (Log.IsDebugEnabled) Log.Warn($"Retransmission count exceeded. No more resend of #{datagram.Header.DatagramSequenceNumber.IntValue()} Type: {datagram.FirstMessageId} (0x{datagram.FirstMessageId:x2}) for {session.Username}");

			datagram.PutPool();

			Interlocked.Increment(ref ConnectionInfo.NumberOfFails);
			return;
		}

		datagram.Header.DatagramSequenceNumber = Interlocked.Increment(ref session.DatagramSequenceNumber);
		datagram.TransmissionCount++;
		datagram.RetransmitImmediate = false;

		byte[] buffer = ArrayPool<byte>.Shared.Rent(1600);
		int length = (int) datagram.GetEncoded(ref buffer);

		datagram.Timer.Restart();

		if (!ConnectionInfo.DisableAck && !ConnectionInfo.IsEmulator && !session.WaitingForAckQueue.TryAdd(datagram.Header.DatagramSequenceNumber.IntValue(), datagram))
		{
			Log.Warn($"Datagram sequence unexpectedly existed in the ACK/NAK queue already {datagram.Header.DatagramSequenceNumber.IntValue()}");
			datagram.PutPool();
		}
		await SendDataAsync(buffer, length, session.EndPoint);
		ArrayPool<byte>.Shared.Return(buffer);
	}


	public void SendData(byte[] data, IPEndPoint targetEndPoint)
	{
		try
		{
			_listener.Send(data, data.Length, targetEndPoint);

			Interlocked.Increment(ref ConnectionInfo.NumberOfPacketsOutPerSecond);
			Interlocked.Add(ref ConnectionInfo.TotalPacketSizeOutPerSecond, data.Length);
		}
		catch (ObjectDisposedException)
		{
		}
		catch (Exception e)
		{
			Log.Warn(e);
		}
	}

	public async Task SendDataAsync(byte[] data, IPEndPoint targetEndPoint)
	{
		await SendDataAsync(data, data.Length, targetEndPoint);
	}

	public async Task SendDataAsync(byte[] data, int length, IPEndPoint targetEndPoint)
	{
		try
		{
			await _listener.SendAsync(data, length, targetEndPoint);
			Interlocked.Increment(ref ConnectionInfo.NumberOfPacketsOutPerSecond);
			Interlocked.Add(ref ConnectionInfo.TotalPacketSizeOutPerSecond, length);
		}
		catch (ObjectDisposedException e)
		{
			Log.Warn(e);
		}
		catch (Exception e)
		{
			Log.Warn(e);
		}
	}
}