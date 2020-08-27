using Mabinogi;

namespace XMLDB3
{
	public class LogInOutReportCommand : BasicCommand
	{
		private bool m_Result;

		private LogInOutReport m_LogInOutReport;

		public LogInOutReportCommand()
			: base(NETWORKMSG.NET_DB_COUNTRY_LOGINOUT_REPORT)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_LogInOutReport = LogInOutReportSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("LogInOutReportCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("LogInOutReportCommand.DoProcess() : 국가별 접속 정보를 기록합니다");
			m_Result = QueryManager.LogInOutReport.ReportLogInOut(m_LogInOutReport);
			if (m_Result)
			{
				WorkSession.WriteStatus("LogInOutReportCommand.DoProcess() : 국가별 접속 정보를 기록하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("LogInOutReportCommand.DoProcess() : 국가별 접속 정보 기록에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("LogInOutReportCommand.MakeMessage() : 함수에 진입하였습니다");
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
