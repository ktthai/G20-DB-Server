using System;
using Mabinogi.SQL;


namespace XMLDB3
{
    public class PropIdPoolAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.PropIdPool;

        public PropIdPoolAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        protected long Read(SimpleConnection conn)
        {
            // PROCEDURE: SelectPropIdPool
            using (var readCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PropIdPool))
            using (var reader = readCmd.ExecuteReader())
            {
                if (reader.Read())
                    return reader.GetInt64(Mabinogi.SQL.Columns.PropIdPool.Count);
            }

            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.PropIdPool))
            {
                cmd.Set(Mabinogi.SQL.Columns.PropIdPool.Count, 0);
                cmd.Execute();
                return 0;
            }
        }

        public long GetIdPool()
        {
            WorkSession.WriteStatus("PropIdPoolSqlAdapter.GetIdPool() : 함수에 진입하였습니다");
            try
            {
                using (var conn = Connection)
                {
                    long propIDPool = Read(conn);

                    // PROCEDURE: UpdatePropIdPool
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.PropIdPool))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.PropIdPool.Count, propIDPool + 1000);
                        cmd.Execute();
                    }

                    return propIDPool;
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

