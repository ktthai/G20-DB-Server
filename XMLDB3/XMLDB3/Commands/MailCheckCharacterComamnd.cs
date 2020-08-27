using Mabinogi;

namespace XMLDB3
{
	internal class MailCheckCharacterComamnd : BasicCommand
	{
		private string m_Name = string.Empty;

		private long m_Result;

		private string m_OutName = string.Empty;

		private byte m_ErrorCode;

		public MailCheckCharacterComamnd()
			: base(NETWORKMSG.NET_DB_MAIL_CHECK_CHARACTER)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Name = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("MailCheckCharacterComamnd.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("MailCheckCharacterComamnd.DoProcess() : 캐릭터 이름을 확인합니다.");
			m_Result = QueryManager.MailBox.CheckCharacterName(m_Name, ref m_OutName, ref m_ErrorCode);
			if (m_Result != 0)
			{
				WorkSession.WriteStatus("MailCheckCharacterComamnd.DoProcess() : 캐릭터 이름이 정상입니다.");
			}
			else
			{
				WorkSession.WriteStatus("MailCheckCharacterComamnd.DoProcess() : 존재하지 않는 캐릭터 입니다.");
			}
			if (m_Result == 0)
			{
				return false;
			}
			return true;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MailCheckCharacterComamnd.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result != 0)
			{
				message.WriteU8(1);
				message.WriteS64(m_Result);
				message.WriteString(m_OutName);
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
