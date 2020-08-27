using Mabinogi;

namespace XMLDB3
{
	public class SoulMateRemoveCommand : BasicCommand
	{
		private long m_mainCharId;

		private REPLY_RESULT m_Result;

		private byte m_errorCode;

		public SoulMateRemoveCommand()
			: base(NETWORKMSG.NET_DB_SOULMATE_REMOVE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_mainCharId = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("SoulMateRemoveCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.SoulMate.RemoveSoulMate(m_mainCharId, ref m_errorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("SoulMateRemoveCommand.DoProcess() : 소울메이트 데이터를 성공적으로 지웠습니다.");
			}
			else
			{
				WorkSession.WriteStatus("SoulMateRemoveCommand.DoProcess() : 소울메이트 데이터를 지우는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("SoulMateRemoveCommand.MakeMessage() : 함수에 진입하였습니다");
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
