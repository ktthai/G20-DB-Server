using Mabinogi;

namespace XMLDB3
{
	public class Account2PWSerializer
	{
		public static Account Serialize(Message _message)
		{
			Account account = AccountSerializer.Serialize(_message);
			account.SecondaryPasswordAuth = new AccountSecondaryPasswordAuth();
			account.SecondaryPasswordAuth.password2 = _message.ReadString();
			account.SecondaryPasswordAuth.missCount = _message.ReadU8();
			return account;
		}

		public static void Deserialize(Account _account, Message _message)
		{
			if (_account.SecondaryPasswordAuth == null)
			{
				_account.SecondaryPasswordAuth = new AccountSecondaryPasswordAuth();
				_account.SecondaryPasswordAuth.password2 = string.Empty;
				_account.SecondaryPasswordAuth.missCount = 0;
			}
			AccountSerializer.Deserialize(_account, _message);
			_message.WriteString(_account.SecondaryPasswordAuth.password2);
			_message.WriteU8(_account.SecondaryPasswordAuth.missCount);
		}
	}
}
