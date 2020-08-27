using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class CDBAPI
    {
        public static void DeleteCharacterMeta(SimpleConnection conn, SimpleTransaction _tran, character_meta_row row)
        {
            // PROCEDURE: DeleteCharacterMeta
            using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterMeta, _tran))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharacterMeta.CharId, row.charID);
                cmd.Where(Mabinogi.SQL.Columns.CharacterMeta.MCode, row.mcode);
                cmd.Execute();
            }
        }

        public static void UpdateCharacterMeta(SimpleConnection conn, SimpleTransaction _tran, character_meta_row row)
        {
            int rows = 0;

            // PROCEDURE: UpdateCharacterMeta

            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterMeta, _tran))
            {
                cmd.Where(Mabinogi.SQL.Columns.CharacterMeta.CharId, row.charID);
                cmd.Where(Mabinogi.SQL.Columns.CharacterMeta.MCode, row.mcode);

                cmd.Set(Mabinogi.SQL.Columns.CharacterMeta.MData, row.mdata);
                rows = cmd.Execute();
            }

            if (rows == 0)
            {
                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterMeta, _tran))
                {
                    cmd.Set(Mabinogi.SQL.Columns.CharacterMeta.CharId, row.charID);
                    cmd.Set(Mabinogi.SQL.Columns.CharacterMeta.MCode, row.mcode);
                    cmd.Set(Mabinogi.SQL.Columns.CharacterMeta.MType, row.mtype);
                    cmd.Set(Mabinogi.SQL.Columns.CharacterMeta.MData, row.mdata);
                    cmd.Execute();
                }
            }
        }

        public static Dictionary<string, character_meta_row> GetCharacterMeta(SimpleConnection con, long characterId)
        {
            var result = new Dictionary<string, character_meta_row>();

            SimpleCommand cmd = con.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterMeta);
            cmd.Where(Mabinogi.SQL.Columns.CharacterMeta.CharId, characterId);

            character_meta_row character_meta_row;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    character_meta_row = new character_meta_row();
                    character_meta_row.charID = characterId;
                    character_meta_row.mcode = reader.GetString(Mabinogi.SQL.Columns.CharacterMeta.MCode);
                    character_meta_row.mtype = reader.GetString(Mabinogi.SQL.Columns.CharacterMeta.MType);
                    character_meta_row.mdata = reader.GetString(Mabinogi.SQL.Columns.CharacterMeta.MData);
                    result.Add(character_meta_row.mcode, character_meta_row);
                }
            }
            return result;
        }
    }
}
