using Mabinogi;
using System;

namespace XMLDB3
{
	public class AccountReadCommand : BasicCommand
	{
		private string m_strAccount;

		private Account m_ReadAccount;

		private bool m_Result;

		public AccountReadCommand()
			: base(NETWORKMSG.MC_DB_ACCOUNT_READ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_strAccount = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("AccountReadCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("AccountReadCommand.DoProcess() : [" + m_strAccount + "] 계정 정보 읽기를 쿼리합니다");
			m_ReadAccount = QueryManager.Account.Read(m_strAccount);
			if (m_ReadAccount != null)
			{
				WorkSession.WriteStatus("AccountReadCommand.DoProcess() : [" + m_strAccount + "] 계정 정보를 읽었습니다");
				m_Result = true;
				return true;
			}
			WorkSession.WriteStatus("AccountReadCommand.DoProcess() : [" + m_strAccount + "] 계정에 대한 쿼리를 실패하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("AccountReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result && m_ReadAccount != null)
			{
				message.WriteU8(1);
				AccountSerializer.Deserialize(m_ReadAccount, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
