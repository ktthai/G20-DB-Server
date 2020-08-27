using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetConditionObjectBuilder
	{
		public static PetCondition[] Build(SimpleReader reader)
		{
			return JsonSerializer.Deserialize<PetCondition[]>(reader.GetString(Mabinogi.SQL.Columns.Pet.Condition));
		}
	}
}
