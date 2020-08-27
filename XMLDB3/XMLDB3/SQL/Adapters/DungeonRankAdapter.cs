using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class DungeonRankAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.DungeonRank.DungeonScoreBoard;
        public DungeonRankAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public bool Update(DungeonRank _dungeonRank)
        {
            WorkSession.WriteStatus("DungeonRankSqlAdapter.Update() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            int scoreRow = 0;
            int timeRow = 0;
            DateTime now = DateTime.Now;
            using (var conn = Connection)
            {
                try
                {
                    WorkSession.WriteStatus("DungeonRankSqlAdapter.Update() : 데이터 베이스에 연결합니다.");

                    transaction = conn.BeginTransaction();
                    using (var chkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonScoreRankInfo))
                    {
                        chkCmd.Where(Mabinogi.SQL.Columns.DungeonScoreRankInfo.Server, _dungeonRank.server);
                        chkCmd.Where(Mabinogi.SQL.Columns.DungeonScoreRankInfo.DungeonName, _dungeonRank.dungeonName);
                        chkCmd.Where(Mabinogi.SQL.Columns.DungeonScoreRankInfo.Race, _dungeonRank.race);

                        using (var reader = chkCmd.ExecuteReader())
                        {
                            if (reader.Read() != true)
                            {
                                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonScoreRankInfo, transaction))
                                {
                                    cmd.Set(Mabinogi.SQL.Columns.DungeonScoreRankInfo.Server, _dungeonRank.server);
                                    cmd.Set(Mabinogi.SQL.Columns.DungeonScoreRankInfo.DungeonName, _dungeonRank.dungeonName);
                                    cmd.Set(Mabinogi.SQL.Columns.DungeonScoreRankInfo.Race, _dungeonRank.race);
                                    cmd.Set(Mabinogi.SQL.Columns.DungeonScoreRankInfo.ScoreRow, 0);
                                    cmd.Execute();
                                }
                            }
                            else
                            {
                                scoreRow = reader.GetInt32(Mabinogi.SQL.Columns.DungeonTimeRankInfo.TimeRow);
                            }
                        }
                    }

                    using (var chkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonTimeRankInfo))
                    {
                        chkCmd.Where(Mabinogi.SQL.Columns.DungeonTimeRankInfo.Server, _dungeonRank.server);
                        chkCmd.Where(Mabinogi.SQL.Columns.DungeonTimeRankInfo.DungeonName, _dungeonRank.dungeonName);
                        chkCmd.Where(Mabinogi.SQL.Columns.DungeonTimeRankInfo.Race, _dungeonRank.race);

                        using (var reader = chkCmd.ExecuteReader())
                        {
                            if (reader.Read() != true)
                            {
                                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonTimeRankInfo, transaction))
                                {
                                    cmd.Set(Mabinogi.SQL.Columns.DungeonTimeRankInfo.Server, _dungeonRank.server);
                                    cmd.Set(Mabinogi.SQL.Columns.DungeonTimeRankInfo.DungeonName, _dungeonRank.dungeonName);
                                    cmd.Set(Mabinogi.SQL.Columns.DungeonTimeRankInfo.Race, _dungeonRank.race);
                                    cmd.Set(Mabinogi.SQL.Columns.DungeonTimeRankInfo.TimeRow, 0);
                                    cmd.Execute();
                                }
                            }
                            else
                            {
                                timeRow = reader.GetInt32(Mabinogi.SQL.Columns.DungeonTimeRankInfo.TimeRow);
                            }
                        }
                    }

                    if (_dungeonRank.score != 0)
                    {
                        if (scoreRow < 300)
                        {
                            using (var chkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonScoreBoard))
                            {
                                chkCmd.Where(Mabinogi.SQL.Columns.DungeonScoreBoard.Server, _dungeonRank.server);
                                chkCmd.Where(Mabinogi.SQL.Columns.DungeonScoreBoard.DungeonName, _dungeonRank.dungeonName);
                                chkCmd.Where(Mabinogi.SQL.Columns.DungeonScoreBoard.CharacterId, _dungeonRank.characterID);

                                using (var reader = chkCmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonScoreBoard, transaction))
                                        {
                                            cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.Score, _dungeonRank.score);
                                            cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.RegDate, now);
                                            cmd.ComplexWhere(string.Format("WHERE {0} = @{0} AND {1} = @{1} AND {2} = @{2} AND {3} < @{3}",
                                                Mabinogi.SQL.Columns.DungeonScoreBoard.Server, Mabinogi.SQL.Columns.DungeonScoreBoard.DungeonName, Mabinogi.SQL.Columns.DungeonScoreBoard.CharacterId, Mabinogi.SQL.Columns.DungeonScoreBoard.Score));

                                            cmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.Server, _dungeonRank.server);
                                            cmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.DungeonName, _dungeonRank.dungeonName);
                                            cmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.CharacterId, _dungeonRank.characterID);
                                            cmd.Execute();
                                        }
                                    }
                                    else
                                    {
                                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonScoreBoard, transaction))
                                        {
                                            cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.Server, _dungeonRank.server);
                                            cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.DungeonName, _dungeonRank.dungeonName);
                                            cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.Race, _dungeonRank.race);
                                            cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.CharacterId, _dungeonRank.characterID);
                                            cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.CharacterName, _dungeonRank.characterName);
                                            cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.Score, _dungeonRank.score);
                                            cmd.Execute();
                                        }

                                        using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonScoreRankInfo, transaction))
                                        {
                                            cmd.Where(Mabinogi.SQL.Columns.DungeonScoreRankInfo.Server, _dungeonRank.server);
                                            cmd.Where(Mabinogi.SQL.Columns.DungeonScoreRankInfo.DungeonName, _dungeonRank.dungeonName);
                                            cmd.Where(Mabinogi.SQL.Columns.DungeonScoreRankInfo.Race, _dungeonRank.race);
                                            cmd.Set(Mabinogi.SQL.Columns.DungeonScoreRankInfo.ScoreRow, scoreRow + 1);
                                            cmd.Execute();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            using (var chkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonScoreBoard))
                            {
                                chkCmd.ComplexWhere(string.Format("WHERE {0} = @{0} AND {1} = @{1} AND {2} = @{2} AND {3} < @{3}",
                                            Mabinogi.SQL.Columns.DungeonScoreBoard.Server, Mabinogi.SQL.Columns.DungeonScoreBoard.DungeonName, Mabinogi.SQL.Columns.DungeonScoreBoard.Race, Mabinogi.SQL.Columns.DungeonScoreBoard.Score));

                                chkCmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.Server, _dungeonRank.server);
                                chkCmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.DungeonName, _dungeonRank.dungeonName);
                                chkCmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.Race, _dungeonRank.race);
                                chkCmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.Score, _dungeonRank.score);

                                using (var reader = chkCmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        using (var chkCmd2 = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonScoreBoard))
                                        {
                                            chkCmd2.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.Server, _dungeonRank.server);
                                            chkCmd2.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.DungeonName, _dungeonRank.dungeonName);
                                            chkCmd2.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.CharacterId, _dungeonRank.characterID);

                                            using (var reader2 = chkCmd2.ExecuteReader())
                                            {
                                                if (reader2.HasRows)
                                                {
                                                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonScoreBoard, transaction))
                                                    {
                                                        cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.Score, _dungeonRank.score);
                                                        cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.RegDate, now);
                                                        cmd.ComplexWhere(string.Format("WHERE {0} = @{0} AND {1} = @{1} AND {2} = @{2} AND {3} < @{3}",
                                                            Mabinogi.SQL.Columns.DungeonScoreBoard.Server, Mabinogi.SQL.Columns.DungeonScoreBoard.DungeonName, Mabinogi.SQL.Columns.DungeonScoreBoard.CharacterId, Mabinogi.SQL.Columns.DungeonScoreBoard.Score));

                                                        cmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.Server, _dungeonRank.server);
                                                        cmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.DungeonName, _dungeonRank.dungeonName);
                                                        cmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.CharacterId, _dungeonRank.characterID);
                                                        cmd.Execute();
                                                    }
                                                }
                                                else
                                                {
                                                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonScoreBoard, transaction))
                                                    {
                                                        cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.Server, _dungeonRank.server);
                                                        cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.DungeonName, _dungeonRank.dungeonName);
                                                        cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.Race, _dungeonRank.race);
                                                        cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.CharacterId, _dungeonRank.characterID);
                                                        cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.CharacterName, _dungeonRank.characterName);
                                                        cmd.Set(Mabinogi.SQL.Columns.DungeonScoreBoard.Score, _dungeonRank.score);
                                                        cmd.Execute();
                                                    }

                                                    using (var cmd = conn.GetDeleteCommand(string.Format("DELETE FROM {0} WHERE {1} = (SELECT {1} FROM {0} WHERE {2} = @{2} AND {3} = @{3} AND {4} = @{4} ORDER BY {5} LIMIT 1) AND {5} < @{5}",
                                                        Mabinogi.SQL.Tables.DungeonRank.DungeonScoreBoard, Mabinogi.SQL.Columns.DungeonScoreBoard.Idx, Mabinogi.SQL.Columns.DungeonScoreBoard.Server, Mabinogi.SQL.Columns.DungeonScoreBoard.DungeonName, Mabinogi.SQL.Columns.DungeonScoreBoard.Race, Mabinogi.SQL.Columns.DungeonScoreBoard.Score), transaction))
                                                    {
                                                        cmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.Server, _dungeonRank.server);
                                                        cmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.DungeonName, _dungeonRank.dungeonName);
                                                        cmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.Race, _dungeonRank.race);
                                                        cmd.AddParameter(Mabinogi.SQL.Columns.DungeonScoreBoard.Score, _dungeonRank.score);
                                                        cmd.Execute();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }



                    ////////////////////////
                    if (timeRow < 300)
                    {
                        using (var chkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonTimeBoard))
                        {
                            chkCmd.Where(Mabinogi.SQL.Columns.DungeonTimeBoard.Server, _dungeonRank.server);
                            chkCmd.Where(Mabinogi.SQL.Columns.DungeonTimeBoard.DungeonName, _dungeonRank.dungeonName);
                            chkCmd.Where(Mabinogi.SQL.Columns.DungeonTimeBoard.CharacterId, _dungeonRank.characterID);

                            using (var reader = chkCmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonTimeBoard, transaction))
                                    {
                                        cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.LapTime, _dungeonRank.laptime);
                                        cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.RegDate, now);
                                        cmd.ComplexWhere(string.Format("WHERE {0} = @{0} AND {1} = @{1} AND {2} = @{2} AND {3} > @{3}",
                                            Mabinogi.SQL.Columns.DungeonTimeBoard.Server, Mabinogi.SQL.Columns.DungeonTimeBoard.DungeonName, Mabinogi.SQL.Columns.DungeonTimeBoard.CharacterId, Mabinogi.SQL.Columns.DungeonTimeBoard.LapTime));

                                        cmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.Server, _dungeonRank.server);
                                        cmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.DungeonName, _dungeonRank.dungeonName);
                                        cmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.CharacterId, _dungeonRank.characterID);
                                        cmd.Execute();
                                    }
                                }
                                else
                                {
                                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonTimeBoard, transaction))
                                    {
                                        cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.Server, _dungeonRank.server);
                                        cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.DungeonName, _dungeonRank.dungeonName);
                                        cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.Race, _dungeonRank.race);
                                        cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.CharacterId, _dungeonRank.characterID);
                                        cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.CharacterName, _dungeonRank.characterName);
                                        cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.LapTime, _dungeonRank.laptime);
                                        cmd.Execute();
                                    }

                                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonTimeRankInfo, transaction))
                                    {
                                        cmd.Where(Mabinogi.SQL.Columns.DungeonTimeRankInfo.Server, _dungeonRank.server);
                                        cmd.Where(Mabinogi.SQL.Columns.DungeonTimeRankInfo.DungeonName, _dungeonRank.dungeonName);
                                        cmd.Where(Mabinogi.SQL.Columns.DungeonTimeRankInfo.Race, _dungeonRank.race);
                                        cmd.Set(Mabinogi.SQL.Columns.DungeonTimeRankInfo.TimeRow, timeRow + 1);
                                        cmd.Execute();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        using (var chkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonTimeBoard))
                        {
                            chkCmd.ComplexWhere(string.Format("WHERE {0} = @{0} AND {1} = @{1} AND {2} = @{2} AND {3} > @{3}",
                                        Mabinogi.SQL.Columns.DungeonTimeBoard.Server, Mabinogi.SQL.Columns.DungeonTimeBoard.DungeonName, Mabinogi.SQL.Columns.DungeonTimeBoard.Race, Mabinogi.SQL.Columns.DungeonTimeBoard.LapTime));

                            chkCmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.Server, _dungeonRank.server);
                            chkCmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.DungeonName, _dungeonRank.dungeonName);
                            chkCmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.Race, _dungeonRank.race);
                            chkCmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.LapTime, _dungeonRank.laptime);

                            using (var reader = chkCmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    using (var chkCmd2 = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonTimeBoard))
                                    {
                                        chkCmd2.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.Server, _dungeonRank.server);
                                        chkCmd2.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.DungeonName, _dungeonRank.dungeonName);
                                        chkCmd2.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.CharacterId, _dungeonRank.characterID);

                                        using (var reader2 = chkCmd2.ExecuteReader())
                                        {
                                            if (reader2.HasRows)
                                            {
                                                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonTimeBoard, transaction))
                                                {
                                                    cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.LapTime, _dungeonRank.laptime);
                                                    cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.RegDate, now);
                                                    cmd.ComplexWhere(string.Format("WHERE {0} = @{0} AND {1} = @{1} AND {2} = @{2} AND {3} > @{3}",
                                                        Mabinogi.SQL.Columns.DungeonTimeBoard.Server, Mabinogi.SQL.Columns.DungeonTimeBoard.DungeonName, Mabinogi.SQL.Columns.DungeonTimeBoard.CharacterId, Mabinogi.SQL.Columns.DungeonTimeBoard.LapTime));

                                                    cmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.Server, _dungeonRank.server);
                                                    cmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.DungeonName, _dungeonRank.dungeonName);
                                                    cmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.CharacterId, _dungeonRank.characterID);
                                                    cmd.Execute();
                                                }
                                            }
                                            else
                                            {
                                                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.DungeonRank.DungeonTimeBoard, transaction))
                                                {
                                                    cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.Server, _dungeonRank.server);
                                                    cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.DungeonName, _dungeonRank.dungeonName);
                                                    cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.Race, _dungeonRank.race);
                                                    cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.CharacterId, _dungeonRank.characterID);
                                                    cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.CharacterName, _dungeonRank.characterName);
                                                    cmd.Set(Mabinogi.SQL.Columns.DungeonTimeBoard.LapTime, _dungeonRank.laptime);
                                                    cmd.Execute();
                                                }

                                                using (var cmd = conn.GetDeleteCommand(string.Format("DELETE FROM {0} WHERE {1} = (SELECT {1} FROM {0} WHERE {2} = @{2} AND {3} = @{3} AND {4} = @{4} ORDER BY {5} DESC LIMIT 1) AND {5} > @{5}",
                                                    Mabinogi.SQL.Tables.DungeonRank.DungeonTimeBoard, Mabinogi.SQL.Columns.DungeonTimeBoard.Idx, Mabinogi.SQL.Columns.DungeonTimeBoard.Server, Mabinogi.SQL.Columns.DungeonTimeBoard.DungeonName, Mabinogi.SQL.Columns.DungeonTimeBoard.Race, Mabinogi.SQL.Columns.DungeonTimeBoard.LapTime), transaction))
                                                {
                                                    cmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.Server, _dungeonRank.server);
                                                    cmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.DungeonName, _dungeonRank.dungeonName);
                                                    cmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.Race, _dungeonRank.race);
                                                    cmd.AddParameter(Mabinogi.SQL.Columns.DungeonTimeBoard.LapTime, _dungeonRank.laptime);
                                                    cmd.Execute();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _dungeonRank.server, _dungeonRank.characterID, _dungeonRank.dungeonName);
                    WorkSession.WriteStatus(ex.Message);
                    transaction?.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _dungeonRank.server, _dungeonRank.characterID, _dungeonRank.dungeonName);
                    WorkSession.WriteStatus(ex2.Message);
                    transaction?.Rollback();
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("DungeonRankSqlAdapter.Update() : 연결을 종료합니다");
                }
            }
        }
    }
}
