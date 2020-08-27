using System;
using System.Text;
using MySql.Data.MySqlClient;

namespace Mabinogi.SQL
{
    public class MySqlInsertCommand : MySqlSimpleCommand
    {
        private const string CmdStr = "INSERT INTO `{0}` {1}";

        public MySqlInsertCommand(string command, MySqlConnection conn, MySqlTransaction transaction = null)
            : base(command, conn, transaction)
        {
        }

        public MySqlInsertCommand(MySqlConnection conn, string table, MySqlTransaction transaction = null)
            : base(CmdStr, table, conn, transaction)
        {
        }

        public MySqlInsertCommand(string command, SimpleConnection connection, SimpleTransaction transaction = null)
            : base(command, connection, transaction)
        {
        }

        public MySqlInsertCommand(SimpleConnection connection, string table, SimpleTransaction transaction = null)
            : base(CmdStr, table, connection, transaction)
        {
        }

        protected override string BuildCommandString()
        {
            // Build values part
            var sb1 = new StringBuilder();
            var sb2 = new StringBuilder();
            foreach (var parameter in _set.Keys)
            {
                sb1.AppendFormat("`{0}`, ", parameter);
                sb2.AppendFormat("@{0}, ", parameter);
            }

            // Add values part
            var values = "(" + (sb1.ToString().Trim(' ', ',')) + ") VALUES (" + (sb2.ToString().Trim(' ', ',')) + ")";

            if (_table != null && _table != string.Empty)
                return string.Format(Command.CommandText, _table, values);
            else
                return string.Format(Command.CommandText, values);
        }

        public override SimpleReader ExecuteReader()
        {
            throw new NotImplementedException("This function should never be called in an InsertCommand class");
        }
    }
}
