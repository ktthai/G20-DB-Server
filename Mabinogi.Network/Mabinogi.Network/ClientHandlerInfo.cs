using System.Net;

namespace Mabinogi.Network
{
	public class ClientHandlerInfo
	{
		private long total_connect_tryed;

		private long total_connected;

		private long total_connect_failed;

		private long total_closed;

		private long total_receive_function_called;

		private long total_send_function_called;

		private long total_receive_size;

		private long total_send_size;

		private long total_receive_msg_count;

		private long total_send_msg_count;

		public long TotalConnectTryed => total_connect_tryed;

		public long TotalServerConnected => total_connected;

		public long TotalConnectFailed => total_connect_failed;

		public long TotalServerClosed => total_closed;

		public long TotalReceiveFunctionCalled => total_receive_function_called;

		public long TotalSendFunctionCalled => total_send_function_called;

		public long TotalReceiveDataSize => total_receive_size;

		public long TotalSendDataSize => total_send_size;

		public long TotalReceiveMsgCount => total_receive_msg_count;

		public long TotalSendMsgCount => total_send_msg_count;

		public void TryConnect(IPAddress _IP, int _Port)
		{
			total_connect_tryed++;
		}

		public void Connected()
		{
			total_connected++;
		}

		public void ConnectFailed()
		{
			total_connect_failed++;
		}

		public void Closed()
		{
			total_closed++;
		}

		public void DataReceived(int size)
		{
			total_receive_size += size;
			total_receive_function_called++;
		}

		public void DataSended(int size)
		{
			total_send_size += size;
			total_send_function_called++;
		}

		public void MsgReceived(Message _msg)
		{
			total_receive_msg_count++;
		}

		public void MsgSend(Message _msg)
		{
			total_send_msg_count++;
		}
	}
}
