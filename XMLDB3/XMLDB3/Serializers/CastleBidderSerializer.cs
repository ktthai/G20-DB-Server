using Mabinogi;

namespace XMLDB3
{
	public class CastleBidderSerializer
	{
		public static CastleBidder Serialize(Message _message)
		{
			CastleBidder castleBidder = new CastleBidder();
			castleBidder.castleID = _message.ReadS64();
			castleBidder.bidGuildID = _message.ReadS64();
			castleBidder.bidPrice = _message.ReadS32();
			castleBidder.bidOrder = _message.ReadS32();
			castleBidder.bidGuildName = _message.ReadString();
			castleBidder.bidCharName = _message.ReadString();
			castleBidder.bidCharacterID = _message.ReadS64();
			return castleBidder;
		}

		public static void Deserialize(CastleBidder _bidder, Message _message)
		{
			_message.WriteS64(_bidder.castleID);
			_message.WriteS64(_bidder.bidGuildID);
			_message.WriteS32(_bidder.bidPrice);
			_message.WriteS32(_bidder.bidOrder);
		}
	}
}
