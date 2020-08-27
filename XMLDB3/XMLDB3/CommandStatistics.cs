using System;
using System.Collections;
using System.Timers;

namespace XMLDB3
{
	public class CommandStatistics
	{
		public enum CommandType
		{
			cctCharacterRead,
			cctCharacterWrite,
			cctPetRead,
			cctPetWrite,
			cctBankRead,
			cctBankWrite,
			cctBankWriteEx,
			cctCharItemDelete,
			cctPrivateFarmRead,
			cctPrivateFarmWrite
		}

		private class CommandStat
		{
			public float avg;

			public long min;

			public long max;

			public int count;

			public void Update(long _time)
			{
				avg = avg * (float)count / (float)(count + 1) + (float)_time / (float)(count + 1);
				count++;
				if (_time > max)
				{
					max = _time;
				}
				if (min == 0 || _time < min)
				{
					min = _time;
				}
			}
		}

		private static bool active;

		private static Hashtable commands;

		private static Hashtable sessions;

		private static Timer timer;

		private static string connStr;

		static CommandStatistics()
		{
			active = false;
			connStr = ConfigManager.StatisticsConnection;
			int statisticsPeriod = ConfigManager.StatisticsPeriod;
			if (connStr != null && connStr != string.Empty && statisticsPeriod > 0)
			{
				InitCommands();
				InitSessions();
				timer = new Timer(statisticsPeriod);
				timer.Elapsed += WriteToDB;
				timer.Enabled = true;
				timer.AutoReset = true;
				timer.Start();
				active = true;
			}
		}

		private static void InitCommands()
		{
			commands = new Hashtable();
			commands.Add(CommandType.cctCharacterRead, new CommandStat());
			commands.Add(CommandType.cctCharacterWrite, new CommandStat());
			commands.Add(CommandType.cctPetRead, new CommandStat());
			commands.Add(CommandType.cctPetWrite, new CommandStat());
			commands.Add(CommandType.cctBankRead, new CommandStat());
			commands.Add(CommandType.cctBankWrite, new CommandStat());
			commands.Add(CommandType.cctBankWriteEx, new CommandStat());
			commands.Add(CommandType.cctCharItemDelete, new CommandStat());
			commands.Add(CommandType.cctPrivateFarmRead, new CommandStat());
			commands.Add(CommandType.cctPrivateFarmWrite, new CommandStat());
		}

		private static void InitSessions()
		{
			sessions = new Hashtable();
			sessions.Add("CharacterReadCommand", new CommandStat());
			sessions.Add("CharacterWriteCommand", new CommandStat());
			sessions.Add("PetReadCommand", new CommandStat());
			sessions.Add("PetWriteCommand", new CommandStat());
			sessions.Add("BankReadCommand", new CommandStat());
			sessions.Add("BankUpdateCommand", new CommandStat());
			sessions.Add("BankUpdateExCommand", new CommandStat());
			sessions.Add("PrivateFarmReadCommand", new CommandStat());
			sessions.Add("PrivateFarmWriteCommand", new CommandStat());
		}

		public static void RegisterSessionTime(string _command, long _time)
		{
			if (active)
			{
				CommandStat commandStat = (CommandStat)sessions[_command];
				if (commandStat != null)
				{
					lock (commandStat)
					{
						commandStat.Update(_time);
					}
				}
			}
		}

		public static void RegisterCommandTime(CommandType _type, long _time)
		{
			if (active)
			{
				CommandStat commandStat = (CommandStat)commands[_type];
				if (commandStat != null)
				{
					lock (commandStat)
					{
						commandStat.Update(_time);
					}
				}
			}
		}

		private static void WriteToDB(object sender, ElapsedEventArgs e)
		{
			Hashtable hashtable;
			lock (commands)
			{
				hashtable = commands;
				InitCommands();
			}
			Hashtable hashtable2;
			lock (sessions)
			{
				hashtable2 = sessions;
				InitSessions();
			}

            // TODO: rewrite this?

            /*SqlConnection sqlConnection = new SqlConnection(connStr);
            string text = string.Empty;
            try
            {
                CacheStatistics statistics = ObjectCache.Character.Statistics;
                object obj = text;
                text = string.Concat(obj, "exec InsertCacheHit  @name=", UpdateUtility.BuildString(statistics.Name), ",@total=", statistics.Total, ",@hit=", statistics.Hit, ",@size=", statistics.Size, "\n");
                statistics = ObjectCache.Bank.Statistics;
                object obj2 = text;
                text = string.Concat(obj2, "exec InsertCacheHit  @name=", UpdateUtility.BuildString(statistics.Name), ",@total=", statistics.Total, ",@hit=", statistics.Hit, ",@size=", statistics.Size, "\n");
                statistics = ObjectCache.PrivateFarm.Statistics;
                object obj3 = text;
                text = string.Concat(obj3, "exec InsertCacheHit  @name=", UpdateUtility.BuildString(statistics.Name), ",@total=", statistics.Total, ",@hit=", statistics.Hit, ",@size=", statistics.Size, "\n");
                IDictionaryEnumerator enumerator = hashtable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    object obj4 = text;
                    text = string.Concat(obj4, "exec InsertCommandStat  @command=", UpdateUtility.BuildString(enumerator.Key.ToString()), ",@avg=", ((CommandStat)enumerator.Value).avg, ",@min=", ((CommandStat)enumerator.Value).min, ",@max=", ((CommandStat)enumerator.Value).max, ",@count=", ((CommandStat)enumerator.Value).count, "\n");
                }
                enumerator = hashtable2.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    object obj5 = text;
                    text = string.Concat(obj5, "exec InsertCommandStat  @command=", UpdateUtility.BuildString(enumerator.Key.ToString()), ",@avg=", ((CommandStat)enumerator.Value).avg, ",@min=", ((CommandStat)enumerator.Value).min, ",@max=", ((CommandStat)enumerator.Value).max, ",@count=", ((CommandStat)enumerator.Value).count, "\n");
                }
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(text, sqlConnection);
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlCommand.ExecuteNonQuery();
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, text);
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, text);
            }
            finally
            {
                sqlConnection.Close();
            }*/
        }
    }
}
