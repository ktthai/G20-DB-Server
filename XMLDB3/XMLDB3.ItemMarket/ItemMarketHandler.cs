using Mabinogi;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace XMLDB3.ItemMarket
{
	public class ItemMarketHandler : ItemMarketClient
	{
		private const long heartbeatPeriod = 120000L;

		private int packetNo;

		private QueryManager queryManager = new QueryManager();

		private int gameNo;

		private int serverNo;

		private string name = string.Empty;

		private bool bHeartBeatReceived;

		private Timer timerReconnect;

		private Timer timerHeartbeat;

		public int PacketNo => Interlocked.Increment(ref packetNo);

		public string Name => name;

		public bool IsWorking => m_State == ConnectionState.Initialized;

		public ItemMarketHandler(string _name, int _gameNo, int _serverNo)
		{
			name = _name;
			gameNo = _gameNo;
			serverNo = _serverNo;
			bHeartBeatReceived = true;
			timerReconnect = new Timer(ConnectionProcess, this, -1, -1);
			timerHeartbeat = new Timer(CheckHeartbeat, null, -1, -1);
		}

		protected override void OnConnect()
		{
			Console.WriteLine("Item Market Client [{0}]'s Connected.", Name);
			ConnectionProcess(null);
		}

		public bool Send(ItemMarketCommand _command, uint _ID, uint _queryID, uint _targetID, int _clientID)
		{
			if (m_State != ConnectionState.Initialized)
			{
				return false;
			}
			return _Send(_command, _ID, _queryID, _targetID, _clientID);
		}

		private bool _Send(ItemMarketCommand _command, uint _ID, uint _queryID, uint _targetID, int _clientID)
		{
			lock (this)
			{
				int num = 0;
				bool flag = false;
				try
				{
					num = queryManager.PushQuery(_ID, _queryID, _targetID, _clientID);
					flag = true;
					_command.BuildPacket(num);
					Send(_command.Packet, _command.Packet.Length);
				}
				catch (Exception ex)
				{
					ExceptionMonitor.ExceptionRaised(ex);
					if (flag)
					{
						queryManager.PopQuery(num);
					}
					return false;
				}
				return true;
			}
		}

		protected override int OnReceive(byte[] _buffer, int _length)
		{
			BinaryReader binaryReader = null;
			try
			{
				MemoryStream input = new MemoryStream(_buffer, 0, _length);
				binaryReader = new BinaryReader(input);
				ItemMarketResponse itemMarketResponse = ItemMarketResponse.BuildRespose(binaryReader);
				if (itemMarketResponse != null)
				{
					Query query = queryManager.PopQuery(itemMarketResponse.PacketNo);
					if (query != null)
					{
						if (itemMarketResponse.IsSystemMessage)
						{
							itemMarketResponse.Build(binaryReader, null);
							OnSystemMessage(itemMarketResponse);
						}
						else
						{
							Message message = new Message(query.ID, query.targetID);
							message.WriteU32(query.queryID);
							itemMarketResponse.Build(binaryReader, message);
							MainProcedure.ServerSend(query.clientID, message);
						}
					}
					else
					{
						ExceptionMonitor.ExceptionRaised(new Exception("There is no query ID."), itemMarketResponse.PacketNo);
					}
					return itemMarketResponse.PacketLength;
				}
				return 0;
			}
			catch (SocketException ex)
			{
				throw ex;
			}
			catch (Exception ex2)
			{
				ExceptionMonitor.ExceptionRaised(ex2);
				return 0;
			}
			finally
			{
				binaryReader?.Close();
			}
		}

		private void ScheduleHeartbeat()
		{
			timerHeartbeat.Change(120000L, -1L);
		}

		public void ScheduleReconnect()
		{
			timerReconnect.Change(10000, -1);
		}

		private void ConnectionProcess(object state)
		{
			switch (m_State)
			{
			case ConnectionState.JustConnected:
			{
				Console.WriteLine("Item Market Client [{0}]'s sending Initialize Packet.", Name);
				ItemMarketCommand command = new IMInitializeCommand(gameNo, serverNo);
				_Send(command, 0u, 0u, 0u, 0);
				timerReconnect.Change(30000, -1);
				break;
			}
			case ConnectionState.NotInitialized:
				ItemMarketManager.Connect(this);
				break;
			}
		}

		private void CheckHeartbeat(object state)
		{
			if (bHeartBeatReceived)
			{
				ItemMarketCommand command = new IMHeartbeatCommand(gameNo, serverNo);
				Send(command, 0u, 0u, 0u, 0);
				bHeartBeatReceived = false;
				ScheduleHeartbeat();
			}
			else
			{
				ExceptionMonitor.ExceptionRaised(new Exception($"Item Market Client [{Name}] Heartbeat failed."));
				Stop();
			}
		}

		private void OnSystemMessage(ItemMarketResponse response)
		{
			switch (response.Type)
			{
			case IMMessage.Initialize:
				if (response.Result == 1)
				{
					m_State = ConnectionState.Initialized;
					bHeartBeatReceived = true;
					ScheduleHeartbeat();
					if (OnInitialized != null)
					{
						OnInitialized(this, response.Result);
					}
				}
				else
				{
					if (OnInitializeFailed != null)
					{
						OnInitializeFailed(this, response.Result);
					}
					Stop();
				}
				break;
			case IMMessage.Heartbeat:
				if (response.Result == 1 || response.Result == 17)
				{
					bHeartBeatReceived = true;
				}
				break;
			}
		}
	}
}
