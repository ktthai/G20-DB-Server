using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class ArbeitObjectBuilder
	{
		public static CharacterArbeit Build(SimpleReader reader)
		{
			CharacterArbeit characterArbeit = new CharacterArbeit();

			characterArbeit.history = JsonSerializer.Deserialize<CharacterArbeitDay[]>(reader.GetString(Mabinogi.SQL.Columns.Character.History));
			characterArbeit.collection = JsonSerializer.Deserialize<CharacterArbeitInfo[]>(reader.GetString(Mabinogi.SQL.Columns.Character.Collection));

			return characterArbeit;
		}
	}
}
