
public class ConfigHeader
{
	public Itemshop itemshop { get; set; }
	public Sql sql { get; set; }
	public Test test { get; set; }
	public Event Event { get; set; }
	public int port { get; set; }

	public ConfigHeader()
    {
		itemshop = new Itemshop();
		sql = new Sql();
		test = new Test();
		Event = new Event();
    }
}
