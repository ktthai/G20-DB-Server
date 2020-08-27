using Mabinogi;

namespace XMLDB3
{
	public class ChronicleSerializer
	{
		public static Chronicle Serialize(Message _message)
		{
			Chronicle chronicle = new Chronicle();
			chronicle.serverName = _message.ReadString();
			chronicle.questID = _message.ReadS32();
			chronicle.charID = _message.ReadS64();
			chronicle.meta = _message.ReadString();
			return chronicle;
		}
	}
}
