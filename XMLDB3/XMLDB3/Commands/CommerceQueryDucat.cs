using Mabinogi;

namespace XMLDB3
{
	public class CommerceQueryDucat : BasicCommand
	{
		private long m_charId;

		private long m_ducat;

		private REPLY_RESULT m_result;

		public CommerceQueryDucat()
			: base(NETWORKMSG.NET_DB_COMMERCE_T_QUERY_DUCAT)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_charId = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CommerceQueryDucat.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.GSCommerce.QueryDucat(m_charId, out m_ducat);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("CommerceQueryDucat.DoProcess() : 무역 데이타 목록을 읽는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CommerceQueryDucat.DoProcess() : 무역 데이타 목록을 읽는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CommerceQueryDucat.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_result == REPLY_RESULT.SUCCESS)
			{
				message.WriteU8(1);
				message.WriteS64(m_ducat);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
