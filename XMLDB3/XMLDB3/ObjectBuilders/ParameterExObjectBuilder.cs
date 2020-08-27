using Mabinogi.SQL;

namespace XMLDB3
{
	public class ParameterExObjectBuilder
	{
		public static CharacterParameterEx Build(SimpleReader reader)
		{
			CharacterParameterEx characterParameterEx = new CharacterParameterEx();
			characterParameterEx.str_boost = reader.GetByte(Mabinogi.SQL.Columns.Character.StrengthBoost);
			characterParameterEx.dex_boost = reader.GetByte(Mabinogi.SQL.Columns.Character.DexterityBoost);
			characterParameterEx.int_boost = reader.GetByte(Mabinogi.SQL.Columns.Character.IntelligenceBoost);
			characterParameterEx.will_boost = reader.GetByte(Mabinogi.SQL.Columns.Character.WillBoost);
			characterParameterEx.luck_boost = reader.GetByte(Mabinogi.SQL.Columns.Character.LuckBoost);
			characterParameterEx.height_boost = reader.GetByte(Mabinogi.SQL.Columns.Character.HeightBoost);
			characterParameterEx.fatness_boost = reader.GetByte(Mabinogi.SQL.Columns.Character.FatnessBoost);
			characterParameterEx.upper_boost = reader.GetByte(Mabinogi.SQL.Columns.Character.UpperBoost);
			characterParameterEx.lower_boost = reader.GetByte(Mabinogi.SQL.Columns.Character.LowerBoost);
			characterParameterEx.life_boost = reader.GetByte(Mabinogi.SQL.Columns.Character.LifeBoost);
			characterParameterEx.mana_boost = reader.GetByte(Mabinogi.SQL.Columns.Character.ManaBoost);
			characterParameterEx.stamina_boost = reader.GetByte(Mabinogi.SQL.Columns.Character.StaminaBoost);
			characterParameterEx.toxic = reader.GetFloat(Mabinogi.SQL.Columns.Character.Toxic);
			characterParameterEx.toxic_drunken_time = reader.GetInt64(Mabinogi.SQL.Columns.Character.ToxicDrunkenTime);
			characterParameterEx.toxic_str = reader.GetFloat(Mabinogi.SQL.Columns.Character.ToxicStrength);
			characterParameterEx.toxic_int = reader.GetFloat(Mabinogi.SQL.Columns.Character.ToxicIntelligence);
			characterParameterEx.toxic_dex = reader.GetFloat(Mabinogi.SQL.Columns.Character.ToxicDexterity);
			characterParameterEx.toxic_will = reader.GetFloat(Mabinogi.SQL.Columns.Character.ToxicWill);
			characterParameterEx.toxic_luck = reader.GetFloat(Mabinogi.SQL.Columns.Character.ToxicLuck);
			characterParameterEx.lastdungeon = reader.GetString(Mabinogi.SQL.Columns.Character.LastDungeon);
			characterParameterEx.lasttown = reader.GetString(Mabinogi.SQL.Columns.Character.LastTown);
			characterParameterEx.exploLevel = reader.GetInt16(Mabinogi.SQL.Columns.Character.ExploLevel);
			characterParameterEx.exploMaxKeyLevel = reader.GetInt16(Mabinogi.SQL.Columns.Character.ExploMaxKeyLevel);
			characterParameterEx.exploCumLevel = reader.GetInt32(Mabinogi.SQL.Columns.Character.ExploCumLevel);
			characterParameterEx.exploExp = reader.GetInt64(Mabinogi.SQL.Columns.Character.ExploExp);
			characterParameterEx.discoverCount = reader.GetInt32(Mabinogi.SQL.Columns.Character.DiscoverCount);
			return characterParameterEx;
		}
	}
}
