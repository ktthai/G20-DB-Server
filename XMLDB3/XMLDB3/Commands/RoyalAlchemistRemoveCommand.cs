using Mabinogi;

namespace XMLDB3
{
	public class RoyalAlchemistRemoveCommand : BasicCommand
	{
		private long[] m_removeIDs;

		private REPLY_RESULT m_Result;

		private byte m_errorCode;

		public override bool IsPrimeCommand => true;

		public RoyalAlchemistRemoveCommand()
			: base(NETWORKMSG.NET_DB_ROYALALCHEMIST_REMOVE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			ushort num = _message.ReadU16();
			m_removeIDs = new long[num];
			for (ushort num2 = 0; num2 < num; num2 = (ushort)(num2 + 1))
			{
				m_removeIDs[num2] = _message.ReadS64();
			}
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("RoyalAlchemistRemoveCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.RoyalAlchemist.Remove(m_removeIDs, ref m_errorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("RoyalAlchemistRemoveCommand.DoProcess() : 왕궁 연금술사 데이터를 성공적으로 지웠습니다.");
			}
			else
			{
				WorkSession.WriteStatus("RoyalAlchemistRemoveCommand.DoProcess() : 왕궁 연금술 데이터를 지우는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("RoyalAlchemistRemoveCommand.MakeMessage() : 함수에 진입하였습니다");
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
