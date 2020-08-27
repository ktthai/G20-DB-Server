using Mabinogi;

namespace XMLDB3
{
	public class RecommendSelectCommand : BasicCommand
	{
		private Recommend m_Recommend;

		private string m_newbieCharName;

		private string m_newbieServerId;

		public RecommendSelectCommand()
			: base(NETWORKMSG.NET_DB_RECOMMEND_SELECT)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_newbieCharName = _message.ReadString();
			m_newbieServerId = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("RecommendSelectCommand.DoProcess() : 함수에 진입하였습니다");
			m_Recommend = QueryManager.Recommend.Select(m_newbieCharName, m_newbieServerId);
			if (m_Recommend != null)
			{
				WorkSession.WriteStatus("RecommendSelectCommand.DoProcess() : 추천 데이터를 성공적으로 select했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("RecommendSelectCommand.DoProcess() : 추천 데이터를 select하는데 실패하였습니다.");
			}
			return m_Recommend != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("RecommendSelectCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Recommend != null)
			{
				message.WriteU8(1);
				RecommendSerializer.DeserializeOldbie(m_Recommend, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
