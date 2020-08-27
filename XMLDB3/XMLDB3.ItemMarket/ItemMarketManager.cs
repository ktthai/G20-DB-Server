using System;
using System.Collections;
using System.Threading;

namespace XMLDB3.ItemMarket
{
	public class ItemMarketManager
	{
		private static object syncObj = new object();

		private static string serverIP;

		private static short serverPort;

		private static int connectionPoolSize = 0;

		private static ArrayList connectionPool = null;

		private static int roundRobin = 0;

		private static bool running = false;

		private static bool initialized = false;

        public static bool Init(int _gameNo, int _serverNo, string _serverIP, short _serverPort, int _connectionPoolSize, int _codePage)
		{
			Console.WriteLine("Item Market's Enabled.");
			PacketHelper.Init(_codePage);
			if (_connectionPoolSize == 0)
			{
				_connectionPoolSize = 1;
			}

			serverIP = _serverIP;
			serverPort = _serverPort;
			connectionPool = new ArrayList(_connectionPoolSize);
			try
			{
				for (int i = 0; i < _connectionPoolSize; i++)
				{
					ItemMarketHandler itemMarketHandler = new ItemMarketHandler(i.ToString(), _gameNo, _serverNo);
					itemMarketHandler.OnClosed = OnClientClosed;
					itemMarketHandler.OnInitialized = OnClientConnected;
					itemMarketHandler.OnConnectionFailed = OnConnectionFailed;
					itemMarketHandler.OnInitializeFailed = OnInitialzeFailed;
					Console.WriteLine("Item Market Client [{0}]'s Connecting...", itemMarketHandler.Name);
					if (!itemMarketHandler.ConnectIP(serverIP, serverPort))
					{
						return false;
					}
				}
				while (_connectionPoolSize > connectionPoolSize)
				{
					Thread.Sleep(100);
				}
				if (connectionPoolSize >= connectionPool.Count)
				{
					initialized = true;
				}
				running = true;
				return initialized;
			}
			catch (Exception ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
				return false;
			}
		}

		public static void Stop()
		{
			initialized = false;
			running = false;
			ArrayList arrayList = null;
			lock (syncObj)
			{
				arrayList = (ArrayList)connectionPool.Clone();
			}
			if (arrayList != null)
			{
				foreach (ItemMarketHandler item in arrayList)
				{
					item.Stop();
				}
			}
			connectionPoolSize = int.MaxValue;
		}

		public static void OnClientConnected(object _arg, int _result)
		{
			string message = $"Item Market Client [{((ItemMarketHandler)_arg).Name}]'s Initialized.";
			ExceptionMonitor.ExceptionRaised(new Exception(message));
			lock (syncObj)
			{
				connectionPool.Add(_arg);
			}
			if (!initialized)
			{
				Interlocked.Increment(ref connectionPoolSize);
			}
		}

		private static void ScheduleReconnect(ItemMarketHandler _handler)
		{
			if (running)
			{
				Console.WriteLine("Item Market Client [{0}] schedule reconnecting...", _handler.Name);
				_handler.ScheduleReconnect();
			}
		}

		public static void Connect(ItemMarketHandler _handler)
		{
			if (_handler != null)
			{
				Console.WriteLine("Item Market Client [{0}] is reconnecting...", _handler.Name);
				try
				{
					_handler.ConnectIP(serverIP, serverPort);
				}
				catch (Exception ex)
				{
					ExceptionMonitor.ExceptionRaised(ex);
					ScheduleReconnect(_handler);
				}
			}
			else
			{
				Console.WriteLine("Item Market Client is null...");
			}
		}

		public static void OnConnectionFailed(object _arg, int _result)
		{
			Console.WriteLine("Item Market Client [{0}]'s failed to connect with {1}.", ((ItemMarketHandler)_arg).Name, _result);
			if (!initialized)
			{
				Interlocked.Increment(ref connectionPoolSize);
			}
			ScheduleReconnect((ItemMarketHandler)_arg);
		}

		public static void OnClientClosed(object _arg, int _result)
		{
			string message = $"Item Market Client [{((ItemMarketHandler)_arg).Name}]'s closed with {_result}.";
			ExceptionMonitor.ExceptionRaised(new Exception(message));
			lock (syncObj)
			{
				connectionPool.Remove(_arg);
			}
			if (!initialized)
			{
				Interlocked.Increment(ref connectionPoolSize);
			}
			ScheduleReconnect((ItemMarketHandler)_arg);
		}

		public static void OnInitialzeFailed(object _arg, int _result)
		{
			Console.WriteLine("Item Market Client [{0}]'s failed to initialize with {1}.", ((ItemMarketHandler)_arg).Name, _result);
		}

		public static ItemMarketHandler GetHandler()
		{
			lock (syncObj)
			{
				if (connectionPool.Count > 0)
				{
					return (ItemMarketHandler)connectionPool[roundRobin++ % connectionPool.Count];
				}
				return null;
			}
		}

		public static void CheckHandlers()
		{
			Console.WriteLine("Checking Item Market Handlers...");
			ArrayList arrayList = null;
			lock (syncObj)
			{
				arrayList = (ArrayList)connectionPool.Clone();
			}
			foreach (ItemMarketHandler item in arrayList)
			{
				if (!item.IsWorking)
				{
					Console.WriteLine("Invalid handler [{0}] found. Shutting down...", item.Name);
					OnClientClosed(item, -1);
				}
			}
			Console.WriteLine("Done...");
		}
	}
}
