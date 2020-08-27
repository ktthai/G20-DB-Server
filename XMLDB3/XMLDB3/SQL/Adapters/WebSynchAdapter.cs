using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class WebSynchAdapter : SqlAdapter
	{
		protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.CharRefSync;

        public WebSynchAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public void AddCharRefSync(string account, string server, CharacterInfo data, SimpleConnection conn, SimpleTransaction transaction)
		{
			
			using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharRefSync, transaction))
			{
				cmd.Set(Mabinogi.SQL.Columns.CharRefSync.Id, account);
				cmd.Set(Mabinogi.SQL.Columns.CharRefSync.CharacterId, data.id);
				cmd.Set(Mabinogi.SQL.Columns.CharRefSync.CharacterName, data.name);
				cmd.Set(Mabinogi.SQL.Columns.CharRefSync.Server, server);

				cmd.Execute();
			}
		}

        public void RemoveCharacter(string account, long charID, string serverName, SimpleConnection conn, SimpleTransaction trans)
        {
			using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharRefSync))
			{
				cmd.Where(Mabinogi.SQL.Columns.CharRefSync.Id, account);
				cmd.Where(Mabinogi.SQL.Columns.CharRefSync.CharacterId, charID);
				cmd.Where(Mabinogi.SQL.Columns.CharRefSync.Server, serverName);

				using (var reader = cmd.ExecuteReader())
				{

					if (!reader.Read())
						throw new SimpleSqlException("No such reference!");

					using (var cmd1 = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CharDeletedRefSync, trans))
					{
						cmd1.Set(Mabinogi.SQL.Columns.CharDeletedRefSync.Id, account);
						cmd1.Set(Mabinogi.SQL.Columns.CharDeletedRefSync.CharacterId, charID);
						cmd1.Set(Mabinogi.SQL.Columns.CharDeletedRefSync.CharacterName, reader.GetString(Mabinogi.SQL.Columns.CharRefSync.CharacterName));
						cmd1.Set(Mabinogi.SQL.Columns.CharDeletedRefSync.Server, serverName);
						cmd1.Set(Mabinogi.SQL.Columns.CharDeletedRefSync.DeletedTime, DateTime.Now);

						cmd1.Execute();
					}

					using (var cmd1 = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CharRefSync, trans))
					{
						cmd1.Where(Mabinogi.SQL.Columns.CharRefSync.Id, account);
						cmd1.Where(Mabinogi.SQL.Columns.CharRefSync.CharacterId, charID);
						cmd1.Where(Mabinogi.SQL.Columns.CharRefSync.Server, serverName);

						cmd1.Execute();
					}
				}
			}
        }
    }
}
