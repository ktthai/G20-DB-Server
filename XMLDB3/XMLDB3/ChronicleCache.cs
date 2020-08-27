using System;
using System.Collections;

namespace XMLDB3
{
	public class ChronicleCache
	{
		private string serverName;

		private Hashtable table;

		public ChronicleCache(string _serverName)
		{
			serverName = _serverName;
			table = new Hashtable();
		}

		public ChronicleCache(string _serverName, IDictionary _dic)
		{
			serverName = _serverName;
			table = new Hashtable(_dic);
		}

		public void InsertKey(int _questID, int _count)
		{
			table.Add(_questID, _count);
		}

		public bool Exists(int _queryID)
		{
			return table.Contains(_queryID);
		}

		public int GetNextCount(string _serverName, int _queryID, out DateTime _createTime)
		{
			if (_serverName != serverName)
			{
				throw new Exception("서버이름 불일치:" + serverName + ":" + _serverName);
			}
			if (!table.Contains(_queryID))
			{
				throw new Exception("없는 키:" + _queryID);
			}
			lock (this)
			{
				int num = (int)table[_queryID];
				table[_queryID] = ++num;
				_createTime = DateTime.Now;
				return num;
			}
		}

		public DateTime GetNextTime()
		{
			lock (this)
			{
				return DateTime.Now;
			}
		}
	}
}
