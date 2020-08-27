using Mabinogi;

namespace XMLDB3
{
	public class FamilyUpdateMemberCommand : BasicCommand
	{
		private long m_familyID;

		private FamilyListFamilyMember m_FamilyMember;

		private REPLY_RESULT m_Result;

		private byte m_errorCode;

		public FamilyUpdateMemberCommand()
			: base(NETWORKMSG.NET_DB_FAMILY_UPDATE_MEMBER)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_familyID = _message.ReadS64();
			m_FamilyMember = FamilyMemberSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("FamilyUpdateMemberCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Family.UpdateMember(m_familyID, m_FamilyMember, ref m_errorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("FamilyUpdateMemberCommand.DoProcess() : 가문 멤버 데이터를 성공적으로 업데이트했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("FamilyUpdateMemberCommand.DoProcess() : 가문 멤버 데이터를 업데이트하는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("FamilyUpdateMemberCommand.MakeMessage() : 함수에 진입하였습니다");
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
