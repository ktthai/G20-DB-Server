using Mabinogi;

namespace XMLDB3
{
	public class GuildNameUsableCommand : BasicCommand
	{
		private string m_Name;

		private bool m_Result;

		public GuildNameUsableCommand()
			: base(NETWORKMSG.NET_DB_GUILD_NAME_USABLE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Name = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildNameUsableCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.IsUsableName(m_Name);
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildNameUsableCommand.MakeMessage() : 함수에 진입하였습니다");
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
