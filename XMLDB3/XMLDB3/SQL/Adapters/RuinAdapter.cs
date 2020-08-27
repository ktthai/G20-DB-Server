using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class RuinAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Ruin;

        public RuinAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public RuinList Read(RuinType _type)
        {
            WorkSession.WriteStatus("RuinSqlAdapter.Read() : 함수에 진입하였습니다.");

            try
            {
                WorkSession.WriteStatus("RuinSqlAdapter.Read() : 데이터 베이스에 연결합니다.");

                switch (_type)
                {
                    case RuinType.rtRuin:
                        return GetRelics();
                    case RuinType.rtRelic:
                        return GetRuins();
                    default:
                        throw new Exception("잘못된 유적 타입입니다");
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _type);
                WorkSession.WriteStatus(ex.Message, ex.Number);
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
            }
            finally
            {
                WorkSession.WriteStatus("RuinSqlAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");

            }
            return null;
        }

        private RuinList GetRelics()
        {
            RuinList ruinList = new RuinList();

            // PROCEDURE: SelectRelic
            using (var conn = Connection)
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Relic))
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Ruin ruin;
                    while (reader.Read())
                    {
                        ruin = new Ruin();
                        ruin.ruinID = reader.GetInt32(Mabinogi.SQL.Columns.Relic.RuinId);
                        ruin.state = reader.GetInt32(Mabinogi.SQL.Columns.Relic.State);
                        ruin.position = reader.GetInt32(Mabinogi.SQL.Columns.Relic.Position);
                        ruin.lastTime = reader.GetInt32(Mabinogi.SQL.Columns.Relic.LastTime);
                        ruin.exploCharID = reader.GetInt64(Mabinogi.SQL.Columns.Relic.ExploCharId);
                        ruin.exploCharName = reader.GetString(Mabinogi.SQL.Columns.Relic.ExploCharName);

                        DateTime exploTime = reader.GetDateTime(Mabinogi.SQL.Columns.Relic.ExploTime);
                        if (exploTime == null)
                        {
                            ruin.exploTime = DateTime.MinValue;
                        }
                        else
                        {
                            ruin.exploTime = exploTime;
                        }
                    }
                }
            }

            return ruinList;
        }

        private RuinList GetRuins()
        {
            RuinList ruinList = new RuinList();

            // PROCEDURE: SelectRuin
            using (var conn = Connection)
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Ruin))
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    Ruin ruin;
                    while (reader.Read())
                    {
                        ruin = new Ruin();
                        ruin.ruinID = reader.GetInt32(Mabinogi.SQL.Columns.Ruin.RuinId);
                        ruin.state = reader.GetInt32(Mabinogi.SQL.Columns.Ruin.State);
                        ruin.position = reader.GetInt32(Mabinogi.SQL.Columns.Ruin.Position);
                        ruin.lastTime = reader.GetInt32(Mabinogi.SQL.Columns.Ruin.LastTime);
                        ruin.exploCharID = reader.GetInt64(Mabinogi.SQL.Columns.Ruin.ExploCharId);
                        ruin.exploCharName = reader.GetString(Mabinogi.SQL.Columns.Ruin.ExploCharName);

                        DateTime exploTime = reader.GetDateTime(Mabinogi.SQL.Columns.Ruin.ExploTime);
                        if (exploTime == null)
                        {
                            ruin.exploTime = DateTime.MinValue;
                        }
                        else
                        {
                            ruin.exploTime = exploTime;
                        }
                    }
                }
            }

            return ruinList;
        }

        public bool Write(RuinList _ruinList, RuinType _type)
        {
            WorkSession.WriteStatus("RuinSqlAdapter.Write() : 함수에 진입하였습니다");
            if (_ruinList.ruins == null || _ruinList.ruins.Length == 0)
            {
                WorkSession.WriteStatus("RuinSqlAdapter.Write() : 업데이트할 유적이 없습니다.");
                return true;
            }

            try
            {
                WorkSession.WriteStatus("RuinSqlAdapter.Write() : 데이터베이스와 연결합니다");

                switch (_type)
                {
                    case RuinType.rtRuin:
                        return UpdateRuins(_ruinList.ruins);
                        break;
                    case RuinType.rtRelic:
                        return UpdateRelics(_ruinList.ruins);
                        break;
                    default:
                        throw new Exception("잘못된 유적 타입입니다");
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("RuinSqlAdapter.Write() : 연결을 종료합니다");
            }
        }

        private bool UpdateRuins(Ruin[] ruins)
        {
            using(var conn = Connection)
            {
                foreach(Ruin ruin in ruins)
                {
                    UpdateRuin(conn, ruin);
                }
            }
            return true;
        }

        private static void UpdateRuin(SimpleConnection conn, Ruin ruin)
        {
            // PROCEDURE: UpdateRuin
            using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Ruin))
            {
                upCmd.Where(Mabinogi.SQL.Columns.Ruin.RuinId, ruin.ruinID);

                upCmd.Set(Mabinogi.SQL.Columns.Ruin.State, ruin.state);
                upCmd.Set(Mabinogi.SQL.Columns.Ruin.Position, ruin.position);
                upCmd.Set(Mabinogi.SQL.Columns.Ruin.LastTime, ruin.lastTime);
                upCmd.Set(Mabinogi.SQL.Columns.Ruin.ExploCharId, ruin.exploCharID);
                upCmd.Set(Mabinogi.SQL.Columns.Ruin.ExploCharName, ruin.exploCharName);
                upCmd.Set(Mabinogi.SQL.Columns.Ruin.ExploTime, ruin.exploTime);

                if (upCmd.Execute() < 1)
                {
                    using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Ruin))
                    {
                        insCmd.Set(Mabinogi.SQL.Columns.Ruin.RuinId, ruin.ruinID);
                        insCmd.Set(Mabinogi.SQL.Columns.Ruin.State, ruin.state);
                        insCmd.Set(Mabinogi.SQL.Columns.Ruin.Position, ruin.position);
                        insCmd.Set(Mabinogi.SQL.Columns.Ruin.LastTime, ruin.lastTime);
                        insCmd.Set(Mabinogi.SQL.Columns.Ruin.ExploCharId, ruin.exploCharID);
                        insCmd.Set(Mabinogi.SQL.Columns.Ruin.ExploCharName, ruin.exploCharName);
                        insCmd.Set(Mabinogi.SQL.Columns.Ruin.ExploTime, ruin.exploTime);

                        insCmd.Execute();
                    }
                }
            }
        }

        private bool UpdateRelics(Ruin[] ruins)
        {
            using (var conn = Connection)
            {
                foreach (Ruin ruin in ruins)
                {
                    UpdateRelic(conn, ruin);
                }
            }
            return true;
        }

        private void UpdateRelic(SimpleConnection conn, Ruin ruin)
        {
            // PROCEDURE: UpdateRelic
            using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Relic))
            {
                upCmd.Where(Mabinogi.SQL.Columns.Relic.RuinId, ruin.ruinID);

                upCmd.Set(Mabinogi.SQL.Columns.Relic.State, ruin.state);
                upCmd.Set(Mabinogi.SQL.Columns.Relic.Position, ruin.position);
                upCmd.Set(Mabinogi.SQL.Columns.Relic.LastTime, ruin.lastTime);
                upCmd.Set(Mabinogi.SQL.Columns.Relic.ExploCharId, ruin.exploCharID);
                upCmd.Set(Mabinogi.SQL.Columns.Relic.ExploCharName, ruin.exploCharName);
                upCmd.Set(Mabinogi.SQL.Columns.Relic.ExploTime, ruin.exploTime);

                if (upCmd.Execute() < 1)
                {
                    using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Relic))
                    {
                        insCmd.Set(Mabinogi.SQL.Columns.Relic.RuinId, ruin.ruinID);
                        insCmd.Set(Mabinogi.SQL.Columns.Relic.State, ruin.state);
                        insCmd.Set(Mabinogi.SQL.Columns.Relic.Position, ruin.position);
                        insCmd.Set(Mabinogi.SQL.Columns.Relic.LastTime, ruin.lastTime);
                        insCmd.Set(Mabinogi.SQL.Columns.Relic.ExploCharId, ruin.exploCharID);
                        insCmd.Set(Mabinogi.SQL.Columns.Relic.ExploCharName, ruin.exploCharName);
                        insCmd.Set(Mabinogi.SQL.Columns.Relic.ExploTime, ruin.exploTime);

                        insCmd.Execute();
                    }
                }
            }
        }

        public bool Write(Ruin _ruin, RuinType _type)
        {
            WorkSession.WriteStatus("RuinSqlAdapter.Write() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("RuinSqlAdapter.Write() : 데이터베이스와 연결합니다");

                using (var conn = Connection)
                {
                    WorkSession.WriteStatus("RuinSqlAdapter.Write() : 명령을 실행합니다");
                    switch (_type)
                    {
                        case RuinType.rtRuin:
                            UpdateRuin(conn, _ruin);
                            break;
                        case RuinType.rtRelic:
                            UpdateRelic(conn, _ruin);
                            break;
                        default:
                            throw new Exception("잘못된 유적 타입입니다");
                    }
                }

                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("RuinSqlAdapter.Write() : 연결을 종료합니다");
            }
        }
    }
}
