using Mabinogi;
using System;

namespace XMLDB3
{
	public class HouseAppearSerializer
	{
		public static void Deserialize(House _house, Message _message)
		{
			_message.WriteS64(_house.houseID);
			_message.WriteU8(_house.constructed);
			_message.WriteS64(_house.updateTime.Ticks);
			_message.WriteString(_house.account);
			_message.WriteString(_house.charName);
			_message.WriteString(_house.houseName);
			_message.WriteS32(_house.houseClass);
			_message.WriteU8(_house.roofSkin);
			_message.WriteU8(_house.roofColor1);
			_message.WriteU8(_house.roofColor2);
			_message.WriteU8(_house.roofColor3);
			_message.WriteU8(_house.wallSkin);
			_message.WriteU8(_house.wallColor1);
			_message.WriteU8(_house.wallColor2);
			_message.WriteU8(_house.wallColor3);
			_message.WriteU8(_house.innerSkin);
			_message.WriteU8(_house.innerColor1);
			_message.WriteU8(_house.innerColor2);
			_message.WriteU8(_house.innerColor3);
			_message.WriteS32(_house.width);
			_message.WriteS32(_house.height);
			_message.WriteS64(_house.bidSuccessDate.Ticks);
			_message.WriteS64(_house.taxPrevDate.Ticks);
			_message.WriteS64(_house.taxNextDate.Ticks);
			_message.WriteS32(_house.taxPrice);
			_message.WriteU8(_house.taxAutopay);
			_message.WriteS32(_house.houseMoney);
			_message.WriteS32(_house.deposit);
			_message.WriteS64(_house.flag);
		}

		public static House Serialize(Message _message)
		{
			House house = new House();
			house.houseID = _message.ReadS64();
			house.constructed = _message.ReadU8();
			house.updateTime = new DateTime(_message.ReadS64());
			house.account = _message.ReadString();
			house.charName = _message.ReadString();
			house.houseName = _message.ReadString();
			house.houseClass = _message.ReadS32();
			house.roofSkin = _message.ReadU8();
			house.roofColor1 = _message.ReadU8();
			house.roofColor2 = _message.ReadU8();
			house.roofColor3 = _message.ReadU8();
			house.wallSkin = _message.ReadU8();
			house.wallColor1 = _message.ReadU8();
			house.wallColor2 = _message.ReadU8();
			house.wallColor3 = _message.ReadU8();
			house.innerSkin = _message.ReadU8();
			house.innerColor1 = _message.ReadU8();
			house.innerColor2 = _message.ReadU8();
			house.innerColor3 = _message.ReadU8();
			house.width = _message.ReadS32();
			house.height = _message.ReadS32();
			house.bidSuccessDate = new DateTime(_message.ReadS64());
			house.taxPrevDate = new DateTime(_message.ReadS64());
			house.taxNextDate = new DateTime(_message.ReadS64());
			house.taxPrice = _message.ReadS32();
			house.taxAutopay = _message.ReadU8();
			house.houseMoney = _message.ReadS32();
			house.deposit = _message.ReadS32();
			house.flag = _message.ReadS64();
			return house;
		}
	}
}
