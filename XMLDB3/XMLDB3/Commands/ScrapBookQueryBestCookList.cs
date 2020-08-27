using Mabinogi;
using System.Collections;

namespace XMLDB3
{
	public class ScrapBookQueryBestCookList : BasicCommand
	{
		private Hashtable m_bestCookList;

		private REPLY_RESULT m_result;

		public ScrapBookQueryBestCookList()
			: base(NETWORKMSG.NET_DB_SCRAPBOOK_QUERY_BEST_COOK_LIST)
		{
		}

		protected override void ReceiveData(Message _message)
		{
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ScrapBookQueryBestCookList.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.ScrapBook.QueryBestCookList(out m_bestCookList);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("ScrapBookQueryBestCookList.DoProcess() : 최고요리사 목록을 읽는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("ScrapBookQueryBestCookList.DoProcess() : 최고요리사 목록을 읽는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ScrapBookQueryBestCookList.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_result);
			if (m_result == REPLY_RESULT.SUCCESS)
			{
				message.WriteS32(m_bestCookList.Count);
				{
					foreach (BestCookData value in m_bestCookList.Values)
					{
						message.WriteS32(value.classId);
						message.WriteS64(value.characterId);
						message.WriteString(value.characterName);
						message.WriteS32(value.quality);
						message.WriteString(value.comment);
					}
					return message;
				}
			}
			return message;
		}
	}
}
