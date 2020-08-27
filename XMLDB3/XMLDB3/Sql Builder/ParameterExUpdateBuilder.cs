using Mabinogi.SQL;

namespace XMLDB3
{
	public class ParameterExUpdateBuilder
	{
		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.parameterEx != null && _old.parameterEx != null)
			{
				if (_new.parameterEx.str_boost != _old.parameterEx.str_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.StrengthBoost, _new.parameterEx.str_boost);
					result = true;
				}
				if (_new.parameterEx.dex_boost != _old.parameterEx.dex_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.DexterityBoost, _new.parameterEx.dex_boost);
                    result = true;
                }
				if (_new.parameterEx.int_boost != _old.parameterEx.int_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.IntelligenceBoost, _new.parameterEx.int_boost);
                    result = true;
                }
				if (_new.parameterEx.will_boost != _old.parameterEx.will_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.WillBoost, _new.parameterEx.will_boost);
                    result = true;
                }
				if (_new.parameterEx.luck_boost != _old.parameterEx.luck_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.LuckBoost, _new.parameterEx.luck_boost);
                    result = true;
                }
				if (_new.parameterEx.height_boost != _old.parameterEx.height_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.HeightBoost, _new.parameterEx.height_boost);
                    result = true;
                }
				if (_new.parameterEx.fatness_boost != _old.parameterEx.fatness_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.FatnessBoost, _new.parameterEx.fatness_boost);
                    result = true;
                }
				if (_new.parameterEx.upper_boost != _old.parameterEx.upper_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.UpperBoost, _new.parameterEx.upper_boost);
                    result = true;
                }
				if (_new.parameterEx.lower_boost != _old.parameterEx.lower_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.LowerBoost, _new.parameterEx.lower_boost);
                    result = true;
                }
				if (_new.parameterEx.life_boost != _old.parameterEx.life_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.LifeBoost, _new.parameterEx.life_boost);
                    result = true;
                }
				if (_new.parameterEx.mana_boost != _old.parameterEx.mana_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.ManaBoost, _new.parameterEx.mana_boost);
                    result = true;
                }
				if (_new.parameterEx.stamina_boost != _old.parameterEx.stamina_boost)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.StaminaBoost, _new.parameterEx.stamina_boost);
                    result = true;
                }
				if (_new.parameterEx.toxic != _old.parameterEx.toxic)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Toxic, _new.parameterEx.toxic);
                    result = true;
                }
				if (_new.parameterEx.toxic_drunken_time != _old.parameterEx.toxic_drunken_time)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.ToxicDrunkenTime, _new.parameterEx.toxic_drunken_time);
                    result = true;
                }
				if (_new.parameterEx.toxic_str != _old.parameterEx.toxic_str)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.ToxicStrength, _new.parameterEx.toxic_str);
                    result = true;
                }
				if (_new.parameterEx.toxic_int != _old.parameterEx.toxic_int)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.ToxicIntelligence, _new.parameterEx.toxic_int);
                    result = true;
                }
				if (_new.parameterEx.toxic_dex != _old.parameterEx.toxic_dex)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.ToxicDexterity, _new.parameterEx.toxic_dex);
                    result = true;
                }
				if (_new.parameterEx.toxic_will != _old.parameterEx.toxic_will)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.ToxicWill, _new.parameterEx.toxic_will);
                    result = true;
                }
				if (_new.parameterEx.toxic_luck != _old.parameterEx.toxic_luck)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.ToxicLuck, _new.parameterEx.toxic_luck);
                    result = true;
                }
				if (_new.parameterEx.lasttown != _old.parameterEx.lasttown)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.LastTown, _new.parameterEx.lasttown);
                    result = true;
                }
				if (_new.parameterEx.lastdungeon != _old.parameterEx.lastdungeon)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.LastDungeon, _new.parameterEx.lastdungeon);
                    result = true;
                }
				if (_new.parameterEx.exploLevel != _old.parameterEx.exploLevel)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.ExploLevel, _new.parameterEx.exploLevel);
                    result = true;
                }
				if (_new.parameterEx.exploMaxKeyLevel != _old.parameterEx.exploMaxKeyLevel)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.ExploMaxKeyLevel, _new.parameterEx.exploMaxKeyLevel);
                    result = true;
                }
				if (_new.parameterEx.exploCumLevel != _old.parameterEx.exploCumLevel)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.ExploCumLevel, _new.parameterEx.exploCumLevel);
                    result = true;
                }
				if (_new.parameterEx.exploExp != _old.parameterEx.exploExp)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.ExploExp, _new.parameterEx.exploExp);
                    result = true;
                }
				if (_new.parameterEx.discoverCount != _old.parameterEx.discoverCount)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.DiscoverCount, _new.parameterEx.discoverCount);
                    result = true;
                }
			}
			return result;
		}
	}
}
