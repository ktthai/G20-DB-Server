using Mabinogi;

namespace XMLDB3
{
	public class SoulMateUpdateCommand : BasicCommand
	{
		private SoulMate m_SoulMate;

		private REPLY_RESULT m_Result;

		private byte m_errorCode;

		public SoulMateUpdateCommand()
			: base(NETWORKMSG.NET_DB_SOULMATE_UPDATE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_SoulMate = SoulMateSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("SoulMateUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.SoulMate.UpdateSoulMate(m_SoulMate, ref m_errorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("SoulMateUpdateCommand.DoProcess() : 소울메이트 데이터를 성공적으로 추가했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("SoulMateUpdateCommand.DoProcess() : 소울메이트 데이터를 추가하는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("SoulMateUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
