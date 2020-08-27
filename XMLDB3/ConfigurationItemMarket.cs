
public class ConfigurationItemMarket
{
	public bool IsEnabled { get; set; }
	public int ServerNo { get; set; }
	public int GameNo { get; set; }
	public int ConnectionPool { get; set; }
	public string Ip { get; set; }
	public short Port { get; set; }
	public int CodePage { get; set; }

	public ConfigurationItemMarket()
    {
		Ip = string.Empty;
    }
}
