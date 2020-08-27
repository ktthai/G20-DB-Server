public class ItemList
{
	public long itemID;

	public byte storedtype;

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
