using Mabinogi;
using System;
using System.Collections;
using System.Collections.Generic;

namespace XMLDB3
{
	public class AccountrefSerializer
	{
		public static AccountRef Serialize(Message _message)
		{
			AccountRef accountref = new AccountRef();
			accountref.account = _message.ReadString();
			accountref.flag = _message.ReadS32();
			accountref.maxslot = 0;
			accountref.In = new DateTime(_message.ReadS64());
			accountref.Out = new DateTime(_message.ReadS64());
			accountref.playabletime = _message.ReadS32();
			accountref.supportLastChangeTime = _message.ReadS32();
			accountref.supportRace = _message.ReadU8();
			accountref.supportRewardState = _message.ReadU8();
			accountref.lobbyOption = _message.ReadS32();
			int num = _message.ReadS16();
			accountref.Character = new List<AccountrefCharacter>(num);
			for (int i = 0; i < num; i++)
			{
				accountref.Character[i] = new AccountrefCharacter();
				accountref.Character[i].id = (long)_message.ReadU64();
				accountref.Character[i].name = _message.ReadString();
				accountref.Character[i].server = _message.ReadString();
				accountref.Character[i].deleted = (long)_message.ReadU64();
				accountref.Character[i].groupID = _message.ReadU8();
				accountref.Character[i].race = _message.ReadU8();
				accountref.Character[i].supportCharacter = (_message.ReadU8() != 0);
				accountref.Character[i].tab = (_message.ReadU8() != 0);
			}
			num = _message.ReadS16();
			accountref.Pet = new List<AccountrefPet>(num);
			for (int j = 0; j < num; j++)
			{
				accountref.Pet[j] = new AccountrefPet();
				accountref.Pet[j].id = (long)_message.ReadU64();
				accountref.Pet[j].name = _message.ReadString();
				accountref.Pet[j].server = _message.ReadString();
				accountref.Pet[j].deleted = _message.ReadS64();
				accountref.Pet[j].remaintime = (int)_message.ReadU32();
				accountref.Pet[j].lasttime = (long)_message.ReadU64();
				accountref.Pet[j].expiretime = (long)_message.ReadU64();
				accountref.Pet[j].groupID = _message.ReadU8();
				accountref.Pet[j].tab = (_message.ReadU8() != 0);
			}
			accountref.macroCheckFailure = _message.ReadU8();
			accountref.macroCheckSuccess = _message.ReadU8();
			accountref.beginnerFlag = ((_message.ReadU8() != 0) ? true : false);
			string text = _message.ReadString();
			if (text != null && text != string.Empty && text.Length > 0)
			{
				Hashtable hashtable = CAccountMetaHelper.AccountMetaStringToMetaRowList(text);
				if (hashtable != null && hashtable.Count > 0)
				{
					accountref.AccountMeta = (List<AccountrefMeta>)hashtable.Values;
				}
			}
			return accountref;
		}

		public static void Deserialize(AccountRef _accountref, Message _message)
		{
			if (_accountref == null)
			{
				_accountref = new AccountRef();
			}
			_message.WriteString(_accountref.account);
			_message.WriteS32(_accountref.flag);
			_message.WriteS16(_accountref.maxslot);
			_message.WriteS64(_accountref.In.Ticks);
			_message.WriteS64(_accountref.Out.Ticks);
			_message.WriteS32(_accountref.playabletime);
			_message.WriteS32(_accountref.supportLastChangeTime);
			_message.WriteU8(_accountref.supportRace);
			_message.WriteU8(_accountref.supportRewardState);
			_message.WriteS32(_accountref.lobbyOption);
			if (_accountref.Character != null)
			{
				_message.WriteS16((short)_accountref.Character.Count);

				foreach (AccountrefCharacter accountrefCharacter in _accountref.Character)
				{
					_message.WriteS64(accountrefCharacter.id);
					_message.WriteString(accountrefCharacter.name);
					_message.WriteString(accountrefCharacter.server);
					_message.WriteS64(accountrefCharacter.deleted);
					_message.WriteU8(accountrefCharacter.groupID);
					_message.WriteU8(accountrefCharacter.race);
					_message.WriteU8((byte)(accountrefCharacter.supportCharacter ? 1 : 0));
					_message.WriteU8((byte)(accountrefCharacter.tab ? 1 : 0));
				}
			}
			else
			{
				_message.WriteS16(0);
			}
			if (_accountref.Pet != null)
			{
				_message.WriteS16((short)_accountref.Pet.Count);

				foreach (AccountrefPet accountrefPet in _accountref.Pet)
				{
					_message.WriteS64(accountrefPet.id);
					_message.WriteString(accountrefPet.name);
					_message.WriteString(accountrefPet.server);
					_message.WriteS64(accountrefPet.deleted);
					_message.WriteS32(accountrefPet.remaintime);
					_message.WriteS64(accountrefPet.lasttime);
					_message.WriteS64(accountrefPet.expiretime);
					_message.WriteU8(accountrefPet.groupID);
					_message.WriteU8((byte)(accountrefPet.tab ? 1 : 0));
				}
			}
			else
			{
				_message.WriteS16(0);
			}
			_message.WriteU8(_accountref.macroCheckFailure);
			_message.WriteU8(_accountref.macroCheckSuccess);
			_message.WriteU8((byte)(_accountref.beginnerFlag ? 1 : 0));
			_message.WriteString(CAccountMetaHelper.AccountMetaListToString(_accountref.AccountMeta));
		}
	}
}
