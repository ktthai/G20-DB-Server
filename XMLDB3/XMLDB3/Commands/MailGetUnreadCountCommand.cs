using Mabinogi;

namespace XMLDB3
{
	internal class MailGetUnreadCountCommand : BasicCommand
	{
		private long m_ReceiverID;

		private int m_UnreadCount;

		private bool m_Result;

		public MailGetUnreadCountCommand()
			: base(NETWORKMSG.NET_DB_MAIL_GET_UNREAD_COUNT)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_ReceiverID = _Msg.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("MailGetUnreadCountCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("MailGetUnreadCountCommand.DoProcess() : 안읽은 메일 갯수를 가져옵니다.");
			m_Result = QueryManager.MailBox.GetUnreadCount(m_ReceiverID, out m_UnreadCount);
			if (m_Result)
			{
				WorkSession.WriteStatus("MailGetUnreadCountCommand.DoProcess() : 안읽은 메일 갯수를 가져오는데 성공하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("MailGetUnreadCountCommand.DoProcess() : 안읽은 메일 갯수를 가져오는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MailGetUnreadCountCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				message.WriteU32((uint)m_UnreadCount);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
