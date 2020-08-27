
public class Item : Itembase
{
	public enum StoredType : byte
	{
		IstMin,
		IstLarge,
		IstSmall,
		IstHuge,
		IstQuest,
		IstEgo,
		IstMax
	}

	public StoredType Type
	{
		get
		{
			return (StoredType)storedtype;
		}
		set
		{
			storedtype = (byte)value;
		}
	}
}
