using Mabinogi.SQL;

namespace XMLDB3
{
	public class FarmObjectBuilder
	{
		public static CharacterFarm Build(SimpleReader reader)
		{
			CharacterFarm characterFarm = new CharacterFarm();
			characterFarm.farmID = reader.GetInt64(Mabinogi.SQL.Columns.Character.FarmId);
			return characterFarm;
		}
	}
}
