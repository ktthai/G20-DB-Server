using System;
using System.Text;
using MySql.Data.MySqlClient;

namespace Mabinogi.SQL
{
    public class MySqlSelectCommand : MySqlSimpleCommand
    {
        private const string CmdStr = "SELECT {0} FROM `{1}`";
        private const string WhereConst = "WHERE {0}";

        public MySqlSelectCommand(MySqlConnection conn, string table, MySqlTransaction trans = null)
            : base(CmdStr, table, conn, trans)
        {

        }

        public MySqlSelectCommand(string command, MySqlConnection conn, MySqlTransaction trans = null)
            : base(command, conn, trans)
        {

        }

        public MySqlSelectCommand(SimpleConnection conn, string table, SimpleTransaction transaction = null)
            : base(CmdStr, table, conn, transaction)
        {
        }

        public MySqlSelectCommand(string command, SimpleConnection connection, SimpleTransaction transaction = null)
            : base(command, connection, transaction)
        {
        }
        public override int Execute()
        {
            throw new NotImplementedException("This function should never be called in a SelectCommand class");
        }

        protected override string BuildCommandString()
        {
            string result;

            // Build select part
            var sb = new StringBuilder();
            foreach (var parameter in _set.Keys)
                sb.AppendFormat("`{0}`,", parameter);

            if (sb.Length == 0)
                sb.Append("*");

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

            if (_table != null && _table != string.Empty)
            {

                // Build where part
                if (_complexWhere != null && _complexWhere != string.Empty)
                    result = string.Format(CmdStr, sb.ToString().Trim(' ', ','), _table) + whereStr;
                else
                    result = string.Format(CmdStr, sb.ToString().Trim(' ', ','), _table) + whereStr;

            }
            else
            {
                if (Command.CommandText.Contains("{1}"))
                    result = string.Format(Command.CommandText, sb.ToString().Trim(' ', ',')) + whereStr;
                else
                    result = string.Format(Command.CommandText, whereStr);
            }

            if (_limit != null)
                result += _limit;

            if (_orderBy != null)
                result += _orderBy;

            return result;
        }

        protected override void BuildQuery()
        {
            // Add setting part
            Command.CommandText = BuildCommandString();

            // Add Where param values
            foreach (var parameter in _where)
                Command.Parameters.AddWithValue("@" + parameter.Key, parameter.Value);
        }
    }
}
