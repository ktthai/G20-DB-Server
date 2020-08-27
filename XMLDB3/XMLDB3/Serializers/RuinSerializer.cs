using Mabinogi;
using System;

namespace XMLDB3
{
	public class RuinSerializer
	{
		public static Ruin Serialize(Message _Msg)
		{
			Ruin ruin = new Ruin();
			ruin.ruinID = _Msg.ReadS32();
			ruin.state = _Msg.ReadS32();
			ruin.position = _Msg.ReadS32();
			ruin.lastTime = _Msg.ReadS32();
			ruin.exploCharID = _Msg.ReadS64();
			ruin.exploCharName = _Msg.ReadString();
			ruin.exploTime = new DateTime(_Msg.ReadS64());
			return ruin;
		}

		public static Message Deserialize(Ruin _ruin, Message _Msg)
		{
			_Msg.WriteS32(_ruin.ruinID);
			_Msg.WriteS32(_ruin.state);
			_Msg.WriteS32(_ruin.position);
			_Msg.WriteS32(_ruin.lastTime);
			_Msg.WriteS64(_ruin.exploCharID);
			_Msg.WriteString(_ruin.exploCharName);
			_Msg.WriteS64(_ruin.exploTime.Ticks);
			return _Msg;
		}
	}
}
