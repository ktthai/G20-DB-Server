using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class ScrapBookQueryScrappedList : BasicCommand
	{
		private long m_charId;

		private Dictionary<long, ScrapBookData> m_scrappedList;

		private REPLY_RESULT m_result;

		public ScrapBookQueryScrappedList()
			: base(NETWORKMSG.NET_DB_SCRAPBOOK_QUERY_SCRAPPED_LIST)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_charId = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ScrapBookQueryScrappedFishList.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.ScrapBook.QueryScrappedList(m_charId, out m_scrappedList);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("ScrapBookQueryScrappedFishList.DoProcess() : 물고기 목록을 읽는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("ScrapBookQueryScrappedFishList.DoProcess() : 물고기 목록을 읽는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ScrapBookQueryScrappedFishList.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_result);
			if (m_result == REPLY_RESULT.SUCCESS)
			{
				message.WriteS32(m_scrappedList.Count);
				{
					foreach (ScrapBookData value in m_scrappedList.Values)
					{
						message.WriteU8(value.scrapType);
						message.WriteS32(value.classId);
						message.WriteS32(value.scrapData);
						message.WriteS32(value.regionId);
						message.WriteS64(value.updatetime.Ticks);
					}
					return message;
				}
			}
			return message;
		}
	}
}
