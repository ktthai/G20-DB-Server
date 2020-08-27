using Mabinogi;
using System.Collections;

namespace XMLDB3
{
	public class AccountRefUpdateMetaCommand : BasicCommand
	{
		private string account_;

		private bool result_;

		private Hashtable metaData_;

		public AccountRefUpdateMetaCommand()
			: base(NETWORKMSG.MC_DB_ACCOUNT_META_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			account_ = _Msg.ReadString();
			string text = _Msg.ReadString();
			if (text != null && text != string.Empty && text.Length > 0)
			{
				metaData_ = CAccountMetaHelper.AccountMetaStringToMetaRowList(text);
			}
			else
			{
				metaData_ = null;
			}
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("AccountRefUpdateMetaCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("AccountRefUpdateMetaCommand.DoProcess() : [" + account_ + "] 업데이트 합니다.");
			result_ = QueryManager.Accountref.UpdateAccountMeta(account_, metaData_);
			if (result_)
			{
				WorkSession.WriteStatus("AccountRefUpdateMetaCommand.DoProcess() [" + account_ + "] 업데이트 하였습니다.");
				return true;
			}
			WorkSession.WriteStatus("AccountRefUpdateMetaCommand.DoProcess() : [" + account_ + "] 업데이트 실패 하였습니다.");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("AccountRefUpdateMetaCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (result_)
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
