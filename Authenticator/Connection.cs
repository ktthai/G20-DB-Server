

public class Connection
{
    public string server { get; set; }
    public string port { get; set; }
    public string database { get; set; }
    public string user { get; set; }
    public string password { get; set; }

    public Connection()
    {
        database = user = password = server = port = string.Empty;
    }
}
