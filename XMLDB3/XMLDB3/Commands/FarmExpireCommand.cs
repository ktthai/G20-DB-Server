using Mabinogi;

namespace XMLDB3
{
	public class FarmExpireCommand : BasicCommand
	{
		private long m_FarmId;

		private REPLY_RESULT m_Result;

		private byte m_ErrorCode;

		public override bool IsPrimeCommand => true;

		public FarmExpireCommand()
			: base(NETWORKMSG.NET_DB_FARM_EXPIRE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_FarmId = _Msg.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("FarmExpireCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("FarmExpireCommand.DoProcess() : 농장을 만료시킵니다.");
			m_Result = QueryManager.Farm.Expire(m_FarmId, ref m_ErrorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("FarmExpireCommand.DoProcess() :  농장을 만료시켰습니다");
				return true;
			}
			WorkSession.WriteStatus("FarmExpireCommand.DoProcess() : 농장을 만료시키는데 실패하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("FarmExpireCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result != REPLY_RESULT.FAIL_EX)
			{
				message.WriteU8((byte)m_Result);
			}
			else
			{
				message.WriteU8((byte)m_Result);
				message.WriteU8(m_ErrorCode);
			}
			return message;
		}
	}
}
