using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class InventorySerializer
	{
		public static Dictionary<long, Item> Serialize(Message _message)
		{
			int num = _message.ReadS32();
			if (num > 0)
			{
				Dictionary<long, Item> hashtable = new Dictionary<long, Item>(num);
				for (int i = 0; i < num; i++)
				{
					Item item = ItemSerializer.Serialize(_message);
					hashtable.Add(item.id, item);
				}
				return hashtable;
			}
			return null;
		}

		public static void Deserialize(Dictionary<long, Item> _inventory, Message _message)
		{
			if (_inventory != null && _inventory.Count > 0)
			{
				_message.WriteS32(_inventory.Count);
				foreach (Item value in _inventory.Values)
				{
					ItemSerializer.Deserialize(value, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}
	}
}
