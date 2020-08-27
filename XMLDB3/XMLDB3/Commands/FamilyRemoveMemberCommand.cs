using Mabinogi;

namespace XMLDB3
{
	public class FamilyRemoveMemberCommand : BasicCommand
	{
		private long m_familyID;

		private long m_familyMemberID;

		private REPLY_RESULT m_Result;

		private byte m_errorCode;

		public FamilyRemoveMemberCommand()
			: base(NETWORKMSG.NET_DB_FAMILY_REMOVE_MEMBER)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_familyID = _message.ReadS64();
			m_familyMemberID = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("FamilyRemoveMemberCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Family.RemoveMember(m_familyID, m_familyMemberID, ref m_errorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("FamilyRemoveMemberCommand.DoProcess() : 가문 멤버 데이터를 성공적으로 지웠습니다.");
			}
			else
			{
				WorkSession.WriteStatus("FamilyRemoveMemberCommand.DoProcess() : 가문 멤버 데이터를 지우는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("FamilyRemoveMemberCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			if (m_Result == REPLY_RESULT.FAIL_EX)
			{
				message.WriteU8(m_errorCode);
			}
			return message;
		}
	}
}
