using Mabinogi;

namespace XMLDB3
{
	internal class PrivateFarmQueryUsableName : BasicCommand
	{
		private string m_name;

		private bool m_Result;

		public PrivateFarmQueryUsableName()
			: base(NETWORKMSG.NET_DB_PRIVATEFARM_QUERY_USABLE_NAME)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_name = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PrivateFarmQueryUsableName.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.PrivateFarm.IsUsableName(m_name);
			WorkSession.WriteStatus("PrivateFarmQueryUsableName.DoProcess() : 개인농장의 사용중인 이름 여부를 쿼리하는 데 성공했습니다.");
			return true;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PrivateFarmQueryUsableName.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)(m_Result ? 1 : 0));
			return message;
		}
	}
}
