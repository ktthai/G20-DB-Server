using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text.Json;

namespace Authenticator
{
	public class ServerConfiguration
	{
		//private const string m_LocalCharacterCardPath = "CharacterCard";

		//private const string m_LocalPetCardPath = "PetCard";

		//private const string m_LocalGiftPath = "Gift";

		//private const string m_LocalCouponPath = "Coupon";

		//private const string m_LocalFreeServicePath = "FreeService";

		public const string ConfigFile = "authconfig.json";

		private string m_LocalDbLocation = string.Empty;

		private int m_Mainport = 15001;

		private bool m_Test = false;

		private bool m_isSessionLogToConsole;

		private string m_FantasyLifeClubConnectionString = string.Empty;

		private string m_PremiumPackConnectionString = string.Empty;

		private string m_CouponConnectionString = string.Empty;

		private string m_CharacterCardConnectionString = string.Empty;
		private string m_PetCardConnectionString = string.Empty;

		private string m_GiftConnectionString = string.Empty;

		private string m_FreeServiceConnectionString = string.Empty;

		private string m_NexonIdMapConnectionString = string.Empty;

		private string m_PasswordChange2010ConnectionString = string.Empty;

		private string m_WebDBConnectionString = string.Empty;

		private string m_ItemShopConnectionString = string.Empty;

		private int m_GameNumber;

		private ListDictionary m_DomainMap;

		private ListDictionary m_ServerMap;

		private EventFantasylifeclub m_FantasyLifeClubEvent;

		private EventPremiumPack m_PremiumPackEvent;

		private Hashtable m_AuthorizedAccountTable = new Hashtable();

		private static ServerConfiguration m_Configuration = new ServerConfiguration();

		public static bool IsSessionLogToConsole
		{
			get
			{
				return m_Configuration.m_isSessionLogToConsole;
			}
			set
			{
				m_Configuration.m_isSessionLogToConsole = value;
			}
		}

		public static bool IsLocalTestMode => m_Configuration.m_Test;

		public static string LocalDbLocation => m_Configuration.m_LocalDbLocation;

		public static int ServicePort => m_Configuration.m_Mainport;

		public static string FantasyLifeClubConnectionString => m_Configuration.m_FantasyLifeClubConnectionString;

		public static string PremiumPackConnectionString => m_Configuration.m_PremiumPackConnectionString;

		public static string CouponConnectionString => m_Configuration.m_CouponConnectionString;

		public static string CharacterCardConnectionString => m_Configuration.m_CharacterCardConnectionString;

		public static string PetCardConnectionString => m_Configuration.m_PetCardConnectionString;

		public static string GiftConnectionString => m_Configuration.m_GiftConnectionString;

		public static string FreeServiceConnectionString => m_Configuration.m_FreeServiceConnectionString;

		public static string NexonIdMapConnectionString => m_Configuration.m_NexonIdMapConnectionString;

		public static string ItemshopConnectionString => m_Configuration.m_ItemShopConnectionString;

		public static string Passwordchange2010ConnectionString => m_Configuration.m_PasswordChange2010ConnectionString;

		public static string WebDBConnectionString => m_Configuration.m_WebDBConnectionString;

		public static int GameNumber => m_Configuration.m_GameNumber;

		//public static string CharacterCardPath => Directory.GetCurrentDirectory() + "\\CharacterCard";

		//public static string PetCardPath => Directory.GetCurrentDirectory() + "\\PetCard";

		//public static string GiftPath => Directory.GetCurrentDirectory() + "\\Gift";

		//public static string CouponPath => Directory.GetCurrentDirectory() + "\\Coupon";

		//public static string FreeServicePath => Directory.GetCurrentDirectory() + "\\FreeService";

		public static EventFantasylifeclub FantasyLifeClubEvent => m_Configuration.m_FantasyLifeClubEvent;

		public static EventPremiumPack PremiumPackEvent => m_Configuration.m_PremiumPackEvent;

