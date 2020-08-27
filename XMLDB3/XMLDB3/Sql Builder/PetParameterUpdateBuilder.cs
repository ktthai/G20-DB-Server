using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetParameterUpdateBuilder
	{
		public static void Build(Pet _new, Pet _old, ref SimpleCommand cmd)
		{
			if (_new.parameter != null && _old.parameter != null)
			{
				if (_new.parameter.life != _old.parameter.life)
				{
					if (float.IsNaN(_new.parameter.life))
					{
						_new.parameter.life = -9999f;
					}
					cmd.Set(Mabinogi.SQL.Columns.Pet.Life, _new.parameter.life);
				}
				if (_new.parameter.life_damage != _old.parameter.life_damage)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.LifeDamage, _new.parameter.life_damage);
				}
				if (_new.parameter.life_max != _old.parameter.life_max)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.LifeMax, _new.parameter.life_max);
				}
				if (_new.parameter.mana != _old.parameter.mana)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Mana, _new.parameter.mana);
				}
				if (_new.parameter.mana_max != _old.parameter.mana_max)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.ManaMax, _new.parameter.mana_max);
				}
				if (_new.parameter.stamina != _old.parameter.stamina)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Stamina, _new.parameter.stamina);
				}
				if (_new.parameter.stamina_max != _old.parameter.stamina_max)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.StaminaMax, _new.parameter.stamina_max);
				}
				if (_new.parameter.food != _old.parameter.food)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Food, _new.parameter.food);
				}
				if (_new.parameter.level != _old.parameter.level)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Level, _new.parameter.level);
				}
				if (_new.parameter.cumulatedlevel != _old.parameter.cumulatedlevel)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.CumulatedLevel, _new.parameter.cumulatedlevel);
				}
				if (_new.parameter.maxlevel != _old.parameter.maxlevel)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.MaxLevel, _new.parameter.maxlevel);
				}
				if (_new.parameter.rebirthcount != _old.parameter.rebirthcount)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.RebirthCount, _new.parameter.rebirthcount);
				}
				if (_new.parameter.experience != _old.parameter.experience)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Experience, _new.parameter.experience);
				}
				if (_new.parameter.age != _old.parameter.age)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Age, _new.parameter.age);
				}
				if (_new.parameter.strength != _old.parameter.strength)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Strength, _new.parameter.strength);
				}
				if (_new.parameter.dexterity != _old.parameter.dexterity)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Dexterity, _new.parameter.dexterity);
				}
				if (_new.parameter.intelligence != _old.parameter.intelligence)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Intelligence, _new.parameter.intelligence);
				}
				if (_new.parameter.will != _old.parameter.will)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Will, _new.parameter.will);
				}
				if (_new.parameter.luck != _old.parameter.luck)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Luck, _new.parameter.luck);
				}
				if (_new.parameter.attack_min != _old.parameter.attack_min)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.AttackMin, _new.parameter.attack_min);
				}
				if (_new.parameter.attack_max != _old.parameter.attack_max)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.AttackMax, _new.parameter.attack_max);
				}
				if (_new.parameter.wattack_min != _old.parameter.wattack_min)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.WAttackMin, _new.parameter.wattack_min);
				}
				if (_new.parameter.wattack_max != _old.parameter.wattack_max)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.WAttackMax, _new.parameter.wattack_max);
				}
				if (_new.parameter.critical != _old.parameter.critical)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Critical, _new.parameter.critical);
				}
				if (_new.parameter.protect != _old.parameter.protect)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Protect, _new.parameter.protect);
				}
				if (_new.parameter.defense != _old.parameter.defense)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Defense, _new.parameter.defense);
				}
				if (_new.parameter.rate != _old.parameter.rate)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Rate, _new.parameter.rate);
				}
			}
		}
	}
}
