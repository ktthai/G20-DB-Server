using Mabinogi.SQL;

namespace XMLDB3
{
	public class ParameterUpdateBuilder
	{
		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.parameter != null && _old.parameter != null)
			{
				if (_new.parameter.life != _old.parameter.life)
				{
					if (float.IsNaN(_new.parameter.life))
					{
						_new.parameter.life = -9999f;
					}
					cmd.Set(Mabinogi.SQL.Columns.Character.Life,  _new.parameter.life);
                    result = true;
                }
				if (_new.parameter.life_damage != _old.parameter.life_damage)
				{
					if (float.IsNaN(_new.parameter.life_damage))
					{
						_new.parameter.life_damage = -9999f;
					}
					cmd.Set(Mabinogi.SQL.Columns.Character.LifeDamage, _new.parameter.life_damage);
                    result = true;
                }
				if (_new.parameter.life_max != _old.parameter.life_max)
				{
					if (float.IsNaN(_new.parameter.life_max))
					{
						_new.parameter.life_max = -9999f;
					}
					cmd.Set(Mabinogi.SQL.Columns.Character.LifeMax, _new.parameter.life_max);
                    result = true;
                }
				if (_new.parameter.mana != _old.parameter.mana)
				{
					if (float.IsNaN(_new.parameter.mana))
					{
						_new.parameter.mana = -9999f;
					}
					cmd.Set(Mabinogi.SQL.Columns.Character.Mana, _new.parameter.mana);
                    result = true;
                }
				if (_new.parameter.mana_max != _old.parameter.mana_max)
				{
					if (float.IsNaN(_new.parameter.mana_max))
					{
						_new.parameter.mana_max = -9999f;
					}
					cmd.Set(Mabinogi.SQL.Columns.Character.ManaMax, _new.parameter.mana_max);
                    result = true;
                }
				if (_new.parameter.stamina != _old.parameter.stamina)
				{
					if (float.IsNaN(_new.parameter.stamina))
					{
						_new.parameter.stamina = -9999f;
					}
					cmd.Set(Mabinogi.SQL.Columns.Character.Stamina, _new.parameter.stamina);
                    result = true;
                }
				if (_new.parameter.stamina_max != _old.parameter.stamina_max)
				{
					if (float.IsNaN(_new.parameter.stamina_max))
					{
						_new.parameter.stamina_max = -9999f;
					}
					cmd.Set(Mabinogi.SQL.Columns.Character.StaminaMax, _new.parameter.stamina_max);
                    result = true;
                }
				if (_new.parameter.food != _old.parameter.food)
				{
					if (float.IsNaN(_new.parameter.food))
					{
						_new.parameter.food = -9999f;
					}
					cmd.Set(Mabinogi.SQL.Columns.Character.Food, _new.parameter.food);
                    result = true;
                }
				if (_new.parameter.level != _old.parameter.level)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Level,_new.parameter.level);
                    result = true;
                }
				if (_new.parameter.cumulatedlevel != _old.parameter.cumulatedlevel)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.CumulatedLevel, _new.parameter.cumulatedlevel);
                    result = true;
                }
				if (_new.parameter.maxlevel != _old.parameter.maxlevel)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.MaxLevel, _new.parameter.maxlevel);
                    result = true;
                }
				if (_new.parameter.rebirthcount != _old.parameter.rebirthcount)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.RebirthCount, _new.parameter.rebirthcount);
                    result = true;
                }
				if (_new.parameter.lifetimeskill != _old.parameter.lifetimeskill)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.LifetimeSkill, _new.parameter.lifetimeskill);
                    result = true;
                }
				if (_new.parameter.experience != _old.parameter.experience)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Experience, _new.parameter.experience);
                    result = true;
                }
				if (_new.parameter.age != _old.parameter.age)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Age, _new.parameter.age);
                    result = true;
                }
				if (_new.parameter.strength != _old.parameter.strength)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Strength, _new.parameter.strength);
                    result = true;
                }
				if (_new.parameter.dexterity != _old.parameter.dexterity)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Dexterity, _new.parameter.dexterity);
                    result = true;
                }
				if (_new.parameter.intelligence != _old.parameter.intelligence)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Intelligence, _new.parameter.intelligence);
                    result = true;
                }
				if (_new.parameter.will != _old.parameter.will)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Will, _new.parameter.will);
                    result = true;
                }
				if (_new.parameter.luck != _old.parameter.luck)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Luck, _new.parameter.luck);
                    result = true;
                }
				if (_new.parameter.life_max_by_food != _old.parameter.life_max_by_food)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.LifeMaxByFood,_new.parameter.life_max_by_food);
                    result = true;
                }
				if (_new.parameter.mana_max_by_food != _old.parameter.mana_max_by_food)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.ManaMaxByFood, _new.parameter.mana_max_by_food);
                    result = true;
                }
				if (_new.parameter.stamina_max_by_food != _old.parameter.stamina_max_by_food)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.StaminaMaxByFood, _new.parameter.stamina_max_by_food);
                    result = true;
                }
				if (_new.parameter.strength_by_food != _old.parameter.strength_by_food)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.StrengthByFood, _new.parameter.strength_by_food);
                    result = true;
                }
				if (_new.parameter.dexterity_by_food != _old.parameter.dexterity_by_food)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.DexterityByFood, _new.parameter.dexterity_by_food);
                    result = true;
                }
				if (_new.parameter.intelligence_by_food != _old.parameter.intelligence_by_food)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.IntelligenceByFood, _new.parameter.intelligence_by_food);
                    result = true;
                }
				if (_new.parameter.will_by_food != _old.parameter.will_by_food)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.WillByFood, _new.parameter.will_by_food);
                    result = true;
                }
				if (_new.parameter.luck_by_food != _old.parameter.luck_by_food)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.LuckByFood, _new.parameter.luck_by_food);
                    result = true;
                }
				if (_new.parameter.ability_remain != _old.parameter.ability_remain)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.AbilityRemain, _new.parameter.ability_remain);
                    result = true;
                }
				if (_new.parameter.attack_min != _old.parameter.attack_min)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.AttackMin, _new.parameter.attack_min);
                    result = true;
                }
				if (_new.parameter.attack_max != _old.parameter.attack_max)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.AttackMax, _new.parameter.attack_max);
                    result = true;
                }
				if (_new.parameter.wattack_min != _old.parameter.wattack_min)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.WAttackMin, _new.parameter.wattack_min);
                    result = true;
                }
				if (_new.parameter.wattack_max != _old.parameter.wattack_max)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.WAttackMax, _new.parameter.wattack_max);
                    result = true;
                }
				if (_new.parameter.critical != _old.parameter.critical)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Critical, _new.parameter.critical);
                    result = true;
                }
				if (_new.parameter.protect != _old.parameter.protect)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Protect, _new.parameter.protect);
                    result = true;
                }
				if (_new.parameter.defense != _old.parameter.defense)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Defense, _new.parameter.defense);
                    result = true;
                }
				if (_new.parameter.rate != _old.parameter.rate)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Rate, _new.parameter.rate);
                    result = true;
                }
				if (_new.parameter.rank1 != _old.parameter.rank1)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Rank1, _new.parameter.rank1);
                    result = true;
                }
				if (_new.parameter.rank2 != _old.parameter.rank2)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Rank2, _new.parameter.rank2);
                    result = true;
                }
				if (_new.parameter.score != _old.parameter.score)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Score, _new.parameter.score);
                    result = true;
                }
			}
			return result;
		}
	}
}
