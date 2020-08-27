namespace Mabinogi.Network
{
	internal class ConsoleTestServer : ServerHandler
	{
		protected override void OnConnect(int _id)
		{
		}

		protected override void OnClose(int _id)
		{
		}

		protected override void OnReceive(int _id, Message _msg)
		{
			SendMessage(_id, _msg);
		}
	}
}
