using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class InventoryObjectBuilder
	{
		public static void BuildLargeItems(SimpleReader reader, CharacterInfo characterInfo, ref InventoryHash inventoryHash)
		{
			BuildItems(reader, characterInfo, ref inventoryHash, Item.StoredType.IstLarge);
		}

        public static void BuildHugeItems(SimpleReader reader, CharacterInfo characterInfo, ref InventoryHash inventoryHash)
        {
            BuildItems(reader, characterInfo, ref inventoryHash, Item.StoredType.IstHuge);
        }

        public static void BuildSmallItems(SimpleReader reader, CharacterInfo characterInfo, ref InventoryHash inventoryHash)
        {
            BuildItems(reader, characterInfo, ref inventoryHash, Item.StoredType.IstSmall);
        }

        public static void BuildQuestItems(SimpleReader reader, CharacterInfo characterInfo, ref InventoryHash inventoryHash)
        {
            BuildItems(reader, characterInfo, ref inventoryHash, Item.StoredType.IstQuest);
        }

        public static void BuildEgoItems(SimpleReader reader, CharacterInfo characterInfo, ref InventoryHash inventoryHash)
        {
            BuildItems(reader, characterInfo, ref inventoryHash, Item.StoredType.IstEgo);
        }

        private static void BuildItems(SimpleReader reader, CharacterInfo characterInfo, ref InventoryHash inventoryHash, Item.StoredType storedType)
        {
            if (inventoryHash == null)
                inventoryHash = new InventoryHash(characterInfo.id);

			if (characterInfo.inventory == null)
				characterInfo.inventory = new Dictionary<long, Item>();

            if (reader.HasRows)
            {
                Item item = null;
                while (reader.Read())
                {
                    item = ItemSqlBuilder.GetCharItem((byte)storedType, reader);
                    characterInfo.inventory.Add(item.id, item);
                    inventoryHash.Add(item);
                }
            }
        }
	}
}
