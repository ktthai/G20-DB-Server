using Mabinogi.Network;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace XMLDB3.ItemMarket
{
	public class ItemMarketClient
	{
		public delegate void ClientEvent(object _arg, int _result);

		private class AsynchSendObject
		{
			private ItemMarketClient m_Instance;

			private Socket m_WorkSocket;

			private byte[] m_SendBuffer;

			private int m_SendBufSize;

			public AsynchSendObject(ItemMarketClient _Instance, byte[] _buffer, int _buffersize)
			{
				m_Instance = _Instance;
				m_WorkSocket = m_Instance.m_ClientSocket;
				m_SendBuffer = _buffer;
				m_SendBufSize = _buffersize;
			}

			private bool IsValid()
			{
				if (m_Instance != null)
				{
					return true;
				}
				return false;
			}

			public void Send(IAsyncResult ar)
			{
				if (IsValid())
				{
					lock (m_Instance)
					{
						if (m_WorkSocket != null)
						{
							try
							{
								m_WorkSocket.EndSend(ar);
							}
							catch (ObjectDisposedException)
							{
							}
							catch (SocketException ex2)
							{
								if (m_WorkSocket.Connected)
								{
									throw ex2;
								}
							}
						}
					}
				}
			}
		}

		private class AsynchReceiveObject
		{
			private ItemMarketClient m_Instance;

			private Socket m_WorkSocket;

			private byte[] m_Buffer = new byte[1024];

			private NetworkBuffer m_DataBuffer = new NetworkBuffer();

			public AsynchReceiveObject(ItemMarketClient _Instance)
			{
				m_Instance = _Instance;
				m_WorkSocket = m_Instance.m_ClientSocket;
			}

			public void Receive()
			{
				try
				{
					m_WorkSocket.BeginReceive(m_Buffer, 0, m_Buffer.Length, SocketFlags.None, ReceiveCallBack, this);
				}
				catch (SocketException ex)
				{
					if (m_WorkSocket.Connected)
					{
						throw ex;
					}
					m_Instance.Stop();
				}
				catch (ObjectDisposedException)
				{
				}
			}

			public void Receive(IAsyncResult ar)
			{
				lock (m_Instance)
				{
					try
					{
						int num = m_WorkSocket.EndReceive(ar);
						if (num > 0)
						{
							m_DataBuffer.AddBuffer(m_Buffer, num);
							for (int num2 = m_Instance.OnReceive(m_DataBuffer.GetBuffer(), m_DataBuffer.GetBufSize()); num2 != 0; num2 = m_Instance.OnReceive(m_DataBuffer.GetBuffer(), m_DataBuffer.GetBufSize()))
							{
								m_DataBuffer.PopBuffer(num2);
							}
							m_WorkSocket.BeginReceive(m_Buffer, 0, m_Buffer.Length, SocketFlags.None, ReceiveCallBack, this);
						}
						else
						{
							m_Instance.Stop();
						}
					}
					catch (SocketException ex)
					{
						if (m_WorkSocket.Connected)
						{
							throw ex;
						}
						m_Instance.Stop();
					}
					catch (ObjectDisposedException)
					{
					}
				}
			}
		}

		protected enum ConnectionState
		{
			NotInitialized,
			JustConnected,
			Initialized
		}

		private class ForceTerminateException : Exception
		{
		}

		protected Socket m_ClientSocket;

		protected byte m_ReadCount;

		protected byte m_WriteCount;

		protected ConnectionState m_State;

		private IPAddress m_TargetIP;

		private int m_TargetPort;

		protected volatile bool m_Running;

		private bool m_Closing;

		private Mutex m_CloseMutex = new Mutex();

		public ClientEvent OnInitialized;

		public ClientEvent OnClosed;

		public ClientEvent OnConnectionFailed;

		public ClientEvent OnInitializeFailed;

		public IPAddress Address => m_TargetIP;

		public int Port => m_TargetPort;

		public bool ConnectIP(string _Address, int _Port)
		{
			IPAddress iP = IPAddress.Parse(_Address);
			return Connect(iP, _Port);
		}

		protected bool Connect(IPAddress _IP, int _Port)
		{
			try
			{
				lock (this)
				{
					if (m_Running)
					{
						throw new Exception("already connected to " + m_TargetIP.ToString() + " : " + m_TargetPort);
					}
					EndPoint remoteEP = new IPEndPoint(_IP, _Port);
					m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					m_ClientSocket.Connect(remoteEP);
					m_Running = true;
					m_State = ConnectionState.JustConnected;
					m_TargetIP = _IP;
					m_TargetPort = _Port;
					m_WriteCount = 0;
					m_ReadCount = 0;
					AsynchReceiveObject asynchReceiveObject = new AsynchReceiveObject(this);
					asynchReceiveObject.Receive();
					OnConnect();
					return true;
				}
			}
			catch (SocketException ex)
			{
				if (m_Running)
				{
					throw ex;
				}
				m_ClientSocket.Close();
				m_ClientSocket = null;
				if (OnClosed != null)
				{
					OnClosed(this, 0);
				}
				return false;
			}
		}

		public void Stop()
		{
			lock (m_CloseMutex)
			{
				if (m_Closing)
				{
					return;
				}
				m_Closing = true;
			}
			try
			{
				ConnectionState state = m_State;
				lock (this)
				{
					m_Running = false;
					m_State = ConnectionState.NotInitialized;
					if (m_ClientSocket != null)
					{
						if (m_ClientSocket.Connected)
						{
							m_ClientSocket.Shutdown(SocketShutdown.Both);
							m_ClientSocket.Close();
						}
						m_ClientSocket = null;
					}
				}
				switch (state)
				{
				case ConnectionState.JustConnected:
					if (OnConnectionFailed != null)
					{
						OnConnectionFailed(this, 0);
					}
					break;
				case ConnectionState.Initialized:
					if (OnClosed != null)
					{
						OnClosed(this, 0);
					}
					break;
				}
			}
			finally
			{
				lock (m_CloseMutex)
				{
					m_Closing = false;
				}
			}
		}

		public void Send(byte[] _Buffer, int _Length)
		{
			if (m_Running)
			{
				lock (this)
				{
					try
					{
						AsynchSendObject state = new AsynchSendObject(this, _Buffer, _Buffer.Length);
						m_ClientSocket.BeginSend(_Buffer, 0, _Buffer.Length, SocketFlags.None, SendCallBack, state);
					}
					catch (SocketException ex)
					{
						if (m_ClientSocket.Connected)
						{
							throw ex;
						}
						Stop();
					}
					catch (ObjectDisposedException)
					{
					}
				}
			}
		}

		protected virtual int OnReceive(byte[] _buffer, int _length)
		{
			return 0;
		}

		protected virtual void OnConnect()
		{
		}

		private static void ReceiveCallBack(IAsyncResult ar)
		{
			if (ar != null)
			{
				((AsynchReceiveObject)ar.AsyncState)?.Receive(ar);
			}
		}

		private static void SendCallBack(IAsyncResult ar)
		{
			if (ar != null)
			{
				((AsynchSendObject)ar.AsyncState)?.Send(ar);
			}
		}
	}
}
