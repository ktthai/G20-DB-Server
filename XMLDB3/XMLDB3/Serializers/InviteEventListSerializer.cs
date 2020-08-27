using Mabinogi;

namespace XMLDB3
{
	public class InviteEventListSerializer
	{
		public static void Deserialize(InviteEventList _list, Message _message)
		{
			if (_list == null || _list.InviteEvents == null || _list.InviteEvents.Length == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_list.InviteEvents.Length);
			InviteEvent[] inviteEvents = _list.InviteEvents;
			foreach (InviteEvent data in inviteEvents)
			{
				InviteEventSerializer.Deserialize(data, _message);
			}
		}

		public static InviteEventList Serialize(Message _message)
		{
			InviteEventList inviteEventList = new InviteEventList();
			int num = _message.ReadS32();
			if (num > 0)
			{
				inviteEventList.InviteEvents = new InviteEvent[num];
				for (int i = 0; i < num; i++)
				{
					inviteEventList.InviteEvents[i] = InviteEventSerializer.Serialize(_message);
				}
			}
			else
			{
				inviteEventList.InviteEvents = null;
			}
			return inviteEventList;
		}
	}
}
