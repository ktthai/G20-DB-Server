using Mabinogi.SQL;
using System.Text.Json;

namespace XMLDB3
{
	public class PetConditionUpdateBuilder
	{
		public static void Build(Pet _new, Pet _old, ref SimpleCommand cmd)
		{
			string text = JsonSerializer.Serialize(_new.conditions);
			string b = JsonSerializer.Serialize(_old.conditions);
			if (text != b)
			{
				cmd.Set(Mabinogi.SQL.Columns.Pet.Condition, text);
			}
		}
	}
}
