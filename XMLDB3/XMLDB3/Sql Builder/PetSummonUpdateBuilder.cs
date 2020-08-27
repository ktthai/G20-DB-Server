using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetSummonUpdateBuilder
	{
		public static void Build(Pet _new, Pet _old, ref SimpleCommand cmd)
		{
			if (_new.data != null && _old.data != null)
			{
				if (_new.summon.loyalty != _old.summon.loyalty)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Loyalty,  _new.summon.loyalty);
				}
				if (_new.summon.favor != _old.summon.favor)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Favor, _new.summon.favor);
				}
			}
		}
	}
}
