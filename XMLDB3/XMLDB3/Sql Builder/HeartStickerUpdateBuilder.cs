using Mabinogi.SQL;

namespace XMLDB3
{
	public class HeartStickerUpdateBuilder
	{
		public static bool Build(Character _new, Character _old, SimpleCommand cmd)
		{
			bool result = false;
			if (_new.heartSticker != null && _old.heartSticker != null)
			{
				string text = string.Empty;
				if (_new.heartSticker.heartUpdateTime != _old.heartSticker.heartUpdateTime)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.HeartUpdateTime, _new.heartSticker.heartUpdateTime);
                    result = true;
                }
				if (_new.heartSticker.heartPoint != _old.heartSticker.heartPoint)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.HeartPoint, _new.heartSticker.heartPoint);
                    result = true;
                }
				if (_new.heartSticker.heartTotalPoint != _old.heartSticker.heartTotalPoint)
				{
					cmd.Set(Mabinogi.SQL.Columns.Character.HeartTotalPoint, _new.heartSticker.heartTotalPoint);
                    result = true;
                }
			}
			return result;
		}
	}
}
