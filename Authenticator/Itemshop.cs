

public class Itemshop
{
	public bool UsingItemShop { get; set; }
	public Domain[] domains { get; set; }

	public Connection sql { get; set; }

	public int gameNumber { get; set; }

	public Itemshop()
    {
		domains = new Domain[0];
		sql = new Connection();
    }
}
