using Mabinogi;

namespace XMLDB3
{
	public class MembershipBillingCheckCommand : BasicCommand
	{
		private string m_accountID;

		private string m_setDate;

		private bool m_Result;

		private byte m_errorCode;

		private int m_resultValue;

		public MembershipBillingCheckCommand()
			: base(NETWORKMSG.MC_DB_ACCOUNT_MEMBERSHIP_BILLING)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_accountID = _message.ReadString();
			m_setDate = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("MembershipBillingCheckCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Accountref.VerifyMembershipFromBilling(m_accountID, m_setDate, ref m_errorCode, ref m_resultValue);
			if (m_Result)
			{
				WorkSession.WriteStatus("MembershipBillingCheckCommand.DoProcess() : 멤버쉽 여부를 받아 왔습니다");
			}
			else
			{
				WorkSession.WriteStatus("MembershipBillingCheckCommand.DoProcess() : 멤버쉽 여부를 받아오는데 실패했습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MembershipBillingCheckCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				message.WriteU32((uint)m_resultValue);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
