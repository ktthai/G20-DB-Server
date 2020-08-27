using Mabinogi;

namespace XMLDB3
{
	public class HouseBidRepayEndCommand : BasicCommand
	{
		private long m_HouseID;

		private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

		public HouseBidRepayEndCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_BID_REPAY_END)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_HouseID = _Msg.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("HouseBidRepayEndCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseBidRepayEndCommand.DoProcess() : 환불을 종료합니다.");
			m_Result = QueryManager.House.EndBidRepay(m_HouseID);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("HouseBidRepayEndCommand.DoProcess() : 환불을 종료 하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("HouseBidRepayEndCommand.DoProcess() : 환불을 종료하는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseBidRepayEndCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			return message;
		}
	}
}
