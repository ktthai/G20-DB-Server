using Mabinogi;
using System;

namespace XMLDB3
{
	public class GoldLogSerializer
	{
		public static GoldLog Serialize(Message _message)
		{
			GoldLog goldLog = new GoldLog();
			goldLog.characterID = (long)_message.ReadU64();
			goldLog.quest = _message.ReadS32();
			goldLog.field = _message.ReadS32();
			goldLog.dynamicRegion = _message.ReadS32();
			goldLog.commerce = _message.ReadS32();
			goldLog.mail = _message.ReadS32();
			goldLog.bank = _message.ReadS32();
			goldLog.itembuysell = _message.ReadS32();
			goldLog.itemrepair = _message.ReadS32();
			goldLog.itemupgrade = _message.ReadS32();
			goldLog.itemspecialupgrade = _message.ReadS32();
			goldLog.mint = _message.ReadS32();
			goldLog.guild = _message.ReadS32();
			goldLog.privateshop = _message.ReadS32();
			goldLog.housing = _message.ReadS32();
			goldLog.etc = _message.ReadS32();
			goldLog.logDate = DateTime.Now.ToString();
			return goldLog;
		}

		public static void Deserialize(GoldLog _goldlog, Message _message)
		{
			_message.WriteU64((ulong)_goldlog.characterID);
			_message.WriteS32(_goldlog.quest);
			_message.WriteS32(_goldlog.field);
			_message.WriteS32(_goldlog.dynamicRegion);
			_message.WriteS32(_goldlog.commerce);
			_message.WriteS32(_goldlog.mail);
			_message.WriteS32(_goldlog.bank);
			_message.WriteS32(_goldlog.itembuysell);
			_message.WriteS32(_goldlog.itemrepair);
			_message.WriteS32(_goldlog.itemupgrade);
			_message.WriteS32(_goldlog.itemspecialupgrade);
			_message.WriteS32(_goldlog.mint);
			_message.WriteS32(_goldlog.guild);
			_message.WriteS32(_goldlog.privateshop);
			_message.WriteS32(_goldlog.housing);
			_message.WriteS32(_goldlog.etc);
		}
	}
}
