using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class HouseInventoryObjectBuilder
    {
        public static void BuildItemLarge(SimpleReader _itemLarge, HouseInventory houseInventory)
        {
            if (_itemLarge == null)
            {
                throw new Exception("집 아이템 테이블을 얻지 못하였습니다.");
            }

            if (_itemLarge.HasRows)
            {
                while (_itemLarge.Read())
                {
                    houseInventory.Items.Add(ItemSqlBuilder.GetHouseItem((byte)Item.StoredType.IstLarge, _itemLarge));
                }
            }
        }

        public static void BuildItemSmall(SimpleReader _itemSmall, HouseInventory houseInventory)
        {
            if (_itemSmall.HasRows)
            {
                while (_itemSmall.Read())
                {
                    houseInventory.Items.Add(ItemSqlBuilder.GetHouseItem((byte)Item.StoredType.IstSmall, _itemSmall));
                }
            }
        }

        public static void BuildItemHuge(SimpleReader _itemHuge, HouseInventory houseInventory)
        {
            if (_itemHuge.HasRows)
            {
                while (_itemHuge.Read())
                {
                    houseInventory.Items.Add(ItemSqlBuilder.GetHouseItem((byte)Item.StoredType.IstHuge, _itemHuge));
                }
            }
        }

        public static void BuildItemQuest(SimpleReader _itemQuest, HouseInventory houseInventory)
        {
            if (_itemQuest.HasRows)
            {
                while (_itemQuest.Read())
                {
                    houseInventory.Items.Add(ItemSqlBuilder.GetHouseItem((byte)Item.StoredType.IstQuest, _itemQuest));
                }
            }
        }
    }
}
