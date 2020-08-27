using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class PrivateFarmVisitorListSerializer
	{
		public static Dictionary<long, PrivateFarmVisitor> Serialize(Message _message)
		{
			int num = _message.ReadS32();
			if (num > 0)
			{
				Dictionary<long, PrivateFarmVisitor> hashtable = new Dictionary<long, PrivateFarmVisitor>(num);
				for (int i = 0; i < num; i++)
				{
					PrivateFarmVisitor privateFarmVisitor = PrivateFarmVisitorSerializer.Serialize(_message);
					if (privateFarmVisitor != null)
					{
						hashtable.Add(privateFarmVisitor.charId, privateFarmVisitor);
					}
				}
				return hashtable;
			}
			return null;
		}

		public static void Deserialize(Dictionary<long, PrivateFarmVisitor> _visitorList, Message _message)
		{
			if (_visitorList != null && _visitorList.Count > 0)
			{
				_message.WriteS32(_visitorList.Count);
				foreach (PrivateFarmVisitor value in _visitorList.Values)
				{
					PrivateFarmVisitorSerializer.Deserialize(value, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
		}
	}
}
