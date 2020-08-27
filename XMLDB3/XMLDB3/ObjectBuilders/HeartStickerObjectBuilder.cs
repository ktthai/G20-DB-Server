using Mabinogi.SQL;

namespace XMLDB3
{
	public class HeartStickerObjectBuilder
	{
		public static CharacterHeartSticker Build(SimpleReader reader)
		{
			CharacterHeartSticker characterHeartSticker = new CharacterHeartSticker();
			characterHeartSticker.heartUpdateTime = reader.GetInt64(Mabinogi.SQL.Columns.Character.HeartUpdateTime);
			characterHeartSticker.heartPoint = reader.GetInt16(Mabinogi.SQL.Columns.Character.HeartPoint);
			characterHeartSticker.heartTotalPoint = reader.GetInt16(Mabinogi.SQL.Columns.Character.HeartTotalPoint);
			return characterHeartSticker;
		}
	}
}
