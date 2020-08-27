using Mabinogi;

namespace XMLDB3
{
	public class LogInOutReportSerializer
	{
		public static LogInOutReport Serialize(Message _message)
		{
			LogInOutReport logInOutReport = new LogInOutReport();
			logInOutReport.inout = _message.ReadString();
			logInOutReport.account = _message.ReadString();
			logInOutReport.ip = _message.ReadString();
			logInOutReport.countrycode = _message.ReadString();
			return logInOutReport;
		}

		public static void Deserialize(LogInOutReport _report, Message _message)
		{
		}
	}
}
