using Mabinogi;

namespace XMLDB3
{
	public class BidAddCommand : BasicCommand
	{
		private Bid m_Bid;

		private bool m_Result;

		public BidAddCommand()
			: base(NETWORKMSG.NET_DB_AUCTION_BID_ADD)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Bid = BidSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("BidAddCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Bid.Add(m_Bid);
			if (m_Result)
			{
				WorkSession.WriteStatus("BidAddCommand.DoProcess() : 경매 데이터를 성공적으로 생성하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("BidAddCommand.DoProcess() : 경매 데이터를 생성하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("BidAddCommand.MakeMessage() : 함수에 진입하였습니다");
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
