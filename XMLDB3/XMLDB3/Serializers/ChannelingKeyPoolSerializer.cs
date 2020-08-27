using Mabinogi;

namespace XMLDB3
{
	public class ChannelingKeyPoolSerializer
	{
		public static ChannelingKey Serialize(Message _message)
		{
			ChannelingKey channelingKey = new ChannelingKey();
			channelingKey.provider = _message.ReadU8();
			channelingKey.keystring = _message.ReadString();
			return channelingKey;
		}

		public static void Deserialize(ChannelingKey _chKeyPool, Message _message)
		{
		}
	}
}
