using Mabinogi;

namespace XMLDB3
{
	public class CastleBidderCreateCommand : BasicCommand
	{
		private CastleBidder m_CastleBidder;

		private int m_RemainMoney;

		private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

		public CastleBidderCreateCommand()
			: base(NETWORKMSG.NET_DB_CASTLE_BIDDER_CREATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_CastleBidder = CastleBidderSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CastleBidderCreateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CastleBidderCreateCommand.DoProcess() : 성 입찰자를 생성합니다.");
			m_Result = QueryManager.Castle.CreateBidder(m_CastleBidder, QueryManager.Guild, ref m_RemainMoney);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("CastleBidderCreateCommand.DoProcess() : 성 입찰자를 생성하였습니다.");
				return true;
			}
			WorkSession.WriteStatus("CastleBidderCreateCommand.DoProcess() : 성 입찰자를 생성하는데 실패하였습니다.");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CastleBidderCreateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				message.WriteS32(m_RemainMoney);
			}
			else if (m_Result == REPLY_RESULT.FAIL_EX)
			{
				message.WriteU8(0);
				message.WriteS32(m_RemainMoney);
			}
			return message;
		}
	}
}
