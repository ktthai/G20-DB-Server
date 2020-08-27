using Mabinogi.SQL;

namespace XMLDB3
{
	public class ServiceUpdateBuilder
	{
		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.service != null && _old.service != null)
			{
				if (_new.service.nsrespawncount != _old.service.nsrespawncount)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.NsRespawnCount, _new.service.nsrespawncount);
                    result = true;
                }
				if (_new.service.nslastrespawnday != _old.service.nslastrespawnday)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.NsLastRespawnDay, _new.service.nslastrespawnday);
                    result = true;
                }
				if (_new.service.nsgiftreceiveday != _old.service.nsgiftreceiveday)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.NsGiftReceiveDay, _new.service.nsgiftreceiveday);
                    result = true;
                }
				if (_new.service.apgiftreceiveday != _old.service.apgiftreceiveday)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.ApGiftReceiveDay, _new.service.apgiftreceiveday);
                    result = true;
                }
				if (_new.service.nsbombcount != _old.service.nsbombcount)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.NsBombCount, _new.service.nsbombcount);
                    result = true;
                }
				if (_new.service.nsbombday != _old.service.nsbombday)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.NsBombDay, _new.service.nsbombday);
                    result = true;
                }
			}
			return result;
		}
	}
}
