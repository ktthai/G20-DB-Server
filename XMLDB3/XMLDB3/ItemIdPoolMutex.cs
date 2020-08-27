using System.Threading;

namespace XMLDB3
{
	public class ItemIdPoolMutex
	{
		private static Mutex idpoolmutex;

		static ItemIdPoolMutex()
		{
			idpoolmutex = new Mutex();
		}

		public static void Enter()
		{
			idpoolmutex.WaitOne();
		}

		public static void Leave()
		{
			idpoolmutex.ReleaseMutex();
		}
	}
}
