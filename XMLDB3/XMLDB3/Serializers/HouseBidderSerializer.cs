using Mabinogi;

namespace XMLDB3
{
	public class HouseBidderSerializer
	{
		public static HouseBidder Serialize(Message _message)
		{
			HouseBidder houseBidder = new HouseBidder();
			houseBidder.bidAccount = _message.ReadString();
			houseBidder.bidPrice = _message.ReadS32();
			houseBidder.bidOrder = _message.ReadS32();
			houseBidder.bidCharName = _message.ReadString();
			houseBidder.bidCharacter = _message.ReadS64();
			return houseBidder;
		}

		public static void Deserialize(HouseBidder _housebidder, Message _message)
		{
			_message.WriteString(_housebidder.bidAccount);
			_message.WriteS32(_housebidder.bidPrice);
			_message.WriteS32(_housebidder.bidOrder);
			_message.WriteString(_housebidder.bidCharName);
		}
	}
}
