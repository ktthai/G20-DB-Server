using Mabinogi;

namespace XMLDB3
{
	internal class MailReadCommand : BasicCommand
	{
		private long m_CharacterID;

		private bool m_Result;

		private MailBox m_ReceiveBox = new MailBox();

		private MailBox m_SendBox = new MailBox();

		public MailReadCommand()
			: base(NETWORKMSG.NET_DB_MAIL_READ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_CharacterID = _Msg.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("MailReadCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("MailReadCommand.DoProcess() : 편지함을 받아옵니다.");
			m_Result = QueryManager.MailBox.ReadMail(m_CharacterID, m_ReceiveBox, m_SendBox);
			if (m_Result)
			{
				WorkSession.WriteStatus("MailReadCommand.DoProcess() :  편지함을 받아오는데 성공하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("MailReadCommand.DoProcess() :  편지함을 받아오는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MailReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				MailBoxSerializer.Deserialize(m_ReceiveBox, message);
				MailBoxSerializer.Deserialize(m_SendBox, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
