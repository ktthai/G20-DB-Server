using Mabinogi.SQL;

namespace XMLDB3
{
	public class CommercePostCreditUpdateBuilder
	{
		public static void Build(long _charID, int _postID, int _credit, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_postID != 0)
			{
				using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Commerce, transaction))
				{
					// PROCEDURE: dbo.UpdateCommercePostCredit
					cmd.Where(Mabinogi.SQL.Columns.Commerce.CharId, _charID);
					cmd.Set(Mabinogi.SQL.Columns.Commerce.Post[_postID - 1], _credit);
					cmd.Execute();
                }
            }
		}
	}
}
