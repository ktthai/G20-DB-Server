using Mabinogi;

namespace XMLDB3
{
	public class LinkedApCharacterUpdate : BasicCommand
	{
		private LinkedApCharacter m_linkedApCharacter;

		private REPLY_RESULT m_Result;

		public LinkedApCharacterUpdate()
			: base(NETWORKMSG.NET_DB_LINK_CHARACTER_UPDATE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_linkedApCharacter = new LinkedApCharacter();
			m_linkedApCharacter.serverName = _message.ReadString();
			m_linkedApCharacter.charID = _message.ReadS64();
			m_linkedApCharacter.savedAp = _message.ReadS32();
			m_linkedApCharacter.termAp = _message.ReadS32();
			m_linkedApCharacter.resetTime = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("LinkedApCharacterUpdate.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("LinkedApCharacterUpdate.DoProcess() : 로그를 보냅니다");
			m_Result = QueryManager.LinkedApCharacter.UpdateLinkedApCharacter(m_linkedApCharacter);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("LinkedApCharacterUpdate.DoProcess() : 로그를 보냈습니다.");
			}
			else
			{
				WorkSession.WriteStatus("LinkedApCharacterUpdate.DoProcess() : 로그를 보내는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("LinkedApCharacterUpdate.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result == REPLY_RESULT.SUCCESS)
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
