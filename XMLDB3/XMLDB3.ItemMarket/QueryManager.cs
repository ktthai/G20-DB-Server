using System.Collections;

namespace XMLDB3.ItemMarket
{
	public class QueryManager
	{
		private int idx;

		private Hashtable queryTable = new Hashtable();

		public Query PopQuery(int _packetNo)
		{
			Query result = null;
			if (queryTable.ContainsKey(_packetNo))
			{
				result = (Query)queryTable[_packetNo];
				queryTable.Remove(_packetNo);
			}
			return result;
		}

		public int PushQuery(uint _ID, uint _queryID, uint _targetID, int _clientID)
		{
			int num = idx++;
			while (queryTable.ContainsKey(num))
			{
				num = idx++;
			}
			Query query = new Query();
			query.packetNo = num;
			query.ID = _ID;
			query.queryID = _queryID;
			query.targetID = _targetID;
			query.clientID = _clientID;
			queryTable[num] = query;
			return num;
		}
	}
}
