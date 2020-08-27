using Mabinogi.SQL;
using System;

namespace XMLDB3
{
	public class BankUpdateBuilder
	{
		public static void UpdateBank(Bank _bank, BankCache _cache, SimpleConnection conn, SimpleTransaction transaction)
		{
			if (_bank.account.ToLower() == _cache.Account.ToLower())
			{
				var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Bank, transaction);

				if (_bank.data.deposit != _cache.bank.deposit)
				{
					cmd.Set(Mabinogi.SQL.Columns.Bank.Deposit, _bank.data.deposit);
				}
				if (_bank.data.password != _cache.bank.password)
				{
					cmd.Set(Mabinogi.SQL.Columns.Bank.Password, _bank.data.password);
				}
				
				if(_bank.IsBankLoaded(BankRace.Elf) && _bank.elfWealth != _cache.GetWealth(BankRace.Elf))
				{
					cmd.Set(Mabinogi.SQL.Columns.Bank.ElfWealth, _bank.elfWealth);
					_cache.SetWealth(BankRace.Elf, _bank.elfWealth);
				}

                if (_bank.IsBankLoaded(BankRace.Human) && _bank.humanWealth != _cache.GetWealth(BankRace.Human))
                {
                    cmd.Set(Mabinogi.SQL.Columns.Bank.HumanWealth, _bank.humanWealth);
					_cache.SetWealth(BankRace.Human, _bank.humanWealth);
				}

                if (_bank.IsBankLoaded(BankRace.Giant) && _bank.giantWealth != _cache.GetWealth(BankRace.Giant))
                {
                    cmd.Set(Mabinogi.SQL.Columns.Bank.GiantWealth, _bank.giantWealth);
					_cache.SetWealth(BankRace.Giant, _bank.giantWealth);
				}


				if (!InventoryHashUtility.CheckHash(_cache.bank.hash, _cache.Account, _cache.bank.deposit, _cache.bank.updatetime) && _bank.data.deposit == _cache.bank.deposit)
				{
					_bank.data.hash = _cache.bank.hash;
				}
				else
				{
					_bank.data.hash = InventoryHashUtility.ComputeHash(_bank.account, _bank.data.deposit);
				}
				_cache.bank = _bank.data;
				cmd.Set(Mabinogi.SQL.Columns.Bank.CouponCode, _bank.data.hash);

				cmd.Where(Mabinogi.SQL.Columns.Bank.Account, _bank.account);
				return;
			}
			throw new Exception($"Bank account [{_bank.account}] is different from [{_cache.Account}]");
		}
	}
}
