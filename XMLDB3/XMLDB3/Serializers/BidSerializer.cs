using Mabinogi;
using System;

namespace XMLDB3
{
	public class BidSerializer
	{
		public static Bid Serialize(Message _message)
		{
			Bid bid = new Bid();
			bid.bidID = _message.ReadS64();
			bid.charID = _message.ReadS64();
			bid.charName = _message.ReadString();
			bid.auctionItemID = _message.ReadS32();
			bid.price = _message.ReadS32();
			bid.time = new DateTime(_message.ReadS64());
			bid.bidState = _message.ReadU8();
			return bid;
		}

		public static void Deserialize(Bid _bid, Message _message)
		{
			if (_bid != null)
			{
				_message.WriteS64(_bid.bidID);
				_message.WriteS64(_bid.charID);
				_message.WriteString(_bid.charName);
				_message.WriteS32(_bid.auctionItemID);
				_message.WriteS32(_bid.price);
				_message.WriteS64(_bid.time.Ticks);
				_message.WriteU8(_bid.bidState);
			}
		}
	}
}
