using System.Data.SQLite;
using MySql.Data.MySqlClient;

namespace Mabinogi.SQL
{
    public abstract class SqlAdapter
    {
        protected abstract string ConfigRef { get; }

        private string ConnectionStr;
        protected bool IsLocalMode;

        public SqlAdapter()
        {
        }
        
        protected void SetConnection(string connStr, bool local)
        {
            ConnectionStr = connStr;
            IsLocalMode = local;
        }


        public SimpleConnection Connection
        {
            get
            {
                if (IsLocalMode)
                    return new SQLiteSimpleConnection(ConnectionL);
                else
                    return new MySqlSimpleConnection(ConnectionM);
            }
        }

        private MySqlConnection ConnectionM { get { return new MySqlConnection(ConnectionStr); } }

        private SQLiteConnection ConnectionL { get { return new SQLiteConnection(ConnectionStr); } }
    }
}

