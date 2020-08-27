using Mabinogi;
using Mabinogi.Network;

namespace XMLDB3
{
	public class ProfilerClient
	{
		private const int MaxProfileCount = 10000;

		public int ID;

		private bool m_Running;

		private int m_SendCount;

		public ProfilerClient(int _id)
		{
			ID = _id;
			m_Running = true;
			m_SendCount = 0;
		}

		public void SendProfile(ServerHandler _handler, Message _msg)
		{
			if (m_Running)
			{
				_handler.SendMessage(ID, _msg);
			}
			lock (this)
			{
				m_SendCount++;
				if (m_Running && m_SendCount >= 10000)
				{
					m_Running = false;
				}
			}
		}

		public int ResetSendCount()
		{
			int result = 0;
			lock (this)
			{
				if (m_SendCount > 10000)
				{
					result = m_SendCount - 10000;
				}
				m_SendCount = 0;
				m_Running = true;
				return result;
			}
		}
	}
}
