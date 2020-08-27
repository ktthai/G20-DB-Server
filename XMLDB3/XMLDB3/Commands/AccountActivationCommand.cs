using Mabinogi;

namespace XMLDB3
{
	public class AccountActivationCommand : BasicCommand
	{
		private AccountActivation m_WriteAccount;

		private bool m_Result;

		public AccountActivationCommand()
			: base(NETWORKMSG.MC_DB_ACCOUNT_CREATE_ACTIVATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_WriteAccount = AccountSerializer.SerializeForActivation(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("AccountActivationCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("AccountActivationCommand.DoProcess() : [" + m_WriteAccount.id + "] 계정을 생성합니다");
			if (QueryManager.AccountActivation.Create(m_WriteAccount))
			{
				WorkSession.WriteStatus("AccountActivationCommand.DoProcess() : [" + m_WriteAccount.id + "] 계정을 성공적으로 생성하였습니다");
				m_Result = true;
				return true;
			}
			if (m_WriteAccount != null)
			{
				WorkSession.WriteStatus("AccountActivationCommand.DoProcess() : [" + m_WriteAccount.id + "] 계정 생성에 실패하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("AccountActivationCommand.DoProcess() : 계정 정보가 null 로 생성에 실패하였습니다");
			}
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("AccountActivationCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result && m_WriteAccount != null)
			{
				message.WriteU8(1);
				message.WriteString(m_WriteAccount.id);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
