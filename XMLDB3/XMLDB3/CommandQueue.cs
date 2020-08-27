using System.Collections;

namespace XMLDB3
{
	public class CommandQueue
	{
		private Queue m_CmdQueue = new Queue();

		public CacheStatistics Statistics
		{
			get
			{
				lock (m_CmdQueue.SyncRoot)
				{
					return new CacheStatistics("CommandQueue", m_CmdQueue.Count);
				}
			}
		}

		public void Push(BasicCommand _cmd)
		{
			lock (m_CmdQueue.SyncRoot)
			{
				m_CmdQueue.Enqueue(_cmd);
			}
		}

		public BasicCommand Pop()
		{
			lock (m_CmdQueue.SyncRoot)
			{
				if (m_CmdQueue.Count > 0)
				{
					return (BasicCommand)m_CmdQueue.Dequeue();
				}
				return null;
			}
		}
	}
}
