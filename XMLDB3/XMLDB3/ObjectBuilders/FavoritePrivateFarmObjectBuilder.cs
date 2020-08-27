using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class FavoritePrivateFarmObjectBuilder
	{
		public static CharacterPrivateFarm Build(SimpleReader reader)
		{
			CharacterPrivateFarm characterPrivateFarm = new CharacterPrivateFarm();
			if (reader.HasRows)
			{
				List<CharacterFavoritePrivateFarm> arrayList = new List<CharacterFavoritePrivateFarm>();
				while(reader.Read())
				{
					CharacterFavoritePrivateFarm characterFavoritePrivateFarm = new CharacterFavoritePrivateFarm();
					characterFavoritePrivateFarm.privateFarmId = reader.GetInt64(Mabinogi.SQL.Columns.FavoritePrivateFarm.PrivateFarmId);
					characterFavoritePrivateFarm.themeId = reader.GetInt32(Mabinogi.SQL.Columns.FavoritePrivateFarm.ThemeId);
					characterFavoritePrivateFarm.posX = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.FavoritePrivateFarm.WorldPosX);
					characterFavoritePrivateFarm.posY = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.FavoritePrivateFarm.WorldPosY);
					characterFavoritePrivateFarm.farmName = reader.GetString(Mabinogi.SQL.Columns.FavoritePrivateFarm.FarmName);
					characterFavoritePrivateFarm.ownerName = reader.GetString(Mabinogi.SQL.Columns.FavoritePrivateFarm.OwnerName);
					arrayList.Add(characterFavoritePrivateFarm);
				}
				characterPrivateFarm.favoriteFarm = arrayList.ToArray();
			}
			return characterPrivateFarm;
		}
	}
}
