using Mabinogi.SQL;
using System;

namespace XMLDB3
{
	public class CommerceLostPercentUpdateBuilder
	{
		public static void Build(long _charID, int _lostPercent, SimpleConnection conn, SimpleTransaction transaction)
		{
            // PROCEDURE: dbo.UpdateCommerceLostPercent"
			using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Commerce, transaction))
            {
				cmd.Where(Mabinogi.SQL.Columns.Commerce.CharId, _charID);
				cmd.Set(Mabinogi.SQL.Columns.Commerce.LostPercent, _lostPercent);
				cmd.Set(Mabinogi.SQL.Columns.Commerce.UpdateDate, DateTime.Now);
			}
        }
	}
}
