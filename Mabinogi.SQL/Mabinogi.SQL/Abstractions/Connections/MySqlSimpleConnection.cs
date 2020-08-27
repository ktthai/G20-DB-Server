using MySql.Data.MySqlClient;

namespace Mabinogi.SQL
{
    public class MySqlSimpleConnection : SimpleConnection
    {
        internal MySqlConnection Connection;

        internal MySqlSimpleConnection(MySqlConnection connection)
        {
            Connection = connection;
            connection.Open();
        }

        public MySqlSimpleConnection(string connectionStr)
        {
            Connection = new MySqlConnection(connectionStr);
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        public SimpleTransaction BeginTransaction()
        {
            return new MySqlSimpleTransaction(Connection.BeginTransaction());
        }

        public SimpleCommand GetInsertCommand(string command)
        {
            return new MySqlInsertCommand(command, this);
        }

        public SimpleCommand GetInsertCommand(string command, SimpleTransaction transaction = null)
        {
            return new MySqlInsertCommand(command, this, transaction);
        }

        public SimpleCommand GetDefaultDeleteCommand(string table)
        {
            return new MySqlDeleteCommand(table, this);
        }

        public SimpleCommand GetDefaultDeleteCommand(string table, SimpleTransaction transaction = null)
        {
            return new MySqlDeleteCommand(table, this, transaction);
        }

        public SimpleCommand GetDefaultInsertCommand(string table)
        {
            return new MySqlInsertCommand(this, table);
        }

        public SimpleCommand GetDefaultInsertCommand(string table, SimpleTransaction transaction = null)
        {
            return new MySqlInsertCommand(this, table, transaction);
        }

        public SimpleCommand GetUpdateCommand(string command)
        {
            return new MySqlUpdateCommand(command, this);
        }

        public SimpleCommand GetUpdateCommand(string command, SimpleTransaction transaction = null)
        {
            return new MySqlUpdateCommand(command, this, transaction);
        }

        public SimpleCommand GetDefaultUpdateCommand(string table)
        {
            return new MySqlUpdateCommand(this, table);
        }

        public SimpleCommand GetDefaultUpdateCommand(string table, SimpleTransaction transaction = null)
        {
            return new MySqlUpdateCommand(this, table, transaction);
        }

        public SimpleCommand GetSelectCommand(string command)
        {
            return new MySqlSelectCommand(command, this);
        }

        public SimpleCommand GetSelectCommand(string command, SimpleTransaction transaction = null)
        {
            return new MySqlSelectCommand(command, this, transaction);
        }

        public SimpleCommand GetDefaultSelectCommand(string table)
        {
            return new MySqlSelectCommand(this, table);
        }

        public SimpleCommand GetDefaultSelectCommand(string table, SimpleTransaction transaction = null)
        {
            return new MySqlSelectCommand(this, table, transaction);
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
