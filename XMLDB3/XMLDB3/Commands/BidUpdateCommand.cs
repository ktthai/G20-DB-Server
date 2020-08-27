using Mabinogi;

namespace XMLDB3
{
	public class BidUpdateCommand : BasicCommand
	{
		private Bid m_Bid;

		private REPLY_RESULT m_Result;

		private byte m_errorCode;

		public BidUpdateCommand()
			: base(NETWORKMSG.NET_DB_AUCTION_BID_UPDATE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Bid = BidSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("BidUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Bid.Update(m_Bid, ref m_errorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("BidUpdateCommand.DoProcess() : 경매 데이터를 성공적으로 삭제했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("BidUpdateCommand.DoProcess() : 경매 데이터를 삭제하는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("BidRemoveCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			if (m_Result == REPLY_RESULT.FAIL_EX)
			{
				message.WriteU8(m_errorCode);
			}
			return message;
		}
	}
}
