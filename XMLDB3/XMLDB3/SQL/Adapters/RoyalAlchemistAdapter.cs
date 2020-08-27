using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class RoyalAlchemistAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.RoyalAlchemist;

        public RoyalAlchemistAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public RoyalAlchemist Read(long _charID)
        {
            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Read() : 함수에 진입하였습니다.");
            try
            {
                // PROCEDURE: RoyalAlchemistSelect

                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Read() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.RoyalAlchemist))
                {
                    cmd.Where(Mabinogi.SQL.Columns.RoyalAlchemist.CharId, _charID);

                    WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Read() : 데이터를 채웁니다.");
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ReadRoyal(reader);
                        }
                    }
                }

                return null;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _charID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _charID);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");
            }
        }

        private RoyalAlchemist ReadRoyal(SimpleReader reader)
        {
            RoyalAlchemist royalAlchemist = new RoyalAlchemist();
            royalAlchemist.charID = reader.GetInt64(Mabinogi.SQL.Columns.RoyalAlchemist.CharId);
            royalAlchemist.charName = reader.GetString(Mabinogi.SQL.Columns.RoyalAlchemist.CharName);
            royalAlchemist.registrationFlag = reader.GetByte(Mabinogi.SQL.Columns.RoyalAlchemist.RegistrationFlag);
            royalAlchemist.rank = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.RoyalAlchemist.Rank);
            royalAlchemist.meta = reader.GetString(Mabinogi.SQL.Columns.RoyalAlchemist.Meta);
            return royalAlchemist;
        }

        public RoyalAlchemistList ReadList()
        {
            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.ReadList() : 함수에 진입하였습니다.");

            try
            {

                // PROCEDURE: RoyalAlchemistSelectAll
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.ReadList() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.RoyalAlchemist))
                using (var reader = cmd.ExecuteReader())
                {
                    WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.ReadList() : 데이터를 채웁니다.");


                    if (reader.HasRows)
                    {
                        RoyalAlchemistList result = new RoyalAlchemistList();
                        result.alchemists = new List<RoyalAlchemist>();
                        while (reader.Read())
                        {
                            ReadRoyal(reader);
                        }
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
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.ReadList() : 데이터 베이스에 연결을 종료합니다.");
            }
            return new RoyalAlchemistList();
        }

        public REPLY_RESULT Remove(long[] _removeIDs, ref byte _errorCode)
        {
            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Delete() : 함수에 진입하였습니다");

            try
            {

                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Remove() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {

                    SimpleTransaction sqlTransaction = null;
                    try
                    {
                        sqlTransaction = conn.BeginTransaction();
                        foreach (long num in _removeIDs)
                        {
                            // PROCEDURE: RoyalAlchemistRemove

                            using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.RoyalAlchemist, sqlTransaction))
                            {
                                cmd.Where(Mabinogi.SQL.Columns.RoyalAlchemist.CharId, num);
                                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Remove() : 명령을 실행합니다");
                                cmd.Execute();
                            }

                        }
                        WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Remove() : 트랜잭션을 커밋합니다");
                        sqlTransaction.Commit();
                        return REPLY_RESULT.SUCCESS;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        if (sqlTransaction != null)
                        {
                            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Remove() : 트랜잭션을 롤백합니다");
                            sqlTransaction.Rollback();
                        }
                        return REPLY_RESULT.FAIL;
                    }
                    catch (Exception ex2)
                    {
                        ExceptionMonitor.ExceptionRaised(ex2);
                        WorkSession.WriteStatus(ex2.Message);
                        if (sqlTransaction != null)
                        {
                            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Remove() : 트랜잭션을 롤백합니다");
                            sqlTransaction.Rollback();
                        }
                        return REPLY_RESULT.FAIL;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Remove() : 연결을 종료합니다");
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
        }

        public REPLY_RESULT Update(RoyalAlchemist _royalAlchemist, ref byte _errorCode)
        {
            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 함수에 진입하였습니다");

            try
            {
                SimpleTransaction sqlTransaction = null;
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 데이터베이스와 연결합니다");
                // PROCEDURE: RoyalAlchemistUpdate
                using (var conn = Connection)
                {
                    try
                    {


                        sqlTransaction = conn.BeginTransaction();
                        using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.RoyalAlchemist, sqlTransaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.RoyalAlchemist.CharId, _royalAlchemist.charID);

                            cmd.Set(Mabinogi.SQL.Columns.RoyalAlchemist.CharName, _royalAlchemist.charName);
                            cmd.Set(Mabinogi.SQL.Columns.RoyalAlchemist.RegistrationFlag, _royalAlchemist.registrationFlag);
                            cmd.Set(Mabinogi.SQL.Columns.RoyalAlchemist.Rank, _royalAlchemist.rank);
                            cmd.Set(Mabinogi.SQL.Columns.RoyalAlchemist.Meta, _royalAlchemist.meta);

                            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 명령을 실행합니다");
                            cmd.Execute();
                        }

                        WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 트랜잭션을 커밋합니다");
                        sqlTransaction.Commit();
                        return REPLY_RESULT.SUCCESS;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, _royalAlchemist.charID);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        if (sqlTransaction != null)
                        {
                            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 트랜잭션을 롤백합니다");
                            sqlTransaction.Rollback();
                        }
                        return REPLY_RESULT.FAIL;
                    }
                    catch (Exception ex2)
                    {
                        ExceptionMonitor.ExceptionRaised(ex2, _royalAlchemist.charID);
                        WorkSession.WriteStatus(ex2.Message);
                        if (sqlTransaction != null)
                        {
                            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 트랜잭션을 롤백합니다");
                            sqlTransaction.Rollback();
                        }
                        return REPLY_RESULT.FAIL;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Update() : 연결을 종료합니다");
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _royalAlchemist.charID);
                WorkSession.WriteStatus(ex.Message, ex.Number);

                return REPLY_RESULT.FAIL;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _royalAlchemist.charID);
                WorkSession.WriteStatus(ex2.Message);

                return REPLY_RESULT.FAIL;
            }
        }

        public REPLY_RESULT Add(RoyalAlchemist _royalAlchemist, ref byte _errorCode)
        {
            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    SimpleTransaction sqlTransaction = null;
                    try
                    {

                        sqlTransaction = conn.BeginTransaction();
                        // PROCEDURE: RoyalAlchemistAdd
                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.RoyalAlchemist, sqlTransaction))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.RoyalAlchemist.CharId, _royalAlchemist.charID);
                            cmd.Set(Mabinogi.SQL.Columns.RoyalAlchemist.CharName, _royalAlchemist.charName);
                            cmd.Set(Mabinogi.SQL.Columns.RoyalAlchemist.RegistrationFlag, _royalAlchemist.registrationFlag);
                            cmd.Set(Mabinogi.SQL.Columns.RoyalAlchemist.Rank, _royalAlchemist.rank);
                            cmd.Set(Mabinogi.SQL.Columns.RoyalAlchemist.Meta, _royalAlchemist.meta);

                            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 명령을 실행합니다");
                            cmd.Execute();
                        }
                        WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 트랜잭션을 커밋합니다");
                        sqlTransaction.Commit();
                        return REPLY_RESULT.SUCCESS;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, _royalAlchemist.charID);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        if (sqlTransaction != null)
                        {
                            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 트랜잭션을 롤백합니다");
                            sqlTransaction.Rollback();
                        }
                        return REPLY_RESULT.FAIL;
                    }
                    catch (Exception ex2)
                    {
                        ExceptionMonitor.ExceptionRaised(ex2, _royalAlchemist.charID);
                        WorkSession.WriteStatus(ex2.Message);
                        if (sqlTransaction != null)
                        {
                            WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 트랜잭션을 롤백합니다");
                            sqlTransaction.Rollback();
                        }
                        return REPLY_RESULT.FAIL;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("RoyalAlchemistSqlAdapter.Add() : 연결을 종료합니다");
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _royalAlchemist.charID);
                WorkSession.WriteStatus(ex.Message, ex.Number);

                return REPLY_RESULT.FAIL;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _royalAlchemist.charID);
                WorkSession.WriteStatus(ex2.Message);

                return REPLY_RESULT.FAIL;
            }
        }
    }
}
