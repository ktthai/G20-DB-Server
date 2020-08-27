using Mabinogi;
using System;
using System.Collections;
using System.Diagnostics;

namespace Authenticator
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

		private static ArrayList exceptoin_list = new ArrayList();

		public static void ExceptionRaised(Exception _ex)
		{
			ExceptionRaised(_ex, null);
		}

		public static void ExceptionRaised(Exception _ex, object _obj)
		{
			ExceptionRaised(_ex, _obj?.ToString());
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

		public static void ExceptionRaised(UserException _ex)
		{
			lock (exceptoin_list.SyncRoot)
			{
				exceptoin_list.Insert(0, _ex);
			}
			if (Console.Error != null)
			{
				Console.Error.WriteLine(_ex.FullMessage.ToString());
			}
			EventLogger.WriteEventLog(_ex.FullMessage, EventLogEntryType.Error);
		}

		public static Message ToMessage()
		{
			Message message = new Message(0u, 0uL);
			lock (exceptoin_list.SyncRoot)
			{
				message.WriteS32(exceptoin_list.Count);
				foreach (UserException item in exceptoin_list)
				{
					message.WriteS64(item.RaisedTime.Ticks);
					message.WriteString(item.Message);
					message.WriteString(item.FullMessage);
					message.WriteString(item.LastStatus);
				}
				return message;
			}
		}
	}
}
