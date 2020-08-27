using Mabinogi;

namespace XMLDB3
{
	public class GuildDeleteCommand : BasicCommand
	{
		private long m_Id;

		private bool m_Result;

		public GuildDeleteCommand()
			: base(NETWORKMSG.NET_DB_GUILD_DELETE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = _Msg.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildDeleteCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("GuildDeleteCommand.DoProcess() : [" + m_Id + "] 길드를 제거합니다");
			m_Result = QueryManager.Guild.Delete(m_Id);
			if (m_Result)
			{
				WorkSession.WriteStatus("GuildDeleteCommand.DoProcess() : [" + m_Id + "] 길드를 제거하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("GuildDeleteCommand.DoProcess() : [" + m_Id + "] 길드 제거에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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
