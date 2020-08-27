using System;
using Mabinogi.SQL;

namespace XMLDB3
{
	public class EventAdapter : SqlAdapter
	{
		private const byte NEWYEAR_NEXON_EVENT_TYPE = 1;

		protected override string ConfigRef => "";

        public EventAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public REPLY_RESULT Update(Event _event, ref byte _errorCode)
		{
			return REPLY_RESULT.FAIL;
            /*if (_event.eventType != 1)
            {
                return REPLY_RESULT.FAIL;
            }
            WorkSession.WriteStatus("EventSqlAdapter.Update() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            try
            {
                WorkSession.WriteStatus("EventSqlAdapter.Update() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    transaction = conn.BeginTransaction();

                    //Proc is "dbo.newYearNexonEvent", it is likely something like:
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.NewYearTable, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.NewYearEvent.Account, _event.account);
                        cmd.Set(Mabinogi.SQL.Columns.NewYearEvent.CharacterName, _event.charName);
                        cmd.Set(Mabinogi.SQL.Columns.NewYearEvent.Server, _event.serverName);
                        WorkSession.WriteStatus("EventSqlAdapter.Update() : 명령을 실행합니다");
                        cmd.Execute();
                    }

                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _event.account, _event.charName, _event.serverName);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                transaction?.Rollback();
                return REPLY_RESULT.FAIL;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _event.account, _event.charName, _event.serverName);
                WorkSession.WriteStatus(ex2.Message);
                transaction?.Rollback();
                return REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("EventSqlAdapter.Update() : 연결을 종료합니다");
            }*/
        }
    }
}
