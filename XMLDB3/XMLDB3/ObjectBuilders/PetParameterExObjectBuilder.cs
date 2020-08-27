using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetParameterExObjectBuilder
	{
		public static PetParameterEx Build(SimpleReader reader)
		{
			PetParameterEx petParameterEx = new PetParameterEx();
			petParameterEx.str_boost = reader.GetByte(Mabinogi.SQL.Columns.Pet.StrengthBoost);
			petParameterEx.dex_boost = reader.GetByte(Mabinogi.SQL.Columns.Pet.DexterityBoost);
			petParameterEx.int_boost = reader.GetByte(Mabinogi.SQL.Columns.Pet.IntelligenceBoost);
			petParameterEx.will_boost = reader.GetByte(Mabinogi.SQL.Columns.Pet.WillBoost);
			petParameterEx.luck_boost = reader.GetByte(Mabinogi.SQL.Columns.Pet.LuckBoost);
			petParameterEx.height_boost = reader.GetByte(Mabinogi.SQL.Columns.Pet.HeightBoost);
			petParameterEx.fatness_boost = reader.GetByte(Mabinogi.SQL.Columns.Pet.FatnessBoost);
			petParameterEx.upper_boost = reader.GetByte(Mabinogi.SQL.Columns.Pet.UpperBoost);
			petParameterEx.lower_boost = reader.GetByte(Mabinogi.SQL.Columns.Pet.LowerBoost);
			petParameterEx.life_boost = reader.GetByte(Mabinogi.SQL.Columns.Pet.LifeBoost);
			petParameterEx.mana_boost = reader.GetByte(Mabinogi.SQL.Columns.Pet.ManaBoost);
			petParameterEx.stamina_boost = reader.GetByte(Mabinogi.SQL.Columns.Pet.StaminaBoost);
			petParameterEx.toxic = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Toxic);
			petParameterEx.toxic_drunken_time = reader.GetInt64(Mabinogi.SQL.Columns.Pet.ToxicDrunkenTime);
			petParameterEx.toxic_str = reader.GetFloat(Mabinogi.SQL.Columns.Pet.ToxicStrength);
			petParameterEx.toxic_int = reader.GetFloat(Mabinogi.SQL.Columns.Pet.ToxicIntelligence);
			petParameterEx.toxic_dex = reader.GetFloat(Mabinogi.SQL.Columns.Pet.ToxicDexterity);
			petParameterEx.toxic_will = reader.GetFloat(Mabinogi.SQL.Columns.Pet.ToxicWill);
			petParameterEx.toxic_luck = reader.GetFloat(Mabinogi.SQL.Columns.Pet.ToxicLuck);
			petParameterEx.lastdungeon = reader.GetString(Mabinogi.SQL.Columns.Pet.LastDungeon);
			petParameterEx.lasttown = reader.GetString(Mabinogi.SQL.Columns.Pet.LastTown);
			return petParameterEx;
		}
	}
}
