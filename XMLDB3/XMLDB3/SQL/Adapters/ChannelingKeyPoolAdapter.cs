using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class ChannelingKeyPoolAdapter : SqlAdapter
	{
		protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.ChannelingKeyPool;
        public ChannelingKeyPoolAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public bool Do(ChannelingKey _chKey)
		{
			if (_chKey == null)
			{
				return false;
			}
			WorkSession.WriteStatus("ChannelingKeyPoolSqlAdapter.Do() : 함수에 진입하였습니다");

			try
			{
				using(var conn = Connection)
				using (var readCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.ChannelingKeyPool))
				{
					readCmd.Where(Mabinogi.SQL.Columns.ChannelingKeyPool.KeyString, _chKey.keystring);
					readCmd.Where(Mabinogi.SQL.Columns.ChannelingKeyPool.ProviderCode, _chKey.provider);

					if (!readCmd.ExecuteReader().HasRows)
						using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.ChannelingKeyPool))
						{
							cmd.Set(Mabinogi.SQL.Columns.ChannelingKeyPool.KeyString, _chKey.keystring);
							cmd.Set(Mabinogi.SQL.Columns.ChannelingKeyPool.ProviderCode, _chKey.provider);
							cmd.Set(Mabinogi.SQL.Columns.ChannelingKeyPool.InsertDate, DateTime.Now);

							WorkSession.WriteStatus("ChannelingKeyPoolSqlAdapter.Do() : 명령을 실행합니다");
							return cmd.Execute() > 0;
						}
				}
				WorkSession.WriteStatus("ChannelingKeyPoolSqlAdapter.Do() : 트랜잭션을 커밋합니다");

				return false;
			}
			catch (SimpleSqlException ex)
			{

				WorkSession.WriteStatus("ChannelingKeyPoolSqlAdapter.Do() : 트랜잭션을 롤백합니다");

				ExceptionMonitor.ExceptionRaised(ex, _chKey);
				WorkSession.WriteStatus(ex.Message, ex.Number);
				return false;
			}
			catch (Exception ex2)
			{
				WorkSession.WriteStatus("ChannelingKeyPoolSqlAdapter.Do() : 트랜잭션을 롤백합니다");
				ExceptionMonitor.ExceptionRaised(ex2, _chKey);
				WorkSession.WriteStatus(ex2.Message);
				return false;
			}
			finally
			{
				WorkSession.WriteStatus("ChannelingKeyPoolSqlAdapter.Do() : 연결을 종료합니다");
			}
		}
	}
}
