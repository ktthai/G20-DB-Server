using Mabinogi;

namespace XMLDB3
{
	public class CheckCacheKeyCommand : BasicCommand
	{
		private string m_Account;

		private int m_CacheKey;

		private int m_OldCacheKey;

		private bool m_bResult;

		public CheckCacheKeyCommand()
			: base(NETWORKMSG.NET_DB_CHECK_CACHEKEY)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_CacheKey = _Msg.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("UpdateCacheKeyCommand.DoProcess() : 함수에 진입하였습니다");
			m_bResult = QueryManager.Accountref.CacheKeyCheck(m_Account, m_CacheKey, out m_OldCacheKey);
			if (m_bResult)
			{
				WorkSession.WriteStatus("UpdateCacheKeyCommand.DoProcess() : [" + m_Account + "," + m_CacheKey + "] 캐쉬 키를 업데이트 하여습니다");
				return true;
			}
			WorkSession.WriteStatus("UpdateCacheKeyCommand.DoProcess() : [" + m_Account + "," + m_CacheKey + "] 캐쉬 키를 업데이트 하지 못하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("UpdateCacheKeyCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_bResult)
			{
				message.WriteU8(1);
				message.WriteS32(m_OldCacheKey);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
