using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using log4net;
using log4net.Config;
using log4net.Repository;
using PigNet;
using PigNet.Console;
using PigNet.Net;
using PigNet.Utils;

class Program
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
	
    static void Main(string[] args)
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (args.Length > 0 && args[0] == "listener")
                {
                    try
                    {
                        using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
						socket.ExclusiveAddressUse = true;
						socket.Bind(new IPEndPoint(IPAddress.Any, 19132));

                        Console.WriteLine("LISTENING!");
                        new ManualResetEvent(false).WaitOne();
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine($"Socket error: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Unexpected error in listener mode: {ex.Message}");
                    }
                    finally
                    {
                        Console.WriteLine("EXIT!");
                    }
                    return;
                }

                try
                {
                    ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Assembly not found."));
                    string logConfigPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException("Failed to get executable directory."), "log4net.xml");

                    if (!File.Exists(logConfigPath))
						Console.WriteLine("Warning: log4net.xml not found. Logging may not work correctly.");
					else
						XmlConfigurator.Configure(logRepository, new FileInfo(logConfigPath));
					if (Log.IsInfoEnabled) Log.Info(MiNetServer.MiNET);
                    else Console.WriteLine(MiNetServer.MiNET);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Logging initialization error: {ex.Message}");
                }

                try
                {
                    var service = new MiNetServer();
                    Log.Info($"Starting PigNet for Minecraft Bedrock Edition {McpeProtocolInfo.GameVersion}...");

                    if (Config.GetProperty("UserBedrockGenerator", false))
                    {
                        service.LevelManager = new LevelManager
                        {
                            Generator = new BedrockGenerator()
                        };
                    }

                    service.StartServer();
                    Console.WriteLine("PigNet running. Press <enter> to stop service.");

                    Console.ReadLine();
                    service.StopServer();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Server encountered an error: {ex.Message}");
                    Log.Error("Unhandled server error", ex);
                }
            }
            else
				Console.WriteLine("Unsupported platform.");
		}
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
