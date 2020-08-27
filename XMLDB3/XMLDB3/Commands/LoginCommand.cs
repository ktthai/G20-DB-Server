using Mabinogi;

namespace XMLDB3
{
	public class LoginCommand : SerializedCommand
	{
		private string m_Account = string.Empty;

		private long m_SessionKey;

		private string m_Address = string.Empty;

		private int m_ISPCode;

		private bool m_Result;

		public LoginCommand()
			: base(NETWORKMSG.NET_DB_SIGNAL_LOGIN)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_SessionKey = _Msg.ReadS64();
			m_Address = _Msg.ReadString();
			m_ISPCode = _Msg.ReadS32();
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.StringIDRegistant(m_Account);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("LoginCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("LoginCommand.DoProcess() : [" + m_Account + "] 계정 로그인 히스토리를 기록합니다.");
			if (QueryManager.Account.LoginSignal(m_Account, m_SessionKey, m_Address, m_ISPCode))
			{
				WorkSession.WriteStatus("LoginCommand.DoProcess() : [" + m_Account + "] 계정 로그인 히스토리를 기록하였습니다");
				m_Result = true;
				return true;
			}
			WorkSession.WriteStatus("LoginCommand.DoProcess() : [" + m_Account + "] 계정 로그인 히스토리를 기록하지 못하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("LoginCommand.MakeMessage() : 함수에 진입하였습니다");
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
