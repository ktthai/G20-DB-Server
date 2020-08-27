using System;
using System.Collections;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class CastleListObjectBuilder
    {
        public static void BuildCastleBid(SimpleReader reader, CastleList castleList)
        {
            CastleBid castleBid;
            while (reader.Read())
            {
                castleBid = new CastleBid();
                castleBid.castleID = reader.GetInt64(Mabinogi.SQL.Columns.CastleBid.CastleId);
                castleBid.bidEndTime = reader.GetDateTime(Mabinogi.SQL.Columns.CastleBid.BidEndTime);
                castleBid.minBidPrice = reader.GetInt32(Mabinogi.SQL.Columns.CastleBid.MinBidPrice);
                castleList.bids.Add(castleBid);
            }
        }

        public static void BuildCastleBidder(SimpleReader reader, CastleList castleList)
        {
            CastleBidder bidder;
            while (reader.Read())
            {
                bidder = new CastleBidder();

                bidder.castleID = reader.GetInt64(Mabinogi.SQL.Columns.CastleBidder.CastleId);
                bidder.bidGuildID = reader.GetInt64(Mabinogi.SQL.Columns.CastleBidder.BidGuildId);
                bidder.bidPrice = reader.GetInt32(Mabinogi.SQL.Columns.CastleBidder.BidPrice);
                bidder.bidOrder = reader.GetInt32(Mabinogi.SQL.Columns.CastleBidder.BidOrder);

                castleList.bidders.Add(bidder);
            }
        }

        public static Hashtable BuildCastleBuildResource(SimpleReader reader)
        {
            Hashtable hashtable = new Hashtable();
            CastleBuildResource castleBuildResource;
            while (reader.Read())
            {
                castleBuildResource = new CastleBuildResource();

                long num = reader.GetInt64(Mabinogi.SQL.Columns.CastleBuildResource.CastleId);

                castleBuildResource.classID = reader.GetInt32(Mabinogi.SQL.Columns.CastleBuildResource.ClassId);
                castleBuildResource.curAmount = reader.GetInt32(Mabinogi.SQL.Columns.CastleBuildResource.CurrentAmount);
                castleBuildResource.maxAmount = reader.GetInt32(Mabinogi.SQL.Columns.CastleBuildResource.MaxAmount);

                ArrayList arrayList = null;
                if (hashtable.ContainsKey(num))
                {
                    arrayList = (ArrayList)hashtable[num];
                }
                else
                {
                    arrayList = new ArrayList();
                    hashtable[num] = arrayList;
                }
                arrayList.Add(castleBuildResource);
            }
            return hashtable;
        }

        public static void BuildCastles(SimpleReader reader, CastleList castleList, Hashtable hashtable)
        {
            Castle castle;
            while (reader.Read())
            {
                castle = new Castle();
                castle.castleID = reader.GetInt64(Mabinogi.SQL.Columns.Castle.CastleId);
                castle.guildID = reader.GetInt64(Mabinogi.SQL.Columns.Castle.GuildId);
                castle.constructed = reader.GetByte(Mabinogi.SQL.Columns.Castle.Constructed);
                castle.castleMoney = reader.GetInt32(Mabinogi.SQL.Columns.Castle.CastleMoney);
                castle.weeklyIncome = reader.GetInt32(Mabinogi.SQL.Columns.Castle.WeeklyIncome);
                castle.taxrate = reader.GetByte(Mabinogi.SQL.Columns.Castle.TaxRate);
                castle.updateTime = reader.GetDateTime(Mabinogi.SQL.Columns.Castle.UpdateTime);
                castle.sellDungeonPass = reader.GetByte(Mabinogi.SQL.Columns.Castle.SellDungeonPass);
                castle.dungeonPassPrice = reader.GetInt32(Mabinogi.SQL.Columns.Castle.DungeonPassPrice);
                castle.flag = reader.GetInt64(Mabinogi.SQL.Columns.Castle.Flag);
                castle.build = new CastleBuild();
                castle.build.durability = reader.GetInt32(Mabinogi.SQL.Columns.Castle.Durability);
                castle.build.maxDurability = reader.GetInt32(Mabinogi.SQL.Columns.Castle.MaxDurability);
                castle.build.buildState = reader.GetByte(Mabinogi.SQL.Columns.Castle.BuildState);

                DateTime nextBuild = reader.GetDateTime(Mabinogi.SQL.Columns.Castle.BuildNextTime);
                if (nextBuild != null)
                {
                    castle.build.buildNextTime = nextBuild;
                }
                else
                {
                    castle.build.buildNextTime = DateTime.MinValue;
                }

                castle.build.buildStep = reader.GetByte(Mabinogi.SQL.Columns.Castle.BuildStep);
                if (hashtable.ContainsKey(castle.castleID))
                {
                    ArrayList arrayList2 = (ArrayList)hashtable[castle.castleID];
                    castle.build.resource = (CastleBuildResource[])arrayList2.ToArray(typeof(CastleBuildResource));
                }
                castleList.castles.Add(castle);
            }
        }

        public static void BuildCastleBlock(SimpleReader reader, CastleList castleList)
        {
            Hashtable hashtable = new Hashtable();
            ArrayList arrayList3 = null;
            CastleBlock castleBlock;
            while (reader.Read())
            {
                long num2 = reader.GetInt64(Mabinogi.SQL.Columns.CastleBlock.CastleId);

                arrayList3 = (hashtable.ContainsKey(num2) ? ((ArrayList)hashtable[num2]) : new ArrayList());
                castleBlock = new CastleBlock();
                castleBlock.gameName = reader.GetString(Mabinogi.SQL.Columns.CastleBlock.GameName);
                castleBlock.flag = reader.GetByte(Mabinogi.SQL.Columns.CastleBlock.Flag);
                castleBlock.entry = reader.GetByte(Mabinogi.SQL.Columns.CastleBlock.Entry);
                arrayList3.Add(castleBlock);
                hashtable[num2] = arrayList3;
            }

            int num3 = 0;
            {
                CastleBlockList castleBlockList;
                foreach (long key in hashtable.Keys)
                {
                    castleBlockList = new CastleBlockList();
                    castleBlockList.castleID = key;
                    castleBlockList.block = (CastleBlock[])((ArrayList)hashtable[key]).ToArray(typeof(CastleBlock));
                    castleList.blocks.Add(castleBlockList);
                    num3++;
                }
            }
        }
    }
}
