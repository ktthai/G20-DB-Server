using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class LoginIdPoolAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.LoginIdPool;

        public LoginIdPoolAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        private long Read(SimpleConnection conn)
        {
            // PROCEDURE: SelectLoginIdPool
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.LoginIdPool))
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                    return reader.GetInt64(Mabinogi.SQL.Columns.LoginIdPool.Count);
            }

            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.LoginIdPool))
            {
                cmd.Set(Mabinogi.SQL.Columns.LoginIdPool.Count, 0);
                cmd.Execute();
            }

            return 0;
        }

        public long GetIdPool(int _size)
        {
            WorkSession.WriteStatus("LoginIdPoolSqlAdapter.GetIdPool() : 함수에 진입하였습니다");

            try
            {
                using (var conn = Connection)
                {
                    long loginIDPool = Read(conn);

                    // PROCEDURE: UpdateLoginIdPool
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.LoginIdPool))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.LoginIdPool.Count, loginIDPool + _size);
                        cmd.Execute();
                        return loginIDPool;
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return 0L;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return 0L;
            }
            finally
            {
             
            }
        }
    }
}
