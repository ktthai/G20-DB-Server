using Mabinogi;

namespace XMLDB3
{
	public class FarmUpdateCommand : BasicCommand
	{
		private Farm m_Farm;

		private REPLY_RESULT m_Result;

		private byte m_ErrorCode;

		public FarmUpdateCommand()
			: base(NETWORKMSG.NET_DB_FARM_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Farm = FarmSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("FarmUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("FarmUpdateCommand.DoProcess() : 농장을 업데이트합니다.");
			m_Result = QueryManager.Farm.Update(m_Farm, ref m_ErrorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("FarmUpdateCommand.DoProcess() :  농장을 업데이트했습니다");
				return true;
			}
			WorkSession.WriteStatus("FarmUpdateCommand.DoProcess() : 농장을 업데이트하는데 실패하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("FarmUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			if (m_Result == REPLY_RESULT.FAIL_EX)
			{
				message.WriteU8(m_ErrorCode);
			}
			return message;
		}
	}
}
