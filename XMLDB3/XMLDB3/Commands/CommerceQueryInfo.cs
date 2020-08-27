using Mabinogi;

namespace XMLDB3
{
	public class CommerceQueryInfo : BasicCommand
	{
		private long m_charId;

		private GS_CommerceInfo m_commerceInfo;

		private REPLY_RESULT m_result;

		public CommerceQueryInfo()
			: base(NETWORKMSG.NET_DB_COMMERCE_T_QUERY_INFO)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_charId = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CommerceQueryInfo.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.GSCommerce.QueryInfo(m_charId, out m_commerceInfo);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("CommerceQueryInfo.DoProcess() : 무역 데이타 목록을 읽는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CommerceQueryInfo.DoProcess() : 무역 데이타 목록을 읽는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CommerceQueryInfo.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_result == REPLY_RESULT.SUCCESS && m_commerceInfo != null)
			{
				message.WriteU8(1);
				message.WriteS64(m_commerceInfo.ducat);
				message.WriteS64(m_commerceInfo.unlockTransport);
				message.WriteS32(m_commerceInfo.currentTransport);
				message.WriteS32(m_commerceInfo.lost_percent);
				if (m_commerceInfo.credibilityTable != null)
				{
					message.WriteS32(m_commerceInfo.credibilityTable.Count);
					{
						foreach (CommerceCredibility value in m_commerceInfo.credibilityTable.Values)
						{
							message.WriteS32(value.postId);
							message.WriteS32(value.credibility);
						}
						return message;
					}
				}
				message.WriteS32(0);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
