using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetMemoryObjectBuilder
	{

		public static PetMemory[] Build(SimpleReader reader)
		{
			return JsonSerializer.Deserialize<PetMemory[]>(reader.GetString(Mabinogi.SQL.Columns.Pet.Memory));
		}
	}
}
