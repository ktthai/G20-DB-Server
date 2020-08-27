using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmPenaltyDelete : BasicCommand
	{
		private long m_idZone;

		private REPLY_RESULT m_Result;

		private uint m_Penalty;

		public PrivateFarmPenaltyDelete()
			: base(NETWORKMSG.NET_DB_PRIVATE_FARM_PENALTY_DELETE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_idZone = _message.ReadS64();
			m_Penalty = _message.ReadU32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PrivateFarmPenaltyDelete.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.PrivateFarm.DeletePenaltyState(m_idZone, m_Penalty);
			if (m_Result != 0)
			{
				WorkSession.WriteStatus("PrivateFarmPenaltyDelete.DoProcess() : 농장 패널티를 없애는데 성공하였습니다..");
			}
			else
			{
				WorkSession.WriteStatus("PrivateFarmPenaltyDelete.DoProcess() : 농장 패널티를 없애는데 실패했습니다.");
			}
			return m_Result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PrivateFarmPenaltyDelete.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			return message;
		}
	}
}
