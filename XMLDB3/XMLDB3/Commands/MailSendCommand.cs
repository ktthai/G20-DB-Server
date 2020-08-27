using Mabinogi;

namespace XMLDB3
{
	internal class MailSendCommand : SerializedCommand
	{
		private MailItem m_MailItem;

		private long m_Result;

		private byte m_ErrorCode;

		public MailSendCommand()
			: base(NETWORKMSG.NET_DB_MAIL_SEND)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_MailItem = MailItemSeirializer.Serialize(_Msg);
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			if (m_MailItem.item != null)
			{
				if (m_MailItem.senderCharID != 0)
				{
					_helper.ObjectIDRegistant(m_MailItem.senderCharID);
				}
				_helper.ObjectIDRegistant(m_MailItem.item.id);
			}
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("MailSendCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("MailSendCommand.DoProcess() : [" + m_MailItem.senderCharID + "] 의 데이터를 캐쉬에서 읽습니다");
			m_Result = QueryManager.MailBox.SendMail(m_MailItem, ref m_ErrorCode);
			if (m_Result != 0)
			{
				WorkSession.WriteStatus("MailSendCommand.DoProcess() : 메일을 보냇습니다..");
			}
			else
			{
				WorkSession.WriteStatus("MailSendCommand.DoProcess() : 메일을 보내는데 실패하였습니다..");
			}
			return m_Result != 0;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MailSendCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result != 0)
			{
				message.WriteU8(1);
				message.WriteS64(m_Result);
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
