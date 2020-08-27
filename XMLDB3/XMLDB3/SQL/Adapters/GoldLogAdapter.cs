using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class GoldLogAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.GoldLog;
        public GoldLogAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public bool InsertLog(GoldLog _goldlog)
        {
            WorkSession.WriteStatus("GoldLogSqlAdapter.InsertLog() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GoldLogSqlAdapter.InsertLog() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();
                    // PROCEDURE: usp_InsertGoldlog
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.GoldLog, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.Id, _goldlog.characterID);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.Quest, _goldlog.quest);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.Field, _goldlog.field);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.Commerce, _goldlog.commerce);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.Mail, _goldlog.mail);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.Bank, _goldlog.bank);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.ItemBuySell, _goldlog.itembuysell);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.ItemRepair, _goldlog.itemrepair);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.ItemUpgrade, _goldlog.itemupgrade);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.ItemSpecialUpgrade, _goldlog.itemspecialupgrade);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.Mint, _goldlog.mint);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.Guild, _goldlog.guild);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.PrivateShop, _goldlog.privateshop);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.Housing, _goldlog.housing);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.Etc, _goldlog.etc);
                        cmd.Set(Mabinogi.SQL.Columns.GoldLog.DynamicRegion, _goldlog.dynamicRegion);

                        WorkSession.WriteStatus("GoldLogSqlAdapter.InsertLog() : 명령을 실행합니다");
                        cmd.Execute();
                    }

                    WorkSession.WriteStatus("GoldLogSqlAdapter.InsertLog() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return true;

                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _goldlog.characterID);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GoldLogSqlAdapter.InsertLog() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return false;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _goldlog.characterID);
                    WorkSession.WriteStatus(ex2.Message);
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GoldLogSqlAdapter.InsertLog() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GoldLogSqlAdapter.InsertLog() : 연결을 종료합니다");
                }
            }
        }
    }
}
