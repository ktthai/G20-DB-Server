public class ConfigurationSql
{
    public bool LocalSQL { get; set; }
    public bool FirstRun { get; set; }
    public string LocalDbLocation { get; set; }
    public ConfigurationSqlConnections Connections { get; set; }

    public ConfigurationSql()
    {
        Connections = new ConfigurationSqlConnections();
        LocalDbLocation = string.Empty;
        FirstRun = true;
    }

}
