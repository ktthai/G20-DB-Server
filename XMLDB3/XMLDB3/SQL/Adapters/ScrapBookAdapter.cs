using System;
using System.Collections;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class ScrapBookAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.ScrapBook;
        private BestCookAdapter bestCookAdapter;

        public ScrapBookAdapter()
        {
            bestCookAdapter = new BestCookAdapter();
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public REPLY_RESULT Scrap(long _charId, byte _scrapType, int _classId, int _oldScrapData, int _scrapData, int _regionId)
        {
            WorkSession.WriteStatus("ScrapBookSqlAdapter.Scrap() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("ScrapBookSqlAdapter.Scrap() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {

                    SimpleTransaction sqlTransaction = null;
                    try
                    {
                        // PROCEDURE: UpdateScrapBook
                        sqlTransaction = conn.BeginTransaction();

                        using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.ScrapBook, sqlTransaction))
                        {
                            upCmd.Where(Mabinogi.SQL.Columns.ScrapBook.ScrapData, _oldScrapData);
                            upCmd.Where(Mabinogi.SQL.Columns.ScrapBook.CharId, _charId);
                            upCmd.Where(Mabinogi.SQL.Columns.ScrapBook.ScrapType, _scrapType);
                            upCmd.Where(Mabinogi.SQL.Columns.ScrapBook.ClassId, _classId);

                            upCmd.Set(Mabinogi.SQL.Columns.ScrapBook.UpdateTime, DateTime.Now);
                            upCmd.Set(Mabinogi.SQL.Columns.ScrapBook.ScrapData, _scrapData);
                            upCmd.Set(Mabinogi.SQL.Columns.ScrapBook.RegionId, _regionId);

                            WorkSession.WriteStatus("ScrapBookSqlAdapter.Scrap() : 명령을 실행합니다");
                            if (upCmd.Execute() < 1)
                            {
                                using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.ScrapBook, sqlTransaction))
                                {
                                    insCmd.Set(Mabinogi.SQL.Columns.ScrapBook.CharId, _charId);
                                    insCmd.Set(Mabinogi.SQL.Columns.ScrapBook.ScrapType, _scrapType);
                                    insCmd.Set(Mabinogi.SQL.Columns.ScrapBook.ClassId, _classId);
                                    insCmd.Set(Mabinogi.SQL.Columns.ScrapBook.ScrapData, _scrapData);
                                    insCmd.Set(Mabinogi.SQL.Columns.ScrapBook.RegionId, _regionId);

                                    insCmd.Execute();
                                }
                            }
                            sqlTransaction.Commit();

                            return REPLY_RESULT.SUCCESS;
                        }
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        sqlTransaction?.Rollback();
                        return REPLY_RESULT.FAIL;
                    }
                    catch (Exception ex2)
                    {
                        ExceptionMonitor.ExceptionRaised(ex2);
                        WorkSession.WriteStatus(ex2.Message);
                        sqlTransaction?.Rollback();
                        return REPLY_RESULT.FAIL;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("ScrapBookSqlAdapter.Scrap() : 연결을 종료합니다");
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
                WorkSession.WriteStatus(ex2.Message); ;
                return REPLY_RESULT.FAIL;
            }
        }

        public REPLY_RESULT QueryScrappedList(long _charId, out Dictionary<long, ScrapBookData> _scrappedList)
        {
            _scrappedList = null;
            WorkSession.WriteStatus("ScrapBookSqlAdapter.QueryScrappedList() : 함수에 진입하였습니다.");
            
            try
            {
                // PROCEDURE: QueryScrapBookList
                WorkSession.WriteStatus("ScrapBookSqlAdapter.QueryScrappedList() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.ScrapBook))
                {
                    cmd.Where(Mabinogi.SQL.Columns.ScrapBook.CharId, _charId);

                    WorkSession.WriteStatus("ScrapBookSqlAdapter.QueryScrappedList() : 데이터를 채웁니다.");
                    using (var reader = cmd.ExecuteReader())
                    {


                        if (reader == null)
                        {
                            throw new Exception("도감 테이블을 얻어오지 못했습니다.");
                        }
                        _scrappedList = new Dictionary<long, ScrapBookData>();
                        if (reader.HasRows)
                        {
                            ScrapBookData scrapBookData;
                            while (reader.Read())
                            {
                                scrapBookData = new ScrapBookData();
                                scrapBookData.scrapType = reader.GetByte(Mabinogi.SQL.Columns.ScrapBook.ScrapType);
                                scrapBookData.classId = reader.GetInt32(Mabinogi.SQL.Columns.ScrapBook.ClassId);
                                scrapBookData.scrapData = reader.GetInt32(Mabinogi.SQL.Columns.ScrapBook.ScrapData);
                                scrapBookData.regionId = reader.GetInt32(Mabinogi.SQL.Columns.ScrapBook.RegionId);
                                scrapBookData.updatetime = reader.GetDateTime(Mabinogi.SQL.Columns.ScrapBook.UpdateTime);

                                if (_scrappedList.ContainsKey(scrapBookData.Key))
                                {
                                    _scrappedList.Remove(scrapBookData.Key);
                                }
                                _scrappedList.Add(scrapBookData.Key, scrapBookData);
                            }
                        }

                        return REPLY_RESULT.SUCCESS;
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
                WorkSession.WriteStatus("ScrapBookSqlAdapter.QueryScrappedList() : 데이터 베이스에 연결을 종료합니다.");
            }
            return REPLY_RESULT.FAIL;
        }

        public REPLY_RESULT QueryBestCookList(out Hashtable _bestCookList)
        {
            return bestCookAdapter.QueryBestCookList(out _bestCookList);
        }

        public REPLY_RESULT UpdateBestCook(BestCookData _newData)
        {
            return bestCookAdapter.UpdateBestCook(_newData);
        }
    }
}
