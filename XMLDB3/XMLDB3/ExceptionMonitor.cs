using Mabinogi;
using System;
using System.Collections;
using System.Diagnostics;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class ExceptionMonitor
	{
		public class UserException
		{
			private string exMessage;

			private string fullExMessage;

			private string userData;

			private DateTime raisedTime;

			private string lastStatus;

			public string Message => exMessage;

			public string FullMessage => fullExMessage;

			public DateTime RaisedTime => raisedTime;

			public string LastStatus => lastStatus;

			public UserException(Exception _ex, string _msg)
			{
				exMessage = _ex.Message;
				userData = _msg;
				raisedTime = DateTime.Now;
				lastStatus = WorkSession.GetLastStatus();
				fullExMessage = "[" + ((userData != null) ? userData : "N/A") + "]\r\n" + _ex.ToString();
			}
		}

		private static ArrayList exceptionList = new ArrayList();

		public static void Clear()
		{
			lock (exceptionList.SyncRoot)
			{
				exceptionList.Clear();
				exceptionList.TrimToSize();
			}
		}

		public static void ExceptionRaised(Exception _ex)
		{
			ExceptionRaised(_ex, null);
		}

		public static void ExceptionRaised(Exception _ex, object _obj)
		{
			ExceptionRaised(_ex, _obj?.ToString());
		}

		private static string MakeSqlExceptionInfo(SimpleSqlException _ex)
		{
			// TODO: add procedure and line number to SimpleSqlExecption
			return string.Format("Error number {0}: {1}", _ex.Number.ToString() , _ex.Message);
		}

		public static void ExceptionRaised(Exception _ex, string _msg)
		{
			UserException ex = new UserException(_ex, _msg);
			ExceptionRaised(ex);
		}

		public static void ExceptionRaised(Exception _ex, object _msg1, object _msg2)
		{
			ExceptionRaised(_ex, ((_msg1 == null) ? "N/A" : _msg1.ToString()) + "][" + ((_msg2 == null) ? "N/A" : _msg2.ToString()));
		}

		public static void ExceptionRaised(Exception _ex, object _msg1, object _msg2, object _msg3)
		{
			ExceptionRaised(_ex, ((_msg1 == null) ? "N/A" : _msg1.ToString()) + "][" + ((_msg2 == null) ? "N/A" : _msg2.ToString()) + "][" + ((_msg3 == null) ? "N/A" : _msg3.ToString()));
		}

		public static void ExceptionRaised(Exception _ex, string _msg1, string _msg2)
		{
			ExceptionRaised(_ex, ((_msg1 == null) ? "N/A" : _msg1) + "][" + ((_msg2 == null) ? "N/A" : _msg2));
		}

		public static void ExceptionRaised(Exception _ex, string _msg1, string _msg2, string _msg3)
		{
			ExceptionRaised(_ex, ((_msg1 == null) ? "N/A" : _msg1) + "][" + ((_msg2 == null) ? "N/A" : _msg2) + "][" + ((_msg3 == null) ? "N/A" : _msg3));
		}

		internal static void ExceptionRaised(SimpleSqlException _ex)
		{
			ExceptionRaised((Exception)_ex, MakeSqlExceptionInfo(_ex));
		}

		public static void ExceptionRaised(SimpleSqlException _ex, string _msg1)
		{
			ExceptionRaised((Exception)_ex, MakeSqlExceptionInfo(_ex), _msg1);
		}

		public static void ExceptionRaised(SimpleSqlException _ex, object _obj)
		{
			ExceptionRaised(_ex, _obj?.ToString());
		}

		public static void ExceptionRaised(SimpleSqlException _ex, object _obj1, object _obj2)
		{
			ExceptionRaised(_ex, ((_obj1 == null) ? "N/A" : _obj1.ToString()) + "][" + ((_obj2 == null) ? "N/A" : _obj2.ToString()));
		}

		public static void ExceptionRaised(SimpleSqlException _ex, object _obj1, object _obj2, object _obj3)
		{
			ExceptionRaised(_ex, ((_obj1 == null) ? "N/A" : _obj1.ToString()) + "][" + ((_obj2 == null) ? "N/A" : _obj2.ToString()) + "][" + ((_obj3 == null) ? "N/A" : _obj3.ToString()));
		}

		public static void ExceptionRaised(SimpleSqlException _ex, string _msg1, string _msg2)
		{
			ExceptionRaised(_ex, ((_msg1 == null) ? "N/A" : _msg1) + "][" + ((_msg2 == null) ? "N/A" : _msg2));
		}

		public static void ExceptionRaised(SimpleSqlException _ex, string _msg1, string _msg2, string _msg3)
		{
			ExceptionRaised(_ex, ((_msg1 == null) ? "N/A" : _msg1) + "][" + ((_msg2 == null) ? "N/A" : _msg2) + "][" + ((_msg3 == null) ? "N/A" : _msg3));
		}

		private static void ExceptionRaised(UserException _ex)
		{
			lock (exceptionList.SyncRoot)
			{
				exceptionList.Insert(0, _ex);
			}
			if (Console.Error != null)
			{
				Console.Error.WriteLine(_ex.FullMessage.ToString());
			}
			EventLogger.WriteEventLog(_ex.FullMessage, EventLogEntryType.Error);
		}

		public static Message ToMessage(int _startIdx, int _count)
		{
			Message message = new Message(0u, 0uL);
			lock (exceptionList.SyncRoot)
			{
				int num = (_startIdx + _count <= exceptionList.Count) ? _count : (exceptionList.Count - _startIdx);
				message.WriteS32(exceptionList.Count);
				if (num <= 0)
				{
					message.WriteS32(0);
					return message;
				}
				message.WriteS32(num);
				for (int i = _startIdx; i < _startIdx + num; i++)
				{
					UserException ex = (UserException)exceptionList[i];
					message.WriteS64(ex.RaisedTime.Ticks);
					message.WriteString(ex.Message);
					message.WriteString(ex.FullMessage);
					message.WriteString(ex.LastStatus);
				}
				return message;
			}
		}
	}
}
