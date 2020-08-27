using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmUpdateTimeSelect : BasicCommand
	{
		private long m_ZoneID;

		private long m_UpdateTime;

		public PrivateFarmUpdateTimeSelect()
			: base(NETWORKMSG.NET_DB_PRIVATE_FARM_UPDATETIME_SELECT)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_ZoneID = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PrivateFarmUpdateTimeSelect.DoProcess() : 함수에 진입하였습니다");
			m_UpdateTime = QueryManager.PrivateFarm.SelectPrivateFarmUpdateTime(m_ZoneID);
			if (m_UpdateTime != 0)
			{
				WorkSession.WriteStatus("PrivateFarmUpdateTimeSelect.DoProcess() : 개인농장 UPdateTime을 읽는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("PrivateFarmUpdateTimeSelect.DoProcess() : 개인농장 UPdateTime을 읽는데 실패했습니다.");
			}
			return true;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PrivateFarmUpdateTimeSelect.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU64((ulong)m_UpdateTime);
			return message;
		}
	}
}
