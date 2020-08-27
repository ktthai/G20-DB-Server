using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class PrivateFarmFieldUpdateBuilder
	{
		public static void Build(long _id, Dictionary<long, PrivateFarmFacility> _new, Dictionary<long, PrivateFarmFacility> _cache, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_new != null)
			{
				if (_cache != null)
				{
					PrivateFarmFacility privateFarmFacility2;
					foreach (PrivateFarmFacility value in _new.Values)
					{
						privateFarmFacility2 = _cache[value.facilityId];
						PrivateFarmFacilitySqlBuilder.UpdateFacility(_id, value, privateFarmFacility2, conn, transaction);
						if (privateFarmFacility2 != null)
						{
							_cache.Remove(privateFarmFacility2.facilityId);
						}
					}
				}
				else
				{
					foreach (PrivateFarmFacility value in _new.Values)
					{
						PrivateFarmFacilitySqlBuilder.UpdateFacility(_id, value, null, conn, transaction);
					}
				}
			}

			if (_cache != null)
			{
				foreach (PrivateFarmFacility value3 in _cache.Values)
				{
					PrivateFarmFacilitySqlBuilder.DeleteFacility(value3.facilityId, conn, transaction);
				}
			}
		}
	}
}
