using Mabinogi;

namespace XMLDB3
{
	public class ShopAdvertiseItemSerializer
	{
		public static ShopAdvertiseItem Serialize(Message _message)
		{
			ShopAdvertiseItem shopAdvertiseItem = new ShopAdvertiseItem();
			shopAdvertiseItem.id = _message.ReadS64();
			shopAdvertiseItem.storedtype = _message.ReadU8();
			shopAdvertiseItem.itemName = _message.ReadString();
			shopAdvertiseItem.price = _message.ReadS32();
			shopAdvertiseItem.@class = _message.ReadS32();
			shopAdvertiseItem.color_01 = _message.ReadS32();
			shopAdvertiseItem.color_02 = _message.ReadS32();
			shopAdvertiseItem.color_03 = _message.ReadS32();
			return shopAdvertiseItem;
		}
	}
}
