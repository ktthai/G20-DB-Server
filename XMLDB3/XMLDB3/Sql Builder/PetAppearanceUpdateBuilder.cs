using Mabinogi.SQL;

namespace XMLDB3
{
	public class PetAppearanceUpdateBuilder
	{
		public static void Build(Pet _new, Pet _old, ref SimpleCommand cmd)
		{
			if (_new.appearance != null && _old.appearance != null)
			{
				if (_new.appearance.type != _old.appearance.type)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Type, _new.appearance.type);
				}
				if (_new.appearance.skin_color != _old.appearance.skin_color)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.SkinColor, _new.appearance.skin_color);
				}
				if (_new.appearance.eye_color != _old.appearance.eye_color)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.EyeColor, _new.appearance.eye_color);
				}
				if (_new.appearance.eye_type != _old.appearance.eye_type)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.EyeType, _new.appearance.eye_type);
				}
				if (_new.appearance.mouth_type != _old.appearance.mouth_type)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.MouthType, _new.appearance.mouth_type);
				}
				if (_new.appearance.status != _old.appearance.status)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Status, _new.appearance.status);
				}
				if (_new.appearance.height != _old.appearance.height)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Height, _new.appearance.height);
				}
				if (_new.appearance.fatness != _old.appearance.fatness)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Fatness, _new.appearance.fatness);
				}
				if (_new.appearance.upper != _old.appearance.upper)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Upper, _new.appearance.upper);
				}
				if (_new.appearance.lower != _old.appearance.lower)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Lower, _new.appearance.lower);
				}
				if (_new.appearance.region != _old.appearance.region)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Region, _new.appearance.region);
				}
				if (_new.appearance.x != _old.appearance.x)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.X, _new.appearance.x);
				}
				if (_new.appearance.y != _old.appearance.y)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Y, _new.appearance.y);
				}
				if (_new.appearance.direction != _old.appearance.direction)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Direction, _new.appearance.direction);
				}
				if (_new.appearance.battle_state != _old.appearance.battle_state)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.BattleState, _new.appearance.battle_state);
				}
				if (_new.appearance.extra_01 != _old.appearance.extra_01)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Extra1, _new.appearance.extra_01);
				}
				if (_new.appearance.extra_02 != _old.appearance.extra_02)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Extra2, _new.appearance.extra_02);
				}
				if (_new.appearance.extra_03 != _old.appearance.extra_03)
				{
					cmd.Set(Mabinogi.SQL.Columns.Pet.Extra3, _new.appearance.extra_03);
				}
			}
		}
	}
}
