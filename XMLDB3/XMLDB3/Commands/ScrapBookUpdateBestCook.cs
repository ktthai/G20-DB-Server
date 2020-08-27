using Mabinogi;

namespace XMLDB3
{
	public class ScrapBookUpdateBestCook : BasicCommand
	{
		private BestCookData m_bestCookData;

		private REPLY_RESULT m_result;

		public ScrapBookUpdateBestCook()
			: base(NETWORKMSG.NET_DB_SCRAPBOOK_UPDATE_BEST_COOK)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_bestCookData = new BestCookData();
			m_bestCookData.classId = _message.ReadS32();
			m_bestCookData.characterId = _message.ReadS64();
			m_bestCookData.characterName = _message.ReadString();
			m_bestCookData.quality = _message.ReadS32();
			m_bestCookData.comment = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ScrapBookUpdateBestCook.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.ScrapBook.UpdateBestCook(m_bestCookData);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("ScrapBookUpdateBestCook.DoProcess() : 최고 요리사를 갱신하는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("ScrapBookUpdateBestCook.DoProcess() : 최고 요리사를 갱신하는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ScrapBookUpdateBestCook.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_result);
			return message;
		}
	}
}
