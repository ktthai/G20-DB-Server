using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetPrivateObjectBuilder
	{
		
		public static PetPrivate Build(SimpleReader reader)
		{
			PetPrivate result = new PetPrivate();
			result.reserveds = JsonSerializer.Deserialize<PetPrivateReserved[]>(reader.GetString(Mabinogi.SQL.Columns.Pet.Reserved));
			result.registereds = JsonSerializer.Deserialize<PetPrivateRegistered[]>(reader.GetString(Mabinogi.SQL.Columns.Pet.Registered));

			return result;
		}
	}
}
