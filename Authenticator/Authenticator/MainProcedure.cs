using Mabinogi;
using Mabinogi.Network;
using System;

namespace Authenticator
{
	public class MainProcedure : ServerHandler
	{
		private static MainProcedure m_MainServer = new MainProcedure();

		public static ServerInstanceInfo[] ConnectionInfo => m_MainServer.InstanceInfo;

		public static ServerHandlerInfo ServerInfo => m_MainServer.StatisticInfo;

		public static void ServerStart(int port)
		{
			m_MainServer.Start(port);
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
				if (remoteFunction != null)
				{
					try
					{
						WorkSession.Begin(remoteFunction.Name, _id);
						remoteFunction.Process();
					}
					finally
					{
						WorkSession.End();
					}
				}
			}
			catch (Exception ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
			}
		}

		private string MakeJobName(Message _msg)
		{
			return "test job";
		}
	}
}
