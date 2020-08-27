using Mabinogi;

namespace XMLDB3
{
	public class GoldLogInsert : BasicCommand
	{
		private GoldLog m_GoldLog;

		private bool m_Result;

		public GoldLogInsert()
			: base(NETWORKMSG.NET_DB_GOLD_LOG_INSERT)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_GoldLog = GoldLogSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GoldLogInsert.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("GoldLogInsert.DoProcess() : 로그를 보냅니다");
			m_Result = QueryManager.GoldLog.InsertLog(m_GoldLog);
			if (m_Result)
			{
				WorkSession.WriteStatus("GoldLogInsert.DoProcess() : 로그를 보냈습니다.");
			}
			else
			{
				WorkSession.WriteStatus("GoldLogInsert.DoProcess() : 로그를 보내는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GoldLogInsert.MakeMessage() : 함수에 진입하였습니다");
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
