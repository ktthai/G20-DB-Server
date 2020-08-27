using Mabinogi;

namespace XMLDB3
{
	public class AccountrefCreateCommand : BasicCommand
	{
		private AccountRef m_WriteAccountref;

		private bool m_Result;

		public AccountrefCreateCommand()
			: base(NETWORKMSG.MC_DB_ACCOUNT_REF_CREATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_WriteAccountref = AccountrefSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("AccountrefCreateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("AccountrefCreateCommand.DoProcess() : [" + m_WriteAccountref.account + "] 게임계정을 생성합니다");
			m_Result = QueryManager.Accountref.Create(m_WriteAccountref);
			if (m_Result)
			{
				WorkSession.WriteStatus("AccountrefCreateCommand.DoProcess() [" + m_WriteAccountref.account + "] 게임계정을 생성하였습니다.");
				return true;
			}
			if (m_WriteAccountref != null)
			{
				WorkSession.WriteStatus("AccountrefCreateCommand.DoProcess() : [" + m_WriteAccountref.account + "] 게임계정 생성에 실패하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("AccountrefCreateCommand.DoProcess() : 입력된 게임계정 정보가 null 로 생성에 실패하였습니다");
			}
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("AccountrefCreateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				message.WriteString(m_WriteAccountref.account);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
