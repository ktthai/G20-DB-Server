using Mabinogi;

namespace XMLDB3
{
	public class GuildReadCommand : BasicCommand
	{
		private Guild m_Guild;

		private long m_Id;

		public override bool IsPrimeCommand => true;

		public GuildReadCommand()
			: base(NETWORKMSG.NET_DB_GUILD_READ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = (long)_Msg.ReadU64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildReadCommand.DoProcess() : 함수에 진입하였습니다");
			m_Guild = QueryManager.Guild.Read(m_Id);
			if (m_Guild != null && m_Guild.guildtitle == null)
			{
				m_Guild.guildtitle = "";
			}
			if (m_Guild == null)
			{
				return false;
			}
			return true;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Guild != null)
			{
				message.WriteU8(1);
				GuildSerializer.Deserialize(m_Guild, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
