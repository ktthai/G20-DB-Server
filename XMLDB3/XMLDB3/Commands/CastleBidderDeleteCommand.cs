using Mabinogi;

namespace XMLDB3
{
	public class CastleBidderDeleteCommand : BasicCommand
	{
		private long m_CastleID;

		private long m_GuildID;

		private int m_RepayMoney;

		private int m_RemainMoney;

		private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

		public CastleBidderDeleteCommand()
			: base(NETWORKMSG.NET_DB_CASTLE_BIDDER_DELETE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_CastleID = _Msg.ReadS64();
			m_GuildID = _Msg.ReadS64();
			m_RepayMoney = _Msg.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CastleBidderDeleteCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CastleBidderDeleteCommand.DoProcess() : 성 입찰을 취소합니다.");
			m_Result = QueryManager.Castle.DeleteBidder(m_CastleID, m_GuildID, m_RepayMoney, QueryManager.Guild, ref m_RemainMoney);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("CastleBidderDeleteCommand.DoProcess() : 성 입찰을 취소하였습니다.");
				return true;
			}
			WorkSession.WriteStatus("CastleBidderDeleteCommand.DoProcess() : 성 입찰을 취소하는데 실패하였습니다.");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CastleBidderDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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
