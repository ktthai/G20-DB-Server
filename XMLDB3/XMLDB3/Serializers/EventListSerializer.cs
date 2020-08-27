using Mabinogi;

namespace XMLDB3
{
	public class EventListSerializer
	{
		public static void Deserialize(EventList _list, Message _message)
		{
			if (_list == null || _list.events == null || _list.events.Length == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_list.events.Length);
			Event[] events = _list.events;
			foreach (Event e in events)
			{
				EventSerializer.Deserialize(e, _message);
			}
		}
	}
}
