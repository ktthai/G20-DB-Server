using MySql.Data.MySqlClient;

namespace Mabinogi.SQL
{
    // TODO: Create SimpleException wrapper
    public class MySqlSimpleTransaction : SimpleTransaction
    {
        internal MySqlTransaction Transaction;

        public MySqlSimpleTransaction(MySqlTransaction transaction)
        {
            Transaction = transaction;
        }

        public void Commit()
        {
            Transaction.Commit();
        }

        public void Dispose()
        {
            Transaction.Dispose();
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }
    }
}
