using Mabinogi;
using System;

namespace XMLDB3
{
	public class SessionStatistics
	{
		private string m_Name = string.Empty;

		private string m_Status = string.Empty;

		private DateTime m_StatusTime = DateTime.MinValue;

		private int m_NetworkSession;

		private DateTime m_CurrentTIme = DateTime.Now;

		public string Name => m_Name;

		public string Status => m_Status;

		public DateTime StatusTime => m_StatusTime;

		public int NetworkSession => m_NetworkSession;

		public DateTime CurrentTime => m_CurrentTIme;

		public SessionStatistics(string _name, string _status, DateTime _status_date, int _networksession)
		{
			m_Name = _name;
			m_Status = _status;
			m_StatusTime = _status_date;
			m_NetworkSession = _networksession;
		}

		public Message ToMessage()
		{
			Message message = new Message(0u, 0uL);
			message.WriteString(m_Name);
			message.WriteString(m_Status);
			message.WriteS64(m_StatusTime.Ticks);
			message.WriteS32(m_NetworkSession);
			message.WriteS64(m_CurrentTIme.Ticks);
			return message;
		}

		public Message FromMessage(Message _input)
		{
			m_Name = _input.ReadString();
			m_Status = _input.ReadString();
			m_StatusTime = new DateTime(_input.ReadS64());
			m_NetworkSession = _input.ReadS32();
			m_CurrentTIme = new DateTime(_input.ReadS64());
			return _input;
		}
	}
}
