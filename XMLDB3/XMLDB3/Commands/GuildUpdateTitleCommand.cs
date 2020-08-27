using Mabinogi;

namespace XMLDB3
{
	public class GuildUpdateTitleCommand : BasicCommand
	{
		private long m_Id;

		private string m_strTitle;

		private bool m_bUsable;

		private bool m_Result;

		public GuildUpdateTitleCommand()
			: base(NETWORKMSG.NET_DB_GUILD_UPDATE_TITLE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = _Msg.ReadS64();
			m_strTitle = _Msg.ReadString();
			m_bUsable = ((_Msg.ReadU8() != 0) ? true : false);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildUpdateTitleCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus($"GuildUpdateTitleCommand.DoProcess() : [{m_Id}] 길드의 타이틀을 {m_strTitle} 으로 변경합니다.");
			m_Result = QueryManager.Guild.UpdateTitle(m_Id, m_strTitle, m_bUsable);
			if (m_Result)
			{
				WorkSession.WriteStatus($"GuildUpdateTitleCommand.DoProcess() : [{m_Id}] 길드의 타이틀을 {m_strTitle} 으로 변경했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("GuildUpdateTitleCommand.DoProcess() : [" + m_Id + "] 길드 타이틀 변경에 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildUpdateTitleCommand.MakeMessage() : 함수에 진입하였습니다");
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
