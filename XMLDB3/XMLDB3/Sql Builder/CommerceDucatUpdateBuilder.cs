using Mabinogi.SQL;

namespace XMLDB3
{
	public class CommerceDucatUpdateBuilder
	{
		public static void LogDucat(long _charID, long _ducat, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_ducat < 0)
			{
				using (var logCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.LogDucat, transaction))
				{
					logCmd.Set(Mabinogi.SQL.Columns.LogDucat.CharId, _charID);
					logCmd.Set(Mabinogi.SQL.Columns.LogDucat.Ducat, _ducat);
					logCmd.Execute();
				}
				_ducat = 0;
			}
		}
		public static void Build(long _charID, long _ducat, SimpleConnection conn, SimpleTransaction transaction)
		{
			// PROCEDURE: dbo.UpdateCommerceDucat
			LogDucat(_charID, _ducat, conn, transaction);
			using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Commerce, transaction))
			{
                cmd.Where(Mabinogi.SQL.Columns.Commerce.CharId, _charID);
                cmd.Set(Mabinogi.SQL.Columns.Commerce.Ducat, _ducat);
                cmd.Execute();
            }
		}
	}
}
