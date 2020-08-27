using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class GuildIdPoolAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.MabiGuild.GuildIdPool;
        public GuildIdPoolAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public GuildIDPool Read(SimpleConnection conn)
        {
            GuildIDPool result = new GuildIDPool() { count = 1 };
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.GuildIdPool))
            {
                // PROCEDURE: SelectGuildIdPool2
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result.count = reader.GetInt64(Mabinogi.SQL.Columns.GuildIdPool.Count);
                        return result;
                    }
                }
            }

            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiGuild.GuildIdPool))
            {
                cmd.Set(Mabinogi.SQL.Columns.GuildIdPool.Count, 1);
                cmd.Execute();
            }
            return result;
        }

        public long GetIdPool()
        {
            WorkSession.WriteStatus("GuildIdPoolSqlAdapter.GetIdPool() : 함수에 진입하였습니다");
            try
            {
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.GuildIdPool))
                {
                    GuildIDPool guildIDPool = Read(conn);

                    cmd.Set(Mabinogi.SQL.Columns.GuildIdPool.Count, guildIDPool.count + 1000);
                    cmd.Execute();

                    return guildIDPool.count;
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

