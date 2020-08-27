using Mabinogi;

namespace XMLDB3
{
	public class ShopAdvertiseListSerializer
	{
		public static Message Deserialize(ShopAdvertiseList _list, Message _message)
		{
			if (_list != null && _list.advertises != null && _list.advertises.Length > 0)
			{
				_message.WriteS32(_list.advertises.Length);
				ShopAdvertiseDetail[] advertises = _list.advertises;
				foreach (ShopAdvertiseDetail advertise in advertises)
				{
					WriteShopAdveritise(advertise, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
			return _message;
		}

		private static Message WriteShopAdveritise(ShopAdvertiseDetail _advertise, Message _message)
		{
			if (_advertise == null)
			{
				_advertise = new ShopAdvertiseDetail();
			}
			ShopAdvertisebaseSerializer.Deserialize(_advertise.shopInfo, _message);
			if (_advertise.items != null)
			{
				_message.WriteS32(_advertise.items.Count);

				foreach (ShopAdvertiseItemDetail item in _advertise.items)
				{
					WriteItem(item, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
			return _message;
		}

		private static Message WriteItem(ShopAdvertiseItemDetail _item, Message _message)
		{
			if (_item == null)
			{
				_item = new ShopAdvertiseItemDetail();
			}
			ItemSerializer.Deserialize(_item.item, _message);
			_message.WriteS32(_item.shopPrice);
			return _message;
		}
	}
}
