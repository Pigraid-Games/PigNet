﻿#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/PigNet/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is PigNet.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using log4net;
using PigNet.Utils;

namespace PigNet.Net.RakNet;

public class ConnectionInfo
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(ConnectionInfo));

	private long _avgSizePerPacketIn;
	private long _avgSizePerPacketOut;
	public int ConnectionsInConnectPhase = 0;

	public long NumberOfAckReceive;
	public long NumberOfAckSent;

	public int NumberOfDeniedConnectionRequestsPerSecond;
	public long NumberOfFails;
	public long NumberOfNakReceive;
	public long NumberOfPacketsInPerSecond;
	public long NumberOfPacketsOutPerSecond;
	public long NumberOfResends;
	public long TotalPacketSizeInPerSecond;
	public long TotalPacketSizeOutPerSecond;

	public ConnectionInfo(ConcurrentDictionary<IPEndPoint, RakSession> rakSessions)
	{
		RakSessions = rakSessions;

		if (!Log.IsInfoEnabled) return;

		//CreateCounters();

		//PerformanceCounter ctrNumberOfPacketsOutPerSecond = new PerformanceCounter("PigNet", nameof(NumberOfPacketsOutPerSecond), "PigNet", false);
		//PerformanceCounter ctrNumberOfPacketsInPerSecond = new PerformanceCounter("PigNet", nameof(NumberOfPacketsInPerSecond), "PigNet", false);
		//PerformanceCounter ctrNumberOfAckSent = new PerformanceCounter("PigNet", nameof(NumberOfAckSent), "PigNet", false);
		//PerformanceCounter ctrNumberOfAckReceive = new PerformanceCounter("PigNet", nameof(NumberOfAckReceive), "PigNet", false);
		//PerformanceCounter ctrNumberOfNakReceive = new PerformanceCounter("PigNet", nameof(NumberOfNakReceive), "PigNet", false);
		//PerformanceCounter ctrNumberOfResends = new PerformanceCounter("PigNet", nameof(NumberOfResends), "PigNet", false);
		//PerformanceCounter ctrNumberOfFails = new PerformanceCounter("PigNet", nameof(NumberOfFails), "PigNet", false);

		{
			ThroughPut = new Timer(state =>
			{
				NumberOfPlayers = RakSessions.Count;

				//ctrNumberOfPacketsOutPerSecond.IncrementBy(NumberOfPacketsOutPerSecond);
				//ctrNumberOfPacketsInPerSecond.IncrementBy(NumberOfPacketsInPerSecond);
				//ctrNumberOfAckReceive.IncrementBy(NumberOfAckReceive);
				//ctrNumberOfAckSent.IncrementBy(NumberOfAckSent);
				//ctrNumberOfNakReceive.IncrementBy(NumberOfNakReceive);
				//ctrNumberOfResends.IncrementBy(NumberOfResends);
				//ctrNumberOfFails.IncrementBy(NumberOfFails);


				Interlocked.Exchange(ref NumberOfDeniedConnectionRequestsPerSecond, 0);

				long packetSizeOut = Interlocked.Exchange(ref TotalPacketSizeOutPerSecond, 0);
				long packetSizeIn = Interlocked.Exchange(ref TotalPacketSizeInPerSecond, 0);

				double mbpsPerSecondOut = packetSizeOut * 8 / 1_000_000D;
				double mbpsPerSecondIn = packetSizeIn * 8 / 1_000_000D;

				long numberOfPacketsOutPerSecond = Interlocked.Exchange(ref NumberOfPacketsOutPerSecond, 0);
				long numberOfPacketsInPerSecond = Interlocked.Exchange(ref NumberOfPacketsInPerSecond, 0);


				_avgSizePerPacketIn = _avgSizePerPacketIn <= 0 ? packetSizeIn * 10 : (long) ((_avgSizePerPacketIn * 9) + (numberOfPacketsInPerSecond == 0 ? 0 : packetSizeIn / (double) numberOfPacketsInPerSecond));
				_avgSizePerPacketOut = _avgSizePerPacketOut <= 0 ? packetSizeOut * 10 : (long) ((_avgSizePerPacketOut * 9) + (numberOfPacketsOutPerSecond == 0 ? 0 : packetSizeOut / (double) numberOfPacketsOutPerSecond));
				_avgSizePerPacketIn /= 10; // running avg of 100 prev values
				_avgSizePerPacketOut /= 10; // running avg of 100 prev values

				long numberOfAckIn = Interlocked.Exchange(ref NumberOfAckReceive, 0);
				long numberOfAckOut = Interlocked.Exchange(ref NumberOfAckSent, 0);
				long numberOfNakIn = Interlocked.Exchange(ref NumberOfNakReceive, 0);
				long numberOfResend = Interlocked.Exchange(ref NumberOfResends, 0);
				long numberOfFailed = Interlocked.Exchange(ref NumberOfFails, 0);

				string message =
					$"Players {NumberOfPlayers}, " +
					$"Pkt in/out(#/s) {numberOfPacketsInPerSecond}/{numberOfPacketsOutPerSecond}, " +
					$"ACK(in-out)/NAK/RSND/FTO(#/s) ({numberOfAckIn}-{numberOfAckOut})/{numberOfNakIn}/{numberOfResend}/{numberOfFailed}, " +
					$"THR in/out(Mbps) {mbpsPerSecondIn:F}/{mbpsPerSecondOut:F}, " +
					$"PktSz Total in/out(B/s){packetSizeIn}/{packetSizeOut}, " +
					$"PktSz Avg(100s) in/out(B){_avgSizePerPacketIn}/{_avgSizePerPacketOut}";

				if (Config.GetProperty("ServerInfoInTitle", false))
					Console.Title = message;
				else
					Log.Info(message);
			}, null, 1000, 1000);
		}
	}

	public ConcurrentDictionary<IPEndPoint, RakSession> RakSessions { get; set; }

	// Special property for use with ServiceKiller.
	// Will disable reliability handling after login.
	public bool IsEmulator { get; set; }
	public bool DisableAck { get; set; }

	public int NumberOfPlayers { get; set; }

	public Timer ThroughPut { get; set; }
	public long Latency { get; set; }

	public int MaxNumberOfPlayers { get; set; }
	public int MaxNumberOfConcurrentConnects { get; set; }

	internal void Stop()
	{
		ThroughPut?.Change(Timeout.Infinite, Timeout.Infinite);
		ThroughPut?.Dispose();
		ThroughPut = null;
	}

	protected virtual void CreateCounters()
	{
		//if (PerformanceCounterCategory.Exists("PigNet"))
		//{
		//	PerformanceCounterCategory.Delete("PigNet");
		//}

		//if (!PerformanceCounterCategory.Exists("PigNet"))
		//{
		//	CounterCreationDataCollection ccds = new CounterCreationDataCollection
		//	{
		//		new CounterCreationData(nameof(NumberOfPacketsOutPerSecond), "", PerformanceCounterType.RateOfCountsPerSecond32),
		//		new CounterCreationData(nameof(NumberOfPacketsInPerSecond), "", PerformanceCounterType.RateOfCountsPerSecond32),
		//		new CounterCreationData(nameof(NumberOfAckReceive), "", PerformanceCounterType.RateOfCountsPerSecond32),
		//		new CounterCreationData(nameof(NumberOfAckSent), "", PerformanceCounterType.RateOfCountsPerSecond32),
		//		new CounterCreationData(nameof(NumberOfNakReceive), "", PerformanceCounterType.RateOfCountsPerSecond32),
		//		new CounterCreationData(nameof(NumberOfResends), "", PerformanceCounterType.RateOfCountsPerSecond32),
		//		new CounterCreationData(nameof(NumberOfFails), "", PerformanceCounterType.RateOfCountsPerSecond32),
		//	};

		//	PerformanceCounterCategory.Create("PigNet", "PigNet Performance Counters", PerformanceCounterCategoryType.MultiInstance, ccds);
		//}
	}
}