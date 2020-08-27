using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmCreateCommand : BasicCommand
	{
		private PrivateFarm m_PrivateFarm;

		private bool m_Result;

		public PrivateFarmCreateCommand()
			: base(NETWORKMSG.NET_DB_PRIVATEFARM_CREATE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_PrivateFarm = PrivateFarmSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PrivateFarmCreateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PrivateFarmCreateCommand.DoProcess() : [" + m_PrivateFarm.id + "] 를 생성합니다");
			m_Result = QueryManager.PrivateFarm.Create(m_PrivateFarm);
			if (m_Result)
			{
				WorkSession.WriteStatus("PrivateFarmCreateCommand.DoProcess() : [" + m_PrivateFarm.id + "] 를 생성하였습니다");
				return true;
			}
			WorkSession.WriteStatus("PrivateFarmCreateCommand.DoProcess() : [" + m_PrivateFarm.id + "] 를 생성하는데 실패하였습니다");
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
				message.WriteU64((ulong)m_PrivateFarm.id);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
