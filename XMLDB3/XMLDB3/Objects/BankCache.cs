using System;
using System.Collections;
using System.Collections.Generic;

namespace XMLDB3
{
	public class BankCache
	{
		public BankData bank;

		private string account;

		private int[] wealth;

		private LinkedHybridCache[] inventory;

		private bool[] loadState;

		public string Account => account;

		public BankCache()
		{
			account = null;
			bank = null;
			wealth = new int[3];
			loadState = new bool[3];
			inventory = new LinkedHybridCache[3];
		}

		public BankCache(Bank _bank)
		{
			wealth = new int[3];
			loadState = new bool[3];
			inventory = new LinkedHybridCache[3];
			Update(_bank);
		}

		public void Update(Bank _bank)
		{
			if (_bank == null || !_bank.IsValid())
			{
				throw new ArgumentException("유효하지 않은 은행 데이터입니다.", "_data");
			}
			account = _bank.account;
			bank = _bank.data;
			for (int i = 0; i < 3; i++)
			{
				if (_bank.IsBankLoaded((BankRace)i))
				{
					inventory[i] = CreateInventory(_bank.slot, (BankRace)i);
					loadState[i] = true;
				}
			}
		}

		private LinkedHybridCache CreateInventory(List<BankSlot> _slots, BankRace _race)
		{
			if (_slots != null)
			{
				LinkedHybridCache linkedHybridCache = new LinkedHybridCache();
				new ArrayList();
				{
					foreach (BankSlot _slot in _slots)
					{
						if (_slot != null && _slot.IsValid() && _slot.Race == _race)
						{
							linkedHybridCache.AddSection(_slot.Name, _slot.slot);
							if (_slot.item != null)
							{
								foreach (BankItem bankItem in _slot.item)
								{
									linkedHybridCache.AddItem(_slot.Name, bankItem.item.id, bankItem);
								}
							}
						}
					}
					return linkedHybridCache;
				}
			}
			return null;
		}

		public bool IsValid()
		{
			if (account != null && account != string.Empty)
			{
				return bank != null;
			}
			return false;
		}

		public void Invalidate()
		{
			bank = null;
			for (int i = 0; i < 3; i++)
			{
				loadState[i] = false;
			}
		}

		public static BankSlotInfo GetSlotInfo(ILinkItem _item)
		{
			if (_item.Section != null && _item.Section.Context != null)
			{
				return (BankSlotInfo)_item.Section.Context;
			}
			return null;
		}

		public bool IsRaceLoaded(BankRace _race)
		{
			if (_race != BankRace.None)
			{
				return loadState[(uint)_race];
			}
			return IsValid();
		}

		public Bank ToBank(BankRace _race)
		{
			if (IsValid() && IsRaceLoaded(_race))
			{
				Bank bank = new Bank();
				bank.account = account;
				bank.data = this.bank;
				if (_race != BankRace.None)
				{
					LinkedHybridCache linkedHybridCache = inventory[(uint)_race];
					if (linkedHybridCache != null)
					{
						ICollection section = linkedHybridCache.GetSection();
						if (section.Count > 0)
						{
							foreach (ISection item in section)
							{
								BankSlot value = new BankSlot((BankSlotInfo)item.Context, (BankItem[])item.ToArray(typeof(BankItem)));
								bank.slot.Add(value);
							}
						}
					}
					bank.SetBankLoadState(_race);
				}
				return bank;
			}
			return null;
		}

		public void SetWealth(BankRace _race, int _value)
		{
			if (3 > (int)_race)
			{
				wealth[(uint)_race] = _value;
			}
		}

		public int GetWealth(BankRace _race)
		{
			if (3 <= (int)_race)
			{
				return 0;
			}
			return wealth[(uint)_race];
		}

		public void AddItem(string _slotName, BankRace _race, BankItem _item)
		{
			inventory[(uint)_race].AddItem(_slotName, _item.item.id, _item);
		}

		public ISection FindSlot(string _slotName, BankRace _race)
		{
			return inventory[(uint)_race].FindSection(_slotName);
		}

		public BankSlotInfo FindSlotInfo(string _slotName, BankRace _race)
		{
			return (BankSlotInfo)inventory[(uint)_race].FindSection(_slotName).Context;
		}

		public void MoveSlot(string _slotName, BankRace _newRace, BankRace _oldRace, ILinkItem _item)
		{
			if (_newRace != _oldRace)
			{
				BankItem bankItem = (BankItem)_item.Context;
				inventory[(uint)_oldRace].RemoveItem(bankItem.item.id);
				AddItem(_slotName, _newRace, bankItem);
			}
			else
			{
				inventory[(uint)_newRace].MoveSection(_slotName, _item);
			}
		}

		public void RemoveItem(BankRace _race, BankItem _item)
		{
			inventory[(uint)_race].RemoveItem(_item.item.id);
		}
	}
}
