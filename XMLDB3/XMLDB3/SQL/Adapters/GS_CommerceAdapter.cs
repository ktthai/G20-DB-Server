using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class GS_CommerceAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Commerce;
        public GS_CommerceAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public REPLY_RESULT ReadAll(long _charId, out GS_Commerce _commerce)
        {
            WorkSession.WriteStatus("GS_CommerceSqlAdapter.ReadAll() : 함수에 진입하였습니다");

            _commerce = new GS_Commerce();
            _commerce.info = new GS_CommerceInfo();
            _commerce.products = new CommercePurchasedProducts();
            try
            {

                WorkSession.WriteStatus("GS_CommerceSqlAdapter.ReadAll() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var prodCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CommercePurchasedProduct))
                using (var infoCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Commerce))
                {
                    // PROCEDURE: dbo.QueryAllCommerceData
                    infoCmd.Where(Mabinogi.SQL.Columns.Commerce.CharId, _charId);
                    prodCmd.Where(Mabinogi.SQL.Columns.CommercePurchasedProduct.CharId, _charId);
                    bool exists = false;
                    WorkSession.WriteStatus("GS_CommerceSqlAdapter.ReadAll() : 데이타를 채웁니다.");
                    using (var productReader = prodCmd.ExecuteReader())
                    {
                        _commerce.products = BuildProduct(productReader);
                    }

                    using (var infoReader = infoCmd.ExecuteReader())
                    {
                        exists = infoReader.HasRows;
                        _commerce.info = BuildInfo(infoReader);
                    }

                    if (exists == false)
                    {
                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Commerce))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.Commerce.CharId, _charId);
                            cmd.Execute();
                        }
                    }

                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return REPLY_RESULT.FAIL;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("GS_CommerceSqlAdapter.ReadAll() : 연결을 종료합니다");
            }
            return REPLY_RESULT.SUCCESS;
        }

        public REPLY_RESULT QueryInfo(long _charId, out GS_CommerceInfo _commerceInfo)
        {
            WorkSession.WriteStatus("GS_CommerceSqlAdapter.QueryInfo() : 함수에 진입하였습니다");

            _commerceInfo = new GS_CommerceInfo();
            try
            {
                WorkSession.WriteStatus("GS_CommerceSqlAdapter.QueryInfo() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Commerce))
                {
                    // PROCEDURE: dbo.QueryCommerceInfo
                    cmd.Where(Mabinogi.SQL.Columns.Commerce.CharId, _charId);
                    WorkSession.WriteStatus("GS_CommerceSqlAdapter.QueryInfo() : 데이타를 채웁니다.");
                    bool exists = false;
                    using (var reader = cmd.ExecuteReader())
                    {
                        exists = reader.HasRows;
                        _commerceInfo = BuildInfo(reader);
                    }

                    if (exists == false)
                    {
                        using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Commerce))
                        {
                            insCmd.Set(Mabinogi.SQL.Columns.Commerce.CharId, _charId);
                            insCmd.Execute();
                        }
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return REPLY_RESULT.FAIL;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("GS_CommerceSqlAdapter.QueryInfo() : 연결을 종료합니다");
            }
            return REPLY_RESULT.SUCCESS;
        }

        public REPLY_RESULT QueryDucat(long _charId, out long _ducat)
        {
            WorkSession.WriteStatus("GS_CommerceSqlAdapter.QueryDucat() : 함수에 진입하였습니다");

            _ducat = 0L;
            try
            {
                WorkSession.WriteStatus("GS_CommerceSqlAdapter.QueryDucat() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Commerce))
                {
                    // PROCEDURE: dbo.QueryCommerceDucat
                    cmd.Where(Mabinogi.SQL.Columns.Commerce.CharId, _charId);

                    WorkSession.WriteStatus("GS_CommerceSqlAdapter.() : 데이타를 채웁니다.");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader == null)
                        {
                            throw new Exception("무역 테이블을 얻어오지 못했습니다.");
                        }
                        if (reader.Read() == false)
                            return REPLY_RESULT.FAIL;

                        _ducat = reader.GetInt64(Mabinogi.SQL.Columns.Commerce.Ducat);
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return REPLY_RESULT.FAIL;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("GS_CommerceSqlAdapter.() : 연결을 종료합니다");
            }
            return REPLY_RESULT.SUCCESS;
        }

        public REPLY_RESULT UpdateInfo(long _charId, long _ducat, long _unlockTransport, int _currentTransport, int _lostPercent, int _postID, int _credit)
        {
            WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateInfo() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateInfo() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Commerce))
                    {
                        transaction = conn.BeginTransaction();
                        if (1 > _credit)
                        {
                            _credit = 1;
                        }

                        WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateInfo() : 명령을 실행합니다");
                        CommerceDucatUpdateBuilder.Build(_charId, _ducat, conn, transaction);
                        CommerceUnlockTransportUpdateBuilder.Build(_charId, _unlockTransport, conn, transaction);
                        CommerceDucatUpdateBuilder.LogDucat(_charId, _ducat, conn, transaction);
                        cmd.Where(Mabinogi.SQL.Columns.Commerce.CharId, _charId);
                        cmd.Set(Mabinogi.SQL.Columns.Commerce.Ducat, _ducat);
                        cmd.Set(Mabinogi.SQL.Columns.Commerce.CurrentTransportId, _currentTransport);
                        cmd.Set(Mabinogi.SQL.Columns.Commerce.UnlockTransport, _unlockTransport);
                        cmd.Set(Mabinogi.SQL.Columns.Commerce.UpdateDate, DateTime.Now);
                        if (_postID > 0 && _postID <= 8)
                            cmd.Set(Mabinogi.SQL.Columns.Commerce.Post[_postID - 1], _credit);
                        CommercePostCreditUpdateBuilder.Build(_charId, _postID, _credit, conn, transaction);

                        cmd.Execute();

                        transaction.Commit();
                    }
                    return REPLY_RESULT.SUCCESS;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2);
                    WorkSession.WriteStatus(ex2.Message);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateInfo() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT UpdateProduct(long _charId, long _ducat, CommercePurchasedProducts _newProduct)
        {
            WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateProduct() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateProduct() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();

                    WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateProduct() : 명령을 실행합니다");
                    CommerceDucatUpdateBuilder.Build(_charId, _ducat, conn, transaction);
                    CommercePurchasedProductUpdateBuilder.Build(_charId, _newProduct, conn, transaction);

                    transaction.Commit();

                    return REPLY_RESULT.SUCCESS;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2);
                    WorkSession.WriteStatus(ex2.Message);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateProduct() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT UpdateSellProduct(long _charId, long _ducat, CommercePurchasedProducts _newProduct, int _postID, int _credit)
        {
            WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateSellProduct() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateProduct() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();

                    WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateSellProduct() : 명령을 실행합니다");
                    CommerceDucatUpdateBuilder.Build(_charId, _ducat, conn, transaction);
                    CommercePurchasedProductUpdateBuilder.Build(_charId, _newProduct, conn, transaction);
                    CommercePostCreditUpdateBuilder.Build(_charId, _postID, _credit, conn, transaction);

                    transaction.Commit();

                    return REPLY_RESULT.SUCCESS;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2);
                    WorkSession.WriteStatus(ex2.Message);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateSellProduct() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT UpdateTransport(long _charId, int _lostPercent, CommercePurchasedProducts _newProduct)
        {
            WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateTransport() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateTransport() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {


                    transaction = conn.BeginTransaction();

                    WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateTransport() : 명령을 실행합니다");
                    CommerceLostPercentUpdateBuilder.Build(_charId, _lostPercent, conn, transaction);
                    CommercePurchasedProductUpdateBuilder.Build(_charId, _newProduct, conn, transaction);

                    transaction.Commit();

                    return REPLY_RESULT.SUCCESS;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2);
                    WorkSession.WriteStatus(ex2.Message);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("GS_CommerceSqlAdapter.UpdateTransport() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT RemoveAllProduct(long _charId, long _ducat)
        {
            WorkSession.WriteStatus("GS_CommerceSqlAdapter.RemoveAllProduct() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GS_CommerceSqlAdapter.RemoveAllProduct() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();
                    CommerceDucatUpdateBuilder.Build(_charId, _ducat, conn, transaction);
                    // PROCEDURE: DeleteCommercePurchasedProductAll

                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.CommercePurchasedProduct, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.CommercePurchasedProduct.CharId, _charId);
                        WorkSession.WriteStatus("GS_CommerceSqlAdapter.RemoveAllProduct() : 명령을 실행합니다");
                        cmd.Execute();
                    }

                    transaction.Commit();


                    return REPLY_RESULT.SUCCESS;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2);
                    WorkSession.WriteStatus(ex2.Message);
                    transaction?.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                finally
                {
                    WorkSession.WriteStatus("GS_CommerceSqlAdapter.RemoveAllProduct() : 연결을 종료합니다");
                }
            }
        }

        private GS_CommerceInfo BuildInfo(SimpleReader reader)
        {
            if (reader == null)
            {
                throw new Exception("무역 테이블을 얻어오지 못햇습니다.");
            }
            if (reader.Read())
            {

                GS_CommerceInfo gS_CommerceInfo = new GS_CommerceInfo();
                gS_CommerceInfo.ducat = reader.GetInt64(Mabinogi.SQL.Columns.Commerce.Ducat);
                gS_CommerceInfo.currentTransport = reader.GetInt32(Mabinogi.SQL.Columns.Commerce.CurrentTransportId);
                gS_CommerceInfo.unlockTransport = reader.GetInt64(Mabinogi.SQL.Columns.Commerce.UnlockTransport);
                gS_CommerceInfo.lost_percent = reader.GetInt32(Mabinogi.SQL.Columns.Commerce.LostPercent);
                gS_CommerceInfo.credibilityTable = new Dictionary<int, CommerceCredibility>();
                for (int i = 1; i < 9; i++)
                {
                    CommerceCredibility commerceCredibility = new CommerceCredibility();
                    commerceCredibility.postId = i;
                    commerceCredibility.credibility = reader.GetInt32(Mabinogi.SQL.Columns.Commerce.Post[i - 1]);
                    gS_CommerceInfo.credibilityTable.Add(i, commerceCredibility);
                }
                return gS_CommerceInfo;
            }
            return new GS_CommerceInfo();
        }

        private CommercePurchasedProducts BuildProduct(SimpleReader reader)
        {
            if (reader == null)
            {
                throw new Exception("교역품 테이블을 얻어오지 못햇습니다.");
            }
            if (reader.HasRows)
            {
                CommercePurchasedProducts commercePurchasedProducts = new CommercePurchasedProducts();
                commercePurchasedProducts.productTable = new Dictionary<int, CommercePurchasedProduct>();
                CommercePurchasedProduct commercePurchasedProduct;
                while (reader.Read())
                {
                    commercePurchasedProduct = new CommercePurchasedProduct();
                    commercePurchasedProduct.id = reader.GetInt32(Mabinogi.SQL.Columns.CommercePurchasedProduct.ClassId);
                    commercePurchasedProduct.count = reader.GetInt32(Mabinogi.SQL.Columns.CommercePurchasedProduct.Bundle);
                    commercePurchasedProduct.price = reader.GetInt32(Mabinogi.SQL.Columns.CommercePurchasedProduct.Price);
                    if (commercePurchasedProducts.productTable.ContainsKey(commercePurchasedProduct.id))
                    {
                        commercePurchasedProducts.productTable.Remove(commercePurchasedProduct.id);
                    }
                    commercePurchasedProducts.productTable.Add(commercePurchasedProduct.id, commercePurchasedProduct);
                }
                return commercePurchasedProducts;
            }
            return new CommercePurchasedProducts();
        }
    }
}
