using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmRecommendAdd : BasicCommand
	{
		private string m_strCharName = string.Empty;

		private long m_ZoneID;

		private bool m_Result;

		public PrivateFarmRecommendAdd()
			: base(NETWORKMSG.NET_DB_PRIVATE_FARM_RECOMMEND_ADD)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_ZoneID = _message.ReadS64();
			m_strCharName = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PrivateFarmRecommendAdd.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PrivateFarmRecommendAdd.DoProcess() : [" + m_ZoneID + "] 를 추가합니다.");
			m_Result = QueryManager.PrivateFarmRecommend.Add(m_strCharName, m_ZoneID);
			if (m_Result)
			{
				WorkSession.WriteStatus("PrivateFarmRecommendAdd.DoProcess() : [" + m_ZoneID + "] 를 추가하였습니다.");
				return true;
			}
			WorkSession.WriteStatus("PrivateFarmRecommendAdd.DoProcess() : [" + m_ZoneID + "] 를 추가하는데 실패 하였습니다.");
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PrivateFarmCreateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
