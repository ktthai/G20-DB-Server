using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class InviteEventAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.InviteEvent;
        public InviteEventAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public REPLY_RESULT UpdateInviteEvent(InviteEvent _inviteevent)
        {

            WorkSession.WriteStatus("InviteEventAdapter.UpdateInviteEvent() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("InviteEventAdapter.UpdateInviteEvent() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    // PROCEDURE: UpdateInviteEvent
                    long charId = 0;

                    transaction = conn.BeginTransaction();

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCharacterRef, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.AccountCharacterRef.Server, _inviteevent.servername);
                        cmd.Where(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterName, _inviteevent.invitecharactername);
                        cmd.Set(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterId, 0);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                charId = reader.GetInt64(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterId);
                            }
                        }
                    }


                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.InviteEvent, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.InviteEvent.Id, _inviteevent.mabiId);
                        cmd.Set(Mabinogi.SQL.Columns.InviteEvent.Server, _inviteevent.servername);
                        cmd.Set(Mabinogi.SQL.Columns.InviteEvent.InviteCharacterId, charId);
                        cmd.Set(Mabinogi.SQL.Columns.InviteEvent.InviteCharacterName, _inviteevent.invitecharactername);
                        cmd.Set(Mabinogi.SQL.Columns.InviteEvent.SendDate, _inviteevent.senddate);

                        WorkSession.WriteStatus("InviteEventAdapter.UpdateInviteEvent() : 명령을 실행합니다");
                        if (cmd.Execute() < 1)
                        {
                            Exception ex = new Exception("UpdateInviteEvent 업데이트를 실패하였습니다.", null);
                            WorkSession.WriteStatus(ex.Message, 0);
                            WorkSession.WriteStatus("InviteEventAdapter.UpdateInviteEvent() : 트랜잭션을 롤백합니다");
                            transaction.Rollback();
                            return REPLY_RESULT.FAIL;
                        }
                    }

                    WorkSession.WriteStatus("InviteEventAdapter.UpdateInviteEvent() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
                catch (SimpleSqlException ex2)
                {
                    WorkSession.WriteStatus(ex2.Message, ex2.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("InviteEventAdapter.UpdateInviteEvent() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex3)
                {
                    WorkSession.WriteStatus(ex3.Message);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("InviteEventAdapter.UpdateInviteEvent() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("InviteEventAdapter.UpdateInviteEvent() : 연결을 종료합니다");
                }
            }
        }

        public InviteEventList SelectAllInviteEventList()
        {
            WorkSession.WriteStatus("InviteEventAdapter.SelectAllInviteEventList() : 함수에 진입하였습니다");
            try
            {
                // PROCEDURE: SelectInviteEvent
                WorkSession.WriteStatus("InviteEventAdapter.SelectAllInviteEventList() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.InviteEvent))
                {
                    cmd.Set(Mabinogi.SQL.Columns.InviteEvent.Id, 0);
                    cmd.Set(Mabinogi.SQL.Columns.InviteEvent.Server, 0);
                    cmd.Set(Mabinogi.SQL.Columns.InviteEvent.InviteCharacterName, 0);
                    cmd.Set(Mabinogi.SQL.Columns.InviteEvent.SendDate, 0);

                    WorkSession.WriteStatus("InviteEventAdapter.SelectAllInviteEventList() : 데이터를 가져옵니다.");
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
                WorkSession.WriteStatus("InviteEventAdapter.SelectAllInviteEventList() : 데이터 베이스에 연결을 종료합니다.");
            }
        }

        private InviteEventList Build(SimpleReader reader)
        {
            if (reader == null)
            {
                throw new Exception("InviteEvent 테이블을 얻어오지 못햇습니다.");
            }
            if (reader.HasRows)
            {
                InviteEventList inviteEventList = new InviteEventList();
                List<InviteEvent> list = new List<InviteEvent>();
                InviteEvent inviteEvent;
                while (reader.Read())
                {
                    inviteEvent = new InviteEvent();
                    inviteEvent.mabiId = reader.GetString(Mabinogi.SQL.Columns.InviteEvent.Id);
                    inviteEvent.servername = reader.GetString(Mabinogi.SQL.Columns.InviteEvent.Server);
                    inviteEvent.invitecharactername = reader.GetString(Mabinogi.SQL.Columns.InviteEvent.InviteCharacterName);
                    inviteEvent.senddate = reader.GetDateTime(Mabinogi.SQL.Columns.InviteEvent.SendDate);
                    list.Add(inviteEvent);
                }
                inviteEventList.InviteEvents = list.ToArray();
                return inviteEventList;
            }
            return new InviteEventList();
        }

        public REPLY_RESULT DeleteInviteEvent(string _MabiId)
        {
            WorkSession.WriteStatus("InviteEventAdapter.DeleteInviteEvent() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("InviteEventAdapter.DeleteInviteEvent() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {
                    // PROCEDURE: DeleteInviteEvent
                    transaction = conn.BeginTransaction();
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.InviteEvent, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.InviteEvent.Id, _MabiId);
                        WorkSession.WriteStatus("InviteEventAdapter.DeleteInviteEvent() : 명령을 실행합니다");
                        if (cmd.Execute() < 1)
                        {
                            Exception ex = new Exception("UpdateInviteEvent 업데이트를 실패하였습니다.", null);
                            WorkSession.WriteStatus(ex.Message, 0);
                            if (transaction != null)
                            {
                                WorkSession.WriteStatus("InviteEventAdapter.DeleteInviteEvent() : 트랜잭션을 롤백합니다");
                                transaction.Rollback();
                            }
                            return REPLY_RESULT.FAIL;
                        }
                    }
                    WorkSession.WriteStatus("InviteEventAdapter.DeleteInviteEvent() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;

                }
                catch (SimpleSqlException ex2)
                {
                    WorkSession.WriteStatus(ex2.Message, ex2.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("InviteEventAdapter.DeleteInviteEvent() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex3)
                {
                    WorkSession.WriteStatus(ex3.Message);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("InviteEventAdapter.DeleteInviteEvent() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("InviteEventAdapter.DeleteInviteEvent() : 연결을 종료합니다");
                }
            }
        }
    }
}
