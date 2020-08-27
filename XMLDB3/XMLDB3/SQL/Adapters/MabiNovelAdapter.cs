using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class MabiNovelAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabi_Novel.MabiNovel;

        public MabiNovelAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public PageList Read(long _sn)
        {
            WorkSession.WriteStatus("MabiNovelSqlAdapter.Read() : 함수에 진입하였습니다.");
            try
            {
                // PROCEDURE: GetMabiNovel

                WorkSession.WriteStatus("MabiNovelSqlAdapter.Read() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabi_Novel.MabiNovel))
                {
                    cmd.Where(Mabinogi.SQL.Columns.MabiNovel.BoardSn, _sn);

                    WorkSession.WriteStatus("MabiNovelSqlAdapter.Read() : 데이터를 채웁니다.");
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
                WorkSession.WriteStatus("MabiNovelSqlAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");
            }
        }

        private PageList Build(SimpleReader reader)
        {
            if (reader == null)
            {
                throw new Exception("Not Found MabiNovel Data.");
            }
            if (reader.HasRows)
            {
                PageList pageList = new PageList();
                pageList.novel = new List<PageData>();
                PageData page;
                while (reader.Read())
                {
                    page = new PageData();
                    page.sn = reader.GetInt64(Mabinogi.SQL.Columns.MabiNovel.BoardSn);
                    page.page = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.MabiNovel.Page);
                    page.bgId = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.MabiNovel.BackgroundId);
                    page.bgmId = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.MabiNovel.BgmId);
                    page.portraitId = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.MabiNovel.PortraitId);
                    page.portraitPos = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.MabiNovel.PortraitPos);
                    page.emotionId = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.MabiNovel.EmotionId);
                    page.soundEffectId = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.MabiNovel.SoundEffectId);
                    page.effectId = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.MabiNovel.EffectId);
                    page.ambassador = reader.GetString(Mabinogi.SQL.Columns.MabiNovel.Ambassador);
                    pageList.novel.Add(page);
                }
                return pageList;
            }
            return new PageList();
        }

        public REPLY_RESULT Insert(string _serverName, Posts _posts, PageList _pageList)
        {
            WorkSession.WriteStatus("MabiNovelSqlAdapter.Insert() : 함수에 진입하였습니다");

            SimpleTransaction novelTransaction = null;
            SimpleTransaction boardTransaction = null;
            WorkSession.WriteStatus("MabiNovelSqlAdapter.Insert() : 데이터베이스와 연결합니다");
            using (var novelConn = Connection)
            using (var boardConn = QueryManager.MabiNovelBoard.Connection)
            {
                try
                {
                    boardTransaction = boardConn.BeginTransaction();
                    novelTransaction = novelConn.BeginTransaction();

                    // PROCEDURE: InsertMabiNovelPost 
                    using (var cmd = boardConn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabi_Novel.MabiNovelBoard, boardTransaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.Server, _serverName);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.CharId, _posts.authorId);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.Title, _posts.title);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.TransCount, _posts.transcriptionCount);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.EndDate, _posts.endDate);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.BlockCount, _posts.blockCount);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.Flag, _posts.option);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.ReadCount, _posts.readingCount);

                        if (cmd.Execute() < 1)
                        {
                            throw new Exception("Posts Insert Fail...Novel SN Zero");
                        }
                    }

                    long serialNum = 0;
                    using (var cmd = boardConn.GetSelectCommand("SELECT last_insert_rowid() Identity"))
                    {
                        using (var reader = cmd.ExecuteReader())
                            serialNum = reader.GetInt64("Identity");
                    }


                    WorkSession.WriteStatus("MabiNovelSqlAdapter.Insert() : 실질적인 노벨을 DB 에 반영한다.");
                    InsertNovelSql(serialNum, _pageList, novelConn, novelTransaction);

                    WorkSession.WriteStatus("MabiNovelSqlAdapter.Insert() : 성공하였으니 반영한다.");
                    _posts.sn = serialNum;
                    novelTransaction.Commit();
                    boardTransaction.Commit();
                    return REPLY_RESULT.SUCCESS;

                }
                catch (SimpleSqlException ex)
                {
                    WorkSession.WriteStatus("MabiNovelSqlAdapter.Insert() : 트랜잭션을 롤백합니다");
                    boardTransaction?.Rollback();
                    novelTransaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex, _serverName, _posts.title, _posts.author);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return REPLY_RESULT.FAIL_EX;
                }
                catch (Exception ex2)
                {
                    WorkSession.WriteStatus("MabiNovelSqlAdapter.Insert() : 트랜잭션을 롤백합니다");
                    boardTransaction?.Rollback();
                    novelTransaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex2, _serverName, _posts.title, _posts.author);
                    WorkSession.WriteStatus(ex2.Message);
                    return REPLY_RESULT.FAIL_EX;
                }
                finally
                {
                    WorkSession.WriteStatus("MabiNovelSqlAdapter.Insert() : 연결을 종료합니다");
                }
            }
        }

        private void InsertNovelSql(long _novelSN, PageList _pageList, SimpleConnection conn, SimpleTransaction transaction)
        {

            foreach (PageData pageData in _pageList.novel)
            {
                if (pageData != null)
                {
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabi_Novel.MabiNovel, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovel.BoardSn, _novelSN);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovel.Page, pageData.page);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovel.BackgroundId, pageData.bgId);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovel.BgmId, pageData.bgmId);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovel.PortraitId, pageData.portraitId);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovel.PortraitPos, pageData.portraitPos);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovel.EmotionId, pageData.emotionId);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovel.SoundEffectId, pageData.soundEffectId);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovel.EffectId, pageData.effectId);
                        cmd.Set(Mabinogi.SQL.Columns.MabiNovel.Ambassador, pageData.ambassador);
                    }
                }
            }
        }
    }
}
