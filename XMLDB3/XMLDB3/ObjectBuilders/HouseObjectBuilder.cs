using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class HouseObjectBuilder
	{
		public static House BuildHouse(long houseId, SimpleReader houseReader)
		{
			if (houseReader == null)
			{
				return null;
			}
			if (houseReader.Read() == false)
			{
				return null;
			}
			House house = new House();
			house.houseID = houseId;

			house.constructed = houseReader.GetByte(Mabinogi.SQL.Columns.House.Constructed);
			house.updateTime = houseReader.GetDateTime(Mabinogi.SQL.Columns.House.UpdateTime);
			house.charName = houseReader.GetString(Mabinogi.SQL.Columns.House.CharName);
			house.houseName = houseReader.GetString(Mabinogi.SQL.Columns.House.HouseName);
			house.houseClass = houseReader.GetInt32(Mabinogi.SQL.Columns.House.HouseClass);
			house.roofSkin = houseReader.GetByte(Mabinogi.SQL.Columns.House.RoofSkin);
			house.roofColor1 = houseReader.GetByte(Mabinogi.SQL.Columns.House.RoofColor1);
			house.roofColor2 = houseReader.GetByte(Mabinogi.SQL.Columns.House.RoofColor2);
			house.roofColor3 = houseReader.GetByte(Mabinogi.SQL.Columns.House.RoofColor3);
			house.wallSkin = houseReader.GetByte(Mabinogi.SQL.Columns.House.WallSkin);
			house.wallColor1 = houseReader.GetByte(Mabinogi.SQL.Columns.House.WallColor1);
			house.wallColor2 = houseReader.GetByte(Mabinogi.SQL.Columns.House.WallColor2);
			house.wallColor3 = houseReader.GetByte(Mabinogi.SQL.Columns.House.WallColor3);
			house.innerSkin = houseReader.GetByte(Mabinogi.SQL.Columns.House.InnerSkin);
			house.innerColor1 = houseReader.GetByte(Mabinogi.SQL.Columns.House.InnerColor1);
			house.innerColor2 = houseReader.GetByte(Mabinogi.SQL.Columns.House.InnerColor2);
			house.innerColor3 = houseReader.GetByte(Mabinogi.SQL.Columns.House.InnerColor3);
			house.width = houseReader.GetInt32(Mabinogi.SQL.Columns.House.Width);
			house.height = houseReader.GetInt32(Mabinogi.SQL.Columns.House.Height);

			DateTime time;
			if (houseReader.GetDateTimeSafe(Mabinogi.SQL.Columns.House.BidSuccessDate, out time))
			{
				house.bidSuccessDate = time;
			}
			else
			{
				house.bidSuccessDate = DateTime.MinValue;
			}

			if (houseReader.GetDateTimeSafe(Mabinogi.SQL.Columns.House.TaxPrevDate, out time))
			{
				house.taxPrevDate = time;
			}
			else
			{
				house.taxPrevDate = DateTime.MinValue;
			}

			if (houseReader.GetDateTimeSafe(Mabinogi.SQL.Columns.House.TaxNextDate, out time))
			{
				house.taxNextDate = time;
			}
			else
			{
				house.taxNextDate = DateTime.MinValue;
			}

			house.taxPrice = houseReader.GetInt32(Mabinogi.SQL.Columns.House.TaxPrice);
			house.taxAutopay = houseReader.GetByte(Mabinogi.SQL.Columns.House.TaxAutopay);
			house.houseMoney = houseReader.GetInt32(Mabinogi.SQL.Columns.House.HouseMoney);
			house.deposit = houseReader.GetInt32(Mabinogi.SQL.Columns.House.Deposit);
			house.flag = houseReader.GetInt64(Mabinogi.SQL.Columns.House.Flag);

			return house;
		}

		public static void HouseBidBuilder(SimpleReader houseBidReader, House house)
		{

			if (houseBidReader.Read())
			{
				house.bid = new HouseBid();
				house.bid.bidEndTime = houseBidReader.GetDateTime(Mabinogi.SQL.Columns.HouseBid.BidEndTime);
				house.bid.bidRepayEndTime = houseBidReader.GetDateTime(Mabinogi.SQL.Columns.HouseBid.BidRepayEndTime);
				house.bid.minBidPrice = houseBidReader.GetInt32(Mabinogi.SQL.Columns.HouseBid.MinBidPrice);
			}
		}

		public static void HouseOwnerBuilder(SimpleReader houseOwnerReader, House house)
        {
            if (houseOwnerReader.Read())
                house.account = houseOwnerReader.GetString(Mabinogi.SQL.Columns.HouseOwner.Account);
            else
                house.account = string.Empty;
        }

		public static void HouseBidderBuilder(SimpleReader houseBidderReader, House house)
		{ 
			if (houseBidderReader.HasRows)
			{
				house.bidders = new List<HouseBidder>();
				HouseBidder bidder;
				while(houseBidderReader.Read())
				{
					bidder = new HouseBidder();
					bidder.bidAccount = houseBidderReader.GetString(Mabinogi.SQL.Columns.HouseBidder.BidAccount);
					bidder.bidPrice = houseBidderReader.GetInt32(Mabinogi.SQL.Columns.HouseBidder.BidPrice);
					bidder.bidOrder = houseBidderReader.GetInt32(Mabinogi.SQL.Columns.HouseBidder.BidOrder);
					bidder.bidCharName = houseBidderReader.GetString(Mabinogi.SQL.Columns.HouseBidder.BidCharName);
					house.bidders.Add(bidder);
				}
			}
		}
	}
}
