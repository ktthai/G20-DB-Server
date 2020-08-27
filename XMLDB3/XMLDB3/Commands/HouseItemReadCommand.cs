using Mabinogi;

namespace XMLDB3
{
	public class HouseItemReadCommand : BasicCommand
	{
		private string m_Account = string.Empty;

		private HouseInventory m_HouseInventory;

		public HouseItemReadCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_ITEM_READ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("HouseItemReadCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseItemReadCommand.DoProcess() : 집 아이템을 읽습니다.");
			m_HouseInventory = QueryManager.House.ReadInventory(m_Account);
			if (m_HouseInventory != null)
			{
				WorkSession.WriteStatus("HouseItemReadCommand.DoProcess() : 집 아이템을 읽었습니다.");
			}
			else
			{
				WorkSession.WriteStatus("HouseItemReadCommand.DoProcess() : 집 아이템을 읽는데 실패하였습니다.");
			}
			return m_HouseInventory != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseItemReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_HouseInventory != null)
			{
				message.WriteU8(1);
				HouseInventorySerializer.Deserialize(m_HouseInventory, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
