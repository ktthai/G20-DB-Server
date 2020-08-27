using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class ItemIdPoolAdapter : SqlAdapter
	{
		protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.ItemIdPool;
        public ItemIdPoolAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        private long Read(SimpleConnection conn)
		{
			// PROCEDURE: SelectItemIdPool
			using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.ItemIdPool))
			using (var reader = cmd.ExecuteReader())
			{
				if (reader.Read())
					return reader.GetInt64(Mabinogi.SQL.Columns.ItemIdPool.Count);
			}

			using(var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.ItemIdPool))
            {
				cmd.Set(Mabinogi.SQL.Columns.ItemIdPool.Count, 0);
				cmd.Execute();
            }

			return 0;
		}

		public long GetIdPool()
		{
			WorkSession.WriteStatus("ItemIdPoolSqlAdapter.GetIdPool() : 함수에 진입하였습니다");
			try
			{
				ItemIdPoolMutex.Enter();
				
				try
				{
					using (var conn = Connection)
					using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.ItemIdPool))
					{
						long itemIDPool = Read(conn);

						cmd.Set(Mabinogi.SQL.Columns.ItemIdPool.Count, itemIDPool + 1000);
						cmd.Execute();
						return itemIDPool;
					}
				}
				catch (SimpleSqlException ex)
				{
					ExceptionMonitor.ExceptionRaised(ex);
					WorkSession.WriteStatus(ex.Message, ex.Number);
					return 0L;
				}
				finally
				{
				}
			}
			catch (Exception ex2)
			{
				ExceptionMonitor.ExceptionRaised(ex2);
				WorkSession.WriteStatus(ex2.Message);
				return 0L;
			}
			finally
			{
				ItemIdPoolMutex.Leave();
			}
		}
	}
}
