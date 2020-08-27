using Mabinogi;

namespace XMLDB3
{
	public class GuildDrawMoneyCommand : BasicCommand
	{
		private long m_Id;

		private int m_Money;

		private int m_remainMoney;

		private int m_remainDrawMoney;

		private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

		public GuildDrawMoneyCommand()
			: base(NETWORKMSG.NET_DB_GUILD_DRAW_MONEY)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = _Msg.ReadS64();
			m_Money = _Msg.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildDrawMoneyCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.WithdrawDrawableMoney(m_Id, m_Money, out m_remainMoney, out m_remainDrawMoney);
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildDrawMoneyCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			message.WriteS32(m_remainMoney);
			message.WriteS32(m_remainDrawMoney);
			return message;
		}
	}
}
