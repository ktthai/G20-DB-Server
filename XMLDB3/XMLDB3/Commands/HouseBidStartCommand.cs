using Mabinogi;

namespace XMLDB3
{
	public class HouseBidStartCommand : BasicCommand
	{
		private long m_HouseID;

		private HouseBid m_HouseBid;

		private bool m_Result;

		public HouseBidStartCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_BID_START)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_HouseID = _Msg.ReadS64();
			m_HouseBid = HouseBidSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("HouseBidStartCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseBidStartCommand.DoProcess() : 집 경매를 시작합니다.");
			m_Result = QueryManager.House.CreateBid(m_HouseID, m_HouseBid);
			if (m_Result)
			{
				WorkSession.WriteStatus("HouseBidStartCommand.DoProcess() : 집 경매를 시작하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("HouseBidStartCommand.DoProcess() : 집 경매를 시작하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseListReadCommand.MakeMessage() : 함수에 진입하였습니다");
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
