
public class ConfigurationStatistics
{
	public Connection Database { get; set; }
	public int Period { get; set; }

	public ConfigurationStatistics()
	{
		Database = new Connection();
	}
}
