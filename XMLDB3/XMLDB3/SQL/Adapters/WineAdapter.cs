using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class WineAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Wine;

        public WineAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public REPLY_RESULT Read(long _charID, out Wine _wine)
        {
            _wine = null;
            WorkSession.WriteStatus("WineSqlAdapter.Read() : 함수에 진입하였습니다.");

            try
            {
                // PROCEDURE: WineAdapter
                WorkSession.WriteStatus("WineSqlAdapter.Read() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Wine))
                {
                    cmd.Where(Mabinogi.SQL.Columns.Wine.CharId, _charID);

                    WorkSession.WriteStatus("WineSqlAdapter.Read() : 데이터를 채웁니다.");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            _wine = new Wine();
                            _wine.charID = _charID;
                            _wine.wineType = reader.GetByte(Mabinogi.SQL.Columns.Wine.WineType);
                            _wine.agingCount = reader.GetInt16(Mabinogi.SQL.Columns.Wine.AgingCount);
                            _wine.agingStartTime = reader.GetDateTime(Mabinogi.SQL.Columns.Wine.AgingStartTime);
                            _wine.lastRackingTime = reader.GetDateTime(Mabinogi.SQL.Columns.Wine.LastRackingTime);
                            _wine.acidity = reader.GetInt32(Mabinogi.SQL.Columns.Wine.Acidity);
                            _wine.purity = reader.GetInt32(Mabinogi.SQL.Columns.Wine.Purity);
                            _wine.freshness = reader.GetInt32(Mabinogi.SQL.Columns.Wine.Freshness);
                            return REPLY_RESULT.SUCCESS;
                        }
                        return REPLY_RESULT.FAIL_EX;
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _charID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return REPLY_RESULT.FAIL;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _charID);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("WineSqlAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");
            }
        }

        public bool Delete(long _charID)
        {
            WorkSession.WriteStatus("WineSqlAdapter.Delete() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("WineSqlAdapter.Delete() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.Wine))
                {
                    cmd.Where(Mabinogi.SQL.Columns.Wine.CharId, _charID);

                    WorkSession.WriteStatus("WineSqlAdapter.Delete() : 명령을 실행합니다");
                    cmd.Execute();
                }
                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _charID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _charID);
                WorkSession.WriteStatus(ex2.Message);

                return false;
            }
            finally
            {
                WorkSession.WriteStatus("WineSqlAdapter.Delete() : 연결을 종료합니다");
            }
        }

        public bool Update(Wine _wine)
        {
            WorkSession.WriteStatus("WineSqlAdapter.Update() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("WineSqlAdapter.Update() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    // PROCEDURE: WineUpdate

                    using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Wine))
                    {
                        upCmd.Where(Mabinogi.SQL.Columns.Wine.CharId, _wine.charID);

                        upCmd.Set(Mabinogi.SQL.Columns.Wine.WineType, _wine.wineType);
                        upCmd.Set(Mabinogi.SQL.Columns.Wine.AgingCount, _wine.agingCount);
                        upCmd.Set(Mabinogi.SQL.Columns.Wine.AgingStartTime, _wine.agingStartTime);
                        upCmd.Set(Mabinogi.SQL.Columns.Wine.LastRackingTime, _wine.lastRackingTime);
                        upCmd.Set(Mabinogi.SQL.Columns.Wine.Acidity, _wine.acidity);
                        upCmd.Set(Mabinogi.SQL.Columns.Wine.Purity, _wine.purity);
                        upCmd.Set(Mabinogi.SQL.Columns.Wine.Freshness, _wine.freshness);

                        WorkSession.WriteStatus("WineSqlAdapter.Update() : 명령을 실행합니다");
                        if (upCmd.Execute() < 1)
                        {
                            using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Wine))
                            {
                                insCmd.Set(Mabinogi.SQL.Columns.Wine.CharId, _wine.charID);
                                insCmd.Set(Mabinogi.SQL.Columns.Wine.WineType, _wine.wineType);
                                insCmd.Set(Mabinogi.SQL.Columns.Wine.AgingCount, _wine.agingCount);
                                insCmd.Set(Mabinogi.SQL.Columns.Wine.AgingStartTime, _wine.agingStartTime);
                                insCmd.Set(Mabinogi.SQL.Columns.Wine.LastRackingTime, _wine.lastRackingTime);
                                insCmd.Set(Mabinogi.SQL.Columns.Wine.Acidity, _wine.acidity);
                                insCmd.Set(Mabinogi.SQL.Columns.Wine.Purity, _wine.purity);
                                insCmd.Set(Mabinogi.SQL.Columns.Wine.Freshness, _wine.freshness);

                                insCmd.Execute();
                            }
                        }
                    }
                }

                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _wine.charID);
                WorkSession.WriteStatus(ex.Message, ex.Number);

                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _wine.charID);
                WorkSession.WriteStatus(ex2.Message);

                return false;
            }
            finally
            {
                WorkSession.WriteStatus("WineSqlAdapter.Update() : 연결을 종료합니다");
            }
        }
    }
}
