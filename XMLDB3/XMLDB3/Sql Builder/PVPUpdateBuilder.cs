using Mabinogi.SQL;

namespace XMLDB3
{
	public class PVPUpdateBuilder
	{
		public static void Build(Character _new, Character _old, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_new.PVP == null)
			{
				_new.PVP = new CharacterPVP();
			}

			using(var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterPvP, transaction))
			{
				upCmd.Where(Mabinogi.SQL.Columns.CharacterPvP.Id, _new.id);
				
				upCmd.Set(Mabinogi.SQL.Columns.CharacterPvP.WinCount, _new.PVP.winCnt);
				upCmd.Set(Mabinogi.SQL.Columns.CharacterPvP.LoseCount, _new.PVP.loseCnt);
				upCmd.Set(Mabinogi.SQL.Columns.CharacterPvP.PenaltyPoint, _new.PVP.penaltyPoint);

				if(upCmd.Execute() < 1)
				{
					using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterPvP, transaction))
					{
						insCmd.Where(Mabinogi.SQL.Columns.CharacterPvP.Id, _new.id);

						insCmd.Set(Mabinogi.SQL.Columns.CharacterPvP.WinCount, _new.PVP.winCnt);
						insCmd.Set(Mabinogi.SQL.Columns.CharacterPvP.LoseCount, _new.PVP.loseCnt);
						insCmd.Set(Mabinogi.SQL.Columns.CharacterPvP.PenaltyPoint, _new.PVP.penaltyPoint);

						insCmd.Execute();
					}
                }
			}
		}
	}
}
