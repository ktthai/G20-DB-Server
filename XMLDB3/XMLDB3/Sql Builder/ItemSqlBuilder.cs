using System;
using System.Text.Json;
using Mabinogi.SQL;

namespace XMLDB3
{
    public sealed class ItemSqlBuilder
    {
        private delegate void ParameterBuilder(Item _item, SimpleCommand cmd);

        private delegate Item ObjectBuilder(SimpleReader reader);

        private delegate bool UpdateChecker(Item _new, Item _old);

        private static readonly ParameterBuilder[] paramBuilders;

        private static readonly ObjectBuilder[] objBuilders;

        private static readonly UpdateChecker[] updateCheckers;

        private static readonly string[] charUpdateProc;

        private static readonly string[] charDeleteProc;

        private static readonly string[] charSelfUpdateProc;

        private static readonly string[] charCheckedUpdateProc;

        private static readonly string[] bankUpdateProc;

        private static readonly string[] bankSelfUpdateProc;

        private static readonly string[] bankDeleteProc;

        private static readonly string[] houseSelfUpdateProc;

        private static readonly string[] houseDeleteProc;

        private static readonly string[] houseItemSelectProc;

        private static readonly string[] equipmentCollectionUpdateProc;

        private static readonly string[] equipmentCollectionDeleteProc;

        private static readonly string[] equipmentCollectionSelectProc;

        private static readonly string[] charItemTables;
        private static readonly string[] bankItemTables;
        private static readonly string[] houseItemTables;
        private static readonly string[] itemIdTables;

