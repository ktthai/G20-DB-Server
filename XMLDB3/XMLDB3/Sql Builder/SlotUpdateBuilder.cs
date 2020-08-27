using System;
using System.Collections;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public sealed class SlotUpdateBuilder
    {
        public static void AddBankSlot(BankSlot bankSlot, string account, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var selCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Bank))
            {
                selCmd.Where(Mabinogi.SQL.Columns.Bank.Account, account);

                bool hasRows;
                using (var reader = selCmd.ExecuteReader())
                {
                    hasRows = reader.HasRows;
                }

                if (!hasRows)
                {
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Bank, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.Bank.Account, account);
                        cmd.Set(Mabinogi.SQL.Columns.Bank.Deposit, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Bank.HumanWealth, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Bank.GiantWealth, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Bank.ElfWealth, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Bank.UpdateTime, DateTime.Now);
                        cmd.Set(Mabinogi.SQL.Columns.Bank.Password, string.Empty);
                        cmd.Set(Mabinogi.SQL.Columns.Bank.CouponCode, string.Empty);

                        cmd.Execute();
                    }
                }
                else
                {
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Bank, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Bank.Account, account);
                        cmd.Set(Mabinogi.SQL.Columns.Bank.UpdateTime, DateTime.Now);

                        cmd.Execute();
                    }
                }
            }

            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.BankSlot, transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.BankSlot.Account, account);
                cmd.Set(Mabinogi.SQL.Columns.BankSlot.Name, bankSlot.slot.name);
                cmd.Set(Mabinogi.SQL.Columns.BankSlot.Race, bankSlot.Race);
                cmd.Set(Mabinogi.SQL.Columns.BankSlot.CouponCode, bankSlot.slot.itemHash);
                cmd.Set(Mabinogi.SQL.Columns.BankSlot.Race, bankSlot.Race);
                cmd.Set(Mabinogi.SQL.Columns.BankSlot.UpdateTime, DateTime.Now);
                cmd.Set(Mabinogi.SQL.Columns.BankSlot.Valid, 1);
                cmd.Set(Mabinogi.SQL.Columns.BankSlot.ReceiveTime, 0);

                cmd.Execute();
            }
        }

        public static void RemoveBankSlot(string account, string charName, SimpleConnection conn, SimpleTransaction transaction)
        {
            using(var selCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.BankSlot))
            {
                selCmd.Where(Mabinogi.SQL.Columns.BankSlot.Account, account);
                selCmd.Where(Mabinogi.SQL.Columns.BankSlot.Name, charName);

                if (!selCmd.ExecuteReader().HasRows)
                    return;
            }

            SimpleCommand cmd;
            Dictionary<long, byte> items = new Dictionary<long, byte>();
            string[] tables = new string[] { Mabinogi.SQL.Tables.Mabinogi.BankItemLarge, Mabinogi.SQL.Tables.Mabinogi.BankItemSmall, Mabinogi.SQL.Tables.Mabinogi.BankItemHuge, Mabinogi.SQL.Tables.Mabinogi.BankItemQuest };
            byte i = 1;

            foreach(string table in tables)
            {
                cmd = conn.GetDefaultSelectCommand(table);
            
                cmd.Set(Mabinogi.SQL.Columns.Item.ItemId, 0);
                cmd.Where(Mabinogi.SQL.Columns.BankItem.Account, account);
                cmd.Where(Mabinogi.SQL.Columns.BankItem.SlotName, charName);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(reader.GetInt64(Mabinogi.SQL.Columns.Item.ItemId), i);
                    }
                }
                i++;
            }

            
            cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.BankSlot, transaction);
            cmd.Where(Mabinogi.SQL.Columns.BankItem.Account, account);
            cmd.Where(Mabinogi.SQL.Columns.BankItem.SlotName, charName);
            cmd.Execute();

            tables = new string[] { Mabinogi.SQL.Tables.Mabinogi.ItemLargeId, Mabinogi.SQL.Tables.Mabinogi.ItemSmallId, Mabinogi.SQL.Tables.Mabinogi.ItemHugeId, Mabinogi.SQL.Tables.Mabinogi.ItemQuestId };
            i = 1;

            List<long> deleted;
            foreach (string table in tables)
            {
                deleted = new List<long>();
                foreach (KeyValuePair<long, byte> pair in items)
                {

                    if (pair.Value == i)
                    {
                        cmd = conn.GetDefaultDeleteCommand(table, transaction);
                        cmd.Where(Mabinogi.SQL.Columns.ItemId.Id, pair.Key);

                        cmd.Execute();
                        deleted.Add(pair.Key);
                    }
                }
                i++;

                foreach (long id in deleted)
                    items.Remove(id);
            }

            cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Bank, transaction);
            cmd.Set(Mabinogi.SQL.Columns.Bank.UpdateTime, DateTime.Now);
            cmd.Where(Mabinogi.SQL.Columns.Bank.Account, account);
            cmd.Execute();
        }

        public static void UpdateSlot(Bank _bank, BankCache _cache, SimpleConnection conn, SimpleTransaction transaction)
        {
            if (_bank.slot != null && _bank.slot.Count > 0)
            {
                Hashtable hashtable = new Hashtable();
                foreach (BankSlot item in _bank.slot)
                {
                    ISection section = _cache.FindSlot(item.Name, item.Race);
                    if (section == null)
                    {
                        throw new Exception("Bank slot [" + item.Name + "] doesn't exist!");
                    }
                    for (ILinkItem linkItem = section.First; linkItem != null; linkItem = linkItem.Next)
                    {
                        _ = (BankItem)linkItem.Context;
                        hashtable.Add(((BankItem)linkItem.Context).item.id, linkItem);
                    }
                }

                foreach (BankSlot item in _bank.slot)
                {
                    InventoryHash inventoryHash = new InventoryHash(item.Name);

                    foreach (BankItem bankItem in item.item)
                    {
                        ILinkItem linkItem2 = (ILinkItem)hashtable[bankItem.item.id];
                        if (linkItem2 == null)
                        {
                            ItemSqlBuilder.BankSelfUpdateItem(_bank.account, item.Name, item.Race, bankItem, conn, transaction);
                            _cache.AddItem(item.Name, item.Race, bankItem);
                        }
                        else
                        {
                            BankSlotInfo slotInfo = BankCache.GetSlotInfo(linkItem2);

                            ItemSqlBuilder.BankUpdateItem(item.slot, slotInfo, bankItem, (BankItem)linkItem2.Context, conn, transaction);
                            linkItem2.Context = bankItem;
                            if (item.Name != slotInfo.name)
                            {
                                _cache.MoveSlot(item.Name, item.Race, (BankRace)slotInfo.race, linkItem2);
                                linkItem2 = null;
                            }
                            hashtable.Remove(bankItem.item.id);
                        }
                        inventoryHash.Add(bankItem.item);
                    }
                    item.slot.strToHash = inventoryHash.ToString();
                    item.slot.itemHash = InventoryHashUtility.ComputeHash(item.slot.strToHash);
                    item.slot.updatetime = DateTime.Now;
                    ISection section2 = _cache.FindSlot(item.Name, item.Race);
                    if (section2 != null)
                    {
                        section2.Context = item.slot;
                    }



                    var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.BankSlot, transaction);
                    cmd.Set(Mabinogi.SQL.Columns.BankSlot.CouponCode, item.slot.itemHash);
                    cmd.Set(Mabinogi.SQL.Columns.BankSlot.ReceiveTime, item.slot.AdvancedItemReceiveTime);
                    cmd.Where(Mabinogi.SQL.Columns.BankSlot.Name, item.Name);

                    cmd.Execute();
                }
                foreach (ILinkItem value in hashtable.Values)
                {
                    BankItem bankItem2 = (BankItem)value.Context;
                    ItemSqlBuilder.BankDeleteItem(_bank.account, bankItem2, conn, transaction);
                    BankSlotInfo slotInfo2 = BankCache.GetSlotInfo(value);
                    if (slotInfo2 != null)
                    {
                        _cache.RemoveItem((BankRace)slotInfo2.race, bankItem2);
                    }
                    else
                    {
                        ExceptionMonitor.ExceptionRaised(new Exception("슬롯이 없습니다."), _bank.account, bankItem2.item.id);
                    }
                }
            }
        }
    }
}
