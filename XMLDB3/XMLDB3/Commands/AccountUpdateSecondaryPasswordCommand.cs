using Mabinogi;

namespace XMLDB3
{
	public class AccountUpdateSecondaryPasswordCommand : BasicCommand
	{
		private string strID = string.Empty;

		private string strPassword = string.Empty;

		private bool m_Result;

		public AccountUpdateSecondaryPasswordCommand()
			: base(NETWORKMSG.MC_DB_ACCOUNT_UPDATE_PASSWORD2)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			strID = _Msg.ReadString();
			strPassword = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("AccountUpdateSecondaryPasswordCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("AccountUpdateSecondaryPasswordCommand.DoProcess() : [" + strID + "] 계정의 2차 비번을 설정합니다");
			if (QueryManager.Account.UpdateSecondaryPassword(strID, strPassword))
			{
				WorkSession.WriteStatus("AccountUpdateSecondaryPasswordCommand.DoProcess() : [" + strID + "] 계정의 2차 비번을 성공적으로 설정하였습니다");
				m_Result = true;
				return true;
			}
			if (strID != null)
			{
				WorkSession.WriteStatus("AccountUpdateSecondaryPasswordCommand.DoProcess() : [" + strID + "] 계정의 2차 비번 설정에 실패하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("AccountUpdateSecondaryPasswordCommand.DoProcess() : 계정 정보가 null 로 2차 비번 설정에 실패하였습니다");
			}
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("AccountUpdateSecondaryPasswordCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result && strID != null)
			{
				message.WriteU8(1);
				message.WriteString(strID);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
