using System.Collections.Generic;

public class CastleList
{
	public List<CastleBid> bids;

	public List<CastleBidder> bidders;

	public List<Castle> castles;

	public List<CastleBlockList> blocks;

	public CastleList()
	{
		bids = new List<CastleBid>();
		bidders = new List<CastleBidder>();
		castles = new List<Castle>();
		blocks = new List<CastleBlockList>();
	}
}
