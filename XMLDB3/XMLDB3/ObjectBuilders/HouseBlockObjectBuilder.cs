using Mabinogi.SQL;
using System.Collections.Generic;

namespace XMLDB3
{
	public class HouseBlockObjectBuilder
	{
		public static HouseBlockList Build(SimpleReader blockReader)
		{
			if (blockReader == null)
			{
				return null;
			}
			HouseBlockList houseBlockList = new HouseBlockList();
			if (blockReader.HasRows)
			{
				houseBlockList.Blocks = new List<HouseBlock>();
				HouseBlock block;
				while(blockReader.Read())
				{
					block = new HouseBlock();
					block.gameName = blockReader.GetString(Mabinogi.SQL.Columns.HouseBlock.GameName);
					block.flag = blockReader.GetByte(Mabinogi.SQL.Columns.HouseBlock.Flag);
				}
				return houseBlockList;
			}
			return houseBlockList;
		}
	}
}
