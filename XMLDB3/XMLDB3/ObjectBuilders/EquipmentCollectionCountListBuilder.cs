using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class EquipmentCollectionCountListBuilder
    {
        public static CollectionCountList Build(SimpleReader reader)
        {
            if (reader == null)
            {
                throw new Exception("DB 정보를 가져오지 못하였습니다.");
            }
            CollectionCountList collectionCountList = new CollectionCountList();

            if (reader.HasRows)
            {
                List<CollectionCount> list = new List<CollectionCount>();
                CollectionCount count;
                while (reader.Read())
                {
                    count = new CollectionCount();
                    count.itemType = reader.GetInt32(Mabinogi.SQL.Columns.Item.PosX);
                    count.collectionId = reader.GetInt32(Mabinogi.SQL.Columns.Item.PosY);
                    count.count = reader.GetInt32(Mabinogi.SQL.Columns.Reference.Count);
                    list.Add(count);
                }

                collectionCountList.collectionCounts = list;
            }

            return collectionCountList;
        }
    }
}
