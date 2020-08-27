using Mabinogi;

namespace XMLDB3
{
	public class EventSerializer
	{
		public static Event Serialize(Message _Msg)
		{
			Event @event = new Event();
			@event.eventType = _Msg.ReadU8();
			@event.account = _Msg.ReadString();
			@event.charName = _Msg.ReadString();
			@event.serverName = _Msg.ReadString();
			return @event;
		}

		public static Message Deserialize(Event _e, Message _Msg)
		{
			_Msg.WriteU8(_e.eventType);
			_Msg.WriteString(_e.account);
			_Msg.WriteString(_e.charName);
			_Msg.WriteString(_e.serverName);
			return _Msg;
		}
	}
}
