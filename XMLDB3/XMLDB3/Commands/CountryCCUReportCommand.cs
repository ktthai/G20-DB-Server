using Mabinogi;

namespace XMLDB3
{
	public class CountryCCUReportCommand : BasicCommand
	{
		private bool m_Result;

		private CountryReport m_CountryReport;

		public CountryCCUReportCommand()
			: base(NETWORKMSG.NET_DB_COUNTRY_CCU_REPORT)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_CountryReport = CountryReportSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CountryCCUReportCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CountryCCUReportCommand.DoProcess() : 국가별 동접 정보를 기록합니다");
			m_Result = QueryManager.CountryReport.ReportCCU(m_CountryReport);
			if (m_Result)
			{
				WorkSession.WriteStatus("CountryCCUReportCommand.DoProcess() : 국가별 동접 정보를 기록하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("CountryCCUReportCommand.DoProcess() : 국가별 동접 정보 기록에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CountryCCUReportCommand.MakeMessage() : 함수에 진입하였습니다");
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
