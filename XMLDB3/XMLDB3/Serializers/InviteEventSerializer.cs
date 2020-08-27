using Mabinogi;
using System;

namespace XMLDB3
{
	public class InviteEventSerializer
	{
		public static InviteEvent Serialize(Message _message)
		{
			InviteEvent inviteEvent = new InviteEvent();
			inviteEvent.mabiId = _message.ReadTypeOf(inviteEvent.mabiId);
			inviteEvent.servername = _message.ReadTypeOf(inviteEvent.servername);
			inviteEvent.invitecharactername = _message.ReadTypeOf(inviteEvent.invitecharactername);
			inviteEvent.senddate = new DateTime(_message.ReadS64());
			return inviteEvent;
		}

		public static void Deserialize(InviteEvent _data, Message _message)
		{
			if (_data != null)
			{
				_message.WriteTypeOf(_data.mabiId);
				_message.WriteTypeOf(_data.servername);
				_message.WriteTypeOf(_data.invitecharactername);
				_message.WriteS64(_data.senddate.Ticks);
			}
		}
	}
}
