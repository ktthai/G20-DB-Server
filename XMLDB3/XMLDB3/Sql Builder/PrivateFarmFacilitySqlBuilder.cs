using Mabinogi.SQL;

namespace XMLDB3
{
	public sealed class PrivateFarmFacilitySqlBuilder
	{
		private static bool CheckUpdate(PrivateFarmFacility _new, PrivateFarmFacility _old)
		{
			if (_new == null)
			{
				return false;
			}
			if (_old == null)
			{
				return true;
			}
			if (_new.classId != _old.classId)
			{
				return true;
			}
			if (_new.dir != _old.dir)
			{
				return true;
			}
			if (_new.customName != _old.customName)
			{
				return true;
			}
			if (_new.finishTime != _old.finishTime)
			{
				return true;
			}
			if (_new.lastProcessingTime != _old.lastProcessingTime)
			{
				return true;
			}
			if (_new.linkedFacilityId != _old.linkedFacilityId)
			{
				return true;
			}
			if (_new.permissionFlag != _old.permissionFlag)
			{
				return true;
			}
			if (_new.meta != _old.meta)
			{
				return true;
			}
			if (_new.x != _old.x)
			{
				return true;
			}
			if (_new.y != _old.y)
			{
				return true;
			}
			for (int i = 0; i < 9; i++)
			{
				if (_new.color[i] != _old.color[i])
				{
					return true;
				}
			}
			return false;
		}

		private static void BuildParameters(long _idPrivateFarm, PrivateFarmFacility _new, SimpleCommand cmd)
		{
			cmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacility.PrivateFarmId, _idPrivateFarm);
			cmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacility.ClassId, _new.classId);
			cmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacility.X, _new.x);
			cmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacility.Y, _new.y);
			cmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacility.Dir, _new.dir);
            for (int i = 0; i < 9; i++)
            {
                cmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacility.Color[i], _new.color[i]);
            }
			cmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacility.FinishTime, _new.finishTime);
			cmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacility.LastProcessingTime, _new.lastProcessingTime);
			cmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacility.LinkedFacilityId, _new.linkedFacilityId);
			cmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacility.PermissionFlag, _new.permissionFlag);
			cmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacility.Meta,_new.meta);
			cmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacility.CustomName, _new.customName);
		}

		public static void UpdateFacility(long _idPrivateFarm, PrivateFarmFacility _new, PrivateFarmFacility _old, SimpleConnection conn, SimpleTransaction transaction)
        {
			if (CheckUpdate(_new, _old))
			{
				//PROCEDURE: UpdatePrivateFarmFacility
				using(var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmFacility, transaction))
				{
					cmd.Where(Mabinogi.SQL.Columns.PrivateFarmFacility.ClassId, _new.facilityId);

					BuildParameters(_idPrivateFarm, _new, cmd);

					if(cmd.Execute() < 1)
                    {
						using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmFacility, transaction))
						{
							insCmd.Where(Mabinogi.SQL.Columns.PrivateFarmFacility.ClassId, _new.facilityId);

							BuildParameters(_idPrivateFarm, _new, insCmd);
							insCmd.Execute();
						}
                    }

                }
			}
		}

		public static void DeleteFacility(long _idFacility, SimpleConnection conn, SimpleTransaction transaction)
		{
			// PROCEDURE: DestroyPrivateFarmFacility
			using(var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmFacility, transaction))
			{
				cmd.Where(Mabinogi.SQL.Columns.PrivateFarmFacility.FacilityId, _idFacility);
				cmd.Execute();
			}
		}
	}
}
