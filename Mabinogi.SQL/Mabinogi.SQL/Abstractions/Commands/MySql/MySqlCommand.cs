// Entire class system and idea lifted from Aura, because making the queries by hand is taking forever, and is pretty inefficient

using MySql.Data.MySqlClient;
using System;

namespace Mabinogi.SQL
{
    public abstract class MySqlSimpleCommand : SimpleCommand
    {
        internal MySqlCommand Command;
        internal MySqlTransaction Transaction;


        protected MySqlSimpleCommand(string command, string table, MySqlConnection conn, MySqlTransaction trans = null)
            : base()
        {
            _table = table;
            Command = new MySqlCommand(command, conn, trans);
            Transaction = trans;
        }


        protected MySqlSimpleCommand(string command, MySqlConnection conn, MySqlTransaction trans = null)
            : base()
        {
            Command = new MySqlCommand(command, conn, trans);
            Transaction = trans;
        }

        protected MySqlSimpleCommand(string command, string table, SimpleConnection connection, SimpleTransaction transaction = null)
            : base()
        {
            _table = table;
            Init(command, connection, transaction);
        }

        protected MySqlSimpleCommand(string command, SimpleConnection connection, SimpleTransaction transaction = null)
            : base()
        {
            Init(command, connection, transaction);
        }

        private void Init(string command, SimpleConnection connection, SimpleTransaction transaction)
        {
            MySqlSimpleConnection conn = (MySqlSimpleConnection)connection;

            if (transaction != null)
            {
                Transaction = ((MySqlSimpleTransaction)transaction).Transaction;
                Command = new MySqlCommand(command, conn.Connection, Transaction);
            }
            else
            {
                Command = new MySqlCommand(command, conn.Connection);
            }
        }


        public override int Execute()
        {
            BuildQuery();
            // Run
            try
            {
                return Command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Command type: " + GetType().ToString() + " Command: " + Command.CommandText);
                throw new SimpleSqlException(ex.Message, (int)ex.Code, ex);
            }
        }

        public override SimpleReader ExecuteReader()
        {
            BuildQuery();

            try
            {
                return new MySqlSimpleReader(Command.ExecuteReader());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Command type: " + GetType().ToString() + " Command: " + Command.CommandText);
                throw new SimpleSqlException(ex.Message, (int)ex.Code, ex);
            }
        }

        protected virtual void BuildQuery()
        {
            // Add setting part
            Command.CommandText = BuildCommandString();

            // Add parameters
            foreach (var parameter in _set)
                Command.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);

            foreach (var parameter in _where)
                Command.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
        }

        public override void AddParameter(string name, object value, bool addEscapeChar = true)
        {
            Command.Parameters.AddWithValue((addEscapeChar ? "@" + name : name), value);
        }

        public override void Dispose()
        {
            Command.Dispose();
        }
    }
}
