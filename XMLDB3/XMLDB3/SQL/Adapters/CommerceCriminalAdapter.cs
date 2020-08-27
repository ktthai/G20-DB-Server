using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class CommerceCriminalAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.CommerceCriminal;

        public CommerceCriminalAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public REPLY_RESULT ReadAll(string _serverName, out CommerceCriminals _criminal)
        {
            WorkSession.WriteStatus("CommerceCriminalSqlAdapter.Read() : 함수에 진입하였습니다");

            _criminal = new CommerceCriminals();
            _criminal.criminalTable = new Dictionary<int, CommerceCriminal>();
            try
            {
                using (var conn = Connection)
                {
                    // PROCEDURE dbo.QueryAllCommerceCriminal

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CommerceCriminalReward))
                    using (var rewardReader = cmd.ExecuteReader())
                    {

                        WorkSession.WriteStatus("CommerceCriminalSqlAdapter.Read() : 데이터베이스와 연결합니다");
                        _criminal = BuildCriminalReward(rewardReader);
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CommerceCriminal))
                    using (var crimReader = cmd.ExecuteReader())
                    {
                        WorkSession.WriteStatus("CommerceCriminalSqlAdapter.Read() : 데이타를 채웁니다.");
                        BuildCriminal(crimReader, _criminal);
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
            finally
            {
                WorkSession.WriteStatus("CommerceSystemSqlAdapter.Read() : 연결을 종료합니다");
            }
            return REPLY_RESULT.SUCCESS;
        }

        public REPLY_RESULT UpdateCriminalInfo(string _serverName, int _bossID, int _reward, CommerceCriminalInfo _newInfo)
        {
            WorkSession.WriteStatus("CommerceCriminalSqlAdapter.UpdateCriminalInfo() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();
                    foreach (CCommerceCriminalLost value in _newInfo.criminalTable.Values)
                    {
                        CommerceCriminalUpdateBuilder.Build(_bossID, value.charName, value.stolenDucat, conn, transaction);
                    }
                    CommerceCriminalRewardUpdateBuilder.Build(_bossID, _reward, conn, transaction);
                    WorkSession.WriteStatus("CommerceCriminalSqlAdapter.UpdateCriminalInfo() : 데이터베이스와 연결합니다");

                    WorkSession.WriteStatus("CommerceCriminalSqlAdapter.UpdateCriminalInfo() : 명령을 실행합니다");
                    transaction.Commit();


                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2);
                    WorkSession.WriteStatus(ex2.Message);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("CommerceCriminalSqlAdapter.Update() : 연결을 종료합니다");
                }
            }
            return REPLY_RESULT.SUCCESS;
        }

        public REPLY_RESULT RemoveBossData(string _serverName, int _bossId)
        {
            WorkSession.WriteStatus("CommerceCriminalSqlAdapter.RemoveBossData() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            using (var conn = Connection)
            {
                try
                {
                    transaction = conn.BeginTransaction();
                    CommerceCriminalRemoveBossBuilder.Build(_bossId, conn, transaction);
                    CommerceCriminalRewardUpdateBuilder.Build(_bossId, 0, conn, transaction);

                    WorkSession.WriteStatus("CommerceCriminalSqlAdapter.RemoveBossData() : 데이터베이스와 연결합니다");

                    WorkSession.WriteStatus("CommerceCriminalSqlAdapter.RemoveBossData() : 명령을 실행합니다");
                    transaction.Commit();
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2);
                    WorkSession.WriteStatus(ex2.Message);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("CommerceCriminalSqlAdapter.RemoveBossData() : 연결을 종료합니다");
                }
            }
            return REPLY_RESULT.SUCCESS;
        }

        private CommerceCriminals BuildCriminalReward(SimpleReader rewardReader)
        {
            if (rewardReader == null)
            {
                throw new Exception("현상범 테이블을 얻어오지 못햇습니다.");
            }
            CommerceCriminals commerceCriminals = new CommerceCriminals();
            commerceCriminals.criminalTable = new Dictionary<int, CommerceCriminal>();
            CommerceCriminal commerceCriminal;

            while (rewardReader.Read())
            {
                commerceCriminal = new CommerceCriminal();
                commerceCriminal.id = rewardReader.GetInt32(Mabinogi.SQL.Columns.CommerceCriminalReward.CriminalId);
                commerceCriminal.reward = rewardReader.GetInt32(Mabinogi.SQL.Columns.CommerceCriminalReward.Reward);

                if (commerceCriminals.criminalTable.ContainsKey(commerceCriminal.id))
                {
                    commerceCriminals.criminalTable.Remove(commerceCriminal.id);
                }
                commerceCriminals.criminalTable.Add(commerceCriminal.id, commerceCriminal);
            }
            return commerceCriminals;
        }

        private void BuildCriminal(SimpleReader crimReader, CommerceCriminals commerceCriminals)
        {
            if (crimReader == null)
            {
                throw new Exception("현상범 테이블을 얻어오지 못햇습니다.");
            }

            CCommerceCriminalLost cCommerceCriminalLost;
            while (crimReader.Read())
            {
                cCommerceCriminalLost = new CCommerceCriminalLost();
                int num = crimReader.GetInt32(Mabinogi.SQL.Columns.CommerceCriminal.CriminalId);
                cCommerceCriminalLost.charName = crimReader.GetString(Mabinogi.SQL.Columns.CommerceCriminal.CharName);
                cCommerceCriminalLost.stolenDucat = crimReader.GetInt32(Mabinogi.SQL.Columns.CommerceCriminal.Ducat);
                if (!commerceCriminals.criminalTable.ContainsKey(num))
                {
                    CommerceCriminal value = new CommerceCriminal();
                    commerceCriminals.criminalTable.Add(num, value);
                }
                CommerceCriminal commerceCriminal2 = commerceCriminals.criminalTable[num];
                if (commerceCriminal2.info == null)
                {
                    commerceCriminal2.info = new CommerceCriminalInfo();
                }
                if (commerceCriminal2.info.criminalTable == null)
                {
                    commerceCriminal2.info.criminalTable = new Dictionary<string, CCommerceCriminalLost>();
                }
                if (commerceCriminal2.info.criminalTable.ContainsKey(cCommerceCriminalLost.charName))
                {
                    commerceCriminal2.info.criminalTable.Remove(cCommerceCriminalLost.charName);
                }
                commerceCriminal2.info.criminalTable.Add(cCommerceCriminalLost.charName, cCommerceCriminalLost);
            }
        }
    }
}
