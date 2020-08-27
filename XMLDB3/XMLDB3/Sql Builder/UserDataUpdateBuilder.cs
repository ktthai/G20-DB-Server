using Mabinogi.SQL;

namespace XMLDB3
{
	public class UserDataUpdateBuilder
	{
		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.data != null && _old.data != null)
			{
				if (_new.data.nao_favor != _old.data.nao_favor)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.NaoFavor, _new.data.nao_favor);
                    result = true;
                }
				if (_new.data.nao_memory != _old.data.nao_memory)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.NaoMemory, _new.data.nao_memory);
                    result = true;
                }
				if (_new.data.nao_style != _old.data.nao_style)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.NaoStyle, _new.data.nao_style);
                    result = true;
                }
				if (_new.data.playtime != _old.data.playtime)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Playtime, _new.data.playtime);
                    result = true;
                }
				if (_new.data.birthday != _old.data.birthday)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Birthday, _new.data.birthday);
                    result = true;
                }
				if (_new.data.rebirthday != _old.data.rebirthday)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.RebirthDay, _new.data.rebirthday);
                    result = true;
                }
				if (_new.data.rebirthage != _old.data.rebirthage)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.RebirthAge, _new.data.rebirthage);
                    result = true;
                }
				if (_new.data.wealth != _old.data.wealth)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Wealth, _new.data.wealth);
                    result = true;
                }
				if (_new.data.writeCounter != _old.data.writeCounter)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.WriteCounter, _new.data.writeCounter);
                    result = true;
                }
			}
			return result;
		}
	}
}
