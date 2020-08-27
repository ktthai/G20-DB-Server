using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class CommerceCriminalUpdate : BasicCommand
	{
		private string m_strServerName;

		private int m_criminalId;

		private int m_reward;

		private CommerceCriminalInfo m_commerceCriminalInfo;

		private REPLY_RESULT m_result;

		public CommerceCriminalUpdate()
			: base(NETWORKMSG.NET_DB_COMMERCE_CRIMINAL_UPDATE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_commerceCriminalInfo = new CommerceCriminalInfo();
			m_commerceCriminalInfo.criminalTable = new Dictionary<string, CCommerceCriminalLost>();
			m_strServerName = _message.ReadString();
			m_criminalId = _message.ReadS32();
			m_reward = _message.ReadS32();
			int num = _message.ReadS32();
			for (int i = 0; i < num; i++)
			{
				CCommerceCriminalLost cCommerceCriminalLost = new CCommerceCriminalLost();
				cCommerceCriminalLost.charName = _message.ReadString();
				cCommerceCriminalLost.stolenDucat = _message.ReadS32();
				if (m_commerceCriminalInfo.criminalTable.ContainsKey(cCommerceCriminalLost.charName))
				{
					m_commerceCriminalInfo.criminalTable.Remove(cCommerceCriminalLost.charName);
				}
				m_commerceCriminalInfo.criminalTable.Add(cCommerceCriminalLost.charName, cCommerceCriminalLost);
			}
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CommerceCriminalUpdate.DoProcess() : 함수에 진입하였습니다");
			m_result = QueryManager.CCCommerce.UpdateCriminalInfo(m_strServerName, m_criminalId, m_reward, m_commerceCriminalInfo);
			if (m_result != 0)
			{
				WorkSession.WriteStatus("CommerceCriminalUpdate.DoProcess() : 현상범을 갱신하는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CommerceCriminalUpdate.DoProcess() : 현상범을 갱신하는데 실패했습니다.");
			}
			return m_result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CommerceCriminalUpdate.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_result);
			return message;
		}
	}
}
