using Mabinogi;

namespace XMLDB3
{
	public class HouseItemUpdateCommand : BasicCommand
	{
		private string m_Account = string.Empty;

		private HouseItem m_Item;

		private bool m_Result;

		public HouseItemUpdateCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_ITEM_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_Item = HouseItemSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("HouseItemUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseItemUpdateCommand.DoProcess() : 집 아이템을 업데이트합니다.");
			m_Result = QueryManager.House.UpdateItem(m_Account, m_Item);
			if (m_Result)
			{
				WorkSession.WriteStatus("HouseItemUpdateCommand.DoProcess() : 집 아이템을 업데이트하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("HouseItemUpdateCommand.DoProcess() : 집 아이템을 업데이트하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseItemUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
