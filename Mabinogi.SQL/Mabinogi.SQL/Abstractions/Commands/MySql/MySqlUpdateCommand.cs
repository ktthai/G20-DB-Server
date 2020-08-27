using System;
using System.Text;
using MySql.Data.MySqlClient;

namespace Mabinogi.SQL
{
    public class MySqlUpdateCommand : MySqlSimpleCommand
    {
        private const string CmdStr = "UPDATE `{0}` SET {1}";
        private const string WhereConst = " WHERE {0}";

        public MySqlUpdateCommand(MySqlConnection conn, string table, MySqlTransaction trans = null)
            : base(CmdStr, table, conn, trans)
        {
        }

        public MySqlUpdateCommand(string command, MySqlConnection conn, MySqlTransaction trans = null)
            : base(command, conn, trans)
        {
        }

        public MySqlUpdateCommand(string command, SimpleConnection connection, SimpleTransaction transaction = null)
            : base(command, connection, transaction)
        {
        }

        public MySqlUpdateCommand(SimpleConnection conn, string table, SimpleTransaction trans = null)
            : base(CmdStr, table, conn, trans)
        {
        }

        public override SimpleReader ExecuteReader()
        {
            throw new NotImplementedException("This shouldn't be called in an UpdateCommand");
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

            //Console.WriteLine("Update Command: " + result);
            return result;
        }
    }
}
