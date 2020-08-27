using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class InventoryUpdateBuilder
	{
		public static void Build(long _id, Dictionary<long, Item> _new, Dictionary<long, Item> _cache, bool _forceUpdate, SimpleConnection conn, SimpleTransaction transaction, out string strToHash)
		{
			InventoryHash inventoryHash = new InventoryHash(_id);
			if (_new != null)
			{
				if (_cache != null)
				{
					foreach (Item value in _new.Values)
					{
						Item item2;
						if (_cache.TryGetValue(value.id, out item2))
						{
							ItemSqlBuilder.UpdateItem(_id, value, item2, conn, transaction);
							_cache.Remove(item2.id);
						}
						else
                        {
							ItemSqlBuilder.SelfUpdateItem(_id, value, _forceUpdate, conn, transaction);
						}							
						inventoryHash.Add(value);
					}
				}
				else
				{
					foreach (Item value2 in _new.Values)
					{
						ItemSqlBuilder.SelfUpdateItem(_id, value2, _forceUpdate, conn, transaction);
						inventoryHash.Add(value2);
					}
				}
			}
			if (_cache != null)
			{
				foreach (Item value3 in _cache.Values)
				{
					ItemSqlBuilder.DeleteItem(_id, value3.id, value3.storedtype, conn, transaction);
				}
			}

			strToHash = inventoryHash.ToString();
		}
	}
}
