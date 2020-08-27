using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetMacroCheckerObjectBuilder
	{
		public static PetMacroChecker Build(SimpleReader reader)
		{
			PetMacroChecker petMacroChecker = new PetMacroChecker();
			petMacroChecker.macroPoint = reader.GetInt32(Mabinogi.SQL.Columns.Pet.MacroPoint);
			return petMacroChecker;
		}
	}
}
