using Mabinogi;

namespace XMLDB3
{
	public class ItemListSerializer
	{
		public static ItemList[] Serialize(Message _message)
		{
			int num = _message.ReadS32();
			if (num > 0)
			{
				ItemList[] array = new ItemList[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = new ItemList();
					array[i].itemID = _message.ReadS64();
					array[i].storedtype = _message.ReadU8();
				}
				return array;
			}
			return null;
		}
	}
}
