using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class CommerceUnlockTransportUpdateBuilder
	{
		public static void Build(long _charID, long _unlockTransport, SimpleConnection conn, SimpleTransaction transaction)
		{
			// PROCEDURE: dbo.UpdateCommerceUnlockTransport
			using(var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Commerce, transaction))
			{
				cmd.Where(Mabinogi.SQL.Columns.Commerce.CharId, _charID);
                cmd.Set(Mabinogi.SQL.Columns.Commerce.UnlockTransport, _unlockTransport);
                cmd.Set(Mabinogi.SQL.Columns.Commerce.UpdateDate, DateTime.Now);
				cmd.Execute();
            }
		}
	}
}
