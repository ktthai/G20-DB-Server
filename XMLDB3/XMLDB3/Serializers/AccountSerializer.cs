using Mabinogi;
using System;

namespace XMLDB3
{
	public class AccountSerializer
	{
		public static Account Serialize(Message _message)
		{
			Account account = new Account();
			account.id = _message.ReadString();
			byte[] array = _message.ReadBinary();
			account.name = _message.ReadString();
			// Ignore the KSSN
			_message.ReadString();
			account.email = _message.ReadString();
			account.flag = _message.ReadS16();
			account.blocking_date = new DateTime(_message.ReadS64());
			account.blocking_duration = _message.ReadS16();
			account.authority = _message.ReadU8();
			char[] array2 = new char[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = (char)array[i];
			}
			account.password = new string(array2);
			return account;
		}

		public static AccountActivation SerializeForActivation(Message _message)
		{
			AccountActivation accountActivation = new AccountActivation();
			accountActivation.id = _message.ReadString();
			byte[] array = _message.ReadBinary();
			accountActivation.name = _message.ReadString();
			// Ignore the KSSN
			_message.ReadString();
			accountActivation.email = _message.ReadString();
			accountActivation.flag = _message.ReadS16();
			accountActivation.blocking_date = new DateTime(_message.ReadS64());
			accountActivation.blocking_duration = _message.ReadS16();
			accountActivation.authority = _message.ReadU8();
			accountActivation.provider_code = _message.ReadU8();
			char[] array2 = new char[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = (char)array[i];
			}
			accountActivation.password = new string(array2);
			return accountActivation;
		}

		public static void Deserialize(Account _account, Message _message)
		{
			byte[] array = new byte[64];
			for (int i = 0; i < _account.password.Length; i++)
			{
				if (i < 64)
				{
					array[i] = (byte)_account.password[i];
				}
			}
			_message.WriteString(_account.id);
			_message.WriteBinary(array);
			_message.WriteString(_account.name);
			// Ignore KSSN
			_message.WriteString("");
			_message.WriteString(_account.email);
			_message.WriteS16(_account.flag);
			_message.WriteS64(_account.blocking_date.Ticks);
			_message.WriteS16(_account.blocking_duration);
			_message.WriteU8(_account.authority);
			_message.WriteU8((byte)(_account.changePassword ? 1 : 0));
			if (_account.machineids != null)
			{
				_message.WriteString(_account.machineids);
			}
			else
			{
				_message.WriteString("");
			}
		}
	}
}
