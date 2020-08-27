using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetSummonObjectBuilder
	{
		public static PetSummon Build(SimpleReader reader)
		{
			PetSummon petSummon = new PetSummon();
			petSummon.loyalty = reader.GetByte(Mabinogi.SQL.Columns.Pet.Loyalty);
			petSummon.favor = reader.GetByte(Mabinogi.SQL.Columns.Pet.Favor);
			return petSummon;
		}
	}
}
