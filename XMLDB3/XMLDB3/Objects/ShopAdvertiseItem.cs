
public class ShopAdvertiseItem : ShopAdvertiseItembase
{
	public Item.StoredType Type
	{
		get
		{
			return (Item.StoredType)storedtype;
		}
		set
		{
			storedtype = (byte)value;
		}
	}
}
