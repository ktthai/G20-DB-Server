using Mabinogi;

namespace XMLDB3
{
	public class HouseBidEndCommand : BasicCommand
	{
		private long m_HouseID;

		private string m_Account = string.Empty;

		private string m_Server = string.Empty;

		private bool m_Result;

		public HouseBidEndCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_BID_END)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_HouseID = _Msg.ReadS64();
			m_Account = _Msg.ReadString();
			m_Server = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("HouseBidEndCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseBidEndCommand.DoProcess() : 집 경매를 종료합니다.");
			m_Result = QueryManager.House.EndBid(m_HouseID, m_Account, m_Server);
			if (m_Result)
			{
				WorkSession.WriteStatus("HouseBidEndCommand.DoProcess() : 집 경매를 종료 하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("HouseBidEndCommand.DoProcess() : 집 경매를 종료하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseBidEndCommand.MakeMessage() : 함수에 진입하였습니다");
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
