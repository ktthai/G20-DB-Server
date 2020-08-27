using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetParameterObjectBuilder
	{
		public static PetParameter Build(SimpleReader reader)
		{
			PetParameter petParameter = new PetParameter();
			petParameter.life = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Life);
			petParameter.life_damage = reader.GetFloat(Mabinogi.SQL.Columns.Pet.LifeDamage);
			petParameter.life_max = reader.GetFloat(Mabinogi.SQL.Columns.Pet.LifeMax);
			petParameter.mana = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Mana);
			petParameter.mana_max = reader.GetFloat(Mabinogi.SQL.Columns.Pet.ManaMax);
			petParameter.stamina = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Stamina);
			petParameter.stamina_max = reader.GetFloat(Mabinogi.SQL.Columns.Pet.StaminaMax);
			petParameter.food = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Food);
			petParameter.level = reader.GetInt16(Mabinogi.SQL.Columns.Pet.Level);
			petParameter.cumulatedlevel = reader.GetInt32(Mabinogi.SQL.Columns.Pet.CumulatedLevel);
			petParameter.maxlevel = reader.GetInt16(Mabinogi.SQL.Columns.Pet.MaxLevel);
			petParameter.rebirthcount = reader.GetInt16(Mabinogi.SQL.Columns.Pet.RebirthCount);
			petParameter.experience = reader.GetInt64(Mabinogi.SQL.Columns.Pet.Experience);
			petParameter.age = reader.GetInt16(Mabinogi.SQL.Columns.Pet.Age);
			petParameter.strength = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Strength);
			petParameter.dexterity = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Dexterity);
			petParameter.intelligence = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Intelligence);
			petParameter.will = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Will);
			petParameter.luck = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Luck);
			petParameter.attack_min = reader.GetInt16(Mabinogi.SQL.Columns.Pet.AttackMin);
			petParameter.attack_max = reader.GetInt16(Mabinogi.SQL.Columns.Pet.AttackMax);
			petParameter.wattack_min = reader.GetInt16(Mabinogi.SQL.Columns.Pet.WAttackMin);
			petParameter.wattack_max = reader.GetInt16(Mabinogi.SQL.Columns.Pet.WAttackMax);
			petParameter.critical = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Critical);
			petParameter.protect = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Protect);
			petParameter.defense = reader.GetInt16(Mabinogi.SQL.Columns.Pet.Defense);
			petParameter.rate = reader.GetInt16(Mabinogi.SQL.Columns.Pet.Rate);
			return petParameter;
		}
	}
}
