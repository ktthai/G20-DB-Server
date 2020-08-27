// Entire class system and idea lifted from Aura, because making the queries by hand is taking forever, and is pretty inefficient

using System.Collections.Generic;
using System;

namespace Mabinogi.SQL
{

    public abstract class SimpleCommand : IDisposable
    {
        protected Dictionary<string, object> _set;
        protected Dictionary<string, object> _where;
        protected string _complexWhere;
        protected string _table;
        protected string _orderBy;
        protected string _limit;

        private const string _asc = "ASC";
        private const string _desc = "DESC";
        private const string _orderByString = " ORDER BY {0} {1}";
        private const string _LimitNString = " LIMIT {0}";

        protected SimpleCommand()
        {
            _set = new Dictionary<string, object>();
            _where = new Dictionary<string, object>();
        }

        
        public abstract void AddParameter(string name, object value, bool addEscapeChar = true);

        
        public void Set(string field, object value)
        {
            if (!_where.ContainsKey(field))
                _set[field] = value;
        }

        
        public void Where(string field, object value)
        {
            if (!_set.ContainsKey(field))
                _where[field] = value;
        }

        public void ComplexWhere(string whereString)
        {
            // TODO: Consider writing a function to take a secondary command to use as a complex where statement
            _complexWhere = whereString;
        }

        public abstract int Execute();

        public abstract SimpleReader ExecuteReader();

        public abstract void Dispose();

        public void OrderBy(string column, bool ascending = true)
        {
            _orderBy = string.Format(_orderByString, column, ascending ? _asc : _desc);
        }

        public void Limit(int n)
        {
            _limit = string.Format(_LimitNString, n);
        }

        protected abstract string BuildCommandString();
    }
}