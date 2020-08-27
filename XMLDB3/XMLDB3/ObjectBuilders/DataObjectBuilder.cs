using System;
using System.Collections;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class DataObjectBuilder
	{
		public static CharacterData BuildData(SimpleReader reader)
		{
			CharacterData characterData = new CharacterData();
			characterData.meta = string.Empty;


			characterData.nao_memory = reader.GetInt16(Mabinogi.SQL.Columns.Character.NaoMemory); 
			characterData.nao_favor = reader.GetInt16(Mabinogi.SQL.Columns.Character.NaoFavor);
			characterData.nao_style = reader.GetByte(Mabinogi.SQL.Columns.Character.NaoStyle);
			characterData.birthday = reader.GetDateTime(Mabinogi.SQL.Columns.Character.Birthday);
			characterData.rebirthday = reader.GetDateTime(Mabinogi.SQL.Columns.Character.RebirthDay);
			characterData.rebirthage = reader.GetInt16(Mabinogi.SQL.Columns.Character.RebirthAge);
			characterData.playtime = reader.GetInt32(Mabinogi.SQL.Columns.Character.Playtime);
			characterData.wealth = reader.GetInt32(Mabinogi.SQL.Columns.Character.Wealth);
			characterData.writeCounter = reader.GetByte(Mabinogi.SQL.Columns.Character.WriteCounter);
			return characterData;
		}

		public static string BuildMeta(SimpleReader reader)
		{
			if (reader.HasRows)
            {
                Hashtable hashtable = new Hashtable();
                while (reader.Read())
                {
                    character_meta_row character_meta_row = new character_meta_row();
					character_meta_row.mcode = reader.GetString(Mabinogi.SQL.Columns.CharacterMeta.MCode);
                    character_meta_row.mtype = reader.GetString(Mabinogi.SQL.Columns.CharacterMeta.MType);
					character_meta_row.mdata = reader.GetString(Mabinogi.SQL.Columns.CharacterMeta.MData);
					hashtable.Add(character_meta_row.mcode, character_meta_row);
                }
				return CMetaHelper.MeteRowListToMetaString(hashtable);
            }
            else
			{ 
                throw new Exception("Meta table is empty.");
            }
        }
	}
}
