using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class LinkedApCharacterAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.AccountCharacterLinkedAP;
        public LinkedApCharacterAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public REPLY_RESULT SelectLinkedApCharacter(string _serverName, long _charId, out LinkedApCharacter _linkedApCharacter)
        {
            WorkSession.WriteStatus("LinkedApCharacterSqlAdapter.SelectLinkedApCharacter() : 함수에 진입하였습니다");

            _linkedApCharacter = new LinkedApCharacter();
            try
            {
                WorkSession.WriteStatus("LinkedApCharacterSqlAdapter.SelectLinkedApCharacter() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCharacterLinkedAP))
                {
                    // PROCEDURE: SelectLinkedAP
                    cmd.Where(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.ServerId, _serverName);
                    cmd.Where(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.CharId, _charId);

                    WorkSession.WriteStatus("LinkedApCharacterSqlAdapter.SelectLinkedApCharacter() : 데이타를 채웁니다.");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader == null || reader.Read() == false)
                        {
                            _linkedApCharacter = null;
                        }
                        else
                        {
                            _linkedApCharacter.serverName = reader.GetString(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.ServerId);
                            _linkedApCharacter.charID = reader.GetInt64(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.CharId);
                            _linkedApCharacter.savedAp = reader.GetInt32(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.SavedAp);
                            _linkedApCharacter.termAp = reader.GetInt32(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.TermAp);
                            _linkedApCharacter.resetTime = reader.GetInt64(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.ResetTime);
                        }
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
                WorkSession.WriteStatus("LinkedApCharacterSqlAdapter.SelectLinkedApCharacter() : 연결을 종료합니다");
            }
            return REPLY_RESULT.SUCCESS;
        }

        public REPLY_RESULT UpdateLinkedApCharacter(LinkedApCharacter _linkedApCharacter)
        {
            WorkSession.WriteStatus("LinkedApCharacterSqlAdapter.UpdateLinkedApCharacter() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("LinkedApCharacterSqlAdapter.UpdateLinkedApCharacter() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
            {

                // PROCEDURE: UpdateLinkedAP
                
                    transaction = conn.BeginTransaction();
                    WorkSession.WriteStatus("LinkedApCharacterSqlAdapter.UpdateLinkedApCharacter() : 명령을 실행합니다");

                    using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCharacterLinkedAP, transaction))
                    {
                        upCmd.Where(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.ServerId, _linkedApCharacter.serverName);
                        upCmd.Where(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.CharId, _linkedApCharacter.charID);

                        upCmd.Set(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.SavedAp, _linkedApCharacter.savedAp);
                        upCmd.Set(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.TermAp, _linkedApCharacter.termAp);
                        upCmd.Set(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.ResetTime, _linkedApCharacter.resetTime);

                        if (upCmd.Execute() < 1)
                        {
                            using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCharacterLinkedAP, transaction))
                            {
                                insCmd.Set(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.ServerId, _linkedApCharacter.serverName);
                                insCmd.Set(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.CharId, _linkedApCharacter.charID);
                                insCmd.Set(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.SavedAp, _linkedApCharacter.savedAp);
                                insCmd.Set(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.TermAp, _linkedApCharacter.termAp);
                                insCmd.Set(Mabinogi.SQL.Columns.AccountCharacterLinkedAP.ResetTime, _linkedApCharacter.resetTime);
                                insCmd.Execute();
                            }
                        }
                    }
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
                WorkSession.WriteStatus("LinkedApCharacterSqlAdapter.UpdateLinkedApCharacter() : 연결을 종료합니다");
            }
            return REPLY_RESULT.SUCCESS;
        }
    }
    }
}
