using Mabinogi.SQL;

namespace XMLDB3
{
	public class MarriageUpdateBuilder
	{
		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.marriage != null && _old.marriage != null)
			{
				if (_new.marriage.mateid != _old.marriage.mateid)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.MateId, _new.marriage.mateid);
                    result = true;
                }
				if (_new.marriage.matename != _old.marriage.matename)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.MateName, _new.marriage.matename);
                    result = true;
                }
				if (_new.marriage.marriagetime != _old.marriage.marriagetime)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.MarriageTime, _new.marriage.marriagetime);
                    result = true;
                }
				if (_new.marriage.marriagecount != _old.marriage.marriagecount)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.MarriageCount, _new.marriage.marriagecount);
                    result = true;
                }
			}
			return result;
		}
	}
}
