using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class SkillUpdateBuilder
	{
		private static void BuildSkill(CharacterSkill _new, CharacterSkill _old, long _idchar, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_new != null && _old != null && _new.id == _old.id)
			{
				using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterSkill, transaction))
				{
					bool exec = false;
					if (_new.version != _old.version)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.Version, _new.version);
						exec = true;
					}
					if (_new.level != _old.level)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.Level, _new.level);
                        exec = true;
                    }
					if (_new.maxlevel != _old.maxlevel)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.MaxLevel, _new.maxlevel);
                        exec = true;
                    }
					if (_new.experience != _old.experience)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.Experience, _new.experience);
                        exec = true;
                    }
					if (_new.count != _old.count)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.Count, _new.count);
                        exec = true;
                    }
					if (_new.flag != _old.flag)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.Flag, _new.flag);
                        exec = true;
                    }
					if (_new.subflag1 != _old.subflag1)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag1, _new.subflag1);
                        exec = true;
                    }
					if (_new.subflag2 != _old.subflag2)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag2, _new.subflag2);
                        exec = true;
                    }
					if (_new.subflag3 != _old.subflag3)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag3, _new.subflag3);
                        exec = true;
                    }
					if (_new.subflag4 != _old.subflag4)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag4, _new.subflag4);
                        exec = true;
                    }
					if (_new.subflag5 != _old.subflag5)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag5, _new.subflag5);
                        exec = true;
                    }
					if (_new.subflag6 != _old.subflag6)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag6, _new.subflag6);
                        exec = true;
                    }
					if (_new.subflag7 != _old.subflag7)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag7, _new.subflag7);
                        exec = true;
                    }
					if (_new.subflag8 != _old.subflag8)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag8, _new.subflag8);
                        exec = true;
                    }
					if (_new.subflag9 != _old.subflag9)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag9, _new.subflag9);
                        exec = true;
                    }
					if (_new.lastPromotionTime != _old.lastPromotionTime)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.LastPromotionTime, _new.lastPromotionTime);
                        exec = true;
                    }
					if (_new.promotionConditionCount != _old.promotionConditionCount)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.PromotionConditionCount, _new.promotionConditionCount);
                        exec = true;
                    }
					if (_new.promotionExperience != _old.promotionExperience)
					{
						cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.PromotionExperience, _new.promotionExperience);
                        exec = true;
                    }
					
					cmd.Where(Mabinogi.SQL.Columns.CharacterSkill.Skill, _new.id);
					cmd.Where(Mabinogi.SQL.Columns.CharacterSkill.Id, _idchar);

					if (exec)
						cmd.Execute();
				}
			}
			if (_new != null && _old == null)
			{
				using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterSkill, transaction))
				{
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.Skill, _new.id);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.Id, _idchar);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.Version, _new.version);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.Level, _new.level);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.MaxLevel, _new.maxlevel);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.Experience, _new.experience);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.Count, _new.count);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.Flag, _new.flag);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag1, _new.subflag1);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag2, _new.subflag2);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag3, _new.subflag3);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag4, _new.subflag4);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag5, _new.subflag5);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag6, _new.subflag6);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag7, _new.subflag7);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag8, _new.subflag8);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.SubFlag9, _new.subflag9);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.LastPromotionTime, _new.lastPromotionTime);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.PromotionConditionCount, _new.promotionConditionCount);
					cmd.Set(Mabinogi.SQL.Columns.CharacterSkill.PromotionExperience, _new.promotionExperience);

					cmd.Execute();
				}
			}
		}

		public static void Build(Character _new, Character _old, SimpleConnection conn, SimpleTransaction transaction)
		{
			Dictionary<short, CharacterSkill> hashtable = new Dictionary<short, CharacterSkill>();
			Dictionary<short, CharacterSkill> hashtable2 = new Dictionary<short, CharacterSkill>();

			if (_new.skills != null)
			{
				CharacterSkill[] skills = _new.skills;
				foreach (CharacterSkill characterSkill in skills)
				{
					hashtable.Add(characterSkill.id, characterSkill);
				}
			}
			if (_old.skills != null)
			{
				foreach (CharacterSkill characterSkill2 in _old.skills)
				{
					hashtable2.Add(characterSkill2.id, characterSkill2);
				}
			}

			foreach (CharacterSkill value in hashtable2.Values)
			{
				if (!hashtable.ContainsKey(value.id))
				{
					using(var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CharacterSkill, transaction))
					{
						cmd.Where(Mabinogi.SQL.Columns.CharacterSkill.Id, _new.id);
						cmd.Where(Mabinogi.SQL.Columns.CharacterSkill.Skill, value.id);
						cmd.Execute();
					}
				}
			}
			CharacterSkill old;
			foreach (CharacterSkill value2 in hashtable.Values)
			{
				old = null;
				if (hashtable2.ContainsKey(value2.id))
				{
					old = hashtable2[value2.id];
				}
				BuildSkill(value2, old, _new.id, conn, transaction);
			}
		}
	}
}
