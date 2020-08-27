using System;
using System.Collections;

namespace XMLDB3
{
	public class ObjectCache
	{
		private static BasicCache characterCache;

		private static BasicCache bankCache;

		private static ChronicleCache chronicleCache;

		private static BasicCache privateFarmCache;

		private static BasicCache equipmentCollectionCache;

		public static BasicCache Character => characterCache;

		public static BasicCache Bank => bankCache;

		public static ChronicleCache Chronicle
		{
			get
			{
				if (chronicleCache == null)
				{
					throw new Exception("Chronicle cache 초기화 안됨");
				}
				return chronicleCache;
			}
		}

		public static BasicCache PrivateFarm => privateFarmCache;

		public static BasicCache EquipmentCollection => equipmentCollectionCache;

		private ObjectCache()
		{
		}

		static ObjectCache()
		{
			characterCache = new BasicCache(ConfigManager.CharacterCacheSize, "CharacterCache");
			bankCache = new BasicCache(ConfigManager.BankCacheSize, "BankCache");
			privateFarmCache = new BasicCache(ConfigManager.PrivateFarmCacheSize, "PrivateFarmCache");
			equipmentCollectionCache = new BasicCache(ConfigManager.EquipmentCollectionCacheSize, "EquipmentCollectionCache");
			chronicleCache = null;
		}

		public static void Init()
		{
		}

		public static void InitChronicleCache(string _serverName)
		{
			chronicleCache = new ChronicleCache(_serverName);
		}

		public static void InitChronicleCache(string _serverName, IDictionary _dic)
		{
			chronicleCache = new ChronicleCache(_serverName, _dic);
		}

		public static void DeleteChronicleCache()
		{
			chronicleCache = null;
		}
	}
}
