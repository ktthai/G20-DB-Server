public class Sql
{
	public SqlConnections connections { get; set; }
    public string LocalDbLocation { get; set; }

    public Sql()
    {
        connections = new SqlConnections();
        LocalDbLocation = string.Empty;
    }
}
