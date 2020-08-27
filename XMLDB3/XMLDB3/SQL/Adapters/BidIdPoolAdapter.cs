using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class BidIdPoolAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.BidIdPool;

        public BidIdPoolAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public long GetIdPool()
        {
            WorkSession.WriteStatus("BidIdPoolSqlAdapter.GetIdPool() : 함수에 진입하였습니다");

            try
            {
                BidIDPool bidIDPool = new BidIDPool();
                // PROCEDURE: SelectBidIdPool
                using (var conn = Connection)
                {
                    using (var reader = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.BidIdPool).ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            bidIDPool.count = reader.GetInt64(Mabinogi.SQL.Columns.BidIdPool.Count);

                            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.BidIdPool))
                            {
                                cmd.Set(Mabinogi.SQL.Columns.BidIdPool.Count, bidIDPool.count + 1000);
                                cmd.Execute();
                            }
                            return bidIDPool.count;
                        }
                    }
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.BidIdPool))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.BidIdPool.Count, 0);
                        cmd.Execute();
                    }
                }

                return bidIDPool.count;
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
	}
}
