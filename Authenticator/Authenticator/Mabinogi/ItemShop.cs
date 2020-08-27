using Mabinogi;
using System;
using System.Collections;
using Mabinogi.SQL;

using Tables = Mabinogi.SQL.Tables;
using Columns = Mabinogi.SQL.Columns;

namespace Authenticator
{
    public class ItemShop
    {
        private class GameItem
        {
            public string name;

            public string attribute;

            public short expire;

            public void ToMessage(Message _msg)
            {
                _msg.WriteString(name);
                _msg.WriteString(attribute);
                _msg.WriteS16(expire);
            }
        }

        private class ItemShopProduct
        {
            public enum EProductType
            {
                isProduct,
                isPackage,
                isLottery
            }

            public int orderNumber;

            public int productNumber;

            public EProductType ProductType;

            public GameItem product;

            public GameItem[] items;

            public short quantity;

            public string senderServer = string.Empty;

            public string senderName = string.Empty;

            public string senderMessage = string.Empty;

            public void ToMessage(Message _msg)
            {
                _msg.WriteS32(orderNumber);
                _msg.WriteS32(productNumber);
                _msg.WriteU8((byte)ProductType);
                product.ToMessage(_msg);
                _msg.WriteS16(quantity);
                _msg.WriteString(senderServer);
                _msg.WriteString(senderName);
                _msg.WriteString(senderMessage);
                if (ProductType != EProductType.isPackage && ProductType != EProductType.isLottery)
                {
                    return;
                }
                if (items == null || items.Length == 0)
                {
                    _msg.WriteS32(0);
                    return;
                }
                _msg.WriteS32(items.Length);
                GameItem[] array = items;
                foreach (GameItem gameItem in array)
                {
                    gameItem.ToMessage(_msg);
                }
            }
        }

        private const string LoadProcedure = "ssp_order_get_list";

        private const string UseProcedure = "ssp_product_content_receive";

        private const string RollbackProcedure = "ssp_product_content_receive_rollback";

        private const int MainPageIndex = 1;

        private const int MaxCountPerPage = 100;

        private int totalCount;

        private ArrayList products;

        private int domainNumber;

        public static ItemShop operator +(ItemShop _1, ItemShop _2)
        {
            _1.totalCount += _2.totalCount;
            if (_2.products != null && _2.products.Count > 0)
            {
                if (_1.products == null)
                {
                    _1.products = new ArrayList(_2.products);
                }
                else
                {
                    _1.products.AddRange(_2.products);
                }
            }
            return _1;
        }

        protected static SimpleConnection Connection
        {
            get
            {
                if (ServerConfiguration.IsLocalTestMode)
                {
                    return new SQLiteSimpleConnection(ServerConfiguration.ItemshopConnectionString);
                }
                else
                {
                    return new MySqlSimpleConnection(ServerConfiguration.ItemshopConnectionString);
                }
            }
        }

        public static bool Use(int _orderNumber, int _productNumber)
        {
            return false;

            //try
            //{
            //    using (var conn = Connection)
            //    {
            //        try
            //        {
            //            SqlCommand sqlCommand2 = new SqlCommand("ssp_product_content_receive", sqlConnection);
            //            sqlCommand2.CommandType = CommandType.StoredProcedure;
            //            SqlParameter sqlParameter = sqlCommand2.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            //            sqlParameter.Direction = ParameterDirection.ReturnValue;
            //            SqlParameter sqlParameter2 = sqlCommand2.Parameters.Add("@order_no", SqlDbType.Int);
            //            sqlParameter2.Value = _orderNumber;
            //            sqlParameter2 = sqlCommand2.Parameters.Add("@product_no", SqlDbType.Int);
            //            sqlParameter2.Value = _productNumber;
            //            sqlParameter2 = sqlCommand2.Parameters.Add("@receive_order_quantity", SqlDbType.SmallInt);
            //            sqlParameter2.Value = 1;
            //            sqlParameter2 = sqlCommand2.Parameters.Add("@process_message", SqlDbType.VarChar, 100);
            //            sqlParameter2.Value = "Request from gameserver";
            //            sqlCommand2.ExecuteNonQuery();
            //            if (sqlParameter != null && (int)sqlParameter.Value == 0)
            //            {
            //                return true;
            //            }
            //            ExceptionMonitor.ExceptionRaised(new Exception("아이템 사용을 마킹하는데 오류 발생"), _orderNumber, _productNumber);
            //            return false;
            //        }
            //        catch (SqlException ex)
            //        {
            //            ExceptionMonitor.ExceptionRaised(ex, ex.Number, _orderNumber, _productNumber);
            //            WorkSession.WriteStatus(ex.Message);
            //            return false;
            //        }
            //        catch (Exception ex2)
            //        {
            //            ExceptionMonitor.ExceptionRaised(ex2, _orderNumber, _productNumber);
            //            WorkSession.WriteStatus(ex2.Message);
            //            return false;
            //        }
            //        finally
            //        {
            //            sqlConnection.Close();
            //        }
            //    }
            //}
            //catch (Exception ex3)
            //{
            //    ExceptionMonitor.ExceptionRaised(ex3, _orderNumber, _productNumber);
            //    WorkSession.WriteStatus(ex3.Message);
            //    return false;
            //}
        }

