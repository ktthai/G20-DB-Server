using System;
using System.Data.SQLite;
using System.Text;

namespace Mabinogi.SQL
{
    public class SQLiteInsertCommand : SQLiteSimpleCommand
    {
        private const string CmdStr = "INSERT INTO `{0}` {1}";

        public SQLiteInsertCommand(string command, SQLiteConnection conn, SQLiteTransaction transaction = null)
            : base(command, conn, transaction)
        {
        }

        public SQLiteInsertCommand(string command, SimpleConnection connection, SimpleTransaction transaction = null)
            : base(command, connection, transaction)
        {
        }

        public SQLiteInsertCommand(SQLiteConnection conn, string table, SQLiteTransaction trans = null)
            : base(CmdStr, table, conn, trans)
        {
        }

        public SQLiteInsertCommand(SimpleConnection connection, string table, SimpleTransaction transaction = null)
            : base(CmdStr, table, connection, transaction)
        {
        }

        protected override string BuildCommandString()
        {
            string result;
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

            if (_table != null && _table != "")
                result = string.Format(Command.CommandText, _table, values);
            else
                result = string.Format(Command.CommandText, values);

            return result;
        }

        public override SimpleReader ExecuteReader()
        {
            throw new NotImplementedException("This function should never be called in an InsertCommand class");
        }
    }
}
