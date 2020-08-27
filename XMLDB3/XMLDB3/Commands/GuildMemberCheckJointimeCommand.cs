using Mabinogi;

namespace XMLDB3
{
	public class GuildMemberCheckJointimeCommand : BasicCommand
	{
		private long m_Id;

		private bool m_Result;

		private string m_Server = string.Empty;

		public GuildMemberCheckJointimeCommand()
			: base(NETWORKMSG.NET_DB_GUILD_MEMEBER_CHECK_JOINTIME)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = (long)_Msg.ReadU64();
			m_Server = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildMemberCheckJointimeCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.CheckMemberJointime(m_Id, m_Server);
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildMemberCheckJointimeCommand.MakeMessage() : 함수에 진입하였습니다");
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
