using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Mabinogi.Network
{
	public class ClientHandler
	{
		private class AsynchSendObject
		{
			private ClientHandler m_Instance;

			private Socket m_WorkSocket;

			private byte[] m_SendBuffer;

			private int m_SendBufSize;

			public AsynchSendObject(ClientHandler _Instance, byte[] _buffer, int _buffersize)
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
								int size = m_WorkSocket.EndSend(ar);
								m_Instance.m_StatisticsInfo.DataSended(size);
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
			private ClientHandler m_Instance;

			private Socket m_WorkSocket;

			private byte[] m_Buffer = new byte[1024];

			private NetworkBuffer m_DataBuffer = new NetworkBuffer();

			public AsynchReceiveObject(ClientHandler _Instance)
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
							m_Instance.m_StatisticsInfo.DataReceived(num);
							m_DataBuffer.AddBuffer(m_Buffer, num);
							ReadPacket readPacket;
							while ((readPacket = ReadPacket.Build(m_DataBuffer.GetBuffer(), 0, m_DataBuffer.GetBufSize(), m_Instance.m_Decript)) != null)
							{
								m_DataBuffer.PopBuffer(readPacket.PacketTotalSize);
								if (readPacket.IsSystemPacket())
								{
									if (!m_Instance.SystemProcedure(readPacket))
									{
										throw new Exception("system procedure error");
									}
								}
								else
								{
									Message message = readPacket.ToMessage();
									m_Instance.m_StatisticsInfo.MsgReceived(message);
									m_Instance.OnReceive(message);
								}
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
			JustConnected,
			VersionMatching,
			ConnectionConfirmed,
			NotInitialized
		}

		private class ForceTerminateException : Exception
		{
		}

		protected Socket m_ClientSocket;

		private XOREncription m_Encript;

		private XOREncription m_Decript;

		protected byte m_ReadCount;

		protected byte m_WriteCount;

		protected ConnectionState m_State = ConnectionState.NotInitialized;

		private IPAddress m_TargetIP;

		private int m_TargetPort;

		protected volatile bool m_Running;

		private ClientHandlerInfo m_StatisticsInfo = new ClientHandlerInfo();

		private bool m_Closing;

		private Mutex m_CloseMutex = new Mutex();

		public IPAddress Address => m_TargetIP;

		public int Port => m_TargetPort;

		public ClientHandlerInfo StatisticInfo => m_StatisticsInfo;

		public bool ConnectIP(string _Address, int _Port)
		{
			IPAddress iP = IPAddress.Parse(_Address);
			return Connect(iP, _Port);
		}

		public bool ConnectDNS(string _Address, int _Port)
		{
			IPAddress iP = Dns.GetHostEntry(_Address).AddressList[0];
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
					m_StatisticsInfo.TryConnect(_IP, _Port);
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
					SendVersion();
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
				OnConnectFail();
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
				case ConnectionState.VersionMatching:
					break;
				case ConnectionState.JustConnected:
					m_StatisticsInfo.ConnectFailed();
					OnConnectFail();
					break;
				case ConnectionState.ConnectionConfirmed:
					m_StatisticsInfo.Closed();
					OnClose();
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

		private bool SystemProcedure(ReadPacket _inPacket)
		{
			switch (_inPacket.Binary.ReadByte())
			{
			case 0:
				if ((double)_inPacket.Binary.ReadSingle() == 2.0)
				{
					SendEncription();
					return true;
				}
				return false;
			case 1:
			{
				int network = _inPacket.Binary.ReadInt32();
				int network2 = _inPacket.Binary.ReadInt32();
				m_Decript = new XOREncription((uint)IPAddress.NetworkToHostOrder(network2), (uint)IPAddress.NetworkToHostOrder(network));
				SendConnectionConfirm();
				return true;
			}
			case 2:
				m_State = ConnectionState.ConnectionConfirmed;
				m_StatisticsInfo.Connected();
				OnConnect();
				return true;
			default:
				return false;
			}
		}

		private void SendVersion()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write((byte)0);
			binaryWriter.Write(2f);
			Send(memoryStream.GetBuffer(), (int)memoryStream.Position, _bSystemMsg: true);
		}

		private void SendEncription()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			m_Encript = new XOREncription(64u, 0u);
			binaryWriter.Write((byte)1);
			binaryWriter.Write((uint)IPAddress.HostToNetworkOrder((int)m_Encript.GetSeed()));
			binaryWriter.Write((uint)IPAddress.HostToNetworkOrder((int)m_Encript.GetSize()));
			Send(memoryStream.GetBuffer(), (int)memoryStream.Position, _bSystemMsg: true);
		}

		private void SendConnectionConfirm()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write((byte)2);
			Send(memoryStream.GetBuffer(), (int)memoryStream.Position, _bSystemMsg: true);
		}

		public void SendMessage(Message _Message)
		{
			if (m_Running)
			{
				WritePacket writePacket = WritePacket.Build(_Message, IncreaseAndGetWriteCount(), _bEncription: false, _bSystemMsg: false, _bUsingChecksum: true, m_Encript);
				lock (this)
				{
					try
					{
						byte[] array = writePacket.ToBuffer();
						AsynchSendObject state = new AsynchSendObject(this, array, array.Length);
						m_ClientSocket.BeginSend(array, 0, array.Length, SocketFlags.None, SendCallBack, state);
						m_StatisticsInfo.MsgSend(_Message);
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

		public void Send(byte[] _Buffer, int _Length, bool _bSystemMsg)
		{
			if (m_Running)
			{
				WritePacket writePacket = WritePacket.Build(_Buffer, _Length, IncreaseAndGetWriteCount(), _bEncription: false, _bSystemMsg, _bUsingChecksum: false, m_Encript);
				lock (this)
				{
					try
					{
						byte[] array = writePacket.ToBuffer();
						AsynchSendObject state = new AsynchSendObject(this, array, array.Length);
						m_ClientSocket.BeginSend(array, 0, array.Length, SocketFlags.None, SendCallBack, state);
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

		private byte IncreaseAndGetWriteCount()
		{
			m_WriteCount++;
			return m_WriteCount;
		}

		protected virtual void OnConnect()
		{
		}

		protected virtual void OnConnectFail()
		{
		}

		protected virtual void OnReceive(Message _Message)
		{
		}

		protected virtual void OnClose()
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
