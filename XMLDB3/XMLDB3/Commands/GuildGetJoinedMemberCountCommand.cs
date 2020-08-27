using Mabinogi;
using System;

namespace XMLDB3
{
	public class GuildGetJoinedMemberCountCommand : BasicCommand
	{
		private long m_Id;

		private DateTime m_startTime;

		private DateTime m_endTime;

		private int m_count;

		private bool m_Result;

		public GuildGetJoinedMemberCountCommand()
			: base(NETWORKMSG.NET_DB_GUILD_JOINED_MEMBER_COUNT)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = _Msg.ReadS64();
			m_startTime = new DateTime((long)_Msg.ReadU64());
			m_endTime = new DateTime((long)_Msg.ReadU64());
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildGetJoinedMemberCountCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("GuildGetJoinedMemberCountCommand.DoProcess() : 길드에 해당 기간동안 가입한 인원수를 가져옵니다.");
			m_Result = QueryManager.Guild.GetJoinedMemberCount(m_Id, m_startTime, m_endTime, out m_count);
			if (m_Result)
			{
				WorkSession.WriteStatus("GuildGetJoinedMemberCountCommand.DoProcess() : 길드에 해당 기간동안 가입한 인원수를 가져오는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("GuildGetJoinedMemberCountCommand.DoProcess() : 길드에 해당 기간동안 가입한 인원수를 가져오는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildGetJoinedMemberCountCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				message.WriteU16((ushort)m_count);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
