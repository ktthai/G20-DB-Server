using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmVisitorUpdateCommand : SerializedCommand
	{
		private PrivateFarm m_PrivateFarm;

		private bool m_Result;

		public PrivateFarmVisitorUpdateCommand()
			: base(NETWORKMSG.NET_DB_PRIVATEFARM_UPDATE_VISITOR)
		{
		}

		protected override void _ReceiveData(Message _message)
		{
			m_PrivateFarm = new PrivateFarm();
			m_PrivateFarm = PrivateFarmSerializer.Serialize(_message);
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.ObjectIDRegistant(m_PrivateFarm.id);
			if (m_PrivateFarm.field != null)
			{
				foreach (PrivateFarmFacility value in m_PrivateFarm.field.Values)
				{
					_helper.ObjectIDRegistant(value.facilityId);
				}
			}
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("PrivateFarmVisitorUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PrivateFarmVisitorUpdateCommand.DoProcess() : [" + m_PrivateFarm.id + "] 의 데이터를 캐쉬에서 읽습니다");
			PrivateFarm privateFarm = (PrivateFarm)ObjectCache.PrivateFarm.Extract(m_PrivateFarm.id);
			WorkSession.WriteStatus("PrivateFarmVisitorUpdateCommand.DoProcess() : [" + m_PrivateFarm.id + "] 의 데이터를 업데이트합니다");
			m_Result = QueryManager.PrivateFarm.Write(m_PrivateFarm, privateFarm, PRIVATEFARM_WRITEMODE.VISITOR_LIST);
			if (!m_Result)
			{
				WorkSession.WriteStatus("PrivateFarmVisitorUpdateCommand.DoProcess() : [" + m_PrivateFarm.id + "] 의 데이터 저장에 실패하였습니다. 다시 시도합니다");
				m_Result = QueryManager.PrivateFarm.Write(m_PrivateFarm, null, PRIVATEFARM_WRITEMODE.VISITOR_LIST);
			}
			if (m_Result)
			{
				WorkSession.WriteStatus("PrivateFarmVisitorUpdateCommand.DoProcess() : [" + m_PrivateFarm.id + "] 의 데이터를 업데이트 하였습니다");
				if (privateFarm != null)
				{
					privateFarm.visitorList = m_PrivateFarm.visitorList;
					ObjectCache.PrivateFarm.Push(privateFarm.id, m_PrivateFarm);
				}
			}
			else
			{
				WorkSession.WriteStatus("PrivateFarmVisitorUpdateCommand.DoProcess() : [" + m_PrivateFarm.id + "] 의 데이터 저장에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PrivateFarmVisitorUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
