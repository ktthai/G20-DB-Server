using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	internal class DivineKnightUpdateBuilder
	{
		public static void Build(Character _new, Character _old, SimpleConnection conn, SimpleTransaction transaction)
		{
			string text = string.Empty;
			bool flag = false;
			if (_old.divineKnight != null && _new.divineKnight != null)
			{
				if (!_new.divineKnight.Equals(_old.divineKnight))
				{
					flag = true;
				}
			}
			else if (_new.divineKnight != null)
			{
				flag = true;
			}
			if (flag)
			{
				DateTime date = DateTime.Now;
				// PROCEDURE: UpdateCharacterDivineKnight
				using(var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterDivineKnight, transaction))
				{
					upCmd.Where(Mabinogi.SQL.Columns.CharacterDivineKnight.Id, _new.id);

					upCmd.Set(Mabinogi.SQL.Columns.CharacterDivineKnight.Experience, _new.divineKnight.exp);
					upCmd.Set(Mabinogi.SQL.Columns.CharacterDivineKnight.GroupLimit, _new.divineKnight.groupLimit);
					upCmd.Set(Mabinogi.SQL.Columns.CharacterDivineKnight.GroupSelected, _new.divineKnight.groupSelected);
					upCmd.Set(Mabinogi.SQL.Columns.CharacterDivineKnight.UpdateDate, date);

					if (upCmd.Execute() < 1)
					{
						using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterDivineKnight, transaction))
						{
							insCmd.Set(Mabinogi.SQL.Columns.CharacterDivineKnight.Id, _new.id);
							insCmd.Set(Mabinogi.SQL.Columns.CharacterDivineKnight.Experience, _new.divineKnight.exp);
							insCmd.Set(Mabinogi.SQL.Columns.CharacterDivineKnight.GroupLimit, _new.divineKnight.groupLimit);
							insCmd.Set(Mabinogi.SQL.Columns.CharacterDivineKnight.GroupSelected, _new.divineKnight.groupSelected);
							insCmd.Set(Mabinogi.SQL.Columns.CharacterDivineKnight.UpdateDate, date);

							insCmd.Execute();
						}
                    }
				}
			}
		}
	}
}
