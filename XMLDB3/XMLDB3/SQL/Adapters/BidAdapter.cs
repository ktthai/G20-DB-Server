using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class BidAdapter : SqlAdapter
	{
		protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Bid;

        public BidAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public BidList Read()
		{
			WorkSession.WriteStatus("BidSqlAdapter.Read() : 함수에 진입하였습니다.");

			try
			{
				using (var reader = Connection.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Bid).ExecuteReader())
					return Build(reader);
			}
			catch (SimpleSqlException ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
				WorkSession.WriteStatus(ex.Message, ex.Number);
			}
			catch (Exception ex2)
			{
				ExceptionMonitor.ExceptionRaised(ex2);
				WorkSession.WriteStatus(ex2.Message);
			}
			finally
			{
				WorkSession.WriteStatus("BidSqlAdapter.Read() : 데이터 베이스에 연결을 종료합니다.");
			}
			return null;
		}

		private BidList Build(SimpleReader reader)
		{
			if (reader == null)
			{
				throw new Exception("경매 테이블을 얻어오지 못햇습니다.");
			}
			if (reader.HasRows)
			{
				BidList bidList = new BidList();
				Bid bid;
				while(reader.Read())
				{
					bid = new Bid();
					bid.bidID = reader.GetInt64(Mabinogi.SQL.Columns.Bid.BidId);
					bid.charID = reader.GetInt64(Mabinogi.SQL.Columns.Bid.CharId);
					bid.charName = reader.GetString(Mabinogi.SQL.Columns.Bid.CharName);
					bid.auctionItemID = reader.GetInt32(Mabinogi.SQL.Columns.Bid.AuctionItemId);
					bid.price = reader.GetInt32(Mabinogi.SQL.Columns.Bid.Price);
					bid.time = reader.GetDateTime(Mabinogi.SQL.Columns.Bid.Time);
					bid.bidState = reader.GetByte(Mabinogi.SQL.Columns.Bid.BidState);
				}
				return bidList;
			}
			return new BidList();
		}

		public bool Add(Bid _bid)
		{
			WorkSession.WriteStatus("BidSqlAdapter.Add() : 함수에 진입하였습니다");
			
			try
			{
				WorkSession.WriteStatus("BidSqlAdapter.Add() : 데이터베이스와 연결합니다");

				using (var cmd = Connection.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Bid))
				{
					cmd.Set(Mabinogi.SQL.Columns.Bid.BidId, _bid.bidID);
					cmd.Set(Mabinogi.SQL.Columns.Bid.CharId, _bid.charID);
					cmd.Set(Mabinogi.SQL.Columns.Bid.CharName, _bid.charName);
					cmd.Set(Mabinogi.SQL.Columns.Bid.AuctionItemId, _bid.auctionItemID);
					cmd.Set(Mabinogi.SQL.Columns.Bid.Price, _bid.price);
					cmd.Set(Mabinogi.SQL.Columns.Bid.Time, _bid.time);
					cmd.Set(Mabinogi.SQL.Columns.Bid.BidState, _bid.bidState);

					WorkSession.WriteStatus("BidSqlAdapter.Add() : 명령을 실행합니다");
					cmd.Execute();
				}
				return true;
			}
			catch (SimpleSqlException ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
				WorkSession.WriteStatus(ex.Message, ex.Number);
				return false;
			}
			catch (Exception ex2)
			{
				ExceptionMonitor.ExceptionRaised(ex2);
				WorkSession.WriteStatus(ex2.Message);
				return false;
			}
			finally
			{
				WorkSession.WriteStatus("BidSqlAdapter.Add() : 연결을 종료합니다");
			}
		}

		public REPLY_RESULT Remove(long _bidID, ref byte _errorCode)
		{
			WorkSession.WriteStatus("BidSqlAdapter.Remove() : 함수에 진입하였습니다");
			try
			{
				WorkSession.WriteStatus("BidSqlAdapter.Remove() : 데이터베이스와 연결합니다");
				
				using (var cmd = Connection.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.Bid))
				{
					cmd.Where(Mabinogi.SQL.Columns.Bid.BidId, _bidID);

					WorkSession.WriteStatus("BidSqlAdapter.Remove() : 명령을 실행합니다");
					if(cmd.Execute() > 0)
						return REPLY_RESULT.SUCCESS;
					else
						return REPLY_RESULT.FAIL_EX;
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
				WorkSession.WriteStatus("BidSqlAdapter.Remove() : 연결을 종료합니다");
			}
		}

		public REPLY_RESULT Update(Bid _bid, ref byte _errorCode)
		{
			WorkSession.WriteStatus("BidSqlAdapter.Update() : 함수에 진입하였습니다");
			
			try
			{
				WorkSession.WriteStatus("BidSqlAdapter.Update() : 데이터베이스와 연결합니다");

                using (var cmd = Connection.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Bid))
                {
                    cmd.Where(Mabinogi.SQL.Columns.Bid.BidId, _bid.bidID);

                    cmd.Set(Mabinogi.SQL.Columns.Bid.CharId, _bid.charID);
                    cmd.Set(Mabinogi.SQL.Columns.Bid.CharName, _bid.charName);
                    cmd.Set(Mabinogi.SQL.Columns.Bid.AuctionItemId, _bid.auctionItemID);
                    cmd.Set(Mabinogi.SQL.Columns.Bid.Price, _bid.price);
                    cmd.Set(Mabinogi.SQL.Columns.Bid.Time, _bid.time);
                    cmd.Set(Mabinogi.SQL.Columns.Bid.BidState, _bid.bidState);

                    WorkSession.WriteStatus("BidSqlAdapter.Update() : 명령을 실행합니다");
					if (cmd.Execute() > 0)
                        return REPLY_RESULT.SUCCESS;
                    else
                        return REPLY_RESULT.FAIL_EX;
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
				WorkSession.WriteStatus("BidSqlAdapter.Update() : 연결을 종료합니다");
			}
		}
	}
}
