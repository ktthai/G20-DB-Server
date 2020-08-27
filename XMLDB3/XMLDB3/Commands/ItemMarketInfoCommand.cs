using Mabinogi;

namespace XMLDB3
{
	public class ItemMarketInfoCommand : BasicCommand
	{
		public ItemMarketInfoCommand()
			: base(NETWORKMSG.NET_DB_ITEMMARKET_INFO)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
		}

		public override bool DoProcess()
		{
			return ConfigManager.ItemMarketEnabled;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ItemMarketInfoCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (ConfigManager.ItemMarketEnabled)
			{
				message.WriteU8(1);
				message.WriteU32((uint)ConfigManager.ItemMarketGameNo);
				message.WriteU32((uint)ConfigManager.ItemMarketServerNo);
				message.WriteString(ConfigManager.ItemMarketIP);
				message.WriteU16((ushort)ConfigManager.ItemMarketPort);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
