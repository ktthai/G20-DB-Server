using System.Collections;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class FavoritePrivateFarmUpdateBuilder
	{
		public static void Build(long _id, Hashtable _new, Hashtable _old, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_new != null)
			{
				if (_old != null)
				{
					CharacterFavoritePrivateFarm characterFavoritePrivateFarm2;
					foreach (CharacterFavoritePrivateFarm value in _new.Values)
					{
						characterFavoritePrivateFarm2 = (CharacterFavoritePrivateFarm)_old[value.privateFarmId];
						FavoritePrivateFarmSqlBuilder.UpdateFavorite(_id, value, characterFavoritePrivateFarm2, conn, transaction);
						if (characterFavoritePrivateFarm2 != null)
						{
							_old.Remove(characterFavoritePrivateFarm2.privateFarmId);
						}
					}
				}
				else
				{
					foreach (CharacterFavoritePrivateFarm value2 in _new.Values)
					{
						FavoritePrivateFarmSqlBuilder.UpdateFavorite(_id, value2, null, conn, transaction);
					}
				}
			}

			if (_old != null)
			{
				foreach (CharacterFavoritePrivateFarm value3 in _old.Values)
				{
					FavoritePrivateFarmSqlBuilder.DeleteFavorite(_id, value3.privateFarmId, conn, transaction);
				}
			}
		}

		public static void Build(Character _new, Character _old, SimpleConnection conn, SimpleTransaction transaction)
		{
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();

			if (_new.prifateFarm != null && _new.prifateFarm.favoriteFarm != null)
			{
				CharacterFavoritePrivateFarm[] favoriteFarm = _new.prifateFarm.favoriteFarm;
				foreach (CharacterFavoritePrivateFarm characterFavoritePrivateFarm in favoriteFarm)
				{
					hashtable.Add(characterFavoritePrivateFarm.privateFarmId, characterFavoritePrivateFarm);
				}
			}
			if (_old.prifateFarm != null && _old.prifateFarm.favoriteFarm != null)
			{
				CharacterFavoritePrivateFarm[] favoriteFarm2 = _old.prifateFarm.favoriteFarm;
				foreach (CharacterFavoritePrivateFarm characterFavoritePrivateFarm2 in favoriteFarm2)
				{
					hashtable2.Add(characterFavoritePrivateFarm2.privateFarmId, characterFavoritePrivateFarm2);
				}
			}
			Build(_new.id, hashtable, hashtable2, conn, transaction);
		}
	}
}
