using Mabinogi;

namespace XMLDB3
{
	public class AccountBanCommand : BasicCommand
	{
		private string m_Account = string.Empty;

		private short m_banType = -1;

		private string m_ManagerName = string.Empty;

		private short m_duration;

		private string m_Purpose = string.Empty;

		private bool m_Result;

		public AccountBanCommand()
			: base(NETWORKMSG.NET_DB_BAN_ACCOUNT)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_banType = _Msg.ReadS8();
			m_ManagerName = _Msg.ReadString();
			m_duration = _Msg.ReadS8();
			m_Purpose = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("AccountBanCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("AccountBanCommand.DoProcess() : [" + m_Account + "] 계정을 밴합니다");
			if (QueryManager.Account.Ban(m_Account, m_banType, m_ManagerName, m_duration, m_Purpose))
			{
				WorkSession.WriteStatus("AccountBanCommand.DoProcess() : [" + m_Account + "] 계정을 성공적으로 밴하였습니다");
				m_Result = true;
				return true;
			}
			WorkSession.WriteStatus("AccountBanCommand.DoProcess() : [" + m_Account + "] 계정 밴에 실패하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("AccountBanCommand.MakeMessage() : 함수에 진입하였습니다");
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
