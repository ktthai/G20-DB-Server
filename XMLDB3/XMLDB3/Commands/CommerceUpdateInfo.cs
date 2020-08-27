using Mabinogi;

namespace XMLDB3
{
	public class CommerceUpdateInfo : BasicCommand
	{
		private long m_charId;

		private long m_ducat;

		private long m_unlockTransport;

		private int m_currentTransport;

		private int m_lostPercent;

		private int m_postId;

		private int m_postCredit;

		private REPLY_RESULT m_result;

		public CommerceUpdateInfo()
			: base(NETWORKMSG.NET_DB_COMMERCE_T_UPDATE_INFO)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_charId = _message.ReadS64();
			m_ducat = _message.ReadS64();
			m_unlockTransport = _message.ReadS64();
			m_currentTransport = _message.ReadS32();
			m_lostPercent = _message.ReadS32();
			m_postId = _message.ReadS32();
			m_postCredit = _message.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CommerceUpdateInfo.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.GSCommerce.UpdateInfo(m_charId, m_ducat, m_unlockTransport, m_currentTransport, m_lostPercent, m_postId, m_postCredit);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("CommerceUpdateInfo.DoProcess() : 교역정보를 읽는것을 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CommerceUpdateInfo.DoProcess() : 교역정보를 읽는것을 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CommerceUpdateInfo.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_result);
			return message;
		}
	}
}
