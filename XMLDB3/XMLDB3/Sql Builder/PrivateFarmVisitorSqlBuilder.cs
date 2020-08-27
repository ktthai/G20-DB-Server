using Mabinogi.SQL;

namespace XMLDB3
{
	public sealed class PrivateFarmVisitorSqlBuilder
	{
		private static bool CheckUpdate(PrivateFarmVisitor _new, PrivateFarmVisitor _old)
		{
			if (_new == null)
			{
				return false;
			}
			if (_old == null)
			{
				return true;
			}
			if (_new.status != _old.status)
			{
				return true;
			}
			if (_new.charName != _old.charName)
			{
				return true;
			}
			return false;
		}

		public static void UpdateVisitor(long _idPrivateFarm, PrivateFarmVisitor _new, PrivateFarmVisitor _old, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (CheckUpdate(_new, _old))
			{
				// PROCEDURE: UpdatePrivateFarmVisitor
				using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmVisitor, transaction))
				{
					cmd.Where(Mabinogi.SQL.Columns.PrivateFarmVisitor.PrivateFarmId, _idPrivateFarm);
					cmd.Where(Mabinogi.SQL.Columns.PrivateFarmVisitor.CharId, _new.charId);
					
					cmd.Set(Mabinogi.SQL.Columns.PrivateFarmVisitor.Status, _new.status);

					if (cmd.Execute() < 1)
					{
						using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmVisitor, transaction))
						{
							insCmd.Set(Mabinogi.SQL.Columns.PrivateFarmVisitor.PrivateFarmId, _idPrivateFarm);
							insCmd.Set(Mabinogi.SQL.Columns.PrivateFarmVisitor.CharId, _new.charId);
							insCmd.Set(Mabinogi.SQL.Columns.PrivateFarmVisitor.Account, _new.accountName);
							insCmd.Set(Mabinogi.SQL.Columns.PrivateFarmVisitor.Status, _new.status);

                            insCmd.Execute();
						}
					}
				}
			}
		}

		public static void DeleteVisitor(long _idPrivateFarm, long _idChar, SimpleConnection conn, SimpleTransaction transaction)
		{
			// PROCEDURE: DeletePrivateFarmVisitor
			using(var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmVisitor, transaction))
			{
				cmd.Where(Mabinogi.SQL.Columns.PrivateFarmVisitor.PrivateFarmId, _idPrivateFarm);
				cmd.Where(Mabinogi.SQL.Columns.PrivateFarmVisitor.CharId, _idChar);
			}
		}
	}
}
