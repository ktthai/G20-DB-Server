using Mabinogi;

namespace XMLDB3
{
	internal class MailBoxSerializer
	{
		public static Message Deserialize(MailBox _mailBox, Message _message)
		{
			if (_mailBox.mailItem != null)
			{
				_message.WriteS32(_mailBox.mailItem.Length);
				MailItem[] mailItem = _mailBox.mailItem;
				foreach (MailItem mailItem2 in mailItem)
				{
					MailItemSeirializer.Deserialize(mailItem2, _message);
				}
			}
			else
			{
				_message.WriteS32(0);
			}
			return _message;
		}
	}
}
