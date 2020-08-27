using Mabinogi.SQL;
using System.Text.Json;

namespace XMLDB3
{
	public class PetPrivateUpdateBuilder
	{
		public static void Build(Pet _new, Pet _old, ref SimpleCommand cmd)
		{
			if (_new.@private != null && _old.@private != null)
			{
				string text = JsonSerializer.Serialize(_new.@private.reserveds);
				string b = JsonSerializer.Serialize(_old.@private.reserveds);
				string text2 = JsonSerializer.Serialize(_new.@private.registereds);
				string b2 = JsonSerializer.Serialize(_old.@private.registereds);

				if (text != b)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Reserved, text);
				}
				if (text2 != b2)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Registered, text2);
				}
			}
		}
	}
}
