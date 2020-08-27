using Mabinogi;

namespace XMLDB3
{
	public class ShopAdvertisebaseSerializer
	{
		public static ShopAdvertisebase Serialize(Message _message)
		{
			ShopAdvertisebase shopAdvertisebase = new ShopAdvertisebase();
			shopAdvertisebase.account = _message.ReadString();
			shopAdvertisebase.server = _message.ReadString();
			shopAdvertisebase.shopName = _message.ReadString();
			shopAdvertisebase.area = _message.ReadString();
			shopAdvertisebase.characterName = _message.ReadString();
			shopAdvertisebase.comment = _message.ReadString();
			shopAdvertisebase.startTime = _message.ReadS64();
			shopAdvertisebase.region = _message.ReadS32();
			shopAdvertisebase.x = _message.ReadS32();
			shopAdvertisebase.y = _message.ReadS32();
			shopAdvertisebase.leafletCount = _message.ReadS32();
			return shopAdvertisebase;
		}

		public static Message Deserialize(ShopAdvertisebase _advertise, Message _message)
		{
			if (_advertise == null)
			{
				_advertise = new ShopAdvertisebase();
				_advertise.account = string.Empty;
				_advertise.server = string.Empty;
				_advertise.shopName = string.Empty;
				_advertise.area = string.Empty;
				_advertise.characterName = string.Empty;
				_advertise.comment = string.Empty;
			}
			_message.WriteString(_advertise.account);
			_message.WriteString(_advertise.server);
			_message.WriteString(_advertise.shopName);
			_message.WriteString(_advertise.area);
			_message.WriteString(_advertise.characterName);
			_message.WriteString(_advertise.comment);
			_message.WriteS64(_advertise.startTime);
			_message.WriteS32(_advertise.region);
			_message.WriteS32(_advertise.x);
			_message.WriteS32(_advertise.y);
			_message.WriteS32(_advertise.leafletCount);
			return _message;
		}
	}
}
