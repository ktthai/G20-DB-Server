using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class MabiNovelBoardAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabi_Novel.MabiNovelBoard;

        private const string MabinogiDB = "mabinogi";

        public MabiNovelBoardAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public PostsList ReadPostsList(string _serverName)
        {
            WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.ReadPostsList() : 함수에 진입하였습니다.");
            try
            {
                // Procedure: SelectMabiNovelBoardList

                WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.ReadPostsList() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetSelectCommand(string.Format("SELECT m.{0}, m.{1}, c.{2}, m.{3}, m.{4}, m.{5}, m.{6}, m.{7}, m.{8} " +
                    "FROM {9} m, {10} c " +
                    "WHERE m.{11} = c.{12} AND m.{11} = @{11} AND m.{13} = c.{14} AND m.{15} = @{15} AND m.{5} >= @{5}",
                    /* 0 */Mabinogi.SQL.Columns.MabiNovelBoard.BoardSn, /* 1 */Mabinogi.SQL.Columns.MabiNovelBoard.CharId, /* 2 */Mabinogi.SQL.Columns.AccountCharacterRef.CharacterId, /* 3 */Mabinogi.SQL.Columns.MabiNovelBoard.Title,
                    /* 4 */Mabinogi.SQL.Columns.MabiNovelBoard.TransCount, /* 5 */Mabinogi.SQL.Columns.MabiNovelBoard.EndDate, /* 6 */Mabinogi.SQL.Columns.MabiNovelBoard.BlockCount, /* 7 */Mabinogi.SQL.Columns.MabiNovelBoard.Flag,
                    /* 8 */Mabinogi.SQL.Columns.MabiNovelBoard.ReadCount,/* 9 */Mabinogi.SQL.Tables.Mabi_Novel.MabiNovelBoard,  /* 10 */MabinogiDB + "." + Mabinogi.SQL.Tables.Mabinogi.AccountCharacterRef, /* 11 */Mabinogi.SQL.Columns.MabiNovelBoard.Server,
                    /* 12 */Mabinogi.SQL.Columns.AccountCharacterRef.Server, /* 13 */Mabinogi.SQL.Columns.MabiNovelBoard.CharId, /* 14 */Mabinogi.SQL.Columns.AccountCharacterRef.CharacterId, /* 15 */Mabinogi.SQL.Columns.MabiNovelBoard.Flag)))
                {
                    cmd.AddParameter(Mabinogi.SQL.Columns.MabiNovelBoard.Server, _serverName);
                    cmd.AddParameter(Mabinogi.SQL.Columns.MabiNovelBoard.Flag, 0);
                    cmd.AddParameter(Mabinogi.SQL.Columns.MabiNovelBoard.EndDate, DateTime.Now);

                    WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.ReadPostsList() : 데이터를 채웁니다.");
                    return BuildList(cmd.ExecuteReader());
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
                WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.ReadPostsList() : 데이터 베이스에 연결을 종료합니다.");
            }
        }

        public PostsList ReadPostsRoyaltiesList(string _serverName)
        {
            WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.ReadPostsRoyaltiesList() : 함수에 진입하였습니다.");
            try
            {
                // PROCEDURE: SelectMabiNovelRoyaltiesList
                WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.ReadPostsRoyaltiesList() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetSelectCommand(string.Format("SELECT m.{0}, m.{1}, c.{2}, m.{3}, m.{4}, m.{5}, m.{6}, m.{7}, m.{8} " +
                    "FROM {9} m, {10} c " +
                    "WHERE m.{11} = c.{12} AND m.{11} = @{11} AND m.{13} = c.{14} AND m.{15} = @{15} AND m.{5} < @{5}",
                    /* 0 */Mabinogi.SQL.Columns.MabiNovelBoard.BoardSn, /* 1 */Mabinogi.SQL.Columns.MabiNovelBoard.CharId, /* 2 */Mabinogi.SQL.Columns.AccountCharacterRef.CharacterId, /* 3 */Mabinogi.SQL.Columns.MabiNovelBoard.Title,
                    /* 4 */Mabinogi.SQL.Columns.MabiNovelBoard.TransCount, /* 5 */Mabinogi.SQL.Columns.MabiNovelBoard.EndDate, /* 6 */Mabinogi.SQL.Columns.MabiNovelBoard.BlockCount, /* 7 */Mabinogi.SQL.Columns.MabiNovelBoard.Flag,
                    /* 8 */Mabinogi.SQL.Columns.MabiNovelBoard.ReadCount,/* 9 */Mabinogi.SQL.Tables.Mabi_Novel.MabiNovelBoard,  /* 10 */MabinogiDB + "." + Mabinogi.SQL.Tables.Mabinogi.AccountCharacterRef, /* 11 */Mabinogi.SQL.Columns.MabiNovelBoard.Server,
                    /* 12 */Mabinogi.SQL.Columns.AccountCharacterRef.Server, /* 13 */Mabinogi.SQL.Columns.MabiNovelBoard.CharId, /* 14 */Mabinogi.SQL.Columns.AccountCharacterRef.CharacterId, /* 15 */Mabinogi.SQL.Columns.MabiNovelBoard.Flag)))
                {
                    cmd.AddParameter(Mabinogi.SQL.Columns.MabiNovelBoard.Server, _serverName);
                    cmd.AddParameter(Mabinogi.SQL.Columns.MabiNovelBoard.Flag, 0);
                    cmd.AddParameter(Mabinogi.SQL.Columns.MabiNovelBoard.EndDate, DateTime.Now);

                    WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.ReadPostsRoyaltiesList() : 데이터를 채웁니다.");
                    return BuildList(cmd.ExecuteReader());
                };
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
                WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.ReadPostsRoyaltiesList() : 데이터 베이스에 연결을 종료합니다.");
            }
        }

        private PostsList BuildList(SimpleReader reader)
        {
            if (reader == null)
            {
                throw new Exception("Not Found MabiNovel Data.");
            }
            if (reader.HasRows)
            {
                PostsList postsList = new PostsList();
                postsList.list = new List<Posts>();
                Posts post;
                while (reader.Read())
                {
                    post = BuildPost(reader);

                    postsList.list.Add(post);
                }
                return postsList;
            }
            return new PostsList();
        }

        private Posts BuildPost(SimpleReader reader)
        {
            Posts post = new Posts();

            post.sn = reader.GetInt64(Mabinogi.SQL.Columns.MabiNovelBoard.BoardSn);
            post.authorId = reader.GetInt64(Mabinogi.SQL.Columns.MabiNovelBoard.CharId);
            post.title = reader.GetString(Mabinogi.SQL.Columns.MabiNovelBoard.Title);
            post.transcriptionCount = (uint)reader.GetInt32(Mabinogi.SQL.Columns.MabiNovelBoard.TransCount);
            post.endDate = reader.GetDateTime(Mabinogi.SQL.Columns.MabiNovelBoard.EndDate);
            post.blockCount = (uint)reader.GetInt32(Mabinogi.SQL.Columns.MabiNovelBoard.BlockCount);
            post.option = (uint)reader.GetInt32(Mabinogi.SQL.Columns.MabiNovelBoard.Flag);
            post.readingCount = (uint)reader.GetInt32(Mabinogi.SQL.Columns.MabiNovelBoard.ReadCount);
            post.author = reader.GetString(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterName);

            return post;
        }

        public REPLY_RESULT FindTitleByAuthor(string _serverName, long _authorId, string _author, string _title)
        {
            WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.FindTitleByAuthor() : 함수에 진입하였습니다.");
            try
            {
                // PROCEDURE: FindMabiNovelTitle
                WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.FindTitleByAuthor() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabi_Novel.MabiNovelBoard))
                {
                    cmd.Where(Mabinogi.SQL.Columns.MabiNovelBoard.Title, _title);
                    cmd.Where(Mabinogi.SQL.Columns.MabiNovelBoard.CharId, _authorId);
                    cmd.Where(Mabinogi.SQL.Columns.MabiNovelBoard.Server, _serverName);

                    WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.FindTitleByAuthor() : 데이터를 찾습니다.");
                    if (cmd.ExecuteReader().HasRows)
                        return REPLY_RESULT.SUCCESS;
                    else
                        return REPLY_RESULT.FAIL;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return REPLY_RESULT.FAIL_EX;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.FAIL_EX;
            }
            finally
            {
                WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.ReadPostsList() : 데이터 베이스에 연결을 종료합니다.");
            }
        }

        public REPLY_RESULT UpdataPosts(PostsList _postsList)
        {
            WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.UpdataPosts() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.UpdataPosts() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();
                    WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.UpdataPosts() : DB 에 업데이트 요청을 한다.");
                    foreach (Posts posts in _postsList.list)
                    {
                        using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabi_Novel.MabiNovelBoard, transaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.MabiNovelBoard.BoardSn, posts.sn);

                            cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.CharId, posts.authorId);
                            cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.Title, posts.title);
                            cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.TransCount, posts.transcriptionCount);
                            cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.EndDate, posts.endDate);
                            cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.BlockCount, posts.blockCount);
                            cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.Flag, posts.option);
                            cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.ReadCount, posts.readingCount);
                            if (posts.option == 4)
                                cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.BlockDate, null);
                            else
                                cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.BlockDate, DateTime.Now);
                            WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.UpdataPosts() : 성공하였으니 반영한다.");
                            cmd.Execute();
                        }
                    }

                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;

                }
                catch (SimpleSqlException ex)
                {
                    WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.UpdataPosts() : 트랜잭션을 롤백합니다");
                    transaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return REPLY_RESULT.FAIL_EX;
                }
                catch (Exception ex2)
                {
                    WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.UpdataPosts() : 트랜잭션을 롤백합니다");
                    transaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex2);
                    WorkSession.WriteStatus(ex2.Message);
                    return REPLY_RESULT.FAIL_EX;
                }
                finally
                {
                    WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.UpdataPosts() : 연결을 종료합니다");
                }
            }
        }

        public Posts GetChangePosts(long _sn)
        {
            WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.GetChangePosts() : 함수에 진입하였습니다.");

            try
            {
                // PROCEDURE: SelectMabiNovelBoard
                WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.GetChangePosts() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabi_Novel.MabiNovelBoard))
                {
                    cmd.Where(Mabinogi.SQL.Columns.MabiNovelBoard.BoardSn, _sn);


                    WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.GetChangePosts() : 데이터를 채웁니다.");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return BuildPost(reader);
                    }
                }

                throw new Exception("GetChangePosts Error : Posts Not Found");
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
                WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.GetChangePosts() : 데이터 베이스에 연결을 종료합니다.");
            }
        }

        public REPLY_RESULT GetUpdateTime(long _sn, out long _updateTime)
        {
            _updateTime = 0L;
            WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.GetUpdateTime() : 함수에 진입하였습니다.");

            try
            {
                WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.GetUpdateTime() : 데이터 베이스에 연결합니다.");
                // PROCEDURE: MabiNovelBoardUpdateCheck
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabi_Novel.MabiNovelBoard))
                {
                    cmd.Where(Mabinogi.SQL.Columns.MabiNovelBoard.BoardSn, _sn);
                    cmd.Set(Mabinogi.SQL.Columns.MabiNovelBoard.UpdateTime, 0);

                    WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.GetUpdateTime() : 데이터를 찾습니다.");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            _updateTime = reader.GetDateTime(Mabinogi.SQL.Columns.MabiNovelBoard.UpdateTime).Ticks;
                        }
                    }
                }
                return REPLY_RESULT.SUCCESS;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return REPLY_RESULT.FAIL_EX;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.FAIL_EX;
            }
            finally
            {
                WorkSession.WriteStatus("MabiNovelBoardSqlAdapter.GetUpdateTime() : 데이터 베이스에 연결을 종료합니다.");
            }
        }
    }
}
