using System;
using System.Data.SQLite;
using System.Text;

namespace Mabinogi.SQL
{
    public class SQLiteUpdateCommand : SQLiteSimpleCommand
    {
        private const string CmdStr = "UPDATE `{0}` SET {1}";
        private const string WhereConst = " WHERE {0}";
        public SQLiteUpdateCommand(string command, SQLiteConnection conn, SQLiteTransaction trans = null)
            : base(command, conn, trans)
        {
        }

        public SQLiteUpdateCommand(string command, SimpleConnection connection, SimpleTransaction transaction = null)
            : base(command, connection, transaction)
        {
        }

        public SQLiteUpdateCommand(SQLiteConnection conn, string table, SQLiteTransaction trans = null)
            : base(CmdStr, table, conn, trans)
        {
        }

        public SQLiteUpdateCommand(SimpleConnection connection, string table, SimpleTransaction transaction = null)
            : base(CmdStr, table, connection, transaction)
        {
        }

        public override SimpleReader ExecuteReader()
        {
            throw new NotImplementedException("This function should never be called in an UpdateCommand class");
        }

        protected override string BuildCommandString()
        {
            string result;

            if (_table != null && _table != string.Empty)
            {
                // Build setting part
                var sb = new StringBuilder();
                foreach (var parameter in _set.Keys)
                    sb.AppendFormat("`{0}` = @{0}, ", parameter);



                string whereStr = string.Empty;

                // Build where part
                if (_complexWhere != null && _complexWhere != string.Empty)
                {
                    whereStr = _complexWhere;
                }
                else if (_where.Count > 0)
                {
                    var wb = new StringBuilder();

                    int i = 0;
                    foreach (var parameter in _where.Keys)
                    {
                        wb.AppendFormat("`{0}` = @{0} ", parameter);
                        i++;
                        if (i < _where.Count)
                            wb.Append("AND ");
                    }
                    whereStr = string.Format(WhereConst, wb.ToString().Trim(' '));
                }



                result = string.Format(CmdStr, _table, sb.ToString().Trim(' ', ',')) + whereStr;
            }
            else
            {
                // Build setting part
                var sb = new StringBuilder();
                foreach (var parameter in _set.Keys)
                    sb.AppendFormat("`{0}` = @{0}, ", parameter);


                string whereStr = string.Empty;

                // Build where part
                if (_complexWhere != null && _complexWhere != string.Empty)
                {
                    whereStr = _complexWhere;
                }
                else if (_where.Count > 0)
                {
                    var wb = new StringBuilder();

                    int i = 0;
                    foreach (var parameter in _where.Keys)
                    {
                        wb.AppendFormat("`{0}` = @{0} ", parameter);
                        i++;
                        if (i < _where.Count)
                            wb.Append("AND ");
                    }
                    whereStr = string.Format(WhereConst, wb.ToString().Trim(' '));
                }

                result = string.Format(Command.CommandText, sb.ToString().Trim(' ', ',')) + whereStr;

            }

            return result;
        }
    }
}
