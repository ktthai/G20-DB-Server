using XMLDB3.ItemMarket;

namespace XMLDB3
{
	public class XMLDB3Main
	{
		public static void Start()
		{
			ConfigManager.Load();

			BasicCommand.Initialize();
			ObjectCache.Init();
			QueryManager.Initialize();
			CommandSerializer.Initialize();
			ProcessManager.Start();
			if (ConfigManager.IsRedirectionEnabled)
			{
				CommandRedirection.Init(ConfigManager.RedirectionServer, ConfigManager.RedirectionPort);
			}
			if (ConfigManager.ItemMarketEnabled)
			{
				ItemMarketManager.Init(ConfigManager.ItemMarketGameNo, ConfigManager.ItemMarketServerNo, ConfigManager.ItemMarketIP, ConfigManager.ItemMarketPort, ConfigManager.ItemMarketConnectionPoolNo, ConfigManager.ItemMarketCodePage);
			}
			MonitorProcedure.ServerStart(ConfigManager.MonitorPort);
			Profiler.ServerStart(ConfigManager.ProfilerPort);
			if (ConfigManager.ExternalServerEnabled)
				DataBridge.ServerStart();
			MainProcedure.ServerStart(ConfigManager.MainPort);
		}

		public static void Shutdown()
		{
			MainProcedure.ServerStop();
			Profiler.ServerStop();
			MonitorProcedure.ServerStop();
			ProcessManager.Shutdown();
			CommandSerializer.Shutdown();
			if (ConfigManager.ItemMarketEnabled)
			{
				ItemMarketManager.Stop();
			}
		}

		public static void ClearException()
		{
			ExceptionMonitor.Clear();
		}
	}
}
