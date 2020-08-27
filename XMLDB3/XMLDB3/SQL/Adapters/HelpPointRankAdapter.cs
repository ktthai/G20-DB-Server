using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class HelpPointRankAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.HelpPointRanking;
        public HelpPointRankAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public REPLY_RESULT UpdateHelpPoint(HelpPointRankList _UpdateRankList)
        {
            WorkSession.WriteStatus("HelpPointRankAdapter.UpdateHelpPoint() : 함수에 진입하였습니다");

            if (_UpdateRankList == null || _UpdateRankList.HelpPointRanks == null)
                return REPLY_RESULT.FAIL;

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("HelpPointRankAdapter.UpdateHelpPoint() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {
                    transaction = conn.BeginTransaction();

                    foreach (HelpPointRank helpPointRank in _UpdateRankList.HelpPointRanks)
                    {
                        WorkSession.WriteStatus("HelpPointRankAdapter.UpdateHelpPoint() : 명령을 실행합니다");

                        // PROCEDURE: UpdateHelpPointRanking
                        using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.HelpPointRanking, transaction))
                        {
                            upCmd.Where(Mabinogi.SQL.Columns.HelpPointRanking.CharId, helpPointRank.charId);
                            upCmd.Set(Mabinogi.SQL.Columns.HelpPointRanking.Score1, helpPointRank.NormalHelpPoint);
                            upCmd.Set(Mabinogi.SQL.Columns.HelpPointRanking.Score2, helpPointRank.AccumulatedHelpPoint);
                            upCmd.Set(Mabinogi.SQL.Columns.HelpPointRanking.UpdateTime, helpPointRank.lastUpdate);

                            if (upCmd.Execute() < 1)
                            {
                                using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.HelpPointRanking, transaction))
                                {
                                    insCmd.Set(Mabinogi.SQL.Columns.HelpPointRanking.CharId, helpPointRank.charId);
                                    insCmd.Set(Mabinogi.SQL.Columns.HelpPointRanking.Score1, helpPointRank.NormalHelpPoint);
                                    insCmd.Set(Mabinogi.SQL.Columns.HelpPointRanking.Score2, helpPointRank.AccumulatedHelpPoint);
                                    insCmd.Set(Mabinogi.SQL.Columns.HelpPointRanking.UpdateTime, helpPointRank.lastUpdate);

                                    if (insCmd.Execute() < 1)
                                    {
                                        transaction.Rollback();
                                        return REPLY_RESULT.FAIL;
                                    }
                                }
                            }
                        }
                    }
                    WorkSession.WriteStatus("HelpPointRankAdapter.UpdateHelpPoint() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;

                }
                catch (SimpleSqlException ex2)
                {
                    WorkSession.WriteStatus(ex2.Message, ex2.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("HelpPointRankAdapter.UpdateHelpPoint() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex3)
                {
                    WorkSession.WriteStatus(ex3.Message);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("HelpPointRankAdapter.UpdateHelpPoint() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("HelpPointRankAdapter.UpdateHelpPoint() : 연결을 종료합니다");

                }
            }
        }

        public HelpPointRankList ReadList()
        {
            WorkSession.WriteStatus("HelpPointRankSqlAdapter.ReadList() : 함수에 진입하였습니다.");
            try
            {                
                WorkSession.WriteStatus("HelpPointRankSqlAdapter.ReadList() : 데이터 베이스에 연결합니다.");
                // PROCEDURE: SelectHelpPointRanking
                using (var conn = Connection)
                using (var cmd = conn.GetSelectCommand(string.Format("SELECT P.{0}, C.{1}, P.{2}, P.{3}, P.{4} FROM `{5}` P, `{6}` C WHERE P.{0} = C.{7} AND C.{8} IS NULL;",
                    /*0*/ Mabinogi.SQL.Columns.HelpPointRanking.CharId, /*1*/ Mabinogi.SQL.Columns.Character.Name, /*2*/ Mabinogi.SQL.Columns.HelpPointRanking.Score1, /*3*/ Mabinogi.SQL.Columns.HelpPointRanking.Score2, /*4*/ Mabinogi.SQL.Columns.HelpPointRanking.UpdateTime,
                    /*5*/ Mabinogi.SQL.Tables.Mabinogi.HelpPointRanking, /*6*/ Mabinogi.SQL.Tables.Mabinogi.Character, /*7*/ Mabinogi.SQL.Columns.Character.Id, /*8*/ Mabinogi.SQL.Columns.Character.DeleteTime)))
                {
                    return Build(cmd.ExecuteReader());
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
                WorkSession.WriteStatus("HelpPointRankSqlAdapter.ReadList() : 데이터 베이스에 연결을 종료합니다.");
            }
        }

        private HelpPointRankList Build(SimpleReader reader)
        {
            if (reader == null)
            {
                throw new Exception("HelpPointRanking 테이블을 얻어오지 못햇습니다.");
            }
            if (reader.HasRows)
            {
                HelpPointRankList helpPointRankList = new HelpPointRankList();
                List<HelpPointRank> list = new List<HelpPointRank>();
                while(reader.Read())
                {
                    HelpPointRank helpPointRank = new HelpPointRank();
                    helpPointRank.charId = (ulong)reader.GetInt64(Mabinogi.SQL.Columns.HelpPointRanking.CharId);
                    helpPointRank.charName = reader.GetString(Mabinogi.SQL.Columns.Character.Name);
                    helpPointRank.NormalHelpPoint = reader.GetInt32(Mabinogi.SQL.Columns.HelpPointRanking.Score1);
                    helpPointRank.AccumulatedHelpPoint = reader.GetInt32(Mabinogi.SQL.Columns.HelpPointRanking.Score2);
                    helpPointRank.lastUpdate = reader.GetDateTime(Mabinogi.SQL.Columns.HelpPointRanking.UpdateTime);
                    list.Add(helpPointRank);
                }
                helpPointRankList.HelpPointRanks = list.ToArray();
                return helpPointRankList;
            }
            return new HelpPointRankList();
        }
    }
}
