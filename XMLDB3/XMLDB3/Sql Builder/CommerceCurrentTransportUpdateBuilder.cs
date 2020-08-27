using Mabinogi.SQL;

namespace XMLDB3
{
	public class CommerceCurrentTransportUpdateBuilder
	{
		public static void Build(long _charID, int _currentTransport, SimpleConnection conn, SimpleTransaction transaction)
		{
			// PROCEDURE: dbo.UpdateCommerceCurrentTransportID
			using(var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Commerce, transaction))
			{
				cmd.Where(Mabinogi.SQL.Columns.Commerce.CharId, _charID);
				cmd.Set(Mabinogi.SQL.Columns.Commerce.CurrentTransportId, _currentTransport);
				cmd.Execute();
			}
		}
	}
}
