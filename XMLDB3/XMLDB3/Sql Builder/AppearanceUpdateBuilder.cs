using Mabinogi.SQL;

namespace XMLDB3
{
	public class AppearanceUpdateBuilder
	{
		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.appearance != null && _old.appearance != null)
			{
				if (_new.appearance.type != _old.appearance.type)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Type, _new.appearance.type);
                    result = true;
                }
				if (_new.appearance.skin_color != _old.appearance.skin_color)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.SkinColor, _new.appearance.skin_color);
                    result = true;
                }
				if (_new.appearance.eye_type != _old.appearance.eye_type)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.EyeType, _new.appearance.eye_type);
                    result = true;
                }
				if (_new.appearance.eye_color != _old.appearance.eye_color)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.EyeColor, _new.appearance.eye_color);
                    result = true;
                }
				if (_new.appearance.mouth_type != _old.appearance.mouth_type)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.MouthType, _new.appearance.mouth_type);
                    result = true;
                }
				if (_new.appearance.status != _old.appearance.status)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Status, _new.appearance.status);
                    result = true;
                }
				if (_new.appearance.height != _old.appearance.height)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Height, _new.appearance.height);
                    result = true;
                }
				if (_new.appearance.fatness != _old.appearance.fatness)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Fatness, _new.appearance.fatness);
                    result = true;
                }
				if (_new.appearance.upper != _old.appearance.upper)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Upper, _new.appearance.upper);
                    result = true;
                }
				if (_new.appearance.lower != _old.appearance.lower)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Lower, _new.appearance.lower);
                    result = true;
                }
				if (_new.appearance.region != _old.appearance.region)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Region, _new.appearance.region);
                    result = true;
                }
				if (_new.appearance.x != _old.appearance.x)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.X, _new.appearance.x);
                    result = true;
                }
				if (_new.appearance.y != _old.appearance.y)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Y, _new.appearance.y);
                    result = true;
                }
				if (_new.appearance.direction != _old.appearance.direction)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.Direction, _new.appearance.direction);
					System.Console.WriteLine("Direction: " + _new.appearance.direction);
                    result = true;
                }
				if (_new.appearance.battle_state != _old.appearance.battle_state)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.BattleState, _new.appearance.battle_state);
                    result = true;
                }
				if (_new.appearance.weapon_set != _old.appearance.weapon_set)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.WeaponSet, _new.appearance.weapon_set);
					result = true;
				}
			}
			return result;
		}
	}
}
