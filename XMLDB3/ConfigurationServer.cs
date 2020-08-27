public class ConfigurationServer
{
	public ConfigurationServerMain Main { get; set; }

	public ConfigurationServerBridge Bridge { get; set; }

	public ConfigurationServerProfiler Profiler { get; set; }

	public ConfigurationServerMonitor Monitor { get; set; }

	public ConfigurationServer()
    {
		Main = new ConfigurationServerMain();
		Profiler = new ConfigurationServerProfiler();
		Monitor = new ConfigurationServerMonitor();
		Bridge = new ConfigurationServerBridge();

	}
}
