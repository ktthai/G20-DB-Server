using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class CharIdPoolAdapter : SqlAdapter
	{
		protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.CharIdPool;
        public CharIdPoolAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public long GetIdPool()
		{
			WorkSession.WriteStatus("CharIdPoolSqlAdapter.GetIdPool() : 함수에 진입하였습니다");

			try
			{
				// PROCEDURE: SelectCharIdPool
				using (var conn = Connection)
				{
					CharIDPool charIDPool = new CharIDPool();
					using (var reader = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharIdPool).ExecuteReader())
					{

						if (reader.Read())
						{
							charIDPool.count = reader.GetInt64(Mabinogi.SQL.Columns.CharIdPool.Count);
							reader.Close();

							using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CharIdPool))
							{
								cmd.Set(Mabinogi.SQL.Columns.CharIdPool.Count, charIDPool.count + 1000);
								cmd.Execute();
							}
							return charIDPool.count;
						}
					}

					using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharIdPool))
					{
						cmd.Set(Mabinogi.SQL.Columns.CharIdPool.Count, 0);
						cmd.Execute();
					}

					return charIDPool.count;
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
