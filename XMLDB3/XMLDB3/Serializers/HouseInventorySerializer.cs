using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class HouseInventorySerializer
	{
		public static void Deserialize(HouseInventory _inventory, Message _message)
		{
			if (_inventory.Items != null)
			{
				_message.WriteS32(_inventory.Items.Count);

				foreach (HouseItem item in _inventory.Items)
				{
					HouseItemSerializer.Deserialize(item, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}

		public static HouseInventory Serialize(Message _message)
		{
			int num = _message.ReadS32();
			HouseInventory houseInventory = new HouseInventory();
			if (num > 0)
			{
				houseInventory.Items = new List<HouseItem>(num);
				for (int i = 0; i < num; i++)
				{
					houseInventory.Items[i] = HouseItemSerializer.Serialize(_message);
				}
			}
			return houseInventory;
		}
	}
}
