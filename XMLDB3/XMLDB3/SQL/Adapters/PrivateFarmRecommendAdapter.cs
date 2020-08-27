using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class PrivateFarmRecommendAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.PrivateFarmRecommend;

        public PrivateFarmRecommendAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        // Do not have procedures or tables for most of this, so many guesses
        public bool Add(string _CharName, long _ZoneID)
        {
            WorkSession.WriteStatus("PrivateFarmRecommendSqlAdapter.Add() : 함수에 진입하였습니다");
            try
            {
                try
                {
                    WorkSession.WriteStatus("PrivateFarmRecommendSqlAdapter.Create() : 데이터베이스와 연결합니다");
                    // PROCEDURE: InsertPrivateFarmRecommend
                    using (var conn = Connection)
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmRecommend))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.PrivateFarmRecommend.FarmId, _ZoneID);
                        cmd.Set(Mabinogi.SQL.Columns.PrivateFarmRecommend.OwnerName, _CharName);

                        WorkSession.WriteStatus("PrivateFarmRecommendSqlAdapter.Add() : 명령을 실행합니다");
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
                    WorkSession.WriteStatus("PrivateFarmRecommendSqlAdapter.SetFlag() : 연결을 종료합니다");
                }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
        }

        public PrivateFarmRecommendList ReadList()
        {
            WorkSession.WriteStatus("PrivateFarmRecommendSqlAdapter.ReadList() : 함수에 진입하였습니다.");

            try
            {
                // PROCEDURE: SelectPrivateFarmRecommend

                WorkSession.WriteStatus("PrivateFarmRecommendSqlAdapter.ReadList() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                {
                    WorkSession.WriteStatus("PrivateFarmRecommendSqlAdapter.ReadList() : 데이터를 채웁니다.");
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmRecommend))
                    using (var reader = cmd.ExecuteReader())
                    {
                        return Build(reader);
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
                WorkSession.WriteStatus("PrivateFarmRecommendSqlAdapter.ReadList() : 데이터 베이스에 연결을 종료합니다.");
            }

            return null;
        }

        public bool DeleteAllList()
        {
            WorkSession.WriteStatus("PrivateFarmRecommendSqlAdapter.DeleteAllList() : 함수에 진입하였습니다");
            try
            {
                // PROCEDURE: DeleteAllPrivateFarmRecommend
                WorkSession.WriteStatus("PrivateFarmRecommendSqlAdapter.DeleteAllList() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmRecommend))
                {
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
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("PrivateFarmRecommendSqlAdapter.DeleteAllList() : 연결을 종료합니다");
            }
        }

        public REPLY_RESULT QueryZoneId(string _strCharName, out long _idZone)
        {
            _idZone = 0L;
            return REPLY_RESULT.ERROR;
        }

        private PrivateFarmRecommendList Build(SimpleReader reader)
        {
            if (reader == null)
            {
                throw new Exception("추천 농장 테이블을 얻어오지 못햇습니다.");
            }
            if (reader.HasRows)
            {
                PrivateFarmRecommendList privateFarmRecommendList = new PrivateFarmRecommendList();
                privateFarmRecommendList.recommendList = new List<PrivateFarmRecommend>();
                PrivateFarmRecommend privateFarmRecommend;
                while (reader.Read())
                {
                    privateFarmRecommend = new PrivateFarmRecommend();
                    privateFarmRecommend.FarmZoneID = reader.GetInt64(Mabinogi.SQL.Columns.PrivateFarmRecommend.FarmId);
                    privateFarmRecommend.OwnerCharName = reader.GetString(Mabinogi.SQL.Columns.PrivateFarmRecommend.OwnerName);

                    privateFarmRecommendList.recommendList.Add(privateFarmRecommend);
                }
                return privateFarmRecommendList;
            }
            return new PrivateFarmRecommendList();
        }
    }
}
