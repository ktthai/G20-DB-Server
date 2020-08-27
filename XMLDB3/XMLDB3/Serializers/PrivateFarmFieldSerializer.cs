using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class PrivateFarmFieldSerializer
	{
		public static Dictionary<long, PrivateFarmFacility> Serialize(Message _message)
		{
			int num = _message.ReadS32();
			if (num > 0)
			{
				Dictionary<long, PrivateFarmFacility> hashtable = new Dictionary<long, PrivateFarmFacility>(num);
				for (int i = 0; i < num; i++)
				{
					PrivateFarmFacility privateFarmFacility = PrivateFarmFacilitySerializer.Serialize(_message);
					if (privateFarmFacility != null)
					{
						hashtable.Add(privateFarmFacility.facilityId, privateFarmFacility);
					}
				}
				return hashtable;
			}
			return null;
		}

		public static void Deserialize(Dictionary<long, PrivateFarmFacility> _field, Message _message)
		{
			if (_field != null && _field.Count > 0)
			{
				_message.WriteS32(_field.Count);
				foreach (PrivateFarmFacility value in _field.Values)
				{
					PrivateFarmFacilitySerializer.Deserialize(value, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}
	}
}
