using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class AccountAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Account;

        public AccountAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public Account Read(string _id, bool get2ndPass = false)
        {
            using (var conn = Connection)
                return GetAccount(_id, get2ndPass, conn);
        }

        internal static Account GetAccount(string id, bool get2ndPass, SimpleConnection conn)
        {
            // PROCEDURE: SelectAccount2
            using (var mc = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Account))
            {
                mc.Where(Mabinogi.SQL.Columns.Account.Id, id);

                using (var reader = mc.ExecuteReader())
                {
                    if (reader.Read() == false)
                    {
                        return null;
                    }

                    return Account.Build(reader, get2ndPass);
                }
            }
        }

        public Account Read2PW(string _id)
        {
            // For some reason this function read the account from the DB, then added the SecondaryPassAuth to the account
            return Read(_id, true);
        }

        internal static bool CreateAccount(Account data, SimpleConnection conn)
        {
            // PROCEDURE: CreateAccount
            try
            {
                using (var mc = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Account))
                {
                    mc.Set(Mabinogi.SQL.Columns.Account.Id, data.id);
                    mc.Set(Mabinogi.SQL.Columns.Account.Password, data.password);
                    mc.Set(Mabinogi.SQL.Columns.Account.Name, data.name);
                    mc.Set(Mabinogi.SQL.Columns.Account.Email, data.email);
                    mc.Set(Mabinogi.SQL.Columns.Account.Flag, data.flag);
                    mc.Set(Mabinogi.SQL.Columns.Account.BlockingDate, data.blocking_date);
                    mc.Set(Mabinogi.SQL.Columns.Account.BlockingDuration, data.blocking_duration);
                    mc.Set(Mabinogi.SQL.Columns.Account.Authority, data.authority);
                    mc.Set(Mabinogi.SQL.Columns.Account.MachineIDs, data.machineids);

                    if (data.SecondaryPasswordAuth != null)
                    {
                        mc.Set(Mabinogi.SQL.Columns.Account.SecondPassword, data.SecondaryPasswordAuth.password2);
                        mc.Set(Mabinogi.SQL.Columns.Account.SecondPassMissCount, data.SecondaryPasswordAuth.missCount);
                    }

                    mc.Execute();
                    return true;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, data);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message);
                ExceptionMonitor.ExceptionRaised(ex2);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountSqlAdapter.CreateGMAccount() : 연결을 종료합니다");
            }
        }

        public bool Create(Account data)
        {
            using (var conn = Connection)
                return CreateAccount(data, conn);
        }

        public bool CreateGMAccount(string _id)
        {
            WorkSession.WriteStatus("AccountSqlAdapter.CreateGMAccount() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("AccountSqlAdapter.CreateGMAccount() : 데이터베이스와 연결합니다");

                using (var cmd = Connection.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Account))
                {

                    WorkSession.WriteStatus("AccountSqlAdapter.CreateGMAccount() : 프로시져 명령 객체를 작성합니다");
                    cmd.Where(Mabinogi.SQL.Columns.Account.Id, _id);
                    cmd.Set(Mabinogi.SQL.Columns.Account.Authority, Authority.GM);

                    WorkSession.WriteStatus("AccountSqlAdapter.CreateGMAccount() : 명령을 실행합니다");
                    cmd.Execute();
                }
                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _id);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message);
                ExceptionMonitor.ExceptionRaised(ex2);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountSqlAdapter.CreateGMAccount() : 연결을 종료합니다");
            }
        }

        public bool CreateNxK(long nexonOID, string nexonId, byte authority, out string mabinogiId)
        {
            WorkSession.WriteStatus("AccountSqlAdapter.CreateNxK() : 함수에 진입하였습니다");

            mabinogiId = null;
            return false;
        }

        public bool CreateNxJ(string nexonId, string mabinogiId, string password)
        {
            return false;
        }

        public bool AccountListNxJ(string nexonId, out string[] mabiIdList)
        {
            mabiIdList = null;
            return false;
        }

        public bool LoginSignal(string _account, long _sessionKey, string _address, int _ispCode)
        {
            WorkSession.WriteStatus("AccountSqlAdapter.LoginHistory() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("AccountSqlAdapter.LoginHistory() : 데이터베이스와 연결합니다");
                using (var conn = this.Connection)
                using (var transaction = conn.BeginTransaction())
                {
                    int rows = 0;
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountSession, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.AccountSession.Account, _account);
                        cmd.Set(Mabinogi.SQL.Columns.AccountSession.Session, _sessionKey);

                        rows = cmd.Execute();
                    }

                    if (rows == 0)
                    {
                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.AccountSession))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.AccountSession.Account, _account);
                            cmd.Set(Mabinogi.SQL.Columns.AccountSession.Session, _sessionKey);

                            cmd.Execute();
                        }
                    }

                    LoginHistory(_account, _address, _ispCode, conn, transaction);
                    WorkSession.WriteStatus("AccountSqlAdapter.LoginHistory() : 프로시져 명령 객체를 작성합니다");
                    transaction.Commit();
                }
                WorkSession.WriteStatus("AccountSqlAdapter.LoginHistory() : 명령을 실행합니다");

                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message);
                ExceptionMonitor.ExceptionRaised(ex2);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountSqlAdapter.LoginHistory() : 연결을 종료합니다");
            }
        }

        private void LoginHistory(string account, string address, int ispCode, SimpleConnection conn, SimpleTransaction transaction)
        {
            var mc = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.AccountLog);
            mc.Where(Mabinogi.SQL.Columns.AccountLog.Account, account);

            int rows = 0;
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountLog, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.AccountLog.Account, account);

                cmd.Set(Mabinogi.SQL.Columns.AccountLog.Address, address);
                cmd.Set(Mabinogi.SQL.Columns.AccountLog.RegDate, DateTime.Now);
                cmd.Set(Mabinogi.SQL.Columns.AccountLog.IntISPCode, ispCode);

                rows = cmd.Execute();
            }

            if (rows == 0)
            {
                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.AccountLog, transaction))
                {
                    cmd.Set(Mabinogi.SQL.Columns.AccountLog.Account, account);
                    cmd.Set(Mabinogi.SQL.Columns.AccountLog.Address, address);
                    cmd.Set(Mabinogi.SQL.Columns.AccountLog.RegDate, DateTime.Now);
                    cmd.Set(Mabinogi.SQL.Columns.AccountLog.IntISPCode, ispCode);

                    cmd.Execute();
                }
            }
        }

        public bool LogoutSignal(string _account)
        {
            WorkSession.WriteStatus("AccountSqlAdapter.LogoutSignal() : 함수에 진입하였습니다");

            // TODO: Check if this works, since it is all guess
            try
            {
                WorkSession.WriteStatus("AccountSqlAdapter.LogoutSignal() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    WorkSession.WriteStatus("AccountSqlAdapter.LogoutSignal() : 프로시져 명령 객체를 작성합니다");

                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.AccountSession))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.AccountSession.Account, _account);
                        cmd.Execute();
                    }

                    using (var getCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.AccountLoginHistory))
                    {
                        getCmd.Where(Mabinogi.SQL.Columns.AccountLoginHistory.Id, _account);
                        getCmd.Set(Mabinogi.SQL.Columns.AccountLoginHistory.LoginTime, 0);
                        getCmd.Set(Mabinogi.SQL.Columns.AccountLoginHistory.LoginCount, 0);

                        using (var reader = getCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var loginTime = reader.GetDateTime(Mabinogi.SQL.Columns.AccountLoginHistory.LoginTime);
                                var loginCount = reader.GetInt32(Mabinogi.SQL.Columns.AccountLoginHistory.LoginCount);
                                reader.Close();

                                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountLoginHistory))
                                {
                                    cmd.Where(Mabinogi.SQL.Columns.AccountLoginHistory.Id, _account);
                                    cmd.Set(Mabinogi.SQL.Columns.AccountLoginHistory.LoginTime, DateTime.Now);
                                    cmd.Set(Mabinogi.SQL.Columns.AccountLoginHistory.LastLoginTime, loginTime);
                                    cmd.Set(Mabinogi.SQL.Columns.AccountLoginHistory.LoginCount, loginCount + 1);
                                    cmd.Execute();
                                }
                                return true;
                            }
                        }

                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.AccountLoginHistory))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.AccountLoginHistory.Id, _account);
                            cmd.Set(Mabinogi.SQL.Columns.AccountLoginHistory.LoginTime, DateTime.Now);
                            cmd.Set(Mabinogi.SQL.Columns.AccountLoginHistory.LastLoginTime, DateTime.Now);
                            cmd.Set(Mabinogi.SQL.Columns.AccountLoginHistory.LoginCount, 1);
                            cmd.Execute();
                        }
                    }
                }
                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message);
                ExceptionMonitor.ExceptionRaised(ex2);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountSqlAdapter.LoginHistory() : 연결을 종료합니다");
            }

        }

        public bool Ban(string _account, short _bantype, string _manager, short _duration, string _purpose)
        {
            WorkSession.WriteStatus("AccountSqlAdapter.Ban() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("AccountSqlAdapter.Ban() : 데이터베이스와 연결합니다");

                WorkSession.WriteStatus("AccountSqlAdapter.Ban() : 프로시져 명령 객체를 작성합니다");
                using (var mc = Connection.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Account))
                {
                    mc.Where(Mabinogi.SQL.Columns.Account.Id, _account);

                    mc.Set(Mabinogi.SQL.Columns.Account.Flag, _bantype);
                    mc.Set(Mabinogi.SQL.Columns.Account.BlockingDate, DateTime.Now);
                    mc.Set(Mabinogi.SQL.Columns.Account.BlockingDuration, _duration);

                    mc.Execute();
                }
                WorkSession.WriteStatus("AccountSqlAdapter.Ban() : 명령을 실행합니다");
                // TODO: log the purpose and who banned (manager)

                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message);
                ExceptionMonitor.ExceptionRaised(ex2);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountSqlAdapter.Ban() : 연결을 종료합니다");
            }
        }

        public bool Unban(string _account, string _manager)
        {
            WorkSession.WriteStatus("AccountSqlAdapter.Unban() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("AccountSqlAdapter.Unban() : 데이터베이스와 연결합니다");
                WorkSession.WriteStatus("AccountSqlAdapter.Unban() : 프로시져 명령 객체를 작성합니다");
                using (var mc = Connection.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Account))
                {
                    mc.Where(Mabinogi.SQL.Columns.Account.Id, _account);
                    mc.Set(Mabinogi.SQL.Columns.Account.Flag, -1);
                    mc.Set(Mabinogi.SQL.Columns.Account.BlockingDuration, 0);

                    // TODO: log who unbanned (manager)

                    mc.Execute();
                }
                WorkSession.WriteStatus("AccountSqlAdapter.Unban() : 명령을 실행합니다");
                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message);
                ExceptionMonitor.ExceptionRaised(ex2);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountSqlAdapter.Unban() : 연결을 종료합니다");
            }
        }

        public bool UpdateSecondaryPassword(string _id, string _password)
        {
            WorkSession.WriteStatus("AccountSqlAdapter.UpdateSecondaryPassword() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("AccountSqlAdapter.UpdateSecondaryPassword() : 데이터베이스와 연결합니다");


                using (var conn = Connection)
                using (var mc = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Account))
                {
                    mc.Where(Mabinogi.SQL.Columns.Account.Id, _id);
                    mc.Set(Mabinogi.SQL.Columns.Account.SecondPassword, _password);

                    WorkSession.WriteStatus("AccountSqlAdapter.UpdateSecondaryPassword() : 프로시져 명령 객체를 작성합니다");
                    WorkSession.WriteStatus("AccountSqlAdapter.UpdateSecondaryPassword() : 명령을 실행합니다");

                    return mc.Execute() != 0;
                }

            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _id);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message);
                ExceptionMonitor.ExceptionRaised(ex2);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountSqlAdapter.UpdateSecondaryPassword() : 연결을 종료합니다");
            }
        }

        public bool UpdateSecondaryPasswordMissCount(string _id, byte _missCount)
        {
            WorkSession.WriteStatus("AccountSqlAdapter.UpdateSecondaryPasswordMissCount() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("AccountSqlAdapter.UpdateSecondaryPasswordMissCount() : 데이터베이스와 연결합니다");
                WorkSession.WriteStatus("AccountSqlAdapter.UpdateSecondaryPasswordMissCount() : 프로시져 명령 객체를 작성합니다");
                using (var conn = Connection)
                using (var mc = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Account))
                {
                    mc.Where(Mabinogi.SQL.Columns.Account.Id, _id);
                    mc.Set(Mabinogi.SQL.Columns.Account.SecondPassMissCount, _missCount);
                    WorkSession.WriteStatus("AccountSqlAdapter.UpdateSecondaryPasswordMissCount() : 명령을 실행합니다");
                    return mc.Execute() != 0;
                }

            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _id);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message);
                ExceptionMonitor.ExceptionRaised(ex2);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountSqlAdapter.UpdateSecondaryPasswordMissCount() : 연결을 종료합니다");
            }
        }

        public bool QuerySimpleAccount(string _id)
        {
            WorkSession.WriteStatus("AccountSqlAdapter.QuerySimpleAccount() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("AccountSqlAdapter.QuerySimpleAccount() : 데이터베이스와 연결합니다");

                WorkSession.WriteStatus("AccountSqlAdapter.QuerySimpleAccount() : 프로시져 명령 객체를 작성합니다");
                using (var mc = Connection.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Account))
                {
                    mc.Where(Mabinogi.SQL.Columns.Account.Id, _id);

                    using (var reader = mc.ExecuteReader())
                    {
                        WorkSession.WriteStatus("AccountSqlAdapter.QuerySimpleAccount() : 명령을 실행합니다");
                        return reader.HasRows;
                    }
                }

            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _id);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message);
                ExceptionMonitor.ExceptionRaised(ex2);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountSqlAdapter.QuerySimpleAccount() : 연결을 종료합니다");
            }
        }

        public bool QueryInactiveAccount(string _id)
        {
            WorkSession.WriteStatus("AccountSqlAdapter.QueryInactiveAccount() : 함수에 진입하였습니다");

            return false;
        }
    }
}
