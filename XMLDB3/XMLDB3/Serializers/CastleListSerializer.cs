using Mabinogi;

namespace XMLDB3
{
	public class CastleListSerializer
	{
		public static void Deserialize(CastleList _list, Message _message)
		{
			if (_list.castles != null)
			{
				_message.WriteS32(_list.castles.Count);
				for (int i = 0; i < _list.castles.Count; i++)
				{
					CastleSerializer.Deserialize(_list.castles[i], _message);
					CastleBuildSerializer.Deserialize(_list.castles[i].build, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
			if (_list.bids != null)
			{
				_message.WriteS32(_list.bids.Count);

				foreach (CastleBid bid in _list.bids)
				{
					CastleBidSerializer.Deserialize(bid, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
			if (_list.bidders != null)
			{
				_message.WriteS32(_list.bidders.Count);

				foreach (CastleBidder bidder in _list.bidders)
				{
					CastleBidderSerializer.Deserialize(bidder, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
			if (_list.blocks != null)
			{
				_message.WriteS32(_list.blocks.Count);

				foreach (CastleBlockList castleBlockList in _list.blocks)
				{
					_message.WriteS64(castleBlockList.castleID);
					CastleBlockSerializer.Deserialize(castleBlockList.block, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}
	}
}
