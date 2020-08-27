using Mabinogi.SQL;

namespace XMLDB3
{
	internal class DivineKnightObjectBuilder
	{
		public static CharacterDivineKnight Build(SimpleReader reader)
		{
			CharacterDivineKnight characterDivineKnight = new CharacterDivineKnight();
			if (reader.Read())
			{
				characterDivineKnight.groupLimit = (byte)reader.GetInt32(Mabinogi.SQL.Columns.CharacterDivineKnight.GroupLimit);
				characterDivineKnight.groupSelected = (byte)reader.GetInt32(Mabinogi.SQL.Columns.CharacterDivineKnight.GroupSelected);
				characterDivineKnight.exp = (ulong)reader.GetInt64(Mabinogi.SQL.Columns.CharacterDivineKnight.Experience);
			}
			return characterDivineKnight;
		}
	}
}
