using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class InventoryCreateBuilder
	{
		public static void Build(long _id, Dictionary<long, Item> _inventory, out string _strToHash, SimpleConnection conn, SimpleTransaction transaction)
		{
			InventoryHash inventoryHash = new InventoryHash(_id);
			if (_inventory != null)
			{
				foreach (Item value in _inventory.Values)
				{
					if (value.storedtype > 0 && value.storedtype < 6)
					{
						ItemSqlBuilder.SelfUpdateItem(_id, value, _bForceUpdate: true, conn, transaction);
						inventoryHash.Add(value);
					}
				}
				_strToHash = inventoryHash.ToString();
			}
			_strToHash = inventoryHash.ToString();
		}
	}
}
