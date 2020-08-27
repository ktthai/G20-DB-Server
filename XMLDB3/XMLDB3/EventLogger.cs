using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace XMLDB3
{
	public class EventLogger
	{
		private static bool m_AutoClear = false;

		private static bool m_Initialize = false;

		private static EventLog m_EventLog = new EventLog();

		private static readonly int ErrorEventLogFull = 1502;

		public static readonly int MaxMessageSize = 8192;

		private EventLogger()
		{
		}

		public static void WriteEventLog(string _message)
		{
			WriteEventLog(_message, EventLogEntryType.Information);
		}

		public static void WriteEventLog(string _message, EventLogEntryType _type)
		{
			if (_message.Length > MaxMessageSize)
			{
				_message.Substring(0, MaxMessageSize);
			}
			lock (m_EventLog)
			{
				try
				{
					if (!m_Initialize)
					{
						Initialize();
					}
					m_EventLog.WriteEntry((_message.Length > MaxMessageSize) ? _message.Substring(0, MaxMessageSize) : _message, _type);
				}
				catch (Win32Exception ex)
				{
					if (ex.NativeErrorCode == ErrorEventLogFull && m_AutoClear && BackupEventLog())
					{
						m_EventLog.Clear();
						WriteEventLog(_message, _type);
					}
					else
					{
						WriteErrorLog(_message, _type, ex.ToString());
					}
				}
				catch (Exception ex2)
				{
					WriteErrorLog(_message, _type, ex2.ToString());
				}
			}
		}

		public static void SetAutoClear(bool _auto)
		{
			lock (m_EventLog)
			{
				m_AutoClear = _auto;
			}
		}

		private static void Initialize()
		{
			string eventLogSource = ConfigManager.EventLogSource;
			string text;
			if (eventLogSource != null && eventLogSource.Length > 0)
			{
				text = eventLogSource;
			}
			else
			{
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				text = entryAssembly.GetName().Name;
			}
			m_EventLog.Log = text;
			m_EventLog.Source = text;
			if (!EventLog.SourceExists(text))
			{
				EventLog.CreateEventSource(text, text);
			}
			m_Initialize = true;
		}

		private static void WriteErrorLog(string _message, EventLogEntryType _type, string _error)
		{
			string path = "./EventLogError.log";
			FileStream fileStream = null;
			try
			{
				fileStream = (File.Exists(path) ? File.Open(path, FileMode.Append, FileAccess.Write) : File.Create(path));
				StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Unicode);
				streamWriter.WriteLine("{0};{1};{2};{3}", DateTime.Now, _type, _message, _error);
				streamWriter.Close();
			}
			catch
			{
			}
		}

		private static bool BackupEventLog()
		{
			string path = "./" + m_EventLog.Log + "_" + DateTime.Now.ToString().Replace(":", "-") + ".log";
			FileStream fileStream = null;
			try
			{
				fileStream = (File.Exists(path) ? File.Open(path, FileMode.Append, FileAccess.Write) : File.Create(path));
				StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Unicode);
				foreach (EventLogEntry entry in m_EventLog.Entries)
				{
					streamWriter.WriteLine("{0};{1};{2}", entry.TimeGenerated, entry.EntryType, entry.Message);
				}
				streamWriter.Close();
				return true;
			}
			catch (Exception ex)
			{
				WriteErrorLog("Fail to backup event log", EventLogEntryType.Error, ex.ToString());
				return false;
			}
		}
	}
}
