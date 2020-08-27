using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class DeedObejctBuilder
	{
		public static CharacterDeed Build(SimpleReader reader)
		{
			if (reader.Read())
			{
				CharacterDeed characterDeed = new CharacterDeed();
				List<CharacterDeedSet> arrayList = new List<CharacterDeedSet>();
				for (int i = 0; i < 10; i++)
				{
					CharacterDeedSet characterDeedSet = new CharacterDeedSet();
					characterDeedSet.deedBitFlag = reader.GetInt64(Mabinogi.SQL.Columns.CharacterDeed.Flag[i]);
					characterDeedSet.deedUpdateTime = reader.GetInt32(Mabinogi.SQL.Columns.CharacterDeed.DayCount[i]);
					arrayList.Add(characterDeedSet);
				}
				characterDeed.deedSet = arrayList.ToArray();
				return characterDeed;
			}
			return null;
		}
	}
}
