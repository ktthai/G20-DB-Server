using Mabinogi.SQL;

namespace XMLDB3
{
	public class ParameterObjectBuilder
	{
		public static CharacterParameter Build(SimpleReader reader)
		{
			CharacterParameter characterParameter = new CharacterParameter();
			characterParameter.life = reader.GetFloat(Mabinogi.SQL.Columns.Character.Life);
			characterParameter.life_damage = reader.GetFloat(Mabinogi.SQL.Columns.Character.LifeDamage);
			characterParameter.life_max = reader.GetFloat(Mabinogi.SQL.Columns.Character.LifeMax);
			characterParameter.mana = reader.GetFloat(Mabinogi.SQL.Columns.Character.Mana);
			characterParameter.mana_max = reader.GetFloat(Mabinogi.SQL.Columns.Character.ManaMax);
			characterParameter.stamina = reader.GetFloat(Mabinogi.SQL.Columns.Character.Stamina);
			characterParameter.stamina_max = reader.GetFloat(Mabinogi.SQL.Columns.Character.StaminaMax);
			characterParameter.food = reader.GetFloat(Mabinogi.SQL.Columns.Character.Food);
			characterParameter.level = reader.GetInt16(Mabinogi.SQL.Columns.Character.Level);
			characterParameter.cumulatedlevel = reader.GetInt32(Mabinogi.SQL.Columns.Character.CumulatedLevel);
			characterParameter.maxlevel = reader.GetInt16(Mabinogi.SQL.Columns.Character.MaxLevel);
			characterParameter.rebirthcount = reader.GetInt16(Mabinogi.SQL.Columns.Character.RebirthCount);
			characterParameter.lifetimeskill = reader.GetInt16(Mabinogi.SQL.Columns.Character.LifetimeSkill);
			characterParameter.experience = reader.GetInt64(Mabinogi.SQL.Columns.Character.Experience);
			characterParameter.age = reader.GetInt16(Mabinogi.SQL.Columns.Character.Age);
			characterParameter.strength = reader.GetFloat(Mabinogi.SQL.Columns.Character.Strength);
			characterParameter.dexterity = reader.GetFloat(Mabinogi.SQL.Columns.Character.Dexterity);
			characterParameter.intelligence = reader.GetFloat(Mabinogi.SQL.Columns.Character.Intelligence);
			characterParameter.will = reader.GetFloat(Mabinogi.SQL.Columns.Character.Will);
			characterParameter.luck = reader.GetFloat(Mabinogi.SQL.Columns.Character.Luck);
			characterParameter.life_max_by_food = reader.GetFloat(Mabinogi.SQL.Columns.Character.LifeMaxByFood);
			characterParameter.mana_max_by_food = reader.GetFloat(Mabinogi.SQL.Columns.Character.ManaMaxByFood);
			characterParameter.stamina_max_by_food = reader.GetFloat(Mabinogi.SQL.Columns.Character.StaminaMaxByFood);
			characterParameter.strength_by_food = reader.GetFloat(Mabinogi.SQL.Columns.Character.StrengthByFood);
			characterParameter.dexterity_by_food = reader.GetFloat(Mabinogi.SQL.Columns.Character.DexterityByFood);
			characterParameter.intelligence_by_food = reader.GetFloat(Mabinogi.SQL.Columns.Character.IntelligenceByFood);
			characterParameter.will_by_food = reader.GetFloat(Mabinogi.SQL.Columns.Character.WillByFood);
			characterParameter.luck_by_food = reader.GetFloat(Mabinogi.SQL.Columns.Character.LuckByFood);
			characterParameter.ability_remain = reader.GetInt32(Mabinogi.SQL.Columns.Character.AbilityRemain);
			characterParameter.attack_min = reader.GetInt16(Mabinogi.SQL.Columns.Character.AttackMin);
			characterParameter.attack_max = reader.GetInt16(Mabinogi.SQL.Columns.Character.AttackMax);
			characterParameter.wattack_min = reader.GetInt16(Mabinogi.SQL.Columns.Character.WAttackMin);
			characterParameter.wattack_max = reader.GetInt16(Mabinogi.SQL.Columns.Character.WAttackMax);
			characterParameter.critical = reader.GetFloat(Mabinogi.SQL.Columns.Character.Critical);
			characterParameter.protect = reader.GetFloat(Mabinogi.SQL.Columns.Character.Protect);
			characterParameter.defense = reader.GetInt16(Mabinogi.SQL.Columns.Character.Defense);
			characterParameter.rate = reader.GetInt16(Mabinogi.SQL.Columns.Character.Rate);
			characterParameter.rank1 = reader.GetInt16(Mabinogi.SQL.Columns.Character.Rank1);
			characterParameter.rank2 = reader.GetInt16(Mabinogi.SQL.Columns.Character.Rank2);
			characterParameter.score = reader.GetInt64(Mabinogi.SQL.Columns.Character.Score);
			return characterParameter;
		}
	}
}
