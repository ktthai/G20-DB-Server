using Mabinogi;

namespace XMLDB3
{
	public class LinkedApCharacterSelect : BasicCommand
	{
		private string m_serverName;

		private long m_charId;

		private LinkedApCharacter m_linkedApCharacter;

		private REPLY_RESULT m_Result;

		public LinkedApCharacterSelect()
			: base(NETWORKMSG.NET_DB_LINK_CHARACTER_SELECT)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_serverName = _message.ReadString();
			m_charId = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("LinkedApCharacterSelect.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("LinkedApCharacterSelect.DoProcess() : 링크AP 를 쿼리합니다.");
			m_Result = QueryManager.LinkedApCharacter.SelectLinkedApCharacter(m_serverName, m_charId, out m_linkedApCharacter);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("LinkedApCharacterSelect.DoProcess() : 링크AP 정보를 받아옵니다.");
			}
			else
			{
				WorkSession.WriteStatus("LinkedApCharacterSelect.DoProcess() : 링크 AP 정보를 받아오는데 실패했습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("LinkedApCharacterSelect.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result == REPLY_RESULT.SUCCESS && m_linkedApCharacter != null && m_linkedApCharacter.charID != 0 && m_serverName != null)
			{
				message.WriteU8(1);
				message.WriteS32(m_linkedApCharacter.savedAp);
				message.WriteS32(m_linkedApCharacter.termAp);
				message.WriteS64(m_linkedApCharacter.resetTime);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
