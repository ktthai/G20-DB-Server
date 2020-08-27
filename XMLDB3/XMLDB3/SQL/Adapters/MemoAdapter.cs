using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class MemoAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.MabiMemo.Memo;

        public MemoAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public bool SendMemo(Memo _memo)
        {
            WorkSession.WriteStatus("MemoSqlAdapter.SendMemo() : 함수에 진입하였습니다.");
            if (_memo.receipants == null)
            {
                WorkSession.WriteStatus("MemoSqlAdapter.SendMemo() : 받는 사람이 없어 바로 리턴합니다.");
                return true;
            }
            try
            {

                SimpleTransaction transaction = null;
                WorkSession.WriteStatus("SendMemo.SendMemo() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    try
                    {

                        foreach (MemoCharacter memoCharacter in _memo.receipants)
                        {
                            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiMemo.MemoBlacklist, transaction))
                            {
                                cmd.Where(Mabinogi.SQL.Columns.MemoBlacklist.ToId, _memo.sender.account);
                                cmd.Where(Mabinogi.SQL.Columns.MemoBlacklist.ToName, _memo.sender.name);
                                cmd.Where(Mabinogi.SQL.Columns.MemoBlacklist.FromId, memoCharacter.account);
                                cmd.Where(Mabinogi.SQL.Columns.MemoBlacklist.FromName, memoCharacter.name);

                                cmd.Set(Mabinogi.SQL.Columns.MemoBlacklist.Id, 0);

                                if (cmd.ExecuteReader().HasRows)
                                {
                                }
                                else
                                {
                                    using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiMemo.Memo, transaction))
                                    {
                                        cmd.Where(Mabinogi.SQL.Columns.Memo.FromName, _memo.sender.name);
                                        cmd.Where(Mabinogi.SQL.Columns.Memo.FromId, _memo.sender.account);
                                        cmd.Where(Mabinogi.SQL.Columns.Memo.ToName, memoCharacter.name);
                                        cmd.Where(Mabinogi.SQL.Columns.Memo.ToId, memoCharacter.account);
                                        cmd.Where(Mabinogi.SQL.Columns.Memo.Content, _memo.content);
                                        cmd.Where(Mabinogi.SQL.Columns.Memo.ToServer, _memo.sender.server);
                                        cmd.Where(Mabinogi.SQL.Columns.Memo.ToLevel, 3);
                                        cmd.Where(Mabinogi.SQL.Columns.Memo.FromServer, _memo.sender.server);
                                        cmd.Where(Mabinogi.SQL.Columns.Memo.FromDate, DateTime.Now);
                                        cmd.Where(Mabinogi.SQL.Columns.Memo.FromLevel, 0);

                                        WorkSession.WriteStatus("SendMemo.SendMemo() : 명령을 실행합니다");
                                        cmd.Execute();
                                    }
                                }
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (SimpleSqlException ex)
                    {
                        transaction?.Rollback();
                        ExceptionMonitor.ExceptionRaised(ex);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return false;
                    }
                    catch (Exception ex2)
                    {
                        transaction?.Rollback();
                        ExceptionMonitor.ExceptionRaised(ex2);
                        WorkSession.WriteStatus(ex2.Message);
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("HouseSqlAdapter.Read() : 연결을 종료합니다");
                    }
                }
            }
            catch (Exception ex3)
            {
                WorkSession.WriteStatus(ex3.Message);
                ExceptionMonitor.ExceptionRaised(ex3);
                return false;
            }
        }
    }
}
