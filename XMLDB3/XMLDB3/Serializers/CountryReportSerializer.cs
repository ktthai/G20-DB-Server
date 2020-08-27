using Mabinogi;

namespace XMLDB3
{
	public class CountryReportSerializer
	{
		public static CountryReport Serialize(Message _message)
		{
			CountryReport countryReport = new CountryReport();
			countryReport.reportstring = _message.ReadString();
			return countryReport;
		}

		public static void Deserialize(CountryReport _report, Message _message)
		{
		}
	}
}
