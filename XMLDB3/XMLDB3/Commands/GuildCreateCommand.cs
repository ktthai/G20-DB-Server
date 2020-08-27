using Mabinogi;

namespace XMLDB3
{
	public class GuildCreateCommand : BasicCommand
	{
		private Guild m_Guild;

		private bool m_Result;

		public GuildCreateCommand()
			: base(NETWORKMSG.NET_DB_GUILD_CREATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Guild = GuildSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildCreateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("GuildCreateCommand.DoProcess() : [" + m_Guild.id + "/" + m_Guild.name + "@" + m_Guild.server + "] 길드를 생성합니다");
			m_Result = QueryManager.Guild.Create(m_Guild);
			if (m_Result)
			{
				WorkSession.WriteStatus("GuildCreateCommand.DoProcess() : [" + m_Guild.id + "/" + m_Guild.name + "@" + m_Guild.server + "] 길드를 생성하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("GuildCreateCommand.DoProcess() : [" + m_Guild.id + "/" + m_Guild.name + "@" + m_Guild.server + "] 길드 생성에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildCreateCommand.MakeMessage() : 함수에 진입하였습니다");
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
