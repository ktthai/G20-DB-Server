using Mabinogi.SQL;

namespace XMLDB3
{
	public class PVPObjectBuilder
	{
		public static CharacterPVP Build(SimpleReader reader)
		{
			CharacterPVP characterPVP = new CharacterPVP();
			if (reader.Read())
			{
				characterPVP.winCnt = reader.GetInt64(Mabinogi.SQL.Columns.CharacterPvP.WinCount);
				characterPVP.loseCnt = reader.GetInt64(Mabinogi.SQL.Columns.CharacterPvP.LoseCount);
				characterPVP.penaltyPoint = reader.GetInt32(Mabinogi.SQL.Columns.CharacterPvP.PenaltyPoint);
			}
			return characterPVP;
		}
	}
}
