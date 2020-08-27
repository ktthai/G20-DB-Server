using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace Mabinogi.Network
{
	public class ServerHandler
	{
		private class AsynchAcceptObject
		{
			private int m_ClientId;

			private ServerHandler m_Handler;

			public AsynchAcceptObject(ServerHandler _handler)
			{
				m_Handler = _handler;
				m_ClientId = 0;
			}

			public bool IsValid()
			{
				if (m_Handler != null && m_Handler.m_ServerSocket != null && m_Handler.m_ClientTable != null && m_Handler.m_Running)
				{
					return true;
				}
				return false;
			}

			public void AcceptClient(IAsyncResult ar)
			{
				try
				{
					lock (m_Handler)
					{
						if (IsValid())
						{
							Socket socket = m_Handler.m_ServerSocket.EndAccept(ar);
							if (socket == null)
							{
								throw new Exception("EndAccept() function returns null socket");
							}
							ServerCommInstance serverCommInstance = new ServerCommInstance(++m_ClientId, socket, m_Handler.Connect_Evented, m_Handler.Close_Evented, m_Handler.Recv_Evented, m_Handler.RecvMsg_Evented, m_Handler.Send_Evented, m_Handler.SendMsg_Evented, m_Handler.Exception_Rasied);
							if (m_Handler.m_StatisticsInfo != null)
							{
								m_Handler.m_StatisticsInfo.Connected();
							}
							lock (m_Handler.m_ClientTable.SyncRoot)
							{
								m_Handler.m_ClientTable.Add(serverCommInstance.ID, serverCommInstance);
							}
							try
							{
								serverCommInstance.Start();
							}
							catch (Exception ex)
							{
								Console.WriteLine(ex.ToString());
								lock (m_Handler.m_ClientTable.SyncRoot)
								{
									m_Handler.m_ClientTable.Remove(serverCommInstance.ID);
									serverCommInstance.Stop();
								}
							}
							m_Handler.m_ServerSocket.BeginAccept(AcceptCallBack, this);
						}
					}
				}
				catch (SocketException ex2)
				{
					if (ex2.ErrorCode != 10004)
					{
						throw ex2;
					}
					m_Handler.Stop();
				}
				catch (Exception ex3)
				{
					Console.WriteLine(ex3.ToString());
					Console.WriteLine("여기에서 에러 메시지를 처리하여야 한다");
				}
			}
		}

		private Socket m_ServerSocket;

		private Hashtable m_ClientTable;

		private bool m_Running;

		private IPAddress m_BindingHost;

		private int m_Port;

		private ServerHandlerInfo m_StatisticsInfo = new ServerHandlerInfo();

		public ServerHandlerInfo StatisticInfo => m_StatisticsInfo;

		public ServerInstanceInfo[] InstanceInfo
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				lock (m_ClientTable.SyncRoot)
				{
					foreach (ServerCommInstance value in m_ClientTable.Values)
					{
						arrayList.Add(value.StatisticInfo);
					}
				}
				return (ServerInstanceInfo[])arrayList.ToArray(typeof(ServerInstanceInfo));
			}
		}

		public void Start(int _Port)
		{
			Start(IPAddress.Any, _Port);
		}

		public void StartLocal(int _Port)
		{
			Start(null, _Port);
		}

		private void Start(IPAddress _ipAddress, int _Port)
		{
			try
			{
				lock (this)
				{
					if (m_Running)
					{
						throw new Exception("already server running : port number " + _Port);
					}
					m_Running = true;
					if (_ipAddress == null)
					{
						IPHostEntry hostEntry = Dns.GetHostEntry("");
						IPAddress[] addressList = hostEntry.AddressList;
						foreach (IPAddress iPAddress in addressList)
						{
							if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
							{
								_ipAddress = iPAddress;
								break;
							}
						}
					}
					if (_ipAddress == null)
					{
						throw new Exception("IPv4 주소를 찾을 수 없습니다.");
					}
					EndPoint localEP = new IPEndPoint(_ipAddress, _Port);
					m_ServerSocket = new Socket(_ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					m_ServerSocket.Bind(localEP);
					m_ServerSocket.Listen(10);
					m_BindingHost = _ipAddress;
					m_Port = _Port;
					m_ClientTable = new Hashtable();
					OnInitialized();
					AsynchAcceptObject state = new AsynchAcceptObject(this);
					m_ServerSocket.BeginAccept(AcceptCallBack, state);
				}
			}
			catch (Exception ex)
			{
				Stop();
				throw ex;
			}
		}

		public void Stop()
		{
			lock (this)
			{
				m_Running = false;
				if (m_ClientTable != null)
				{
					ArrayList arrayList = new ArrayList();
					lock (m_ClientTable.SyncRoot)
					{
						foreach (ServerCommInstance value in m_ClientTable.Values)
						{
							arrayList.Add(value);
						}
						m_ClientTable.Clear();
					}
					foreach (ServerCommInstance item in arrayList)
					{
						item.Stop();
						m_StatisticsInfo.Closed();
						OnClose(item.ID);
					}
					arrayList.Clear();
				}
				if (m_ServerSocket != null)
				{
					if (m_ServerSocket.Connected)
					{
						m_ServerSocket.Shutdown(SocketShutdown.Both);
						m_ServerSocket.Close();
					}
					m_ServerSocket = null;
				}
			}
		}

		public void SendMessage(int _Id, Message _Message)
		{
			if (m_Running)
			{
				FindInstance(_Id)?.SendMessage(_Message);
			}
		}

		public IPAddress GetClientIP(int _Id)
		{
			if (m_Running)
			{
				ServerCommInstance serverCommInstance = FindInstance(_Id);
				if (serverCommInstance != null)
				{
					return serverCommInstance.StatisticInfo.TargetAddress;
				}
			}
			return null;
		}

		public void DestroyClient(int _Id)
		{
			if (m_ClientTable != null)
			{
				ServerCommInstance serverCommInstance = FindInstance(_Id);
				if (serverCommInstance != null)
				{
					lock (m_ClientTable.SyncRoot)
					{
						m_ClientTable.Remove(_Id);
					}
					serverCommInstance.Stop();
					m_StatisticsInfo.Closed();
					OnClose(_Id);
				}
			}
		}

		private ServerCommInstance FindInstance(int _Id)
		{
			lock (m_ClientTable.SyncRoot)
			{
				if (m_ClientTable.Contains(_Id))
				{
					return (ServerCommInstance)m_ClientTable[_Id];
				}
				return null;
			}
		}

		protected virtual void OnInitialized()
		{
		}

		protected virtual void OnConnect(int _ClientId)
		{
		}

		protected virtual void OnReceive(int _ClientId, Message _Message)
		{
		}

		protected virtual void OnClose(int _ClientId)
		{
		}

		protected virtual void OnExceptionRaised(int _clientId, Exception _ex)
		{
		}

		private void Connect_Evented(int _id)
		{
			m_StatisticsInfo.ConnectionConfirmed();
			OnConnect(_id);
		}

		private void Close_Evented(int _id)
		{
			DestroyClient(_id);
		}

		private void RecvMsg_Evented(int _id, Message _msg)
		{
			m_StatisticsInfo.MsgReceived(_msg);
			OnReceive(_id, _msg);
		}

		private void Recv_Evented(int _id, int _size, byte[] _buffer)
		{
			m_StatisticsInfo.DataReceived(_size);
		}

		private void Send_Evented(int _id, int _size, byte[] _buffer)
		{
			m_StatisticsInfo.DataSended(_size);
		}

		private void SendMsg_Evented(int _id, Message _msg)
		{
			m_StatisticsInfo.MsgSend(_msg);
		}

		private void Exception_Rasied(int _id, Exception _ex)
		{
			OnExceptionRaised(_id, _ex);
		}

		private static void AcceptCallBack(IAsyncResult ar)
		{
			if (ar != null)
			{
				AsynchAcceptObject asynchAcceptObject = (AsynchAcceptObject)ar.AsyncState;
				if (asynchAcceptObject != null && asynchAcceptObject.IsValid())
				{
					asynchAcceptObject.AcceptClient(ar);
				}
			}
		}
	}
}
