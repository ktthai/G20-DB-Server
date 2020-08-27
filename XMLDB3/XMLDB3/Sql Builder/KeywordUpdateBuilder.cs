using System.Collections;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class KeywordUpdateBuilder
	{
		public static void Build(Character _new, Character _old, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_old.keyword != null && _old.keyword.keywordSet != null && _new.keyword != null && _new.keyword.keywordSet != null)
			{
				Hashtable hashtable = new Hashtable(_old.keyword.keywordSet.Length);
				Hashtable hashtable2 = new Hashtable(_new.keyword.keywordSet.Length);

				foreach (CharacterKeywordSet characterKeywordSet in _old.keyword.keywordSet)
				{
					hashtable[characterKeywordSet.keywordId] = characterKeywordSet;
				}

				foreach (CharacterKeywordSet characterKeywordSet in _new.keyword.keywordSet)
				{
					hashtable2[characterKeywordSet.keywordId] = characterKeywordSet;
				}

				CharacterKeywordSet characterKeyword;
				foreach (CharacterKeywordSet characterKeywordSet in _new.keyword.keywordSet)
				{
					characterKeyword = (CharacterKeywordSet)hashtable[characterKeywordSet.keywordId];
					if (characterKeyword == null)
					{
                        // PROCEDURE: InsertCharacterKeyword
                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterKeyword, transaction))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.CharacterKeyword.CharId, _new.id);
                            cmd.Set(Mabinogi.SQL.Columns.CharacterKeyword.KeywordId, characterKeywordSet.keywordId);
                            cmd.Execute();
                        }
                    }
				}

				foreach (CharacterKeywordSet characterKeywordSet in _old.keyword.keywordSet)
				{
					characterKeyword = (CharacterKeywordSet)hashtable2[characterKeywordSet.keywordId];
					if (characterKeyword == null)
					{
						// PROCEDURE: DeleteCharacterKeyword
						using(var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterKeyword, transaction))
						{
							cmd.Where(Mabinogi.SQL.Columns.CharacterKeyword.CharId, _new.id);
							cmd.Where(Mabinogi.SQL.Columns.CharacterKeyword.KeywordId, characterKeywordSet.keywordId);
							cmd.Execute();
						}
					}
				}
				return;
			}
			if (_new.keyword != null && _new.keyword.keywordSet != null)
			{
				foreach (CharacterKeywordSet characterKeywordSet in _new.keyword.keywordSet)
				{
                    // PROCEDURE: InsertCharacterKeyword
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterKeyword, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.CharacterKeyword.CharId, _new.id);
                        cmd.Set(Mabinogi.SQL.Columns.CharacterKeyword.KeywordId, characterKeywordSet.keywordId);
                        cmd.Execute();
                    }
                }
			}
		}
	}
}