        public static bool Rollback(int _orderNumber, int _productNumber)
        {

            return false;

            //try
            //{
            //    SqlConnection sqlConnection = new SqlConnection(ServerConfiguration.ItemshopConnectionString);
            //    try
            //    {
            //        sqlConnection.Open();
            //        SqlCommand sqlCommand = new SqlCommand("SET ARITHABORT ON", sqlConnection);
            //        sqlCommand.ExecuteNonQuery();
            //        SqlCommand sqlCommand2 = new SqlCommand("ssp_product_content_receive_rollback", sqlConnection);
            //        sqlCommand2.CommandType = CommandType.StoredProcedure;
            //        SqlParameter sqlParameter = sqlCommand2.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
            //        sqlParameter.Direction = ParameterDirection.ReturnValue;
            //        SqlParameter sqlParameter2 = sqlCommand2.Parameters.Add("@order_no", SqlDbType.Int);
            //        sqlParameter2.Value = _orderNumber;
            //        sqlParameter2 = sqlCommand2.Parameters.Add("@product_no", SqlDbType.Int);
            //        sqlParameter2.Value = _productNumber;
            //        sqlParameter2 = sqlCommand2.Parameters.Add("@process_message", SqlDbType.VarChar, 100);
            //        sqlParameter2.Value = "Request from gameserver";
            //        sqlCommand2.ExecuteNonQuery();
            //        if (sqlParameter != null && (int)sqlParameter.Value == 0)
            //        {
            //            return true;
            //        }
            //        ExceptionMonitor.ExceptionRaised(new Exception("아이템 사용 취소를 마킹하는데 오류 발생"), _orderNumber, _productNumber);
            //        return false;
            //    }
            //    catch (SqlException ex)
            //    {
            //        ExceptionMonitor.ExceptionRaised(ex, ex.Number, _orderNumber, _productNumber);
            //        WorkSession.WriteStatus(ex.Message);
            //        return false;
            //    }
            //    catch (Exception ex2)
            //    {
            //        ExceptionMonitor.ExceptionRaised(ex2, _orderNumber, _productNumber);
            //        WorkSession.WriteStatus(ex2.Message);
            //        return false;
            //    }
            //    finally
            //    {
            //        sqlConnection.Close();
            //    }
            //}
            //catch (Exception ex3)
            //{
            //    ExceptionMonitor.ExceptionRaised(ex3, _orderNumber, _productNumber);
            //    WorkSession.WriteStatus(ex3.Message);
            //    return false;
            //}
        }

