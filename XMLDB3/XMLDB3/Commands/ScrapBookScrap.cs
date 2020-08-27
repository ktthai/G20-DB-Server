using Mabinogi;

namespace XMLDB3
{
	public class ScrapBookScrap : BasicCommand
	{
		private long m_charId;

		private byte m_type;

		private int m_classId;

		private int m_oldScrapData;

		private int m_scrapData;

		private int m_regionId;

		private REPLY_RESULT m_result;

		public ScrapBookScrap()
			: base(NETWORKMSG.NET_DB_SCRAPBOOK_SCRAP)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_charId = _message.ReadS64();
			m_type = _message.ReadU8();
			m_classId = _message.ReadS32();
			m_oldScrapData = _message.ReadS32();
			m_scrapData = _message.ReadS32();
			m_regionId = _message.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ScrapBookScrap.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.ScrapBook.Scrap(m_charId, m_type, m_classId, m_oldScrapData, m_scrapData, m_regionId);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("ScrapBookScrap.DoProcess() : 물고기 길이를 읽는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("ScrapBookScrap.DoProcess() : 물고기 길이를 읽는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ScrapBookScrap.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_result);
			return message;
		}
	}
}
