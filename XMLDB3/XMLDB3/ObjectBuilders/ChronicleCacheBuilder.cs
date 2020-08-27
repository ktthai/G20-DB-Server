using System;
using Mabinogi.SQL;
using System.Collections.Generic;

namespace XMLDB3
{
	public class ChronicleCacheBuilder
	{
		public static Dictionary<int, int> Build(SimpleReader reader, ChronicleInfo[] _infos)
		{
			Dictionary<int, int> result = new Dictionary<int, int>();

			if (reader == null)
			{
				throw new Exception("탐사연표 랭킹 테이블이 없습니다.");
			}
			if (reader.HasRows == true)
			{
				int num;
				while (reader.Read())
				{
					num = reader.GetInt32(Mabinogi.SQL.Columns.ChronicleInfo.QuestId);
					result[num] = reader.GetInt32(Mabinogi.SQL.Columns.Reference.TotalCount);
				}
			}
			if (_infos != null && _infos.Length > 0)
			{
				foreach (ChronicleInfo chronicleInfo in _infos)
				{
					if (chronicleInfo.IsRankingChronicle && !result.ContainsKey(chronicleInfo.questID))
					{
						result[chronicleInfo.questID] = 0;
					}
				}
			}
			return result;
		}
	}
}
