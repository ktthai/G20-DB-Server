using Mabinogi.SQL;

namespace XMLDB3
{
	public class MacroCheckerUpdateBuilder
	{
		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.macroChecker != null && _old.macroChecker != null)
			{
				if (_new.macroChecker.macroPoint != _old.macroChecker.macroPoint)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.MacroPoint, _new.macroChecker.macroPoint);
                    result = true;
                }
			}
			return result;
		}
	}
}