        public static ItemShop Load(string _serverName, string _characterName)
        {
            return null;

            //int num = ServerConfiguration.GetDomainNumber(_serverName);
            //int gameNumber = ServerConfiguration.GameNumber;

            //try
            //{
            //    SqlConnection sqlConnection = new SqlConnection(ServerConfiguration.ItemshopConnectionString);
            //    try
            //    {
            //        SqlCommand sqlCommand = new SqlCommand("ssp_order_get_list", sqlConnection);
            //        sqlCommand.CommandType = CommandType.StoredProcedure;
            //        SqlParameter sqlParameter = sqlCommand.Parameters.Add("@game_no", SqlDbType.Int);
            //        sqlParameter.Value = gameNumber;
            //        sqlParameter = sqlCommand.Parameters.Add("@game_server", SqlDbType.Int);
            //        sqlParameter.Value = num;
            //        sqlParameter = sqlCommand.Parameters.Add("@game_id", SqlDbType.VarChar, 100);
            //        sqlParameter.Value = _characterName;
            //        SqlParameter sqlParameter2 = sqlCommand.Parameters.Add("@is_present", SqlDbType.Bit);
            //        sqlParameter2.Value = 0;
            //        sqlParameter = sqlCommand.Parameters.Add("@page_index", SqlDbType.Int);
            //        sqlParameter.Value = 1;
            //        sqlParameter = sqlCommand.Parameters.Add("@row_per_page", SqlDbType.Int);
            //        sqlParameter.Value = 100;
            //        SqlParameter sqlParameter3 = sqlCommand.Parameters.Add("@total_row_count", SqlDbType.Int);
            //        sqlParameter3.Direction = ParameterDirection.Output;
            //        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            //        sqlDataAdapter.TableMappings.Add("Table", "orderTable");
            //        sqlDataAdapter.TableMappings.Add("Table1", "packageTable");
            //        sqlDataAdapter.TableMappings.Add("Table2", "lotteryTable");
            //        DataSet dataSet = new DataSet();
            //        sqlDataAdapter.Fill(dataSet);
            //        sqlDataAdapter.Dispose();
            //        if (sqlParameter3.Value == null)
            //        {
            //            throw new Exception("총 아이템 갯수를 얻어오지 못했습니다.");
            //        }
            //        ItemShop itemShop = LoadFromReader(dataSet, (int)sqlParameter3.Value);
            //        sqlParameter2.Value = 1;
            //        sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            //        sqlDataAdapter.TableMappings.Add("Table", "orderTable");
            //        sqlDataAdapter.TableMappings.Add("Table1", "packageTable");
            //        sqlDataAdapter.TableMappings.Add("Table2", "lotteryTable");
            //        dataSet = new DataSet();
            //        sqlDataAdapter.Fill(dataSet);
            //        sqlDataAdapter.Dispose();
            //        if (sqlParameter3.Value == null)
            //        {
            //            throw new Exception("총 아이템 갯수를 얻어오지 못했습니다.");
            //        }
            //        itemShop += LoadFromReader(dataSet, (int)sqlParameter3.Value);
            //        itemShop.domainNumber = num;
            //        return itemShop;
            //    }
            //    catch (SqlException ex)
            //    {
            //        ExceptionMonitor.ExceptionRaised(ex, ex.Number, _serverName, _characterName);
            //        WorkSession.WriteStatus(ex.Message);
            //        return null;
            //    }
            //    catch (Exception ex2)
            //    {
            //        ExceptionMonitor.ExceptionRaised(ex2, _serverName, _characterName);
            //        WorkSession.WriteStatus(ex2.Message);
            //        return null;
            //    }
            //    finally
            //    {
            //        sqlConnection.Close();
            //    }
            //}
            //catch (Exception ex3)
            //{
            //    ExceptionMonitor.ExceptionRaised(ex3, _serverName, _characterName);
            //    WorkSession.WriteStatus(ex3.Message);
            //    return null;
            //}
        }

        //protected static void FillError(object sender, FillErrorEventArgs args)
        //{
        //    ExceptionMonitor.ExceptionRaised(args.Errors);
        //    if (args.Errors.GetType() == typeof(OverflowException))
        //    {
        //        args.Continue = false;
        //    }
        //}

        private static ulong GetPackageKey(int _orderNumber, int _packageNumber)
        {
            return (ulong)(((long)_orderNumber << 32) | (uint)_packageNumber);
        }

