using Mabinogi;

namespace XMLDB3
{
	public class GuildStoneChangeCommand : BasicCommand
	{
		private long m_Id;

		private int m_StoneType;

		private int m_RequiredMoney;

		private int m_RequiredGP;

		private int m_Result = -1;

		public GuildStoneChangeCommand()
			: base(NETWORKMSG.NET_DB_GUILD_CHANGE_GUILDSTONE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = (long)_Msg.ReadU64();
			m_StoneType = _Msg.ReadS32();
			m_RequiredMoney = _Msg.ReadS32();
			m_RequiredGP = _Msg.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildStoneChangeCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.ChangeGuildStone(m_Id, m_StoneType, m_RequiredMoney, m_RequiredGP);
			return m_Result == 0;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildStoneChangeCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result == 0)
			{
				message.WriteU8(1);
			}
			else
			{
				message.WriteU8(0);
				message.WriteS32(m_Result);
			}
			return message;
		}
	}
}
