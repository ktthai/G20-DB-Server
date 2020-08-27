public class ConfigurationRedirection
{
	public string Server { get; set; }
	public int Port { get; set; }
	public bool Enable { get; set; }

	public ConfigurationRedirection()
    {
		Server = string.Empty;
    }
}
