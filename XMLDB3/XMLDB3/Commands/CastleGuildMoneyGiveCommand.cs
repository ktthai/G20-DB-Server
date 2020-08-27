using Mabinogi;

namespace XMLDB3
{
	public class CastleGuildMoneyGiveCommand : BasicCommand
	{
		private long m_CastleID;

		private long m_GuildID;

		private int m_Money;

		private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

		public CastleGuildMoneyGiveCommand()
			: base(NETWORKMSG.NET_DB_CASTLE_GUILD_MONEY_GIVE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_CastleID = _Msg.ReadS64();
			m_GuildID = _Msg.ReadS64();
			m_Money = _Msg.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CastleGuildMoneyGiveCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CastleGuildMoneyGiveCommand.DoProcess() : 성의 돈을 길드에 줍니다.");
			m_Result = QueryManager.Castle.GiveGuildMoney(m_CastleID, m_GuildID, m_Money, QueryManager.Guild);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("CastleGuildMoneyGiveCommand.DoProcess() : 성의 돈을 길드에 줬습니다.");
				return true;
			}
			WorkSession.WriteStatus("CastleGuildMoneyGiveCommand.DoProcess() : 성의 돈을 길드에 주는데 실패하였습니다.");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CastleGuildMoneyGiveCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			return message;
		}
	}
}
