using Mabinogi.SQL;

namespace XMLDB3
{
	public class FarmUpdateBuilder
	{
		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.farm != null && _old.farm != null)
			{
				if (_new.farm.farmID != _old.farm.farmID)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.FarmId, _new.farm.farmID);
                    result = true;
                }
			}
			return result;
		}
	}
}
