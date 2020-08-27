using Mabinogi;

namespace XMLDB3
{
	public class GuildStoneAddCommand : BasicCommand
	{
		private long m_Id;

		private GuildStone m_GuildStone;

		private bool m_Result;

		public GuildStoneAddCommand()
			: base(NETWORKMSG.NET_DB_GUILD_STONE_ADD)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = (long)_Msg.ReadU64();
			m_GuildStone = GuildStoneSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildStoneAddCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.SetGuildStone(m_Id, m_GuildStone);
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildStoneAddCommand.MakeMessage() : 함수에 진입하였습니다");
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
