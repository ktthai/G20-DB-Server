using Mabinogi;

namespace XMLDB3
{
	public class LogoutCommand : SerializedCommand
	{
		private string m_Account = string.Empty;

		private bool m_Result;

		public override bool ReplyEnable => false;

		public LogoutCommand()
			: base(NETWORKMSG.NET_DB_SIGNAL_LOGOUT)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.StringIDRegistant(m_Account);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("LogoutCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("LogoutCommand.DoProcess() : [" + m_Account + "] 계정 로그아웃 히스토리를 기록합니다.");
			if (QueryManager.Account.LogoutSignal(m_Account))
			{
				WorkSession.WriteStatus("LogoutCommand.DoProcess() : [" + m_Account + "] 계정 로그아웃 히스토리를 기록하였습니다");
				m_Result = true;
				return true;
			}
			WorkSession.WriteStatus("LogoutCommand.DoProcess() : [" + m_Account + "] 계정 로그아웃 히스토리를 기록하지 못하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("LogoutCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
