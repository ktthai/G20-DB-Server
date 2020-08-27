using Mabinogi;
using System;

namespace XMLDB3
{
	public class CastleBidSerializer
	{
		public static CastleBid Serialize(Message _message)
		{
			CastleBid castleBid = new CastleBid();
			castleBid.castleID = _message.ReadS64();
			castleBid.bidEndTime = new DateTime(_message.ReadS64());
			castleBid.minBidPrice = _message.ReadS32();
			return castleBid;
		}

		public static void Deserialize(CastleBid _bid, Message _message)
		{
			_message.WriteS64(_bid.castleID);
			_message.WriteS64(_bid.bidEndTime.Ticks);
			_message.WriteS32(_bid.minBidPrice);
		}
	}
}
