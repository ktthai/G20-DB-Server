using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmRecommendQueryZoneId : BasicCommand
	{
		private string m_strCharName = string.Empty;

		private long m_ZoneID;

		private REPLY_RESULT m_Result;

		public PrivateFarmRecommendQueryZoneId()
			: base(NETWORKMSG.NET_DB_PRIVATE_FARM_RECOMMEND_QUERY_ZONE_ID)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_strCharName = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PrivateFarmRecommendQueryZoneId.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.PrivateFarmRecommend.QueryZoneId(m_strCharName, out m_ZoneID);
			if (m_Result != 0)
			{
				WorkSession.WriteStatus("PrivateFarmRecommendQueryZoneId.DoProcess() : 개인농장ID를 읽는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("PrivateFarmRecommendQueryZoneId.DoProcess() : 개인농장ID를 읽는데 실패했습니다.");
			}
			return m_Result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PrivateFarmRecommendQueryZoneId.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				message.WriteS64(m_ZoneID);
			}
			return message;
		}
	}
}
