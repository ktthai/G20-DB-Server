using System;


public class TestAuthorized
{
	public string id { get; set; }

	public DateTime naosupportexpiration{ get; set; }

	public bool naosupportexpirationSpecified{ get; set; }

	public DateTime storageexpiration{ get; set; }

	public bool storageexpirationSpecified{ get; set; }

	public DateTime advancedplayexpiration{ get; set; }

	public bool advancedplayexpirationSpecified{ get; set; }

	public DateTime inventoryplus{ get; set; }

	public bool inventoryplusSpecified{ get; set; }

	public DateTime premiumpack{ get; set; }

	public bool premiumpackSpecified{ get; set; }

	public DateTime vip{ get; set; }

	public bool vipSpecified{ get; set; }

	public DateTime premiumvip{ get; set; }

	public bool premiumvipSpecified{ get; set; }

	public DateTime guildpack{ get; set; }

	public bool guildpackSpecified{ get; set; }

	public bool justupdated{ get; set; }

	public TestAuthorized()
    {
		id = string.Empty;
	}
}
