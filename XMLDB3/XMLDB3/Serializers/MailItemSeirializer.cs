using Mabinogi;
using System;

namespace XMLDB3
{
	internal class MailItemSeirializer
	{
		public static MailItem Serialize(Message _message)
		{
			MailItem mailItem = new MailItem();
			mailItem.receiverCharID = _message.ReadS64();
			mailItem.receiverCharName = _message.ReadString();
			mailItem.senderCharID = _message.ReadS64();
			mailItem.senderCharName = _message.ReadString();
			mailItem.itemCharge = _message.ReadS32();
			mailItem.senderMsg = _message.ReadString();
			mailItem.sendDate = new DateTime(_message.ReadS64());
			mailItem.postType = _message.ReadU8();
			mailItem.location = _message.ReadString();
			mailItem.status = _message.ReadU8();
			byte b = _message.ReadU8();
			if (b == 1)
			{
				mailItem.item = ItemSerializer.Serialize(_message);
			}
			return mailItem;
		}

		public static Message Deserialize(MailItem _mailItem, Message _message)
		{
			_message.WriteS64(_mailItem.postID);
			_message.WriteS64(_mailItem.receiverCharID);
			_message.WriteString(_mailItem.receiverCharName);
			_message.WriteS64(_mailItem.senderCharID);
			_message.WriteString(_mailItem.senderCharName);
			_message.WriteS32(_mailItem.itemCharge);
			_message.WriteString(_mailItem.senderMsg);
			_message.WriteS64(_mailItem.sendDate.Ticks);
			_message.WriteU8(_mailItem.postType);
			_message.WriteString(_mailItem.location);
			_message.WriteU8(_mailItem.status);
			if (_mailItem.item != null)
			{
				ItemSerializer.Deserialize(_mailItem.item, _message);
			}
			else
			{
				_message.WriteS64(0L);
			}
			return _message;
		}
	}
}
