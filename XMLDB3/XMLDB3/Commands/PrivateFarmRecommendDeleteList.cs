using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmRecommendDeleteList : BasicCommand
	{
		private bool m_Result;

		public PrivateFarmRecommendDeleteList()
			: base(NETWORKMSG.NET_DB_PRIVATE_FARM_RECOMMEND_DELETE_LIST)
		{
		}

		protected override void ReceiveData(Message _message)
		{
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PrivateFarmRecommendDeleteList.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.PrivateFarmRecommend.DeleteAllList();
			if (m_Result)
			{
				WorkSession.WriteStatus("PrivateFarmRecommendDeleteList.DoProcess() : 추천 농장 정보 리스트를 삭제하는데 성공하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("PrivateFarmRecommendDeleteList.DoProcess() : 추천 농장 정보 리스트를 삭제하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PrivateFarmRecommendDeleteList.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
