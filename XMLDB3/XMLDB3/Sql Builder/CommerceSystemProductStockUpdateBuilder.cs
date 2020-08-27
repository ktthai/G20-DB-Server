using Mabinogi.SQL;

namespace XMLDB3
{
    public class CommerceSystemProductStockUpdateBuilder
    {
        public static void Build(int _idProduct, COStockInfo _StockInfo, SimpleConnection conn, SimpleTransaction transaction)
        {
            int rows = 0;
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CommerceProductStock, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.CommerceProductStock.ProductId, _idProduct);
                cmd.Where(Mabinogi.SQL.Columns.CommerceProductStock.ProductSellPostId, _StockInfo.idPost);
                cmd.Set(Mabinogi.SQL.Columns.CommerceProductStock.ProductStock, _StockInfo.currentStock);
                cmd.Set(Mabinogi.SQL.Columns.CommerceProductStock.ProductStockPrice, _StockInfo.price);
                rows = cmd.Execute();
            }

            if (rows == 0)
            {
                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CommerceProductStock, transaction))
                {
                    cmd.Set(Mabinogi.SQL.Columns.CommerceProductStock.ProductId, _idProduct);
                    cmd.Set(Mabinogi.SQL.Columns.CommerceProductStock.ProductSellPostId, _StockInfo.idPost);
                    cmd.Set(Mabinogi.SQL.Columns.CommerceProductStock.ProductStock, _StockInfo.currentStock);
                    cmd.Set(Mabinogi.SQL.Columns.CommerceProductStock.ProductStockPrice, _StockInfo.price);
                    cmd.Execute();
                }
            }
        }
    }
}

