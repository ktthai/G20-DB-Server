using System.Data.SQLite;

namespace Mabinogi.SQL
{
    public class SQLiteSimpleTransaction : SimpleTransaction
    {
        internal SQLiteTransaction Transaction;

        public SQLiteSimpleTransaction(SQLiteTransaction transaction)
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
