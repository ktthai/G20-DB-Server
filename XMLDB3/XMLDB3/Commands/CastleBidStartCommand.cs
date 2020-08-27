using Mabinogi;

namespace XMLDB3
{
	public class CastleBidStartCommand : BasicCommand
	{
		private CastleBid m_CastleBid;

		private bool m_Result;

		public CastleBidStartCommand()
			: base(NETWORKMSG.NET_DB_CASTLE_BID_START)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_CastleBid = CastleBidSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CastleBidStartCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CastleBidStartCommand.DoProcess() : 성 경매를 시작.");
			m_Result = QueryManager.Castle.CreateBid(m_CastleBid);
			if (m_Result)
			{
				WorkSession.WriteStatus("CastleBidStartCommand.DoProcess() : 성 경매를 시작하였습니다..");
				return true;
			}
			WorkSession.WriteStatus("CastleBidStartCommand.DoProcess() : 성 경매를 시작하는데 실패하였습니다.");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CastleBidStartCommand.MakeMessage() : 함수에 진입하였습니다");
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
