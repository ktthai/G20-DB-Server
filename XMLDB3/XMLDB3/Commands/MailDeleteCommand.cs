using Mabinogi;

namespace XMLDB3
{
	internal class MailDeleteCommand : SerializedCommand
	{
		private long m_PostID;

		private long m_itemID;

		private byte m_itemType;

		private long m_ReceiverID;

		private long m_SenderID;

		private bool m_Result;

		private byte m_ErrorCode;

		public MailDeleteCommand()
			: base(NETWORKMSG.NET_DB_MAIL_DELETE)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_PostID = _Msg.ReadS64();
			m_itemID = _Msg.ReadS64();
			m_itemType = _Msg.ReadU8();
			m_ReceiverID = _Msg.ReadS64();
			m_SenderID = _Msg.ReadS64();
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			if (m_itemID != 0)
			{
				_helper.ObjectIDRegistant(m_itemID);
			}
			if (m_ReceiverID != 0)
			{
				_helper.ObjectIDRegistant(m_ReceiverID);
			}
			if (m_SenderID != 0)
			{
				_helper.ObjectIDRegistant(m_SenderID);
			}
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("MailDeleteCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("MailDeleteCommand.DoProcess() : 받은 편지함을 삭제합니다.");
			m_Result = QueryManager.MailBox.DeleteMail(m_PostID, m_itemID, m_itemType, m_ReceiverID, m_SenderID, ref m_ErrorCode);
			if (m_Result)
			{
				WorkSession.WriteStatus("MailDeleteCommand.DoProcess() : 받은 편지함을 삭제하는데 성공하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("MailDeleteCommand.DoProcess() : 받은 편지함을 삭제하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MailDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
			}
			else
			{
				message.WriteU8(0);
				message.WriteU8(m_ErrorCode);
			}
			return message;
		}
	}
}
