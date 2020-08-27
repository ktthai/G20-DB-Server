using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class EquipmentCollectionUpdateBuilder
    {

        public static void Build(EquipmentCollection _equipmentCollection, EquipmentCollectionCache _cache, SimpleConnection conn, SimpleTransaction transaction)
        {
            Dictionary<long, bool> hashtable = new Dictionary<long, bool>();

            if (_equipmentCollection.Account.ToLower() == _cache.Account.ToLower())
            {
                if (_equipmentCollection != null && _equipmentCollection.IsValid() && _equipmentCollection.Account == _cache.Account)
                {
                    if (_equipmentCollection.item != null && _equipmentCollection.item.Count > 0)
                    {
                        if (_cache == null || !_cache.IsValid() || _cache.Inventory == null)
                        {
                            foreach (CollectionItem item2 in _equipmentCollection.item)
                            {
                                SelfUpdateEquipCollectionItem(_equipmentCollection.Account, item2, conn, transaction);
                            }
                        }
                        else
                        {
                            foreach (CollectionItem collectionItem in _equipmentCollection.item)
                            {
                                if (collectionItem.item != null)
                                {
                                    CollectionItem collectionItem2 = _cache.FindItem(collectionItem.item.id);
                                    if (collectionItem2 != null)
                                    {
                                        CheckedSelfUpdateEquipCollectionItem(_equipmentCollection.Account, collectionItem, collectionItem2, conn, transaction);
                                        hashtable.Add(collectionItem.item.id, true);
                                    }
                                    else
                                    {
                                        SelfUpdateEquipCollectionItem(_equipmentCollection.Account, collectionItem, conn, transaction);
                                    }
                                }
                            }
                        }
                    }
                    if (_cache != null && _cache.IsValid() && _cache.Inventory != null)
                    {
                        foreach (CollectionItem value in _cache.Inventory.Values)
                        {
                            if (!hashtable.ContainsKey(value.item.id))
                            {
                                ItemSqlBuilder.EquipmentCollectionDeleteItem(_equipmentCollection.Account, value, conn, transaction);
                            }
                        }
                    }
                }
            }
            else
            {
                throw new Exception($"EquipmentCollection account [{_equipmentCollection.Account}] is different from {_cache.Account}");
            }
        }

        public static void CheckedSelfUpdateEquipCollectionItem(string _accountId, CollectionItem _new, CollectionItem _old, SimpleConnection conn, SimpleTransaction transaction)
        {
            if (1 == _new.item.storedtype)
            {
                if (_new.lockTime != _old.lockTime || _new.item.id != _old.item.id || ItemUpdateChecker.CheckLargeItem(_new.item, _old.item))
                {
                    SelfUpdateEquipCollectionItem(_accountId, _new, conn, transaction);
                }
            }
        }

        public static void SelfUpdateEquipCollectionItem(string _accountId, CollectionItem _item, SimpleConnection conn, SimpleTransaction transaction)
        {
            DateTime now = DateTime.Now;
            bool chk;

            using (var chkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.EquipCollectItemLarge))
            {
                chkCmd.Where(Mabinogi.SQL.Columns.EquipCollectItem.ItemLoc, 4);
                chkCmd.Where(Mabinogi.SQL.Columns.Item.ItemId, _item.item.id);

                using (var reader = chkCmd.ExecuteReader())
                    chk = reader.HasRows;
            }



            if (chk)
            {
                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.EquipCollectItemLarge, transaction))
                {
                    cmd.Where(Mabinogi.SQL.Columns.EquipCollectItem.ItemLoc, 4);
                    cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, _item.item.id);

                    cmd.Set(Mabinogi.SQL.Columns.EquipCollectItem.Account, _accountId);
                    cmd.Set(Mabinogi.SQL.Columns.EquipCollectItem.LockTime, _item.lockTime);
                    cmd.Set(Mabinogi.SQL.Columns.EquipCollectItem.UpdateTime, now);

                    cmd.Set(Mabinogi.SQL.Columns.Item.Class, _item.item.Class);
                    cmd.Set(Mabinogi.SQL.Columns.Item.PosX, _item.item.pos_x);
                    cmd.Set(Mabinogi.SQL.Columns.Item.PosY, _item.item.pos_y);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Color1, _item.item.color_01);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Color2, _item.item.color_02);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Color3, _item.item.color_03);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Price, _item.item.price);
                    cmd.Set(Mabinogi.SQL.Columns.Item.SellingPrice, _item.item.sellingprice);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Bundle, _item.item.bundle);
                    cmd.Set(Mabinogi.SQL.Columns.Item.LinkedPocket, _item.item.linked_pocket);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Figure, _item.item.figure);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Flag, _item.item.flag);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Durability, _item.item.durability);
                    cmd.Set(Mabinogi.SQL.Columns.Item.DurabilityMax, _item.item.durability_max);
                    cmd.Set(Mabinogi.SQL.Columns.Item.OriginalDurabilityMax, _item.item.origin_durability_max);
                    cmd.Set(Mabinogi.SQL.Columns.Item.AttackMin, _item.item.attack_min);
                    cmd.Set(Mabinogi.SQL.Columns.Item.AttackMax, _item.item.attack_max);
                    cmd.Set(Mabinogi.SQL.Columns.Item.WAttackMin, _item.item.wattack_min);
                    cmd.Set(Mabinogi.SQL.Columns.Item.WAttackMax, _item.item.wattack_max);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Balance, _item.item.balance);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Critical, _item.item.critical);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Defence, _item.item.defence);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Protect, _item.item.protect);
                    cmd.Set(Mabinogi.SQL.Columns.Item.EffectiveRange, _item.item.effective_range);
                    cmd.Set(Mabinogi.SQL.Columns.Item.AttackSpeed, _item.item.attack_speed);
                    cmd.Set(Mabinogi.SQL.Columns.Item.DownHitCount, _item.item.down_hit_count);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Experience, _item.item.experience);
                    cmd.Set(Mabinogi.SQL.Columns.Item.ExpPoint, _item.item.exp_point);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Upgraded, _item.item.upgraded);
                    cmd.Set(Mabinogi.SQL.Columns.Item.UpgradeMax, _item.item.upgrade_max);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Grade, _item.item.grade);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Prefix, _item.item.prefix);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Suffix, _item.item.suffix);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Data, _item.item.data);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Option, _item.item.options);
                    cmd.Set(Mabinogi.SQL.Columns.Item.Expiration, _item.item.expiration);

                    if (cmd.Execute() == 0)
                    {
                        UpdateItemLoc(now, _item, conn, transaction);

                        InsertItem(_accountId, now, _item, conn, transaction);
                    }
                }
            }
            else
            {
                UpdateItemLoc(now, _item, conn, transaction);
                InsertItem(_accountId, now, _item, conn, transaction);
            }
        }

        private static void UpdateItemLoc(DateTime now, CollectionItem _item, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var chkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.ItemLargeId))
            {
                chkCmd.Where(Mabinogi.SQL.Columns.ItemId.Id, _item.item.id);
                using (var reader = chkCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var loc = reader.GetByte(Mabinogi.SQL.Columns.ItemId.ItemLoc);
                        reader.Close();

                        if (loc == 0)
                        {
                            using (var charChk = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CharItemLarge))
                            {
                                charChk.Set(Mabinogi.SQL.Columns.CharItemLarge.Id, 0);
                                charChk.Where(Mabinogi.SQL.Columns.ItemId.Id, _item.item.id);
                                charChk.Where(Mabinogi.SQL.Columns.ItemId.ItemLoc, 0);
                                using (var reader2 = charChk.ExecuteReader())
                                {
                                    if (reader2.Read())
                                    {
                                        var charID = reader2.GetInt64(Mabinogi.SQL.Columns.CharItemLarge.Id);
                                        reader2.Close();

                                        using (var cmd1 = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Character, transaction))
                                        {
                                            cmd1.Where(Mabinogi.SQL.Columns.Character.Id, charID);
                                            cmd1.Set(Mabinogi.SQL.Columns.Character.UpdateTime, now);
                                            cmd1.Execute();
                                        }

                                        using (var cmd1 = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CharItemLarge, transaction))
                                        {
                                            cmd1.Where(Mabinogi.SQL.Columns.CharItemLarge.ItemId, _item.item.id);
                                            cmd1.Where(Mabinogi.SQL.Columns.CharItemLarge.ItemLoc, 0);
                                            cmd1.Execute();
                                        }

                                        using (var cmd1 = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.ItemLargeId, transaction))
                                        {
                                            cmd1.Where(Mabinogi.SQL.Columns.ItemId.Id, _item.item.id);
                                            cmd1.Set(Mabinogi.SQL.Columns.ItemId.ItemLoc, 4);
                                            cmd1.Execute();
                                        }
                                    }
                                }
                            }
                        }
                        else if (loc != 4)
                        {
                            reader.Close();
                            using (var cmd1 = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.ItemLargeId, transaction))
                            {
                                cmd1.Set(Mabinogi.SQL.Columns.ItemId.Id, _item.item.id);
                                cmd1.Set(Mabinogi.SQL.Columns.ItemId.ItemLoc, 4);
                                cmd1.Execute();
                            }
                        }
                    }
                }
            }
        }

        private static void InsertItem(string _accountId, DateTime now, CollectionItem _item, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.EquipCollectItemLarge, transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.EquipCollectItem.ItemLoc, 4);
                cmd.Set(Mabinogi.SQL.Columns.Item.ItemId, _item.item.id);

                cmd.Set(Mabinogi.SQL.Columns.EquipCollectItem.Account, _accountId);
                cmd.Set(Mabinogi.SQL.Columns.EquipCollectItem.LockTime, _item.lockTime);
                cmd.Set(Mabinogi.SQL.Columns.EquipCollectItem.UpdateTime, now);

                cmd.Set(Mabinogi.SQL.Columns.Item.Class, _item.item.Class);
                cmd.Set(Mabinogi.SQL.Columns.Item.PosX, _item.item.pos_x);
                cmd.Set(Mabinogi.SQL.Columns.Item.PosY, _item.item.pos_y);
                cmd.Set(Mabinogi.SQL.Columns.Item.Color1, _item.item.color_01);
                cmd.Set(Mabinogi.SQL.Columns.Item.Color2, _item.item.color_02);
                cmd.Set(Mabinogi.SQL.Columns.Item.Color3, _item.item.color_03);
                cmd.Set(Mabinogi.SQL.Columns.Item.Price, _item.item.price);
                cmd.Set(Mabinogi.SQL.Columns.Item.SellingPrice, _item.item.sellingprice);
                cmd.Set(Mabinogi.SQL.Columns.Item.Bundle, _item.item.bundle);
                cmd.Set(Mabinogi.SQL.Columns.Item.LinkedPocket, _item.item.linked_pocket);
                cmd.Set(Mabinogi.SQL.Columns.Item.Figure, _item.item.figure);
                cmd.Set(Mabinogi.SQL.Columns.Item.Flag, _item.item.flag);
                cmd.Set(Mabinogi.SQL.Columns.Item.Durability, _item.item.durability);
                cmd.Set(Mabinogi.SQL.Columns.Item.DurabilityMax, _item.item.durability_max);
                cmd.Set(Mabinogi.SQL.Columns.Item.OriginalDurabilityMax, _item.item.origin_durability_max);
                cmd.Set(Mabinogi.SQL.Columns.Item.AttackMin, _item.item.attack_min);
                cmd.Set(Mabinogi.SQL.Columns.Item.AttackMax, _item.item.attack_max);
                cmd.Set(Mabinogi.SQL.Columns.Item.WAttackMin, _item.item.wattack_min);
                cmd.Set(Mabinogi.SQL.Columns.Item.WAttackMax, _item.item.wattack_max);
                cmd.Set(Mabinogi.SQL.Columns.Item.Balance, _item.item.balance);
                cmd.Set(Mabinogi.SQL.Columns.Item.Critical, _item.item.critical);
                cmd.Set(Mabinogi.SQL.Columns.Item.Defence, _item.item.defence);
                cmd.Set(Mabinogi.SQL.Columns.Item.Protect, _item.item.protect);
                cmd.Set(Mabinogi.SQL.Columns.Item.EffectiveRange, _item.item.effective_range);
                cmd.Set(Mabinogi.SQL.Columns.Item.AttackSpeed, _item.item.attack_speed);
                cmd.Set(Mabinogi.SQL.Columns.Item.DownHitCount, _item.item.down_hit_count);
                cmd.Set(Mabinogi.SQL.Columns.Item.Experience, _item.item.experience);
                cmd.Set(Mabinogi.SQL.Columns.Item.ExpPoint, _item.item.exp_point);
                cmd.Set(Mabinogi.SQL.Columns.Item.Upgraded, _item.item.upgraded);
                cmd.Set(Mabinogi.SQL.Columns.Item.UpgradeMax, _item.item.upgrade_max);
                cmd.Set(Mabinogi.SQL.Columns.Item.Grade, _item.item.grade);
                cmd.Set(Mabinogi.SQL.Columns.Item.Prefix, _item.item.prefix);
                cmd.Set(Mabinogi.SQL.Columns.Item.Suffix, _item.item.suffix);
                cmd.Set(Mabinogi.SQL.Columns.Item.Data, _item.item.data);
                cmd.Set(Mabinogi.SQL.Columns.Item.Option, _item.item.options);
                cmd.Set(Mabinogi.SQL.Columns.Item.Expiration, _item.item.expiration);
            }
        }
    }
}
