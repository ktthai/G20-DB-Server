using Mabinogi;

namespace XMLDB3
{
	public class GuildUpdateCommand : BasicCommand
	{
		private long m_Id;

		private int m_Gold;

		private int m_GP;

		private int m_Flag;

		private bool m_Result;

		public GuildUpdateCommand()
			: base(NETWORKMSG.NET_DB_GUILD_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = (long)_Msg.ReadU64();
			m_GP = _Msg.ReadS32();
			m_Gold = _Msg.ReadS32();
			m_Flag = _Msg.ReadS8();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.UpdateGuildProperties(m_Id, m_GP, m_Gold, m_Flag);
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)(m_Result ? 1 : 0));
			return message;
		}
	}
}
