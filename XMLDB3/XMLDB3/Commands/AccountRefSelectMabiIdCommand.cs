using Mabinogi;

namespace XMLDB3
{
	public class AccountRefSelectMabiIdCommand : BasicCommand
	{
		private string ServerName_;

		private string CharName_;

		private string MabinogiId;

		public AccountRefSelectMabiIdCommand()
			: base(NETWORKMSG.NET_DB_QUERY_ACCOUNT_WITH_SERVER_AND_CHAR_NAME)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			ServerName_ = _Msg.ReadString();
			CharName_ = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("AccountRefSelectMabiIdCommand.DoProcess() : 함수에 진입하였습니다");
			MabinogiId = QueryManager.Accountref.SelectMabinogiId(ServerName_, CharName_);
			WorkSession.WriteStatus("AccountRefSelectMabiIdCommand.DoProcess() : 함수 실행 완료.");
			return true;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("AccountRefSelectMabiIdCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (MabinogiId != null)
			{
				message.WriteU8(1);
				message.WriteString(MabinogiId);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
