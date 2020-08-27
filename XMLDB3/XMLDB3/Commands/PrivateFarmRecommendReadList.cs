using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmRecommendReadList : BasicCommand
	{
		private PrivateFarmRecommendList m_RecommendList;

		public PrivateFarmRecommendReadList()
			: base(NETWORKMSG.NET_DB_PRIVATE_FARM_RECOMMEND_READ_LIST)
		{
		}

		protected override void ReceiveData(Message _message)
		{
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PrivateFarmRecommendReadList.DoProcess() : 함수에 진입하였습니다");
			m_RecommendList = QueryManager.PrivateFarmRecommend.ReadList();
			if (m_RecommendList != null)
			{
				WorkSession.WriteStatus("PrivateFarmRecommendReadList.DoProcess() : 추천 농장 정보 리스트를 가져오는데 성공하였습니다.");
				return true;
			}
			WorkSession.WriteStatus("PrivateFarmRecommendReadList.DoProcess() : 추천 농장 정보 리스트를 가져오는데 실패하였습니다.");
			return null != m_RecommendList;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PrivateFarmRecommendReadList.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_RecommendList != null)
			{
				message.WriteU8(1);
				PrivateFarmRecommendListSerializer.Deserialize(m_RecommendList, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