        private static ItemShop LoadFromReader(SimpleReader reader, int _totalCount)
        {
            return null;
            //if (reader.Tables["orderTable"] == null || reader.Tables["packageTable"] == null)
            //{
            //    throw new Exception("테이블을 얻어오지 못했습니다.");
            //}
            //ItemShop itemShop = new ItemShop();
            //Hashtable hashtable = new Hashtable();
            //Hashtable hashtable2 = new Hashtable();
            //itemShop.totalCount = _totalCount;
            //if (reader.Tables["packageTable"].Rows != null && reader.Tables["packageTable"].Rows.Count > 0)
            //{
            //    foreach (DataRow row in reader.Tables["packageTable"].Rows)
            //    {
            //        GameItem gameItem = new GameItem();
            //        int orderNumber = (int)row["order_no"];
            //        int packageNumber = (int)row["package_product_no"];
            //        gameItem.name = (string)row["product_name"];
            //        gameItem.attribute = BuildItemAttribute(row);
            //        if (!string.IsNullOrEmpty(gameItem.name) && !string.IsNullOrEmpty(gameItem.attribute))
            //        {
            //            ulong packageKey = GetPackageKey(orderNumber, packageNumber);
            //            ArrayList arrayList;
            //            if (!hashtable.ContainsKey(packageKey))
            //            {
            //                arrayList = new ArrayList();
            //                hashtable.Add(packageKey, arrayList);
            //            }
            //            else
            //            {
            //                arrayList = (ArrayList)hashtable[packageKey];
            //            }
            //            arrayList.Add(gameItem);
            //        }
            //    }
            //}
            //if (reader.Tables["lotteryTable"] != null && reader.Tables["lotteryTable"].Rows != null && reader.Tables["lotteryTable"].Rows.Count > 0)
            //{
            //    foreach (DataRow row2 in reader.Tables["lotteryTable"].Rows)
            //    {
            //        GameItem gameItem2 = new GameItem();
            //        int orderNumber2 = (int)row2["order_no"];
            //        int packageNumber2 = (int)row2["lottery_product_no"];
            //        gameItem2.name = (string)row2["product_name"];
            //        gameItem2.expire = (short)row2["product_expireDay"];
            //        gameItem2.attribute = BuildItemAttribute(row2);
            //        if (!string.IsNullOrEmpty(gameItem2.name) && !string.IsNullOrEmpty(gameItem2.attribute) && gameItem2.attribute.IndexOf("id:0") < 0)
            //        {
            //            ulong packageKey2 = GetPackageKey(orderNumber2, packageNumber2);
            //            ArrayList arrayList2;
            //            if (!hashtable2.ContainsKey(packageKey2))
            //            {
            //                arrayList2 = new ArrayList();
            //                hashtable2.Add(packageKey2, arrayList2);
            //            }
            //            else
            //            {
            //                arrayList2 = (ArrayList)hashtable2[packageKey2];
            //            }
            //            arrayList2.Add(gameItem2);
            //        }
            //    }
            //}
            ////if (reader.Tables["orderTable"].Rows != null && reader.Tables["orderTable"].Rows.Count > 0)
            //{
            //    itemShop.products = new ArrayList();
            //    {
            //        while (reader.Read())
            //        {
            //            ItemShopProduct itemShopProduct = new ItemShopProduct();
            //            itemShopProduct.orderNumber = (int)row3["order_no"];
            //            itemShopProduct.productNumber = (int)row3["product_no"];
            //            itemShopProduct.product = new GameItem();
            //            itemShopProduct.product.name = (string)row3["product_name"];
            //            itemShopProduct.product.expire = (short)row3["product_expireDay"];
            //            itemShopProduct.quantity = (short)row3["remain_order_quantity"];
            //            if (!row3.IsNull("sender_game_server"))
            //            {
            //                itemShopProduct.senderServer = ServerConfiguration.GetServerName((int)row3["sender_game_no"]);
            //            }
            //            if (!row3.IsNull("sender_game_id"))
            //            {
            //                itemShopProduct.senderName = (string)row3["sender_game_id"];
            //            }
            //            if (!row3.IsNull("present_message"))
            //            {
            //                itemShopProduct.senderMessage = (string)row3["present_message"];
            //            }
            //            if (!(bool)row3["is_order_package"])
            //            {
            //                itemShopProduct.ProductType = ItemShopProduct.EProductType.isProduct;
            //                itemShopProduct.product.attribute = BuildItemAttribute(row3);
            //                if (string.IsNullOrEmpty(itemShopProduct.product.attribute))
            //                {
            //                    continue;
            //                }
            //                itemShopProduct.product.expire = (short)row3["product_expireDay"];
            //            }
            //            else
            //            {
            //                itemShopProduct.ProductType = ItemShopProduct.EProductType.isPackage;
            //                itemShopProduct.product.attribute = string.Empty;
            //                itemShopProduct.product.expire = 0;
            //                if (row3.IsNull("package_product_no"))
            //                {
            //                    throw new Exception("패키지에 아이템이 없습니다.");
            //                }
            //                int packageNumber3 = (int)row3["package_product_no"];
            //                ArrayList arrayList3 = (ArrayList)hashtable[GetPackageKey(itemShopProduct.orderNumber, packageNumber3)];
            //                if (arrayList3 == null)
            //                {
            //                    throw new Exception("패키지에 아이템이 없습니다.");
            //                }
            //                itemShopProduct.items = (GameItem[])arrayList3.ToArray(typeof(GameItem));
            //            }
            //            itemShop.products.Add(itemShopProduct);
            //            if (reader.Tables["orderTable"].Mabinogi.SQL.Columns.Contains("product_type"))
            //            {
            //                string a = (string)row3["product_type"];
            //                if (itemShopProduct.ProductType == ItemShopProduct.EProductType.isProduct && a == "LT")
            //                {
            //                    itemShopProduct.ProductType = ItemShopProduct.EProductType.isLottery;
            //                    ulong packageKey3 = GetPackageKey(itemShopProduct.orderNumber, itemShopProduct.productNumber);
            //                    ArrayList arrayList4 = (ArrayList)hashtable2[packageKey3];
            //                    if (arrayList4 == null)
            //                    {
            //                        throw new Exception("복권에 딸린 아이템이 없습니다.");
            //                    }
            //                    itemShopProduct.items = (GameItem[])arrayList4.ToArray(typeof(GameItem));
            //                }
            //            }
            //        }
            //        return itemShop;
            //    }
            //}
            //return itemShop;
        }

