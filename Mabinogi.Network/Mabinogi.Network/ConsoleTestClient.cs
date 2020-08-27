using System;
using System.Threading;

namespace Mabinogi.Network
{
	internal class ConsoleTestClient : ClientHandler
	{
		public volatile TESTMODE test_mode;

		private int receive_count;

		private bool reconnect;

		private int fusion_test_seed;

		protected override void OnConnect()
		{
			switch (test_mode)
			{
			case TESTMODE.TEST_NORMAL:
			case TESTMODE.TEST_PACKET:
			{
				Random random4 = new Random(DateTime.Now.Millisecond);
				int num4 = random4.Next(256, 1024);
				byte[] data4 = new byte[num4];
				Message message4 = new Message(0u, 0uL);
				message4.WriteBinary(data4);
				SendMessage(message4);
				break;
			}
			case TESTMODE.TEST_CONNECT:
			{
				Random random3 = new Random(DateTime.Now.Millisecond);
				int num3 = random3.Next(256, 1024);
				byte[] data3 = new byte[num3];
				Message message3 = new Message(0u, 0uL);
				message3.WriteBinary(data3);
				SendMessage(message3);
				receive_count = 0;
				reconnect = false;
				break;
			}
			case TESTMODE.TEST_BIG_PACKET:
			{
				Random random2 = new Random(DateTime.Now.Millisecond);
				int num2 = random2.Next(10240, 102400);
				byte[] data2 = new byte[num2];
				Message message2 = new Message(0u, 0uL);
				message2.WriteBinary(data2);
				SendMessage(message2);
				break;
			}
			case TESTMODE.TEST_FUSION:
			{
				Random random = new Random(DateTime.Now.Millisecond);
				fusion_test_seed = random.Next(10, 5120);
				int num = random.Next(10, 10240);
				byte[] data = new byte[num];
				Message message = new Message(0u, 0uL);
				message.WriteBinary(data);
				SendMessage(message);
				receive_count = 0;
				reconnect = false;
				break;
			}
			case TESTMODE.TEST_QUIT:
				Stop();
				break;
			}
		}

		protected override void OnConnectFail()
		{
		}

		protected override void OnClose()
		{
			if (reconnect)
			{
				reconnect = false;
				TESTMODE tESTMODE = test_mode;
				if (tESTMODE == TESTMODE.TEST_FUSION)
				{
					Random random = new Random(DateTime.Now.Millisecond);
					int millisecondsTimeout = random.Next(10, 5000);
					Thread.Sleep(millisecondsTimeout);
				}
				Connect(base.Address, base.Port);
			}
			else
			{
				Stop();
			}
		}

		protected override void OnReceive(Message _msg)
		{
			switch (test_mode)
			{
			case TESTMODE.TEST_NORMAL:
			{
				Random random6 = new Random(DateTime.Now.Millisecond);
				int num5 = random6.Next(256, 1024);
				byte[] data5 = new byte[num5];
				Message message5 = new Message(0u, 0uL);
				message5.WriteBinary(data5);
				SendMessage(message5);
				break;
			}
			case TESTMODE.TEST_CONNECT:
			{
				receive_count++;
				if (receive_count > 10)
				{
					reconnect = true;
					Stop();
					break;
				}
				Random random5 = new Random(DateTime.Now.Millisecond);
				int num4 = random5.Next(256, 1024);
				byte[] data4 = new byte[num4];
				Message message4 = new Message(0u, 0uL);
				message4.WriteBinary(data4);
				SendMessage(message4);
				break;
			}
			case TESTMODE.TEST_PACKET:
			{
				Random random3 = new Random(DateTime.Now.Millisecond);
				int num2 = random3.Next(256, 1024);
				byte[] data2 = new byte[num2];
				Message message2 = new Message(0u, 0uL);
				message2.WriteBinary(data2);
				SendMessage(message2);
				SendMessage(message2);
				break;
			}
			case TESTMODE.TEST_BIG_PACKET:
			{
				Random random4 = new Random(DateTime.Now.Millisecond);
				int num3 = random4.Next(10240, 102400);
				byte[] data3 = new byte[num3];
				Message message3 = new Message(0u, 0uL);
				message3.WriteBinary(data3);
				SendMessage(message3);
				break;
			}
			case TESTMODE.TEST_FUSION:
			{
				receive_count++;
				if (fusion_test_seed == 0)
				{
					Random random = new Random(DateTime.Now.Millisecond);
					fusion_test_seed = random.Next(10, 5120);
				}
				if (receive_count > fusion_test_seed)
				{
					reconnect = true;
					Stop();
					break;
				}
				Random random2 = new Random(DateTime.Now.Millisecond);
				int num = random2.Next(10, 10240);
				byte[] data = new byte[num];
				Message message = new Message(0u, 0uL);
				message.WriteBinary(data);
				SendMessage(message);
				break;
			}
			case TESTMODE.TEST_QUIT:
				Stop();
				break;
			}
		}
	}
}
