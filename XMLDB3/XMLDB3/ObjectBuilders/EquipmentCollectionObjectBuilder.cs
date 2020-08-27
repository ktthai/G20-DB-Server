using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class EquipmentCollectionObjectBuilder
	{
		public static EquipmentCollection Build(SimpleReader reader)
		{
			if (reader.HasRows != true)
			{
				throw new Exception("의상 수집 시스템 테이블이 없습니다.");
			}
			EquipmentCollection equipmentCollection = new EquipmentCollection();

			while (reader.Read())
			{
				equipmentCollection.item.Add(ItemSqlBuilder.GetCollectionItem(Item.StoredType.IstLarge, reader));
			}
			return equipmentCollection;
		}
	}
}
