using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class ShopAdvertiseAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.ShopAdvertise.Advertise;

        public ShopAdvertiseAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public ShopAdvertiseList Read(string _server, HouseAdapter _houseAdapter)
        {
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Read() : 함수에 진입하였습니다");
            try
            {
                if (_Read(out Dictionary<string, ShopAdvertiseDetail> _shopTable, out Dictionary<string, List<ShopAdvertiseItem>> _itemTable, _server))
                {
                    if (_itemTable != null && _itemTable.Count > 0)
                    {
                        List<ShopAdvertiseItem> itemList;
                        ShopAdvertiseDetail shopAdvertiseDetail;

                        foreach (string key in _itemTable.Keys)
                        {
                            shopAdvertiseDetail = _shopTable[key];
                            itemList = _itemTable[key];
                            if (itemList != null && itemList.Count > 0 && shopAdvertiseDetail != null)
                            {
                                List<ShopAdvertiseItemDetail> itemDetailList = new List<ShopAdvertiseItemDetail>();
                                foreach (ShopAdvertiseItem item in itemList)
                                {
                                    ShopAdvertiseItemDetail shopAdvertiseItemDetail = ReadHouseItem(key, item, _houseAdapter);
                                    if (shopAdvertiseItemDetail != null)
                                    {
                                        itemDetailList.Add(shopAdvertiseItemDetail);
                                    }
                                    else
                                    {
                                        ExceptionMonitor.ExceptionRaised(new Exception("Fail to read shop item"), item.id, key);
                                        if (Console.Out != null)
                                        {
                                            Console.WriteLine("Fail to read shop item [{0}][{1}]", key, item.id);
                                        }
                                    }
                                }
                                if (itemDetailList.Count > 0)
                                {
                                    shopAdvertiseDetail.items = itemDetailList;
                                }
                            }
                        }
                    }
                    ShopAdvertiseList shopAdvertiseList = new ShopAdvertiseList();
                    shopAdvertiseList.advertises = new ShopAdvertiseDetail[_shopTable.Values.Count];
                    _shopTable.Values.CopyTo(shopAdvertiseList.advertises, 0);
                    return shopAdvertiseList;
                }
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Read() : 광고 테이블을 읽지 못했습니다.");
                return null;
            }
            catch (Exception ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _server);
                WorkSession.WriteStatus(ex.Message, _server);
                return null;
            }
        }

        private bool _Read(out Dictionary<string, ShopAdvertiseDetail> _shopTable, out Dictionary<string, List<ShopAdvertiseItem>> _itemTable, string _server)
        {
            try
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter._Read() : 데이터베이스에 연결합니다");
                // PROCEDURE: SelectShopAdvertiseList
                using (var conn = Connection)
                {
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter._Read() : 데이터를 채웁니다.");
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.ShopAdvertise.Advertise))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.ShopAdvertise.Server, _server);
                        using (var advertiseReader = cmd.ExecuteReader())
                            _shopTable = BuildShop(advertiseReader);
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.ShopAdvertise.Item))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.ShopAdvertiseItem.Server, _server);
                        using (var itemReader = cmd.ExecuteReader())
                            _itemTable = BuildItem(itemReader);
                    }

                    return true;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _server);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                _shopTable = null;
                _itemTable = null;
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter._Read() : 데이터 베이스에 연결을 해제합니다");
            }
        }

        private ShopAdvertiseItemDetail ReadHouseItem(string _account, ShopAdvertiseItem _item, HouseAdapter _houseAdapter)
        {

            try
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.ReadHouseItem() : 데이터베이스에 연결합니다");

                HouseItem houseItem;
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.ReadHouseItem() : 데이터를 채웁니다");
                using (var conn = Connection)
                    houseItem = ItemSqlBuilder.GetHouseItem(_account, _item.id, (byte)_item.Type, conn);

                if (houseItem == null)
                    return null;

                ShopAdvertiseItemDetail shopAdvertiseItemDetail = new ShopAdvertiseItemDetail();
                shopAdvertiseItemDetail.item = houseItem.item;
                shopAdvertiseItemDetail.shopPrice = _item.price;
                return shopAdvertiseItemDetail;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.ReadHouseItem() : 데이터베이스에 연결을 해제합니다");
            }
        }



        private Dictionary<string, ShopAdvertiseDetail> BuildShop(SimpleReader reader)
        {
            if (reader == null)
            {
                throw new Exception("Shop Advertise reader is NULL");
            }
            Dictionary<string, ShopAdvertiseDetail> hashtable = new Dictionary<string, ShopAdvertiseDetail>();
            if (reader.HasRows)
            {
                ShopAdvertiseDetail shopAdvertiseDetail;
                while (reader.Read())
                {
                    shopAdvertiseDetail = new ShopAdvertiseDetail();
                    shopAdvertiseDetail.shopInfo = new ShopAdvertisebase();
                    shopAdvertiseDetail.shopInfo.account = reader.GetString(Mabinogi.SQL.Columns.ShopAdvertise.Account);
                    shopAdvertiseDetail.shopInfo.server = reader.GetString(Mabinogi.SQL.Columns.ShopAdvertise.Server);
                    shopAdvertiseDetail.shopInfo.shopName = reader.GetString(Mabinogi.SQL.Columns.ShopAdvertise.ShopName);
                    shopAdvertiseDetail.shopInfo.area = reader.GetString(Mabinogi.SQL.Columns.ShopAdvertise.Area);
                    shopAdvertiseDetail.shopInfo.characterName = reader.GetString(Mabinogi.SQL.Columns.ShopAdvertise.CharacterName);
                    shopAdvertiseDetail.shopInfo.comment = reader.GetString(Mabinogi.SQL.Columns.ShopAdvertise.Comment);
                    shopAdvertiseDetail.shopInfo.startTime = reader.GetInt64(Mabinogi.SQL.Columns.ShopAdvertise.StartTime);
                    shopAdvertiseDetail.shopInfo.region = reader.GetInt32(Mabinogi.SQL.Columns.ShopAdvertise.Region);
                    shopAdvertiseDetail.shopInfo.x = reader.GetInt32(Mabinogi.SQL.Columns.ShopAdvertise.X);
                    shopAdvertiseDetail.shopInfo.y = reader.GetInt32(Mabinogi.SQL.Columns.ShopAdvertise.Y);
                    shopAdvertiseDetail.shopInfo.leafletCount = reader.GetInt32(Mabinogi.SQL.Columns.ShopAdvertise.LeafletCount);
                    hashtable.Add(shopAdvertiseDetail.shopInfo.account, shopAdvertiseDetail);
                }
                return hashtable;
            }
            return hashtable;
        }

        private Dictionary<string, List<ShopAdvertiseItem>> BuildItem(SimpleReader reader)
        {
            if (reader == null)
            {
                throw new Exception("Shop Advertise Item reader is NULL");
            }
            Dictionary<string, List<ShopAdvertiseItem>> result = new Dictionary<string, List<ShopAdvertiseItem>>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string key = reader.GetString(Mabinogi.SQL.Columns.ShopAdvertiseItem.Account);

                    if (result[key] == null)
                    {
                        result[key] = new List<ShopAdvertiseItem>();
                    }
                    ShopAdvertiseItem shopAdvertiseItem = new ShopAdvertiseItem();
                    shopAdvertiseItem.id = reader.GetInt64(Mabinogi.SQL.Columns.ShopAdvertiseItem.ItemID);
                    shopAdvertiseItem.storedtype = reader.GetByte(Mabinogi.SQL.Columns.ShopAdvertiseItem.StoredType);
                    shopAdvertiseItem.price = reader.GetInt32(Mabinogi.SQL.Columns.ShopAdvertiseItem.Price);

                    result[key].Add(shopAdvertiseItem);
                }
                return result;
            }
            return result;
        }

        public bool Register(ShopAdvertise _advertise)
        {
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Register() : 함수에 진입하였습니다");
            try
            {

                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Register() : 데이터 베이스에 연결합니다");
                using (var conn = Connection)
                {
                    SimpleTransaction sqlTransaction = conn.BeginTransaction();
                    try
                    {
                        // PROCEDURE: CreateShopAdvertise
                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.ShopAdvertise.Advertise, sqlTransaction))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.Account, _advertise.shopInfo.account);
                            cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.Server, _advertise.shopInfo.server);
                            cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.ShopName, _advertise.shopInfo.shopName);
                            cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.Area, _advertise.shopInfo.area);
                            cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.CharacterName, _advertise.shopInfo.characterName);
                            cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.Comment, _advertise.shopInfo.comment);
                            cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.StartTime, _advertise.shopInfo.startTime);
                            cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.Region, _advertise.shopInfo.region);
                            cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.X, _advertise.shopInfo.x);
                            cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.Y, _advertise.shopInfo.y);
                            cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.LeafletCount, _advertise.shopInfo.leafletCount);

                            cmd.Execute();
                        }

                        if (_advertise.items != null && _advertise.items.Length > 0)
                        {
                            foreach (ShopAdvertiseItem shopAdvertiseItem in _advertise.items)
                            {
                                // PROCEDURE: CreateShopAdvertiseItem
                                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.ShopAdvertise.Item, sqlTransaction))
                                {
                                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Account, _advertise.shopInfo.account);
                                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Server, _advertise.shopInfo.server);
                                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.ItemID, shopAdvertiseItem.id);
                                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.StoredType, shopAdvertiseItem.storedtype);
                                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.ItemName, shopAdvertiseItem.itemName);
                                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Price, shopAdvertiseItem.price);
                                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Class, shopAdvertiseItem.@class);
                                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Color1, shopAdvertiseItem.color_01);
                                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Color2, shopAdvertiseItem.color_02);
                                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Color3, shopAdvertiseItem.color_03);
                                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Register() : 명령을 수행합니다.");
                                    cmd.Execute();
                                }
                            }
                        }

                        sqlTransaction.Commit();
                        return true;
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex, _advertise);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        sqlTransaction.Rollback();
                        return false;
                    }
                    catch (Exception ex2)
                    {
                        ExceptionMonitor.ExceptionRaised(ex2, _advertise);
                        WorkSession.WriteStatus(ex2.Message, _advertise);
                        sqlTransaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Register() : 데이터 베이스에 연결을 해제합니다");
                    }
                }
            }
            catch (Exception ex3)
            {
                ExceptionMonitor.ExceptionRaised(ex3, _advertise);
                WorkSession.WriteStatus(ex3.Message, _advertise);
                return false;
            }
        }

        public bool Unregister(string _account, string _server)
        {
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Unregister() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Unregister() : 데이터 베이스에 연결합니다");
                using (var conn = Connection)
                {
                    // PROCEDURE: DeleteShopAdvertise

                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.ShopAdvertise.Advertise))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.ShopAdvertise.Account, _account);
                        cmd.Where(Mabinogi.SQL.Columns.ShopAdvertise.Server, _server);
                        WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Unregister() : 명령을 수행합니다");
                        cmd.Execute();
                    }

                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.ShopAdvertise.Item))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.ShopAdvertiseItem.Account, _account);
                        cmd.Where(Mabinogi.SQL.Columns.ShopAdvertiseItem.Server, _server);
                        WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Unregister() : 명령을 수행합니다");
                        cmd.Execute();
                    }
                }
                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account, _server);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.Unregister() : 데이터 베이스에 연결을 해제합니다");
            }
        }

        public bool UpdateShopAdvertise(ShopAdvertisebase _advertise)
        {
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.UpdateShopAdvertise() : 함수에 진입하였습니다");
            try
            {
                // PROCEDURE: UpdateShopAdvertise
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.UpdateShopAdvertise() : 데이터 베이스에 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.ShopAdvertise.Advertise))
                {
                    cmd.Where(Mabinogi.SQL.Columns.ShopAdvertise.Account, _advertise.account);
                    cmd.Where(Mabinogi.SQL.Columns.ShopAdvertise.Server, _advertise.server);

                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.ShopName, _advertise.shopName);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.Area, _advertise.area);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.CharacterName, _advertise.characterName);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.Comment, _advertise.comment);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.StartTime, _advertise.startTime);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.Region, _advertise.region);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.X, _advertise.x);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.Y, _advertise.y);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertise.LeafletCount, _advertise.leafletCount);

                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.UpdateShopAdvertise() : 명령을 수행합니다.");
                    return cmd.Execute() > 0;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _advertise.account, _advertise.server);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _advertise.account, _advertise.server);
                WorkSession.WriteStatus(ex2.Message, _advertise.account);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.UpdateShopAdvertise() : 데이터 베이스에 연결을 해제합니다");
            }
        }

        public bool AddItem(string _account, string _server, ShopAdvertiseItem _item)
        {
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.AddItem() : 함수에 진입하였습니다");

            try
            {
                // PROCEDURE: CreateShopAdvertiseItem
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.ShopAdvertise.Item))
                {
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Account, _account);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Server, _server);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.ItemID, _item.id);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.StoredType, _item.storedtype);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.ItemName, _item.itemName);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Price, _item.price);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Class, _item.@class);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Color1, _item.color_01);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Color2, _item.color_02);
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Color3, _item.color_03);

                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.AddItem() : 명령을 수행합니다");
                    cmd.Execute();
                }

                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account, _server, _item.id);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.AddItem() : 데이터 베이스에 연결을 해제합니다");
            }
        }

        public bool DeleteItem(string _account, string _server, long _itemID)
        {
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.DeleteItem() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.DeleteItem() : 데이터 베이스에 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.ShopAdvertise.Item))
                {
                    // PROCEDURE: DeleteShopAdvertiseItem
                    cmd.Where(Mabinogi.SQL.Columns.ShopAdvertiseItem.Account, _account);
                    cmd.Where(Mabinogi.SQL.Columns.ShopAdvertiseItem.Server, _server);
                    cmd.Where(Mabinogi.SQL.Columns.ShopAdvertiseItem.ItemID, _itemID);

                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.DeleteItem() : 명령을 수행합니다");
                    cmd.Execute();

                    return true;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account, _server, _itemID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.AddItem() : 데이터 베이스에 연결을 해제합니다");
            }
        }

        public bool SetItemPrice(string _account, string _server, long _itemID, int _shopPrice)
        {
            WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.SetItemPrice() : 함수에 진입하였습니다");

            try
            {
                string cmdText = "exec dbo.  @strAccount=" + UpdateUtility.BuildString(_account) + ",@server=" + UpdateUtility.BuildString(_server) + ",@itemID=" + _itemID + ",@price=" + _shopPrice + "\n";
                // PROCEDURE: UpdateShopAdvertiseItemPrice
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.SetItemPrice() : 데이터 베이스에 연결합니다");
                using(var conn = Connection)
                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.ShopAdvertise.Advertise))
                {
                    cmd.Set(Mabinogi.SQL.Columns.ShopAdvertiseItem.Price, _shopPrice);
                    
                    cmd.Where(Mabinogi.SQL.Columns.ShopAdvertiseItem.ItemID, _itemID);
                    cmd.Where(Mabinogi.SQL.Columns.ShopAdvertiseItem.Server, _server);
                    cmd.Where(Mabinogi.SQL.Columns.ShopAdvertiseItem.Account, _account);
                    WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.SetItemPrice() : 명령을 수행합니다");
                    cmd.Execute();
                }

                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account, _server, _itemID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("ShopAdvertiseSqlAdapter.SetItemPrice() : 데이터 베이스에 연결을 해제합니다");
            }
        }
    }
}
