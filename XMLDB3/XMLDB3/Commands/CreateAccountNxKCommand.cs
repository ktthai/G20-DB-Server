using Mabinogi;

namespace XMLDB3
{
	public class CreateAccountNxKCommand : BasicCommand
	{
		private long m_nexonOID;

		private string m_nexonId;

		private byte m_authority;

		private bool m_Result;

		private string m_mabinogiId;

		public CreateAccountNxKCommand()
			: base(NETWORKMSG.MC_DB_ACCOUNT_CREATE_NXK)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_nexonOID = (long)_Msg.ReadU64();
			m_nexonId = _Msg.ReadString();
			m_authority = _Msg.ReadU8();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CreateAccountNxKCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CreateAccountNxKCommand.DoProcess() : [" + m_nexonOID + "(" + m_authority + ")] 계정을 생성합니다");
			if (QueryManager.Account.CreateNxK(m_nexonOID, m_nexonId, m_authority, out m_mabinogiId))
			{
				WorkSession.WriteStatus("CreateAccountNxKCommand.DoProcess() : [" + m_nexonOID + "(" + m_authority + ")] 계정을 성공적으로 생성하였습니다");
				m_Result = true;
				return true;
			}
			WorkSession.WriteStatus("CreateAccountNxKCommand.DoProcess() : [" + m_nexonOID + "(" + m_authority + ")] 계정 생성에 실패하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("AccountCreateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				message.WriteString(m_mabinogiId);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
