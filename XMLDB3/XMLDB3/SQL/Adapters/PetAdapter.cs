using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class PetAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Pet;

        public PetAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        private PetInfo GetPet(long _id)
        {
            WorkSession.WriteStatus("PetSqlAdapter._Read() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("PetSqlAdapter._Read() : 데이터베이스와 연결합니다");
                long timestamp = Stopwatch.GetTimestamp();
                // PROCEDURE: SelectPet
                using (var conn = Connection)
                {
                    WorkSession.WriteStatus("PetSqlAdapter._Read() : DataSet 에 펫 정보를 채웁니다");
                    
                    CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctPetRead, Stopwatch.GetElapsedMilliseconds(timestamp));
                    WorkSession.WriteStatus("PetSqlAdapter._Read() : DataSet 으로부터 펫 정보를 생성합니다");
                    return PetObjectBuilder.Build(_id, conn);
                }
            }
            finally
            {
                WorkSession.WriteStatus("PetSqlAdapter._Read() : 연결을 종료합니다");
            }
        }

        public PetInfo Read(string _account, string _server, long _id, PetInfo _cache, AccountRefAdapter _accountref)
        {
            WorkSession.WriteStatus("PetSqlAdapter.Read() : 함수에 진입하였습니다");
            PetInfo petInfo = null;
            try
            {

                if (_cache != null && IsValidCache(_cache))
                {
                    petInfo = _cache;
                }
                else
                {
                    petInfo = GetPet(_id);
                }

                if (_account != null && _server != null)
                {
                    WorkSession.WriteStatus("PetSqlAdapter.Read() : 펫의 소환 시간을 확인합니다.");

                    try
                    {
                        WorkSession.WriteStatus("PetSqlAdapter.Read() : 데이터베이스와 연결합니다");
                        // PROCEDURE: GetPetSummonTime
                        using (var conn = Connection)
                        using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.AccountPetRef))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.AccountPetRef.Id, _account);
                            cmd.Where(Mabinogi.SQL.Columns.AccountPetRef.PetId, _id);
                            cmd.Where(Mabinogi.SQL.Columns.AccountPetRef.Server, _server);

                            cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.RemainTime, 0);
                            cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.LastTime, 0);
                            cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.ExpireTime, 0);

                            WorkSession.WriteStatus("PetSqlAdapter.Read() : DataSet 에 펫 정보를 채웁니다");
                            using (var reader = cmd.ExecuteReader())
                            {

                                if (reader.Read() == false)
                                {
                                    throw new Exception("PetSqlAdapter.Read() : [" + _id + "/" + _account + "/" + _server + "] 소환 정보를 가져오지 못하였습니다.");
                                }

                                petInfo.summon.remaintime = reader.GetInt32(Mabinogi.SQL.Columns.AccountPetRef.RemainTime);
                                petInfo.summon.lasttime = reader.GetInt64(Mabinogi.SQL.Columns.AccountPetRef.LastTime);
                                petInfo.summon.expiretime = reader.GetInt64(Mabinogi.SQL.Columns.AccountPetRef.ExpireTime);
                                return petInfo;
                            }
                        }
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, _id);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return null;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("PetSqlAdapter.Read() : 데이터베이스에 연결을 종료합니다");
                    }
                }
                WorkSession.WriteStatus("PetSqlAdapter.Read() : 계정, 서버 정보가 없어 소환 시간을 가져 오지 않습니다.");
                return petInfo;
            }
            catch (SimpleSqlException ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _id);
                WorkSession.WriteStatus(ex2.Message, ex2.Number);
                return null;
            }
            catch (Exception ex3)
            {
                ExceptionMonitor.ExceptionRaised(ex3, _id);
                WorkSession.WriteStatus(ex3.Message);
                return null;
            }
        }

        public bool IsValidCache(PetInfo _cache)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 함수에 진입하였습니다");
            if (_cache == null)
            {
                return false;
            }
            try
            {


                try
                {
                    WorkSession.WriteStatus("PetSqlAdapter.IsValidCache() : 데이터베이스에 연결합니다");
                    using (var conn = Connection)
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Pet))
                    {
                        // PROCEDURE: GetPetUpdateTime
                        cmd.Where(Mabinogi.SQL.Columns.Pet.Id, _cache.id);
                        cmd.Set(Mabinogi.SQL.Columns.Pet.UpdateTime, 0);

                        WorkSession.WriteStatus("PetSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜를 얻어옵니다");
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DateTime updateTime;
                                if (reader.GetDateTimeSafe(Mabinogi.SQL.Columns.Pet.UpdateTime, out updateTime) && updateTime.Ticks <= _cache.updatetime.Ticks)
                                {
                                    WorkSession.WriteStatus("PetSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜가 캐쉬 날짜보다 빠릅니다. 캐쉬의 정보가 최신입니다");
                                    return true;
                                }
                                WorkSession.WriteStatus("PetSqlAdapter.IsValidCache() : 캐쉬가 유효하지 않습니다.");
                                return false;
                            }
                        }
                    }
                    return false;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _cache);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("PetSqlAdapter.IsValidCache() : 데이터베이스에 연결을 종료합니다");
                }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _cache);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
        }

        public bool Write(string _account, string _server, byte _channelgroupid, PetInfo _pet, PetInfo _cache, AccountRefAdapter _accountref)
        {
            return _Write(_account, _server, _channelgroupid, _pet, _cache, _accountref, _forceLinkUpdate: false);
        }

        private bool _Write(string _account, string _server, byte _channelgroupid, PetInfo _pet, PetInfo _cache, AccountRefAdapter _accountref, bool _forceLinkUpdate)
        {
            WorkSession.WriteStatus("PetSqlAdapter.Write() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("PetSqlAdapter.Write() : 비교본으로 사용할 수 있는 유효한 저장 데이터를 읽어옵니다");
                if (_cache == null)
                {
                    WorkSession.WriteStatus("PetSqlAdapter.Write() : 저장되어 있는 데이터가 없어 새로 만듭니다.");
                    _cache = GetPet(_pet.id);
                }
                else if (!IsValidCache(_cache))
                {
                    _cache = GetPet(_pet.id);
                }



                SimpleTransaction petTransaction = null;
                SimpleTransaction accRefTransaction = null;

                WorkSession.WriteStatus("PetSqlAdapter.Write() : 소환시간을 기록합니다.");
                using (var petConn = Connection)
                using (var accRefConn = _accountref.Connection)
                {
                    try
                    {
                        long timestamp = Stopwatch.GetTimestamp();
                        WorkSession.WriteStatus("PetSqlAdapter.Write() : SQL 명령문을 생성합니다");
                        InventoryUpdateBuilder.Build(_pet.id, _pet.inventory, _cache.inventory, _forceLinkUpdate, petConn, petTransaction, out _pet.strToHash);
                        _pet.inventoryHash = InventoryHashUtility.ComputeHash(_pet.strToHash);

                        petTransaction = petConn.BeginTransaction();

                        PetUpdateBuilder.Build(_pet, _cache, petConn, petTransaction);

                        accRefTransaction = accRefConn.BeginTransaction();

                        // PROCEDURE: UpdatePetSummonTime
                        using (var cmd = accRefConn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountPetRef, accRefTransaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.AccountPetRef.Id, _account);
                            cmd.Where(Mabinogi.SQL.Columns.AccountPetRef.Server, _server);
                            cmd.Where(Mabinogi.SQL.Columns.AccountPetRef.PetId, _pet.id);

                            cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.GroupId, _channelgroupid);
                            cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.RemainTime, _pet.summon.remaintime);
                            cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.LastTime, _pet.summon.lasttime);

                            if (cmd.Execute() < 1)
                            {
                                throw new SimpleSqlException("Failed to update pet summon time");
                            }
                        }
                        WorkSession.WriteStatus("PetSqlAdapter.Write() : 트랜잭션을 커밋합니다");
                        petTransaction?.Commit();
                        accRefTransaction?.Commit();
                        CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctPetWrite, Stopwatch.GetElapsedMilliseconds(timestamp));
                        return true;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, _pet);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        petTransaction?.Rollback();
                        accRefTransaction?.Rollback();
                        if (!_forceLinkUpdate && ItemSqlBuilder.ForceUpdateRetry(ex))
                        {
                            WorkSession.WriteStatus("PetSqlAdapter.Write() : 아이템 오류로 재시도합니다.");
                            return _Write(_account, _server, _channelgroupid, _pet, null, _accountref, _forceLinkUpdate: true);
                        }
                        return false;
                    }
                    catch (Exception ex2)
                    {
                        ExceptionMonitor.ExceptionRaised(ex2, _pet);
                        WorkSession.WriteStatus(ex2.Message);
                        petTransaction?.Rollback();
                        accRefTransaction?.Rollback();
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("PetSqlAdapter.Write() : 연결을 종료합니다");
                    }
                }
            }
            catch (SimpleSqlException ex3)
            {
                ExceptionMonitor.ExceptionRaised(ex3);
                WorkSession.WriteStatus(ex3.Message, _pet);
                return false;
            }
            catch (Exception ex4)
            {
                ExceptionMonitor.ExceptionRaised(ex4);
                WorkSession.WriteStatus(ex4.Message, _pet);
                return false;
            }
        }

        public bool CreateEx(string _account, string _server, PetInfo _pet, AccountRefAdapter _accountref, WebSynchAdapter _websynch)
        {
            WorkSession.WriteStatus("PetSqlAdapter.Create() : 함수에 진입하였습니다");

            SimpleTransaction petTransaction = null;
            SimpleTransaction accRefTransaction = null;
            SimpleTransaction webSyncTransaction = null;

            using (var petConn = Connection)
            using (var accRefConn = _accountref.Connection)
            using (var webConn = _websynch.Connection)
            {
                try
                {
                    WorkSession.WriteStatus("PetSqlAdapter.Create() : 데이터베이스와 연결합니다");


                    // PROCEDURE: CheckUsableName
                    using (var cmd = petConn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.NotUsableGameId))
                    {

                        cmd.Where(Mabinogi.SQL.Columns.NotUsableGameId.Name, _pet.name);
                        bool result;

                        using (var reader = cmd.ExecuteReader())
                            result = reader.HasRows;

                        if (result == false)
                        {
                            using (var cmd1 = petConn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.GameId))
                            {
                                cmd1.Where(Mabinogi.SQL.Columns.GameId.Name, _pet.name);
                                using (var reader = cmd1.ExecuteReader())
                                    result = reader.HasRows;
                                
                            }
                        }

                        WorkSession.WriteStatus("PetSqlAdapter.CreateEx() : 펫 이름 중복 검사를 실행합니다");
                        if (result)
                        {
                            ExceptionMonitor.ExceptionRaised(new Exception("펫 이름이 중복됩니다."), _account, _pet.name);
                            return false;
                        }
                    }

                    petTransaction = petConn.BeginTransaction();
                    accRefTransaction = accRefConn.BeginTransaction();
                    webSyncTransaction = webConn.BeginTransaction();

                    PetCreateBuilder.Build(_pet, petConn, petTransaction);


                    WorkSession.WriteStatus("PetSqlAdapter.Create() : 계정-펫 링크 생성 명령을 실행합니다");
                    // PROCEDURE: AddAccountrefPet
                    using (var cmd = petConn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.AccountPetRef, petTransaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.Id, _account);
                        cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.PetId, _pet.id);
                        cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.PetName, _pet.name);
                        cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.Server, _server);
                        cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.Deleted, 0);
                        cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.RemainTime, _pet.summon.remaintime);
                        cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.LastTime, _pet.summon.lasttime);
                        cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.GroupId, 0);
                        cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.ExpireTime, _pet.summon.expiretime);


                        cmd.Execute();
                    }

                    WorkSession.WriteStatus("PetSqlAdapter.Create() : 트랜잭션을 커밋합니다");
                    petTransaction.Commit();
                    accRefTransaction.Commit();
                    webSyncTransaction.Commit();
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    petTransaction?.Rollback();
                    accRefTransaction?.Rollback();
                    webSyncTransaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    WorkSession.WriteStatus("PetSqlAdapter.Create() : 트랜잭션을 롤백합니다");
                    return false;
                }
                catch (Exception ex2)
                {
                    petTransaction?.Rollback();
                    accRefTransaction?.Rollback();
                    webSyncTransaction?.Rollback();
                    WorkSession.WriteStatus(ex2.Message);
                    ExceptionMonitor.ExceptionRaised(ex2);
                    WorkSession.WriteStatus("PetSqlAdapter.Create() : 트랜잭션을 롤백합니다");
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("PetSqlAdapter.Create() : 연결을 종료합니다");
                }
            }
        }

        public bool DeleteEx(string _account, string _server, long _idPet, AccountRefAdapter _accountref, WebSynchAdapter _websynch)
        {
            WorkSession.WriteStatus("PetSqlAdapter.Delete() : 함수에 진입하였습니다");


            SimpleTransaction petTransaction = null;
            SimpleTransaction accRefTransaction = null;
            SimpleTransaction webSyncTransaction = null;

            WorkSession.WriteStatus("PetSqlAdapter.Delete() : 데이터베이스와 연결합니다");
            using (var petConn = Connection)
            using (var accRefConn = _accountref.Connection)
            using (var webSyncConn = _websynch.Connection)
            {
                try
                {


                    petTransaction = petConn.BeginTransaction();
                    accRefTransaction = accRefConn.BeginTransaction();
                    webSyncTransaction = webSyncConn.BeginTransaction();

                    WorkSession.WriteStatus("PetSqlAdapter.Delete() : 계정-펫 링크를 제거명령을 실행합니다");
                    // PROCEDURE: DeleteAccountrefPet

                    using (var cmd = accRefConn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.AccountPetRef, accRefTransaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.AccountPetRef.Id, _account);
                        cmd.Where(Mabinogi.SQL.Columns.AccountPetRef.PetId, _idPet);
                        cmd.Where(Mabinogi.SQL.Columns.AccountPetRef.Server, _server);

                        if (cmd.Execute() != 1)
                        {
                            throw new Exception("account_pet_ref 테이블에서 [" + _idPet + "/" + _account + "/" + _server + "] 열을 찾을 수 없습니다.");
                        }
                    }

                    WorkSession.WriteStatus("PetSqlAdapter.Delete() : 캐릭터 제거 명령을 실행합니다");
                    // PROCEDURE: DeletePet
                    {
                        byte flag;
                        string name;
                        using (var cmd = petConn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.GameId, petTransaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.GameId.Id, _idPet);
                            cmd.Set(Mabinogi.SQL.Columns.GameId.Name, 0);
                            cmd.Set(Mabinogi.SQL.Columns.GameId.Flag, 0);

                            using (var reader = cmd.ExecuteReader())
                            {
                                name = reader.GetString(Mabinogi.SQL.Columns.GameId.Name);
                                flag = reader.GetByte(Mabinogi.SQL.Columns.GameId.Flag);
                            }
                        }

                        using (var cmd = petConn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.NotUsableGameId, petTransaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.NotUsableGameId.Name, name);

                            bool result;

                            using (var reader = cmd.ExecuteReader())
                                result = reader.HasRows;

                            if (result == false)
                            {
                                using (var insCmd = petConn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.NotUsableGameId, petTransaction))
                                {
                                    insCmd.Set(Mabinogi.SQL.Columns.NotUsableGameId.Name, name);
                                    insCmd.Set(Mabinogi.SQL.Columns.NotUsableGameId.Id, _idPet);
                                    insCmd.Set(Mabinogi.SQL.Columns.NotUsableGameId.Flag, flag);
                                    insCmd.Set(Mabinogi.SQL.Columns.NotUsableGameId.InsertDate, DateTime.Now);
                                    insCmd.Execute();
                                }
                            }
                        }

                        using (var cmd = petConn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.GameId, petTransaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.GameId.Id, _idPet);
                            cmd.Set(Mabinogi.SQL.Columns.GameId.Name, name + _idPet);
                            cmd.Execute();
                        }

                        using (var cmd = petConn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Pet, petTransaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.Pet.Id, _idPet);
                            cmd.Set(Mabinogi.SQL.Columns.Pet.Name, 0);

                            using (var reader = cmd.ExecuteReader())
                            {
                                name = reader.GetString(Mabinogi.SQL.Columns.Pet.Name);
                            }
                        }

                        using (var cmd = petConn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Pet, petTransaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.Pet.Id, _idPet);
                            cmd.Set(Mabinogi.SQL.Columns.Pet.Name, name + _idPet);
                            cmd.Execute();
                        }
                    }

                    WorkSession.WriteStatus("PetSqlAdapter.Delete() : 웹 싱크 명령을 실행합니다");
                    // PROCEDURE: WebSynch_RemoveCharacter
                    {
                        using (var cmd = webSyncConn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharRefSync, webSyncTransaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.CharRefSync.CharacterId, _idPet);
                            cmd.Where(Mabinogi.SQL.Columns.CharRefSync.Id, _account);
                            cmd.Where(Mabinogi.SQL.Columns.CharRefSync.Server, _server);

                            bool result;

                            using (var reader = cmd.ExecuteReader())
                                result = reader.HasRows;

                            if (result )
                            {
                                using (var insCmd = webSyncConn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharDeletedRefSync, webSyncTransaction))
                                {
                                    insCmd.Set(Mabinogi.SQL.Columns.CharDeletedRefSync.CharacterId, _idPet);
                                    insCmd.Set(Mabinogi.SQL.Columns.CharDeletedRefSync.Id, _account);
                                    insCmd.Set(Mabinogi.SQL.Columns.CharDeletedRefSync.Server, _server);
                                    insCmd.Set(Mabinogi.SQL.Columns.CharDeletedRefSync.DeletedTime, DateTime.Now);
                                    insCmd.Execute();
                                }
                                using (var delCmd = webSyncConn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CharRefSync, webSyncTransaction))
                                {
                                    delCmd.Where(Mabinogi.SQL.Columns.CharRefSync.CharacterId, _idPet);
                                    delCmd.Where(Mabinogi.SQL.Columns.CharRefSync.Id, _account);
                                    delCmd.Where(Mabinogi.SQL.Columns.CharRefSync.Server, _server);

                                    delCmd.Execute();
                                }

                            }
                        }
                    }

                    WorkSession.WriteStatus("PetSqlAdapter.Delete() : 트랜잭션을 커밋합니다");
                    petTransaction.Commit();
                    accRefTransaction.Commit();
                    webSyncTransaction.Commit();
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    petTransaction?.Rollback();
                    accRefTransaction?.Rollback();
                    webSyncTransaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    ExceptionMonitor.ExceptionRaised(new Exception("[" + ex.Number + "]" + ex.Message));
                    WorkSession.WriteStatus("PetSqlAdapter.Delete() : 트랜잭션을 롤백합니다");
                    return false;
                }
                catch (Exception ex2)
                {
                    petTransaction?.Rollback();
                    accRefTransaction?.Rollback();
                    webSyncTransaction?.Rollback();
                    WorkSession.WriteStatus(ex2.Message);
                    ExceptionMonitor.ExceptionRaised(ex2);
                    WorkSession.WriteStatus("PetSqlAdapter.Delete() : 트랜잭션을 롤백합니다");
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("PetSqlAdapter.Delete() : 연결을 종료합니다");
                }
            }
        }

        public bool DeleteItem(long _id, ItemList[] _list, ref PetInfo _cache)
        {
            WorkSession.WriteStatus("PetSqlAdapter.DeleteItem() : 함수에 진입하였습니다");
            try
            {
                if (_cache == null)
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteItem() : 저장되어 있는 데이터가 없어 새로 만듭니다.");
                    _cache = GetPet(_id);
                }
                else if (!IsValidCache(_cache))
                {
                    _cache = GetPet(_id);
                }

                bool flag = false;
                InventoryHash inventoryHash = new InventoryHash(_id);
                if (_cache != null && _cache.inventory != null)
                {
                    flag = true;
                    inventoryHash.Parse(_cache.strToHash);
                }

                if (_list != null && _list.Length > 0)
                {
                    SimpleTransaction transaction = null;
                    using (var conn = Connection)
                    {
                        try
                        {
                            long timestamp = Stopwatch.GetTimestamp();
                            transaction = conn.BeginTransaction();
                            foreach (ItemList itemList in _list)
                            {
                                ItemSqlBuilder.DeleteItem(_id, itemList.itemID, itemList.storedtype, conn, transaction);
                                if (_cache != null && _cache.inventory != null && _cache.inventory.ContainsKey(itemList.itemID))
                                {
                                    Item item = (Item)_cache.inventory[itemList.itemID];
                                    inventoryHash.Remove(item);
                                    _cache.inventory.Remove(itemList.itemID);
                                }
                            }

                            WorkSession.WriteStatus("PetSqlAdapter.DeleteItem() : 데이터베이스와 연결합니다");

                            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Pet, transaction))
                            {
                                cmd.Where(Mabinogi.SQL.Columns.Pet.Id, _id);
                                if (flag)
                                {
                                    cmd.Set(Mabinogi.SQL.Columns.Pet.UpdateTime, DateTime.Now);
                                    cmd.Set(Mabinogi.SQL.Columns.Pet.CouponCode, inventoryHash.Compute());
                                }
                                else
                                {
                                    cmd.Set(Mabinogi.SQL.Columns.Pet.UpdateTime, UpdateUtility.CacheMissDate.AddDays(-1.0));
                                    cmd.Set(Mabinogi.SQL.Columns.Pet.CouponCode, inventoryHash.ComputeInitial());
                                }
                                WorkSession.WriteStatus("PetSqlAdapter.DeleteItem() : 명령을 수행합니다.");
                                cmd.Execute();
                            }

                                transaction.Commit();
                                CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctCharItemDelete, Stopwatch.GetElapsedMilliseconds(timestamp));
                                return true;
                            }
                        catch (SimpleSqlException ex)
                        {
                            ExceptionMonitor.ExceptionRaised(ex, _list);
                            WorkSession.WriteStatus(ex.Message, ex.Number);
                            transaction?.Rollback();
                            return false;
                        }
                        catch (Exception ex2)
                        {
                            ExceptionMonitor.ExceptionRaised(ex2, _list);
                            WorkSession.WriteStatus(ex2.Message);
                            transaction?.Rollback();
                            return false;
                        }
                        finally
                        {
                            WorkSession.WriteStatus("PetSqlAdapter.DeleteItem() : 연결을 종료합니다");

                        }
                    }
                }
                WorkSession.WriteStatus("PetSqlAdapter.DeleteItem() : 삭제할 아이템이 없습니다.");
                return true;
            }
            catch (Exception ex3)
            {
                WorkSession.WriteStatus(ex3.Message);
                ExceptionMonitor.ExceptionRaised(ex3);
                return false;
            }
        }

        public bool GetWriteCounter(long _id, out byte _counter)
        {
            WorkSession.WriteStatus("PetSqlAdapter.GetWriteCounter() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("PetSqlAdapter.GetWriteCounter() : 데이터베이스와 연결합니다");
                // PROCEDURE: GetPetWriteCounter
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Pet))
                {
                    cmd.Where(Mabinogi.SQL.Columns.Pet.Id, _id);
                    cmd.Set(Mabinogi.SQL.Columns.Pet.WriteCounter, 0);

                    using (var reader = cmd.ExecuteReader())
                    {
                        _counter = reader.GetByte(Mabinogi.SQL.Columns.Pet.WriteCounter);
                    }
                }

                WorkSession.WriteStatus("PetSqlAdapter.GetWriteCounter() : 저장 카운터를 읽었습니다.");
                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _id);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                _counter = 0;
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _id);
                WorkSession.WriteStatus(ex2.Message, _id);
                _counter = 0;
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("PetSqlAdapter.GetWriteCounter() : 연결을 종료합니다");
            }
        }
    }
}
