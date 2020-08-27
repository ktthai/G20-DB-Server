using Mabinogi;

namespace XMLDB3
{
	public class GuildIdPoolCommand : BasicCommand
	{
		private long m_IdOffset;

		public GuildIdPoolCommand()
			: base(NETWORKMSG.NET_DB_GUILD_ID_POOL)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildIdPoolCommand.DoProcess() : 함수에 진입하였습니다");
			m_IdOffset = QueryManager.GuildIdPool.GetIdPool();
			return true;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildIdPoolCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU64((ulong)m_IdOffset);
			return message;
		}
	}
}
