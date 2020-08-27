using Mabinogi;

namespace XMLDB3
{
	public class GuildRobeDeleteCommand : BasicCommand
	{
		private long m_Id;

		private bool m_Result;

		public GuildRobeDeleteCommand()
			: base(NETWORKMSG.NET_DB_GUILD_DESTROY_ROBE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = (long)_Msg.ReadU64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildRobeAddCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.DeleteGuildRobe(m_Id);
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildRobeAddCommand.MakeMessage() : 함수에 진입하였습니다");
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
