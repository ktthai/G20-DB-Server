using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmPenaltySelect : BasicCommand
	{
		private long m_idZone;

		private REPLY_RESULT m_Result;

		public PrivateFarmPenaltySelect()
			: base(NETWORKMSG.NET_DB_PRIVATE_FARM_PENALTY_CHECK)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_idZone = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PrivateFarmPenaltySelect.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.PrivateFarm.FindPenaltyState(m_idZone);
			if (m_Result != 0)
			{
				WorkSession.WriteStatus("PrivateFarmPenaltySelect.DoProcess() : 해당 농장은 패널티가 존재합니다.");
			}
			else
			{
				WorkSession.WriteStatus("PrivateFarmPenaltySelect.DoProcess() : 해당 농장은 패널티가 없습니다.");
			}
			return m_Result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PrivateFarmPenaltySelect.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			return message;
		}
	}
}
