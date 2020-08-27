using Mabinogi;
using Mabinogi.Network;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;

namespace XMLDB3
{
	public class Profiler : ServerHandler
	{
		private enum Command
		{
			PROFILER_COMMAND_PING_REQUEST = 100,
			PROFILER_COMMAND_PING_REPLY = 200
		}

		private static Profiler m_MainProfiler = new Profiler();

		private SortedList m_ClientList;

		private Profiler()
		{
			m_ClientList = new SortedList();
		}

		public static void ServerStart(int _port)
		{
			m_MainProfiler.Start(_port);
		}

		public static void ServerStop()
		{
			m_MainProfiler.Stop();
		}

		public static void ServerSend(int _id, Message _msg)
		{
			m_MainProfiler.SendMessage(_id, _msg);
		}

		protected override void OnConnect(int _id)
		{
			ProfilerClient value = new ProfilerClient(_id);
			lock (m_ClientList.SyncRoot)
			{
				m_ClientList.Add(_id, value);
			}
		}

		protected override void OnClose(int _id)
		{
			lock (m_ClientList.SyncRoot)
			{
				m_ClientList.Remove(_id);
			}
		}

		protected override void OnReceive(int _id, Message _msg)
		{
			ProfilerClient profilerClient = null;
			if (m_ClientList.ContainsKey(_id))
			{
				profilerClient = (ProfilerClient)m_ClientList[_id];
				Command iD = (Command)_msg.ID;
				if (iD == Command.PROFILER_COMMAND_PING_REQUEST)
				{
					int data = profilerClient.ResetSendCount();
					Message message = new Message(200u, 0uL);
					message.WriteS32(data);
					SendMessage(_id, message);
				}
			}
		}

		private void BroadCast(Message _msg)
		{
			ProfilerClient[] array = null;
			lock (m_ClientList.SyncRoot)
			{
				if (m_ClientList.Count > 0)
				{
					array = new ProfilerClient[m_ClientList.Count];
					m_ClientList.Values.CopyTo(array, 0);
				}
			}
			if (array != null)
			{
				ProfilerClient[] array2 = array;
				foreach (ProfilerClient profilerClient in array2)
				{
					try
					{
						profilerClient.SendProfile(this, _msg);
					}
					catch (SocketException ex)
					{
						if (ex.ErrorCode == 10055)
						{
							DestroyClient(profilerClient.ID);
						}
						ExceptionMonitor.ExceptionRaised(ex);
					}
					catch (Exception ex2)
					{
						ExceptionMonitor.ExceptionRaised(ex2);
					}
				}
			}
		}

		public static void AddProfileString(string _id)
		{
			Message message = new Message(0u, 0uL);
			message.WriteString(_id);
			message.WriteString((Thread.CurrentThread.Name == null) ? string.Empty : Thread.CurrentThread.Name);
			m_MainProfiler.BroadCast(message);
		}
	}
}
