using System.Collections;
using System.Collections.Generic;

public class CommerceSystemProduct
{
	public int id { get; set; }

	public int price { get; set; }

	public int count { get; set; }

	public Dictionary<int, COStockInfo> stockTable { get; set; }
}
