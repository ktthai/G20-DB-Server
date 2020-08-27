using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class KeywordObjectBuilder
	{
		public static CharacterKeyword Build(SimpleReader reader)
		{
			CharacterKeyword characterKeyword = new CharacterKeyword();
			if (reader.HasRows)
			{
				List< CharacterKeywordSet> arrayList = new List<CharacterKeywordSet>();
				CharacterKeywordSet characterKeywordSet;
				while (reader.Read())
				{
					characterKeywordSet = new CharacterKeywordSet();
					characterKeywordSet.keywordId = (short)reader.GetInt32(Mabinogi.SQL.Columns.CharacterKeyword.KeywordId); 
					arrayList.Add(characterKeywordSet);
				}
				characterKeyword.keywordSet = arrayList.ToArray();
			}
			return characterKeyword;
		}
	}
}
