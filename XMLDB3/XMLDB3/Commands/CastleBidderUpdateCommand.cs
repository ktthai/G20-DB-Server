using Mabinogi;

namespace XMLDB3
{
	public class CastleBidderUpdateCommand : BasicCommand
	{
		private long m_CastleID;

		private long m_GuildID;

		private int m_BidPrice;

		private int m_BidDiffPrice;

		private int m_BidOrder;

		private int m_RemainMoney;

		private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

		public CastleBidderUpdateCommand()
			: base(NETWORKMSG.NET_DB_CASTLE_BIDDER_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_CastleID = _Msg.ReadS64();
			m_GuildID = _Msg.ReadS64();
			m_BidPrice = _Msg.ReadS32();
			m_BidDiffPrice = _Msg.ReadS32();
			m_BidOrder = _Msg.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CastleBidderUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CastleBidderUpdateCommand.DoProcess() : 성 입찰가를 업데이트합니다.");
			m_Result = QueryManager.Castle.UpdateBidder(m_CastleID, m_GuildID, m_BidPrice, m_BidDiffPrice, m_BidOrder, QueryManager.Guild, ref m_RemainMoney);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("CastleBidderUpdateCommand.DoProcess() : 성 입찰가를 업데이트 하였습니다.");
				return true;
			}
			WorkSession.WriteStatus("CastleBidderUpdateCommand.DoProcess() : 성 입찰가를 업데이트 하는데 실패하였습니다.");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CastleBidderUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				message.WriteS32(m_RemainMoney);
			}
			return message;
		}
	}
}
