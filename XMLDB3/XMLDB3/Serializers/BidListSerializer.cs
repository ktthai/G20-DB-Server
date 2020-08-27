using Mabinogi;

namespace XMLDB3
{
	public class BidListSerializer
	{
		public static void Deserialize(BidList _list, Message _message)
		{
			if (_list == null || _list.bids == null || _list.bids.Count == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_list.bids.Count);

			foreach (Bid bid in _list.bids)
			{
				BidSerializer.Deserialize(bid, _message);
			}
		}
	}
}
