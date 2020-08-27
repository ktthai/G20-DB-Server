using Mabinogi;
using Mabinogi.Network;
using System;

namespace XMLDB3
{
	public class MonitorProcedure : ServerHandler
	{
		private static MonitorProcedure m_MainServer = new MonitorProcedure();

		public static void ServerStart(int _port)
		{
			m_MainServer.Start(_port);
		}

		public static void ServerStop()
		{
			m_MainServer.Stop();
		}

		public static void CloseConnection(int _id)
		{
			m_MainServer.DestroyClient(_id);
		}

		public static void ServerSend(int _id, Message _msg)
		{
			m_MainServer.SendMessage(_id, _msg);
		}

		protected override void OnConnect(int _id)
		{
		}

		protected override void OnClose(int _id)
		{
		}

		protected override void OnReceive(int _id, Message _msg)
		{
			try
			{
				RemoteFunction remoteFunction = RemoteFunction.Parse(_id, _msg);
				if (remoteFunction == null)
				{
					throw new Exception("connection " + _id + " send invalid message " + _msg.ID);
				}
				try
				{
					WorkSession.Begin(remoteFunction.Name, null, _id);
					remoteFunction.Process();
				}
				finally
				{
					WorkSession.End();
				}
			}
			catch (Exception ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
			}
		}
	}
}
