using Mabinogi;

namespace XMLDB3
{
	public class HouseItemSerializer
	{
		public static void Deserialize(HouseItem _item, Message _message)
		{
			_message.WriteU8(_item.posX);
			_message.WriteU8(_item.posY);
			_message.WriteU8(_item.direction);
			_message.WriteS32(_item.userprice);
			_message.WriteU8(_item.pocket);
			ItemSerializer.Deserialize(_item.item, _message);
		}

		public static HouseItem Serialize(Message _message)
		{
			HouseItem houseItem = new HouseItem();
			houseItem.posX = _message.ReadU8();
			houseItem.posY = _message.ReadU8();
			houseItem.direction = _message.ReadU8();
			houseItem.userprice = _message.ReadS32();
			houseItem.pocket = _message.ReadU8();
			houseItem.item = ItemSerializer.Serialize(_message);
			return houseItem;
		}
	}
}
