#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
//
// The Original Code is MiNET.
// 
// The Original Developer is Niclas Olofsson.
// Copyright (c) 2014-2020 Niclas Olofsson. All Rights Reserved.

#endregion

using System;
using System.Diagnostics;
using System.Net;
using System.Numerics;
using log4net;
using Microsoft.IO;
using MiNET.Net;
using MiNET.Net.RakNet;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Utils.IO;

namespace MiNET;

public class MiNetServer
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));

	public const string MiNET = "\r\n __   __  ___   __    _  _______  _______ \r\n|  |_|  ||   | |  |  | ||       ||       |\r\n|       ||   | |   |_| ||    ___||_     _|\r\n|       ||   | |       ||   |___   |   |  \r\n|       ||   | |  _    ||    ___|  |   |  \r\n| ||_|| ||   | | | |   ||   |___   |   |  \r\n|_|   |_||___| |_|  |__||_______|  |___|  \r\n";

	private const int DefaultPort = 19132;

	public IPEndPoint Endpoint { get; private set; }
	private RakConnection _listener;

	public MotdProvider MotdProvider { get; set; }

	public static RecyclableMemoryStreamManager MemoryStreamManager { get; set; } = new RecyclableMemoryStreamManager();

	public IServerManager ServerManager { get; set; }
	public LevelManager LevelManager { get; set; }
	public PlayerFactory PlayerFactory { get; set; }
	public GreyListManager GreyListManager { get; set; }

	public bool IsEdu { get; set; } = Config.GetProperty("EnableEdu", false);
	public EduTokenManager EduTokenManager { get; set; }

	public PluginManager PluginManager { get; set; }
	public SessionManager SessionManager { get; set; }

	public ConnectionInfo ConnectionInfo { get; set; }

	public ServerRole ServerRole { get; set; }

	internal static DedicatedThreadPool FastThreadPool { get; set; }

	static MiNetServer()
	{
		Log.Info("Static MiNetServer constructor initialized.");
	}

	public MiNetServer()
	{
		ServerRole = Config.GetProperty("ServerRole", ServerRole.Full);
		InitializeFastThreadPool();
	}

	public MiNetServer(IPEndPoint endpoint) : this()
	{
		Endpoint = endpoint;
	}

	// Dynamically scale the thread pool based on system resources
	private void InitializeFastThreadPool()
	{
		FastThreadPool?.Dispose();
		int threadCount = Environment.ProcessorCount * 10;
		Log.Info($"Initializing thread pool with {threadCount} threads.");
		FastThreadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(threadCount, "Fast_Thread"));
	}

	public static void DisplayTimerProperties()
	{
		Console.WriteLine($"HW accelerated vectors: {(Vector.IsHardwareAccelerated ? "Yes" : "No")}");
		Console.WriteLine(Stopwatch.IsHighResolution
			? "High-resolution performance counter available."
			: "Using DateTime for timing.");

		long frequency = Stopwatch.Frequency;
		long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
		Console.WriteLine($"Timer frequency: {frequency} ticks/sec");
		Console.WriteLine($"Timer accuracy: {nanosecPerTick} ns");
	}

	public bool StartServer()
	{
		if (_listener != null) return false;

		try
		{
			Log.Info("Starting server...");

			InitializeServerRole();
			InitializeManagers();
			LoadPlugins();
			StartRakNetListener();

			Log.Info($"Server running on port {Endpoint?.Port}.");
			return true;
		}
		catch (Exception e)
		{
			Log.Error("Error during startup!", e);
			_listener?.Stop();
			return false;
		}
	}

	private void InitializeServerRole()
	{
		if (ServerRole != ServerRole.Full && ServerRole != ServerRole.Proxy) return;
		if (IsEdu)
		{
			Log.Info("Education Edition features enabled.");
			EduTokenManager = new EduTokenManager();
		}
		ConfigureEndpoint();
	}

	private void ConfigureEndpoint()
	{
		if (Endpoint != null) return;
		var ip = IPAddress.Parse(Config.GetProperty("ip", "0.0.0.0"));
		int port = Config.GetProperty("port", DefaultPort);
		Endpoint = new IPEndPoint(ip, port);
	}

	private void InitializeManagers()
	{
		ServerManager ??= new DefaultServerManager(this);
		SessionManager ??= new SessionManager();
		LevelManager ??= new LevelManager();
		PlayerFactory ??= new PlayerFactory();
		GreyListManager ??= new GreyListManager();
		MotdProvider ??= new MotdProvider();
	}

	private void LoadPlugins()
	{
		Log.Info("Loading plugins...");
		PluginManager = new PluginManager();
		PluginManager.LoadPlugins();
		PluginManager.ExecuteStartup(this);
		PluginManager.EnablePlugins(this, LevelManager);
	}

	private void StartRakNetListener()
	{
		_listener = new RakConnection(Endpoint, GreyListManager, MotdProvider)
		{
			CustomMessageHandlerFactory = session => new BedrockMessageHandler(session, ServerManager, PluginManager)
		};

		ConnectionInfo = _listener.ConnectionInfo;
		ConnectionInfo.MaxNumberOfPlayers = Config.GetProperty("MaxNumberOfPlayers", 1000); // Higher capacity
		ConnectionInfo.MaxNumberOfConcurrentConnects = Config.GetProperty("MaxNumberOfConcurrentConnects", ConnectionInfo.MaxNumberOfPlayers);

		_listener.Start();
	}

	public void StopServer()
	{
		Log.Info("Disconnecting all players...");
		foreach(var level in LevelManager.Levels)
		{
			foreach (var player in level.Players)
			{
				player.Value.Disconnect("The server has been closed.");
			}
		}

		Log.Info("Stopping server...");
		LevelManager.Close();

		Log.Info("Disabling plugins...");
		PluginManager?.DisablePlugins();

		_listener?.Stop();
		ConnectionInfo?.Stop();

		FastThreadPool?.Dispose();
		Log.Info("Waiting for threads to exit...");
		FastThreadPool?.WaitForThreadsExit();
	}
}

public enum ServerRole
{
	Node,
	Proxy,
	Full
}