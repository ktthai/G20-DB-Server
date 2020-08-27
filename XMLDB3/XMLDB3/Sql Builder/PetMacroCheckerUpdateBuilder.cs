using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetMacroCheckerUpdateBuilder
	{
		public static void Build(Pet _new, Pet _old, ref SimpleCommand cmd)
		{
			if (_new.macroChecker != null && _old.macroChecker != null)
			{
				if (_new.macroChecker.macroPoint != _old.macroChecker.macroPoint)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.MacroPoint, _new.macroChecker.macroPoint);
				}
			}
		}
	}
}
