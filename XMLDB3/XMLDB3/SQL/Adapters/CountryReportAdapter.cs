using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class CountryReportAdapter : SqlAdapter
	{
		protected override string ConfigRef => "";

        public CountryReportAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public bool ReportCCU(CountryReport _report)
		{
			// TODO: no example of this, does not seem important
			return false;

            /*if (_report == null)
            {
                return false;
            }
            WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 함수에 진입하였습니다");

            WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 함수에 진입하였습니다");

            SimpleTransaction sqlTransaction = null;
            try
            {
                using (var conn = Connection)
                {
                    sqlTransaction = conn.BeginTransaction();
                    SqlCommand sqlCommand = new SqlCommand("uspInsertMabiRecord", sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add("@userCountData", SqlDbType.VarChar, 6000).Value = _report.reportstring;
                    sqlCommand.Transaction = sqlTransaction;
                    WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 명령을 실행합니다");
                    int num = sqlCommand.ExecuteNonQuery();
                    WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 트랜잭션을 커밋합니다");
                    sqlTransaction.Commit();
                    if (num > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (SimpleSqlException ex)
            {
                if (sqlTransaction != null)
                {
                    WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 트랜잭션을 롤백합니다");
                    sqlTransaction.Rollback("COUNTRY_CCU_REPORT");
                }
                ExceptionMonitor.ExceptionRaised(ex, _report);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                if (sqlTransaction != null)
                {
                    WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 트랜잭션을 롤백합니다");
                    sqlTransaction.Rollback("COUNTRY_CCU_REPORT");
                }
                ExceptionMonitor.ExceptionRaised(ex2, _report);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("CountryReportSqlAdapter.Do() : 연결을 종료합니다");
                //sqlConnection.Close();
            }*/
        }
    }
}