        static ItemSqlBuilder()
        {
            itemIdTables = new string[] { null, Mabinogi.SQL.Tables.Mabinogi.ItemLargeId, Mabinogi.SQL.Tables.Mabinogi.ItemSmallId, Mabinogi.SQL.Tables.Mabinogi.ItemHugeId, Mabinogi.SQL.Tables.Mabinogi.ItemQuestId, Mabinogi.SQL.Tables.Mabinogi.ItemEgoId };
            charItemTables = new string[] { null, Mabinogi.SQL.Tables.Mabinogi.CharItemLarge, Mabinogi.SQL.Tables.Mabinogi.CharItemSmall, Mabinogi.SQL.Tables.Mabinogi.CharItemHuge, Mabinogi.SQL.Tables.Mabinogi.CharItemQuest, Mabinogi.SQL.Tables.Mabinogi.CharItemEgo };
            bankItemTables = new string[] { null, Mabinogi.SQL.Tables.Mabinogi.BankItemLarge, Mabinogi.SQL.Tables.Mabinogi.BankItemSmall, Mabinogi.SQL.Tables.Mabinogi.BankItemHuge, Mabinogi.SQL.Tables.Mabinogi.BankItemQuest, null };
            houseItemTables = new string[] { null, Mabinogi.SQL.Tables.Mabinogi.HouseItemLarge, Mabinogi.SQL.Tables.Mabinogi.HouseItemSmall, Mabinogi.SQL.Tables.Mabinogi.HouseItemHuge, Mabinogi.SQL.Tables.Mabinogi.HouseItemQuest, null };
            charUpdateProc = new string[6];
            charUpdateProc[0] = null;
            charUpdateProc[1] = "CharItemUpdate_Large";
            charUpdateProc[2] = "CharItemUpdate_Small";
            charUpdateProc[3] = "CharItemUpdate_Huge";
            charUpdateProc[4] = "CharItemUpdate_Quest";
            charUpdateProc[5] = "CharItemUpdate_Ego";
            charDeleteProc = new string[6];
            charDeleteProc[0] = null;
            charDeleteProc[1] = "CharItemDelete_Large";
            charDeleteProc[2] = "CharItemDelete_Small";
            charDeleteProc[3] = "CharItemDelete_Huge";
            charDeleteProc[4] = "CharItemDelete_Quest";
            charDeleteProc[5] = "CharItemDelete_Ego";
            charSelfUpdateProc = new string[6];
            charSelfUpdateProc[0] = null;
            charSelfUpdateProc[1] = "CharItemSelfUpdate_Large";
            charSelfUpdateProc[2] = "CharItemSelfUpdate_Small";
            charSelfUpdateProc[3] = "CharItemSelfUpdate_Huge";
            charSelfUpdateProc[4] = "CharItemSelfUpdate_Quest";
            charSelfUpdateProc[5] = "CharItemSelfUpdate_Ego";
            charCheckedUpdateProc = new string[6];
            charCheckedUpdateProc[0] = null;
            charCheckedUpdateProc[1] = "CharItemCheckedUpdate_Large";
            charCheckedUpdateProc[2] = "CharItemCheckedUpdate_Small";
            charCheckedUpdateProc[3] = "CharItemCheckedUpdate_Huge";
            charCheckedUpdateProc[4] = "CharItemCheckedUpdate_Quest";
            charCheckedUpdateProc[5] = "CharItemCheckedUpdate_Ego";
            bankUpdateProc = new string[6];
            bankUpdateProc[0] = null;
            bankUpdateProc[1] = "BankItemUpdate_Large";
            bankUpdateProc[2] = "BankItemUpdate_Small";
            bankUpdateProc[3] = "BankItemUpdate_Huge";
            bankUpdateProc[4] = "BankItemUpdate_Quest";
            bankUpdateProc[5] = null;
            bankSelfUpdateProc = new string[6];
            bankSelfUpdateProc[0] = null;
            bankSelfUpdateProc[1] = "BankItemSelfUpdate_Large";
            bankSelfUpdateProc[2] = "BankItemSelfUpdate_Small";
            bankSelfUpdateProc[3] = "BankItemSelfUpdate_Huge";
            bankSelfUpdateProc[4] = "BankItemSelfUpdate_Quest";
            bankSelfUpdateProc[5] = null;
            bankDeleteProc = new string[6];
            bankDeleteProc[0] = null;
            bankDeleteProc[1] = "BankItemDelete_Large";
            bankDeleteProc[2] = "BankItemDelete_Small";
            bankDeleteProc[3] = "BankItemDelete_Huge";
            bankDeleteProc[4] = "BankItemDelete_Quest";
            bankDeleteProc[5] = null;
            houseSelfUpdateProc = new string[6];
            houseSelfUpdateProc[0] = null;
            houseSelfUpdateProc[1] = "HouseItemSelfUpdate_Large";
            houseSelfUpdateProc[2] = "HouseItemSelfUpdate_Small";
            houseSelfUpdateProc[3] = "HouseItemSelfUpdate_Huge";
            houseSelfUpdateProc[4] = "HouseItemSelfUpdate_Quest";
            houseSelfUpdateProc[5] = null;
            houseDeleteProc = new string[6];
            houseDeleteProc[0] = null;
            houseDeleteProc[1] = "HouseItemDelete_Large";
            houseDeleteProc[2] = "HouseItemDelete_Small";
            houseDeleteProc[3] = "HouseItemDelete_Huge";
            houseDeleteProc[4] = "HouseItemDelete_Quest";
            houseDeleteProc[5] = null;
            houseItemSelectProc = new string[6];
            houseItemSelectProc[0] = null;
            houseItemSelectProc[1] = "SelectHouseItemLarge";
            houseItemSelectProc[2] = "SelectHouseItemSmall";
            houseItemSelectProc[3] = "SelectHouseItemHuge";
            houseItemSelectProc[4] = "SelectHouseItemQuest";
            houseItemSelectProc[5] = null;
            equipmentCollectionUpdateProc = new string[6];
            equipmentCollectionUpdateProc[0] = null;
            equipmentCollectionUpdateProc[1] = "EquipCollectItemUpdate_Large";
            equipmentCollectionUpdateProc[2] = null;
            equipmentCollectionUpdateProc[3] = null;
            equipmentCollectionUpdateProc[4] = null;
            equipmentCollectionUpdateProc[5] = null;
            equipmentCollectionSelectProc = new string[6];
            equipmentCollectionSelectProc[0] = null;
            equipmentCollectionSelectProc[1] = "SelectCharacterEquipCollectItemLarge";
            equipmentCollectionSelectProc[2] = null;
            equipmentCollectionSelectProc[3] = null;
            equipmentCollectionSelectProc[4] = null;
            equipmentCollectionSelectProc[5] = null;
            equipmentCollectionDeleteProc = new string[6];
            equipmentCollectionDeleteProc[0] = null;
            equipmentCollectionDeleteProc[1] = "EquipCollectItemDelete_Large";
            equipmentCollectionDeleteProc[2] = null;
            equipmentCollectionDeleteProc[3] = null;
            equipmentCollectionDeleteProc[4] = null;
            equipmentCollectionDeleteProc[5] = null;
            paramBuilders = new ParameterBuilder[6];
            paramBuilders[0] = null;
            paramBuilders[1] = ItemParameterBuilder.BuildLargeItem;
            paramBuilders[2] = ItemParameterBuilder.BuildSmallItem;
            paramBuilders[3] = ItemParameterBuilder.BuildHugeItem;
            paramBuilders[4] = ItemParameterBuilder.BuildQuestItem;
            paramBuilders[5] = ItemParameterBuilder.BuildEgoItem;
            objBuilders = new ObjectBuilder[6];
            objBuilders[0] = null;
            objBuilders[1] = ItemObjectBuilder.BuildLargeItem;
            objBuilders[2] = ItemObjectBuilder.BuildSmallItem;
            objBuilders[3] = ItemObjectBuilder.BuildHugeItem;
            objBuilders[4] = ItemObjectBuilder.BuildQuestItem;
            objBuilders[5] = ItemObjectBuilder.BuildEgoItem;
            updateCheckers = new UpdateChecker[6];
            updateCheckers[0] = null;
            updateCheckers[1] = ItemUpdateChecker.CheckLargeItem;
            updateCheckers[2] = ItemUpdateChecker.CheckSmallItem;
            updateCheckers[3] = ItemUpdateChecker.CheckHugeItem;
            updateCheckers[4] = ItemUpdateChecker.CheckQuestItem;
            updateCheckers[5] = ItemUpdateChecker.CheckEgoItem;
        }

