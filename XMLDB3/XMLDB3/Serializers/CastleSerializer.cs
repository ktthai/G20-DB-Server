using Mabinogi;
using System;

namespace XMLDB3
{
	public class CastleSerializer
	{
		public static Castle Serialize(Message _message)
		{
			Castle castle = new Castle();
			castle.castleID = _message.ReadS64();
			castle.guildID = _message.ReadS64();
			castle.constructed = _message.ReadU8();
			castle.castleMoney = _message.ReadS32();
			castle.weeklyIncome = _message.ReadS32();
			castle.taxrate = _message.ReadU8();
			castle.updateTime = new DateTime(_message.ReadS64());
			castle.sellDungeonPass = _message.ReadU8();
			castle.dungeonPassPrice = _message.ReadS32();
			castle.flag = _message.ReadS64();
			return castle;
		}

		public static void Deserialize(Castle _castle, Message _message)
		{
			_message.WriteS64(_castle.castleID);
			_message.WriteS64(_castle.guildID);
			_message.WriteU8(_castle.constructed);
			_message.WriteS32(_castle.castleMoney);
			_message.WriteS32(_castle.weeklyIncome);
			_message.WriteU8(_castle.taxrate);
			_message.WriteS64(_castle.updateTime.Ticks);
			_message.WriteU8(_castle.sellDungeonPass);
			_message.WriteS32(_castle.dungeonPassPrice);
			_message.WriteS64(_castle.flag);
		}
	}
}
