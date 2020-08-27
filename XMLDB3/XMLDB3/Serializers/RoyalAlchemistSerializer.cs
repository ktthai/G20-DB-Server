using Mabinogi;

namespace XMLDB3
{
	public class RoyalAlchemistSerializer
	{
		public static RoyalAlchemist Serialize(Message _message)
		{
			RoyalAlchemist royalAlchemist = new RoyalAlchemist();
			royalAlchemist.charID = _message.ReadS64();
			royalAlchemist.charName = _message.ReadString();
			royalAlchemist.registrationFlag = _message.ReadU8();
			royalAlchemist.rank = _message.ReadU16();
			royalAlchemist.meta = _message.ReadString();
			return royalAlchemist;
		}

		public static void Deserialize(RoyalAlchemist _data, Message _message)
		{
			if (_data != null)
			{
				_message.WriteS64(_data.charID);
				_message.WriteString(_data.charName);
				_message.WriteU8(_data.registrationFlag);
				_message.WriteU16(_data.rank);
				_message.WriteString(_data.meta);
			}
		}
	}
}
