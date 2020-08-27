namespace Mabinogi.Network
{
	public class ServerHandlerInfo
	{
		private long total_receive_function_called;

		private long total_send_function_called;

		private long total_receive_size;

		private long total_send_size;

		private long total_receive_msg_count;

		private long total_send_msg_count;

		private long total_connected_count;

		private long total_closed_count;

		private long total_connection_confirmed_count;

		public long TotalReceiveFunctionCalled => total_receive_function_called;

		public long TotalSendFunctionCalled => total_send_function_called;

		public long TotalReceiveDataSize => total_receive_size;

		public long TotalSendDataSize => total_send_size;

		public long TotalReceiveMsgCount => total_receive_msg_count;

		public long TotalSendMsgCount => total_send_msg_count;

		public long TotalConnectedCount => total_connected_count;

		public long TotalClosedCount => total_closed_count;

		public long TotalConfirmedConnectionCount => total_connection_confirmed_count;

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

		public void Connected()
		{
			total_connected_count++;
		}

		public void Closed()
		{
			total_closed_count++;
		}

		public void ConnectionConfirmed()
		{
			total_connection_confirmed_count++;
		}

		public Message ToMessage()
		{
			Message message = new Message(0u, 0uL);
			message.WriteS64(total_receive_function_called);
			message.WriteS64(total_send_function_called);
			message.WriteS64(total_receive_size);
			message.WriteS64(total_send_size);
			message.WriteS64(total_receive_msg_count);
			message.WriteS64(total_send_msg_count);
			message.WriteS64(total_connected_count);
			message.WriteS64(total_closed_count);
			message.WriteS64(total_connection_confirmed_count);
			return message;
		}

		public Message FromMessage(Message _input)
		{
			total_receive_function_called = _input.ReadS64();
			total_send_function_called = _input.ReadS64();
			total_receive_size = _input.ReadS64();
			total_send_size = _input.ReadS64();
			total_receive_msg_count = _input.ReadS64();
			total_send_msg_count = _input.ReadS64();
			total_connected_count = _input.ReadS64();
			total_closed_count = _input.ReadS64();
			total_connection_confirmed_count = _input.ReadS64();
			return _input;
		}
	}
}
