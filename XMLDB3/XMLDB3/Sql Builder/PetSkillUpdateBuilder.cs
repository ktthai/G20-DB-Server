using System.Collections;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class PetSkillUpdateBuilder
    {
        private static void BuildSkill(PetSkill _new, PetSkill _old, long _idPet, SimpleConnection conn, SimpleTransaction transaction)
        {
            if (_new != null && _old != null && _new.id == _old.id)
            {
                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.PetSkill, transaction))
                {
                    bool doUpdate = false;
                    if (_new.level != _old.level)
                    {
                        cmd.Set(Mabinogi.SQL.Columns.PetSkill.Level, _new.level);
                        doUpdate = true;
                    }

                    if (_new.flag != _old.flag)
                    {
                        cmd.Set(Mabinogi.SQL.Columns.PetSkill.Flag, _new.flag);
                        doUpdate = true;
                    }

                    if (doUpdate)
                    {
                        cmd.Where(Mabinogi.SQL.Columns.PetSkill.Id, _idPet);
                        cmd.Where(Mabinogi.SQL.Columns.PetSkill.Skill, _new.id);

                        cmd.Execute();
                    }
                }
            }
            if (_new != null && _old == null)
            {
                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.PetSkill, transaction))
                {
                    cmd.Set(Mabinogi.SQL.Columns.PetSkill.Flag, _new.flag);
                    cmd.Set(Mabinogi.SQL.Columns.PetSkill.Id, _idPet);
                    cmd.Set(Mabinogi.SQL.Columns.PetSkill.Skill, _new.id);
                    cmd.Set(Mabinogi.SQL.Columns.PetSkill.Level, _new.level);

                    cmd.Execute();
                }
            }
        }

        public static void Build(Pet _new, Pet _old, SimpleConnection conn, SimpleTransaction transaction)
        {
            Hashtable hashtable = new Hashtable();
            Hashtable hashtable2 = new Hashtable();
            if (_new.skills != null)
            {
                foreach (PetSkill petSkill in _new.skills)
                {
                    hashtable.Add(petSkill.id, petSkill);
                }
            }

            if (_old.skills != null)
            {
                foreach (PetSkill petSkill2 in _old.skills)
                {
                    hashtable2.Add(petSkill2.id, petSkill2);
                }
            }
            foreach (PetSkill value in hashtable2.Values)
            {
                if (!hashtable.Contains(value.id))
                {
                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.PetSkill, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.PetSkill.Id, _new.id);
                        cmd.Set(Mabinogi.SQL.Columns.PetSkill.Skill, value.id);

                        cmd.Execute();
                    }
                }
            }

            PetSkill old;
            foreach (PetSkill value2 in hashtable.Values)
            {
                old = null;
                if (hashtable2.Contains(value2.id))
                {
                    old = (PetSkill)hashtable2[value2.id];
                }
                BuildSkill(value2, old, _new.id, conn, transaction);
            }
        }
    }
}