        private static string BuildItemAttribute(SimpleReader reader)
        {
            string text = string.Empty;
            //string text2 = (string)reader["product_dbid"];
            //if (string.IsNullOrEmpty(text2))
            //{
            //    return "";
            //}
            //int num = 0;
            //try
            //{
            //    num = int.Parse(text2);
            //}
            //catch (Exception)
            //{
            //    return "";
            //}
            //if (num == 0)
            //{
            //    return "";
            //}
            //if (num != 0)
            //{
            //    text = text + "id:" + num + " ";
            //}
            //if (reader["product_pieces"] != null)
            //{
            //    text = text + "count:" + (short)reader["product_pieces"] + " ";
            //}
            //if (!reader.IsNull("product_attribute0"))
            //{
            //    text = text + (string)reader["product_attribute0"] + " ";
            //}
            //if (!reader.IsNull("product_attribute1"))
            //{
            //    text = text + (string)reader["product_attribute1"] + " ";
            //}
            //if (!reader.IsNull("product_attribute2"))
            //{
            //    text = text + (string)reader["product_attribute2"] + " ";
            //}
            //if (!reader.IsNull("product_attribute3"))
            //{
            //    text = text + (string)reader["product_attribute3"] + " ";
            //}
            //if (!reader.IsNull("product_attribute4"))
            //{
            //    text += (string)reader["product_attribute4"];
            //}
            return text;
        }

        public void ToMessage(Message _msg)
        {
            _msg.WriteS32(domainNumber);
            if (products == null || products.Count == 0)
            {
                _msg.WriteS32(0);
                _msg.WriteS32(0);
            }
            else
            {
                _msg.WriteS32(totalCount);
                _msg.WriteS32(products.Count);
                foreach (ItemShopProduct product in products)
                {
                    product.ToMessage(_msg);
                }
            }
        }
    }
}
