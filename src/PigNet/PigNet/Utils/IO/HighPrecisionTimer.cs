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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace PigNet.Utils.IO;

public static class WinApi
{
	/// <summary>TimeBeginPeriod(). See the Windows API documentation for details.</summary>
	[SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
	[SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
	[SuppressUnmanagedCodeSecurity]
	[DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
	public static extern uint TimeBeginPeriod(uint uMilliseconds);

	/// <summary>TimeEndPeriod(). See the Windows API documentation for details.</summary>
	[SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible")]
	[SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage")]
	[SuppressUnmanagedCodeSecurity]
	[DllImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)]
	public static extern uint TimeEndPeriod(uint uMilliseconds);
}

public class HighPrecisionTimer : IDisposable
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(HighPrecisionTimer));
	private Action<object> _action;

	private CancellationTokenSource _cancelSource;

	private bool _disposed;
	private bool _running;
	private Thread _timerThread;

	public AutoResetEvent AutoReset = new(true);
	public long Avarage;
	public long Misses;
	public long Sleeps;


	public long Spins;
	public long Yields;


	public HighPrecisionTimer(int interval, Action<object> action, bool useSignaling = false, bool skipTicks = true)
	{
		// The following is dangerous code. It will increase windows timer frequence to interrupt
		// every 1ms instead of default 15ms. It is the same tech that games use to increase FPS and
		// media decoders to increase precision of movies (chrome does this).
		// We use this here to increase the precision of our thread scheduling, since this will have a big
		// impact on overall performance of the server. DO note that changing this is a global setting in
		// Windows and will affect every running process. It will automatically disable itself and reset
		// to default after closing the process.
		// It will have a major impact bettery and energy consumption in general. So don't go tell GreenPeace about
		// this, thanks.
		//
		// HERE BE DRAGONS!
		if (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX) WinApi.TimeBeginPeriod(1);
		// END IS HERE. SAFE AGAIN ...

		Avarage = interval;
		_action = action;

		if (interval < 1)
			throw new ArgumentOutOfRangeException();

		_cancelSource = new CancellationTokenSource();

		var watch = Stopwatch.StartNew();
		long nextStop = interval;

		var task = new Task(() =>
		{
			_timerThread = Thread.CurrentThread;
			Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

			try
			{
				_running = true;

				while (!_cancelSource.IsCancellationRequested)
				{
					long msLeft = nextStop - watch.ElapsedMilliseconds;
					if (msLeft <= 0)
					{
						if (msLeft < -1) Misses++;
						//if (!skipTicks && msLeft < -4) Log.Warn($"We are {msLeft}ms late for action execution");

						long execTime = watch.ElapsedMilliseconds;
						try
						{
							_action?.Invoke(this);
						}
						catch (Exception)
						{
							if (!ContinueOnError) throw;
						}
						AutoReset.Reset();
						execTime = watch.ElapsedMilliseconds - execTime;
						Avarage = ((Avarage * 9) + execTime) / 10L;

						if (skipTicks)
							// Calculate when the next stop is. If we're too slow on the trigger then we'll skip ticks
							nextStop = (long) (interval * (Math.Floor(watch.ElapsedMilliseconds / (float) interval /*+ 1f*/) + 1));
						else
						{
							long calculatedNextStop = (long) (interval * (Math.Floor(watch.ElapsedMilliseconds / (float) interval /*+ 1f*/) + 1));
							nextStop += interval;

							// Calculate when the next stop is. If we're very behind on ticks then we'll skip ticks
							if (calculatedNextStop - nextStop > 2 * interval)
								//Log.Warn($"Skipping ticks because behind {calculatedNextStop - nextStop}ms. Too much");
								nextStop = calculatedNextStop;
						}

						// If we can't keep up on execution time, we start skipping ticks until we catch up again.
						if (Avarage > interval) nextStop += interval;

						continue;
					}
					if (msLeft < 5)
					{
						Spins++;

						if (useSignaling) AutoReset.WaitOne(50);

						long stop = nextStop;
						if (watch.ElapsedMilliseconds < stop) SpinWait.SpinUntil(() => watch.ElapsedMilliseconds >= stop);

						continue;
					}

					if (msLeft < 16)
					{
						if (Thread.Yield())
						{
							Yields++;
							continue;
						}

						Sleeps++;
						Thread.Sleep(1);
						if (!skipTicks && Log.IsDebugEnabled)
						{
							long t = nextStop - watch.ElapsedMilliseconds;
							if (t < -5) Log.Warn($"We overslept {t}ms in thread yield/sleep");
						}
						continue;
					}

					Sleeps++;
					Thread.Sleep(Math.Max(1, (int) (msLeft - 16)));
					if (!skipTicks && Log.IsDebugEnabled)
					{
						long t = nextStop - watch.ElapsedMilliseconds;
						if (t < -5) Log.Warn($"We overslept {t}ms in thread sleep");
					}
				}
			}
			catch (Exception e)
			{
				Log.Error("Timer crashed out with uncaught exception leaving it in an undisposed state.", e);
				throw;
			}
			finally
			{
				_running = false;

				_cancelSource?.Dispose();
				_cancelSource = null;

				AutoReset?.Dispose();
				AutoReset = null;
			}
		}, _cancelSource.Token, TaskCreationOptions.LongRunning);

		task.Start();
	}

	public bool ContinueOnError { get; set; } = true;

	public void Dispose()
	{
		if (_disposed)
			return;

		_disposed = true;
		//Console.WriteLine("Called Disposed");

		_action = null; // Make sure this will not fire again

		AutoResetEvent autoReset = AutoReset;
		CancellationTokenSource cancelSource = _cancelSource;
		if (cancelSource == null) return;
		if (autoReset == null) return;

		if (_timerThread != Thread.CurrentThread)
		{
			if (_running)
			{
				cancelSource.Cancel();

				if (!autoReset.SafeWaitHandle.IsClosed)
					autoReset.Set();
			}

			while (_running) Thread.Yield();

			//Console.WriteLine("Disposed from other thread");
			return;
		}

		if (_running)
		{
			cancelSource.Cancel();

			if (!autoReset.SafeWaitHandle.IsClosed)
				autoReset.Set();
		}
		//Console.WriteLine("Disposed from same thread");
	}
}