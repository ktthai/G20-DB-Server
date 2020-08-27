public class Configuration
{
	public ConfigurationSql Sql { get; set; }
	public ConfigurationServer Server { get; set; }

	public ConfigurationReport Report { get; set; }

	public ConfigurationEventLog Eventlog { get; set; }

	public ConfigurationStatistics Statistics { get; set; }

	public ConfigurationChronicleRank ChronicleRank { get; set; }

	public ConfigurationCache Cache { get; set; }

	public ConfigurationFeature Feature { get; set; }

	public ConfigurationRedirection Redirection { get; set; }

	public ConfigurationItemMarket ItemMarket { get; set; }

    public Configuration()
    {
        Sql = new ConfigurationSql();
        Server = new ConfigurationServer();
        Report = new ConfigurationReport();
        Statistics = new ConfigurationStatistics();
        ChronicleRank = new ConfigurationChronicleRank();
        Cache = new ConfigurationCache();
        Feature = new ConfigurationFeature();
        Redirection = new ConfigurationRedirection();
        ItemMarket = new ConfigurationItemMarket();
    }
}
