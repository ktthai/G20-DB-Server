using System.Net;

namespace Mabinogi.Network
{
	public class ServerInstanceInfo
	{
		private int id = -1;

		private IPAddress target_address;

		private int port;

		private long total_receive_function_called;

		private long total_send_function_called;

		private long total_receive_size;

		private long total_send_size;

		private long total_receive_msg_count;

		private long total_send_msg_count;

		public int ID => id;

		public IPAddress TargetAddress => target_address;

		public int Port => port;

		public long TotalReceiveFunctionCalled => total_receive_function_called;

		public long TotalSendFunctionCalled => total_send_function_called;

		public long TotalReceiveDataSize => total_receive_size;

		public long TotalSendDataSize => total_send_size;

		public long TotalReceiveMsgCount => total_receive_msg_count;

		public long TotalSendMsgCount => total_send_msg_count;

		public ServerInstanceInfo(int _id, IPAddress _address, int _port)
		{
			id = _id;
			target_address = _address;
			port = _port;
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

		public Message ToMessage()
		{
			Message message = new Message(0u, 0uL);
			message.WriteS32(id);
			message.WriteString(target_address.ToString());
			message.WriteS32(port);
			message.WriteS64(total_receive_function_called);
			message.WriteS64(total_send_function_called);
			message.WriteS64(total_receive_size);
			message.WriteS64(total_send_size);
			message.WriteS64(total_receive_msg_count);
			message.WriteS64(total_send_msg_count);
			return message;
		}

		public Message FromMessage(Message _input)
		{
			id = _input.ReadS32();
			target_address = IPAddress.Parse(_input.ReadString());
			port = _input.ReadS32();
			total_receive_function_called = _input.ReadS64();
			total_send_function_called = _input.ReadS64();
			total_receive_size = _input.ReadS64();
			total_send_size = _input.ReadS64();
			total_receive_msg_count = _input.ReadS64();
			total_send_msg_count = _input.ReadS64();
			return _input;
		}
	}
}
