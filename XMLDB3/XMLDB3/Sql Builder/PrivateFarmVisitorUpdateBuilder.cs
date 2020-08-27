using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class PrivateFarmVisitorUpdateBuilder
	{
		public static void Build(long _id, Dictionary<long, PrivateFarmVisitor> _new, Dictionary<long, PrivateFarmVisitor> _cache, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_new != null)
			{
				if (_cache != null)
				{
					foreach (PrivateFarmVisitor value in _new.Values)
					{
						PrivateFarmVisitor privateFarmVisitor2 = (PrivateFarmVisitor)_cache[value.charId];
						PrivateFarmVisitorSqlBuilder.UpdateVisitor(_id, value, privateFarmVisitor2, conn, transaction);
						if (privateFarmVisitor2 != null)
						{
							_cache.Remove(privateFarmVisitor2.charId);
						}
					}
				}
				else
				{
					foreach (PrivateFarmVisitor value2 in _new.Values)
					{
						PrivateFarmVisitorSqlBuilder.UpdateVisitor(_id, value2, null, conn, transaction);
					}
				}
			}
			if (_cache != null)
			{
				foreach (PrivateFarmVisitor value3 in _cache.Values)
				{
					PrivateFarmVisitorSqlBuilder.DeleteVisitor(_id, value3.charId, conn, transaction);
				}
			}
		}
	}
}
