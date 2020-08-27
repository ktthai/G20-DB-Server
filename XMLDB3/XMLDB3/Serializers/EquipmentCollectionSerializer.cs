using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class EquipmentCollectionSerializer
	{
		public static EquipmentCollection Serialize(Message _message)
		{
			EquipmentCollection equipmentCollection = new EquipmentCollection();
			equipmentCollection.Account = _message.ReadString();
			equipmentCollection.item = ReadItemFromMsg(_message);
			return equipmentCollection;
		}

		private static List<CollectionItem> ReadItemFromMsg(Message _message)
		{
			List<CollectionItem> list = null;
			int num = _message.ReadS32();
			if (num > 0)
			{
				list = new List<CollectionItem>(num);
				for (int i = 0; i < num; i++)
				{
					list[i] = new CollectionItem();
					list[i].lockTime = _message.ReadS64();
					list[i].item = ItemSerializer.Serialize(_message);
				}
			}
			return list;
		}

		public static void Deserialize(EquipmentCollection _equipmentCollection, Message _message)
		{
			if (_equipmentCollection == null)
			{
				return;
			}
			if (_equipmentCollection.Account != null && string.Empty != _equipmentCollection.Account)
			{
				_message.WriteString(_equipmentCollection.Account);
			}
			if (_equipmentCollection.item != null)
			{
				_message.WriteS32(_equipmentCollection.item.Count);

				foreach (CollectionItem item in _equipmentCollection.item)
				{
					WriteItemToMsg(item, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}

		private static void WriteItemToMsg(CollectionItem _item, Message _message)
		{
			if (_item == null)
			{
				_item = new CollectionItem();
			}
			_message.WriteS64(_item.lockTime);
			ItemSerializer.Deserialize(_item.item, _message);
		}
	}
}
