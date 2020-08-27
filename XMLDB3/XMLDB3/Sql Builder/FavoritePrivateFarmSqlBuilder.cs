using Mabinogi.SQL;

namespace XMLDB3
{
	public sealed class FavoritePrivateFarmSqlBuilder
	{
		private static bool CheckUpdate(CharacterFavoritePrivateFarm _new, CharacterFavoritePrivateFarm _old)
		{
			if (_new == null)
			{
				return false;
			}
			if (_old == null)
			{
				return true;
			}
			if (_new.posX != _old.posX)
			{
				return true;
			}
			if (_new.posY != _old.posY)
			{
				return true;
			}
			if (_new.ownerName != _old.ownerName)
			{
				return true;
			}
			if (_new.farmName != _old.farmName)
			{
				return true;
			}
			if (_new.themeId != _old.themeId)
			{
				return true;
			}
			return false;
		}

		public static void UpdateFavorite(long _idChar, CharacterFavoritePrivateFarm _new, CharacterFavoritePrivateFarm _old, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (CheckUpdate(_new, _old))
			{
				// PROCEDURE: UpdateFavoritePrivateFarm
				using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.FavoritePrivateFarm, transaction))
				{
					upCmd.Where(Mabinogi.SQL.Columns.FavoritePrivateFarm.CharId, _idChar);
					upCmd.Where(Mabinogi.SQL.Columns.FavoritePrivateFarm.PrivateFarmId, _new.privateFarmId);

					upCmd.Set(Mabinogi.SQL.Columns.FavoritePrivateFarm.WorldPosX, _new.posX);
					upCmd.Set(Mabinogi.SQL.Columns.FavoritePrivateFarm.WorldPosY, _new.posY);
					upCmd.Set(Mabinogi.SQL.Columns.FavoritePrivateFarm.ThemeId, _new.themeId);

					if(upCmd.Execute() < 1)
					{
						using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.FavoritePrivateFarm, transaction))
						{
							insCmd.Set(Mabinogi.SQL.Columns.FavoritePrivateFarm.CharId, _idChar);
							insCmd.Set(Mabinogi.SQL.Columns.FavoritePrivateFarm.PrivateFarmId, _new.privateFarmId);
							insCmd.Set(Mabinogi.SQL.Columns.FavoritePrivateFarm.FarmName, _new.farmName);
							insCmd.Set(Mabinogi.SQL.Columns.FavoritePrivateFarm.OwnerName, _new.ownerName);
							insCmd.Set(Mabinogi.SQL.Columns.FavoritePrivateFarm.WorldPosX, _new.posX);
							insCmd.Set(Mabinogi.SQL.Columns.FavoritePrivateFarm.WorldPosY, _new.posY);
							insCmd.Set(Mabinogi.SQL.Columns.FavoritePrivateFarm.ThemeId, _new.themeId);

							insCmd.Execute();
						}
                    }
				}
			}
		}

		public static void DeleteFavorite(long _idChar, long _idPrivateFarm, SimpleConnection conn, SimpleTransaction transaction)
		{
			// PROCEDURE: DeleteFavoritePrivateFarm
			using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.FavoritePrivateFarm, transaction))
			{
                cmd.Where(Mabinogi.SQL.Columns.FavoritePrivateFarm.CharId, _idChar);
                cmd.Where(Mabinogi.SQL.Columns.FavoritePrivateFarm.PrivateFarmId, _idPrivateFarm);

                cmd.Execute();
			}
		}
	}
}
