using Mabinogi;

namespace XMLDB3
{
	public class CommerceCriminalBossDie : BasicCommand
	{
		private string m_strServerName;

		private int m_criminalId;

		private REPLY_RESULT m_result;

		public CommerceCriminalBossDie()
			: base(NETWORKMSG.NET_DB_COMMERCE_CRIMINAL_BOSS_DIE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_strServerName = _message.ReadString();
			m_criminalId = _message.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CommerceCriminalBossDie.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.CCCommerce.RemoveBossData(m_strServerName, m_criminalId);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("CommerceCriminalBossDie.DoProcess() : 현상범을 제거 하는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CommerceCriminalBossDie.DoProcess() : 현상범을 제거 하는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CommerceCriminalBossDie.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_result);
			return message;
		}
	}
}
