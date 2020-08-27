using Mabinogi;

namespace XMLDB3
{
	public class AccountCreateCommand : BasicCommand
	{
		private Account m_WriteAccount;

		private bool m_Result;

		public AccountCreateCommand()
			: base(NETWORKMSG.MC_DB_ACCOUNT_CREATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_WriteAccount = AccountSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("AccountCreateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("AccountCreateCommand.DoProcess() : [" + m_WriteAccount.id + "] 계정을 생성합니다");
			if (QueryManager.Account.Create(m_WriteAccount))
			{
				WorkSession.WriteStatus("AccountCreateCommand.DoProcess() : [" + m_WriteAccount.id + "] 계정을 성공적으로 생성하였습니다");
				m_Result = true;
				return true;
			}
			if (m_WriteAccount != null)
			{
				WorkSession.WriteStatus("AccountCreateCommand.DoProcess() : [" + m_WriteAccount.id + "] 계정 생성에 실패하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("AccountCreateCommand.DoProcess() : 계정 정보가 null 로 생성에 실패하였습니다");
			}
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("AccountCreateCommand.MakeMessage() : 함수에 진입하였습니다");
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