        public static void UpdateItem(long charId, Item _new, Item _old, SimpleConnection conn, SimpleTransaction transaction)
        {
            if (updateCheckers[_new.storedtype](_new, _old))
            {
                using (var cmd = conn.GetDefaultUpdateCommand(charItemTables[_new.storedtype], transaction))
                {
                    cmd.Where(Mabinogi.SQL.Columns.CharItem.Id, charId);
                    cmd.Where(Mabinogi.SQL.Columns.Item.ItemLoc, 0);
                    cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, _new.id);

                    cmd.Set(Mabinogi.SQL.Columns.Item.PosX, _new.pos_x);
                    cmd.Set(Mabinogi.SQL.Columns.Item.PosY, _new.pos_y);
                    cmd.Set(Mabinogi.SQL.Columns.CharItem.VarInt, _new.varint);
                    cmd.Set(Mabinogi.SQL.Columns.Item.UpdateTime, DateTime.Now);
                    cmd.Set(Mabinogi.SQL.Columns.Item.PocketId, _new.pocket);

                    paramBuilders[_new.storedtype](_new, cmd);
                    cmd.Execute();
                }
            }
        }

        public static void SelfUpdateItem(long _gameID, Item _item, bool _bForceUpdate, SimpleConnection conn, SimpleTransaction transaction)
        {
            // The procedures for this are a nightmare.
            using (var upCmd = conn.GetDefaultUpdateCommand(charItemTables[_item.storedtype], transaction))
            {
                DateTime now = DateTime.Now;

                upCmd.Where(Mabinogi.SQL.Columns.Item.ItemId, _item.id);
                if (_bForceUpdate)
                {
                    upCmd.Set(Mabinogi.SQL.Columns.CharItem.Id, _gameID);
                }
                else
                {
                    upCmd.Where(Mabinogi.SQL.Columns.CharItem.Id, _gameID);
                }
                upCmd.Set(Mabinogi.SQL.Columns.Item.PosX, _item.pos_x);
                upCmd.Set(Mabinogi.SQL.Columns.Item.PosY, _item.pos_y);
                upCmd.Set(Mabinogi.SQL.Columns.CharItem.VarInt, _item.varint);
                upCmd.Set(Mabinogi.SQL.Columns.Item.UpdateTime, now);
                upCmd.Set(Mabinogi.SQL.Columns.Item.PocketId, _item.pocket);


                paramBuilders[_item.storedtype](_item, upCmd);

                int val = upCmd.Execute();
                if (val < 1)
                {
                    if (CheckItemLoc(_item, 1, conn))
                    {
                        string account;
                        using (var cmd = conn.GetDefaultSelectCommand(bankItemTables[_item.storedtype], transaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, _item.id);
                            cmd.Where(Mabinogi.SQL.Columns.Item.ItemLoc, 1);
                            cmd.Set(Mabinogi.SQL.Columns.BankItem.Account, 0);

                            using (var reader = cmd.ExecuteReader())
                            {
                                reader.Read();
                                account = reader.GetString(Mabinogi.SQL.Columns.BankItem.Account);
                            }
                        }

                        using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Bank, transaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.Bank.Account, account);
                            cmd.Set(Mabinogi.SQL.Columns.Bank.UpdateTime, now);
                            cmd.Execute();
                        }

                        using (var cmd = conn.GetDefaultDeleteCommand(bankItemTables[_item.storedtype], transaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, _item.id);
                            cmd.Where(Mabinogi.SQL.Columns.Item.ItemLoc, 1);
                            cmd.Execute();
                        }

                        using (var cmd = conn.GetDefaultUpdateCommand(itemIdTables[_item.storedtype], transaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.ItemId.Id, _item.id);
                            cmd.Set(Mabinogi.SQL.Columns.ItemId.ItemLoc, 0);
                            cmd.Execute();
                        }


                    }
                    else if (_item.storedtype == 1)
                    {
                        if (CheckItemLoc(_item, 4, conn))
                        {
                            string account;
                            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.EquipCollectItemLarge, transaction))
                            {
                                cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, _item.id);
                                cmd.Where(Mabinogi.SQL.Columns.EquipCollectItem.ItemLoc, 4);
                                cmd.Set(Mabinogi.SQL.Columns.EquipCollectItem.Account, 0);

                                using (var reader = cmd.ExecuteReader())
                                {
                                    reader.Read();
                                    account = reader.GetString(Mabinogi.SQL.Columns.EquipCollectItem.Account);
                                }
                            }

                            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.EquipCollect, transaction))
                            {
                                cmd.Set(Mabinogi.SQL.Columns.EquipCollect.UpdateTime, now);
                                cmd.Where(Mabinogi.SQL.Columns.EquipCollectItem.Account, account);
                                cmd.Execute();
                            }

                            using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.EquipCollectItemLarge, transaction))
                            {
                                cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, _item.id);
                                cmd.Where(Mabinogi.SQL.Columns.EquipCollectItem.ItemLoc, 4);
                                cmd.Execute();
                            }

                            using (var cmd = conn.GetDefaultUpdateCommand(itemIdTables[_item.storedtype], transaction))
                            {
                                cmd.Where(Mabinogi.SQL.Columns.ItemId.Id, _item.id);
                                cmd.Set(Mabinogi.SQL.Columns.ItemId.ItemLoc, 0);

                                cmd.Execute();
                            }
                        }
                        else
                        {

                            using (var cmd = conn.GetDefaultInsertCommand(itemIdTables[_item.storedtype], transaction))
                            {
                                cmd.Set(Mabinogi.SQL.Columns.ItemId.Id, _item.id);
                                cmd.Set(Mabinogi.SQL.Columns.ItemId.ItemLoc, 0);
                                cmd.Execute();
                            }
                        }
                    }
                    else
                    {

                        using (var cmd = conn.GetDefaultInsertCommand(itemIdTables[_item.storedtype], transaction))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.ItemId.Id, _item.id);
                            cmd.Set(Mabinogi.SQL.Columns.ItemId.ItemLoc, 0);
                            cmd.Execute();
                        }
                    }

                    using (var insCmd = conn.GetDefaultInsertCommand(charItemTables[_item.storedtype], transaction))
                    {
                        insCmd.Set(Mabinogi.SQL.Columns.CharItem.Id, _gameID);
                        insCmd.Set(Mabinogi.SQL.Columns.Item.PosX, _item.pos_x);
                        insCmd.Set(Mabinogi.SQL.Columns.Item.PosY, _item.pos_y);
                        insCmd.Set(Mabinogi.SQL.Columns.CharItem.VarInt, _item.varint);
                        insCmd.Set(Mabinogi.SQL.Columns.Item.UpdateTime, now);
                        insCmd.Set(Mabinogi.SQL.Columns.Item.PocketId, _item.pocket);
                        insCmd.Set(Mabinogi.SQL.Columns.Item.ItemId, _item.id);

                        paramBuilders[_item.storedtype](_item, insCmd);

                        insCmd.Execute();
                    }
                }
            }
        }

        public static void DeleteItem(long charID, long itemID, byte storedtype, SimpleConnection conn, SimpleTransaction transaction)
        {
            if (storedtype != 4)
            {
                var item = GetCharItem(storedtype, charID, conn);

                if (item == null)
                    return;

                if (storedtype == 5)
                    InsertEgoItemHistory(item, conn, transaction);
                else
                    InsertItemHistory(item, conn, transaction);
            }

            DeleteItemID(itemID, storedtype, 0, conn, transaction);
            DeleteCharItem(itemID, charID, storedtype, conn, transaction);
        }

        public static void BankUpdateItem(BankSlotInfo _newSlot, BankSlotInfo _oldSlot, BankItem _new, BankItem _old, SimpleConnection conn, SimpleTransaction transaction)
        {
            if (_newSlot.name != _oldSlot.name || _newSlot.race != _oldSlot.race || _new.location != _old.location || _new.extraTime != _old.extraTime || _new.time != _old.time || updateCheckers[_new.item.storedtype](_new.item, _old.item))
            {
                UpdateBankItem(null, _newSlot.name, (BankRace)_newSlot.race, _new, conn, transaction);
            }
        }

        public static void BankDeleteItem(string _account, BankItem _item, SimpleConnection conn = null, SimpleTransaction transaction = null)
        {
            using (var cmd = conn.GetDefaultDeleteCommand(bankItemTables[_item.item.storedtype], transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.BankItem.Account, _account);
                cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, _account);

                cmd.Execute();
            }
        }

        public static void BankSelfUpdateItem(string account, string slotName, BankRace race, BankItem item, SimpleConnection conn, SimpleTransaction transaction = null)
        {
            byte loc = GetBankItemLoc(item.item, conn);

            if (loc == 1)
            {
                if (UpdateBankItem(account, slotName, race, item, conn, transaction))
                    return;
            }

            if (CheckItemLoc(item.item, 0, conn))
            {
                long idCharacter = GetCharItemCharID(item.item, 0, conn);
                if (idCharacter != 0)
                {
                    UpdateCharacterTime(idCharacter, conn, transaction);
                    DeleteCharItem(item.item.id, idCharacter, item.item.storedtype, conn, transaction);

                    UpdateItemLoc(item.item, 1, conn, transaction);
                }
            }
            else
            {
                InsertItemID(item.item, 1, conn, transaction);
            }

            InsertBankItem(account, slotName, race, item, conn, transaction);
        }

        public static void HouseSelfUpdateItem(string _account, HouseItem _item, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultUpdateCommand(houseItemTables[_item.item.storedtype], transaction))
            {



                cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, _item.item.id);
                cmd.Where(Mabinogi.SQL.Columns.Item.ItemLoc, 2);

                cmd.Set(Mabinogi.SQL.Columns.Item.PosX, _item.posX);
                cmd.Set(Mabinogi.SQL.Columns.Item.PosY, _item.posY);
                cmd.Set(Mabinogi.SQL.Columns.HouseItem.Direction, _item.direction);
                cmd.Set(Mabinogi.SQL.Columns.HouseItem.UserPrice, _item.userprice);
                cmd.Set(Mabinogi.SQL.Columns.HouseItem.Pocket, _item.pocket);

                paramBuilders[_item.item.storedtype](_item.item, cmd);

                if (cmd.Execute() < 1)
                {
                    using (var insCmd = conn.GetDefaultInsertCommand(houseItemTables[_item.item.storedtype], transaction))
                    {
                        insCmd.Set(Mabinogi.SQL.Columns.Item.ItemId, _item.item.id);
                        insCmd.Set(Mabinogi.SQL.Columns.Item.ItemLoc, 2);

                        insCmd.Set(Mabinogi.SQL.Columns.Item.PosX, _item.posX);
                        insCmd.Set(Mabinogi.SQL.Columns.Item.PosY, _item.posY);
                        insCmd.Set(Mabinogi.SQL.Columns.HouseItem.Direction, _item.direction);
                        insCmd.Set(Mabinogi.SQL.Columns.HouseItem.UserPrice, _item.userprice);
                        insCmd.Set(Mabinogi.SQL.Columns.HouseItem.Pocket, _item.pocket);

                        paramBuilders[_item.item.storedtype](_item.item, insCmd);
                        insCmd.Execute();

                        using (var selCmd = conn.GetDefaultSelectCommand(itemIdTables[_item.item.storedtype], transaction))
                        {
                            selCmd.Where(Mabinogi.SQL.Columns.ItemId.Id, _item.item.id);
                            selCmd.Where(Mabinogi.SQL.Columns.ItemId.ItemLoc, 2);

                            bool chk;

                            using (var reader = selCmd.ExecuteReader())
                            {
                                chk = reader.HasRows;
                            }


                            if (chk == false)
                            {
                                using (var insIdCmd = conn.GetDefaultInsertCommand(itemIdTables[_item.item.storedtype], transaction))
                                {
                                    insIdCmd.Set(Mabinogi.SQL.Columns.ItemId.Id, _item.item.id);
                                    insIdCmd.Set(Mabinogi.SQL.Columns.ItemId.ItemLoc, 2);
                                    insIdCmd.Execute();
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void HouseDeleteItem(string _account, long _houseID, Item _item, int _money, SimpleConnection conn)
        {

            using (var transaction = conn.BeginTransaction())
            {

                using (var cmd = conn.GetDefaultDeleteCommand(houseItemTables[_item.storedtype], transaction))
                {
                    cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, _item.id);
                    cmd.Where(Mabinogi.SQL.Columns.Item.ItemLoc, 2);
                    cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, _account);

                    if (cmd.Execute() < 1)
                    {
                        transaction.Rollback();
                        return;
                    }
                }

                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.House, transaction))
                {
                    cmd.Where(Mabinogi.SQL.Columns.House.HouseId, _houseID);

                    cmd.Set(Mabinogi.SQL.Columns.House.HouseMoney, _money);

                    if (cmd.Execute() < 1)
                    {
                        transaction.Rollback();
                        return;
                    }
                }
                transaction.Commit();
            }
        }

        public static Item GetCharItem(byte storedType, long itemID, SimpleConnection conn)
        {
            var cmd = conn.GetDefaultSelectCommand(charItemTables[storedType]);
            cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, itemID);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                    return GetCharItem(storedType, reader);
            }
            return null;
        }

        public static Item GetItem(byte storedType, SimpleReader reader)
        {
            if (reader.IsRead == false && reader.Read() == false)
            {
                return null;
            }

            Item item = objBuilders[storedType](reader);
            return item;
        }

        public static Item GetCharItem(byte _storedType, SimpleReader reader)
        {
            if (reader.IsRead == false && reader.Read() == false)
            {
                return null;
            }

            Item item = objBuilders[_storedType](reader);
            item.pocket = (byte)reader.GetInt32(Mabinogi.SQL.Columns.Item.PocketId);
            item.varint = reader.GetInt32(Mabinogi.SQL.Columns.CharItem.VarInt);
            item.pos_x = reader.GetInt32(Mabinogi.SQL.Columns.Item.PosX);
            item.pos_y = reader.GetInt32(Mabinogi.SQL.Columns.Item.PosY);
            return item;
        }

        public static CollectionItem GetCollectionItem(Item.StoredType _storedType, SimpleReader reader)
        {
            CollectionItem collectionItem = new CollectionItem();
            collectionItem.item = GetCharItem((byte)_storedType, reader);
            collectionItem.lockTime = reader.GetInt64(Mabinogi.SQL.Columns.EquipCollectItem.LockTime);
            return collectionItem;
        }

        public static HouseItem GetHouseItem(string account, long itemID, byte storedType, SimpleConnection conn)
        {
            using (var reader = GetHouseItemReader(account, itemID, byte.MaxValue, storedType, conn))
            {
                return GetHouseItem(storedType, reader);
            }
        }

        public static HouseItem GetHouseItem(byte storedType, SimpleReader reader)
        {
            if (!reader.Read())
                return null;

            HouseItem houseItem = new HouseItem();
            houseItem.posX = reader.GetByte(Mabinogi.SQL.Columns.Item.PosX);
            houseItem.posY = reader.GetByte(Mabinogi.SQL.Columns.Item.PosY);
            houseItem.direction = reader.GetByte(Mabinogi.SQL.Columns.HouseItem.Direction);
            houseItem.userprice = reader.GetInt32(Mabinogi.SQL.Columns.HouseItem.UserPrice);
            houseItem.pocket = reader.GetByte(Mabinogi.SQL.Columns.HouseItem.Pocket);

            houseItem.item = ItemSqlBuilder.GetCharItem(storedType, reader);

            return houseItem;
        }

        private static SimpleReader GetHouseItemReader(string account, long itemID, byte loc, byte storedType, SimpleConnection conn)
        {
            using (var cmd = conn.GetDefaultSelectCommand(houseItemTables[storedType]))
            {
                cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, itemID);

                if (account != "" && account != string.Empty && account != null)
                    cmd.Where(Mabinogi.SQL.Columns.HouseItem.Account, account);

                if (loc != byte.MaxValue)
                    cmd.Where(Mabinogi.SQL.Columns.Item.ItemLoc, loc);

                return cmd.ExecuteReader();
            }
        }

        // TODO: Deal with this
        public static bool ForceUpdateRetry(SimpleSqlException _ex)
        {
            if (_ex.Number == 2627)
            {
                string[] array = charCheckedUpdateProc;
                foreach (string b in array)
                {
                    /*if (_ex.Procedure == b)
                    {
                        return true;
                    }*/
                }
                return false;
            }
            return false;
        }

        public static SimpleReader GetEquipmentCollectionSelectProc(string account, SimpleConnection conn)
        {
            var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.EquipCollectItemLarge);
            cmd.Where(Mabinogi.SQL.Columns.EquipCollectItem.Account, account);

            return cmd.ExecuteReader();
        }

        public static void EquipmentCollectionDeleteItem(string _accountId, CollectionItem _item, SimpleConnection conn, SimpleTransaction transaction)
        {
            if (1 == _item.item.storedtype)
            {
                using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.EquipCollectItemLarge, transaction))
                {
                    cmd.Where(Mabinogi.SQL.Columns.EquipCollectItem.Account, _accountId);
                    cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, _item.item.id);
                    cmd.Where(Mabinogi.SQL.Columns.Item.ItemLoc, 4);
                    cmd.Execute();
                }
                using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.ItemLargeId, transaction))
                {
                    cmd.Where(Mabinogi.SQL.Columns.ItemId.Id, _item.item.id);
                    cmd.Where(Mabinogi.SQL.Columns.ItemId.ItemLoc, 4);
                    cmd.Execute();
                }
            }
        }

        private static byte GetBankItemLoc(Item item, SimpleConnection conn)
        {
            using (var reader = GetBankItemReader(item.id, byte.MaxValue, item.storedtype, conn))
            {
                if (reader.Read())
                    return reader.GetByte(Mabinogi.SQL.Columns.Item.ItemId);

                return byte.MaxValue;
            }
        }

        private static SimpleReader GetBankItemReader(long itemID, byte loc, byte storedType, SimpleConnection conn = null)
        {
            using (var cmd = conn.GetDefaultSelectCommand(bankItemTables[storedType]))
            {
                cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, itemID);

                if (loc != byte.MaxValue)
                    cmd.Where(Mabinogi.SQL.Columns.Item.ItemLoc, loc);

                return cmd.ExecuteReader();
            }
        }

        private static bool CheckItemLoc(Item item, byte loc, SimpleConnection conn)
        {
            using (var cmd = conn.GetDefaultSelectCommand(itemIdTables[item.storedtype]))
            {
                cmd.Where(Mabinogi.SQL.Columns.ItemId.Id, item.id);
                cmd.Where(Mabinogi.SQL.Columns.ItemId.ItemLoc, loc);

                using (var reader = cmd.ExecuteReader())
                    return reader.HasRows;
            }
        }

        private static long GetCharItemCharID(Item item, byte loc, SimpleConnection conn)
        {
            using (var reader = GetCharItemReader(-1, item.id, loc, item.storedtype, conn))
            {
                if (reader != null)
                    return reader.GetInt64(Mabinogi.SQL.Columns.CharItem.Id);

                return 0;
            }
        }

        private static SimpleReader GetCharItemReader(long charID, long itemID, byte loc, byte storedType, SimpleConnection conn)
        {
            using (var cmd = conn.GetDefaultSelectCommand(charItemTables[storedType]))
            {
                cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, itemID);
                cmd.Where(Mabinogi.SQL.Columns.Item.ItemLoc, loc);

                if (charID != -1)
                    cmd.Where(Mabinogi.SQL.Columns.CharItem.Id, charID);

                return cmd.ExecuteReader();
            }
        }

        private static void DeleteCharItem(long itemID, long charID, byte storedtype, SimpleConnection conn, SimpleTransaction transaction)
        {
            SimpleCommand cmd = conn.GetDefaultDeleteCommand(charItemTables[storedtype], transaction);

            cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, itemID);
            cmd.Where(Mabinogi.SQL.Columns.Item.ItemLoc, 0);
            cmd.Where(Mabinogi.SQL.Columns.CharItem.Id, charID);
            cmd.Execute();
        }

        private static void UpdateItemLoc(Item item, byte loc, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultUpdateCommand(itemIdTables[item.storedtype], transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.ItemId.ItemLoc, loc);
                cmd.Where(Mabinogi.SQL.Columns.ItemId.Id, item.id);
                cmd.Execute();
            }
        }

        private static void InsertItemID(Item item, byte loc, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultInsertCommand(itemIdTables[item.storedtype], transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.ItemId.Id, item.id);
                cmd.Set(Mabinogi.SQL.Columns.ItemId.ItemLoc, loc);
                cmd.Execute();
            }
        }

        private static void InsertBankItem(string account, string slotName, BankRace race, BankItem item, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultInsertCommand(bankItemTables[item.item.storedtype], transaction))
            {

                cmd.Set(Mabinogi.SQL.Columns.BankItem.Account, account);
                cmd.Set(Mabinogi.SQL.Columns.BankItem.SlotName, slotName);
                cmd.Set(Mabinogi.SQL.Columns.BankItem.Race, (byte)race);
                cmd.Set(Mabinogi.SQL.Columns.BankItem.Location, item.location);
                cmd.Set(Mabinogi.SQL.Columns.BankItem.ExtraTime, item.extraTime);
                cmd.Set(Mabinogi.SQL.Columns.BankItem.Time, item.time);
                cmd.Set(Mabinogi.SQL.Columns.Item.PosX, item.item.pos_x);
                cmd.Set(Mabinogi.SQL.Columns.Item.PosY, item.item.pos_y);
                cmd.Set(Mabinogi.SQL.Columns.Item.ItemLoc, (byte)1);
                cmd.Set(Mabinogi.SQL.Columns.Item.ItemId, item.item.id);

                cmd.Set(Mabinogi.SQL.Columns.Item.UpdateTime, DateTime.Now);

                paramBuilders[item.item.storedtype](item.item, cmd);
                cmd.Execute();
            }
        }

        private static bool UpdateBankItem(string account, string slotName, BankRace race, BankItem item, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultUpdateCommand(bankItemTables[item.item.storedtype], transaction))
            {

                if (account != null && account != string.Empty)
                    cmd.Set(Mabinogi.SQL.Columns.BankItem.Account, account);

                cmd.Set(Mabinogi.SQL.Columns.Item.PosX, item.item.pos_x);
                cmd.Set(Mabinogi.SQL.Columns.Item.PosY, item.item.pos_y);


                cmd.Set(Mabinogi.SQL.Columns.BankItem.SlotName, slotName);
                cmd.Set(Mabinogi.SQL.Columns.BankItem.Race, (byte)race);
                cmd.Set(Mabinogi.SQL.Columns.BankItem.Location, item.location);
                cmd.Set(Mabinogi.SQL.Columns.BankItem.ExtraTime, item.extraTime);
                cmd.Set(Mabinogi.SQL.Columns.BankItem.Time, item.time);

                cmd.Set(Mabinogi.SQL.Columns.Item.UpdateTime, DateTime.Now);

                cmd.Where(Mabinogi.SQL.Columns.Item.ItemLoc, (byte)1);
                cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, item.item.id);

                paramBuilders[item.item.storedtype](item.item, cmd);
                return cmd.Execute() > 0;
            }
        }

        private static void InsertItemHistory(Item item, SimpleConnection conn, SimpleTransaction transaction)
        {
            SimpleCommand cmd;
            if (DateTime.Now.DayOfYear % 2 == 1)
            {
                cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.ItemHistory1, transaction);

            }
            else
            {
                cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.ItemHistory2, transaction);
            }

            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Id, item.id);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.PocketId, item.pocket);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Class, item.Class);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Color1, item.color_01);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Color2, item.color_02);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Color3, item.color_03);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Price, item.price);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Bundle, item.bundle);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.LinkedPocket, item.linked_pocket);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Figure, item.figure);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Flag, item.flag);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Durability, item.durability);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.DurabilityMax, item.durability_max);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.OriginalDurabilityMax, item.origin_durability_max);

            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.AttackMin, item.attack_min);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.AttackMax, item.attack_max);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.WAttackMin, item.wattack_min);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.WAttackMax, item.wattack_max);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Balance, item.balance);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Critical, item.critical);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Defence, item.defence);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Protect, item.protect);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.EffectiveRange, item.effective_range);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.AttackSpeed, item.attack_speed);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.DownHitCount, item.down_hit_count);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Experience, item.experience);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.ExpPoint, item.exp_point);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Upgraded, item.upgraded);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.UpgradeMax, item.upgrade_max);

            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Grade, item.grade);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Prefix, item.prefix);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Suffix, item.suffix);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Data, item.data);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Option, JsonSerializer.Serialize(item.options));
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.SellingPrice, item.sellingprice);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.DeleteTime, DateTime.Now);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.StoredType, item.storedtype);
            cmd.Set(Mabinogi.SQL.Columns.ItemHistory.Expiration, item.expiration);

            cmd.Execute();


        }

        public static void InsertEgoItemHistory(Item item, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var delCmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.ItemEgoHistory, transaction))
            {
                delCmd.Where(Mabinogi.SQL.Columns.ItemHistoryEgo.Id, item.id);
                delCmd.Execute();
            }


            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.ItemEgoHistory, transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Id, item.id);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Pocket, item.pocket);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Class, item.Class);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Color1, item.color_01);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Color2, item.color_02);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Color3, item.color_03);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Price, item.price);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Bundle, item.bundle);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.LinkedPocket, item.linked_pocket);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Figure, item.figure);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Flag, item.flag);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Durability, item.durability);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.DurabilityMax, item.durability_max);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.OriginalDurabilityMax, item.origin_durability_max);

                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.AttackMin, item.attack_min);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.AttackMax, item.attack_max);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.WAttackMin, item.wattack_min);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.WAttackMax, item.wattack_max);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Balance, item.balance);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Critical, item.critical);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Defence, item.defence);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Protect, item.protect);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EffectiveRange, item.effective_range);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.AttackSpeed, item.attack_speed);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.DownHitCount, item.down_hit_count);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Experience, item.experience);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.ExpPoint, item.exp_point);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Upgraded, item.upgraded);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.UpgradeMax, item.upgrade_max);

                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Grade, item.grade);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Prefix, item.prefix);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Suffix, item.suffix);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Data, item.data);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.SellingPrice, item.sellingprice);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Expiration, item.expiration);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoName, item.ego.egoName);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoType, item.ego.egoType);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoDesire, item.ego.egoDesire);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoSocialLevel, item.ego.egoSocialLevel);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoSocialExp, item.ego.egoSocialExp);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoStrengthLevel, item.ego.egoStrLevel);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoStrengthExp, item.ego.egoStrExp);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoIntelligenceLevel, item.ego.egoIntLevel);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoIntelligenceExp, item.ego.egoIntExp);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoDexterityLevel, item.ego.egoDexLevel);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoDexterityExp, item.ego.egoDexExp);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoWillLevel, item.ego.egoWillLevel);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoWillExp, item.ego.egoWillExp);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoLuckLevel, item.ego.egoLuckLevel);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoLuckExp, item.ego.egoLuckExp);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoSkillGauge, item.ego.egoSkillGauge);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoSkillCooldown, item.ego.egoSkillCoolTime);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.EgoSkillCount, item.ego.egoSkillCount);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.DeleteTime, DateTime.Now);
                cmd.Set(Mabinogi.SQL.Columns.ItemHistoryEgo.Option, JsonSerializer.Serialize(item.options));

                cmd.Execute();
            }
        }

        private static void DeleteItemID(long itemID, byte storedtype, byte loc, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultDeleteCommand(itemIdTables[storedtype], transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.ItemId.Id, itemID);
                cmd.Where(Mabinogi.SQL.Columns.ItemId.ItemLoc, loc);
                cmd.Execute();
            }
        }

        private static void UpdateCharacterTime(long id, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Character, transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.Character.UpdateTime, DateTime.Now);
                cmd.Where(Mabinogi.SQL.Columns.Character.Id, id);
                cmd.Execute();
            }
        }


    }
}
