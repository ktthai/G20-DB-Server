using Mabinogi;

namespace XMLDB3
{
	public abstract class RemoteFunction
	{
		private enum Command
		{
			MONITOR_COMMAND_QUERY_CACHE_STATISTICS_REQUEST = 100,
			MONITOR_COMMAND_QUERY_EXCEPTIONLIST_REQUEST = 101,
			MONITOR_COMMAND_QUERY_NETWORK_STATISTICS_REQUEST = 102,
			MONITOR_COMMAND_QUERY_WORKSESSION_INFO_REQUEST = 103,
			MONITOR_COMMAND_QUERY_CACHE_STATISTICS_REPLY = 104,
			MONITOR_COMMAND_QUERY_EXCEPTIONLIST_REPLY = 105,
			MONITOR_COMMAND_QUERY_NETWORK_STATISTICS_REPLY = 106,
			MONITOR_COMMAND_QUERY_WORKSESSION_INFO_REPLY = 107,
			MONITOR_COMMAND_QUERY_SESSION_INFO_INVALID = -1
		}

		private int m_NetworkId;

		private Message m_InputMsg;

		private Command m_ReplyCommand = Command.MONITOR_COMMAND_QUERY_SESSION_INFO_INVALID;

		private int m_QueryKey;

		private string m_Name;

		public string Name => m_Name;

		protected Message Input => m_InputMsg;

		public abstract Message Process();

		protected abstract string BuildName(Message _input);

		protected Message GetNewReply()
		{
			Message message = new Message((uint)m_ReplyCommand, 0uL);
			message.WriteS32(m_QueryKey);
			return message;
		}

		protected void Reply(Message _input)
		{
			MonitorProcedure.ServerSend(m_NetworkId, _input);
		}

		public static RemoteFunction Parse(int _networkid, Message _inputmsg)
		{
			RemoteFunction remoteFunction = null;
			switch (_inputmsg.ID)
			{
			case 100u:
				remoteFunction = new RemoteFunction_CacheInfo();
				break;
			case 101u:
				remoteFunction = new RemoteFunction_ExceptionInfo();
				break;
			case 102u:
				remoteFunction = new RemoteFunction_NetworkInfo();
				break;
			case 103u:
				remoteFunction = new RemoteFunction_SessionInfo();
				break;
			default:
				remoteFunction = null;
				break;
			}
			if (remoteFunction != null)
			{
				remoteFunction.m_NetworkId = _networkid;
				remoteFunction.m_InputMsg = _inputmsg;
				remoteFunction.m_ReplyCommand = MatchReplyCommand((Command)_inputmsg.ID);
				remoteFunction.m_QueryKey = _inputmsg.ReadS32();
				remoteFunction.m_Name = remoteFunction.BuildName(_inputmsg.Clone());
			}
			return remoteFunction;
		}

		private static Command MatchReplyCommand(Command _requestcmd)
		{
			switch (_requestcmd)
			{
			case Command.MONITOR_COMMAND_QUERY_CACHE_STATISTICS_REQUEST:
				return Command.MONITOR_COMMAND_QUERY_CACHE_STATISTICS_REPLY;
			case Command.MONITOR_COMMAND_QUERY_EXCEPTIONLIST_REQUEST:
				return Command.MONITOR_COMMAND_QUERY_EXCEPTIONLIST_REPLY;
			case Command.MONITOR_COMMAND_QUERY_NETWORK_STATISTICS_REQUEST:
				return Command.MONITOR_COMMAND_QUERY_NETWORK_STATISTICS_REPLY;
			case Command.MONITOR_COMMAND_QUERY_WORKSESSION_INFO_REQUEST:
				return Command.MONITOR_COMMAND_QUERY_WORKSESSION_INFO_REPLY;
			default:
				return Command.MONITOR_COMMAND_QUERY_SESSION_INFO_INVALID;
			}
		}
	}
}
