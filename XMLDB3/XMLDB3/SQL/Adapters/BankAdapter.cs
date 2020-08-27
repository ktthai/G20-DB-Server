using Mabinogi;
using Mabinogi.SQL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XMLDB3
{
    public class BankAdapter : SqlAdapter
    {
        private static string[] wealthColum = new string[3]
        {
            "humanWealth",
            "elfWealth",
            "giantWealth"
        };

        public BankAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Bank;

        public static string GetWealthColumn(BankRace _race)
        {
            return wealthColum[(uint)_race];
        }

        private Bank _Read(string _account, string _charName, bool bRaceSpecified, BankRace _race)
        {
            WorkSession.WriteStatus("BankSqlAdapter._Read() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("BankSqlAdapter._Read() : 데이터베이스와 연결합니다");
                long timestamp = Stopwatch.GetTimestamp();
                Bank bank;
                using (var conn = Connection)
                {

                    if (!CheckBank(_account, _charName, conn))
                        return null;

                    if (bRaceSpecified)
                    {
                        if (_race != BankRace.None)
                        {
                            bank = GetBank(_account, _charName, _race, conn);
                            bank.SetBankLoadState(_race);
                        }
                        else
                        {
                            bank = SelectBank(_account, conn);
                        }
                    }
                    else
                    {
                        bank = GetBank(_account, _charName, conn);
                    }
                }
                return bank;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _account);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("BankSqlAdapter._Read() : 연결을 종료합니다");
            }
        }

        public bool IsValidCache(BankCache _cache)
        {
            WorkSession.WriteStatus("BankSqlAdapter.IsValidCache() : 함수에 진입하였습니다");
            if (_cache == null || !_cache.IsValid())
            {
                return false;
            }
            try
            {
                WorkSession.WriteStatus("BankSqlAdapter.IsValidCache() : 데이터베이스에 연결합니다");
                WorkSession.WriteStatus("BankSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜를 얻어옵니다");
                if (_cache == null || !_cache.IsValid())
                {
                    return false;
                }

                using (var mc = Connection.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Bank))
                {
                    mc.Where(Mabinogi.SQL.Columns.Bank.Account, _cache.Account);

                    using (var reader = mc.ExecuteReader())
                    {
                        long ticks = UpdateUtility.CacheMissDate.Ticks;
                        DateTime dateTime = reader.GetDateTime(Mabinogi.SQL.Columns.Bank.UpdateTime);
                        if (dateTime != null)
                        {
                            if (ticks >= dateTime.Ticks)
                            {
                                return false;
                            }

                            if (dateTime.Ticks <= _cache.bank.updatetime.Ticks)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _cache.Account);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _cache.Account);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("BankSqlAdapter.IsValidCache() : 데이터베이스에 연결을 종료합니다");
            }
        }


        public Bank Read(string _account, string _charName, BankRace _race, BankCache _cache)
        {
            WorkSession.WriteStatus("BankSqlAdapter.Read() : 함수에 진입하였습니다");
            try
            {
                if (_cache == null)
                {
                    return _Read(_account, _charName, bRaceSpecified: true, _race);
                }
                Bank bank = null;
                if (!IsValidCache(_cache))
                {
                    _cache.Invalidate();
                }
                if (_cache.IsRaceLoaded(_race))
                {
                    bank = _cache.ToBank(_race);
                }
                else
                {
                    bank = _Read(_account, _charName, bRaceSpecified: true, _race);
                    _cache.Update(bank);
                }
                return bank;
            }
            catch (Exception ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message);
                return null;
            }
        }

        public bool Write(string _charName, Bank _data, BankCache _cache)
        {
            WorkSession.WriteStatus("BankSqlAdapter.Write() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            using (var conn = Connection)
            {
                try
                {
                    WorkSession.WriteStatus("BankSqlAdapter.Write() : 저장되어 있는 데이터를 읽어옵니다");
                    _cache = ValidateBankCache(_charName, _data, _cache);


                    transaction = conn.BeginTransaction();
                    BankUpdateBuilder.UpdateBank(_data, _cache, conn, transaction);
                    SlotUpdateBuilder.UpdateSlot(_data, _cache, conn, transaction);

                    UpdateBank(_data, _cache, conn, transaction);

                    WorkSession.WriteStatus("BankSqlAdapter.Write() : 변경점이 없습니다. 쿼리를 생략합니다");
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex, _data.account);
                    WorkSession.WriteStatus(ex.Message);
                    return false;
                }
            }
        }

        private BankCache ValidateBankCache(string _charName, Bank _bank, BankCache _bankCache)
        {
            if (_bankCache == null)
            {
                WorkSession.WriteStatus("BankSqlAdapter.ValidateBankCache() : 저장되어 있는 은행 데이터가 없어 새로 만듭니다.");
                return new BankCache(_Read(_bank.account, _charName, bRaceSpecified: false, BankRace.None));
            }
            bool flag = false;
            if (!IsValidCache(_bankCache))
            {
                _bankCache.Invalidate();
                flag = true;
            }
            BankRace bankRace = BankRace.None;
            bool bRaceSpecified = true;
            for (int i = 0; i < 3; i++)
            {
                BankRace bankRace2 = (BankRace)i;
                if (_bank.IsBankLoaded(bankRace2) && !_bankCache.IsRaceLoaded(bankRace2))
                {
                    flag = true;
                    if (bankRace != BankRace.None)
                    {
                        bankRace = BankRace.None;
                        bRaceSpecified = false;
                        break;
                    }
                    bankRace = bankRace2;
                }
            }
            if (flag)
            {
                _bankCache.Update(_Read(_bank.account, _charName, bRaceSpecified, bankRace));
            }
            return _bankCache;
        }

        public bool AddBankDeposit(string account, int repayMoney, int maxRemainMoney, string hash, SimpleConnection conn, SimpleTransaction transaction, out int remainMoney)
        {
            // PROCEDURE: SelectBankDeposit
            remainMoney = GetBankDeposit(account, conn);

            if (remainMoney + repayMoney > maxRemainMoney)
            {
                WorkSession.WriteStatus("BankAdapter.AddBankDeposit() : 은행에 돈이 너무 많습니다.");
                return false;
            }



            // PROCEDURE: AddBankDeposit
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Bank, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.Bank.Account, account);
                cmd.Set(Mabinogi.SQL.Columns.Bank.Deposit, remainMoney + repayMoney);
                cmd.Set(Mabinogi.SQL.Columns.Bank.CouponCode, hash);

                WorkSession.WriteStatus("BankAdapter.AddBankDeposit(): 은행에 입금합니다.");
                if (cmd.Execute() < 1)
                {
                    WorkSession.WriteStatus("BankAdapter.AddBankDeposit() : 은행에 돈이 너무 많습니다.");
                    return false;
                }
            }
            return true;
        }

        public bool WithdrawBankDeposit(string account, int money, string hash, SimpleConnection conn, SimpleTransaction transaction, out int remainMoney)
        {
            // PROCEDURE: WithdrawBankDeposit

            remainMoney = GetBankDeposit(account, conn);

            if (remainMoney < money)
                return false;

            remainMoney -= money;

            if (remainMoney >= 0)
            {
                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Bank, transaction))
                {
                    cmd.Where(Mabinogi.SQL.Columns.Bank.Account, account);
                    cmd.Set(Mabinogi.SQL.Columns.Bank.Deposit, remainMoney);
                    cmd.Set(Mabinogi.SQL.Columns.Bank.CouponCode, hash);
                    cmd.Execute();
                }

                return true;
            }
            return false;
        }

        public int GetBankDeposit(string account, SimpleConnection conn)
        {
            // PROCEDURE: SelectBankDeposit
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Bank))
            {
                cmd.Where(Mabinogi.SQL.Columns.Bank.Account, account);
                cmd.Set(Mabinogi.SQL.Columns.Bank.Deposit, 0);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return reader.GetInt32(Mabinogi.SQL.Columns.Bank.Deposit);
                    else
                        return 0;
                }
            }
        }

        public bool WriteEx(Bank _bank, CharacterInfo _character, BankCache _bankCache, CharacterInfo _charCache, CharacterAdapter _charAdapter, out Message _outBuildResult)
        {
            return _WriteEx(_bank, _character, _bankCache, _charCache, _charAdapter, _forceUpdate: false, out _outBuildResult);
        }

        private bool _WriteEx(Bank _bank, CharacterInfo _character, BankCache _bankCache, CharacterInfo _charCache, CharacterAdapter _charAdapter, bool _forceUpdate, out Message _outBuildResult)
        {
            WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 함수에 진입하였습니다");
            _outBuildResult = new Message();

            try
            {
                WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 저장되어 있는 데이터를 읽어옵니다");
                if (_charCache == null)
                {
                    WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 저장되어 있는 캐릭터 데이터가 없어 새로 만듭니다.");
                    _charCache = _charAdapter.Read(_character.id, null);
                }
                else if (!_charAdapter.IsValidCache(_charCache))
                {
                    _charCache = _charAdapter.Read(_character.id, null);
                }

                _bankCache = ValidateBankCache(_character.name, _bank, _bankCache);


                SimpleTransaction transaction = null;
                using (var conn = Connection)
                {
                    try
                    {

                        transaction = conn.BeginTransaction();
                        UpdateSlot(_bank, _bankCache, conn, transaction);
                        InventoryUpdateBuilder.Build(_character.id, _character.inventory, _charCache.inventory, _forceUpdate, conn, transaction, out _character.strToHash);
                        _character.inventoryHash = InventoryHashUtility.ComputeHash(_character.strToHash);

                        BankUpdateBuilder.UpdateBank(_bank, _bankCache, conn, transaction);

                        CharacterUpdateBuilder.Build(_character, _charCache, conn, transaction, out _outBuildResult);


                        WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 데이터베이스와 연결합니다");
                        long timestamp = Stopwatch.GetTimestamp();

                        UpdateBank(_bank, _bankCache, conn, transaction);

                        _charAdapter.UpdateMeta(_character, _charCache, conn, transaction);

                        transaction.Commit();
                        CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctBankWrite, Stopwatch.GetElapsedMilliseconds(timestamp));
                        WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 수행할 명령이 없습니다.");
                        return true;


                    }
                    catch (SimpleSqlException ex)
                    {
                        transaction?.Rollback();
                        if (!_forceUpdate && ItemSqlBuilder.ForceUpdateRetry(ex))
                        {
                            WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 아이템 오류로 재시도합니다.");
                            ExceptionMonitor.ExceptionRaised(ex);
                            WorkSession.WriteStatus(ex.Message, ex.Number);
                            _bankCache.Invalidate();
                            return _WriteEx(_bank, _character, _bankCache, null, _charAdapter, _forceUpdate: true, out _outBuildResult);
                        }
                        ExceptionMonitor.ExceptionRaised(ex);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("BankSqlAdapter.WriteEx() : 연결을 종료합니다");
                    }
                }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _bank.account);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
        }



        #region Bank SQL Methods
        private bool CheckBank(string account, string charName, SimpleConnection conn)
        {
            using (var mc = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.BankSlot))
            {
                mc.Where(Mabinogi.SQL.Columns.BankSlot.Account, account);
                mc.Where(Mabinogi.SQL.Columns.BankSlot.Name, charName);

                using (var reader = mc.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public static Bank SelectBank(string account, SimpleConnection conn)
        {
            using (var mc = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Bank))
            {
                mc.Where(Mabinogi.SQL.Columns.Bank.Account, account);

                using (var reader = mc.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    Bank bank = new Bank();
                    bank.account = reader.GetString(Mabinogi.SQL.Columns.Bank.Account);
                    bank.data = new BankData();
                    bank.data.deposit = reader.GetInt32(Mabinogi.SQL.Columns.Bank.Deposit);
                    bank.data.updatetime = reader.GetDateTime(Mabinogi.SQL.Columns.Bank.UpdateTime);
                    bank.data.password = reader.GetString(Mabinogi.SQL.Columns.Bank.Password);
                    bank.humanWealth = reader.GetInt32(Mabinogi.SQL.Columns.Bank.HumanWealth);
                    bank.elfWealth = reader.GetInt32(Mabinogi.SQL.Columns.Bank.ElfWealth);
                    bank.giantWealth = reader.GetInt32(Mabinogi.SQL.Columns.Bank.GiantWealth);
                    bank.data.hash = reader.GetString(Mabinogi.SQL.Columns.Bank.CouponCode);

                    return bank;
                }
            }
        }

        private Bank GetBank(string account, string charName, SimpleConnection conn)
        {
            return GetBank(account, charName, BankRace.None, conn);
        }

        private Bank GetBank(string account, string charName, BankRace race, SimpleConnection conn)
        {
            Bank bank = SelectBank(account, conn);

            if (bank != null)
            {
                PopulateSlots(account, race, bank, conn);

                for (byte i = 1; i != 5; i++)
                    PopulateItems(account, race, i, conn, bank);
            }


            foreach (BankSlot slot in bank.slot)
            {
                InventoryHash inventoryHash = new InventoryHash(slot.Name);
                if (slot.item != null)
                {
                    foreach (BankItem bankItem in slot.item)
                    {
                        inventoryHash.Add(bankItem.item);
                    }
                }
                slot.slot.strToHash = inventoryHash.ToString();
            }

            return bank;
        }

        private void PopulateSlots(string account, BankRace race, Bank bank, SimpleConnection conn)
        {
            using (var mc = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.BankSlot))
            {
                mc.Where(Mabinogi.SQL.Columns.BankSlot.Account, account);
                if (race != BankRace.None)
                    mc.Where(Mabinogi.SQL.Columns.BankSlot.Race, (byte)race);

                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BankRace raceRef;

                        if (race == BankRace.None)
                        {
                            raceRef = (BankRace)reader.GetByte(Mabinogi.SQL.Columns.BankSlot.Race);
                        }
                        else
                        {
                            raceRef = race;
                        }

                        string name = reader.GetString(Mabinogi.SQL.Columns.BankSlot.Name);
                        BankSlot bankSlot = new BankSlot(name, raceRef);

                        bankSlot.slot.updatetime = reader.GetDateTime(Mabinogi.SQL.Columns.BankSlot.UpdateTime);
                        bankSlot.slot.itemHash = reader.GetString(Mabinogi.SQL.Columns.BankSlot.CouponCode);

                        bank.slot.Add(bankSlot);
                    }
                }
            }
        }
        private void PopulateItems(string account, BankRace race, byte type, SimpleConnection conn, Bank bank)
        {
            SimpleCommand cmd;

            switch (type)
            {
                case 1:
                    cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.BankItemLarge);
                    break;
                case 2:
                    cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.BankItemSmall);
                    break;
                case 3:
                    cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.BankItemHuge);
                    break;
                case 4:
                    cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.BankItemQuest);
                    break;
                default: throw new Exception("Unexpected type!");
            }

            cmd.Where(Mabinogi.SQL.Columns.BankItem.Account, account);
            if (race != BankRace.None)
                cmd.Where(Mabinogi.SQL.Columns.BankItem.Race, (byte)race);

            using (var reader = cmd.ExecuteReader())
            {
                string slotName;
                BankSlot slot;

                while (reader.Read())
                {
                    BankItem item = new BankItem();

                    item.time = reader.GetInt64(Mabinogi.SQL.Columns.BankItem.Time);
                    item.location = reader.GetString(Mabinogi.SQL.Columns.BankItem.Location);
                    item.extraTime = reader.GetInt64(Mabinogi.SQL.Columns.BankItem.ExtraTime);

                    slotName = reader.GetString(Mabinogi.SQL.Columns.BankItem.SlotName);

                    item.item = ItemSqlBuilder.GetItem(type, reader);

                    slot = bank.slot.Where(x => x.Name == slotName).First();
                    if (slot != null)
                        slot.item.Add(item);
                }
            }
        }

        private bool UpdateBank(Bank bank, BankCache cache, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Bank, transaction))
            {
                bool result = false;
                if (bank.account.ToLower() == cache.Account.ToLower())
                {

                    if (bank.data.deposit != cache.bank.deposit)
                    {
                        cmd.Set(Mabinogi.SQL.Columns.Bank.Deposit, bank.data.deposit);
                        result = true;
                    }
                    if (bank.data.password != cache.bank.password)
                    {
                        cmd.Set(Mabinogi.SQL.Columns.Bank.Password, bank.data.password);
                        result = true;
                    }

                    if (bank.IsBankLoaded(BankRace.Human) && bank.humanWealth != cache.GetWealth(BankRace.Human))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.Bank.HumanWealth, bank.humanWealth);
                        result = true;
                    }

                    if (bank.IsBankLoaded(BankRace.Elf) && bank.elfWealth != cache.GetWealth(BankRace.Elf))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.Bank.ElfWealth, bank.elfWealth);
                        result = true;
                    }

                    if (bank.IsBankLoaded(BankRace.Giant) && bank.giantWealth != cache.GetWealth(BankRace.Giant))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.Bank.GiantWealth, bank.giantWealth);
                        result = true;
                    }

                    if (!InventoryHashUtility.CheckHash(cache.bank.hash, cache.Account, cache.bank.deposit, cache.bank.updatetime) && bank.data.deposit == cache.bank.deposit)
                    {
                        bank.data.hash = cache.bank.hash;
                        result = true;
                    }
                    else
                    {
                        bank.data.hash = InventoryHashUtility.ComputeHash(bank.account, bank.data.deposit);
                        result = true;
                    }
                    cache.bank = bank.data;
                }

                if (result)
                {
                    cache.bank.updatetime = DateTime.Now;
                    cmd.Set(Mabinogi.SQL.Columns.Bank.UpdateTime, cache.bank.updatetime);
                }

                cmd.Set(Mabinogi.SQL.Columns.Bank.CouponCode, bank.data.hash);


                cmd.Where(Mabinogi.SQL.Columns.Bank.Account, bank.account);
                cmd.Execute();

                return result;
            }

            throw new Exception($"Bank account [{bank.account}] is different from {cache.Account}");
        }

        private void UpdateSlot(Bank bank, BankCache cache, SimpleConnection conn, SimpleTransaction transaction)
        {

            if (bank.slot != null && bank.slot.Count > 0)
            {
                Dictionary<long, ILinkItem> itemsToRemove = new Dictionary<long, ILinkItem>();

                foreach (BankSlot slot in bank.slot)
                {
                    ISection section = cache.FindSlot(slot.Name, slot.Race);
                    if (section == null)
                    {
                        throw new Exception("Bank slot [" + slot.Name + "] doesn't exist!");
                    }
                    for (ILinkItem linkItem = section.First; linkItem != null; linkItem = linkItem.Next)
                    {
                        itemsToRemove.Add(((BankItem)linkItem.Context).item.id, linkItem);
                    }
                }

                foreach (BankSlot slot in bank.slot)
                {
                    InventoryHash inventoryHash = new InventoryHash(slot.Name);

                    foreach (BankItem bankItem in slot.item)
                    {
                        ILinkItem linkItem2;


                        if (itemsToRemove.TryGetValue(bankItem.item.id, out linkItem2))
                        {
                            BankSlotInfo slotInfo = BankCache.GetSlotInfo(linkItem2);
                            ItemSqlBuilder.BankUpdateItem(slot.slot, slotInfo, bankItem, (BankItem)linkItem2.Context, conn, transaction);
                            linkItem2.Context = bankItem;
                            if (slot.Name != slotInfo.name)
                            {
                                cache.MoveSlot(slot.Name, slot.Race, (BankRace)slotInfo.race, linkItem2);
                                linkItem2 = null;
                            }
                            itemsToRemove.Remove(bankItem.item.id);
                        }
                        else
                        {
                            ItemSqlBuilder.BankSelfUpdateItem(bank.account, slot.Name, slot.Race, bankItem, conn, transaction);
                            cache.AddItem(slot.Name, slot.Race, bankItem);
                        }
                        
                        inventoryHash.Add(bankItem.item);
                    }
                    slot.slot.strToHash = inventoryHash.ToString();
                    slot.slot.itemHash = InventoryHashUtility.ComputeHash(slot.slot.strToHash);
                    slot.slot.updatetime = DateTime.Now;

                    UpdateSlotCoupon(slot.Name, slot.slot.itemHash, conn, transaction);
                }
            }
        }

        private void UpdateSlotCoupon(string slotName, string couponCode, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.BankSlot, transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.BankSlot.CouponCode, couponCode);
                cmd.Where(Mabinogi.SQL.Columns.BankSlot.Name, slotName);
                cmd.Set(Mabinogi.SQL.Columns.BankSlot.UpdateTime, DateTime.Now);

                cmd.Execute();
            }
        }
        #endregion Bank SQL Methods
    }
}
