using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class PrivateFarmAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.PrivateFarm;

        public PrivateFarmAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public bool Create(PrivateFarm _privateFarm)
        {
            WorkSession.WriteStatus("PrivateFarmSqlAdapter.Create() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("PrivateFarmSqlAdapter.Create() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    try
                    {
                        // PROCEDURE: CreatePrivateFarm
                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarm))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.Id, _privateFarm.id);
                            cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.OwnerId, _privateFarm.ownerId);
                            cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.ClassId, _privateFarm.classId);
                            cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.Name, _privateFarm.name);
                            cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.WorldPosX, _privateFarm.worldPosX);
                            cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.WorldPosY, _privateFarm.worldPosY);
                            cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.OwnerName, _privateFarm.ownerName);
                            cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.BindedChannel, _privateFarm.bindedChannel);
                            cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.NextBindableTime, _privateFarm.nextBindableTime);
                            WorkSession.WriteStatus("PrivateFarmSqlAdapter.Create() : 명령을 실행합니다");
                            cmd.Execute();
                        }
                        return true;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("AccountrefSqlAdapter.SetFlag() : 연결을 종료합니다");
                    }
                }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
        }

        public PrivateFarm Read(long _idZone, PrivateFarm _cache)
        {
            WorkSession.WriteStatus("PrivateFarmSqlAdapter.Read() : 함수에 진입하였습니다");
            try
            {
                if (_cache == null)
                {
                    return _Read(_idZone);
                }
                if (!IsValidCache(_cache))
                {
                    return _Read(_idZone);
                }
                return _cache;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _idZone);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _idZone);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
        }

        private PrivateFarm _Read(long _idZone)
        {
            WorkSession.WriteStatus("PrivateFarmSqlAdapter.Read() : 함수에 진입하였습니다");

            // PROCEDURE: SelectPrivateFarmAll
            try
            {
                WorkSession.WriteStatus("PrivateFarmSqlAdapter._Read() : 데이터베이스와 연결합니다");
                long timestamp = Stopwatch.GetTimestamp();
                PrivateFarm privateFarm;
                using (var conn = Connection)
                {
                    WorkSession.WriteStatus("PrivateFarmSqlAdapter._Read() : DataSet 에 정보를 채웁니다");
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarm))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.PrivateFarm.Id, _idZone);
                        using (var farmReader = cmd.ExecuteReader())
                            privateFarm = PrivateFarmObjectBuilder.Build(farmReader);
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmFacility))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.PrivateFarmFacility.PrivateFarmId, _idZone);
                        using (var facilityReader = cmd.ExecuteReader())
                            privateFarm.field = PrivateFarmFieldObjectBuilder.BuildFacilityTable(facilityReader);
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmVisitor))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.PrivateFarmVisitor.PrivateFarmId, _idZone);
                        using (var visitorReader = cmd.ExecuteReader())
                            privateFarm.visitorList = PrivateFarmFieldObjectBuilder.BuildVisitorTable(visitorReader);
                    }

                    CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctPrivateFarmRead, Stopwatch.GetElapsedMilliseconds(timestamp));
                    WorkSession.WriteStatus("PrivateFarmSqlAdapter._Read() : DataSet 으로부터 정보를 생성합니다");
                    return privateFarm;
                }
            }
            catch (Exception ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _idZone);
                WorkSession.WriteStatus(ex.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("PrivateFarmSqlAdapter._Read() : 연결을 종료합니다");
            }
        }

        private bool IsValidCache(PrivateFarm _cache)
        {
            WorkSession.WriteStatus("PrivateFarmSqlAdapter.IsValidCache() : 함수에 진입하였습니다");
            if (_cache == null)
            {
                return false;
            }


            // PROCEDURE: GetPrivateFarmUpdateTime
            try
            {
                WorkSession.WriteStatus("PrivateFarmSqlAdapter.IsValidCache() : 데이터베이스에 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarm))
                {
                    cmd.Where(Mabinogi.SQL.Columns.PrivateFarm.Id, _cache.id);
                    cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.UpdateTime, 0);

                    WorkSession.WriteStatus("PrivateFarmSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜를 얻어옵니다");
                    var updatetime = cmd.ExecuteReader().GetDateTime(Mabinogi.SQL.Columns.PrivateFarm.UpdateTime);

                    if (updatetime != null)
                    {
                        if (updatetime.Ticks <= _cache.updatetime.Ticks)
                        {
                            WorkSession.WriteStatus("PrivateFarmSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜가 캐쉬 날짜보다 빠릅니다. 캐쉬의 정보가 최신입니다");
                            return true;
                        }
                        WorkSession.WriteStatus("PrivateFarmSqlAdapter.IsValidCache() : 캐쉬가 유효하지 않습니다.");
                        return false;
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
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _cache);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("PrivateFarmSqlAdapter.IsValidCache() : 데이터베이스에 연결을 종료합니다");
            }


        }

        public bool Write(PrivateFarm _privateFarm, PrivateFarm _cache, PRIVATEFARM_WRITEMODE _mode)
        {
            WorkSession.WriteStatus("PrivateFarmSqlAdapter.Write() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("PrivateFarmSqlAdapter.Write() : 비교본으로 사용할 수 있는 유효한 저장 데이터를 읽어옵니다");
                if (_cache == null)
                {
                    WorkSession.WriteStatus("PrivateFarmSqlAdapter.Write() : 저장되어 있는 데이터가 없어 새로 만듭니다.");
                    _cache = _Read(_privateFarm.id);
                }
                else if (!IsValidCache(_cache))
                {
                    _cache = _Read(_privateFarm.id);
                }

                WorkSession.WriteStatus("PrivateFarmSqlAdapter.Write() : SQL 명령문을 생성합니다");

                SimpleTransaction sqlTransaction = null;
                WorkSession.WriteStatus("PrivateFarmSqlAdapter.Write() : Pet 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    try
                    {
                        WorkSession.WriteStatus("PrivateFarmSqlAdapter.Write() : Accountref 데이터베이스와 연결합니다");
                        long timestamp = Stopwatch.GetTimestamp();


                        if (_mode == PRIVATEFARM_WRITEMODE.ALL)
                        {
                            sqlTransaction = conn.BeginTransaction();
                            PrivateFarmUpdateBuilder.Build(_privateFarm, _cache, conn, sqlTransaction);
                            PrivateFarmFieldUpdateBuilder.Build(_privateFarm.id, _privateFarm.field, _cache.field, conn, sqlTransaction);
                            PrivateFarmVisitorUpdateBuilder.Build(_privateFarm.id, _privateFarm.visitorList, _cache.visitorList, conn, sqlTransaction);
                        }
                        else if (PRIVATEFARM_WRITEMODE.VISITOR_LIST == _mode)
                        {
                            sqlTransaction = conn.BeginTransaction();
                            PrivateFarmVisitorUpdateBuilder.Build(_privateFarm.id, _privateFarm.visitorList, _cache.visitorList, conn, sqlTransaction);
                        }
                        else
                        {
                            WorkSession.WriteStatus("PrivateFarmSqlAdapter.Write() : 변경점이 없습니다. 쿼리를 생략합니다");
                        }
                        WorkSession.WriteStatus("PrivateFarmSqlAdapter.Write() : 트랜잭션을 커밋합니다");
                        sqlTransaction?.Commit();
                        CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctPrivateFarmWrite, Stopwatch.GetElapsedMilliseconds(timestamp));
                        return true;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, _privateFarm);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        sqlTransaction?.Rollback();
                        return false;
                    }
                    catch (Exception ex2)
                    {
                        ExceptionMonitor.ExceptionRaised(ex2, _privateFarm);
                        WorkSession.WriteStatus(ex2.Message);
                        sqlTransaction?.Rollback();
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("PrivateFarmSqlAdapter.Write() : 연결을 종료합니다");
                    }
                }
            }
            catch (SimpleSqlException ex3)
            {
                ExceptionMonitor.ExceptionRaised(ex3);
                WorkSession.WriteStatus(ex3.Message, _privateFarm);
                return false;
            }
            catch (Exception ex4)
            {
                ExceptionMonitor.ExceptionRaised(ex4);
                WorkSession.WriteStatus(ex4.Message, _privateFarm);
                return false;
            }
        }

        public REPLY_RESULT QueryOwner(long _idZone, out long _idChar)
        {
            _idChar = 0L;
            try
            {

                // PROCEDURE: QueryPrivateFarmOwnerId
                WorkSession.WriteStatus("PrivateFarmSqlAdapter.QueryOwner() : 데이터베이스에 연결합니다");
                using (var conn = Connection)
                {
                    try
                    {
                        using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarm))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.PrivateFarm.Id, _idZone);
                            cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.OwnerId, 0);

                            WorkSession.WriteStatus("PrivateFarmSqlAdapter.QueryOwner() : 마지막으로 변경된 날짜를 얻어옵니다");

                            using (var reader = cmd.ExecuteReader())
                            {

                                if (reader.Read() == false)
                                    return REPLY_RESULT.FAIL;

                                _idChar = reader.GetInt64(Mabinogi.SQL.Columns.PrivateFarm.OwnerId);
                            }
                            return REPLY_RESULT.SUCCESS;
                        }
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, _idZone);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return REPLY_RESULT.FAIL;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("PrivateFarmSqlAdapter.QueryOwner() : 데이터베이스에 연결을 종료합니다");

                    }
                }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _idZone);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.FAIL;
            }
        }

        public REPLY_RESULT QueryZoneId(string _strCharName, out long _idZone)
        {
            _idZone = 0L;
            try
            {


                WorkSession.WriteStatus("PrivateFarmSqlAdapter.QueryZoneId() : 데이터베이스에 연결합니다");
                using (var conn = Connection)
                {
                    try
                    {
                        // PROCEDURE: QueryPrivateFarmZoneId
                        using (var selCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Character))
                        {
                            selCmd.Where(Mabinogi.SQL.Columns.Character.Name, _strCharName);
                            selCmd.Where(Mabinogi.SQL.Columns.Character.DeleteTime, null);
                            selCmd.Set(Mabinogi.SQL.Columns.Character.Id, 0);

                            long ownerId;
                            using (var reader = selCmd.ExecuteReader())
                            {
                                if (reader.Read() == false)
                                    return REPLY_RESULT.FAIL;

                                ownerId = reader.GetInt64(Mabinogi.SQL.Columns.Character.Id);
                            }
                            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarm))
                            {
                                cmd.Where(Mabinogi.SQL.Columns.PrivateFarm.OwnerId, ownerId);
                                cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.Id, 0);

                                WorkSession.WriteStatus("PrivateFarmSqlAdapter.QueryZoneId() : 마지막으로 변경된 날짜를 얻어옵니다");
                                using (var reader = cmd.ExecuteReader())
                                {

                                    if (reader.Read() == false)
                                        return REPLY_RESULT.FAIL;

                                    _idZone = reader.GetInt64(Mabinogi.SQL.Columns.PrivateFarm.Id);
                                    return REPLY_RESULT.SUCCESS;
                                }
                            }
                        }
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, _strCharName);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return REPLY_RESULT.FAIL;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("PrivateFarmSqlAdapter.QueryZoneId() : 데이터베이스에 연결을 종료합니다");
                    }
                }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _idZone);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.FAIL;
            }
        }

        public bool IsUsableName(string _name)
        {
            WorkSession.WriteStatus("PrivateFarmSqlAdapter.IsUsableName() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("PrivateFarmSqlAdapter.IsUsableName() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    // PROCEDURE: CheckPrivateFarmUsableName
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarm))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.PrivateFarm.Name, _name);
                        cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.Name, 0);

                        return cmd.ExecuteReader().HasRows == false;
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _name);
                WorkSession.WriteStatus(ex.Message, _name);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _name);
                WorkSession.WriteStatus(ex2.Message, _name);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("PrivateFarmSqlAdapter.IsUsableName() : 연결을 종료합니다");
            }
        }

        public REPLY_RESULT FindPenaltyState(long _idZone)
        {
            WorkSession.WriteStatus("PrivateFarmSqlAdapter.FindPenaltyState() : 함수에 진입하였습니다");

            // PROCEDURE: SelectPrivateFarmPenalty
            // Function does not exist, is apparently not used

            return REPLY_RESULT.ERROR;
        }

        public REPLY_RESULT InsertPenaltyState(long _idZone, uint _Penalty)
        {
            WorkSession.WriteStatus("PrivateFarmSqlAdapter.InsertPenaltyState() : 함수에 진입하였습니다");

            // PROCEDURE: InsertPrivateFarmPenalty
            // Function does not exist, is apparently not used

            return REPLY_RESULT.ERROR;
        }

        public REPLY_RESULT DeletePenaltyState(long _idZone, uint _Penalty)
        {
            WorkSession.WriteStatus("PrivateFarmSqlAdapter.DeletePrivateFarmPenalty() : 함수에 진입하였습니다");

            // PROCEDURE: DeletePrivateFarmPenalty
            // Function does not exist, is apparently not used

            return REPLY_RESULT.ERROR;
        }

        public long SelectPrivateFarmUpdateTime(long _idZone)
        {
            WorkSession.WriteStatus("PrivateFarmSqlAdapter.SelectPrivateFarmUpdateTime() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("PrivateFarmSqlAdapter.SelectPrivateFarmUpdateTime() : 데이터베이스와 연결합니다");

                // PROCEDURE: GetPrivateFarmUpdateTime
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarm))
                {
                    cmd.Where(Mabinogi.SQL.Columns.PrivateFarm.Id, _idZone);
                    cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.UpdateTime, 0);

                    WorkSession.WriteStatus("PrivateFarmSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜를 얻어옵니다");
                    var updatetime = cmd.ExecuteReader().GetDateTime(Mabinogi.SQL.Columns.PrivateFarm.UpdateTime);

                    if (updatetime != null)
                        return updatetime.Ticks;

                    return 0L;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _idZone);
                WorkSession.WriteStatus(ex.Message, _idZone);
                return 0L;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _idZone);
                WorkSession.WriteStatus(ex2.Message, _idZone);
                return 0L;
            }
            finally
            {
                WorkSession.WriteStatus("PrivateFarmSqlAdapter.SelectPrivateFarmUpdateTime() : 연결을 종료합니다");
            }
        }
    }
}
