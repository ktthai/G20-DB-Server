using Mabinogi;

namespace XMLDB3
{
	public class CastleGuildMoneyTakeCommand : BasicCommand
	{
		private long m_CastleID;

		private long m_GuildID;

		private int m_Money;

		private int m_RemainMoney;

		private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

		public CastleGuildMoneyTakeCommand()
			: base(NETWORKMSG.NET_DB_CASTLE_GUILD_MONEY_TAKE)
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
			WorkSession.WriteStatus("CastleGuildMoneyTakeCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CastleGuildMoneyTakeCommand.DoProcess() : 길드 머니를 성에 추가합니다.");
			m_Result = QueryManager.Castle.TakeGuildMoney(m_CastleID, m_GuildID, m_Money, ref m_RemainMoney, QueryManager.Guild);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("CastleGuildMoneyTakeCommand.DoProcess() : 길드 머니를 성에 추가하였습니다.");
				return true;
			}
			WorkSession.WriteStatus("CastleGuildMoneyTakeCommand.DoProcess() : 길드 머니를 성에 추가하는데 실패하였습니다.");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CastleGuildMoneyTakeCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			if (m_Result == REPLY_RESULT.FAIL_EX)
			{
				message.WriteU8(0);
				message.WriteS32(m_RemainMoney);
			}
			return message;
		}
	}
}
