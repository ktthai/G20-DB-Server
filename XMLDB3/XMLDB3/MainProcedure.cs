using Mabinogi;
using Mabinogi.Network;
using System;

namespace XMLDB3
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
			WorkSession.WriteStatus("ServerSend(" + _id + ", msg) : 함수에 진입하였습니다");
			m_MainServer.SendMessage(_id, _msg);
		}

		protected override void OnConnect(int _id)
		{
			if (CommandRedirection.Enabled)
			{
				CommandRedirection.CreateClient(_id);
			}
		}

		protected override void OnClose(int _id)
		{
			if (CommandRedirection.Enabled)
			{
				CommandRedirection.DestroyClient(_id);
			}
		}

		protected override void OnExceptionRaised(int _id, Exception _ex)
		{
			ExceptionMonitor.ExceptionRaised(_ex, _id);
		}

		protected override void OnReceive(int _id, Message _msg)
		{
			BasicCommand basicCommand = null;
			try
			{
				basicCommand = BasicCommand.Parse(_id, _msg);
				if (basicCommand != null)
				{
					try
					{
						if (basicCommand.IsPrimeCommand)
						{
							ProcessManager.AddPrimeCommand(basicCommand);
						}
						else
						{
							ProcessManager.AddCommand(basicCommand);
						}
					}
					catch (Exception ex)
					{
						basicCommand.OnError();
						throw ex;
					}
				}
			}
			catch (Exception ex2)
			{
				ExceptionMonitor.ExceptionRaised(ex2, basicCommand);
			}
			try
			{
				if (CommandRedirection.Enabled)
				{
					CommandRedirection.SendMessage(_id, _msg);
				}
			}
			catch
			{
			}
		}
	}
}
