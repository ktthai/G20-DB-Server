using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class BankSerializer
	{
		public static Bank Serialize(Message _message)
		{
			Bank bank = new Bank();
			bank.account = _message.ReadString();
			bank.data = ReadDataFromMsg(_message);
			for (int i = 0; i < 3; i++)
			{
				ReadSlotsFromMsg(_message, bank, (BankRace)i);
			}
			return bank;
		}

		private static BankData ReadDataFromMsg(Message _message)
		{
			BankData bankData = new BankData();
			bankData.deposit = _message.ReadS32();
			bankData.password = _message.ReadString();
			return bankData;
		}

		private static void ReadSlotsFromMsg(Message _message, Bank _bank, BankRace _race)
		{
			int num = _message.ReadS32();
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					_bank.slot.Add(ReadSlotFromMsg(_message, _race));
				}
				_bank.SetWealth(_race, _message.ReadS32());
				_bank.SetBankLoadState(_race);
			}
		}

		private static BankSlot ReadSlotFromMsg(Message _message, BankRace _race)
		{
			string name = _message.ReadString();
			BankSlot bankSlot = new BankSlot(name, _race);
			bankSlot.slot.AdvancedItemReceiveTime = (int)_message.ReadU32();
			int num = _message.ReadS32();
			bankSlot.item = new List<BankItem>(num);
			for (int i = 0; i < num; i++)
			{
				bankSlot.item.Add(ReadItemFromMsg(_message));
			}
			return bankSlot;
		}

		private static BankItem ReadItemFromMsg(Message _message)
		{
			BankItem bankItem = new BankItem();
			bankItem.location = _message.ReadString();
			bankItem.time = _message.ReadS64();
			bankItem.extraTime = _message.ReadS64();
			bankItem.item = ItemSerializer.Serialize(_message);
			return bankItem;
		}

		public static void Deserialize(Bank _bank, BankRace _race, Message _message)
		{
			if (_bank != null && _bank.IsValid() && _bank.IsBankLoaded(_race))
			{
				_message.WriteString(_bank.account);
				WriteDataToMsg(_bank.data, _message);
				if (_bank.slot != null)
				{
					int count = _bank.slot.Count;
					_message.WriteS32(count);
					foreach (BankSlot item in _bank.slot)
					{
						WriteSlotToMsg(item, _message);
					}
				}
				else
				{
					_message.WriteU32(0u);
				}
			}
		}

		private static void WriteDataToMsg(BankData _bankdata, Message _message)
		{
			if (_bankdata == null)
			{
				_bankdata = new BankData();
			}
			_message.WriteS32(_bankdata.deposit);
			_message.WriteString((_bankdata.password == null) ? string.Empty : _bankdata.password);
		}

		private static void WriteSlotToMsg(BankSlot _bankslot, Message _message)
		{
			if (_bankslot == null)
			{
				_bankslot = new BankSlot();
				_bankslot.slot = new BankSlotInfo();
				_bankslot.slot.name = string.Empty;
			}
			_message.WriteString(_bankslot.Name);
			_message.WriteU8((byte)_bankslot.Race);
			_message.WriteU32((uint)_bankslot.slot.AdvancedItemReceiveTime);
			if (_bankslot.item != null)
			{
				int num = _bankslot.item.Count;
				_message.WriteS32(num);
				for (int i = 0; i < num; i++)
				{
					WriteItemToMsg(_bankslot.item[i], _message);
				}
			}
			else
			{
				_message.WriteU32(0u);
			}
		}

		private static void WriteItemToMsg(BankItem _item, Message _message)
		{
			if (_item == null)
			{
				_item = new BankItem();
			}
			_message.WriteString(_item.location);
			_message.WriteS64(_item.time);
			_message.WriteS64(_item.extraTime);
			ItemSerializer.Deserialize(_item.item, _message);
		}
	}
}
