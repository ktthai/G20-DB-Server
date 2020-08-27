using System;
using System.Collections;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class BestCookAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.ScrapBookBestCook;


        public BestCookAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public REPLY_RESULT QueryBestCookList(out Hashtable _bestCookList)
        {
            _bestCookList = null;
            WorkSession.WriteStatus("BestCookSqlAdapter.QueryBestCookList() : 함수에 진입하였습니다.");
            //SqlConnection sqlConnection = new SqlConnection(base.ConnectionString);
            try
            {

                WorkSession.WriteStatus("BestCookSqlAdapter.QueryBestCookList() : 데이터 베이스에 연결합니다.");

                using (var reader = Connection.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.ScrapBookBestCook).ExecuteReader())
                {
                    WorkSession.WriteStatus("BestCookSqlAdapter.QueryBestCookList() : 데이터를 채웁니다.");
                    _bestCookList = new Hashtable();

                    while (reader.Read())
                    {
                        BestCookData bestCookData = new BestCookData();
                        bestCookData.classId = reader.GetInt32(Mabinogi.SQL.Columns.ScrapBookBestCook.ClassId);
                        bestCookData.characterId = reader.GetInt64(Mabinogi.SQL.Columns.ScrapBookBestCook.CharId);
                        bestCookData.characterName = reader.GetString(Mabinogi.SQL.Columns.ScrapBookBestCook.Name);
                        bestCookData.quality = reader.GetInt16(Mabinogi.SQL.Columns.ScrapBookBestCook.Quality);
                        bestCookData.comment = reader.GetString(Mabinogi.SQL.Columns.ScrapBookBestCook.Comment);
                        bestCookData.updateTime = reader.GetDateTime(Mabinogi.SQL.Columns.ScrapBookBestCook.UpdateTime);

                        _bestCookList.Add(bestCookData.classId, bestCookData);
                    }
                    return REPLY_RESULT.SUCCESS;
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
                WorkSession.WriteStatus("BestCookSqlAdapter.QueryBestCookList() : 데이터 베이스에 연결을 종료합니다.");
            }
            return REPLY_RESULT.FAIL;
        }

        public REPLY_RESULT UpdateBestCook(BestCookData _newData)
        {
            WorkSession.WriteStatus("BestCookSqlAdapter.UpdateBestCook() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("BestCookSqlAdapter.UpdateBestCook() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    int rows = 0;
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.ScrapBookBestCook))
                    {

                        cmd.Where(Mabinogi.SQL.Columns.ScrapBookBestCook.ClassId, _newData.classId);

                        cmd.Set(Mabinogi.SQL.Columns.ScrapBookBestCook.CharId, _newData.characterId);
                        cmd.Set(Mabinogi.SQL.Columns.ScrapBookBestCook.Name, _newData.characterName);
                        cmd.Set(Mabinogi.SQL.Columns.ScrapBookBestCook.Quality, _newData.quality);
                        cmd.Set(Mabinogi.SQL.Columns.ScrapBookBestCook.Comment, _newData.comment);
                        cmd.Set(Mabinogi.SQL.Columns.ScrapBookBestCook.UpdateTime, DateTime.Now);
                        rows = cmd.Execute();
                    }

                    if (rows == 0)
                    {
                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.ScrapBookBestCook))
                        {

                            cmd.Set(Mabinogi.SQL.Columns.ScrapBookBestCook.ClassId, _newData.classId);
                            cmd.Set(Mabinogi.SQL.Columns.ScrapBookBestCook.CharId, _newData.characterId);
                            cmd.Set(Mabinogi.SQL.Columns.ScrapBookBestCook.Name, _newData.characterName);
                            cmd.Set(Mabinogi.SQL.Columns.ScrapBookBestCook.Quality, _newData.quality);
                            cmd.Set(Mabinogi.SQL.Columns.ScrapBookBestCook.Comment, _newData.comment);
                            cmd.Execute();
                        }
                    }

                    WorkSession.WriteStatus("BestCookSqlAdapter.UpdateBestCook() : 명령을 실행합니다");
                    return REPLY_RESULT.SUCCESS;
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
                WorkSession.WriteStatus("BestCookSqlAdapter.UpdateBestCook() : 연결을 종료합니다");
            }
        }
    }
}
