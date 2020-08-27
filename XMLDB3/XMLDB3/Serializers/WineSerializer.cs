using Mabinogi;
using System;

namespace XMLDB3
{
	public class WineSerializer
	{
		public static Wine Serialize(Message _message)
		{
			Wine wine = new Wine();
			wine.charID = _message.ReadS64();
			wine.wineType = _message.ReadU8();
			wine.agingCount = _message.ReadS16();
			wine.agingStartTime = new DateTime(_message.ReadS64());
			wine.lastRackingTime = new DateTime(_message.ReadS64());
			wine.acidity = _message.ReadS32();
			wine.purity = _message.ReadS32();
			wine.freshness = _message.ReadS32();
			return wine;
		}

		public static void Deserialize(Wine _wine, Message _message)
		{
			if (_wine != null)
			{
				_message.WriteS64(_wine.charID);
				_message.WriteU8(_wine.wineType);
				_message.WriteS16(_wine.agingCount);
				_message.WriteS64(_wine.agingStartTime.Ticks);
				_message.WriteS64(_wine.lastRackingTime.Ticks);
				_message.WriteS32(_wine.acidity);
				_message.WriteS32(_wine.purity);
				_message.WriteS32(_wine.freshness);
			}
		}
	}
}
