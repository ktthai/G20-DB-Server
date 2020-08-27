using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class GuildBattleGroundTypeClearCommand : BasicCommand
	{
		private string m_Server = string.Empty;

		private List<long> m_GuildList = new List<long>();

		private bool m_Result;

		public GuildBattleGroundTypeClearCommand()
			: base(NETWORKMSG.NET_DB_GUILD_CLEAR_BATTLEGROUND_TYPE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Server = _message.ReadString();
			uint num = _message.ReadU32();
			for (uint num2 = 0u; num2 < num; num2++)
			{
				long num3 = (long)_message.ReadU64();
				m_GuildList.Add(num3);
			}
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildBattleGroundTypeClearCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.ClearBattleGroundType(m_Server, m_GuildList);
			if (m_Result)
			{
				WorkSession.WriteStatus("GuildBattleGroundTypeUpdateCommand.DoProcess() : [" + m_Server + "] 서버 길드의 길드전 참가 여부를 초기화 했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("GuildBattleGroundTypeUpdateCommand.DoProcess() : [" + m_Server + "] 서버 길드의 길드전 참가 여부 초기화를 실패했습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildBattleGroundTypeClearCommand.MakeMessage() : 함수에 진입하였습니다");
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
