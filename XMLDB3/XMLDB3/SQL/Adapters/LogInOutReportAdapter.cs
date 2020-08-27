using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class LogInOutReportAdapter : SqlAdapter
    {
        protected override string ConfigRef => "";
        public LogInOutReportAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public bool ReportLogInOut(LogInOutReport _report)
        {
            return false;

            // TODO: Work out if this is worthwhile or not.
            if (_report == null)
            {
                return false;
            }
            WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 함수에 진입하였습니다");

            WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            //using (var conn = Connection)
            {
                try
                {

                    //	transaction = conn.BeginTransaction();
                    //	// PROCEDURE: uspInsertMabiIPlog

                    //	using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.SomeLoginTableIGuess, transaction))
                    //	{
                    //		upCmd.Where(Mabinogi.SQL.Columns.SomeLoginTableIGuess.Account, _report.account);

                    //		upCmd.Set(Mabinogi.SQL.Columns.SomeLoginTableIGuess.Ip, _report.ip);
                    //		upCmd.Set(Mabinogi.SQL.Columns.SomeLoginTableIGuess.CountryCode, _report.countrycode);
                    //		upCmd.Set(Mabinogi.SQL.Columns.SomeLoginTableIGuess.InOut, _report.inout);

                    //		WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 명령을 실행합니다");
                    //		if (upCmd.Execute() < 1)
                    //		{
                    //			using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.SomeLoginTableIGuess, transaction))
                    //			{
                    //				insCmd.Set(Mabinogi.SQL.Columns.SomeLoginTableIGuess.Account, _report.account);
                    //				insCmd.Set(Mabinogi.SQL.Columns.SomeLoginTableIGuess.Ip, _report.ip);
                    //				insCmd.Set(Mabinogi.SQL.Columns.SomeLoginTableIGuess.CountryCode, _report.countrycode);
                    //				insCmd.Set(Mabinogi.SQL.Columns.SomeLoginTableIGuess.InOut, _report.inout);

                    //				if (insCmd.Execute() < 1)
                    //				{
                    //					transaction.Rollback();
                    //					return false;
                    //				}
                    //			}
                    //		}
                    //	}
                    //	//sqlCommand.Parameters.Add("@loginID", SqlDbType.VarChar, 50).Value = _report.account;
                    //	//sqlCommand.Parameters.Add("@IP", SqlDbType.VarChar, 50).Value = _report.ip;
                    //	//sqlCommand.Parameters.Add("@CountryCode", SqlDbType.VarChar, 5).Value = _report.countrycode;
                    //	//sqlCommand.Parameters.Add("@logType", SqlDbType.VarChar, 2).Value = _report.inout;



                    //	WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 트랜잭션을 커밋합니다");
                    //	transaction.Commit();

                    //		return true;	
                    //}
                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _report);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _report);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("LogInOutReportSqlAdapter.Do() : 연결을 종료합니다");
                }
            }
        }
    }
}
