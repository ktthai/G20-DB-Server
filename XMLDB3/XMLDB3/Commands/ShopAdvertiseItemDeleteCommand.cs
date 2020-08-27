using Mabinogi;

namespace XMLDB3
{
	public class ShopAdvertiseItemDeleteCommand : BasicCommand
	{
		private string m_Account = string.Empty;

		private string m_Server = string.Empty;

		private long m_ItemID;

		private bool m_Result;

		public ShopAdvertiseItemDeleteCommand()
			: base(NETWORKMSG.NET_DB_ADVERTISE_ITEM_REMOVE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Account = _message.ReadString();
			m_Server = _message.ReadString();
			m_ItemID = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ShopAdvertiseItemDeleteCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("ShopAdvertiseItemDeleteCommand.DoProcess() : 상점 광고에 아이템을 삭데합니다.");
			m_Result = QueryManager.ShopAdvertise.DeleteItem(m_Account, m_Server, m_ItemID);
			if (m_Result)
			{
				WorkSession.WriteStatus("ShopAdvertiseItemDeleteCommand.DoProcess() : 상점 광고에 아이템을 삭제하는데 성공하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("ShopAdvertiseItemDeleteCommand.DoProcess() : 점 광고에 아이템을 삭제하는데 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ShopAdvertiseItemDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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
