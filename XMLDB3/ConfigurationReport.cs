
public class ConfigurationReport
{
	public string Server { get; set; }
	public string Sender { get; set; }
	public string Receiver { get; set; }

	public ConfigurationReport()
    {
		Receiver = Sender = Server = string.Empty;
    }
}
