using Mabinogi;

namespace XMLDB3
{
	public class RoyalAlchemistAddCommand : BasicCommand
	{
		private RoyalAlchemist m_RoyalAlchemist;

		private REPLY_RESULT m_Result;

		private byte m_errorCode;

		public RoyalAlchemistAddCommand()
			: base(NETWORKMSG.NET_DB_ROYALALCHEMIST_ADD)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_RoyalAlchemist = RoyalAlchemistSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("RoyalAlchemistUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.RoyalAlchemist.Add(m_RoyalAlchemist, ref m_errorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("RoyalAlchemistAddCommand.DoProcess() : 왕궁 연금술사 데이터를 성공적으로 추가했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("RoyalAlchemistAddCommand.DoProcess() : 왕궁 연금술사 데이터를 추가하는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("RoyalAlchemistAddCommand.MakeMessage() : 함수에 진입하였습니다");
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
