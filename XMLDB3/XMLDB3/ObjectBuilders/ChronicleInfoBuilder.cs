using System;
using Mabinogi.SQL;
using System.Collections.Generic;

namespace XMLDB3
{
	public class ChronicleInfoBuilder
	{
		public static Dictionary<int, ChronicleInfo> Build(SimpleReader reader)
		{
			Dictionary<int, ChronicleInfo> result = new Dictionary<int, ChronicleInfo>();
			if (reader == null)
			{
				throw new Exception("탐사연표 이미지 테이블이 없습니다.");
			}
			if (reader.HasRows)
			{
				ChronicleInfo chronicleInfo;
				while (reader.Read())
				{
					chronicleInfo = new ChronicleInfo();
					chronicleInfo.questID = reader.GetInt32(Mabinogi.SQL.Columns.ChronicleInfo.QuestId);
					chronicleInfo.questName = reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.QuestName);
					chronicleInfo.keyword = reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.Keyword);
					chronicleInfo.localtext = reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.LocalText);
					chronicleInfo.sort = reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.Sort);
					chronicleInfo.group = reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.Group);
					chronicleInfo.source = reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.Source);
					chronicleInfo.width = reader.GetInt16(Mabinogi.SQL.Columns.ChronicleInfo.Width);
					chronicleInfo.height = reader.GetInt16(Mabinogi.SQL.Columns.ChronicleInfo.Height);
					result.Add(chronicleInfo.questID, chronicleInfo);
				}
			}
			return result;
		}
	}
}