		public static void Load()
		{
			//StreamReader streamReader = File.Exists("config.xml") ? new StreamReader("config.xml") : new StreamReader(File.Create("config.xml"));
			try
			{
				ConfigHeader configHeader;
				if (File.Exists(ConfigFile))
				{
					string configStr = File.ReadAllText(ConfigFile);
					configHeader = JsonSerializer.Deserialize<ConfigHeader>(configStr);
				}
				else
				{
					configHeader = new ConfigHeader();
					File.WriteAllText(ConfigFile, JsonSerializer.Serialize(configHeader, new JsonSerializerOptions() { WriteIndented = true }));
				}
				if (configHeader != null)
				{
					m_Configuration.m_Mainport = configHeader.port;
					m_Configuration.m_LocalDbLocation = configHeader.sql.LocalDbLocation;
					if (configHeader.test != null)
					{
						m_Configuration.m_Test = configHeader.test.enable;
						m_Configuration.m_AuthorizedAccountTable.Clear();
						if (configHeader.test.authorizedlist != null)
						{
							lock (m_Configuration.m_AuthorizedAccountTable.SyncRoot)
							{
								TestAuthorized[] authorizedlist = configHeader.test.authorizedlist;
								foreach (TestAuthorized testAuthorized in authorizedlist)
								{
									if (m_Configuration.m_AuthorizedAccountTable.ContainsKey(testAuthorized.id))
									{
										ExceptionMonitor.ExceptionRaised(new Exception("Test user id[" + testAuthorized.id + "] already exists"));
										if (Console.Out != null)
										{
											Console.WriteLine("Test user id[" + testAuthorized.id + "] already exists");
										}
									}
									else
									{
										m_Configuration.m_AuthorizedAccountTable.Add(testAuthorized.id, testAuthorized);
									}
								}
							}
						}
					}
					if (configHeader.sql != null && configHeader.sql.connections != null)
					{
						m_Configuration.m_FantasyLifeClubConnectionString = MakeConnectionString(configHeader.sql.connections.fantasylifeclub);
						m_Configuration.m_PremiumPackConnectionString = MakeConnectionString(configHeader.sql.connections.premiumpack);
						m_Configuration.m_CouponConnectionString = MakeConnectionString(configHeader.sql.connections.pceventcoupon);
						m_Configuration.m_CharacterCardConnectionString = MakeConnectionString(configHeader.sql.connections.CharacterCard);
						m_Configuration.m_PetCardConnectionString = MakeConnectionString(configHeader.sql.connections.PetCard);
						m_Configuration.m_GiftConnectionString = MakeConnectionString(configHeader.sql.connections.gift);
						if (configHeader.sql.connections.freeservice != null)
						{
							m_Configuration.m_FreeServiceConnectionString = MakeConnectionString(configHeader.sql.connections.freeservice);
						}
					}
					if (configHeader.Event != null)
					{
						if (configHeader.Event.fantasylifeclub != null)
						{
							if (configHeader.Event.fantasylifeclub.eventStart == DateTime.MinValue || configHeader.Event.fantasylifeclub.eventEnd == DateTime.MinValue)
							{
								throw new Exception("FantasylifeClub Event Config is not valid.");
							}
							m_Configuration.m_FantasyLifeClubEvent = configHeader.Event.fantasylifeclub;
							if (Console.Out != null)
							{
								Console.WriteLine("Fantasy life club event on during {0} ~ {1}", configHeader.Event.fantasylifeclub.eventStart, configHeader.Event.fantasylifeclub.eventEnd);
							}
						}
						if (configHeader.Event.premiumpack != null)
						{
							if (configHeader.Event.premiumpack.eventStart == DateTime.MinValue || configHeader.Event.premiumpack.eventEnd == DateTime.MinValue)
							{
								throw new Exception("Premium pack event config is not valid.");
							}
							m_Configuration.m_PremiumPackEvent = configHeader.Event.premiumpack;
							if (Console.Out != null)
							{
								Console.WriteLine("Premium pack event on during {0} ~ {1}", configHeader.Event.premiumpack.eventStart, configHeader.Event.premiumpack.eventEnd);
							}
						}
					}
					if (configHeader.itemshop != null && configHeader.itemshop.sql != null && configHeader.itemshop.UsingItemShop)
					{
						m_Configuration.m_GameNumber = configHeader.itemshop.gameNumber;
						m_Configuration.m_ItemShopConnectionString = MakeConnectionString(configHeader.itemshop.sql);
						m_Configuration.m_DomainMap = new ListDictionary();
						m_Configuration.m_ServerMap = new ListDictionary();
						if (configHeader.itemshop.domains != null && configHeader.itemshop.domains.Length != 0)
						{
							Domain[] domains = configHeader.itemshop.domains;
							foreach (Domain domain in domains)
							{
								if (m_Configuration.m_DomainMap.Contains(domain.domainNumber) || m_Configuration.m_ServerMap.Contains(domain.serverName))
								{
									ExceptionMonitor.ExceptionRaised(new Exception("Itemshop Domain Number [" + domain.domainNumber + "] or [" + domain.serverName + "] already exists"));
									if (Console.Out != null)
									{
										Console.WriteLine("Itemshop Domain Number [" + domain.domainNumber + "] or [" + domain.serverName + "] already exists");
									}
								}
								m_Configuration.m_DomainMap.Add(domain.domainNumber, domain.serverName);
								m_Configuration.m_ServerMap.Add(domain.serverName, domain.domainNumber);
							}
						}
						if (m_Configuration.m_ItemShopConnectionString == string.Empty || m_Configuration.m_DomainMap.Count == 0 || m_Configuration.m_ServerMap.Count == 0)
						{
							throw new Exception("Itemshop configuration is not valid.");
						}
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
			}
			finally
			{
				//streamReader.Close();
			}
		}

		public static int GetDomainNumber(string _serverName)
		{
			if (!m_Configuration.m_ServerMap.Contains(_serverName))
			{
				throw new Exception("No Domain has Server Name[" + _serverName + "]");
			}
			return (int)m_Configuration.m_ServerMap[_serverName];
		}

		public static string GetServerName(int _domain)
		{
			if (!m_Configuration.m_DomainMap.Contains(_domain))
			{
				return string.Empty;
			}
			return (string)m_Configuration.m_DomainMap[_domain];
		}

		public static TestAuthorized QueryTestInfo(string _id)
		{
			lock (m_Configuration.m_AuthorizedAccountTable.SyncRoot)
			{
				return (TestAuthorized)m_Configuration.m_AuthorizedAccountTable[_id];
			}
		}

		private static string MakeConnectionString(Connection _connection)
		{
            if (IsLocalTestMode)
            {
                return $"Data Source={LocalDbLocation};Version=3;";
            }

            if (_connection != null)
			{
				
				return $"server={_connection.server}; port={_connection.port}; database={_connection.database}; uid={_connection.user}; password={_connection.password};";
				//if (_connection.user == null || _connection.user == string.Empty)
				//{
				//	return $"Persist Security Info=True;Integrated Security=true;Initial Catalog={_connection.database};server={_connection.server};";
				//}
				//return $"server={_connection.server}; database={_connection.database}; user id={_connection.user}; pwd={_connection.password};";
			}
			return string.Empty;
		}
    }
}
