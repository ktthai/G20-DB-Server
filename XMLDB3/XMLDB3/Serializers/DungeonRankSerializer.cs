using Mabinogi;

namespace XMLDB3
{
	public class DungeonRankSerializer
	{
		public static DungeonRank Serialize(Message _message)
		{
			DungeonRank dungeonRank = new DungeonRank();
			dungeonRank.server = _message.ReadString();
			dungeonRank.characterID = _message.ReadS64();
			dungeonRank.characterName = _message.ReadString();
			dungeonRank.dungeonName = _message.ReadString();
			dungeonRank.race = _message.ReadU8();
			dungeonRank.score = _message.ReadS32();
			dungeonRank.laptime = _message.ReadS32();
			return dungeonRank;
		}
	}
}
