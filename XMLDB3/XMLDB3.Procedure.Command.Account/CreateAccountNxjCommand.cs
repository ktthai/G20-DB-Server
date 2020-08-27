using Mabinogi;

namespace XMLDB3.Procedure.Command.Account
{
	public class CreateAccountNxjCommand : BasicCommand
	{
		private string m_nexonId;

		private string m_mabinogiId;

		private byte m_authority;

		private bool m_Result;

		public CreateAccountNxjCommand()
			: base(NETWORKMSG.MC_DB_ACCOUNT_CREATE_NXJ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_nexonId = _Msg.ReadString();
			m_mabinogiId = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CreateAccountNxjCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CreateAccountNxjCommand.DoProcess() : [" + m_nexonId + "(" + m_authority + ")] 계정을 생성합니다");
			if (QueryManager.Account.CreateNxJ(m_nexonId, m_mabinogiId, "auto_generate_mabi_id"))
			{
				WorkSession.WriteStatus("CreateAccountNxjCommand.DoProcess() : [" + m_nexonId + "(" + m_authority + ")] 계정을 성공적으로 생성하였습니다");
				m_Result = true;
				return true;
			}
			WorkSession.WriteStatus("CreateAccountNxjCommand.DoProcess() : [" + m_nexonId + "(" + m_authority + ")] 계정 생성에 실패하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CreateAccountNxjCommand.MakeMessage() : 함수에 진입하였습니다");
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
