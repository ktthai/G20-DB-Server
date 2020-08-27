using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class CommerceSystemAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.CommerceProduct;

        public CommerceSystemAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public REPLY_RESULT Read(string _serverName, out CommerceSystem _commerce)
        {
            WorkSession.WriteStatus("CommerceSystemSqlAdapter.Read() : 함수에 진입하였습니다");

            _commerce = new CommerceSystem();
            _commerce.posts = new CommerceSystemPosts();
            _commerce.products = new CommerceSystemProducts();
            try
            {
                using (var conn = Connection)
                {
                    // PROCEDURE dbo.QueryAllCommerceServerData;
                    WorkSession.WriteStatus("CommerceSystemSqlAdapter.Read() : 데이터베이스와 연결합니다");

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CommercePost))
                    using (var reader = cmd.ExecuteReader())
                        _commerce.posts = BuildPost(reader);

                    WorkSession.WriteStatus("CommerceSystemSqlAdapter.Read() : 데이타를 채웁니다.");
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CommerceProduct))
                    using (var reader = cmd.ExecuteReader())
                    {
                        _commerce.products = BuildProduct(reader);
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.CommerceProductStock))
                    using (var reader = cmd.ExecuteReader())
                    {
                       BuildProductStock(reader, _commerce.products);
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
                WorkSession.WriteStatus("CommerceSystemSqlAdapter.Read() : 연결을 종료합니다");
            }
            return REPLY_RESULT.SUCCESS;
        }

        public REPLY_RESULT Update(string _serverName, CommerceSystem _commerce)
        {
            WorkSession.WriteStatus("CommerceSystemSqlAdapter.Update() : 함수에 진입하였습니다");

            
            using (var conn = Connection)
            {
                SimpleTransaction transaction = null;
                try
                {

                    transaction = conn.BeginTransaction();
                    foreach (CommerceSystemPost value in _commerce.posts.postTable.Values)
                    {
                        CommerceSystemPostUpdateBuilder.Build(value, conn, transaction);
                    }

                    foreach (CommerceSystemProduct value2 in _commerce.products.productTable.Values)
                    {
                        CommerceSystemProductUpdateBuilder.Build(value2, conn, transaction);
                        foreach (COStockInfo value3 in value2.stockTable.Values)
                        {
                            CommerceSystemProductStockUpdateBuilder.Build(value2.id, value3, conn, transaction);
                        }
                    }
                    WorkSession.WriteStatus("CommerceSystemSqlAdapter.Update() : 데이터베이스와 연결합니다");

                    WorkSession.WriteStatus("CommerceSystemSqlAdapter.Update() : 명령을 실행합니다");

                    transaction.Commit();


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
                    WorkSession.WriteStatus("CommerceSystemSqlAdapter.Update() : 연결을 종료합니다");
                }
            }
            return REPLY_RESULT.SUCCESS;
        }

        private CommerceSystemPosts BuildPost(SimpleReader postReader)
        {
            if (postReader == null)
            {
                throw new Exception("교역 시스템의 교역소 테이블을 얻어오지 못햇습니다.");
            }
            
            if (postReader.HasRows)
            {
                CommerceSystemPosts commerceSystemPosts = new CommerceSystemPosts();
                commerceSystemPosts.postTable = new Dictionary<int, CommerceSystemPost>();

                while (postReader.Read())
                {
                    CommerceSystemPost commerceSystemPost = new CommerceSystemPost();
                    commerceSystemPost.id = postReader.GetInt32(Mabinogi.SQL.Columns.CommercePost.PostId);
                    commerceSystemPost.investment = postReader.GetInt32(Mabinogi.SQL.Columns.CommercePost.PostInvestment);
                    commerceSystemPost.commission = postReader.GetInt32(Mabinogi.SQL.Columns.CommercePost.PostCommission);
                    commerceSystemPosts.postTable[commerceSystemPost.id] = commerceSystemPost;
                }
                return commerceSystemPosts;
            }

            if (ConfigManager.IsFirstRun)
                return CommerceSystemPosts.GetDefaultPosts();

            return new CommerceSystemPosts();
        }

        private CommerceSystemProducts BuildProduct(SimpleReader productReader)
        {
            if (productReader == null )
            {
                throw new Exception("교역 시스템의 교역품 테이블을 얻어오지 못햇습니다.");
            }
            CommerceSystemProducts commerceSystemProducts = new CommerceSystemProducts();
            commerceSystemProducts.productTable = new Dictionary<int, CommerceSystemProduct>();
            CommerceSystemProduct commerceSystemProduct;

            if (productReader.HasRows != true )
            {
                if (!ConfigManager.IsFirstRun)
                    commerceSystemProducts.productTable = CommerceSystemProducts.GetDefaultProducts();
                return commerceSystemProducts;
            }
            

            while (productReader.Read())
            {
                commerceSystemProduct = new CommerceSystemProduct();
                commerceSystemProduct.id = productReader.GetInt32(Mabinogi.SQL.Columns.CommerceProduct.ProductId);
                commerceSystemProduct.price = productReader.GetInt32(Mabinogi.SQL.Columns.CommerceProduct.ProductPrice);
                commerceSystemProduct.count = productReader.GetInt32(Mabinogi.SQL.Columns.CommerceProduct.ProductCount);

                commerceSystemProducts.productTable[commerceSystemProduct.id] = commerceSystemProduct;
            }

            return commerceSystemProducts;
        }

        private void BuildProductStock(SimpleReader productStockReader, CommerceSystemProducts commerceSystemProducts)
        {
            if ( productStockReader == null)
            {
                throw new Exception("교역 시스템의 교역품 테이블을 얻어오지 못햇습니다.");
            }

            if ( productStockReader.HasRows != true)
            {
                return;
            }

            COStockInfo cOStockInfo;
            while (productStockReader.Read())
            {
                int num = productStockReader.GetInt32(Mabinogi.SQL.Columns.CommerceProductStock.ProductId);
                cOStockInfo = new COStockInfo();
                cOStockInfo.idPost = productStockReader.GetInt32(Mabinogi.SQL.Columns.CommerceProductStock.ProductSellPostId);
                cOStockInfo.currentStock = productStockReader.GetInt32(Mabinogi.SQL.Columns.CommerceProductStock.ProductStock);
                cOStockInfo.reserveStock = 0;
                cOStockInfo.price = productStockReader.GetInt32(Mabinogi.SQL.Columns.CommerceProductStock.ProductStockPrice);

                if (!commerceSystemProducts.productTable.ContainsKey(num))
                {
                    throw new Exception("잘못된 재고 정보가 있습니다.");
                }
                CommerceSystemProduct commerceSystemProduct2 = commerceSystemProducts.productTable[num];
                if (commerceSystemProduct2.stockTable == null)
                {
                    commerceSystemProduct2.stockTable = new Dictionary<int, COStockInfo>();
                }
                commerceSystemProduct2.stockTable[cOStockInfo.idPost] = cOStockInfo;
            }
        }
    }
}
