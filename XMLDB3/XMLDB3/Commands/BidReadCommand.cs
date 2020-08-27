using Mabinogi;

namespace XMLDB3
{
	public class BidReadCommand : BasicCommand
	{
		private BidList m_BidList;

		public override bool IsPrimeCommand => true;

		public BidReadCommand()
			: base(NETWORKMSG.NET_DB_AUCTION_BID_READ)
		{
		}

		protected override void ReceiveData(Message _message)
		{
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("BidReadCommand.DoProcess() : 함수에 진입하였습니다");
			m_BidList = QueryManager.Bid.Read();
			if (m_BidList != null)
			{
				WorkSession.WriteStatus("BidReadCommand.DoProcess() : 경매 데이터를 성공적으로 읽었습니다");
			}
			else
			{
				WorkSession.WriteStatus("BidReadCommand.DoProcess() : 경매 데이터를 읽는데 실패하였습니다.");
			}
			return m_BidList != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("BidReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_BidList != null)
			{
				message.WriteU8(1);
				BidListSerializer.Deserialize(m_BidList, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
