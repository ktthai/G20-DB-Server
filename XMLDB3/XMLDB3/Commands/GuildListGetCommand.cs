using Mabinogi;
using System;

namespace XMLDB3
{
	public class GuildListGetCommand : BasicCommand
	{
		private GuildIDList m_GuildList;

		private GuildIDList m_DeleteList;

		private string m_Server = string.Empty;

		private long m_TimeTick;

		private DateTime m_CurrentDBTime = DateTime.MinValue;

		public override bool IsPrimeCommand => true;

		public GuildListGetCommand()
			: base(NETWORKMSG.NET_DB_GUILDLIST_READ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Server = _Msg.ReadString();
			m_TimeTick = (long)_Msg.ReadU64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : DB 시간의 현재시간을 얻어옵니다");
			m_CurrentDBTime = QueryManager.Guild.GetDBCurrentTime();
			if (m_TimeTick == 0)
			{
				WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : 길드 전체 리스트를 얻어옵니다");
				m_GuildList = QueryManager.Guild.LoadGuildList(m_Server, DateTime.MinValue);
				m_DeleteList = new GuildIDList();
				m_DeleteList.guildID = null;
			}
			else
			{
				WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : 특정 시간까지의 변화된 길드 리스트를 얻어옵니다");
				m_GuildList = QueryManager.Guild.LoadGuildList(m_Server, new DateTime(m_TimeTick));
				WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : 특정 시간까지의 삭제된 길드 리스트를 얻어옵니다");
				m_DeleteList = QueryManager.Guild.LoadDeletedGuildList(m_Server, new DateTime(m_TimeTick));
			}
			if (m_GuildList != null && m_DeleteList != null)
			{
				WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : 길드 리스트를 얻어왔습니다");
				return true;
			}
			WorkSession.WriteStatus("GuildListGetCommand.DoProcess() : 길드 리스트를 얻는데 실패하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildListGetCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_GuildList != null && m_DeleteList != null)
			{
				message.WriteU8(1);
				message.WriteU64((ulong)m_CurrentDBTime.Ticks);
				GuildIDListSerializer.Deserialize(m_GuildList, message);
				GuildIDListSerializer.Deserialize(m_DeleteList, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
