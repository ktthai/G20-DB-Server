using Mabinogi;
using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class EquipmentCollectionAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.EquipCollect;
        public EquipmentCollectionAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        private EquipmentCollection _Read(string _accountId)
        {
            WorkSession.WriteStatus("EquipmentCollectionSqlAdapter._Read() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("EquipmentCollectionSqlAdapter._Read() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var reader = ItemSqlBuilder.GetEquipmentCollectionSelectProc(_accountId, conn))
                {
                    WorkSession.WriteStatus("EquipmentCollectionSqlAdapter._Read() : DataSet 으로부터 의상 수집 시스템 정보를 생성합니다");
                    EquipmentCollection equipmentCollection = EquipmentCollectionObjectBuilder.Build(reader);
                    if (equipmentCollection == null)
                    {
                        equipmentCollection = new EquipmentCollection();
                    }
                    equipmentCollection.Account = _accountId;
                    equipmentCollection.UpdateTime = GetEquipCollectUpdateTime(_accountId, conn);
                    return equipmentCollection;
                }

            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _accountId);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _accountId);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("EquipmentCollectionSqlAdapter._Read() : 연결을 종료합니다");
            }
        }

        public bool IsValidCache(EquipmentCollectionCache _cache)
        {
            WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.IsValidCache() : 함수에 진입하였습니다");
            if (_cache == null || !_cache.IsValid())
            {
                return false;
            }
            try
            {
                try
                {
                    WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.IsValidCache() : 데이터베이스에 연결합니다");
                    using (var conn = Connection)
                    {
                        WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜를 얻어옵니다");
                        var updateTime = GetEquipCollectUpdateTime(_cache.Account, conn);

                        if (UpdateUtility.CacheMissDate.Ticks >= updateTime.Ticks)
                        {
                            WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.IsValidCache() : 캐쉬가 유효하지 않습니다. - by CacheMissDate");
                            return false;
                        }
                        if (updateTime.Ticks <= _cache.UpdateTime.Ticks)
                        {
                            WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜가 캐쉬 날짜보다 빠릅니다. 캐쉬의 정보가 최신입니다");
                            return true;
                        }
                        WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.IsValidCache() : 캐쉬가 유효하지 않습니다.");
                        return false;

                    }
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _cache.Account);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.IsValidCache() : 데이터베이스에 연결을 종료합니다");
                }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _cache.Account);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
        }

        public EquipmentCollection Read(string _accountId, EquipmentCollectionCache _cache)
        {
            WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.Read() : 함수에 진입하였습니다");
            try
            {
                if (_cache == null)
                {
                    EquipmentCollection equipmentCollection = _Read(_accountId);
                    if (equipmentCollection != null && equipmentCollection.IsValid())
                    {
                        _cache = new EquipmentCollectionCache(equipmentCollection);
                    }
                    return equipmentCollection;
                }
                EquipmentCollection equipmentCollection2 = null;
                if (!IsValidCache(_cache))
                {
                    _cache.Invalidate();
                    equipmentCollection2 = _Read(_accountId);
                    _cache.Update(equipmentCollection2);
                }
                else
                {
                    equipmentCollection2 = _cache.ToEquipmentCollection();
                }
                return equipmentCollection2;
            }
            catch (Exception ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _accountId);
                WorkSession.WriteStatus(ex.Message);
                return null;
            }
        }

        private DateTime Update(string _accountId, SimpleConnection conn, SimpleTransaction tran)
        {
            WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.Update() : 시간을 업데이트 합니다.");
            DateTime updateTime = DateTime.Now;

            using (var chkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.EquipCollect))
            {
                chkCmd.Where(Mabinogi.SQL.Columns.EquipCollect.Account, _accountId);

                if (chkCmd.ExecuteReader().HasRows)
                {
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.EquipCollect, tran))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.EquipCollect.Account, _accountId);
                        cmd.Set(Mabinogi.SQL.Columns.EquipCollect.UpdateTime, updateTime);
                    }
                }
                else
                {
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.EquipCollect, tran))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.EquipCollect.Account, _accountId);
                        cmd.Set(Mabinogi.SQL.Columns.EquipCollect.UpdateTime, updateTime);
                    }
                }
            }
            WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.Update() : 의상 수집 시스템이 저장된 시간을 읽어서 리턴합니다.");
            
            return updateTime;
        }

        public bool Write(EquipmentCollection _data, EquipmentCollectionCache _cache)
        {
            WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.Write() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            using (var conn = Connection)
            {
                try
                {

                    WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.Write() : 저장되어 있는 데이터를 읽어옵니다");
                    _cache = ValidateEquipmentCollectionCache(_data, _cache);
                    EquipmentCollectionUpdateBuilder.Build(_data, _cache, conn, transaction);


                    DateTime dateTime = Update(_data.Account, conn, transaction);
                    if (dateTime != DateTime.MinValue)
                    {
                        _cache.Update(_data);
                        _cache.UpdateTime = dateTime;
                        return true;
                    }

                    WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.Write() : 변경점이 없습니다. 쿼리를 생략합니다");
                    transaction.Commit();
                    return true;

                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _data.Account);
                    WorkSession.WriteStatus(ex.Message);
                    transaction?.Rollback();
                    return false;
                }
                catch (Exception ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _data.Account);
                    WorkSession.WriteStatus(ex.Message);
                    transaction?.Rollback();
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.Write() : Closing connection");
                }
            }
        }

        private EquipmentCollectionCache ValidateEquipmentCollectionCache(EquipmentCollection _equipmentCollection, EquipmentCollectionCache _cache)
        {
            if (_cache == null)
            {
                WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.ValidateEquipmentCollectionCache() : 저장되어 있는 의상 수집 시스템 데이터가 없어 새로 만듭니다.");
                return new EquipmentCollectionCache(_Read(_equipmentCollection.Account));
            }
            if (!IsValidCache(_cache))
            {
                _cache.Invalidate();
                _cache.Update(_Read(_equipmentCollection.Account));
            }
            return _cache;
        }

        public bool WriteEx(EquipmentCollection _equipmentCollection, CharacterInfo _character, EquipmentCollectionCache _cache, CharacterInfo _charCache, CharacterAdapter _charAdapter, out Message _outBuildResult)
        {
            return _WriteEx(_equipmentCollection, _character, _cache, _charCache, _charAdapter, _forceUpdate: false, out _outBuildResult);
        }

        private bool _WriteEx(EquipmentCollection _equipmentCollection, CharacterInfo _character, EquipmentCollectionCache _cache, CharacterInfo _charCache, CharacterAdapter _charAdapter, bool _forceUpdate, out Message _outBuildResult)
        {
            WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.WriteEx() : 함수에 진입하였습니다");
            _outBuildResult = new Message();

            try
            {
                WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.WriteEx() : 저장되어 있는 데이터를 읽어옵니다");
                if (_charCache == null)
                {
                    WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.WriteEx() : 저장되어 있는 캐릭터 데이터가 없어 새로 만듭니다.");
                    _charCache = _charAdapter.Read(_character.id, null);
                }
                else if (!_charAdapter.IsValidCache(_charCache))
                {
                    _charCache = _charAdapter.Read(_character.id, null);
                }
                _cache = ValidateEquipmentCollectionCache(_equipmentCollection, _cache);



                WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.WriteEx() : 데이터베이스와 연결합니다");
                SimpleTransaction transaction = null;
                using (var conn = Connection)
                {
                    try
                    {

                        transaction = conn.BeginTransaction();
                        EquipmentCollectionUpdateBuilder.Build(_equipmentCollection, _cache, conn, transaction);
                        InventoryUpdateBuilder.Build(_character.id, _character.inventory, _charCache.inventory, _forceUpdate, conn, transaction, out _character.strToHash);
                        _character.inventoryHash = InventoryHashUtility.ComputeHash(_character.strToHash);

                        _character.updatetime = CharacterUpdateBuilder.Build(_character, _charCache, conn, transaction, out _outBuildResult);

                        DateTime updateTime = Update(_equipmentCollection.Account, conn, transaction);
                        _cache.Update(_equipmentCollection);
                        _equipmentCollection.UpdateTime = updateTime;

                        _charAdapter.UpdateMeta(_character, _charCache, conn, transaction);
                        transaction.Commit();
                        return true;

                    }
                    catch (SimpleSqlException ex)
                    {
                        transaction?.Rollback();
                        if (!_forceUpdate && ItemSqlBuilder.ForceUpdateRetry(ex))
                        {
                            WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.WriteEx() : 아이템 오류로 재시도합니다.");
                            ExceptionMonitor.ExceptionRaised(ex);
                            WorkSession.WriteStatus(ex.Message, ex.Number);
                            _cache.Invalidate();
                            return _WriteEx(_equipmentCollection, _character, _cache, null, _charAdapter, _forceUpdate: true, out _outBuildResult);
                        }
                        ExceptionMonitor.ExceptionRaised(ex);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.WriteEx() : 연결을 종료합니다");
                    }
                }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _equipmentCollection.Account);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
        }

        public CollectionCountList ReadCollectionCountList()
        {
            WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.ReadCollectionCountList() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.ReadCollectionCountList() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetSelectCommand(string.Format("SELECT {0}, {1}, COUNT(*) {2} FROM {3} GROUP BY {0}, {1} ORDER BY {2} DESC",
                    Mabinogi.SQL.Columns.Item.PosX, Mabinogi.SQL.Columns.Item.PosY, Mabinogi.SQL.Columns.Reference.Count, Mabinogi.SQL.Tables.Mabinogi.EquipCollectItemLarge)))
                {
                    WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.ReadCollectionCountList() : DataSet 에 의상 수집 시스템 정보를 채웁니다");
                    WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.ReadCollectionCountList() : DataSet 으로부터 의상 수집 시스템 정보를 생성합니다");
                    return EquipmentCollectionCountListBuilder.Build(cmd.ExecuteReader());
                }
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
                WorkSession.WriteStatus("EquipmentCollectionSqlAdapter.ReadCollectionCountList() : 연결을 종료합니다");
            }
        }

        private DateTime GetEquipCollectUpdateTime(string account, SimpleConnection conn)
        {
            var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.EquipCollect);
            cmd.Where(Mabinogi.SQL.Columns.EquipCollect.Account, account);

            using (var reader = cmd.ExecuteReader())
            {
                var result = DateTime.Now;

                if (reader.Read())
                {
                    result = reader.GetDateTime(Mabinogi.SQL.Columns.EquipCollect.UpdateTime);
                }
                else
                {
                    cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.EquipCollect);
                    cmd.Set(Mabinogi.SQL.Columns.EquipCollect.Account, account);
                    cmd.Set(Mabinogi.SQL.Columns.EquipCollect.UpdateTime, result);
                    cmd.Execute();
                }
                return result;
            }
        }
    }
}
