using Mabinogi;

namespace XMLDB3
{
	public class RecommendInsertCommand : BasicCommand
	{
		private string m_oldbieCharName = string.Empty;

		private string m_oldbieServerId = string.Empty;

		private string m_newbieCharName = string.Empty;

		private string m_newbieServerId = string.Empty;

		private long m_recommendTime;

		private REPLY_RESULT m_Result;

		public RecommendInsertCommand()
			: base(NETWORKMSG.NET_DB_RECOMMEND_INSERT)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_oldbieCharName = _message.ReadString();
			m_oldbieServerId = _message.ReadString();
			m_newbieCharName = _message.ReadString();
			m_newbieServerId = _message.ReadString();
			m_recommendTime = (long)_message.ReadU64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("RecommendInsertCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Recommend.Insert(m_oldbieCharName, m_oldbieServerId, m_newbieCharName, m_newbieServerId, m_recommendTime);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("RecommendInsertCommand.DoProcess() : 추천 데이터를 성공적으로 추가했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("RecommendInsertCommand.DoProcess() : 추천 데이터를 추가하는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("RecommendInsertCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			return message;
		}
	}
}
