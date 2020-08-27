using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetDataUpdateBuilder
	{
		public static void Build(Pet _new, Pet _old, ref SimpleCommand cmd)
		{
			if (_new.data != null && _old.data != null)
			{
				if (_new.data.ui != _old.data.ui)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.UI, _new.data.ui);
				}
				if (_new.data.meta != _old.data.meta)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Meta, _new.data.meta);
				}
				if (_new.data.birthday != _old.data.birthday)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Birthday, _new.data.birthday);
				}
				if (_new.data.rebirthday != _old.data.rebirthday)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.RebirthDay, _new.data.rebirthday);
				}
				if (_new.data.rebirthage != _old.data.rebirthage)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.RebirthAge, _new.data.rebirthage);
				}
				if (_new.data.playtime != _old.data.playtime)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.PlayTime, _new.data.playtime);
				}
				if (_new.data.wealth != _old.data.wealth)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Wealth, _new.data.wealth);
				}
				if (_new.data.writeCounter != _old.data.writeCounter)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.WriteCounter, _new.data.writeCounter);
				}
			}
		}
	}
}
