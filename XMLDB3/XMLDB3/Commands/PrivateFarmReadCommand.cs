using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmReadCommand : SerializedCommand
	{
		private PrivateFarm m_PrivateFarm;

		private long m_Id;

		public PrivateFarmReadCommand()
			: base(NETWORKMSG.NET_DB_PRIVATEFARM_READ)
		{
		}

		protected override void _ReceiveData(Message _message)
		{
			m_Id = _message.ReadS64();
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.ObjectIDRegistant(m_Id);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("PrivateFarmReadCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PrivateFarmReadCommand.DoProcess() : [" + m_Id + "] 를 캐쉬에서 읽기를 시도합니다");
			PrivateFarm cache = (PrivateFarm)ObjectCache.PrivateFarm.Extract(m_Id);
			m_PrivateFarm = QueryManager.PrivateFarm.Read(m_Id, cache);
			if (m_PrivateFarm != null)
			{
				WorkSession.WriteStatus("PrivateFarmReadCommand.DoProcess() : [" + m_Id + "] 를 데이터베이스에서 읽었습니다");
				ObjectCache.PrivateFarm.Push(m_Id, m_PrivateFarm);
			}
			else
			{
				WorkSession.WriteStatus("PrivateFarmReadCommand.DoProcess() : [" + m_Id + "] 를 데이터베이스에 쿼리하는데 실패하였습니다");
			}
			return m_PrivateFarm != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PrivateFarmReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_PrivateFarm != null)
			{
				message.WriteU8(1);
				PrivateFarmSerializer.Deserialize(m_PrivateFarm, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
