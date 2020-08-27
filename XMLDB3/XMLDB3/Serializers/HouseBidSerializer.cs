using Mabinogi;
using System;

namespace XMLDB3
{
	public class HouseBidSerializer
	{
		public static HouseBid Serialize(Message _message)
		{
			HouseBid houseBid = new HouseBid();
			houseBid.bidEndTime = new DateTime(_message.ReadS64());
			houseBid.bidRepayEndTime = new DateTime(_message.ReadS64());
			houseBid.minBidPrice = _message.ReadS32();
			return houseBid;
		}

		public static void Deserialize(HouseBid _housebid, Message _message)
		{
			_message.WriteS64(_housebid.bidEndTime.Ticks);
			_message.WriteS64(_housebid.bidRepayEndTime.Ticks);
			_message.WriteS32(_housebid.minBidPrice);
		}
	}
}
