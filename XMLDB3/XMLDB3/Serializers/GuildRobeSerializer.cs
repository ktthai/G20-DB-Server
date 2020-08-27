using Mabinogi;

namespace XMLDB3
{
	public class GuildRobeSerializer
	{
		public static GuildRobe Serialize(Message _message)
		{
			GuildRobe guildRobe = new GuildRobe();
			guildRobe.emblemChestIcon = _message.ReadU8();
			guildRobe.emblemChestDeco = _message.ReadU8();
			guildRobe.emblemBeltDeco = _message.ReadU8();
			guildRobe.color1 = _message.ReadS32();
			guildRobe.color2Index = _message.ReadU8();
			guildRobe.color3Index = _message.ReadU8();
			guildRobe.color4Index = _message.ReadU8();
			guildRobe.color5Index = _message.ReadU8();
			return guildRobe;
		}

		public static void Deserialize(GuildRobe _robe, Message _messsage)
		{
			if (_robe == null)
			{
				_robe = new GuildRobe();
			}
			_messsage.WriteU8(_robe.emblemChestIcon);
			_messsage.WriteU8(_robe.emblemChestDeco);
			_messsage.WriteU8(_robe.emblemBeltDeco);
			_messsage.WriteS32(_robe.color1);
			_messsage.WriteU8(_robe.color2Index);
			_messsage.WriteU8(_robe.color3Index);
			_messsage.WriteU8(_robe.color4Index);
			_messsage.WriteU8(_robe.color5Index);
		}
	}
}
