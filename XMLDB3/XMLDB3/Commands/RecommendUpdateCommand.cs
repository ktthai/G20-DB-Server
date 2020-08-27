using Mabinogi;

namespace XMLDB3
{
	public class RecommendUpdateCommand : BasicCommand
	{
		private string m_newbieCharName = string.Empty;

		private string m_newbieServerId = string.Empty;

		private byte m_flagNum;

		private long m_markTime;

		private REPLY_RESULT m_Result;

		public RecommendUpdateCommand()
			: base(NETWORKMSG.NET_DB_RECOMMEND_UPDATE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_newbieCharName = _message.ReadString();
			m_newbieServerId = _message.ReadString();
			m_flagNum = _message.ReadU8();
			m_markTime = (long)_message.ReadU64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("RecommendUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Recommend.Update(m_newbieCharName, m_newbieServerId, m_flagNum, m_markTime);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("RecommendUpdateCommand.DoProcess() : 추천 데이터를 성공적으로 업데이트했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("RecommendUpdateCommand.DoProcess() : 추천 데이터를 업데이트하는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("RecommendUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			return message;
		}
	}
}
