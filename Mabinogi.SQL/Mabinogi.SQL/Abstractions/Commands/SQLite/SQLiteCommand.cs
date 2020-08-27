// Entire class system and idea lifted from Aura, because making the queries by hand is taking forever, and is pretty inefficient

using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System;
using System.Linq;

namespace Mabinogi.SQL
{
    public abstract class SQLiteSimpleCommand : SimpleCommand
    {
        internal SQLiteCommand Command;
        internal SQLiteTransaction Transaction;
        

        protected SQLiteSimpleCommand(string command, SQLiteConnection conn, SQLiteTransaction trans = null) : base()
        {
            Command = new SQLiteCommand(command, conn, trans);
            Transaction = trans;
        }

        protected SQLiteSimpleCommand(string command, string table, SQLiteConnection conn, SQLiteTransaction trans = null)
            : base()
        {
            _table = table;
            Command = new SQLiteCommand(command, conn, trans);
            Transaction = trans;
        }

        protected SQLiteSimpleCommand(string command, SimpleConnection connection, SimpleTransaction transaction = null) : base()
        {
            Init(command, connection, transaction);
        }

        protected SQLiteSimpleCommand(string command, string table, SimpleConnection connection, SimpleTransaction transaction = null)
        {
            _table = table;
            Init(command, connection, transaction);
        }

        private void Init(string command, SimpleConnection connection, SimpleTransaction transaction)
        {
            SQLiteSimpleConnection conn = (SQLiteSimpleConnection)connection;

            if (transaction != null)
            {
                Transaction = ((SQLiteSimpleTransaction)transaction).Transaction;
                Command = new SQLiteCommand(command, conn.Connection, Transaction);
            }
            else
            {
                Command = new SQLiteCommand(command, conn.Connection);
            }
        }

        public override int Execute()
        {
            BuildQuery();

            try
            {
                // Run
                return Command.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Command type: " + GetType().ToString() + " Command: " + Command.CommandText);
                throw new SimpleSqlException(ex.Message, ex.ErrorCode, ex);
            }
        }


        public override SimpleReader ExecuteReader()
        {
            BuildQuery();

            try
            {
                return new SQLiteSimpleReader(Command.ExecuteReader());
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Command type: " + GetType().ToString() + " Command: " + Command.CommandText);
                throw new SimpleSqlException(ex.Message, ex.ErrorCode, ex);
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
