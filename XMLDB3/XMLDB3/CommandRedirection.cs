using Mabinogi;
using Mabinogi.Network;
using System;
using System.Collections;
using System.Threading;

namespace XMLDB3
{
	public class CommandRedirection
	{
		private static bool bActive = false;

		private static string server = null;

		private static int port = 0;

		private static Hashtable clients = new Hashtable();

		private static Hashtable timers = new Hashtable();

		public static bool Enabled => bActive;

		public static void Init(string _server, int _port)
		{
			Stop();
			if (_server != string.Empty && _port != 0)
			{
				server = _server;
				port = _port;
				clients = new Hashtable();
				timers = new Hashtable();
				bActive = true;
			}
			else
			{
				bActive = false;
			}
		}

		public static void Stop()
		{
			lock (clients.SyncRoot)
			{
				foreach (ClientHandler value in clients.Values)
				{
					value.Stop();
				}
				clients.Clear();
				lock (timers.SyncRoot)
				{
					foreach (Timer value2 in clients.Values)
					{
						value2.Dispose();
					}
					timers.Clear();
				}
				bActive = false;
			}
		}

		public static void CreateClient(int _clientID)
		{
			if (Enabled)
			{
				ClientHandler clientHandler = new ClientHandler();
				try
				{
					if (clientHandler.ConnectIP(server, port))
					{
						lock (clients.SyncRoot)
						{
							if (clients.ContainsKey(_clientID))
							{
								((ClientHandler)clients[_clientID]).Stop();
							}
							clients[_clientID] = clientHandler;
						}
					}
					else
					{
						ReserveConnect(_clientID);
					}
				}
				catch (Exception)
				{
				}
			}
		}

		public static void DestroyClient(int _clientID)
		{
			try
			{
				ClientHandler clientHandler = null;
				lock (clients.SyncRoot)
				{
					if (clients.ContainsKey(_clientID))
					{
						clientHandler = (ClientHandler)clients[_clientID];
						clients.Remove(_clientID);
					}
				}
				clientHandler?.Stop();
			}
			catch (Exception)
			{
			}
		}

		public static void SendMessage(int _clientID, Message _msg)
		{
			if (Enabled)
			{
				ClientHandler clientHandler = null;
				lock (clients.SyncRoot)
				{
					if (clients.ContainsKey(_clientID))
					{
						clientHandler = (ClientHandler)clients[_clientID];
					}
				}
				if (clientHandler != null)
				{
					try
					{
						clientHandler.SendMessage(_msg);
					}
					catch (Exception)
					{
						lock (clients.SyncRoot)
						{
							if (clients.ContainsKey(_clientID))
							{
								clients.Remove(_clientID);
								ReserveConnect(_clientID);
							}
						}
					}
				}
				else
				{
					ReserveConnect(_clientID);
				}
			}
		}

		private static void ReserveConnect(int _clientID)
		{
			lock (timers.SyncRoot)
			{
				if (!timers.ContainsKey(_clientID))
				{
					Timer value = new Timer(Reconnect, _clientID, 10000, -1);
					timers[_clientID] = value;
				}
			}
		}

		private static void Reconnect(object _state)
		{
			int num = (int)_state;
			bool flag = false;
			lock (timers.SyncRoot)
			{
				if (timers.ContainsKey(num))
				{
					timers.Remove(num);
					flag = true;
				}
			}
			if (flag)
			{
				CreateClient(num);
			}
		}
	}
}
