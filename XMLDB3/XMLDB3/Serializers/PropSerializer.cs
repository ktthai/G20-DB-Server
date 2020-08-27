using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class PropSerializer
	{
		public static Prop Serialize(Message _message)
		{
			Prop prop = new Prop();
			prop.id = _message.ReadS64();
			prop.classid = _message.ReadS32();
			prop.region = _message.ReadS32();
			prop.x = _message.ReadS32();
			prop.y = _message.ReadS32();
			prop.z = _message.ReadS32();
			prop.direction = _message.ReadFloat();
			prop.scale = _message.ReadFloat();
			prop.color1 = _message.ReadS32();
			prop.color2 = _message.ReadS32();
			prop.color3 = _message.ReadS32();
			prop.color4 = _message.ReadS32();
			prop.color5 = _message.ReadS32();
			prop.color6 = _message.ReadS32();
			prop.color7 = _message.ReadS32();
			prop.color8 = _message.ReadS32();
			prop.color9 = _message.ReadS32();
			prop.name = _message.ReadString();
			prop.state = _message.ReadString();
			prop.entertime = _message.ReadS64();
			prop.extra = _message.ReadString();
			int num = _message.ReadS32();
			prop.@event = new List<PropEvent>(num);
			for (int i = 0; i < num; i++)
			{
				PropEvent propEvent = ReadPropEventFromMsg(_message);
				prop.@event[i] = propEvent;
			}
			return prop;
		}

		private static PropEvent ReadPropEventFromMsg(Message _message)
		{
			PropEvent propEvent = new PropEvent();
			propEvent.signal = _message.ReadS32();
			propEvent.type = _message.ReadS32();
			propEvent.extra = _message.ReadString();
			return propEvent;
		}

		public static Message Deserialize(Prop _prop, Message _message)
		{
			if (_prop == null)
			{
				_prop = new Prop();
			}
			_message.WriteS64(_prop.id);
			_message.WriteS32(_prop.classid);
			_message.WriteS32(_prop.region);
			_message.WriteS32(_prop.x);
			_message.WriteS32(_prop.y);
			_message.WriteS32(_prop.z);
			_message.WriteFloat(_prop.direction);
			_message.WriteFloat(_prop.scale);
			_message.WriteS32(_prop.color1);
			_message.WriteS32(_prop.color2);
			_message.WriteS32(_prop.color3);
			_message.WriteS32(_prop.color4);
			_message.WriteS32(_prop.color5);
			_message.WriteS32(_prop.color6);
			_message.WriteS32(_prop.color7);
			_message.WriteS32(_prop.color8);
			_message.WriteS32(_prop.color9);
			_message.WriteString(_prop.name);
			_message.WriteString(_prop.state);
			_message.WriteS64(_prop.entertime);
			_message.WriteString(_prop.extra);
			if (_prop.@event != null)
			{
				int data = _prop.@event.Count;
				_message.WriteS32(data);

				foreach (PropEvent event2 in _prop.@event)
				{
					WritePropEventToMsg(event2, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
			return _message;
		}

		private static void WritePropEventToMsg(PropEvent _event, Message _message)
		{
			if (_event == null)
			{
				_event = new PropEvent();
			}
			_message.WriteS32(_event.signal);
			_message.WriteS32(_event.type);
			_message.WriteString(_event.extra);
		}
	}
}
