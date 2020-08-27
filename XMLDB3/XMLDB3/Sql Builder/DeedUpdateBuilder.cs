using Mabinogi.SQL;

namespace XMLDB3
{
	public class DeedUpdateBuilder
	{
		public static void Build(Character _new, Character _old, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_new.deed.deedSet == null || _new.deed.deedSet.Length == 0 || _old.deed.deedSet == null || _old.deed.deedSet.Length == 0 || _new.deed.deedSet.Length != _old.deed.deedSet.Length)
			{
				for (int i = 0; i < _new.deed.deedSet.Length; i++)
				{
					UpdateCharacterDeed(_new.id, i, _new.deed.deedSet[i].deedUpdateTime, _new.deed.deedSet[i].deedBitFlag, conn, transaction);
				}
			}

			for (int j = 0; j < _new.deed.deedSet.Length; j++)
			{
				if (_new.deed.deedSet[j].deedBitFlag != _old.deed.deedSet[j].deedBitFlag || _new.deed.deedSet[j].deedUpdateTime != _old.deed.deedSet[j].deedUpdateTime)
				{
					UpdateCharacterDeed(_new.id, j, _new.deed.deedSet[j].deedUpdateTime, _new.deed.deedSet[j].deedBitFlag, conn, transaction);
				}
			}
		}

		private static void UpdateCharacterDeed(long id, int flagNum, int dayCount, long bitFlag, SimpleConnection conn, SimpleTransaction transaction)
		{
			// PROCEDURE: UpdateCharacterDeed
			using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterDeed, transaction))
			{
				upCmd.Where(Mabinogi.SQL.Columns.CharacterDeed.Id, id);

				upCmd.Set(Mabinogi.SQL.Columns.CharacterDeed.Flag[flagNum], bitFlag);
				upCmd.Set(Mabinogi.SQL.Columns.CharacterDeed.DayCount[flagNum], dayCount);

				if(upCmd.Execute() < 1)
				{
					using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterDeed, transaction))
					{
						insCmd.Set(Mabinogi.SQL.Columns.CharacterDeed.Id, id);

						insCmd.Set(Mabinogi.SQL.Columns.CharacterDeed.Flag[flagNum], bitFlag);
						insCmd.Set(Mabinogi.SQL.Columns.CharacterDeed.DayCount[flagNum], dayCount);

						insCmd.Execute();
					}
                }
			}
		}
	}
}
