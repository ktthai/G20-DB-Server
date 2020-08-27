using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class CommercePurchasedProductUpdateBuilder
	{
		public static void Build(long _charID, CommercePurchasedProducts _newProduct, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_newProduct == null)
			{
				throw new ArgumentException("무역정보 데이터가 없습니다.", "_new");
			}

			foreach (CommercePurchasedProduct value in _newProduct.productTable.Values)
			{
				if (value.count == 0)
				{
					// PROCEDURE: dbo.DeleteCommercePurchasedProduct
					using(var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CommercePurchasedProduct, transaction))
                    {
						cmd.Where(Mabinogi.SQL.Columns.CommercePurchasedProduct.CharId, _charID);
						cmd.Where(Mabinogi.SQL.Columns.CommercePurchasedProduct.ClassId, value.id);
						cmd.Execute();
					}
				}
				else
				{
					int rows = 0;
					using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CommercePurchasedProduct, transaction))
                    {  
						cmd.Where(Mabinogi.SQL.Columns.CommercePurchasedProduct.CharId, _charID);
						cmd.Where(Mabinogi.SQL.Columns.CommercePurchasedProduct.ClassId, value.id);
						cmd.Set(Mabinogi.SQL.Columns.CommercePurchasedProduct.Price, value.price);
						cmd.Set(Mabinogi.SQL.Columns.CommercePurchasedProduct.Bundle, value.count);
						rows = cmd.Execute();
                    }

					if (rows == 0)
					{
						using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CommercePurchasedProduct, transaction))
						{
                            cmd.Set(Mabinogi.SQL.Columns.CommercePurchasedProduct.CharId, _charID);
                            cmd.Set(Mabinogi.SQL.Columns.CommercePurchasedProduct.ClassId, value.id);
                            cmd.Set(Mabinogi.SQL.Columns.CommercePurchasedProduct.Price, value.price);
                            cmd.Set(Mabinogi.SQL.Columns.CommercePurchasedProduct.Bundle, value.count);
                            cmd.Execute();
                        }
					}
				}
			}
		}
	}
}
