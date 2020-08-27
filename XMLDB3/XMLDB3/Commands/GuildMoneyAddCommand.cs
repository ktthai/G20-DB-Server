using Mabinogi;

namespace XMLDB3
{
	public class GuildMoneyAddCommand : BasicCommand
	{
		private long m_Id;

		private int m_Money;

		private bool m_Result;

		public GuildMoneyAddCommand()
			: base(NETWORKMSG.NET_DB_GUILD_MONEY_ADD)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = (long)_Msg.ReadU64();
			m_Money = _Msg.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildMoneyAddCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.AddMoney(m_Id, m_Money);
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildMoneyAddCommand.MakeMessage() : 함수에 진입하였습니다");
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
