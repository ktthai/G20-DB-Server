using Mabinogi.SQL;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace XMLDB3
{
    public class ConfigManager
    {
        private const int defMainPort = 13001;

        private const int defProfilerPort = 15400;

        private const int defMonitorPort = 15401;

        private const string configFile = "config.json";

        private const int defMaxChronicleFirst = 5;

        private const int defMaxChronicleLatest = 5;

        private const int defCharacterCacheSize = 2000;

        private const int defBankCacheSize = 1500;

        private static Configuration config;

        public static bool IsLocalMode => config.Sql.LocalSQL;
        public static bool IsFirstRun => config.Sql.FirstRun;

        public static bool ExternalServerEnabled => config.Server.Bridge.Enabled;

        public static int MainPort
        {
            get
            {
                if (config == null || config.Server == null || config.Server.Main == null)
                {
                    return defMainPort;
                }
                return config.Server.Main.port;
            }
        }

        public static int ProfilerPort
        {
            get
            {
                if (config == null || config.Server == null || config.Server.Profiler == null)
                {
                    return defProfilerPort;
                }
                return config.Server.Profiler.Port;
            }
        }

        public static int MonitorPort
        {
            get
            {
                if (config == null || config.Server == null || config.Server.Monitor == null)
                {
                    return defMonitorPort;
                }
                return config.Server.Monitor.Port;
            }
        }

        public static string ReportServer
        {
            get
            {
                if (config == null || config.Report == null)
                {
                    return null;
                }
                return config.Report.Server;
            }
        }

        public static string EventLogSource
        {
            get
            {
                if (config != null && config.Eventlog != null && config.Eventlog.Source != null && config.Eventlog.Source.Length > 0)
                {
                    return config.Eventlog.Source;
                }
                return null;
            }
        }

        public static int StatisticsPeriod
        {
            get
            {
                if (config == null || config.Statistics == null)
                {
                    return -1;
                }
                return config.Statistics.Period * 1000;
            }
        }

        public static string StatisticsConnection
        {
            get
            {
                if (config == null || config.Statistics == null || config.Statistics.Database == null)
                {
                    return null;
                }
                return MakeMySqlConnectionString(config.Statistics.Database.Server, config.Statistics.Database.Port, config.Statistics.Database.Database, config.Statistics.Database.User, config.Statistics.Database.Password);
            }
        }

        public static string ReportSender
        {
            get
            {
                if (config == null || config.Report == null)
                {
                    return null;
                }
                return config.Report.Sender;
            }
        }

        public static string ReportReceiver
        {
            get
            {
                if (config == null || config.Report == null)
                {
                    return null;
                }
                return config.Report.Receiver;
            }
        }

        public static int MaxChronicleFirst => config.ChronicleRank.MaxFirstRank;

        public static int MaxChronicleLatest => config.ChronicleRank.MaxLatestRank;

        public static int CharacterCacheSize => config.Cache.CharacterSize;

        public static int BankCacheSize => config.Cache.BankSize;

        public static int PrivateFarmCacheSize => config.Cache.PrivateFarmSize;

        public static int EquipmentCollectionCacheSize => config.Cache.EquipmentCollectionSize;

        public static bool IsPVPable
        {
            get
            {
                if (config == null || config.Feature == null)
                {
                    return false;
                }
                return config.Feature.PvP;
            }
        }

        public static bool DoesCheckHash
        {
            get
            {
                if (config == null || config.Feature == null)
                {
                    return false;
                }
                return config.Feature.InventoryHash;
            }
            set
            {
                if (config == null)
                {
                    config = new Configuration();
                }
                if (config.Feature == null)
                {
                    config.Feature = new ConfigurationFeature();
                }
                config.Feature.InventoryHash = true;
            }
        }
        public static bool IsRedirectionEnabled
        {
            get
            {
                if (config != null && config.Redirection != null)
                {
                    return config.Redirection.Enable;
                }
                return false;
            }
        }

        public static string RedirectionServer
        {
            get
            {
                if (config != null && config.Redirection != null)
                {
                    return config.Redirection.Server;
                }
                return string.Empty;
            }
        }

        public static int RedirectionPort
        {
            get
            {
                if (config != null && config.Redirection != null)
                {
                    return config.Redirection.Port;
                }
                return 0;
            }
        }

        public static bool ItemMarketEnabled
        {
            get
            {
                if (config != null)
                {
                    return config.ItemMarket.IsEnabled;
                }
                return false;
            }
        }

        public static int ItemMarketGameNo
        {
            get
            {
                if (config != null && config.ItemMarket != null)
                {
                    return config.ItemMarket.GameNo;
                }
                return 0;
            }
        }

        public static int ItemMarketServerNo
        {
            get
            {
                if (config != null && config.ItemMarket != null)
                {
                    return config.ItemMarket.GameNo + config.ItemMarket.ServerNo;
                }
                return 0;
            }
        }

        public static int ItemMarketConnectionPoolNo
        {
            get
            {
                if (config != null && config.ItemMarket != null)
                {
                    return config.ItemMarket.ConnectionPool;
                }
                return 0;
            }
        }

        public static string ItemMarketIP
        {
            get
            {
                if (config != null && config.ItemMarket != null)
                {
                    return config.ItemMarket.Ip;
                }
                return string.Empty;
            }
        }

        public static short ItemMarketPort
        {
            get
            {
                if (config != null && config.ItemMarket != null)
                {
                    return config.ItemMarket.Port;
                }
                return 0;
            }
        }

        public static int ItemMarketCodePage
        {
            get
            {
                if (config != null && config.ItemMarket != null)
                {
                    return config.ItemMarket.CodePage;
                }
                return 0;
            }
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(configFile))
                {
                    config = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(configFile));
                }
                else
                {
                    config = new Configuration();

                    File.WriteAllText(configFile, JsonSerializer.Serialize(config, new JsonSerializerOptions() { WriteIndented = true }));
                }

                if (config.ChronicleRank == null)
                {
                    config.ChronicleRank = new ConfigurationChronicleRank();
                    config.ChronicleRank.MaxFirstRank = defMaxChronicleFirst;
                    config.ChronicleRank.MaxLatestRank = defMaxChronicleLatest;
                }

                if (config.Cache == null)
                {
                    config.Cache = new ConfigurationCache();
                    config.Cache.CharacterSize = defCharacterCacheSize;
                    config.Cache.BankSize = defBankCacheSize;
                }

                if (config.Feature == null)
                {
                    config.Feature = new ConfigurationFeature();
                }
            }
            catch (Exception ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                Process.GetCurrentProcess().Close();
            }
        }

        public static bool TestSqlConnection()
        {
            if(IsFirstRun)
            {
                var conn = config.Sql.Connections.DefaultConnection;
                string connectionStr = string.Format("server={0}; port={1}; uid={2}; password={3};", conn.Server, conn.Port, conn.User, conn.Password);
                if (MySqlDatabaseBuilder.Test(connectionStr))
                {
                    MySqlDatabaseBuilder.BuildDatabases(connectionStr);
                }
            }
            
            string[] array = new string[12]
            {
                Mabinogi.SQL.Tables.Mabinogi.Account,
                Mabinogi.SQL.Tables.Mabinogi.AccountRef,
                Mabinogi.SQL.Tables.Mabinogi.Character,
                Mabinogi.SQL.Tables.Mabinogi.Prop,
                Mabinogi.SQL.Tables.MabiGuild.Guild,
                Mabinogi.SQL.Tables.Mabinogi.CharRefSync,
                Mabinogi.SQL.Tables.Mabinogi.ItemIdPool,
                Mabinogi.SQL.Tables.Mabinogi.CharIdPool,
                Mabinogi.SQL.Tables.Mabinogi.PropIdPool,
                Mabinogi.SQL.Tables.MabiGuild.GuildIdPool,
                Mabinogi.SQL.Tables.Mabinogi.Castle,
                Mabinogi.SQL.Tables.Mabinogi.House
            };
            string[] array2 = array;
            foreach (string text in array2)
            {
                string connectionString = GetConnectionString(text);
                if (connectionString == null)
                {
                    throw new Exception(text + " 설정이 없습니다.");
                }
                
                try
                {
                    MySqlSimpleConnection sqlConnection = new MySqlSimpleConnection(connectionString);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Sql connection failed.");
                    ExceptionMonitor.ExceptionRaised(ex, text);
                    return false;
                }
            }
            return true;
        }

        private static string GetMySqlString(string _Index)
        {
            switch (_Index)
            {
                case Mabinogi.SQL.Tables.Mabinogi.Account:
                    if (config.Sql.Connections.Account != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Account.Server, config.Sql.Connections.Account.Port, config.Sql.Connections.Account.Database, config.Sql.Connections.Account.User, config.Sql.Connections.Account.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.AccountRef:
                    if (config.Sql.Connections.Accountref != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Accountref.Server, config.Sql.Connections.Accountref.Port, config.Sql.Connections.Accountref.Database, config.Sql.Connections.Accountref.User, config.Sql.Connections.Accountref.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.Character:
                    if (config.Sql.Connections.Character != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Character.Server, config.Sql.Connections.Character.Port, config.Sql.Connections.Character.Database, config.Sql.Connections.Character.User, config.Sql.Connections.Character.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.Bank:
                    if (config.Sql.Connections.Bank != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Bank.Server, config.Sql.Connections.Bank.Port, config.Sql.Connections.Bank.Database, config.Sql.Connections.Bank.User, config.Sql.Connections.Bank.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.Prop:
                    if (config.Sql.Connections.Prop != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Prop.Server, config.Sql.Connections.Prop.Port, config.Sql.Connections.Prop.Database, config.Sql.Connections.Prop.User, config.Sql.Connections.Prop.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.MabiGuild.Guild:
                    if (config.Sql.Connections.Guild != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Guild.Server, config.Sql.Connections.Guild.Port, config.Sql.Connections.Guild.Database, config.Sql.Connections.Guild.User, config.Sql.Connections.Guild.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.CharRefSync:
                    if (config.Sql.Connections.WebSync != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.WebSync.Server, config.Sql.Connections.WebSync.Port, config.Sql.Connections.WebSync.Database, config.Sql.Connections.WebSync.User, config.Sql.Connections.WebSync.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.ItemIdPool:
                    if (config.Sql.Connections.ItemIdPool != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.ItemIdPool.Server, config.Sql.Connections.ItemIdPool.Port, config.Sql.Connections.ItemIdPool.Database, config.Sql.Connections.ItemIdPool.User, config.Sql.Connections.ItemIdPool.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.CharIdPool:
                    if (config.Sql.Connections.CharIdPool != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.CharIdPool.Server, config.Sql.Connections.CharIdPool.Port, config.Sql.Connections.CharIdPool.Database, config.Sql.Connections.CharIdPool.User, config.Sql.Connections.CharIdPool.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.PropIdPool:
                    if (config.Sql.Connections.PropIdPool != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.PropIdPool.Server, config.Sql.Connections.PropIdPool.Port, config.Sql.Connections.PropIdPool.Database, config.Sql.Connections.PropIdPool.User, config.Sql.Connections.PropIdPool.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.MabiGuild.GuildIdPool:
                    if (config.Sql.Connections.GuildIdPool != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.GuildIdPool.Server, config.Sql.Connections.GuildIdPool.Port, config.Sql.Connections.GuildIdPool.Database, config.Sql.Connections.GuildIdPool.User, config.Sql.Connections.GuildIdPool.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.BidIdPool:
                    if (config.Sql.Connections.BidIdPool != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.BidIdPool.Server, config.Sql.Connections.BidIdPool.Port, config.Sql.Connections.BidIdPool.Database, config.Sql.Connections.BidIdPool.User, config.Sql.Connections.BidIdPool.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.Castle:
                    if (config.Sql.Connections.House != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Castle.Server, config.Sql.Connections.Castle.Port, config.Sql.Connections.Castle.Database, config.Sql.Connections.Castle.User, config.Sql.Connections.Castle.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.House:
                    if (config.Sql.Connections.House != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.House.Server, config.Sql.Connections.House.Port, config.Sql.Connections.House.Database, config.Sql.Connections.House.User, config.Sql.Connections.House.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.MabiMemo.Memo:
                    if (config.Sql.Connections.Memo != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Memo.Server, config.Sql.Connections.Memo.Port, config.Sql.Connections.Memo.Database, config.Sql.Connections.Memo.User, config.Sql.Connections.Memo.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.MabiChronicle.Chronicle:
                    if (config.Sql.Connections.Chronicle != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Chronicle.Server, config.Sql.Connections.Chronicle.Port, config.Sql.Connections.Chronicle.Database, config.Sql.Connections.Chronicle.User, config.Sql.Connections.Chronicle.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.Ruin:
                    if (config.Sql.Connections.Ruin != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Ruin.Server, config.Sql.Connections.Ruin.Port, config.Sql.Connections.Ruin.Database, config.Sql.Connections.Ruin.User, config.Sql.Connections.Ruin.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.ShopAdvertise.Advertise:
                    if (config.Sql.Connections.ShopAdvertise != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.ShopAdvertise.Server, config.Sql.Connections.ShopAdvertise.Port, config.Sql.Connections.ShopAdvertise.Database, config.Sql.Connections.ShopAdvertise.User, config.Sql.Connections.ShopAdvertise.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.GuestBook:
                    if (config.Sql.Connections.HouseGuestBook != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.HouseGuestBook.Server, config.Sql.Connections.HouseGuestBook.Port, config.Sql.Connections.HouseGuestBook.Database, config.Sql.Connections.HouseGuestBook.User, config.Sql.Connections.HouseGuestBook.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.DungeonRank.DungeonScoreBoard:
                    if (config.Sql.Connections.DungeonRank != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.DungeonRank.Server, config.Sql.Connections.DungeonRank.Port, config.Sql.Connections.DungeonRank.Database, config.Sql.Connections.DungeonRank.User, config.Sql.Connections.DungeonRank.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.ChannelingKeyPool:
                    if (config.Sql.Connections.ChannelingKeyPool != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.ChannelingKeyPool.Server, config.Sql.Connections.ChannelingKeyPool.Port, config.Sql.Connections.ChannelingKeyPool.Database, config.Sql.Connections.ChannelingKeyPool.User, config.Sql.Connections.ChannelingKeyPool.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.PromotionRank:
                    if (config.Sql.Connections.PromotionRank != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.PromotionRank.Server, config.Sql.Connections.PromotionRank.Port, config.Sql.Connections.PromotionRank.Database, config.Sql.Connections.PromotionRank.User, config.Sql.Connections.PromotionRank.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.MailBoxItem:
                    if (config.Sql.Connections.Mailbox != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Mailbox.Server, config.Sql.Connections.Mailbox.Port, config.Sql.Connections.Mailbox.Database, config.Sql.Connections.Mailbox.User, config.Sql.Connections.Mailbox.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.Farm:
                    if (config.Sql.Connections.Farm != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Farm.Server, config.Sql.Connections.Farm.Port, config.Sql.Connections.Farm.Database, config.Sql.Connections.Farm.User, config.Sql.Connections.Farm.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.Bid:
                    if (config.Sql.Connections.Bid != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Bid.Server, config.Sql.Connections.Bid.Port, config.Sql.Connections.Bid.Database, config.Sql.Connections.Bid.User, config.Sql.Connections.Bid.Password);
                    }
                    return null;
                /*case Mabinogi.SQL.Tables.Mabinogi.InviteEvent:
                    if (config.Sql.Connections.Event != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Event.Server, config.Sql.Connections.Event.Port, config.Sql.Connections.Event.Database, config.Sql.Connections.Event.User, config.Sql.Connections.Event.Password);
                    }
                    return null;*/
                case Mabinogi.SQL.Tables.Mabinogi.WorldMeta:
                    if (config.Sql.Connections.WorldMeta != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.WorldMeta.Server, config.Sql.Connections.WorldMeta.Port, config.Sql.Connections.WorldMeta.Database, config.Sql.Connections.WorldMeta.User, config.Sql.Connections.WorldMeta.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.Wine:
                    if (config.Sql.Connections.Wine != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Wine.Server, config.Sql.Connections.Wine.Port, config.Sql.Connections.Wine.Database, config.Sql.Connections.Wine.User, config.Sql.Connections.Wine.Password);
                    }
                    return null;
                /*case Mabinogi.SQL.Tables.Mabinogi.CountryReport:
                    if (config.Sql.Connections.CountryReport != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.CountryReport.Server, config.Sql.Connections.CountryReport.Port, config.Sql.Connections.CountryReport.Database, config.Sql.Connections.CountryReport.User, config.Sql.Connections.CountryReport.Password);
                    }
                    return null;*/
                /*case Mabinogi.SQL.Tables.Mabinogi.LoginOutReport:
                    if (config.Sql.Connections.LoginOutReport != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.LoginOutReport.Server, config.Sql.Connections.LoginOutReport.Port, config.Sql.Connections.LoginOutReport.Database, config.Sql.Connections.LoginOutReport.User, config.Sql.Connections.LoginOutReport.Password);
                    }
                    return null;*/
                case Mabinogi.SQL.Tables.Mabinogi.Husky:
                    if (config.Sql.Connections.Husky != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Husky.Server, config.Sql.Connections.Husky.Port, config.Sql.Connections.Husky.Database, config.Sql.Connections.Husky.User, config.Sql.Connections.Husky.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.LoginIdPool:
                    if (config.Sql.Connections.LoginIdPool != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.LoginIdPool.Server, config.Sql.Connections.LoginIdPool.Port, config.Sql.Connections.LoginIdPool.Database, config.Sql.Connections.LoginIdPool.User, config.Sql.Connections.LoginIdPool.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.PrivateFarm:
                    if (config.Sql.Connections.PrivateFarm != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.PrivateFarm.Server, config.Sql.Connections.PrivateFarm.Port, config.Sql.Connections.PrivateFarm.Database, config.Sql.Connections.PrivateFarm.User, config.Sql.Connections.PrivateFarm.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.PrivateFarmRecommend:
                    if (config.Sql.Connections.PrivateFarmRecommend != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.PrivateFarmRecommend.Server, config.Sql.Connections.PrivateFarmRecommend.Port, config.Sql.Connections.PrivateFarmRecommend.Database, config.Sql.Connections.PrivateFarmRecommend.User, config.Sql.Connections.PrivateFarmRecommend.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.PrivateFarmFacilityIdPool:
                    if (config.Sql.Connections.FacilityIdPool != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.FacilityIdPool.Server, config.Sql.Connections.FacilityIdPool.Port, config.Sql.Connections.FacilityIdPool.Database, config.Sql.Connections.FacilityIdPool.User, config.Sql.Connections.FacilityIdPool.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.ScrapBook:
                    if (config.Sql.Connections.ScrapBook != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.ScrapBook.Server, config.Sql.Connections.ScrapBook.Port, config.Sql.Connections.ScrapBook.Database, config.Sql.Connections.ScrapBook.User, config.Sql.Connections.ScrapBook.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.Commerce:
                    if (config.Sql.Connections.Commerce != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Commerce.Server, config.Sql.Connections.Commerce.Port, config.Sql.Connections.Commerce.Database, config.Sql.Connections.Commerce.User, config.Sql.Connections.Commerce.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.CommerceProduct:
                    if (config.Sql.Connections.CommerceSystem != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.CommerceSystem.Server, config.Sql.Connections.CommerceSystem.Port, config.Sql.Connections.CommerceSystem.Database, config.Sql.Connections.CommerceSystem.User, config.Sql.Connections.CommerceSystem.Password);
                    }
                    return null;
                case Mabinogi.SQL.Tables.Mabinogi.CommerceCriminal:
                    if (config.Sql.Connections.CommerceCriminal != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.CommerceCriminal.Server, config.Sql.Connections.CommerceCriminal.Port, config.Sql.Connections.CommerceCriminal.Database, config.Sql.Connections.CommerceCriminal.User, config.Sql.Connections.CommerceCriminal.Password);
                    }
                    break;
                case Mabinogi.SQL.Tables.Mabinogi.Recommend:
                    if (config.Sql.Connections.Recommend != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Recommend.Server, config.Sql.Connections.Recommend.Port, config.Sql.Connections.Recommend.Database, config.Sql.Connections.Recommend.User, config.Sql.Connections.Recommend.Password);
                    }
                    break;
                case Mabinogi.SQL.Tables.Mabinogi.GoldLog:
                    if (config.Sql.Connections.GoldLog != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.GoldLog.Server, config.Sql.Connections.GoldLog.Port, config.Sql.Connections.GoldLog.Database, config.Sql.Connections.GoldLog.User, config.Sql.Connections.GoldLog.Password);
                    }
                    break;
                case Mabinogi.SQL.Tables.Mabinogi.AccountCharacterLinkedAP:
                    if (config.Sql.Connections.LinkedApCharacter != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.LinkedApCharacter.Server, config.Sql.Connections.LinkedApCharacter.Port, config.Sql.Connections.LinkedApCharacter.Database, config.Sql.Connections.LinkedApCharacter.User, config.Sql.Connections.LinkedApCharacter.Password);
                    }
                    break;
                case Mabinogi.SQL.Tables.Mabinogi.EquipCollect:
                    if (config.Sql.Connections.EquipmentCollection != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.EquipmentCollection.Server, config.Sql.Connections.EquipmentCollection.Port, config.Sql.Connections.EquipmentCollection.Database, config.Sql.Connections.EquipmentCollection.User, config.Sql.Connections.EquipmentCollection.Password);
                    }
                    break;
                case Mabinogi.SQL.Tables.Mabinogi.SoulMate:
                    if (config.Sql.Connections.Soulmate != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.Soulmate.Server, config.Sql.Connections.Soulmate.Port, config.Sql.Connections.Soulmate.Database, config.Sql.Connections.Soulmate.User, config.Sql.Connections.Soulmate.Password);
                    }
                    break;
                case Mabinogi.SQL.Tables.Mabinogi.PersonalRanking:
                    if (config.Sql.Connections.PersonalRanking != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.PersonalRanking.Server, config.Sql.Connections.PersonalRanking.Port, config.Sql.Connections.PersonalRanking.Database, config.Sql.Connections.PersonalRanking.User, config.Sql.Connections.PersonalRanking.Password);
                    }
                    break;
                /*case Mabinogi.SQL.Tables.Mabinogi.SetInfo:
                    if (config.Sql.Connections.SetInfo != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.SetInfo.Server, config.Sql.Connections.SetInfo.Port, config.Sql.Connections.SetInfo.Database, config.Sql.Connections.SetInfo.User, config.Sql.Connections.SetInfo.Password);
                    }
                    break;*/
                case Mabinogi.SQL.Tables.Mabi_Novel.MabiNovel:
                    if (config.Sql.Connections.MabiNovel != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.MabiNovel.Server, config.Sql.Connections.MabiNovel.Port, config.Sql.Connections.MabiNovel.Database, config.Sql.Connections.MabiNovel.User, config.Sql.Connections.MabiNovel.Password);
                    }
                    break;
                case Mabinogi.SQL.Tables.Mabi_Novel.MabiNovelBoard:
                    if (config.Sql.Connections.MabiNovelBoard != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.MabiNovelBoard.Server, config.Sql.Connections.MabiNovelBoard.Port, config.Sql.Connections.MabiNovelBoard.Database, config.Sql.Connections.MabiNovelBoard.User, config.Sql.Connections.MabiNovelBoard.Password);
                    }
                    break;
                case Mabinogi.SQL.Tables.Mabinogi.HelpPointRanking:
                    if (config.Sql.Connections.HelpPointRank != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.HelpPointRank.Server, config.Sql.Connections.HelpPointRank.Port, config.Sql.Connections.HelpPointRank.Database, config.Sql.Connections.HelpPointRank.User, config.Sql.Connections.HelpPointRank.Password);
                    }
                    break;
                case Mabinogi.SQL.Tables.Mabinogi.InviteEvent:
                    if (config.Sql.Connections.InviteEvent != null)
                    {
                        return MakeMySqlConnectionString(config.Sql.Connections.InviteEvent.Server, config.Sql.Connections.InviteEvent.Port, config.Sql.Connections.InviteEvent.Database, config.Sql.Connections.InviteEvent.User, config.Sql.Connections.InviteEvent.Password);
                    }
                    break;
            }

            if (config.Sql.Connections.DefaultConnection != null)
            {
                Console.WriteLine("Default Connection string in XMLDB : " + _Index);
                return MakeMySqlConnectionString(config.Sql.Connections.DefaultConnection.Server, config.Sql.Connections.DefaultConnection.Port, config.Sql.Connections.DefaultConnection.Database, config.Sql.Connections.DefaultConnection.User, config.Sql.Connections.DefaultConnection.Password);
            }
            return null;
        }

        public static string GetConnectionString(string _Index)
        {
            if (config == null || config.Sql == null || config.Sql.Connections == null)
            {
                return null;
            }

            if (IsLocalMode)
            {
                return GetLocalString(_Index);
            }
            else
            {
                return GetMySqlString(_Index);
            }
        }

        private static string MakeMySqlConnectionString(string server, string port, string database, string username, string pass)
        {
            return string.Format("server={0}; port={1}; database={2}; uid={3}; password={4};", server, port, database, username, pass);
        }

        private static string GetLocalString(string _Index)
        {
            return string.Format("Data Source={0};Version=3;", config.Sql.LocalDbLocation);
        }
    }
}
