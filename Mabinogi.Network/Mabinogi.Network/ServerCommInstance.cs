using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Mabinogi.Network
{
	internal class ServerCommInstance
	{
		private class AsynchSendObject
		{
			public Socket WorkSocket;

			public int SendBufSize;

			public byte[] SendBuffer;
		}

		private class AsynchReceiveObject
		{
			private ServerCommInstance m_Instance;

			private Socket m_WorkSocket;

			private byte[] m_Buffer = new byte[8096];

			private NetworkBuffer m_DataBuffer = new NetworkBuffer();

			public AsynchReceiveObject(ServerCommInstance _Instance)
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
					m_Instance.SelfTermiante();
				}
				catch (ObjectDisposedException)
				{
				}
			}

			public void Receive(IAsyncResult ar)
			{
				try
				{
					lock (m_Instance)
					{
						int num = m_WorkSocket.EndReceive(ar);
						if (num > 0)
						{
							m_Instance.m_StatisticsInfo.DataReceived(num);
							if (m_Instance.recv_event != null)
							{
								m_Instance.recv_event(m_Instance.m_Id, num, m_Buffer);
							}
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
									Message msg = readPacket.ToMessage();
									m_Instance.m_StatisticsInfo.MsgReceived(msg);
									if (m_Instance.rcvmsg_event != null)
									{
										m_Instance.rcvmsg_event(m_Instance.m_Id, msg);
									}
								}
							}
							Receive();
						}
						else
						{
							m_Instance.SelfTermiante();
						}
					}
				}
				catch (SocketException ex)
				{
					if (ex.SocketErrorCode != SocketError.IOPending && ex.SocketErrorCode != SocketError.WouldBlock)
					{
						m_Instance.SelfTermiante();
					}
				}
				catch (ObjectDisposedException)
				{
				}
			}
		}

		protected enum ServerState
		{
			JustConnected,
			ConnectionConfirmed,
			NotConnected
		}

		protected Socket m_ClientSocket;

		protected int m_Id;

		protected bool m_Running;

		private bool m_Closing;

		private Mutex m_CloseMutex = new Mutex();

		private ServerInstanceInfo m_StatisticsInfo;

		private ServerConnectEvent connect_event;

		private ServerCloseEvent close_event;

		private ServerReceiveEvent recv_event;

		private ServerReceiveMsgEvent rcvmsg_event;

		private ServerSendEvent send_event;

		private ServerSendMsgEvent sendmsg_event;

		private ServerExceptionEvent exception_event;

		private XOREncription m_Encript;

		private XOREncription m_Decript;

		protected byte m_ReadCount;

		protected byte m_WriteCount;

		protected Thread ClientThread;

		protected ServerState m_State;

		public int ID => m_Id;

		public ServerInstanceInfo StatisticInfo => m_StatisticsInfo;

		public ServerCommInstance(int _Id, Socket _Socket, ServerConnectEvent _conect_event, ServerCloseEvent _close_event, ServerReceiveEvent _recv_event, ServerReceiveMsgEvent _recvmsg_event, ServerSendEvent _send_event, ServerSendMsgEvent _sendmsg_event, ServerExceptionEvent _exception_event)
		{
			if (_Socket == null)
			{
				throw new Exception("ServerCommInstance() 에 주어진 소켓이 null 이다");
			}
			if (!_Socket.Connected)
			{
				throw new Exception("ServerCommInstance() 에 주어진 소켓이 null 이다");
			}
			m_Running = false;
			m_Closing = false;
			m_Id = _Id;
			m_ClientSocket = _Socket;
			m_ReadCount = 0;
			m_WriteCount = 0;
			m_Encript = null;
			m_Decript = null;
			m_State = ServerState.NotConnected;
			ClientThread = null;
			connect_event = _conect_event;
			close_event = _close_event;
			recv_event = _recv_event;
			rcvmsg_event = _recvmsg_event;
			send_event = _send_event;
			sendmsg_event = _sendmsg_event;
			exception_event = _exception_event;
			IPEndPoint iPEndPoint = (IPEndPoint)_Socket.RemoteEndPoint;
			m_StatisticsInfo = new ServerInstanceInfo(_Id, iPEndPoint.Address, iPEndPoint.Port);
		}

		public void Start()
		{
			lock (this)
			{
				if (m_Running)
				{
					throw new Exception("이미 인스턴스가 동작되고 있습니다");
				}
				if (!m_ClientSocket.Connected)
				{
					throw new Exception("클라이언트와 연결이 종료되었습니다");
				}
				m_Running = true;
				m_ReadCount = 0;
				m_WriteCount = 0;
				m_Encript = null;
				m_Decript = null;
				m_State = ServerState.JustConnected;
				AsynchReceiveObject asynchReceiveObject = new AsynchReceiveObject(this);
				asynchReceiveObject.Receive();
				SendVersion();
			}
		}

		private static void ReceiveCallBack(IAsyncResult ar)
		{
			if (ar != null)
			{
				((AsynchReceiveObject)ar.AsyncState)?.Receive(ar);
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
				lock (this)
				{
					m_Running = false;
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
				m_State = ServerState.ConnectionConfirmed;
				if (connect_event != null)
				{
					connect_event(ID);
				}
				return true;
			default:
				return false;
			}
		}

		protected void SelfTermiante()
		{
			Stop();
			if (close_event != null)
			{
				close_event(ID);
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
						if (m_ClientSocket != null)
						{
							byte[] array = writePacket.ToBuffer();
							if (_Message.Log)
							{
								string str = "Logged character read: " + BitConverter.ToString(array);
								Console.WriteLine(str);
								Console.WriteLine();
							}
							AsynchSendObject asynchSendObject = new AsynchSendObject();
							asynchSendObject.WorkSocket = m_ClientSocket;
							asynchSendObject.SendBufSize = array.Length;
							asynchSendObject.SendBuffer = array;
							m_ClientSocket.BeginSend(array, 0, array.Length, SocketFlags.None, SendCallBack, asynchSendObject);
							m_StatisticsInfo.MsgSend(_Message);
							if (sendmsg_event != null)
							{
								sendmsg_event(m_Id, _Message);
							}
						}
					}
					catch (SocketException ex)
					{
						exception_event(ID, ex);
						if (m_ClientSocket.Connected)
						{
							throw ex;
						}
						SelfTermiante();
					}
					catch (ObjectDisposedException ex2)
					{
						exception_event(ID, ex2);
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
						AsynchSendObject asynchSendObject = new AsynchSendObject();
						asynchSendObject.WorkSocket = m_ClientSocket;
						asynchSendObject.SendBufSize = array.Length;
						asynchSendObject.SendBuffer = array;
						m_ClientSocket.BeginSend(array, 0, array.Length, SocketFlags.None, SendCallBack, asynchSendObject);
					}
					catch (SocketException ex)
					{
						exception_event(ID, ex);
						if (m_ClientSocket.Connected)
						{
							throw ex;
						}
						SelfTermiante();
					}
					catch (ObjectDisposedException ex2)
					{
						exception_event(ID, ex2);
					}
				}
			}
		}

		public void SendCallBack(IAsyncResult ar)
		{
			AsynchSendObject asynchSendObject = (AsynchSendObject)ar.AsyncState;
			if (asynchSendObject != null && asynchSendObject.WorkSocket != null)
			{
				lock (asynchSendObject.WorkSocket)
				{
					try
					{
						if (asynchSendObject.WorkSocket.Connected)
						{
							int size = asynchSendObject.WorkSocket.EndSend(ar);
							m_StatisticsInfo.DataSended(size);
							if (send_event != null)
							{
								send_event(ID, size, asynchSendObject.SendBuffer);
							}
						}
					}
					catch (ObjectDisposedException ex)
					{
						exception_event(ID, ex);
					}
					catch (SocketException ex2)
					{
						exception_event(ID, ex2);
						if (asynchSendObject.WorkSocket.Connected)
						{
							throw ex2;
						}
						SelfTermiante();
					}
				}
			}
		}

		private byte IncreaseAndGetWriteCount()
		{
			m_WriteCount++;
			return m_WriteCount;
		}
	}
}
