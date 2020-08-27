using Mabinogi;

namespace XMLDB3
{
	public class GuildStatusUpdateCommand : BasicCommand
	{
		private long m_Id;

		private byte m_StatusFlag;

		private bool m_Set;

		private int m_PointRequired;

		private bool m_Result;

		public GuildStatusUpdateCommand()
			: base(NETWORKMSG.NET_DB_GUILD_UPDATE_STATUS_FLAG)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Id = (long)_message.ReadU64();
			m_StatusFlag = _message.ReadU8();
			m_Set = (_message.ReadU8() != 0);
			m_PointRequired = _message.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildStatusUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.UpdateGuildStatus(m_Id, m_StatusFlag, m_Set, m_PointRequired);
			if (m_Result)
			{
				WorkSession.WriteStatus("GuildStatusUpdateCommand.DoProcess() : [" + m_Id + "] 길드의 StatusFlag를 [" + m_StatusFlag + "] 로 [" + m_Set + "] 변경했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("GuildStatusUpdateCommand.DoProcess() : [" + m_Id + "] 길드의 StatusFlag를 [" + m_StatusFlag + "] 로 [" + m_Set + "] 변경에 실패했습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildStatusUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
