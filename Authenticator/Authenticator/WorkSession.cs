using System;
using System.Collections;
using System.Threading;

namespace Authenticator
{
	public class WorkSession
	{
		private Thread m_Thread;

		private string m_Name = string.Empty;

		private DateTime m_StartTime = DateTime.MinValue;

		private string m_Status = string.Empty;

		private DateTime m_StatusDate = DateTime.MinValue;

		private int m_NetworkSession;

		private static Hashtable m_SessionTable = new Hashtable();

		private static WorkSession CurrentSession
		{
			get
			{
				lock (m_SessionTable.SyncRoot)
				{
					return (WorkSession)m_SessionTable[Thread.CurrentThread];
				}
			}
		}

		public static SessionStatistics[] Statistics
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				foreach (WorkSession value2 in m_SessionTable.Values)
				{
					SessionStatistics value = new SessionStatistics(value2.m_Name, value2.m_StartTime, value2.m_Status, value2.m_StatusDate, value2.m_NetworkSession);
					arrayList.Add(value);
				}
				return (SessionStatistics[])arrayList.ToArray(typeof(SessionStatistics));
			}
		}

		private WorkSession(string _name, int _networksession)
		{
			m_Thread = Thread.CurrentThread;
			if (_name != null && _name != string.Empty)
			{
				m_Name = m_Thread.GetHashCode() + "_" + _name;
			}
			else
			{
				m_Name = m_Thread.GetHashCode().ToString();
			}
			m_NetworkSession = _networksession;
			m_StartTime = DateTime.Now;
		}

		public static void Begin(string _name, int _networksession)
		{
			WorkSession workSession = new WorkSession(_name, _networksession);
			lock (m_SessionTable.SyncRoot)
			{
				if (m_SessionTable.Contains(workSession.m_Thread))
				{
					throw new Exception("already exists session in this thread");
				}
				m_SessionTable.Add(workSession.m_Thread, workSession);
			}
		}

		public static void End()
		{
			lock (m_SessionTable.SyncRoot)
			{
				m_SessionTable.Remove(Thread.CurrentThread);
			}
		}

		public static void WriteStatus(string _status)
		{
			if (ServerConfiguration.IsSessionLogToConsole)
			{
				Console.WriteLine(_status);
			}
			WorkSession currentSession = CurrentSession;
			if (currentSession != null)
			{
				currentSession.m_Status = _status;
				currentSession.m_StatusDate = DateTime.Now;
			}
		}

		public static string GetLastStatus()
		{
			WorkSession currentSession = CurrentSession;
			if (currentSession != null)
			{
				return "[" + currentSession.m_Name + "]" + currentSession.m_Status;
			}
			return string.Empty;
		}

		public static bool Abort(string _name)
		{
			WorkSession workSession = FindSession(_name);
			if (workSession == null)
			{
				return false;
			}
			if (workSession.m_Thread.IsAlive)
			{
				workSession.m_Thread.Abort();
				if (workSession.m_Thread.Join(10000))
				{
					m_SessionTable.Remove(workSession.m_Thread);
					return true;
				}
				return false;
			}
			m_SessionTable.Remove(workSession.m_Thread);
			return true;
		}

		private static WorkSession FindSession(string _name)
		{
			lock (m_SessionTable.SyncRoot)
			{
				foreach (WorkSession value in m_SessionTable.Values)
				{
					if (value.m_Name == _name)
					{
						return value;
					}
				}
				return null;
			}
		}
	}
}
