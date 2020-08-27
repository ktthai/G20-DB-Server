using Mabinogi;

namespace XMLDB3
{
	public class GuildRobeUpdateCommand : BasicCommand
	{
		private long m_Id;

		private int m_GuildMoney;

		private int m_GuildPoint;

		private GuildRobe m_GuildRobe;

		private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

		private byte m_ErrorCode;

		public GuildRobeUpdateCommand()
			: base(NETWORKMSG.NET_DB_GUILD_UPDATE_ROBE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Id = (long)_message.ReadU64();
			m_GuildPoint = _message.ReadS32();
			m_GuildMoney = _message.ReadS32();
			m_GuildRobe = GuildRobeSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildRobeUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.UpdateGuildRobe(m_Id, m_GuildPoint, m_GuildMoney, m_GuildRobe, out m_ErrorCode);
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildRobeUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			message.WriteU8(m_ErrorCode);
			return message;
		}
	}
}
