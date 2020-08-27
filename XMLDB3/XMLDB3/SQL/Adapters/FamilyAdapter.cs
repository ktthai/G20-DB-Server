using Mabinogi.SQL;
using System;
using System.Collections.Generic;

namespace XMLDB3
{
    public class FamilyAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Family;

        public FamilyAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public FamilyListFamily Read(long _familyID)
        {
            WorkSession.WriteStatus("FamilySqlAdapter.Read() : 함수에 진입하였습니다.");

            try
            {
                WorkSession.WriteStatus("FamilySqlAdapter.Read() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Family))
                {
                    cmd.Where(Mabinogi.SQL.Columns.Family.Id, _familyID);

                    WorkSession.WriteStatus("FamilySqlAdapter.Read() : 데이터를 채웁니다.");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            FamilyListFamily familyListFamily = new FamilyListFamily();
                            familyListFamily.familyID = _familyID;
                            familyListFamily.familyName = reader.GetString(Mabinogi.SQL.Columns.Family.Name);
                            familyListFamily.headID = reader.GetInt64(Mabinogi.SQL.Columns.Family.HeadMemberId);
                            familyListFamily.state = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.Family.State);
                            familyListFamily.tradition = (uint)reader.GetInt32(Mabinogi.SQL.Columns.Family.Tradition);
                            familyListFamily.meta = reader.GetString(Mabinogi.SQL.Columns.Family.Meta);
                            familyListFamily.member = ReadFamilyMember(familyListFamily);
                            return familyListFamily;
                        }
                    }
                    return null;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _familyID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _familyID);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("FamilySqlAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");
            }
        }

        public FamilyList ReadList()
        {
            WorkSession.WriteStatus("FamilySqlAdapter.ReadList() : 함수에 진입하였습니다.");

            try
            {
                WorkSession.WriteStatus("FamilySqlAdapter.ReadList() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Family))
                {
                    // PROCEDURE: "dbo.FamilySelectAll"

                    WorkSession.WriteStatus("FamilySqlAdapter.ReadList() : 데이터를 채웁니다.");
                    return Build(cmd.ExecuteReader());
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
                WorkSession.WriteStatus("FamilySqlAdapter.ReadList() : 데이터 베이스에 연결을 종료합니다.");
            }
            return null;
        }

        private FamilyList Build(SimpleReader reader)
        {
            if (reader == null)
            {
                throw new Exception("패밀리 테이블을 얻어오지 못햇습니다.");
            }
            if (reader.HasRows)
            {
                FamilyList familyList = new FamilyList();
                familyList.Families = new List<FamilyListFamily>();
                FamilyListFamily familyListFamily;
                while (reader.Read())
                {

                    familyListFamily = new FamilyListFamily();
                    familyListFamily.familyID = reader.GetInt64(Mabinogi.SQL.Columns.Family.Id);
                    familyListFamily.familyName = reader.GetString(Mabinogi.SQL.Columns.Family.Name);
                    familyListFamily.headID = reader.GetInt64(Mabinogi.SQL.Columns.Family.HeadMemberId);
                    familyListFamily.state = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.Family.State);
                    familyListFamily.tradition = (uint)reader.GetInt32(Mabinogi.SQL.Columns.Family.Tradition);
                    familyListFamily.meta = reader.GetString(Mabinogi.SQL.Columns.Family.Meta);
                    familyListFamily.member = ReadFamilyMember(familyListFamily);

                    familyList.Families.Add(familyListFamily);
                }
                return familyList;
            }
            return new FamilyList();
        }

        public List<FamilyListFamilyMember> ReadFamilyMember(FamilyListFamily _family)
        {
            WorkSession.WriteStatus("FamilySqlAdapter.ReadFamilyMember() : 함수에 진입하였습니다.");

            try
            {
                WorkSession.WriteStatus("FamilySqlAdapter.ReadFamilyMember() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.FamilyMember))
                {
                    cmd.Where(Mabinogi.SQL.Columns.FamilyMember.FamilyId, _family.familyID);

                    WorkSession.WriteStatus("FamilySqlAdapter.ReadFamilyMember() : 데이터를 채웁니다.");
                    return BuildFamilyMember(cmd.ExecuteReader());
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
                WorkSession.WriteStatus("FamilySqlAdapter.ReadFamilyMember() : 데이터 베이스에 연결을 종료합니다.");
            }
            return null;
        }

        private List<FamilyListFamilyMember> BuildFamilyMember(SimpleReader reader)
        {
            if (reader == null)
            {
                throw new Exception("패밀리멤버 테이블을 얻어오지 못햇습니다.");
            }
            if (reader.HasRows)
            {
                List<FamilyListFamilyMember> list = new List<FamilyListFamilyMember>();
                FamilyListFamilyMember member;
                while (reader.Read())
                {
                    member = new FamilyListFamilyMember();
                    member.memberID = reader.GetInt64(Mabinogi.SQL.Columns.FamilyMember.CharId);
                    member.memberName = reader.GetString(Mabinogi.SQL.Columns.FamilyMember.Name);
                    member.memberClass = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.FamilyMember.Class);
                }
                return list;
            }
            return null;
        }

        public REPLY_RESULT AddFamily(FamilyListFamily _family, ref byte _errorCode)
        {
            WorkSession.WriteStatus("FamilySqlAdapter.Add() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    //sqlConnection.Open();
                    transaction = conn.BeginTransaction();

                    // PROCEDURE: dbo.FamilyAdd
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Family, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.Family.Id, _family.familyID);
                        cmd.Set(Mabinogi.SQL.Columns.Family.HeadMemberId, _family.headID);
                        cmd.Set(Mabinogi.SQL.Columns.Family.Name, _family.familyName);
                        cmd.Set(Mabinogi.SQL.Columns.Family.State, _family.state);
                        cmd.Set(Mabinogi.SQL.Columns.Family.Tradition, _family.tradition);
                        cmd.Set(Mabinogi.SQL.Columns.Family.Meta, _family.meta);

                        WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 명령을 실행합니다");
                        if (cmd.Execute() == 0)
                        {
                            Exception ex = new Exception("패밀리 추가에 실패하였습니다.", null);
                            ExceptionMonitor.ExceptionRaised(ex, _family.familyID);
                            WorkSession.WriteStatus(ex.Message, 0);
                            if (transaction != null)
                            {
                                WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 트랜잭션을 롤백합니다");
                                transaction.Rollback();
                            }
                            return REPLY_RESULT.FAIL;
                        }


                        foreach (FamilyListFamilyMember member in _family.member)
                        {
                            if (_AddMember(conn, transaction, _family.familyID, member, ref _errorCode) != REPLY_RESULT.SUCCESS)
                            {
                                Exception ex2 = new Exception("패밀리 멤버 추가에 실패하였습니다.", null);
                                ExceptionMonitor.ExceptionRaised(ex2, _family.familyID);
                                WorkSession.WriteStatus(ex2.Message, 0);
                                if (transaction != null)
                                {
                                    WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 트랜잭션을 롤백합니다");
                                    transaction.Rollback();
                                }
                                return REPLY_RESULT.FAIL;
                            }
                        }

                        WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 트랜잭션을 커밋합니다");
                        transaction.Commit();
                        return REPLY_RESULT.SUCCESS;
                    }
                }
                catch (SimpleSqlException ex3)
                {
                    ExceptionMonitor.ExceptionRaised(ex3, _family.familyID);
                    WorkSession.WriteStatus(ex3.Message, ex3.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex4)
                {
                    ExceptionMonitor.ExceptionRaised(ex4, _family.familyID);
                    WorkSession.WriteStatus(ex4.Message);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.AddFamily() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT RemoveFamily(long _familyID, ref byte _errorCode)
        {
            WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();
                    // PROCEDURE: dbo.FamilyRemove
                    WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 명령을 실행합니다");
                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.FamilyMember, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.FamilyMember.FamilyId, _familyID);
                        cmd.Execute();
                    }
                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.Family, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Family.Id, _familyID);
                        cmd.Execute();
                    }
                    WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;

                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 트랜잭션을 롤백합니다");
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
                        WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT UpdateFamily(FamilyListFamily _family, ref byte _errorCode)
        {
            WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();
                    // PROCEDURE: dbo.FamilyUpdate
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Family, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Family.Id, _family.familyID);

                        cmd.Set(Mabinogi.SQL.Columns.Family.Name, _family.familyName);
                        cmd.Set(Mabinogi.SQL.Columns.Family.HeadMemberId, _family.headID);
                        cmd.Set(Mabinogi.SQL.Columns.Family.State, _family.state);
                        cmd.Set(Mabinogi.SQL.Columns.Family.Tradition, _family.tradition);
                        cmd.Set(Mabinogi.SQL.Columns.Family.Meta, _family.meta);
                        WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 명령을 실행합니다");
                        cmd.Execute();
                    }

                    WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;

                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _family.familyID);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _family.familyID);
                    WorkSession.WriteStatus(ex2.Message);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.UpdateFamily() : 연결을 종료합니다");
                }
            }
        }

        private REPLY_RESULT _AddMember(SimpleConnection conn, SimpleTransaction transaction, long _familyID, FamilyListFamilyMember _member, ref byte _errorCode)
        {
            using (var chkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Family))
            {
                chkCmd.Where(Mabinogi.SQL.Columns.Family.Id, _familyID);

                if (chkCmd.ExecuteReader().HasRows)
                {

                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.FamilyMember, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.FamilyMember.CharId, _member.memberID);
                        cmd.Set(Mabinogi.SQL.Columns.FamilyMember.Class, _member.memberClass);
                        cmd.Set(Mabinogi.SQL.Columns.FamilyMember.Name, _member.memberName);
                        cmd.Set(Mabinogi.SQL.Columns.FamilyMember.FamilyId, _familyID);

                        WorkSession.WriteStatus("FamilySqlAdapter._AddMember() : 명령을 실행합니다");
                        if (cmd.Execute() == 0)
                        {
                            return REPLY_RESULT.FAIL;
                        }
                        return REPLY_RESULT.SUCCESS;
                    }
                }
            }
            return REPLY_RESULT.ERROR;
        }

        public REPLY_RESULT AddMember(long _familyID, FamilyListFamilyMember _member, ref byte _errorCode)
        {
            WorkSession.WriteStatus("FamilySqlAdapter.AddMember() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("FamilySqlAdapter.AddMember() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();
                    _AddMember(conn, transaction, _familyID, _member, ref _errorCode);

                    WorkSession.WriteStatus("FamilySqlAdapter.AddMember() : 트랜잭션을 커밋합니다");
                    transaction.Commit();

                    return REPLY_RESULT.SUCCESS;

                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _member.memberID);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FamilySqlAdapter.AddMember() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _member.memberID);
                    WorkSession.WriteStatus(ex2.Message);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FamilySqlAdapter.AddMember() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.AddMember() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT RemoveMember(long _familyID, long _memberID, ref byte _errorCode)
        {
            WorkSession.WriteStatus("FamilySqlAdapter.RemoveMember() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("FamilySqlAdapter.RemoveFamily() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();
                    // PROCEDURE dbo.FamilyMemberRemove
                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.FamilyMember, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.FamilyMember.FamilyId, _familyID);
                        cmd.Where(Mabinogi.SQL.Columns.FamilyMember.CharId, _memberID);

                        WorkSession.WriteStatus("FamilySqlAdapter.RemoveMember() : 명령을 실행합니다");
                        cmd.Execute();
                    }

                    WorkSession.WriteStatus("FamilySqlAdapter.RemoveMember() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FamilySqlAdapter.RemoveMember() : 트랜잭션을 롤백합니다");
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
                        WorkSession.WriteStatus("FamilySqlAdapter.RemoveMember() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.RemoveMember() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT UpdateMember(long _familyID, FamilyListFamilyMember _member, ref byte _errorCode)
        {
            WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();
                    // PROCEDURE: dbo.FamilyMemberUpdate
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.FamilyMember, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.FamilyMember.CharId, _member.memberID);
                        cmd.Where(Mabinogi.SQL.Columns.FamilyMember.Class, _member.memberClass);

                        cmd.Set(Mabinogi.SQL.Columns.FamilyMember.FamilyId, _familyID);
                        cmd.Set(Mabinogi.SQL.Columns.FamilyMember.Name, _member.memberName);

                        WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 명령을 실행합니다");
                        cmd.Execute();
                    }

                    WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _member.memberID);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _member.memberID);
                    WorkSession.WriteStatus(ex2.Message);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("FamilySqlAdapter.UpdateMember() : 연결을 종료합니다");
                }
            }
        }
    }
}

