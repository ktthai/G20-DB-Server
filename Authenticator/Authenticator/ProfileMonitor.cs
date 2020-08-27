using Mabinogi;
using Mabinogi.Network;
using System.Collections;

namespace Authenticator
{
	public class ProfileMonitor : ServerHandler
	{
		private static ProfileMonitor ProfileServer = new ProfileMonitor();

		private ArrayList user_array = new ArrayList();

		public static void ProfileStart()
		{
			ProfileServer.Start(15002);
		}

		public static void ProfileStop()
		{
			ProfileServer.Stop();
		}

		protected override void OnConnect(int _id)
		{
			lock (user_array.SyncRoot)
			{
				user_array.Add(_id);
			}
		}

		protected override void OnClose(int _id)
		{
			lock (user_array.SyncRoot)
			{
				user_array.Remove(_id);
			}
		}

		public static void Signal()
		{
			Message message = new Message(0u, 0uL);
			ArrayList arrayList = null;
			lock (ProfileServer.user_array.SyncRoot)
			{
				arrayList = ProfileServer.user_array;
			}
			foreach (int item in arrayList)
			{
				ProfileServer.SendMessage(item, message);
			}
		}
	}
}
