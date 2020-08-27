using Mabinogi;

namespace XMLDB3
{
	public class BidRemoveCommand : BasicCommand
	{
		private long m_BidID;

		private REPLY_RESULT m_Result;

		private byte m_errorCode;

		public BidRemoveCommand()
			: base(NETWORKMSG.NET_DB_AUCTION_BID_REMOVE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_BidID = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("BidRemoveCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Bid.Remove(m_BidID, ref m_errorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("BidRemoveCommand.DoProcess() : 경매 데이터를 성공적으로 삭제했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("BidRemoveCommand.DoProcess() : 경매 데이터를 삭제하는데 실패하였습니다.");
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
