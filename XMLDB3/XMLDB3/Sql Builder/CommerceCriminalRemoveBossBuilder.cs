using Mabinogi.SQL;

namespace XMLDB3
{
	public class CommerceCriminalRemoveBossBuilder
	{
		public static void Build(int _criminalId, SimpleConnection conn, SimpleTransaction transaction)
		{
			using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CommerceCriminal, transaction))
			{
				cmd.Where(Mabinogi.SQL.Columns.CommerceCriminal.CriminalId, _criminalId);
				cmd.Execute();
			}
		}
	}
}
