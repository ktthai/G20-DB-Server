using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class ConditionObjectBuilder
	{
		public static CharacterCondition[] Build(SimpleReader reader)
		{
			return JsonSerializer.Deserialize<CharacterCondition[]>(reader.GetString(Mabinogi.SQL.Columns.Character.Condition));
		}
	}
}
