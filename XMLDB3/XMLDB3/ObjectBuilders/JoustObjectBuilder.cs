using Mabinogi.SQL;

namespace XMLDB3
{
	public class JoustObjectBuilder
	{
		public static CharacterJoust Build(SimpleReader reader)
		{
			CharacterJoust characterJoust = new CharacterJoust();
			characterJoust.joustPoint = reader.GetInt32(Mabinogi.SQL.Columns.Character.JoustPoint);
			characterJoust.joustLastWinYear = reader.GetByte(Mabinogi.SQL.Columns.Character.JoustLastWinYear);
			characterJoust.joustLastWinWeek = reader.GetByte(Mabinogi.SQL.Columns.Character.JoustLastWinWeek);
			characterJoust.joustWeekWinCount = reader.GetByte(Mabinogi.SQL.Columns.Character.JoustWeekWinCount);
			characterJoust.joustDailyWinCount = reader.GetInt16(Mabinogi.SQL.Columns.Character.JoustDailyWinCount);
			characterJoust.joustDailyLoseCount = reader.GetInt16(Mabinogi.SQL.Columns.Character.JoustDailyLoseCount);
			characterJoust.joustServerWinCount = reader.GetInt16(Mabinogi.SQL.Columns.Character.JoustServerWinCount);
			characterJoust.joustServerLoseCount = reader.GetInt16(Mabinogi.SQL.Columns.Character.JoustServerLoseCount);
			return characterJoust;
		}
	}
}
