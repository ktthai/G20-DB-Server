using System.Data.SQLite;

namespace Mabinogi.SQL
{
    public class SQLiteSimpleConnection : SimpleConnection
    {
        internal SQLiteConnection Connection;

        internal SQLiteSimpleConnection(SQLiteConnection connection)
        {
            Connection = connection;
            connection.Open();
        }

        public SQLiteSimpleConnection(string connectionStr)
        {
            Connection = new SQLiteConnection(connectionStr);
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        public SimpleTransaction BeginTransaction()
        {
            return new SQLiteSimpleTransaction(Connection.BeginTransaction());
        }

        public SimpleCommand GetInsertCommand(string command)
        {
            return new SQLiteInsertCommand(command, this);
        }

        public SimpleCommand GetInsertCommand(string command, SimpleTransaction transaction = null)
        {
            return new SQLiteInsertCommand(command, this, transaction);
        }

        public SimpleCommand GetDefaultDeleteCommand(string table)
        {
            return new SQLiteDeleteCommand(table, this);
        }

        public SimpleCommand GetDefaultDeleteCommand(string table, SimpleTransaction transaction = null)
        {
            return new SQLiteDeleteCommand(table, this, transaction);
        }

        public SimpleCommand GetDefaultInsertCommand(string table)
        {
            return new SQLiteInsertCommand(this, table);
        }

        public SimpleCommand GetDefaultInsertCommand(string table, SimpleTransaction transaction = null)
        {
            return new SQLiteInsertCommand(this, table, transaction);
        }

        public SimpleCommand GetUpdateCommand(string command)
        {
            return new SQLiteUpdateCommand(command, this);
        }

        public SimpleCommand GetUpdateCommand(string command, SimpleTransaction transaction = null)
        {
            return new SQLiteUpdateCommand(command, this, transaction);
        }

        public SimpleCommand GetDefaultUpdateCommand(string table)
        {
            return new SQLiteUpdateCommand(this, table);
        }

        public SimpleCommand GetDefaultUpdateCommand(string table, SimpleTransaction transaction = null)
        {
            return new SQLiteUpdateCommand(this, table, transaction);
        }

        public SimpleCommand GetSelectCommand(string command)
        {
            return new SQLiteSelectCommand(command, this);
        }

        public SimpleCommand GetSelectCommand(string command, SimpleTransaction transaction = null)
        {
            return new SQLiteSelectCommand(command, this, transaction);
        }

        public SimpleCommand GetDefaultSelectCommand(string table)
        {
            return new SQLiteSelectCommand(this, table);
        }

        public SimpleCommand GetDefaultSelectCommand(string table, SimpleTransaction transaction = null)
        {
            return new SQLiteSelectCommand(this, table, transaction);
        }

        public SimpleCommand GetDeleteCommand(string command)
        {
            throw new System.NotImplementedException();
        }

        public SimpleCommand GetDeleteCommand(string command, SimpleTransaction transaction = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
