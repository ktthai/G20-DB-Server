using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class FacilityIdPoolAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.PrivateFarmFacilityIdPool;
        public FacilityIdPoolAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public long GetIdPool()
        {
            WorkSession.WriteStatus("FacilityIdPoolSqlAdapter.GetIdPool() : 함수에 진입하였습니다");
            try
            {
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmFacilityIdPool))
                {
                    PrivateFarmFacilityIdPool privateFarmFacilityIdPool = Read(conn);
                    cmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacilityIdPool.Count, privateFarmFacilityIdPool.count + 1000);
                    return privateFarmFacilityIdPool.count;
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
        }

        private PrivateFarmFacilityIdPool Read(SimpleConnection conn)
        {
            PrivateFarmFacilityIdPool result = new PrivateFarmFacilityIdPool() { count = 0 };

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmFacilityIdPool))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result.count = reader.GetInt64(Mabinogi.SQL.Columns.PrivateFarmFacilityIdPool.Count);
                        return result;
                    }
                }
            }
            using (var insertCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarmFacilityIdPool))
            {
                insertCmd.Set(Mabinogi.SQL.Columns.PrivateFarmFacilityIdPool.Count, 0);
                insertCmd.Execute();
            }

            return result;
        }
    }
}
