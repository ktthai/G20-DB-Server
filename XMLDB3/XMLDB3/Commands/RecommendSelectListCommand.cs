using Mabinogi;

namespace XMLDB3
{
	public class RecommendSelectListCommand : BasicCommand
	{
		private RecommendList m_RecommendList;

		private string m_oldbieCharName;

		private string m_oldbieServerId;

		public RecommendSelectListCommand()
			: base(NETWORKMSG.NET_DB_RECOMMEND_SELECT_LIST)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_oldbieCharName = _message.ReadString();
			m_oldbieServerId = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("RecommendSelectListCommand.DoProcess() : 함수에 진입하였습니다");
			m_RecommendList = QueryManager.Recommend.SelectList(m_oldbieCharName, m_oldbieServerId);
			if (m_RecommendList != null)
			{
				WorkSession.WriteStatus("RecommendSelectListCommand.DoProcess() : 추천 데이터를 성공적으로 select했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("RecommendSelectListCommand.DoProcess() : 추천 데이터를 select하는데 실패하였습니다.");
			}
			return m_RecommendList != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("RecommendSelectListCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_RecommendList != null)
			{
				message.WriteU8(1);
				RecommendListSerializer.Deserialize(m_RecommendList, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
