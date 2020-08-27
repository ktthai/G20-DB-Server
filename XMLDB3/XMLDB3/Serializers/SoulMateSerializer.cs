using Mabinogi;
using System;

namespace XMLDB3
{
	public class SoulMateSerializer
	{
		public static SoulMate Serialize(Message _message)
		{
			SoulMate soulMate = new SoulMate();
			soulMate.mainCharId = (long)_message.ReadU64();
			soulMate.mainCharName = _message.ReadString();
			soulMate.subCharId = (long)_message.ReadU64();
			soulMate.subCharName = _message.ReadString();
			soulMate.matePoint = _message.ReadU16();
			soulMate.startTime = new DateTime(_message.ReadS64());
			return soulMate;
		}

		public static void Deserialize(SoulMate _data, Message _message)
		{
			if (_data != null)
			{
				_message.WriteU64((ulong)_data.mainCharId);
				_message.WriteString(_data.mainCharName);
				_message.WriteU64((ulong)_data.subCharId);
				_message.WriteString(_data.subCharName);
				_message.WriteU16(_data.matePoint);
				_message.WriteS64(_data.startTime.Ticks);
			}
		}
	}
}
