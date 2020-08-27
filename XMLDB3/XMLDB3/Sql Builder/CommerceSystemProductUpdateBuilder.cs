using Mabinogi.SQL;

namespace XMLDB3
{
    public class CommerceSystemProductUpdateBuilder
    {
        public static void Build(CommerceSystemProduct _product, SimpleConnection conn, SimpleTransaction transaction)
        {
            int rows = 0;
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.CommerceProduct, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.CommerceProduct.ProductId, _product.id);
                cmd.Set(Mabinogi.SQL.Columns.CommerceProduct.ProductCount, _product.count);
                cmd.Set(Mabinogi.SQL.Columns.CommerceProduct.ProductPrice, _product.price);
                rows = cmd.Execute();
            }

            if (rows == 0)
            {
                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.CommerceProduct, transaction))
                {
                    cmd.Set(Mabinogi.SQL.Columns.CommerceProduct.ProductId, _product.id);
                    cmd.Set(Mabinogi.SQL.Columns.CommerceProduct.ProductCount, _product.count);
                    cmd.Set(Mabinogi.SQL.Columns.CommerceProduct.ProductPrice, _product.price);
                    cmd.Execute();
                }
            }
        }
    }
}
