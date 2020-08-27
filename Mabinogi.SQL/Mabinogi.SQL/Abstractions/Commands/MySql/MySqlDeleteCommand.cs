using System;
using System.Text;
using MySql.Data.MySqlClient;

namespace Mabinogi.SQL
{
    public class MySqlDeleteCommand : MySqlSimpleCommand
    {
        private const string CmdStr = "DELETE FROM `{0}`";
        private const string WhereConst = " WHERE {0}";
        public MySqlDeleteCommand(string table, MySqlConnection conn, MySqlTransaction trans = null)
            : base(CmdStr, table, conn, trans)
        {
        }

        public MySqlDeleteCommand(string table, SimpleConnection connection, SimpleTransaction transaction = null)
            : base(CmdStr, table, connection, transaction)
        {
        }

        public override SimpleReader ExecuteReader()
        {
            throw new NotImplementedException("This function should never be called in a DeleteCommand class");
        }

        protected override string BuildCommandString()
        {
            string result;

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
            return result;
        }
    }
}
