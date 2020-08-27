using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class PetInventoryObjectBuilder
    {
        public static void BuildLargeItems(InventoryHash inventoryHash, Dictionary<long, Item> hashtable, SimpleReader _itemLargeReader)
        {
            Item item = null;
            if (_itemLargeReader != null && _itemLargeReader.HasRows)
            {
                while (_itemLargeReader.Read())
                {
                    item = ItemSqlBuilder.GetCharItem((byte)Item.StoredType.IstLarge, _itemLargeReader);
                    hashtable.Add(item.id, item);
                    inventoryHash.Add(item);
                }
            }
        }
        public static void BuildSmallItems(InventoryHash inventoryHash, Dictionary<long, Item> hashtable, SimpleReader _itemSmallReader)
        {
            Item item = null;
            if (_itemSmallReader != null && _itemSmallReader.HasRows)
            {
                while (_itemSmallReader.Read())
                {
                    item = ItemSqlBuilder.GetCharItem((byte)Item.StoredType.IstSmall, _itemSmallReader);
                    hashtable.Add(item.id, item);
                    inventoryHash.Add(item);
                }
            }
        }
        public static void BuildHugeItems(InventoryHash inventoryHash, Dictionary<long, Item> hashtable, SimpleReader _itemHugeReader)
        {
            Item item = null;
            if (_itemHugeReader != null && _itemHugeReader.HasRows)
            {
                while (_itemHugeReader.Read())
                {
                    item = ItemSqlBuilder.GetCharItem((byte)Item.StoredType.IstHuge, _itemHugeReader);
                    hashtable.Add(item.id, item);
                    inventoryHash.Add(item);
                }
            }
        }
        public static void BuildQuestItems(InventoryHash inventoryHash, Dictionary<long, Item> hashtable, SimpleReader _itemQuestReader)
        {
            Item item = null;
            if (_itemQuestReader != null && _itemQuestReader.HasRows)
            {
                while (_itemQuestReader.Read())
                {
                    item = ItemSqlBuilder.GetCharItem((byte)Item.StoredType.IstQuest, _itemQuestReader);
                    hashtable.Add(item.id, item);
                    inventoryHash.Add(item);
                }
            }
        }
    }
}
