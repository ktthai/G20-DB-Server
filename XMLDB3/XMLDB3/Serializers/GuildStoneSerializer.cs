using Mabinogi;

namespace XMLDB3
{
	public class GuildStoneSerializer
	{
		public static GuildStone Serialize(Message _message)
		{
			GuildStone guildStone = new GuildStone();
			guildStone.server = _message.ReadString();
			guildStone.position_id = _message.ReadS64();
			guildStone.type = _message.ReadS32();
			guildStone.region = _message.ReadS16();
			guildStone.x = _message.ReadS32();
			guildStone.y = _message.ReadS32();
			guildStone.direction = _message.ReadFloat();
			return guildStone;
		}

		public static void Deserialize(GuildStone _stone, Message _messsage)
		{
			if (_stone == null)
			{
				_stone = new GuildStone();
			}
			_messsage.WriteString(_stone.server);
			_messsage.WriteS64(_stone.position_id);
			_messsage.WriteS32(_stone.type);
			_messsage.WriteS16(_stone.region);
			_messsage.WriteS32(_stone.x);
			_messsage.WriteS32(_stone.y);
			_messsage.WriteFloat(_stone.direction);
		}
	}
}
