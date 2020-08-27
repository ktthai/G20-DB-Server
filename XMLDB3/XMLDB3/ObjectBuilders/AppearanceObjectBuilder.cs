using Mabinogi.SQL;

namespace XMLDB3
{
	public class AppearanceObjectBuilder
	{
		public static CharacterAppearance Build(SimpleReader reader)
		{
			CharacterAppearance characterAppearance = new CharacterAppearance();
			characterAppearance.type = reader.GetInt32(Mabinogi.SQL.Columns.Character.Type);
			characterAppearance.skin_color = reader.GetByte(Mabinogi.SQL.Columns.Character.SkinColor);
			characterAppearance.eye_type = reader.GetInt16(Mabinogi.SQL.Columns.Character.EyeType);
			characterAppearance.eye_color = reader.GetByte(Mabinogi.SQL.Columns.Character.EyeColor);
			characterAppearance.mouth_type = reader.GetByte(Mabinogi.SQL.Columns.Character.MouthType);
			characterAppearance.status = reader.GetInt32(Mabinogi.SQL.Columns.Character.Status);
			characterAppearance.height = reader.GetFloat(Mabinogi.SQL.Columns.Character.Height);
			characterAppearance.fatness = reader.GetFloat(Mabinogi.SQL.Columns.Character.Fatness);
			characterAppearance.upper = reader.GetFloat(Mabinogi.SQL.Columns.Character.Upper);
			characterAppearance.lower = reader.GetFloat(Mabinogi.SQL.Columns.Character.Lower);
			characterAppearance.region = reader.GetInt32(Mabinogi.SQL.Columns.Character.Region);
			characterAppearance.x = reader.GetInt32(Mabinogi.SQL.Columns.Character.X);
			characterAppearance.y = reader.GetInt32(Mabinogi.SQL.Columns.Character.Y);
			characterAppearance.direction = reader.GetByte(Mabinogi.SQL.Columns.Character.Direction);
			characterAppearance.battle_state = reader.GetInt32(Mabinogi.SQL.Columns.Character.BattleState);
			characterAppearance.weapon_set = reader.GetByte(Mabinogi.SQL.Columns.Character.WeaponSet);
			return characterAppearance;
		}
	}
}
