using Mabinogi;

namespace XMLDB3
{
	public class GuildPointAddCommand : BasicCommand
	{
		private long m_Id;

		private int m_Point;

		private bool m_Result;

		public GuildPointAddCommand()
			: base(NETWORKMSG.NET_DB_GUILD_POINT_ADD)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = (long)_Msg.ReadU64();
			m_Point = _Msg.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildPointAddCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.AddPoint(m_Id, m_Point);
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildPointAddCommand.MakeMessage() : 함수에 진입하였습니다");
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
