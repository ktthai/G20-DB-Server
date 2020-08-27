using Mabinogi;

namespace XMLDB3
{
	internal class MailUpdateStatusCommand : SerializedCommand
	{
		private long m_PostID;

		private long m_ReceiverID;

		private long m_SenderID;

		private byte m_Status;

		private bool m_Result;

		public MailUpdateStatusCommand()
			: base(NETWORKMSG.NET_DB_MAIL_UPDATE_STATUS)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_PostID = _Msg.ReadS64();
			m_ReceiverID = _Msg.ReadS64();
			m_SenderID = _Msg.ReadS64();
			m_Status = _Msg.ReadU8();
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
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
			WorkSession.WriteStatus("MailUpdateStatusCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("MailUpdateStatusCommand.DoProcess() : 보낸 편지함을 삭제합니다.");
			m_Result = QueryManager.MailBox.UpdateMail(m_PostID, m_Status, m_ReceiverID, m_SenderID);
			if (m_Result)
			{
				WorkSession.WriteStatus("MailUpdateStatusCommand.DoProcess() : 보낸 편지함을 삭제하는데 성공하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("MailUpdateStatusCommand.DoProcess() : 보낸 편지함을 삭제하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MailUpdateStatusCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
