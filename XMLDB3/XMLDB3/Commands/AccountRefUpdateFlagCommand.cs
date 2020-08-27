using Mabinogi;

namespace XMLDB3
{
	public class AccountRefUpdateFlagCommand : BasicCommand
	{
		private string m_Account;

		private int m_Flag;

		private bool m_Result;

		public AccountRefUpdateFlagCommand()
			: base(NETWORKMSG.NET_DB_UPDATE_ACCOUNT_REF_FLAG)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_Flag = _Msg.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("AccountrefUpdateFlagCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("AccountrefUpdateFlagCommand.DoProcess() : [" + m_Flag + "] 업데이트 합니다.");
			m_Result = QueryManager.Accountref.SetFlag(m_Account, m_Flag);
			if (m_Result)
			{
				WorkSession.WriteStatus("AccountrefUpdateFlagCommand.DoProcess() [" + m_Flag + "] 업데이트 하였습니다.");
				return true;
			}
			WorkSession.WriteStatus("AccountrefUpdateFlagCommand.DoProcess() : [" + m_Flag + "] 업데이트 실패 하였습니다.");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("AccountrefUpdateFlagCommand.MakeMessage() : 함수에 진입하였습니다");
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
