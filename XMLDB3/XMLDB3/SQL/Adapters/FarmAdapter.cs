using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class FarmAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Farm;
        public FarmAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public Farm Read(long _farmID)
        {
            Farm result = null;

            try
            {
                // PROCEDURE: FarmSelect
                using (var conn = Connection)
                using (var readCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Farm))
                {
                    readCmd.Where(Mabinogi.SQL.Columns.Farm.FarmId, _farmID);
                    using (var reader = readCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new Farm() { farmID = _farmID };
                            result.ownerAccount = reader.GetString(Mabinogi.SQL.Columns.Farm.OwnerAccount);
                            result.ownerCharID = reader.GetInt64(Mabinogi.SQL.Columns.Farm.OwnerCharId);
                            result.ownerCharName = reader.GetString(Mabinogi.SQL.Columns.Farm.OwnerCharName);
                            result.expireTime = reader.GetInt64(Mabinogi.SQL.Columns.Farm.ExpireTime);
                            result.crop = reader.GetByte(Mabinogi.SQL.Columns.Farm.Crop);
                            result.plantTime = reader.GetInt64(Mabinogi.SQL.Columns.Farm.PlantTime);
                            result.waterWork = reader.GetInt16(Mabinogi.SQL.Columns.Farm.WaterWork);
                            result.nutrientWork = reader.GetInt16(Mabinogi.SQL.Columns.Farm.NutrientWork);
                            result.insectWork = reader.GetInt16(Mabinogi.SQL.Columns.Farm.InsectWork);
                            result.water = reader.GetInt16(Mabinogi.SQL.Columns.Farm.Water);
                            result.nutrient = reader.GetInt16(Mabinogi.SQL.Columns.Farm.Nutrient);
                            result.insect = reader.GetInt16(Mabinogi.SQL.Columns.Farm.Insect);
                            result.growth = reader.GetInt16(Mabinogi.SQL.Columns.Farm.Growth);
                            result.currentWork = reader.GetByte(Mabinogi.SQL.Columns.Farm.CurrentWork);
                            result.workCompleteTime = reader.GetInt64(Mabinogi.SQL.Columns.Farm.WorkCompleteTime);
                            result.todayWorkCount = reader.GetByte(Mabinogi.SQL.Columns.Farm.TodayWorkCount);
                            result.lastWorkTime = reader.GetInt64(Mabinogi.SQL.Columns.Farm.LastWorkTime);
                        }
                        else
                        {
                            reader.Close();

                            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Farm))
                            {
                                cmd.Set(Mabinogi.SQL.Columns.Farm.FarmId, _farmID);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.OwnerAccount, string.Empty);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.OwnerCharId, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.OwnerCharName, string.Empty);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.ExpireTime, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.Crop, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.PlantTime, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.WaterWork, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.NutrientWork, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.InsectWork, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.Water, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.Nutrient, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.Insect, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.Growth, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.CurrentWork, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.WorkCompleteTime, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.TodayWorkCount, 0);
                                cmd.Set(Mabinogi.SQL.Columns.Farm.LastWorkTime, 0);

                                cmd.Execute();

                                result = new Farm() { farmID = _farmID, ownerAccount = string.Empty, ownerCharName = string.Empty };
                            }
                        }
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _farmID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _farmID);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("FarmSqlAdapter.Read() : Leaving function");
            }

            return result;
        }

        public REPLY_RESULT Lease(long _farmID, string _account, long _charID, string _charName, long _expireTime, ref byte _errorCode)
        {
            WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();
                    // PROCEDURE: dbo.FarmLease
                    using (var readCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Farm, transaction))
                    {
                        readCmd.Where(Mabinogi.SQL.Columns.Farm.FarmId, _farmID);
                        using (var reader = readCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader.GetString(Mabinogi.SQL.Columns.Farm.OwnerAccount) != "")
                                {
                                    // This field is owned by someone already
                                    _errorCode = 2;
                                    transaction.Rollback();
                                    return REPLY_RESULT.FAIL_EX;
                                }
                            }
                            else
                            {
                                // The field didn't exist at all
                                _errorCode = 1;
                                return REPLY_RESULT.FAIL_EX;
                            }
                        }
                    }
                    using (var chkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Farm, transaction))
                    {
                        chkCmd.Where(Mabinogi.SQL.Columns.Farm.OwnerAccount, _account);

                        using (var reader = chkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // This account has a field already
                                _errorCode = 3;
                                transaction.Rollback();
                                return REPLY_RESULT.FAIL_EX;
                            }
                        }

                        using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Farm, transaction))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.Farm.OwnerAccount, _account);
                            cmd.Set(Mabinogi.SQL.Columns.Farm.OwnerCharId, _charID);
                            cmd.Set(Mabinogi.SQL.Columns.Farm.OwnerCharName, _charName);
                            cmd.Set(Mabinogi.SQL.Columns.Farm.ExpireTime, _expireTime);

                            cmd.Where(Mabinogi.SQL.Columns.Farm.FarmId, _farmID);

                            WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 명령을 실행합니다");
                            cmd.Execute();
                        }
                        WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 트랜잭션을 커밋합니다");
                        transaction.Commit();

                        // conditions were met; the field exists, isn't owned by another, and the account doesn't already have a farm
                        _errorCode = 0;
                        return REPLY_RESULT.SUCCESS;
                    }
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _farmID, _account);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _farmID, _account);
                    WorkSession.WriteStatus(ex2.Message);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT Expire(long _farmID, ref byte _errorCode)
        {
            WorkSession.WriteStatus("FarmSqlAdapter.Expire() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("FarmSqlAdapter.Expire() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {
                    transaction = conn.BeginTransaction();
                    // PROCEDURE: dbo.FarmExpire

                    using (var ownChkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Farm, transaction))
                    {
                        ownChkCmd.Where(Mabinogi.SQL.Columns.Farm.FarmId, _farmID);
                        ownChkCmd.Set(Mabinogi.SQL.Columns.Farm.OwnerAccount, 0);
                        using (var reader = ownChkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var ownerAcc = reader.GetString(Mabinogi.SQL.Columns.Farm.OwnerAccount);
                                if (ownerAcc != string.Empty)
                                {
                                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Farm, transaction))
                                    {
                                        cmd.Where(Mabinogi.SQL.Columns.Farm.FarmId, _farmID);

                                        cmd.Set(Mabinogi.SQL.Columns.Farm.OwnerAccount, string.Empty);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.OwnerCharId, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.OwnerCharName, string.Empty);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.ExpireTime, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.Crop, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.PlantTime, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.WaterWork, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.NutrientWork, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.InsectWork, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.Water, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.Nutrient, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.Insect, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.Growth, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.CurrentWork, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.WorkCompleteTime, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.TodayWorkCount, 0);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.LastWorkTime, 0);

                                        cmd.Execute();
                                    }
                                    // Farm was expired correctly
                                    WorkSession.WriteStatus("FarmSqlAdapter.Expire() : 명령을 실행합니다");
                                    transaction.Commit();
                                    _errorCode = 0;
                                    return REPLY_RESULT.SUCCESS;
                                }

                                // Nobody owned the farm
                                _errorCode = 2;
                                transaction.Rollback();
                                return REPLY_RESULT.FAIL_EX;
                            }
                        }
                    }



                    transaction.Rollback();
                    // farm didn't exist
                    _errorCode = 1;
                    return REPLY_RESULT.FAIL_EX;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _farmID);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FarmSqlAdapter.Expire() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _farmID);
                    WorkSession.WriteStatus(ex2.Message);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("FarmSqlAdapter.Lease() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT Update(Farm _farm, ref byte _errorCode)
        {
            WorkSession.WriteStatus("FarmSqlAdapter.Update() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("FarmSqlAdapter.Update() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {
                    transaction = conn.BeginTransaction();
                    // PROCEDURE: dbo.FarmUpdate

                    using (var chkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Farm, transaction))
                    {
                        chkCmd.Where(Mabinogi.SQL.Columns.Farm.FarmId, _farm.farmID);
                        chkCmd.Set(Mabinogi.SQL.Columns.Farm.OwnerAccount, 0);

                        using (var reader = chkCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var ownerAcc = reader.GetString(Mabinogi.SQL.Columns.Farm.OwnerAccount);
                                if (ownerAcc != string.Empty)
                                {
                                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Farm, transaction))
                                    {
                                        cmd.Where(Mabinogi.SQL.Columns.Farm.FarmId, _farm.farmID);

                                        cmd.Set(Mabinogi.SQL.Columns.Farm.Crop, _farm.crop);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.PlantTime, _farm.plantTime);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.WaterWork, _farm.waterWork);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.NutrientWork, _farm.nutrientWork);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.InsectWork, _farm.insectWork);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.Water, _farm.water);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.Nutrient, _farm.nutrient);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.Insect, _farm.insect);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.Growth, _farm.growth);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.CurrentWork, _farm.currentWork);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.WorkCompleteTime, _farm.workCompleteTime);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.TodayWorkCount, _farm.todayWorkCount);
                                        cmd.Set(Mabinogi.SQL.Columns.Farm.LastWorkTime, _farm.lastWorkTime);

                                        WorkSession.WriteStatus("FarmSqlAdapter.Update() : 명령을 실행합니다");
                                        cmd.Execute();

                                        WorkSession.WriteStatus("FarmSqlAdapter.Update() : 트랜잭션을 커밋합니다");
                                        _errorCode = 0;
                                        transaction.Commit();
                                        return REPLY_RESULT.SUCCESS;
                                    }
                                }
                                // Farm not owned by anyone
                                _errorCode = 2;
                                transaction.Rollback();
                                return REPLY_RESULT.FAIL_EX;
                            }
                        }
                    }

                    // Farm didn't exist
                    _errorCode = 1;
                    transaction.Rollback();
                    return REPLY_RESULT.FAIL_EX;

                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FarmSqlAdapter.Update() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _farm.farmID);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FarmSqlAdapter.Update() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _farm.farmID);
                    WorkSession.WriteStatus(ex2.Message);
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("FarmSqlAdapter.Update() : 연결을 종료합니다");
                }
            }
        }

        public bool GetOwnerInfo(string _account, ref long _farmID, ref long _ownerCharID, ref string _ownerCharName)
        {
            WorkSession.WriteStatus("FarmSqlAdapter.GetFarmOwnInfo() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("FarmSqlAdapter.GetFarmOwnInfo() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Farm))
                {
                    // PROCEDURE: dbo.FarmGetOwnerInfo

                    cmd.Where(Mabinogi.SQL.Columns.Farm.OwnerAccount, _account);

                    cmd.Set(Mabinogi.SQL.Columns.Farm.OwnerCharName, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Farm.OwnerCharId, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Farm.FarmId, 0);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            _farmID = reader.GetInt64(Mabinogi.SQL.Columns.Farm.FarmId);
                            _ownerCharID = reader.GetInt64(Mabinogi.SQL.Columns.Farm.OwnerCharId);
                            _ownerCharName = reader.GetString(Mabinogi.SQL.Columns.Farm.OwnerCharName);
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("FarmSqlAdapter.GetFarmOwnInfo() : 연결을 종료합니다");
            }
        }
    }
}
