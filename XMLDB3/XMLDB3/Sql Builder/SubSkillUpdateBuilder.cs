using System.Collections;
using Mabinogi.SQL;

namespace XMLDB3
{
    internal class SubSkillUpdateBuilder
    {
        public static void Build(Character _new, Character _old, SimpleConnection conn, SimpleTransaction transaction)
        {
            Hashtable hashtable = new Hashtable();
            Hashtable hashtable2 = new Hashtable();

            if (_new.subSkill != null && _new.subSkill.subSkillSet != null)
            {
                CharacterSubSkill[] subSkillSet = _new.subSkill.subSkillSet;
                foreach (CharacterSubSkill characterSubSkill in subSkillSet)
                {
                    hashtable.Add(characterSubSkill.id, characterSubSkill);
                }
            }

            if (_old.subSkill != null && _old.subSkill.subSkillSet != null)
            {
                CharacterSubSkill[] subSkillSet2 = _old.subSkill.subSkillSet;
                foreach (CharacterSubSkill characterSubSkill2 in subSkillSet2)
                {
                    hashtable2.Add(characterSubSkill2.id, characterSubSkill2);
                }
            }

            foreach (CharacterSubSkill value in hashtable2.Values)
            {
                if (!hashtable.Contains(value.id))
                {
                    // PROCEDURE: DeleteCharacterSubSkill
                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterSubskill, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.CharacterSubskill.Id, _new.id);
                        cmd.Where(Mabinogi.SQL.Columns.CharacterSubskill.Subskill, value.id);
                        cmd.Execute();
                    }
                }
            }

            foreach (CharacterSubSkill value2 in hashtable.Values)
            {
                CharacterSubSkill characterSubSkill5 = null;
                if (hashtable2.Contains(value2.id))
                {
                    characterSubSkill5 = (CharacterSubSkill)hashtable2[value2.id];
                }

                if (characterSubSkill5 == null || !value2.Equals(characterSubSkill5))
                {
                    // PROCEDURE: UpdateCharacterSubSkill
                    using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterSubskill, transaction))
                    {
                        upCmd.Where(Mabinogi.SQL.Columns.CharacterSubskill.Id, _new.id);
                        upCmd.Where(Mabinogi.SQL.Columns.CharacterSubskill.Subskill, value2.id);

                        upCmd.Set(Mabinogi.SQL.Columns.CharacterSubskill.Experience, value2.exp);
                        upCmd.Set(Mabinogi.SQL.Columns.CharacterSubskill.Level, value2.level);

                        if (upCmd.Execute() < 1)
                        {
                            using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterSubskill, transaction))
                            {
                                insCmd.Set(Mabinogi.SQL.Columns.CharacterSubskill.Id, _new.id);
                                insCmd.Set(Mabinogi.SQL.Columns.CharacterSubskill.Subskill, value2.id);

                                insCmd.Set(Mabinogi.SQL.Columns.CharacterSubskill.Experience, value2.exp);
                                insCmd.Set(Mabinogi.SQL.Columns.CharacterSubskill.Level, value2.level);

                                insCmd.Execute();
                            }
                        }
                    }
                }
            }
        }
    }
}
