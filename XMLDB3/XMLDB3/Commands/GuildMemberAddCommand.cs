using Mabinogi;

namespace XMLDB3
{
	public class GuildMemberAddCommand : BasicCommand
	{
		private long m_Id;

		private bool m_Result;

		private GuildMember m_Member;

		private string m_strJoinMsg = string.Empty;

		public GuildMemberAddCommand()
			: base(NETWORKMSG.NET_DB_GUILD_MEMBER_ADD)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = (long)_Msg.ReadU64();
			m_Member = GuildSerializer.ReadGuildMemberFromMsg(_Msg);
			m_strJoinMsg = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildMemberAddCommand.DoProcess() : 함수에 진입하였습니다");
			if (m_Member != null)
			{
				WorkSession.WriteStatus("GuildMemberAddCommand.DoProcess() : 길드 [" + m_Id + "] 에 [" + m_Member.memberid + "/" + m_Member.name + "] 를 멤버로 추가합니다");
				m_Result = QueryManager.Guild.AddMember(m_Id, m_Member, m_strJoinMsg);
				if (m_Result)
				{
					WorkSession.WriteStatus("GuildMemberAddCommand.DoProcess() : 길드 [" + m_Id + "] 에 [" + m_Member.memberid + "/" + m_Member.name + "] 를 멤버로 추가하였습니다");
				}
				else
				{
					WorkSession.WriteStatus("GuildMemberAddCommand.DoProcess() : 길드 [" + m_Id + "] 에 [" + m_Member.memberid + "/" + m_Member.name + "] 를 멤버로 추가하는데 실패하였습니다");
				}
			}
			else
			{
				WorkSession.WriteStatus("GuildMemberAddCommand.DoProcess() : 멤버 정보가 null 이기 때문에 추가 할 수 없습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildMemberAddCommand.MakeMessage() : 함수에 진입하였습니다");
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
