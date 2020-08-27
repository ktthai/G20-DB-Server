using Mabinogi;

namespace XMLDB3
{
	public class EventUpdateCommand : BasicCommand
	{
		private Event m_Event;

		private REPLY_RESULT m_Result;

		private byte m_errorCode;

		public EventUpdateCommand()
			: base(NETWORKMSG.NET_DB_EVENT_UPDATE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Event = EventSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("EventUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Event.Update(m_Event, ref m_errorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("EventUpdateCommand.DoProcess() : 이벤트 데이터를 성공적으로 업데이트했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("EventUpdateCommand.DoProcess() : 이벤트 데이터를 업데이트하는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("EventUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			return message;
		}
	}
}
