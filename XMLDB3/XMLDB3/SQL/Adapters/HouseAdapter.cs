using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class HouseAdapter : SqlAdapter
    {
        private string houseGuestBookConnectionString;
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.House;

        public HouseAdapter()
            : base()
        {
            houseGuestBookConnectionString = ConfigManager.GetConnectionString(Mabinogi.SQL.Tables.Mabinogi.GuestBook);

            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        protected SimpleConnection GuestbookConnection
        {
            get
            {
                if (ConfigManager.IsLocalMode)
                    return new SQLiteSimpleConnection(houseGuestBookConnectionString);
                else
                    return new MySqlSimpleConnection(houseGuestBookConnectionString);
            }
        }



        public bool Read(long _houseID, out House _house)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.Read() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("HouseSqlAdapter.Read() : 데이터베이스와 연결합니다");
                // PROCEDURE: SelectHouse
                using (var conn = Connection)
                {
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.House))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.House.HouseId, _houseID);

                        using (var houseReader = cmd.ExecuteReader())
                        {
                            _house = HouseObjectBuilder.BuildHouse(_houseID, houseReader);
                        }

                        if (_house == null)
                            return false;

                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBid))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseBid.HouseId, _houseID);

                        cmd.Set(Mabinogi.SQL.Columns.HouseBid.BidEndTime, 0);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBid.BidRepayEndTime, 0);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBid.MinBidPrice, 0);

                        using (var houseBidReader = cmd.ExecuteReader())
                        {
                            HouseObjectBuilder.HouseBidBuilder(houseBidReader, _house);
                        }
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBidder))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseBidder.HouseId, _houseID);

                        cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidAccount, 0);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidPrice, 0);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidOrder, 0);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidCharName, 0);

                        using (var houseBidderReader = cmd.ExecuteReader())
                        {
                            HouseObjectBuilder.HouseBidderBuilder(houseBidderReader, _house);
                        }
                    }



                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseOwner))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseOwner.HouseId, _houseID);
                        cmd.Set(Mabinogi.SQL.Columns.HouseOwner.Account, 0);

                        using (var houseOwnerReader = cmd.ExecuteReader())
                        {
                            HouseObjectBuilder.HouseOwnerBuilder(houseOwnerReader, _house);
                        }
                    }
                }

                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, ex.Number, _houseID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                _house = null;
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _houseID);
                WorkSession.WriteStatus(ex2.Message);
                _house = null;
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("HouseSqlAdapter.Read() : 연결을 종료합니다");
            }
        }

        public bool CreateBid(long _houseID, HouseBid _bid)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.CreateBid() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("HouseSqlAdapter.CreateBid() : 데이터베이스와 연결합니다");
                // PROCEDURE: CreateHouseBid
                using (var conn = Connection)
                {
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBid))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.HouseBid.HouseId, _houseID);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBid.BidEndTime, _bid.bidEndTime);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBid.BidRepayEndTime, _bid.bidRepayEndTime);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBid.BidStartTime, DateTime.Now);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBid.MinBidPrice, _bid.minBidPrice);

                        WorkSession.WriteStatus("HouseSqlAdapter.CreateBid() : 명령을 실행합니다");
                        cmd.Execute();
                    }
                    return true;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, ex.Number, _houseID, _bid);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _houseID, _bid);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("HouseSqlAdapter.CreateBid() : 연결을 종료합니다");
            }
        }

        public REPLY_RESULT CreateBidder(long _houseID, HouseBidder _bidder, BankAdapter _bankAdapter, out byte _errorCode, out int _remainMoney, out string _strHash)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 함수에 진입하였습니다");
            SimpleTransaction houseTransaction = null;
            SimpleTransaction bankTransaction = null;

            WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 데이터베이스와 연결합니다");
            using (var houseConn = Connection)
            using (var bankConn = _bankAdapter.Connection)
            {
                try
                {

                    WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 집 아이템을 체크합니다.");
                    BidState bidState = CheckBiddableAccount(_bidder.bidAccount);
                    if (bidState != BidState.Biddable)
                    {
                        WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 집에 아이템이 있습니다.");
                        _errorCode = (byte)bidState;
                        _remainMoney = 0;
                        _strHash = string.Empty;
                        return REPLY_RESULT.FAIL_EX;
                    }



                    _remainMoney = _bankAdapter.GetBankDeposit(_bidder.bidAccount, bankConn);
                    if (_remainMoney - _bidder.bidPrice < 0)
                    {
                        WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 은행 잔고가 부족합니다.");
                        _errorCode = 0;
                        _strHash = string.Empty;
                        return REPLY_RESULT.FAIL_EX;
                    }

                    _strHash = InventoryHashUtility.ComputeHash(_bidder.bidAccount, _remainMoney - _bidder.bidPrice);

                    bankTransaction = bankConn.BeginTransaction();
                    WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 은행에서 돈을 인출합니다.");
                    using (var cmd = bankConn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Bank, bankTransaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Bank.Account, _bidder.bidAccount);
                        cmd.Set(Mabinogi.SQL.Columns.Bank.Deposit, _remainMoney - _bidder.bidPrice);
                        cmd.Set(Mabinogi.SQL.Columns.Bank.CouponCode, _strHash);
                        cmd.Execute();
                    }

                    houseTransaction = houseConn.BeginTransaction();
                    // PROCEDURE: CreateHouseBidder
                    using (var cmd = houseConn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBidder, houseTransaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.HouseBidder.HouseId, _houseID);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidAccount, _bidder.bidAccount);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidPrice, _bidder.bidPrice);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidOrder, _bidder.bidOrder);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidCharacter, _bidder.bidCharacter);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidCharName, _bidder.bidCharName);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidTime, DateTime.Now);
                        WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 입찰자를 생성합니다.");
                        cmd.Execute();
                    }


                    _remainMoney -= _bidder.bidPrice;

                    bankTransaction.Commit();
                    houseTransaction.Commit();
                    _errorCode = 3;
                    return REPLY_RESULT.SUCCESS;


                }
                catch (SimpleSqlException ex)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 트랜잭션을 롤백합니다");
                    bankTransaction?.Rollback();
                    houseTransaction?.Rollback();
                    if (ex.Number == 50000)
                    {
                        WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 계정 [" + _bidder.bidAccount + "] 이미 집을 소유하고 있거나, 입찰에 참여중입니다.");
                        _errorCode = 1;
                        _remainMoney = 0;
                        _strHash = string.Empty;
                        return REPLY_RESULT.FAIL_EX;
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _houseID, _bidder);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    _errorCode = 3;
                    _remainMoney = 0;
                    _strHash = string.Empty;
                    return REPLY_RESULT.ERROR;
                }
                catch (Exception ex2)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 트랜잭션을 롤백합니다");
                    bankTransaction?.Rollback();
                    houseTransaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex2, _houseID, _bidder);
                    WorkSession.WriteStatus(ex2.Message);
                    _errorCode = 3;
                    _remainMoney = 0;
                    _strHash = string.Empty;
                    return REPLY_RESULT.ERROR;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.CreateBidder() : 연결을 종료합니다");
                }
            }
        }



        public bool EndBid(long _houseID, string _account, string _server)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.EndBid() : 함수에 진입하였습니다");
            try
            {
                SimpleTransaction transaction = null;

                using (var conn = Connection)
                {
                    try
                    {
                        WorkSession.WriteStatus("HouseSqlAdapter.EndBid() : 데이터베이스와 연결합니다");

                        if (_account == string.Empty)
                            return false;
                        // PROCEDURE: EndHouseBid

                        HouseBidder bidder;
                        using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBidder))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.HouseBidder.HouseId, _houseID);
                            cmd.Where(Mabinogi.SQL.Columns.HouseBidder.BidAccount, _account);

                            cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidPrice, 0);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidOrder, 0);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidCharacter, 0);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidCharName, 0);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidTime, 0);

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    bidder = new HouseBidder();
                                    bidder.bidAccount = _account;
                                    bidder.bidPrice = reader.GetInt32(Mabinogi.SQL.Columns.HouseBidder.BidPrice);
                                    bidder.bidOrder = reader.GetInt32(Mabinogi.SQL.Columns.HouseBidder.BidOrder);
                                    bidder.bidCharacter = reader.GetInt64(Mabinogi.SQL.Columns.HouseBidder.BidCharacter);
                                    bidder.bidCharName = reader.GetString(Mabinogi.SQL.Columns.HouseBidder.BidCharName);
                                    bidder.bidTime = reader.GetDateTime(Mabinogi.SQL.Columns.HouseBidder.BidTime);
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }

                        transaction = conn.BeginTransaction();
                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.HouseOwner, transaction))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.HouseOwner.Account, _account);
                            cmd.Set(Mabinogi.SQL.Columns.HouseOwner.HouseId, _houseID);
                            cmd.Execute();
                        }

                        using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBidder, transaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.HouseBidder.HouseId, _houseID);
                            cmd.Where(Mabinogi.SQL.Columns.HouseBidder.BidAccount, _account);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidder.IsWinner, (byte)1);
                            cmd.Execute();
                        }

                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBidderHistory, transaction))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.HouseId, _houseID);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidAccount, _account);

                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidPrice, bidder.bidPrice);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidOrder, bidder.bidOrder);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidCharacter, bidder.bidCharacter);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidCharName, bidder.bidCharName);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidTime, bidder.bidTime);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.RefundTime, DateTime.Now);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.Flag, (byte)1);

                            cmd.Execute();
                        }

                        WorkSession.WriteStatus("HouseSqlAdapter.EndBid() : 명령을 실행합니다");
                        transaction.Commit();
                        CreateHouseGuestBook(_account, _server);
                        return true;

                    }
                    catch (SimpleSqlException ex)
                    {
                        transaction?.Rollback();
                        ExceptionMonitor.ExceptionRaised(ex, _houseID, _account);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return false;
                    }
                    catch (Exception ex2)
                    {
                        transaction?.Rollback();
                        ExceptionMonitor.ExceptionRaised(ex2, _houseID, _account);
                        WorkSession.WriteStatus(ex2.Message);
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("HouseSqlAdapter.EndBid() : 연결을 종료합니다");
                    }
                }
            }
            catch (Exception ex3)
            {
                ExceptionMonitor.ExceptionRaised(ex3, _houseID, _account);
                WorkSession.WriteStatus(ex3.Message);
                return false;
            }
        }

        private bool CreateHouseGuestBook(string _account, string _server)
        {
            if (houseGuestBookConnectionString != null)
            {
                try
                {
                    // PROCEDURE: CreateGuestBook
                    WorkSession.WriteStatus("HouseSqlAdapter.CreateHouseGuestBook() : 데이터베이스와 연결합니다");
                    using (var conn = GuestbookConnection)
                    {

                        using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.GuestBook))
                        {
                            upCmd.Where(Mabinogi.SQL.Columns.GuestBook.Server, _server);
                            upCmd.Where(Mabinogi.SQL.Columns.GuestBook.Account, _account);

                            upCmd.Set(Mabinogi.SQL.Columns.GuestBook.Valid, 1);


                            WorkSession.WriteStatus("HouseSqlAdapter.CreateHouseGuestBook() : 명령을 실행합니다");
                            if (upCmd.Execute() < 1)
                            {
                                using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.GuestBook))
                                {
                                    insCmd.Set(Mabinogi.SQL.Columns.GuestBook.Server, _server);
                                    insCmd.Set(Mabinogi.SQL.Columns.GuestBook.Account, _account);
                                    insCmd.Set(Mabinogi.SQL.Columns.GuestBook.Message, string.Empty);
                                    insCmd.Set(Mabinogi.SQL.Columns.GuestBook.Valid, 1);
                                    insCmd.Execute();
                                }
                            }
                        }
                    }
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _account);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _account);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.EndBid() : 연결을 종료합니다");
                }
            }
            return false;
        }

        public REPLY_RESULT DeleteBidder(long _houseID, string _account, string _charName, int _repayMoney, BankAdapter _bankAdapter, int _maxRemainMoney, out int _remainMoney, out string _strHash)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 함수에 진입하였습니다");

            SimpleTransaction houseTransaction = null;
            SimpleTransaction bankTransaction = null;
            WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 데이터베이스와 연결합니다");
            using (var houseConn = Connection)
            using (var bankConn = _bankAdapter.Connection)
            {
                try
                {
                    _remainMoney = _bankAdapter.GetBankDeposit(_account, bankConn);

                    _strHash = InventoryHashUtility.ComputeHash(_account, _repayMoney + _remainMoney);

                    if (_remainMoney + _repayMoney > _maxRemainMoney)
                    {
                        WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 은행에 돈이 너무 많습니다.");
                        bankTransaction?.Rollback();
                        houseTransaction?.Rollback();
                        return REPLY_RESULT.FAIL_EX;
                    }

                    bankTransaction = bankConn.BeginTransaction();

                    // PROCEDURE: AddBankDeposit
                    using (var cmd = bankConn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Bank, bankTransaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Bank.Account, _account);
                        cmd.Set(Mabinogi.SQL.Columns.Bank.Deposit, _remainMoney + _repayMoney);
                        cmd.Set(Mabinogi.SQL.Columns.Bank.CouponCode, _strHash);

                        WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder(): 은행에 입금합니다.");
                        if (cmd.Execute() < 1)
                        {
                            WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 은행에 돈이 너무 많습니다.");
                            bankTransaction?.Rollback();
                            houseTransaction?.Rollback();
                            return REPLY_RESULT.FAIL_EX;
                        }
                    }


                    // PROCEDURE: DeleteHouseBidder
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 입찰자를 삭제합니다.");
                    houseTransaction = houseConn.BeginTransaction();

                    if (DeleteHouseBidder(_account, _houseID, houseConn, houseTransaction) == false)
                    {
                        houseTransaction?.Rollback();
                        bankTransaction?.Rollback();
                        return REPLY_RESULT.FAIL_EX;
                    }

                    _remainMoney += _repayMoney;
                    bankTransaction.Commit();
                    houseTransaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                catch (SimpleSqlException ex)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 트랜잭션을 롤백합니다");
                    bankTransaction?.Rollback();
                    houseTransaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex, _houseID, _account);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    _remainMoney = 0;
                    _strHash = string.Empty;
                    return REPLY_RESULT.ERROR;
                }
                catch (Exception ex2)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 트랜잭션을 롤백합니다");
                    bankTransaction?.Rollback();
                    houseTransaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex2, _houseID, _account);
                    WorkSession.WriteStatus(ex2.Message);
                    _remainMoney = 0;
                    _strHash = string.Empty;
                    return REPLY_RESULT.ERROR;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 연결을 종료합니다");
                }
            }
        }

        public bool AutoRepay(long _houseID, string _account, HouseInventory _inventory)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.AutoRepay() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("HouseSqlAdapter.AutoRepay() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {
                    transaction = conn.BeginTransaction();

                    if (_inventory.Items != null)
                    {
                        List<HouseItem> item = _inventory.Items;
                        foreach (HouseItem item2 in item)
                        {
                            ItemSqlBuilder.HouseSelfUpdateItem(_account, item2, conn, transaction);
                        }
                    }
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 명령을 수행합니다.");
                    if (DeleteHouseBidder(_account, _houseID, conn, transaction))
                    {
                        transaction.Commit();

                        return true;
                    }
                    transaction?.Rollback();
                    return false;

                }
                catch (SimpleSqlException ex)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.AutoRepay() : 트랜잭션을 롤백합니다");
                    transaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex, _houseID, _account);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.AutoRepay() : 트랜잭션을 롤백합니다");
                    transaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex2, _houseID, _account);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.AutoRepay() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT EndBidRepay(long _houseID)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.EndBidRepay() : 함수에 진입하였습니다");
            try
            {
                SimpleTransaction transaction = null;
                WorkSession.WriteStatus("HouseSqlAdapter.EndBidRepay() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    try
                    {

                        // PROCEDURE: EndHouseBidRepay
                        HouseBidder bidder;
                        using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBidder))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.HouseBidder.HouseId, _houseID);
                            cmd.Where(Mabinogi.SQL.Columns.HouseBidder.IsWinner, 0);

                            cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidPrice, 0);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidAccount, 0);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidOrder, 0);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidCharacter, 0);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidCharName, 0);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidTime, 0);

                            using (var reader = cmd.ExecuteReader())
                            {

                                if (reader.Read())
                                {
                                    bidder = new HouseBidder();
                                    bidder.bidAccount = reader.GetString(Mabinogi.SQL.Columns.HouseBidder.BidAccount);
                                    bidder.bidPrice = reader.GetInt32(Mabinogi.SQL.Columns.HouseBidder.BidPrice);
                                    bidder.bidOrder = reader.GetInt32(Mabinogi.SQL.Columns.HouseBidder.BidOrder);
                                    bidder.bidCharacter = reader.GetInt64(Mabinogi.SQL.Columns.HouseBidder.BidCharacter);
                                    bidder.bidCharName = reader.GetString(Mabinogi.SQL.Columns.HouseBidder.BidCharName);
                                    bidder.bidTime = reader.GetDateTime(Mabinogi.SQL.Columns.HouseBidder.BidTime);
                                }
                                else
                                {
                                    return REPLY_RESULT.FAIL_EX;
                                }
                            }
                        }

                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBidderHistory, transaction))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.HouseId, _houseID);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidAccount, bidder.bidAccount);

                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidPrice, bidder.bidPrice);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidOrder, bidder.bidOrder);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidCharacter, bidder.bidCharacter);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidCharName, bidder.bidCharName);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidTime, bidder.bidTime);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.RefundTime, DateTime.Now);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.Flag, (byte)0);

                            if (cmd.Execute() < 1)
                            {
                                transaction?.Rollback();
                                return REPLY_RESULT.FAIL_EX;
                            }
                        }

                        WorkSession.WriteStatus("HouseSqlAdapter.EndBidRepay() : 명령을 실행합니다");
                        using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBidder, transaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.HouseBidder.HouseId, _houseID);
                            cmd.Where(Mabinogi.SQL.Columns.HouseBidder.IsWinner, 0);
                            if (cmd.Execute() < 1)
                            {
                                transaction?.Rollback();
                                return REPLY_RESULT.FAIL_EX;
                            }
                        }

                        using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBid, transaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.HouseBid.HouseId, _houseID);
                            if (cmd.Execute() < 1)
                            {
                                transaction?.Rollback();
                                return REPLY_RESULT.FAIL_EX;
                            }
                        }

                        transaction.Commit();
                        return REPLY_RESULT.SUCCESS;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, _houseID);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        transaction?.Rollback();
                        return REPLY_RESULT.ERROR;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("HouseSqlAdapter.EndBidRepay() : 연결을 종료합니다");
                    }
                }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _houseID);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.ERROR;
            }
        }

        public bool Write(House _data)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.Write() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("HouseSqlAdapter.Write() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    // PROCEDURE: SelfUpdateHouse
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseOwner))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseOwner.HouseId, _data.houseID);
                        cmd.Set(Mabinogi.SQL.Columns.HouseOwner.Account, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() != true)
                                return false;

                            var owner = reader.GetString(Mabinogi.SQL.Columns.HouseOwner.Account);

                            if (owner != null && owner != _data.account)
                                return false;
                        }
                    }

                    transaction = conn.BeginTransaction();
                    using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.House, transaction))
                    {
                        upCmd.Where(Mabinogi.SQL.Columns.House.HouseId, _data.houseID);

                        upCmd.Set(Mabinogi.SQL.Columns.House.Constructed, _data.constructed);
                        upCmd.Set(Mabinogi.SQL.Columns.House.UpdateTime, _data.updateTime);
                        upCmd.Set(Mabinogi.SQL.Columns.House.CharName, _data.charName);
                        upCmd.Set(Mabinogi.SQL.Columns.House.HouseName, _data.houseName);
                        upCmd.Set(Mabinogi.SQL.Columns.House.HouseClass, _data.houseClass);
                        upCmd.Set(Mabinogi.SQL.Columns.House.RoofSkin, _data.roofSkin);
                        upCmd.Set(Mabinogi.SQL.Columns.House.RoofColor1, _data.roofColor1);
                        upCmd.Set(Mabinogi.SQL.Columns.House.RoofColor2, _data.roofColor2);
                        upCmd.Set(Mabinogi.SQL.Columns.House.RoofColor3, _data.roofColor3);
                        upCmd.Set(Mabinogi.SQL.Columns.House.WallSkin, _data.wallSkin);
                        upCmd.Set(Mabinogi.SQL.Columns.House.WallColor1, _data.wallColor1);
                        upCmd.Set(Mabinogi.SQL.Columns.House.WallColor2, _data.wallColor2);
                        upCmd.Set(Mabinogi.SQL.Columns.House.WallColor3, _data.wallColor3);
                        upCmd.Set(Mabinogi.SQL.Columns.House.InnerSkin, _data.innerSkin);
                        upCmd.Set(Mabinogi.SQL.Columns.House.InnerColor1, _data.innerColor1);
                        upCmd.Set(Mabinogi.SQL.Columns.House.InnerColor2, _data.innerColor2);
                        upCmd.Set(Mabinogi.SQL.Columns.House.InnerColor3, _data.innerColor3);
                        upCmd.Set(Mabinogi.SQL.Columns.House.Width, _data.width);
                        upCmd.Set(Mabinogi.SQL.Columns.House.Height, _data.height);
                        upCmd.Set(Mabinogi.SQL.Columns.House.BidSuccessDate, _data.bidSuccessDate);
                        upCmd.Set(Mabinogi.SQL.Columns.House.TaxPrevDate, _data.taxPrevDate);
                        upCmd.Set(Mabinogi.SQL.Columns.House.TaxNextDate, _data.taxNextDate);
                        upCmd.Set(Mabinogi.SQL.Columns.House.TaxPrice, _data.taxPrice);
                        upCmd.Set(Mabinogi.SQL.Columns.House.TaxAutopay, _data.taxAutopay);
                        upCmd.Set(Mabinogi.SQL.Columns.House.HouseMoney, _data.houseMoney);
                        upCmd.Set(Mabinogi.SQL.Columns.House.Deposit, _data.deposit);
                        upCmd.Set(Mabinogi.SQL.Columns.House.Flag, _data.flag);

                        if (upCmd.Execute() < 1)
                        {
                            using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.House, transaction))
                            {
                                insCmd.Set(Mabinogi.SQL.Columns.House.HouseId, _data.houseID);
                                insCmd.Set(Mabinogi.SQL.Columns.House.Constructed, _data.constructed);
                                insCmd.Set(Mabinogi.SQL.Columns.House.UpdateTime, _data.updateTime);
                                insCmd.Set(Mabinogi.SQL.Columns.House.CharName, _data.charName);
                                insCmd.Set(Mabinogi.SQL.Columns.House.HouseName, _data.houseName);
                                insCmd.Set(Mabinogi.SQL.Columns.House.HouseClass, _data.houseClass);
                                insCmd.Set(Mabinogi.SQL.Columns.House.RoofSkin, _data.roofSkin);
                                insCmd.Set(Mabinogi.SQL.Columns.House.RoofColor1, _data.roofColor1);
                                insCmd.Set(Mabinogi.SQL.Columns.House.RoofColor2, _data.roofColor2);
                                insCmd.Set(Mabinogi.SQL.Columns.House.RoofColor3, _data.roofColor3);
                                insCmd.Set(Mabinogi.SQL.Columns.House.WallSkin, _data.wallSkin);
                                insCmd.Set(Mabinogi.SQL.Columns.House.WallColor1, _data.wallColor1);
                                insCmd.Set(Mabinogi.SQL.Columns.House.WallColor2, _data.wallColor2);
                                insCmd.Set(Mabinogi.SQL.Columns.House.WallColor3, _data.wallColor3);
                                insCmd.Set(Mabinogi.SQL.Columns.House.InnerSkin, _data.innerSkin);
                                insCmd.Set(Mabinogi.SQL.Columns.House.InnerColor1, _data.innerColor1);
                                insCmd.Set(Mabinogi.SQL.Columns.House.InnerColor2, _data.innerColor2);
                                insCmd.Set(Mabinogi.SQL.Columns.House.InnerColor3, _data.innerColor3);
                                insCmd.Set(Mabinogi.SQL.Columns.House.Width, _data.width);
                                insCmd.Set(Mabinogi.SQL.Columns.House.Height, _data.height);
                                insCmd.Set(Mabinogi.SQL.Columns.House.BidSuccessDate, _data.bidSuccessDate);
                                insCmd.Set(Mabinogi.SQL.Columns.House.TaxPrevDate, _data.taxPrevDate);
                                insCmd.Set(Mabinogi.SQL.Columns.House.TaxNextDate, _data.taxNextDate);
                                insCmd.Set(Mabinogi.SQL.Columns.House.TaxPrice, _data.taxPrice);
                                insCmd.Set(Mabinogi.SQL.Columns.House.TaxAutopay, _data.taxAutopay);
                                insCmd.Set(Mabinogi.SQL.Columns.House.HouseMoney, _data.houseMoney);
                                insCmd.Set(Mabinogi.SQL.Columns.House.Deposit, _data.deposit);
                                insCmd.Set(Mabinogi.SQL.Columns.House.Flag, _data.flag);

                                if (insCmd.Execute() < 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                        }
                    }
                    WorkSession.WriteStatus("HouseSqlAdapter.Write() : 명령을 실행합니다");
                    transaction.Commit();
                    return true;

                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _data);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    transaction?.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    WorkSession.WriteStatus(ex2.Message, _data);
                    ExceptionMonitor.ExceptionRaised(ex2);
                    transaction?.Rollback();
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.Write() : 연결을 종료합니다");
                }
            }
        }

        public HouseInventory ReadInventory(string _account)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.ReadItem() : 함수에 진입하였습니다");

            WorkSession.WriteStatus("HouseSqlAdapter.ReadItem() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {
                    // PROCEDURE: SelectHouseItem
                    HouseInventory houseInventory = new HouseInventory();
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseItemLarge))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, _account);

                        using (var itemLargeReader = cmd.ExecuteReader())
                            HouseInventoryObjectBuilder.BuildItemLarge(itemLargeReader, houseInventory);
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseItemSmall))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, _account);
                        using (var itemSmallReader = cmd.ExecuteReader())
                            HouseInventoryObjectBuilder.BuildItemLarge(itemSmallReader, houseInventory);
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseItemHuge))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, _account);
                        using (var itemHugeReader = cmd.ExecuteReader())
                            HouseInventoryObjectBuilder.BuildItemLarge(itemHugeReader, houseInventory);
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseItemQuest))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, _account);
                        using (var itemQuestReader = cmd.ExecuteReader())
                            HouseInventoryObjectBuilder.BuildItemLarge(itemQuestReader, houseInventory);
                    }

                    WorkSession.WriteStatus("HouseSqlAdapter.ReadItem() : DataSet 에 집 아이템 정보를 채웁니다.");

                    return houseInventory;

                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _account);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return null;
                }
                catch (Exception ex2)
                {
                    WorkSession.WriteStatus(ex2.Message, _account);
                    ExceptionMonitor.ExceptionRaised(ex2);
                    return null;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.ReadItem() : 연결을 종료합니다");
                }
            }
        }

        public bool UpdateItem(string _account, HouseItem _item)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.UpdateItem() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;

            WorkSession.WriteStatus("HouseSqlAdapter.UpdateItem() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();
                    WorkSession.WriteStatus("HouseSqlAdapter.UpdateItem() : 명령을 실행합니다");
                    ItemSqlBuilder.HouseSelfUpdateItem(_account, _item, conn, transaction);


                    transaction.Commit();
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    transaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex, _account, _item);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    transaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex2, _account, _item);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.UpdateItem() : 연결을 종료합니다");
                }
            }
        }

        public bool DeleteItem(long _houseID, string _account, Item _item, int _houseMoney)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.DeleteItem() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("HouseSqlAdapter.DeleteItem() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteItem() : 명령을 실행합니다");
                    ItemSqlBuilder.HouseDeleteItem(_account, _houseID, _item, _houseMoney, conn);
                }

                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account, _houseMoney);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _account, _houseMoney);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("HouseSqlAdapter.DeleteItem() : 연결을 종료합니다");
            }
        }



        public HouseBlockList ReadBlock(long _houseID)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.ReadBlock() : 함수에 진입하였습니다");

            // PROCEDURE: SelectHouseBlock
            try
            {
                WorkSession.WriteStatus("HouseSqlAdapter.ReadBlock() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBlock))
                {
                    cmd.Where(Mabinogi.SQL.Columns.HouseBlock.HouseId, _houseID);
                    cmd.Set(Mabinogi.SQL.Columns.HouseBlock.GameName, 0);
                    cmd.Set(Mabinogi.SQL.Columns.HouseBlock.Flag, 0);

                    WorkSession.WriteStatus("HouseSqlAdapter.ReadBlock() : DataSet 에 집 출입 정보를 채웁니다.");
                    using (var reader = cmd.ExecuteReader())
                        return HouseBlockObjectBuilder.Build(reader);
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _houseID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _houseID);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("HouseSqlAdapter.ReadBlock() : 연결을 종료합니다");
            }
        }

        public bool UpdateBlock(long _houseID, List<HouseBlock> _added, List<HouseBlock> _deleted)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.UpdateBlock() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            using (var conn = Connection)
            {
                try
                {

                    SimpleCommand cmd;

                    if (_added != null)
                    {
                        if (transaction == null)
                            transaction = conn.BeginTransaction();

                        foreach (HouseBlock houseBlock in _added)
                        {
                            cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBlock, transaction);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBlock.HouseId, _houseID);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBlock.GameName, houseBlock.gameName);
                            cmd.Set(Mabinogi.SQL.Columns.HouseBlock.Flag, houseBlock.flag);
                            cmd.Execute();
                        }
                    }
                    if (_deleted != null)
                    {
                        if (transaction == null)
                            transaction = conn.BeginTransaction();

                        foreach (HouseBlock houseBlock in _deleted)
                        {
                            cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBlock, transaction);
                            cmd.Where(Mabinogi.SQL.Columns.HouseBlock.HouseId, _houseID);
                            cmd.Where(Mabinogi.SQL.Columns.HouseBlock.GameName, houseBlock.gameName);
                            cmd.Where(Mabinogi.SQL.Columns.HouseBlock.Flag, houseBlock.flag);
                            cmd.Execute();
                        }
                    }
                    if (transaction != null)
                    {

                        WorkSession.WriteStatus("HouseSqlAdapter.UpdateBlock() : 데이터베이스와 연결합니다");


                        WorkSession.WriteStatus("HouseSqlAdapter.UpdateBlock() : 명령을 실행합니다");
                        transaction.Commit();
                        return true;
                    }

                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _houseID);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    transaction?.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _houseID);
                    WorkSession.WriteStatus(ex2.Message);
                    transaction?.Rollback();
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.UpdateBlock() : 연결을 종료합니다");
                }
            }

            WorkSession.WriteStatus("HouseSqlAdapter.UpdateBlock() : 생성된 명령이 없어 종료합니다.");
            return true;
        }

        public bool DeleteBlock(long _houseID)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.UpdateBlock() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("HouseSqlAdapter.ClearBlock() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {
                    // PROCEDURE: DeleteHouseBlock
                    transaction = conn.BeginTransaction();
                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBlock, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseBlock.HouseId, _houseID);
                        WorkSession.WriteStatus("HouseSqlAdapter.ClearBlock() : 명령을 실행합니다");
                        cmd.Execute();
                    }

                    transaction.Commit();
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _houseID);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    transaction?.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _houseID);
                    WorkSession.WriteStatus(ex2.Message);
                    transaction?.Rollback();
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.ClearBlock() : 연결을 종료합니다");
                }
            }
        }

        public bool DeleteOwner(long _houseID, string _account, string _server)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.DeleteOwner() : 함수에 진입하였습니다.");

            WorkSession.WriteStatus("HouseSqlAdapter.DeleteOwner() : 지울 아이템 목록을 읽습니다.");
            HouseInventory houseInventory = ReadInventory(_account);
            if (houseInventory == null)
            {
                throw new Exception("집 [" + _houseID + "]인벤토리를 읽는데 실패하였습니다.");
            }

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("HouseSqlAdapter.DeleteOwner() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    // PROCEDURE: DeleteHouseOwner
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.House))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.House.HouseId, _houseID);
                        cmd.Set("1", 1);
                        if (cmd.ExecuteReader().HasRows == false)
                            return false;
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseOwner))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseOwner.HouseId, _houseID);
                        cmd.Where(Mabinogi.SQL.Columns.HouseOwner.Account, _account);
                        cmd.Set("1", 1);
                        if (cmd.ExecuteReader().HasRows == false)
                            return false;
                    }

                    transaction = conn.BeginTransaction();

                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.HouseOwner, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseOwner.HouseId, _houseID);
                        cmd.Where(Mabinogi.SQL.Columns.HouseOwner.Account, _account);
                        cmd.Execute();
                    }

                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBidder, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseBidder.HouseId, _houseID);
                        cmd.Where(Mabinogi.SQL.Columns.HouseBidder.BidAccount, _account);
                        cmd.Execute();
                    }

                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.House, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.House.HouseId, _houseID);
                        cmd.Set(Mabinogi.SQL.Columns.House.HouseMoney, 0);
                        cmd.Execute();
                    }

                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBlock, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseBlock.HouseId, _houseID);
                        cmd.Execute();
                    }

                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.HouseItemLarge, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, _account);
                        cmd.Set(Mabinogi.SQL.Columns.HouseItem.Pocket, 2);
                        cmd.Execute();
                    }

                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.HouseItemSmall, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, _account);
                        cmd.Set(Mabinogi.SQL.Columns.HouseItem.Pocket, 2);
                        cmd.Execute();
                    }

                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.HouseItemHuge, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, _account);
                        cmd.Set(Mabinogi.SQL.Columns.HouseItem.Pocket, 2);
                        cmd.Execute();
                    }

                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.HouseItemQuest, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, _account);
                        cmd.Set(Mabinogi.SQL.Columns.HouseItem.Pocket, 2);
                        cmd.Execute();
                    }



                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteOwner() : 명령을 실행합니다");
                    transaction.Commit();
                    DeleteHouseGuestBook(_account, _server);
                    return true;

                }
                catch (SimpleSqlException ex)
                {
                    transaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex, _houseID, _account);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    transaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex2, _houseID, _account);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteOwner() : 연결을 종료합니다");
                }
            }
        }

        private bool DeleteHouseGuestBook(string _account, string _server)
        {
            if (houseGuestBookConnectionString != null)
            {

                try
                {
                    // PROCEDURE: DeleteGuestBook
                    using (var conn = GuestbookConnection)
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.GuestBook))
                    {
                        WorkSession.WriteStatus("HouseSqlAdapter.DeleteHouseGuestBook() : 데이터베이스와 연결합니다");

                        cmd.Where(Mabinogi.SQL.Columns.GuestBook.Account, _account);
                        cmd.Where(Mabinogi.SQL.Columns.GuestBook.Server, _server);
                        cmd.Set(Mabinogi.SQL.Columns.GuestBook.Valid, 0);
                        WorkSession.WriteStatus("HouseSqlAdapter.DeleteHouseGuestBook() : 명령을 실행합니다");
                        cmd.Execute();
                        return true;
                    }
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _account);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _account);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.DeleteHouseGuestBook() : 연결을 종료합니다");
                }
            }
            return false;
        }

        private BidState CheckBiddableAccount(string _account)
        {
            WorkSession.WriteStatus("HouseSqlAdapter.CheckBiddableAccount() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("HouseSqlAdapter.CheckBiddableAccount() : 데이터베이스와 연결합니다");
                // PROCEDURE: CheckBiddableAccount
                using (var conn = Connection)
                {
                    WorkSession.WriteStatus("HouseSqlAdapter.CheckBiddableAccount() : 명령을 실행합니다.");

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseItemLarge))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, _account);
                        cmd.Set(Mabinogi.SQL.Columns.HouseItem.Account, 0);
                        if (cmd.ExecuteReader().HasRows)
                            return BidState.HouseItemExist;
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseItemSmall))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, _account);
                        cmd.Set(Mabinogi.SQL.Columns.HouseItem.Account, 0);
                        if (cmd.ExecuteReader().HasRows)
                            return BidState.HouseItemExist;
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseItemHuge))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, _account);
                        cmd.Set(Mabinogi.SQL.Columns.HouseItem.Account, 0);
                        if (cmd.ExecuteReader().HasRows)
                            return BidState.HouseItemExist;
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseItemQuest))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, _account);
                        cmd.Set(Mabinogi.SQL.Columns.HouseItem.Account, 0);
                        if (cmd.ExecuteReader().HasRows)
                            return BidState.HouseItemExist;
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBidder))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseBidder.BidAccount, _account);
                        cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidAccount, 0);
                        if (cmd.ExecuteReader().HasRows)
                            return BidState.HouseBidderExist;
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseOwner))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.HouseOwner.Account, _account);
                        cmd.Set(Mabinogi.SQL.Columns.HouseOwner.Account, 0);
                        if (cmd.ExecuteReader().HasRows)
                            return BidState.HouseBidderExist;
                    }

                    return BidState.Biddable;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return BidState.Unknown;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message, _account);
                ExceptionMonitor.ExceptionRaised(ex2);
                return BidState.Unknown;
            }
            finally
            {
                WorkSession.WriteStatus("HouseSqlAdapter.CheckBiddableAccount() : 연결을 종료합니다");
            }


        }

        public bool DeleteHouseBidder(string account, long houseID, SimpleConnection conn, SimpleTransaction transaction)
        {
            // PROCEDURE: DeleteHouseBidder

            WorkSession.WriteStatus("HouseSqlAdapter.DeleteBidder() : 입찰자를 삭제합니다.");

            HouseBidder bidder;
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBidder))
            {
                cmd.Where(Mabinogi.SQL.Columns.HouseBidder.HouseId, houseID);
                cmd.Where(Mabinogi.SQL.Columns.HouseBidder.BidAccount, account);

                cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidPrice, 0);
                cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidOrder, 0);
                cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidCharacter, 0);
                cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidCharName, 0);
                cmd.Set(Mabinogi.SQL.Columns.HouseBidder.BidTime, 0);

                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        bidder = new HouseBidder();
                        bidder.bidAccount = account;
                        bidder.bidPrice = reader.GetInt32(Mabinogi.SQL.Columns.HouseBidder.BidPrice);
                        bidder.bidOrder = reader.GetInt32(Mabinogi.SQL.Columns.HouseBidder.BidOrder);
                        bidder.bidCharacter = reader.GetInt64(Mabinogi.SQL.Columns.HouseBidder.BidCharacter);
                        bidder.bidCharName = reader.GetString(Mabinogi.SQL.Columns.HouseBidder.BidCharName);
                        bidder.bidTime = reader.GetDateTime(Mabinogi.SQL.Columns.HouseBidder.BidTime);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBidderHistory, transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.HouseId, houseID);
                cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidAccount, account);

                cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidPrice, bidder.bidPrice);
                cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidOrder, bidder.bidOrder);
                cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidCharacter, bidder.bidCharacter);
                cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidCharName, bidder.bidCharName);
                cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.BidTime, bidder.bidTime);
                cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.RefundTime, DateTime.Now);
                cmd.Set(Mabinogi.SQL.Columns.HouseBidderHistory.Flag, (byte)2);

                cmd.Execute();
            }

            using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.HouseBidder, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.HouseBidder.BidAccount, account);
                cmd.Where(Mabinogi.SQL.Columns.HouseBidder.HouseId, houseID);
                cmd.Execute();
            }

            return true;
        }
    }


}


