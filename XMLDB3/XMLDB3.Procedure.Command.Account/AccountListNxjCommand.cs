using Mabinogi;

namespace XMLDB3.Procedure.Command.Account
{
	public class AccountListNxjCommand : BasicCommand
	{
		private string m_nexonId;

		private string[] m_mabiIdList;

		private bool m_Result;

		public AccountListNxjCommand()
			: base(NETWORKMSG.MC_DB_ACCOUNT_LIST_NXJ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_nexonId = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("AccountListNxjCommand.DoProcess() : 함수에 진입하였습니다");
			if (QueryManager.Account.AccountListNxJ(m_nexonId, out m_mabiIdList))
			{
				WorkSession.WriteStatus("AccountListNxjCommand.DoProcess() : [" + m_nexonId + "] 계정으로 마비아이디를 조회하였습니다.");
				m_Result = true;
				return true;
			}
			WorkSession.WriteStatus("AccountListNxjCommand.DoProcess() : [" + m_nexonId + "] 계정으로 마비아이디를 조회하는데 실패하였습니다..");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("AccountListNxjCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result && m_mabiIdList != null)
			{
				message.WriteU8(1);
				message.WriteU32((uint)m_mabiIdList.Length);
				string[] mabiIdList = m_mabiIdList;
				foreach (string data in mabiIdList)
				{
					message.WriteString(data);
				}
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
