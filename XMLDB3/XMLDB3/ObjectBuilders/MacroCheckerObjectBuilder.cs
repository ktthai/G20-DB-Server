using Mabinogi.SQL;

namespace XMLDB3
{
	public class MacroCheckerObjectBuilder
	{
		public static CharacterMacroChecker Build(SimpleReader reader)
		{
			CharacterMacroChecker characterMacroChecker = new CharacterMacroChecker();
			characterMacroChecker.macroPoint = reader.GetInt32(Mabinogi.SQL.Columns.Character.MacroPoint);
			return characterMacroChecker;
		}
	}
}
