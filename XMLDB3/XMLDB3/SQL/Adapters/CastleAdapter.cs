using System;
using System.Collections;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class CastleAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Castle;

        public CastleAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public CastleList ReadList()
        {
            WorkSession.WriteStatus("CastleSqlAdapter.ReadList() : 함수에 진입하였습니다");
            CastleList castleList = new CastleList();
            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.ReadList() : 데이터베이스와 연결합니다");

                using (var conn = Connection)
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.ReadList() : DataSet 에 성 정보를 채웁니다");
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBid))
                    using (var reader = cmd.ExecuteReader())
                    {
                        CastleListObjectBuilder.BuildCastleBid(reader, castleList);
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBidder))
                    using (var reader = cmd.ExecuteReader())
                    {
                        CastleListObjectBuilder.BuildCastleBidder(reader, castleList);
                    }

                    Hashtable hashTable;
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBuildResource))
                    using (var reader = cmd.ExecuteReader())
                    {
                        hashTable = CastleListObjectBuilder.BuildCastleBuildResource(reader);
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Castle))
                    using (var reader = cmd.ExecuteReader())
                    {
                        CastleListObjectBuilder.BuildCastles(reader, castleList, hashTable);
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBlock))
                    using (var reader = cmd.ExecuteReader())
                    {
                        CastleListObjectBuilder.BuildCastleBlock(reader, castleList);
                    }
                }
                return castleList;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.ReadList() : 연결을 종료합니다");
            }
        }

        public bool CreateBid(CastleBid _bid)
        {
            WorkSession.WriteStatus("CastleSqlAdapter.CreateBid() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.CreateBid() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBid))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.CastleBid.CastleId, _bid.castleID);
                        cmd.Set(Mabinogi.SQL.Columns.CastleBid.BidStartTime, DateTime.Now);
                        cmd.Set(Mabinogi.SQL.Columns.CastleBid.BidEndTime, _bid.bidEndTime);
                        cmd.Set(Mabinogi.SQL.Columns.CastleBid.MinBidPrice, _bid.minBidPrice);

                        WorkSession.WriteStatus("CastleSqlAdapter.CreateBid() : 명령을 실행합니다");
                        cmd.Execute();
                    }
                }
                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _bid);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _bid);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.CreateBid() : 연결을 종료합니다");
            }
        }

        public bool EndBid(Castle _castle, GuildAdapter _guildAdapter)
        {
            WorkSession.WriteStatus("CastleSqlAdapter.EndBid() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.EndBid() : 데이터베이스와 연결합니다");
                List<CastleBidder> bidders = new List<CastleBidder>();
                using (var conn = Connection)
                {
                    var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBid);
                    cmd.Where(Mabinogi.SQL.Columns.CastleBid.CastleId, _castle.castleID);

                    if (!cmd.ExecuteReader().HasRows)
                        throw new SimpleSqlException("There is no bid for " + _castle.castleID);

                    if (_castle.guildID != 0)
                    {
                        cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBidder);
                        cmd.Where(Mabinogi.SQL.Columns.CastleBidder.CastleId, _castle.castleID);
                        cmd.Where(Mabinogi.SQL.Columns.CastleBidder.BidGuildId, _castle.guildID);

                        if (!cmd.ExecuteReader().HasRows)
                            throw new SimpleSqlException("There is no guild " + _castle.guildID + " bidding for " + _castle.castleID);



                        int rows = 0;
                        cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Castle);
                        cmd.Where(Mabinogi.SQL.Columns.Castle.CastleId, _castle.castleID);

                        cmd.Set(Mabinogi.SQL.Columns.Castle.GuildId, _castle.guildID);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.Constructed, _castle.constructed);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.CastleMoney, _castle.castleMoney);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.WeeklyIncome, _castle.weeklyIncome);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.TaxRate, _castle.taxrate);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.UpdateTime, _castle.updateTime);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.Durability, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.MaxDurability, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.BuildState, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.BuildNextTime, null);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.BuildStep, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.Flag, _castle.flag);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.SellDungeonPass, _castle.sellDungeonPass);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.DungeonPassPrice, _castle.dungeonPassPrice);

                        rows = cmd.Execute();

                        if (rows == 0)
                        {
                            cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Castle);

                            cmd.Set(Mabinogi.SQL.Columns.Castle.CastleId, _castle.castleID);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.GuildId, _castle.guildID);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.Constructed, _castle.constructed);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.CastleMoney, _castle.castleMoney);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.WeeklyIncome, _castle.weeklyIncome);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.TaxRate, _castle.taxrate);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.UpdateTime, _castle.updateTime);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.Durability, 0);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.MaxDurability, 0);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.BuildState, 0);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.BuildNextTime, null);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.BuildStep, 0);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.Flag, _castle.flag);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.SellDungeonPass, _castle.sellDungeonPass);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.DungeonPassPrice, _castle.dungeonPassPrice);

                            cmd.Execute();
                        }

                        bidders.AddRange(DeleteCastleBidder(_castle.castleID, _castle.guildID, 1, conn));
                    }

                    bidders.AddRange(DeleteCastleBidder(_castle.castleID, 0, 0, conn));
                }

                using (var conn = _guildAdapter.Connection)
                using (var transaction = conn.BeginTransaction())
                {
                    foreach (CastleBidder bidder in bidders)
                    {
                        _guildAdapter.AddMoney(bidder.bidGuildID, bidder.bidPrice, conn, transaction);
                    }
                    transaction.Commit();
                }

                return true;

            }
            catch (SimpleSqlException ex)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.EndBid() : 트랜잭션을 롤백합니다");
                ExceptionMonitor.ExceptionRaised(ex, _castle);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.EndBid() : 트랜잭션을 롤백합니다");
                ExceptionMonitor.ExceptionRaised(ex2, _castle);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.EndBid() : 연결을 종료합니다");
            }
        }

        public REPLY_RESULT CreateBidder(CastleBidder _bidder, GuildAdapter _guildAdapter, ref int _remainMoney)
        {
            WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 데이터베이스와 연결합니다");

                // TODO: trim this down to necessary commands only
                using (var conn = Connection)
                {
                    var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Castle);
                    cmd.Where(Mabinogi.SQL.Columns.Castle.GuildId, _bidder.bidGuildID);

                    if (cmd.ExecuteReader().HasRows)
                        throw new SimpleSqlException("Guild " + _bidder.bidGuildID + " already has a castle");


                    cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBidder);
                    cmd.Where(Mabinogi.SQL.Columns.CastleBidder.BidGuildId, _bidder.bidGuildID);

                    if (cmd.ExecuteReader().HasRows)
                        throw new SimpleSqlException("Guild " + _bidder.bidGuildID + " already has bid on a castle");

                    if (!_guildAdapter.WithdrawMoney(_bidder.bidGuildID, _bidder.bidPrice, out _remainMoney))
                    {
                        _remainMoney += _bidder.bidPrice;
                        return REPLY_RESULT.FAIL_EX;
                    }

                    cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBidder);
                    cmd.Set(Mabinogi.SQL.Columns.CastleBidder.CastleId, _bidder.castleID);
                    cmd.Set(Mabinogi.SQL.Columns.CastleBidder.BidGuildId, _bidder.bidGuildID);
                    cmd.Set(Mabinogi.SQL.Columns.CastleBidder.BidPrice, _bidder.bidPrice);
                    cmd.Set(Mabinogi.SQL.Columns.CastleBidder.BidOrder, _bidder.bidOrder);
                    cmd.Set(Mabinogi.SQL.Columns.CastleBidder.BidGuildName, _bidder.bidGuildName);
                    cmd.Set(Mabinogi.SQL.Columns.CastleBidder.BidCharacterID, _bidder.bidCharacterID);
                    cmd.Set(Mabinogi.SQL.Columns.CastleBidder.BidCharName, _bidder.bidCharName);
                    cmd.Set(Mabinogi.SQL.Columns.CastleBidder.BidTime, DateTime.Now);

                    WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 입찰자를 생성합니다.");
                    cmd.Execute();
                    WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 길드에서 돈을 인출합니다.");

                    return REPLY_RESULT.SUCCESS;
                }
            }
            catch (SimpleSqlException ex)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 트랜잭션을 롤백합니다");

                if (ex.Number == 50000)
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 길드 [" + _bidder.bidGuildID + "] 가 이미 성을 소유하고 있거나, 입찰에 참여중입니다.");
                    return REPLY_RESULT.FAIL;
                }
                ExceptionMonitor.ExceptionRaised(ex, _bidder);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return REPLY_RESULT.ERROR;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 트랜잭션을 롤백합니다");
                ExceptionMonitor.ExceptionRaised(ex2, _bidder);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.CreateBidder() : 연결을 종료합니다");
            }
        }

        public REPLY_RESULT UpdateBidder(long _castleID, long _guildID, int _bidPrice, int _bidDiffPrice, int _bidOrder, GuildAdapter _guildAdapter, ref int _remainMoney)
        {
            WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Castle);
                    cmd.Where(Mabinogi.SQL.Columns.Castle.GuildId, _guildID);

                    if (cmd.ExecuteReader().HasRows)
                        throw new SimpleSqlException("Guild " + _guildID + " already has castle");


                    cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBidder);
                    cmd.Where(Mabinogi.SQL.Columns.CastleBidder.BidGuildId, _guildID);
                    cmd.Where(Mabinogi.SQL.Columns.CastleBidder.CastleId, _castleID);

                    if (cmd.ExecuteReader().HasRows)
                    {
                        WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 입찰자를 업데이트합니다.");
                        cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBidder);

                        cmd.Where(Mabinogi.SQL.Columns.CastleBidder.BidGuildId, _guildID);
                        cmd.Where(Mabinogi.SQL.Columns.CastleBidder.CastleId, _castleID);

                        cmd.Set(Mabinogi.SQL.Columns.CastleBidder.BidPrice, _bidPrice);
                        cmd.Set(Mabinogi.SQL.Columns.CastleBidder.BidOrder, _bidOrder);

                        if (cmd.Execute() < 1)
                        {
                            WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 길드가 이미 성을 소유하고 있거나, 입찰하지 않았습니다.");
                            ExceptionMonitor.ExceptionRaised(new Exception("길드 [" + _guildID + "] 가 이미 성을 소유하고 있거나, 입찰하지 않았습니다."));
                            return REPLY_RESULT.FAIL;
                        }
                    }
                }

                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 길드 머니를 업데이트 합니다.");
                if (_bidDiffPrice > 0)
                {
                    if (_guildAdapter.WithdrawMoney(_guildID, _bidDiffPrice, out _remainMoney))
                    {
                        WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 길드 머니가 부족합니다.");
                        return REPLY_RESULT.SUCCESS;
                    }
                }
                else if (_bidDiffPrice < 0)
                {
                    if (_guildAdapter.AddMoney(_guildID, Math.Abs(_bidDiffPrice)))
                    {
                        return REPLY_RESULT.SUCCESS;
                    }
                }

                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 길드 머니가 부족합니다.");
                return REPLY_RESULT.FAIL_EX;
            }
            catch (SimpleSqlException ex)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 트랜잭션을 롤백합니다");
                ExceptionMonitor.ExceptionRaised(ex, _castleID, _guildID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return REPLY_RESULT.ERROR;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 트랜잭션을 롤백합니다");
                ExceptionMonitor.ExceptionRaised(ex2, _castleID, _guildID);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBidder() : 연결을 종료합니다");
            }
        }

        public REPLY_RESULT DeleteBidder(long _castleID, long _guildID, int _repayMoney, GuildAdapter _guildAdapter, ref int _remainMoney)
        {
            WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 데이터베이스와 연결합니다");

                List<CastleBidder> bidders;
                using (var conn = Connection)
                    bidders = DeleteCastleBidder(_castleID, _guildID, 2, conn);

                if (bidders.Count <= 0)
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 길드가 입찰하지 않았습니다.");
                    ExceptionMonitor.ExceptionRaised(new Exception("길드 [" + _guildID + "] 는 입찰하지 않았습니다."));
                    return REPLY_RESULT.FAIL;
                }

                WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 입찰자를 삭제합니다.");

                foreach (CastleBidder bidder in bidders)
                    _guildAdapter.AddMoney(bidder.bidGuildID, bidder.bidPrice);



                WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 길드 머니를 업데이트 합니다.");
                _remainMoney = _guildAdapter.GetMoney(_guildID);

                return REPLY_RESULT.SUCCESS;
            }
            catch (SimpleSqlException ex)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 트랜잭션을 롤백합니다");
                ExceptionMonitor.ExceptionRaised(ex, _castleID, _guildID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return REPLY_RESULT.ERROR;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 트랜잭션을 롤백합니다");
                ExceptionMonitor.ExceptionRaised(ex2, _castleID, _guildID);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.DeleteBidder() : 연결을 종료합니다");
            }
        }

        public REPLY_RESULT TakeGuildMoney(long _castleID, long _guildID, int _money, ref int _remainMoney, GuildAdapter _guildAdapter)
        {
            WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 데이터베이스와 연결합니다");

                using (var conn = Connection)
                {
                    WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 성의 돈을 업데이트 합니다.");

                    var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Castle);
                    cmd.Where(Mabinogi.SQL.Columns.Castle.CastleId, _castleID);

                    if (cmd.ExecuteReader().HasRows)
                    {

                        WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 길드 머니를 업데이트 합니다.");
                        var result = _guildAdapter.GetMoney(_guildID);
                        int remain;
                        if (result >= _money)
                            if (_guildAdapter.WithdrawMoney(_guildID, _money, out remain))
                            {
                                int castleCash = GetCastleCash(_castleID, conn, null);

                                cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Castle);
                                cmd.Where(Mabinogi.SQL.Columns.Castle.CastleId, _castleID);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.CastleMoney, castleCash + _money);

                                return REPLY_RESULT.SUCCESS;
                            }

                        _remainMoney = result + _money;

                    }
                    WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 길드 머니가 부족합니다.");


                    return REPLY_RESULT.FAIL_EX;
                }
            }
            catch (SimpleSqlException ex)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 트랜잭션을 롤백합니다");
                ExceptionMonitor.ExceptionRaised(ex, _castleID, _guildID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return REPLY_RESULT.ERROR;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 트랜잭션을 롤백합니다");
                ExceptionMonitor.ExceptionRaised(ex2, _castleID, _guildID);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.TakeGuildMoney() : 연결을 종료합니다");
            }
        }

        public REPLY_RESULT GiveGuildMoney(long _castleID, long _guildID, int _money, GuildAdapter _guildAdapter)
        {
            WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 데이터베이스와 연결합니다");
                int castleCash = -1;
                using (var conn = Connection)
                {
                    castleCash = GetCastleCash(_castleID, conn, null);

                    if (castleCash >= _money)
                    {
                        var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Castle);
                        cmd.Where(Mabinogi.SQL.Columns.Castle.CastleId, _castleID);
                        cmd.Set(Mabinogi.SQL.Columns.Castle.CastleMoney, castleCash - _money);

                        cmd.Execute();
                    }
                    else
                    {
                        return REPLY_RESULT.FAIL;
                    }
                }

                WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 성의 돈을 업데이트 합니다.");
                if (_guildAdapter.AddMoney(_guildID, _money))
                {
                    return REPLY_RESULT.SUCCESS;
                }

                WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 성의 돈이 부족합니다.");

                return REPLY_RESULT.FAIL;
            }
            catch (SimpleSqlException ex)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 트랜잭션을 롤백합니다");
                ExceptionMonitor.ExceptionRaised(ex, _castleID, _guildID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return REPLY_RESULT.ERROR;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 트랜잭션을 롤백합니다");
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.ERROR;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.GiveGuildMoney() : 연결을 종료합니다");
            }
        }

        public bool Write(Castle _castle)
        {
            WorkSession.WriteStatus("CastleSqlAdapter.Write() : 함수에 진입하였습니다");
            try
            {
                using (var conn = Connection)
                    try
                    {
                        WorkSession.WriteStatus("CastleSqlAdapter.Write() : 데이터베이스와 연결합니다");
                        var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Castle);
                        cmd.Where(Mabinogi.SQL.Columns.Castle.CastleId, _castle.castleID);
                        bool exists = false;

                        using (var reader = cmd.ExecuteReader())
                            exists = reader.HasRows;

                        if (exists)
                        {
                            cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Castle);

                            cmd.Set(Mabinogi.SQL.Columns.Castle.GuildId, _castle.guildID);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.Constructed, _castle.constructed);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.CastleMoney, _castle.castleMoney);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.WeeklyIncome, _castle.weeklyIncome);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.TaxRate, _castle.taxrate);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.UpdateTime, _castle.updateTime);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.Flag, _castle.flag);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.SellDungeonPass, _castle.sellDungeonPass);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.DungeonPassPrice, _castle.dungeonPassPrice);


                            cmd.Where(Mabinogi.SQL.Columns.Castle.CastleId, _castle.castleID);
                            WorkSession.WriteStatus("CastleSqlAdapter.Write() : 성 정보를 업데이트 합니다.");
                            cmd.Execute();
                        }
                        else
                        {
                            if (ConfigManager.IsFirstRun)
                            {
                                cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Castle);

                                cmd.Set(Mabinogi.SQL.Columns.Castle.GuildId, _castle.guildID);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.Constructed, _castle.constructed);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.CastleMoney, _castle.castleMoney);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.WeeklyIncome, _castle.weeklyIncome);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.TaxRate, _castle.taxrate);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.UpdateTime, _castle.updateTime);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.Flag, _castle.flag);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.SellDungeonPass, _castle.sellDungeonPass);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.DungeonPassPrice, _castle.dungeonPassPrice);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.Durability, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.MaxDurability, 4032000);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.BuildState, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.BuildNextTime, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Castle.BuildStep, 0);

                                cmd.Set(Mabinogi.SQL.Columns.Castle.CastleId, _castle.castleID);
                                cmd.Execute();
                                return true;
                            }
                            Console.WriteLine("GuildId: " + _castle.guildID);
                            Console.WriteLine("Constructed: " + _castle.constructed);
                            Console.WriteLine("CastleMoney: " + _castle.castleMoney);
                            Console.WriteLine("WeeklyIncome: " + _castle.weeklyIncome);
                            Console.WriteLine("TaxRate: " + _castle.taxrate);
                            Console.WriteLine("UpdateTime: " + _castle.updateTime);
                            Console.WriteLine("Flag: " + _castle.flag);
                            Console.WriteLine("SellDungeonPass: " + _castle.sellDungeonPass);
                            Console.WriteLine("DungeonPassPrice: " + _castle.dungeonPassPrice);
                            throw new SimpleSqlException(string.Format("Cannot update castle {0}.Castle does not exist", _castle.castleID));
                        }
                        return true;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, _castle);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("CastleSqlAdapter.Write() : 연결을 종료합니다");
                    }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _castle);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
        }

        public bool UpdateBuild(long _castleID, CastleBuild _build)
        {
            WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuild() : 함수에 진입하였습니다");
            try
            {
                using (var conn = Connection)
                using (var transaction = conn.BeginTransaction())
                    try
                    {
                        WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuild() : 데이터베이스와 연결합니다");

                        var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Castle);
                        cmd.Where(Mabinogi.SQL.Columns.Castle.CastleId, _castleID);

                        if (cmd.ExecuteReader().HasRows)
                        {
                            cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Castle, transaction);

                            cmd.Set(Mabinogi.SQL.Columns.Castle.Durability, _build.durability);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.MaxDurability, _build.maxDurability);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.BuildState, _build.buildState);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.BuildNextTime, _build.buildNextTime);
                            cmd.Set(Mabinogi.SQL.Columns.Castle.BuildStep, _build.buildStep);

                            cmd.Where(Mabinogi.SQL.Columns.Castle.CastleId, _castleID);

                            cmd.Execute();


                            cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBuildResource, transaction);
                            cmd.Where(Mabinogi.SQL.Columns.CastleBuildResource.CastleId, _castleID);

                            cmd.Execute();

                            if (_build.resource != null)
                            {
                                foreach (CastleBuildResource castleBuildResource in _build.resource)
                                {
                                    cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBuildResource, transaction);

                                    cmd.Set(Mabinogi.SQL.Columns.CastleBuildResource.CastleId, _castleID);
                                    cmd.Set(Mabinogi.SQL.Columns.CastleBuildResource.ClassId, castleBuildResource.classID);
                                    cmd.Set(Mabinogi.SQL.Columns.CastleBuildResource.CurrentAmount, castleBuildResource.curAmount);
                                    cmd.Set(Mabinogi.SQL.Columns.CastleBuildResource.MaxAmount, castleBuildResource.maxAmount);

                                    cmd.Execute();
                                }
                            }
                        }
                        WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuild() : 성 건설 상태를 업데이트 합니다.");
                        transaction.Commit();
                        return true;
                    }
                    catch (SimpleSqlException ex)
                    {
                        transaction?.Rollback();
                        ExceptionMonitor.ExceptionRaised(ex, _castleID, _build);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return false;
                    }
                    catch (Exception ex2)
                    {
                        transaction?.Rollback();
                        ExceptionMonitor.ExceptionRaised(ex2, _castleID, _build);
                        WorkSession.WriteStatus(ex2.Message);
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuild() : 연결을 종료합니다");
                    }
            }
            catch (Exception ex3)
            {
                ExceptionMonitor.ExceptionRaised(ex3, _castleID, _build);
                WorkSession.WriteStatus(ex3.Message);
                return false;
            }
        }

        public bool UpdateBuildResource(long _castleID, CastleBuildResource _resource)
        {
            WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuildResource() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuildResource() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                    try
                    {
                        WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuildResource() : 리소스를 업데이트 합니다.");

                        SimpleCommand cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBuildResource);
                        cmd.Where(Mabinogi.SQL.Columns.CastleBuildResource.CastleId, _castleID);
                        cmd.Where(Mabinogi.SQL.Columns.CastleBuildResource.ClassId, _resource.classID);

                        if (cmd.ExecuteReader().HasRows)
                        {
                            cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBuildResource);
                            cmd.Where(Mabinogi.SQL.Columns.CastleBuildResource.CastleId, _castleID);
                            cmd.Where(Mabinogi.SQL.Columns.CastleBuildResource.ClassId, _resource.classID);

                            cmd.Set(Mabinogi.SQL.Columns.CastleBuildResource.CurrentAmount, _resource.curAmount);
                            cmd.Set(Mabinogi.SQL.Columns.CastleBuildResource.MaxAmount, _resource.maxAmount);

                            cmd.Execute();
                        }

                        return true;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, _castleID, _resource);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("CastleSqlAdapter.UpdateBuildResource() : 연결을 종료합니다");
                    }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _castleID, _resource);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
        }

        public bool UpdateBlock(long _castleID, CastleBlock[] _added, CastleBlock[] _deleted)
        {
            WorkSession.WriteStatus("CastleSqlAdapter.UpdateBlock() : 함수에 진입하였습니다");
            try
            {
                if ((_added != null) || (_deleted != null))
                {
                    using (var conn = Connection)
                    {
                        WorkSession.WriteStatus("CastleSqlAdapter.UpdateBlock() : 데이터베이스와 연결합니다");
                        SimpleCommand cmd;
                        if (_added != null)
                        {
                            foreach (CastleBlock castleBlock in _added)
                            {
                                cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBlock);

                                cmd.Set(Mabinogi.SQL.Columns.CastleBlock.CastleId, _castleID);
                                cmd.Set(Mabinogi.SQL.Columns.CastleBlock.GameName, castleBlock.gameName);
                                cmd.Set(Mabinogi.SQL.Columns.CastleBlock.Flag, castleBlock.flag);
                                cmd.Set(Mabinogi.SQL.Columns.CastleBlock.Entry, castleBlock.entry);

                                cmd.Execute();
                            }
                        }
                        if (_deleted != null)
                        {
                            foreach (CastleBlock castleBlock in _deleted)
                            {
                                cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBlock);

                                cmd.Where(Mabinogi.SQL.Columns.CastleBlock.CastleId, _castleID);
                                cmd.Where(Mabinogi.SQL.Columns.CastleBlock.GameName, castleBlock.gameName);
                                cmd.Where(Mabinogi.SQL.Columns.CastleBlock.Flag, castleBlock.flag);
                                cmd.Where(Mabinogi.SQL.Columns.CastleBlock.Entry, castleBlock.entry);

                                cmd.Execute();
                            }
                        }
                    }
                    WorkSession.WriteStatus("CastleSqlAdapter.UpdateBlock() : 생성된 명령이 없어 종료합니다.");
                }
                return true;

            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _castleID, _added, _deleted);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _castleID, _added, _deleted);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("CastleSqlAdapter.UpdateBlock() : 연결을 종료합니다");
            }
        }

        private List<CastleBidder> GetCastleBidders(long castleID, long guildID, SimpleConnection conn)
        {
            List<CastleBidder> result = new List<CastleBidder>();

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBidder))
            {
                if (guildID != 0)
                    cmd.Where(Mabinogi.SQL.Columns.CastleBidder.BidGuildId, guildID);
                cmd.Where(Mabinogi.SQL.Columns.CastleBidder.CastleId, castleID);

                using (var reader = cmd.ExecuteReader())
                {
                    CastleBidder bidder;
                    while (reader.Read())
                    {
                        bidder = new CastleBidder();
                        bidder.bidCharacterID = reader.GetInt64(Mabinogi.SQL.Columns.CastleBidder.BidCharacterID);
                        bidder.bidGuildName = reader.GetString(Mabinogi.SQL.Columns.CastleBidder.BidGuildName);
                        bidder.bidPrice = reader.GetInt32(Mabinogi.SQL.Columns.CastleBidder.BidPrice);
                        bidder.bidOrder = reader.GetInt32(Mabinogi.SQL.Columns.CastleBidder.BidOrder);
                        bidder.bidCharName = reader.GetString(Mabinogi.SQL.Columns.CastleBidder.BidCharName);
                        bidder.bidTime = reader.GetDateTime(Mabinogi.SQL.Columns.CastleBidder.BidTime);
                        bidder.bidUpdateTime = reader.GetDateTime(Mabinogi.SQL.Columns.CastleBidder.BidUpdateTime);
                        bidder.bidGuildID = reader.GetInt64(Mabinogi.SQL.Columns.CastleBidder.BidGuildId);
                        bidder.castleID = castleID;
                        result.Add(bidder);
                    }
                }
            }
            return result;
        }

        private List<CastleBidder> DeleteCastleBidder(long castleID, long guildID, byte flag, SimpleConnection conn)
        {
            var bidders = GetCastleBidders(castleID, guildID, conn);

            foreach (CastleBidder bidder in bidders)
            {
                InsertBidderHistory(bidder, flag, conn);

                using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBidder))
                {
                    if (bidder.bidGuildID != 0)
                        cmd.Where(Mabinogi.SQL.Columns.CastleBidder.BidGuildId, bidder.bidGuildID);
                    cmd.Where(Mabinogi.SQL.Columns.CastleBidder.CastleId, castleID);

                    cmd.Execute();
                }
            }

            return bidders;
        }

        private void InsertBidderHistory(CastleBidder castleBidder, byte flag, SimpleConnection conn)
        {

            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CastleBidderHistory))
            {
                cmd.Set(Mabinogi.SQL.Columns.CastleBidderHistory.CastleId, castleBidder.castleID);
                cmd.Set(Mabinogi.SQL.Columns.CastleBidderHistory.BidGuildId, castleBidder.bidGuildID);
                cmd.Set(Mabinogi.SQL.Columns.CastleBidderHistory.BidGuildName, castleBidder.bidGuildName);
                cmd.Set(Mabinogi.SQL.Columns.CastleBidderHistory.BidPrice, castleBidder.bidPrice);
                cmd.Set(Mabinogi.SQL.Columns.CastleBidderHistory.BidOrder, castleBidder.bidOrder);
                cmd.Set(Mabinogi.SQL.Columns.CastleBidderHistory.BidCharacterID, castleBidder.bidCharacterID);
                cmd.Set(Mabinogi.SQL.Columns.CastleBidderHistory.BidCharName, castleBidder.bidCharName);
                cmd.Set(Mabinogi.SQL.Columns.CastleBidderHistory.BidTime, castleBidder.bidTime);
                cmd.Set(Mabinogi.SQL.Columns.CastleBidderHistory.BidUpdateTime, castleBidder.bidUpdateTime);
                cmd.Set(Mabinogi.SQL.Columns.CastleBidderHistory.RefundTime, DateTime.Now);
                cmd.Set(Mabinogi.SQL.Columns.CastleBidderHistory.Flag, flag);

                cmd.Execute();
            }

        }

        private int GetCastleCash(long castleID)
        {
            using (var conn = Connection)
                return GetCastleCash(castleID, conn, null);
        }

        private int GetCastleCash(long castleID, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Castle, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.Castle.CastleId, castleID);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return reader.GetInt32(Mabinogi.SQL.Columns.Castle.CastleMoney);
                    else
                        return 0;
                }
            }
        }

    }
}