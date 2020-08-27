using Mabinogi.SQL;
using System.Text.Json;

namespace XMLDB3
{
	public class MemoryObjectBuilder
	{
		public static CharacterMemory[] Build(SimpleReader reader)
		{
			string memories = reader.GetString(Mabinogi.SQL.Columns.Character.Memory);
			if (memories != string.Empty)
			{
				return JsonSerializer.Deserialize<CharacterMemory[]>(memories);
			}
			return new CharacterMemory[0];
		}
	}
}
