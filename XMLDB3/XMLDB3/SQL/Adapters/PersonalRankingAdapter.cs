using Mabinogi.SQL;
using System;
using System.Collections.Generic;

namespace XMLDB3
{
    public class PersonalRankingAdapter : SqlAdapter
    {
        private const string Database = "mabinogi.";
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.PersonalRanking;
        public PersonalRankingAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public PersonalRankingTable GetRankingTable(PersonalRanking.EId _rankingId, PersonalRanking.EScoreSortType _sortType, DateTime _rankingCycleStartTime, DateTime _currentTime)
        {
            return PersonalRankingTable.GetTable(this, _rankingId, _sortType, _rankingCycleStartTime, null, _currentTime);
        }

        public PersonalRankingList ReadList(PersonalRanking.EId _rankingId, DateTime _rankingCycleStartTime, DateTime? _rankingCycleEndTime)
        {
            WorkSession.WriteStatus("PersonalRankingAdapter.ReadList() : 함수에 진입하였습니다.");

            try
            {
                WorkSession.WriteStatus("PersonalRankingAdapter.ReadList() : 데이터 베이스에 연결합니다.");
                // PROCEDURE: SelectPersonalRanking
                using (var conn = Connection)
                using (var cmd = conn.GetSelectCommand(string.Format("SELECT p.{0}, p.{1}, c.{2}, p.{3}, p.{4} FROM {5} AS p INNER JOIN {6} AS c ON p.{1} = c.{7} AND p.{0} = @{0}",
                    Mabinogi.SQL.Columns.PersonalRanking.RankingId, Mabinogi.SQL.Columns.PersonalRanking.CharId, Mabinogi.SQL.Columns.Character.Name, Mabinogi.SQL.Columns.PersonalRanking.Score, Mabinogi.SQL.Columns.PersonalRanking.LastUpdate, Mabinogi.SQL.Tables.Mabinogi.PersonalRanking, Database + Mabinogi.SQL.Tables.Mabinogi.Character, Mabinogi.SQL.Columns.Character.Id)))
                {
                    cmd.AddParameter(Mabinogi.SQL.Columns.PersonalRanking.RankingId, (uint)_rankingId);

                    WorkSession.WriteStatus("PersonalRankingAdapter.ReadList() : 데이터를 채웁니다.");
                    return Build(cmd.ExecuteReader(), _rankingCycleStartTime, _rankingCycleEndTime);
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
                WorkSession.WriteStatus("PersonalRankingAdapter.ReadList() : 데이터 베이스에 연결을 종료합니다.");
            }
            return null;
        }

        public PersonalRanking Read(PersonalRanking.EId _rankingId, ulong _charId)
        {
            WorkSession.WriteStatus("PersonalRankingAdapter.Read() : 함수에 진입하였습니다.");

            try
            {
                WorkSession.WriteStatus("PersonalRankingAdapter.Read() : 데이터 베이스에 연결합니다.");
                // PROCEDURE: ReadPersonalRanking
                using (var conn = Connection)
                using (var cmd = conn.GetSelectCommand(string.Format("SELECT p.{0}, p.{1}, c.{2}, p.{3}, p.{4} FROM {5} AS p INNER JOIN {6} AS c ON p.{1} = c.{7} AND p.{0} = @{0} AND p.{1} = @{1}",
                    /*0*/Mabinogi.SQL.Columns.PersonalRanking.RankingId, /*1*/Mabinogi.SQL.Columns.PersonalRanking.CharId, /*2*/Mabinogi.SQL.Columns.Character.Name, /*3*/Mabinogi.SQL.Columns.PersonalRanking.Score, /*4*/Mabinogi.SQL.Columns.PersonalRanking.LastUpdate,
                    /*5*/Mabinogi.SQL.Tables.Mabinogi.PersonalRanking, /*6*/ Database + Mabinogi.SQL.Tables.Mabinogi.Character,/*7*/Mabinogi.SQL.Columns.Character.Id)))
                {
                    WorkSession.WriteStatus("PersonalRankingAdapter.Read() : 데이터를 채웁니다.");

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            PersonalRanking personalRanking = new PersonalRanking();
                            personalRanking.rankingId = (uint)_rankingId;
                            personalRanking.charId = _charId;
                            personalRanking.score = reader.GetInt32(Mabinogi.SQL.Columns.PersonalRanking.Score);
                            personalRanking.lastUpdate = reader.GetDateTime(Mabinogi.SQL.Columns.PersonalRanking.LastUpdate);
                            PersonalRankingTableSerializer.CharIdNameMap[_charId] = reader.GetString(Mabinogi.SQL.Columns.Character.Name);
                            return personalRanking;
                        }
                    }
                    return null;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _rankingId, _charId);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _rankingId, _charId);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("PersonalRankingAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");
            }
        }

        private PersonalRankingList Build(SimpleReader reader, DateTime _rankingCycleStartTime, DateTime? _rankingCycleEndTime)
        {
            if (reader == null)
            {
                throw new Exception("PersonalRanking 테이블을 얻어오지 못햇습니다.");
            }
            if (reader.HasRows)
            {
                PersonalRankingList personalRankingList = new PersonalRankingList();
                List<PersonalRanking> list = new List<PersonalRanking>();
                PersonalRanking personalRanking;
                while (reader.Read())
                {
                    personalRanking = new PersonalRanking();
                    personalRanking.rankingId = (uint)reader.GetInt32(Mabinogi.SQL.Columns.PersonalRanking.RankingId);
                    personalRanking.charId = (ulong)reader.GetInt64(Mabinogi.SQL.Columns.PersonalRanking.CharId);
                    personalRanking.score = reader.GetInt32(Mabinogi.SQL.Columns.PersonalRanking.Score);
                    personalRanking.lastUpdate = reader.GetDateTime(Mabinogi.SQL.Columns.PersonalRanking.LastUpdate);
                    PersonalRankingTableSerializer.CharIdNameMap[personalRanking.charId] = reader.GetString(Mabinogi.SQL.Columns.Character.Name);
                    if (personalRanking.IsTimeValid(_rankingCycleStartTime, _rankingCycleEndTime))
                    {
                        list.Add(personalRanking);
                    }
                }
                personalRankingList.Items = list.ToArray();
                return personalRankingList;
            }
            return new PersonalRankingList();
        }

        public REPLY_RESULT RemoveScore(PersonalRanking.EId _rankingId, ulong _charId, ref byte _errorCode)
        {
            WorkSession.WriteStatus("PersonalRankingAdapter.RemoveScore() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("PersonalRankingAdapter.RemoveScore() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();

                    // PROCEDURE: RemovePersonalRanking
                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.PersonalRanking, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.PersonalRanking.RankingId, (int)_rankingId);
                        cmd.Where(Mabinogi.SQL.Columns.PersonalRanking.CharId, _charId);

                        WorkSession.WriteStatus("PersonalRankingAdapter.RemoveScore() : 명령을 실행합니다");
                        cmd.Execute();
                    }


                    WorkSession.WriteStatus("PersonalRankingAdapter.RemoveScore() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("PersonalRankingAdapter.RemoveScore() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2);
                    WorkSession.WriteStatus(ex2.Message);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("PersonalRankingAdapter.RemoveScore() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("PersonalRankingAdapter.RemoveScore() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT Clear(PersonalRanking.EId _rankingId)
        {
            WorkSession.WriteStatus("PersonalRankingAdapter.Clear() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("PersonalRankingAdapter.Clear() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {
                    transaction = conn.BeginTransaction();

                    // PROCEDURE: ClearPersonalRanking
                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.PersonalRanking, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.PersonalRanking.RankingId, (int)_rankingId);

                        WorkSession.WriteStatus("PersonalRankingAdapter.Clear() : 명령을 실행합니다");
                        cmd.Execute();
                    }

                    WorkSession.WriteStatus("PersonalRankingAdapter.Clear() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("PersonalRankingAdapter.Clear() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2);
                    WorkSession.WriteStatus(ex2.Message);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("PersonalRankingAdapter.Clear() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("PersonalRankingAdapter.Clear() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT UpdateScore(PersonalRanking.EId _rankingId, PersonalRanking.EScoreSortType _sortType, DateTime _rankingCycleStartTime, DateTime _currentTime, ulong _charId, int _score, ref byte _errorCode)
        {

            WorkSession.WriteStatus("PersonalRankingAdapter.UpdateScore() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("PersonalRankingAdapter.UpdateScore() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {
                    bool result = false;

                    transaction = conn.BeginTransaction();
                    //PROCEDURE: UpdatePersonalRanking
                    using (var selCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PersonalRanking, transaction))
                    {
                        selCmd.Set(Mabinogi.SQL.Columns.PersonalRanking.Score, 0);
                        selCmd.Set(Mabinogi.SQL.Columns.PersonalRanking.LastUpdate, 0);

                        selCmd.Where(Mabinogi.SQL.Columns.PersonalRanking.RankingId, (int)_rankingId);
                        selCmd.Where(Mabinogi.SQL.Columns.PersonalRanking.CharId, _charId);
                        selCmd.Limit(1);

                        WorkSession.WriteStatus("PersonalRankingAdapter.UpdateScore() : 명령을 실행합니다");

                        using (var reader = selCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int score = reader.GetInt32(Mabinogi.SQL.Columns.PersonalRanking.Score);
                                DateTime updateTime = reader.GetDateTime(Mabinogi.SQL.Columns.PersonalRanking.LastUpdate);

                                if (_sortType == PersonalRanking.EScoreSortType.highScoreWin)
                                {
                                    if (score <= _score || updateTime < _rankingCycleStartTime)
                                    {
                                        using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.PersonalRanking, transaction))
                                        {
                                            cmd.Set(Mabinogi.SQL.Columns.PersonalRanking.Score, _score);
                                            cmd.Set(Mabinogi.SQL.Columns.PersonalRanking.LastUpdate, DateTime.Now);

                                            cmd.Where(Mabinogi.SQL.Columns.PersonalRanking.RankingId, (int)_rankingId);
                                            cmd.Where(Mabinogi.SQL.Columns.PersonalRanking.CharId, _charId);
                                            result = cmd.Execute() > 0;
                                        }
                                    }
                                }
                                else if (_sortType == PersonalRanking.EScoreSortType.lowScoreWin)
                                {
                                    if (score >= _score || updateTime < _rankingCycleStartTime)
                                    {
                                        using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.PersonalRanking, transaction))
                                        {
                                            cmd.Set(Mabinogi.SQL.Columns.PersonalRanking.Score, _score);
                                            cmd.Set(Mabinogi.SQL.Columns.PersonalRanking.LastUpdate, DateTime.Now);

                                            cmd.Where(Mabinogi.SQL.Columns.PersonalRanking.RankingId, (int)_rankingId);
                                            cmd.Where(Mabinogi.SQL.Columns.PersonalRanking.CharId, _charId);
                                            result = cmd.Execute() > 0;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.PersonalRanking, transaction))
                                {
                                    cmd.Set(Mabinogi.SQL.Columns.PersonalRanking.Score, _score);
                                    cmd.Set(Mabinogi.SQL.Columns.PersonalRanking.LastUpdate, DateTime.Now);

                                    cmd.Set(Mabinogi.SQL.Columns.PersonalRanking.RankingId, (int)_rankingId);
                                    cmd.Set(Mabinogi.SQL.Columns.PersonalRanking.CharId, _charId);
                                    result = cmd.Execute() > 0;
                                }
                            }
                        }
                    }


                    if (result)
                    {
                        WorkSession.WriteStatus("PersonalRankingAdapter.UpdateScore() : 트랜잭션을 커밋합니다");
                        transaction.Commit();
                        return REPLY_RESULT.SUCCESS;
                    }
                    Exception ex = new Exception("PersonalRanking 업데이트를 실패하였습니다.", null);
                    ExceptionMonitor.ExceptionRaised(ex, _rankingId, _charId);
                    WorkSession.WriteStatus(ex.Message, 0);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("PersonalRankingAdapter.UpdateScore() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;


                }
                catch (SimpleSqlException ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _rankingId, _charId);
                    WorkSession.WriteStatus(ex2.Message, ex2.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("PersonalRankingAdapter.UpdateScore() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex3)
                {
                    ExceptionMonitor.ExceptionRaised(ex3, _rankingId, _charId);
                    WorkSession.WriteStatus(ex3.Message);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("PersonalRankingAdapter.UpdateScore() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("PersonalRankingAdapter.UpdateScore() : 연결을 종료합니다");
                }
            }
        }
    }
}
