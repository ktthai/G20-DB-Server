using Mabinogi.SQL;

namespace XMLDB3
{
	public class JoustUpdateBuilder
	{
		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.joust != null && _old.joust != null)
			{
				if (_new.joust.joustPoint != _old.joust.joustPoint)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.JoustPoint, _new.joust.joustPoint);
                    result = true;
                }
				if (_new.joust.joustLastWinYear != _old.joust.joustLastWinYear)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.JoustLastWinYear, _new.joust.joustLastWinYear);
                    result = true;
                }
				if (_new.joust.joustLastWinWeek != _old.joust.joustLastWinWeek)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.JoustLastWinWeek, _new.joust.joustLastWinWeek);
                    result = true;
                }
				if (_new.joust.joustWeekWinCount != _old.joust.joustWeekWinCount)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.JoustWeekWinCount, _new.joust.joustWeekWinCount);
                    result = true;
                }
				if (_new.joust.joustDailyWinCount != _old.joust.joustDailyWinCount)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.JoustDailyWinCount, _new.joust.joustDailyWinCount);
                    result = true;
                }
				if (_new.joust.joustDailyLoseCount != _old.joust.joustDailyLoseCount)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.JoustDailyLoseCount, _new.joust.joustDailyLoseCount);
                    result = true;
                }
				if (_new.joust.joustServerWinCount != _old.joust.joustServerWinCount)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.JoustServerWinCount, _new.joust.joustServerWinCount);
                    result = true;
                }
				if (_new.joust.joustServerLoseCount != _old.joust.joustServerLoseCount)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.JoustServerLoseCount, _new.joust.joustServerLoseCount);
                    result = true;
                }
			}
			return result;
		}
	}
}
