using Mabinogi.SQL;
using System;
using System.Text;

namespace XMLDB3
{
	public class PrivateFarmUpdateBuilder
	{
		public static void Build(PrivateFarm _new, PrivateFarm _old, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_new == null)
			{
				throw new ArgumentException("개인농장 데이터가 없습니다.", "_new");
			}
			if (_old == null)
			{
				throw new ArgumentException("개인농장 캐쉬 데이터가 없습니다.", "_old");
			}
			// PROCEDURE: UpdatePrivateFarm

			using(var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarm, transaction))
			{
				cmd.Where(Mabinogi.SQL.Columns.PrivateFarm.Id, _new.id);

                cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.Level,_new.level);
                cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.ClassId,_new.classId);
                cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.Exp,_new.exp);
                cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.Name,_new.name);
                cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.WorldPosX,_new.worldPosX);
                cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.WorldPosY,_new.worldPosY);
                cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.DeleteFlag,_new.deleteFlag);
                cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.BindedChannel,_new.bindedChannel);
                cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.NextBindableTime,_new.nextBindableTime);

				cmd.Execute();
            }
		}
	}
}
