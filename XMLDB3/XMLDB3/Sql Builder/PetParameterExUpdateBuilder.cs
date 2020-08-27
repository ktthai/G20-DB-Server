using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetParameterExUpdateBuilder
	{
		public static void Build(Pet _new, Pet _old, ref SimpleCommand cmd)
		{
			if (_new.parameterEx != null && _old.parameterEx != null)
			{
				if (_new.parameterEx.str_boost != _old.parameterEx.str_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.StrengthBoost, _new.parameterEx.str_boost);
				}
				if (_new.parameterEx.dex_boost != _old.parameterEx.dex_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.DexterityBoost, _new.parameterEx.dex_boost);
				}
				if (_new.parameterEx.int_boost != _old.parameterEx.int_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.IntelligenceBoost, _new.parameterEx.int_boost);
				}
				if (_new.parameterEx.will_boost != _old.parameterEx.will_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.WillBoost, _new.parameterEx.will_boost);
				}
				if (_new.parameterEx.luck_boost != _old.parameterEx.luck_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.LuckBoost, _new.parameterEx.luck_boost);
				}
				if (_new.parameterEx.height_boost != _old.parameterEx.height_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.HeightBoost, _new.parameterEx.height_boost);
				}
				if (_new.parameterEx.fatness_boost != _old.parameterEx.fatness_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.FatnessBoost, _new.parameterEx.fatness_boost);
				}
				if (_new.parameterEx.upper_boost != _old.parameterEx.upper_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.UpperBoost, _new.parameterEx.upper_boost);
				}
				if (_new.parameterEx.lower_boost != _old.parameterEx.lower_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.LowerBoost, _new.parameterEx.lower_boost);
				}
				if (_new.parameterEx.life_boost != _old.parameterEx.life_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.LifeBoost, _new.parameterEx.life_boost);
				}
				if (_new.parameterEx.mana_boost != _old.parameterEx.mana_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.ManaBoost, _new.parameterEx.mana_boost);
				}
				if (_new.parameterEx.stamina_boost != _old.parameterEx.stamina_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.StaminaBoost, _new.parameterEx.stamina_boost);
				}
				if (_new.parameterEx.toxic != _old.parameterEx.toxic)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Toxic, _new.parameterEx.toxic);
				}
				if (_new.parameterEx.toxic_drunken_time != _old.parameterEx.toxic_drunken_time)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.ToxicDrunkenTime, _new.parameterEx.toxic_drunken_time);
				}
				if (_new.parameterEx.toxic_str != _old.parameterEx.toxic_str)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.ToxicStrength, _new.parameterEx.toxic_str);
				}
				if (_new.parameterEx.toxic_int != _old.parameterEx.toxic_int)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.ToxicIntelligence, _new.parameterEx.toxic_int);
				}
				if (_new.parameterEx.toxic_dex != _old.parameterEx.toxic_dex)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.ToxicDexterity, _new.parameterEx.toxic_dex);
				}
				if (_new.parameterEx.toxic_will != _old.parameterEx.toxic_will)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.ToxicWill, _new.parameterEx.toxic_will);
				}
				if (_new.parameterEx.toxic_luck != _old.parameterEx.toxic_luck)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.ToxicLuck, _new.parameterEx.toxic_luck) ;
				}
				if (_new.parameterEx.lasttown != _old.parameterEx.lasttown)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.LastTown, _new.parameterEx.lasttown);
				}
				if (_new.parameterEx.lastdungeon != _old.parameterEx.lastdungeon)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.LastDungeon, _new.parameterEx.lastdungeon);
				}
			}
		}
	}
}
