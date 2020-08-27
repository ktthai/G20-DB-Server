using Mabinogi.SQL;
using System.Text.Json;

namespace XMLDB3
{
	public class PetMemoryUpdateBuilder
	{
		public static void Build(Pet _new, Pet _old, ref SimpleCommand cmd)
		{
			string text = JsonSerializer.Serialize(_new.memorys);
			string b = JsonSerializer.Serialize(_old.memorys);
			if (text != b)
			{
				cmd.Set(Mabinogi.SQL.Columns.Pet.Memory,  text);
			}
		}
	}
}
