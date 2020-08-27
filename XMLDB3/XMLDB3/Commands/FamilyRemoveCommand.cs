using Mabinogi;

namespace XMLDB3
{
	public class FamilyRemoveCommand : BasicCommand
	{
		private long m_FamilyID;

		private REPLY_RESULT m_Result;

		private byte m_errorCode;

		public FamilyRemoveCommand()
			: base(NETWORKMSG.NET_DB_FAMILY_REMOVE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_FamilyID = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("FamilyRemoveCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Family.RemoveFamily(m_FamilyID, ref m_errorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("FamilyRemoveCommand.DoProcess() : 가문 데이터를 성공적으로 지웠습니다.");
			}
			else
			{
				WorkSession.WriteStatus("FamilyRemoveCommand.DoProcess() : 가문 데이터를 지우는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("FamilyRemoveCommand.MakeMessage() : 함수에 진입하였습니다");
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
