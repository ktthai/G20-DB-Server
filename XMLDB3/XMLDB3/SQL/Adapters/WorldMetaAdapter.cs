using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class WorldMetaAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.WorldMeta;
        public WorldMetaAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public WorldMetaList Read()
        {
            WorkSession.WriteStatus("WorldMetaSqlAdapter.Read() : 함수에 진입하였습니다.");

            try
            {
                WorkSession.WriteStatus("WorldMetaSqlAdapter.Read() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.WorldMeta))
                using (var reader = cmd.ExecuteReader())
                {
                    WorldMetaList worldMetaList = new WorldMetaList();
                    // PROCEDURE: WorldMetaSelect
                    if (reader.HasRows)
                    {
                        WorldMeta worldMeta;
                        worldMetaList.metas = new List<WorldMeta>();
                        while (reader.Read())
                        {
                            worldMeta = new WorldMeta();
                            worldMeta.key = reader.GetString(Mabinogi.SQL.Columns.WorldMeta.MetaKey);
                            worldMeta.type = reader.GetByte(Mabinogi.SQL.Columns.WorldMeta.MetaType);
                            worldMeta.value = reader.GetString(Mabinogi.SQL.Columns.WorldMeta.MetaValue);

                            worldMetaList.metas.Add(worldMeta);
                        }
                    }
                    return worldMetaList;

                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
            }
            finally
            {
                WorkSession.WriteStatus("WorldMetaSqlAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");
            }
            return null;
        }

        public REPLY_RESULT UpdateList(WorldMetaList _worldmetaUpdateList, string[] _removeKeys, ref byte _errorCode)
        {
            WorkSession.WriteStatus("WorldMetaSqlAdapter.UpdateList() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("WorldMetaSqlAdapter.UpdateList() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    SimpleTransaction sqlTransaction = null;
                    try
                    {
                        sqlTransaction = conn.BeginTransaction();
                        if (_worldmetaUpdateList != null && _worldmetaUpdateList.metas != null)
                        {
                            foreach (WorldMeta worldmeta in _worldmetaUpdateList.metas)
                            {
                                // PROCEDURE: worldmetaUpdate
                                using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.WorldMeta, sqlTransaction))
                                {
                                    upCmd.Where(Mabinogi.SQL.Columns.WorldMeta.MetaKey, worldmeta.key);

                                    upCmd.Set(Mabinogi.SQL.Columns.WorldMeta.MetaType, worldmeta.type);
                                    upCmd.Set(Mabinogi.SQL.Columns.WorldMeta.MetaValue, worldmeta.value);

                                    if (upCmd.Execute() < 1)
                                    {
                                        using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.WorldMeta, sqlTransaction))
                                        {
                                            insCmd.Set(Mabinogi.SQL.Columns.WorldMeta.MetaKey, worldmeta.key);
                                            insCmd.Set(Mabinogi.SQL.Columns.WorldMeta.MetaType, worldmeta.type);
                                            insCmd.Set(Mabinogi.SQL.Columns.WorldMeta.MetaValue, worldmeta.value);

                                            insCmd.Execute();
                                        }

                                    }
                                }
                            }
                        }
                        if (_removeKeys != null)
                        {
                            foreach (string key in _removeKeys)
                            {
                                // PROCEDURE: worldmetaRemove
                                using (var delCmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.WorldMeta, sqlTransaction))
                                {
                                    delCmd.Where(Mabinogi.SQL.Columns.WorldMeta.MetaKey, key);
                                    delCmd.Execute();
                                }
                            }
                        }
                            WorkSession.WriteStatus("WorldMetaSqlAdapter.UpdateList() : 명령을 실행합니다");
                            sqlTransaction.Commit();
                        
                        return REPLY_RESULT.SUCCESS;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        sqlTransaction?.Rollback();
                        return REPLY_RESULT.FAIL;
                    }
                    catch (Exception ex2)
                    {
                        ExceptionMonitor.ExceptionRaised(ex2);
                        WorkSession.WriteStatus(ex2.Message);
                        sqlTransaction?.Rollback();
                        return REPLY_RESULT.FAIL;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("WorldMetaSqlAdapter.UpdateList() : 연결을 종료합니다");
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return REPLY_RESULT.FAIL;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.FAIL;
            }
        }
    }
}
