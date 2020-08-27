using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class RecommendAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Recommend;

        public RecommendAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public REPLY_RESULT Insert(string _oldbieCharName, string _oldbieServerId, string _newbieCharName, string _newbieServerId, long _recommendTime)
        {
            WorkSession.WriteStatus("RecommendSqlAdapter.Add() : 함수에 진입하였습니다");

            try
            {
                SimpleTransaction sqlTransaction = null;

                WorkSession.WriteStatus("RecommendSqlAdapter.Add() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    try
                    {
                        // PROCEDURE: InsertRecommendInfo
                        sqlTransaction = conn.BeginTransaction();

                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Recommend, sqlTransaction))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.Recommend.OldbieCharName, _oldbieCharName);
                            cmd.Set(Mabinogi.SQL.Columns.Recommend.OldbieServerId, _oldbieServerId);
                            cmd.Set(Mabinogi.SQL.Columns.Recommend.NewbieCharName, _newbieCharName);
                            cmd.Set(Mabinogi.SQL.Columns.Recommend.NewbieServerId, _newbieServerId);
                            cmd.Set(Mabinogi.SQL.Columns.Recommend.RecommendTime, _recommendTime);

                            WorkSession.WriteStatus("RecommendSqlAdapter.Add() : 명령을 실행합니다");
                            if (cmd.Execute() < 1)
                            {
                                WorkSession.WriteStatus("RecommendSqlAdapter.Add() : 트랜잭션을 롤백합니다");
                                sqlTransaction.Rollback();
                                return REPLY_RESULT.FAIL;
                            }
                        }
                        WorkSession.WriteStatus("RecommendSqlAdapter.Add() : 트랜잭션을 커밋합니다");
                        sqlTransaction.Commit();
                        return REPLY_RESULT.SUCCESS;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, _newbieCharName, _newbieServerId);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        if (sqlTransaction != null)
                        {
                            WorkSession.WriteStatus("RecommendSqlAdapter.Add() : 트랜잭션을 롤백합니다");
                            sqlTransaction.Rollback();
                        }
                        return REPLY_RESULT.FAIL;
                    }
                    catch (Exception ex2)
                    {
                        ExceptionMonitor.ExceptionRaised(ex2, _newbieCharName, _newbieServerId);
                        WorkSession.WriteStatus(ex2.Message);
                        if (sqlTransaction != null)
                        {
                            WorkSession.WriteStatus("RecommendSqlAdapter.Add() : 트랜잭션을 롤백합니다");
                            sqlTransaction.Rollback();
                        }
                        return REPLY_RESULT.FAIL;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("RecommendSqlAdapter.Add() : 연결을 종료합니다");
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _newbieCharName, _newbieServerId);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return REPLY_RESULT.FAIL;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _newbieCharName, _newbieServerId);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.FAIL;
            }
        }

        public REPLY_RESULT Update(string _newbieCharName, string _newbieServerId, byte _flagNum, long _markTime)
        {
            WorkSession.WriteStatus("RecommendSqlAdapter.Update() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("RecommendSqlAdapter.Update() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    SimpleTransaction sqlTransaction = null;
                    try
                    {
                        sqlTransaction = conn.BeginTransaction();

                        // PROCEDURE: UpdateRecommendInfo (missing)
                        using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Recommend, sqlTransaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.Recommend.NewbieCharName, _newbieCharName);
                            cmd.Where(Mabinogi.SQL.Columns.Recommend.NewbieServerId, _newbieServerId);

                            cmd.Set(Mabinogi.SQL.Columns.Recommend.FlagTime[_flagNum - 1], _markTime);

                            WorkSession.WriteStatus("RecommendSqlAdapter.Update() : 명령을 실행합니다");
                            cmd.Execute();
                        }

                        WorkSession.WriteStatus("RecommendSqlAdapter.Update() : 트랜잭션을 커밋합니다");
                        sqlTransaction.Commit();
                        return REPLY_RESULT.SUCCESS;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, _newbieCharName, _newbieServerId);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        if (sqlTransaction != null)
                        {
                            WorkSession.WriteStatus("RecommendSqlAdapter.Update() : 트랜잭션을 롤백합니다");
                            sqlTransaction.Rollback();
                        }
                        return REPLY_RESULT.FAIL;
                    }
                    catch (Exception ex2)
                    {
                        ExceptionMonitor.ExceptionRaised(ex2, _newbieCharName, _newbieServerId);
                        WorkSession.WriteStatus(ex2.Message);
                        if (sqlTransaction != null)
                        {
                            WorkSession.WriteStatus("RecommendSqlAdapter.Update() : 트랜잭션을 롤백합니다");
                            sqlTransaction.Rollback();
                        }
                        return REPLY_RESULT.FAIL;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("RecommendSqlAdapter.Update() : 연결을 종료합니다");
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _newbieCharName, _newbieServerId);
                WorkSession.WriteStatus(ex.Message, ex.Number);

                return REPLY_RESULT.FAIL;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _newbieCharName, _newbieServerId);
                WorkSession.WriteStatus(ex2.Message);

                return REPLY_RESULT.FAIL;
            }
        }

        public Recommend Select(string _newbieCharName, string _newbieServerId)
        {
            WorkSession.WriteStatus("RecommendSqlAdapter.Select() : 함수에 진입하였습니다.");


            try
            {
                // PROCEDURE: SelectRecommend
                WorkSession.WriteStatus("RecommendSqlAdapter.Select() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Recommend))
                {
                    cmd.Where(Mabinogi.SQL.Columns.Recommend.NewbieCharName, _newbieCharName);
                    cmd.Where(Mabinogi.SQL.Columns.Recommend.NewbieServerId, _newbieServerId);

                    WorkSession.WriteStatus("RecommendSqlAdapter.Select() : 데이터를 채웁니다.");
                    using (var reader = cmd.ExecuteReader())
                        return Build(reader);
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _newbieCharName, _newbieServerId);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _newbieCharName, _newbieServerId);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("RecommendSqlAdapter.Select() : 데이터 베이스에 연결을 종료합니다.");
            }
        }

        public RecommendList SelectList(string _oldbieCharName, string _oldbieServerId)
        {
            WorkSession.WriteStatus("RecommendSqlAdapter.SelectList() : 함수에 진입하였습니다.");

            try
            {
                WorkSession.WriteStatus("RecommendSqlAdapter.SelectList() : 데이터 베이스에 연결합니다.");
                // PROCEDURE: SelectRecommendList
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Recommend))
                {
                    cmd.Where(Mabinogi.SQL.Columns.Recommend.OldbieCharName, _oldbieCharName);
                    cmd.Where(Mabinogi.SQL.Columns.Recommend.OldbieServerId, _oldbieServerId);

                    WorkSession.WriteStatus("RecommendSqlAdapter.SelectList() : 데이터를 채웁니다.");

                    using (var reader = cmd.ExecuteReader())
                    {
                        RecommendList result = new RecommendList();
                        if (reader.HasRows)
                        {
                            result.recommendList = new List<Recommend>();
                            while (reader.Read())
                            {
                                result.recommendList.Add(Build(reader));
                            }
                        }
                        return result;
                    }
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
                WorkSession.WriteStatus("RecommendSqlAdapter.SelectList() : 데이터 베이스에 연결을 종료합니다.");
            }
            return null;
        }

        private Recommend Build(SimpleReader reader)
        {
            if (reader == null)
            {
                throw new Exception("추천 테이블을 얻어오지 못햇습니다.");
            }
            if (reader.Read())
            {
                Recommend recommend = new Recommend();
                recommend.oldbieCharName = reader.GetString(Mabinogi.SQL.Columns.Recommend.OldbieCharName);
                recommend.oldbieServerId = reader.GetString(Mabinogi.SQL.Columns.Recommend.OldbieServerId);
                recommend.recommendTime = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.RecommendTime);
                recommend.flagTime1 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime1);
                recommend.flagTime2 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime2);
                recommend.flagTime3 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime3);
                recommend.flagTime4 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime4);
                recommend.flagTime5 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime5);
                recommend.flagTime6 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime6);
                recommend.flagTime7 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime7);
                recommend.flagTime8 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime8);
                recommend.flagTime9 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime9);
                recommend.flagTime10 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime10);
                recommend.flagTime11 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime11);
                recommend.flagTime12 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime12);
                recommend.flagTime13 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime13);
                recommend.flagTime14 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime14);
                recommend.flagTime15 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime15);
                recommend.flagTime16 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime16);
                recommend.flagTime17 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime17);
                recommend.flagTime18 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime18);
                recommend.flagTime19 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime19);
                recommend.flagTime20 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime20);
                recommend.flagTime21 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime21);
                recommend.flagTime22 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime22);
                recommend.flagTime23 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime23);
                recommend.flagTime24 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime24);
                recommend.flagTime25 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime25);
                recommend.flagTime26 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime26);
                recommend.flagTime27 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime27);
                recommend.flagTime28 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime28);
                recommend.flagTime29 = reader.GetInt64(Mabinogi.SQL.Columns.Recommend.FlagTime29);
                return recommend;
            }
            return null;
        }
    }
}
