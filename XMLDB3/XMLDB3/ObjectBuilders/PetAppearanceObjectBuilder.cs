using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetAppearanceObjectBuilder
	{
		public static PetAppearance Build(SimpleReader reader)
		{
			PetAppearance petAppearance = new PetAppearance();
			petAppearance.type = reader.GetInt32(Mabinogi.SQL.Columns.Pet.Type);
			petAppearance.skin_color = reader.GetByte(Mabinogi.SQL.Columns.Pet.SkinColor);
			petAppearance.eye_type = reader.GetInt16(Mabinogi.SQL.Columns.Pet.EyeType);
			petAppearance.eye_color = reader.GetByte(Mabinogi.SQL.Columns.Pet.EyeColor);
			petAppearance.mouth_type = reader.GetByte(Mabinogi.SQL.Columns.Pet.MouthType);
			petAppearance.status = reader.GetInt32(Mabinogi.SQL.Columns.Pet.Status);
			petAppearance.height =  reader.GetFloat(Mabinogi.SQL.Columns.Pet.Height);
			petAppearance.fatness = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Fatness);
			petAppearance.upper = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Upper);
			petAppearance.lower = reader.GetFloat(Mabinogi.SQL.Columns.Pet.Lower);
			petAppearance.region = reader.GetInt32(Mabinogi.SQL.Columns.Pet.Region);
			petAppearance.x = reader.GetInt32(Mabinogi.SQL.Columns.Pet.X);
			petAppearance.y = reader.GetInt32(Mabinogi.SQL.Columns.Pet.Y);
			petAppearance.direction = reader.GetByte(Mabinogi.SQL.Columns.Pet.Direction);
			petAppearance.battle_state = reader.GetInt32(Mabinogi.SQL.Columns.Pet.BattleState);
			petAppearance.extra_01 = reader.GetInt32(Mabinogi.SQL.Columns.Pet.Extra1);
			petAppearance.extra_02 = reader.GetInt32(Mabinogi.SQL.Columns.Pet.Extra2);
			petAppearance.extra_03 = reader.GetInt32(Mabinogi.SQL.Columns.Pet.Extra3);
			return petAppearance;
		}
	}
}
