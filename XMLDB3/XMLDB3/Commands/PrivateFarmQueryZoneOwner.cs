using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmQueryZoneOwner : BasicCommand
	{
		private long m_CharID;

		private long m_ZoneID;

		private REPLY_RESULT m_Result;

		public PrivateFarmQueryZoneOwner()
			: base(NETWORKMSG.NET_DB_PRIVATEFARM_QUERY_ZONE_OWNER)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_ZoneID = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PrivateFarmQueryZoneOwner.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.PrivateFarm.QueryOwner(m_ZoneID, out m_CharID);
			if (m_Result != 0)
			{
				WorkSession.WriteStatus("PrivateFarmQueryZoneOwner.DoProcess() : 개인농장 주인ID를 읽는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("PrivateFarmQueryZoneOwner.DoProcess() : 개인농장주인ID를 읽는데 실패했습니다.");
			}
			return m_Result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PrivateFarmQueryZoneOwner.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				message.WriteS64(m_CharID);
			}
			return message;
		}
	}
}
