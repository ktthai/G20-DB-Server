using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class SoulMateAdapter : SqlAdapter
    {
        private const string MainName = "MainCharName";
        private const string SubName = "SubCharName";

        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.SoulMate;

        public SoulMateAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        private const string Database = "mabinogi.";
        public SoulMateList ReadList()
        {
            WorkSession.WriteStatus("SoulMateSqlAdapter.ReadList() : 함수에 진입하였습니다.");

            try
            {
                // PROCEDURE: SelectSoulMate
                WorkSession.WriteStatus("SoulMateSqlAdapter.ReadList() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                using (var cmd = conn.GetSelectCommand(string.Format("SELECT {0}, (SELECT {1} FROM {2} c WHERE c.{3} = S.{0}) AS {4}, {5}, (SELECT {1} FROM {2} c WHERE a.{3} = s.{5}) as {6}, {7}, {8}, FROM {9} s",
                    /* 0 */ Mabinogi.SQL.Columns.SoulMate.MainCharId, /* 1 */ Mabinogi.SQL.Columns.Character.Name, /* 2 */ Mabinogi.SQL.Tables.Mabinogi.Character, /* 3 */ Mabinogi.SQL.Columns.Character.Id, /* 4 */ MainName, /* 5 */ Mabinogi.SQL.Columns.SoulMate.SubCharId, /* 6 */ SubName, /* 7 */ Mabinogi.SQL.Columns.SoulMate.MatePoint, /* 8 */ Mabinogi.SQL.Columns.SoulMate.StartTime, /* 9 */ Mabinogi.SQL.Tables.Mabinogi.SoulMate)))
                using (var reader = cmd.ExecuteReader())
                {
                    WorkSession.WriteStatus("SoulMateSqlAdapter.ReadList() : 데이터를 채웁니다.");

                    SoulMateList soulMateList = new SoulMateList();
                    if (reader.HasRows)
                    {
                        soulMateList.soulmate = new List<SoulMate>();
                        SoulMate soulMate;
                        while(reader.Read())
                        {
                            soulMate = new SoulMate();
                            soulMate.mainCharId = reader.GetInt64(Mabinogi.SQL.Columns.SoulMate.MainCharId);
                            soulMate.mainCharName = reader.GetString(MainName);
                            soulMate.subCharId = reader.GetInt64(Mabinogi.SQL.Columns.SoulMate.SubCharId);
                            soulMate.subCharName = reader.GetString(SubName);
                            soulMate.matePoint = (ushort)reader.GetInt16(Mabinogi.SQL.Columns.SoulMate.MatePoint);
                            soulMate.startTime = reader.GetDateTime(Mabinogi.SQL.Columns.SoulMate.StartTime);
                        }
                    }
                    return soulMateList;

                }
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
                WorkSession.WriteStatus("SoulMateSqlAdapter.ReadList() : 데이터 베이스에 연결을 종료합니다.");
            }
            return null;
        }

        public REPLY_RESULT RemoveSoulMate(long _mainCharId, ref byte _errorCode)
        {
            WorkSession.WriteStatus("SoulMateSqlAdapter.RemoveSoulMate() : 함수에 진입하였습니다");
            
            try
            {
                WorkSession.WriteStatus("SoulMateSqlAdapter.RemoveSoulMate() : 데이터베이스와 연결합니다");
                // PROCEDURE: DeleteSoulMate
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.SoulMate))
                {
                    cmd.Where(Mabinogi.SQL.Columns.SoulMate.MainCharId, _mainCharId);

                    WorkSession.WriteStatus("SoulMateSqlAdapter.RemoveSoulMate() : 명령을 실행합니다");
                    cmd.Execute();
                }

                return REPLY_RESULT.SUCCESS;
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
                WorkSession.WriteStatus("SoulMateSqlAdapter.RemoveSoulMate() : 연결을 종료합니다");
            }
        }

        public REPLY_RESULT UpdateSoulMate(SoulMate _soulmate, ref byte _errorCode)
        {
            WorkSession.WriteStatus("SoulMateSqlAdapter.UpdateSoulMate() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("SoulMateSqlAdapter.UpdateSoulMate() : 데이터베이스와 연결합니다");
                using(var conn = Connection)
                {
                    // PROCEDURE: UpdateSoulMate
                    using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.SoulMate))
                    {
                        DateTime time = DateTime.Now;

                        upCmd.Where(Mabinogi.SQL.Columns.SoulMate.MainCharId, _soulmate.mainCharId);

                        upCmd.Set(Mabinogi.SQL.Columns.SoulMate.SubCharId, _soulmate.subCharId);
                        upCmd.Set(Mabinogi.SQL.Columns.SoulMate.MatePoint, _soulmate.matePoint);
                        upCmd.Set(Mabinogi.SQL.Columns.SoulMate.StartTime, _soulmate.startTime);
                        upCmd.Set(Mabinogi.SQL.Columns.SoulMate.UpdateTime, DateTime.Now);

                        WorkSession.WriteStatus("SoulMateSqlAdapter.UpdateSoulMate() : 명령을 실행합니다");
                        if (upCmd.Execute() < 1)
                        {
                            using (var insCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.SoulMate))
                            {
                                insCmd.Set(Mabinogi.SQL.Columns.SoulMate.MainCharId, _soulmate.mainCharId);
                                insCmd.Set(Mabinogi.SQL.Columns.SoulMate.SubCharId, _soulmate.subCharId);
                                insCmd.Set(Mabinogi.SQL.Columns.SoulMate.MatePoint, _soulmate.matePoint);
                                insCmd.Set(Mabinogi.SQL.Columns.SoulMate.StartTime, time);
                                insCmd.Set(Mabinogi.SQL.Columns.SoulMate.UpdateTime, time);

                                if( insCmd.Execute() < 1)
                                    return REPLY_RESULT.FAIL;
                                return REPLY_RESULT.SUCCESS;
                            }
                        }
                        else
                        {
                            return REPLY_RESULT.SUCCESS;
                        }
                    }
                }
            }
            catch (SimpleSqlException ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _soulmate.mainCharId);
                WorkSession.WriteStatus(ex2.Message, ex2.Number);
                
                return REPLY_RESULT.FAIL;
            }
            catch (Exception ex3)
            {
                ExceptionMonitor.ExceptionRaised(ex3, _soulmate.mainCharId);
                WorkSession.WriteStatus(ex3.Message);

                return REPLY_RESULT.FAIL;
            }
            finally
            {
                WorkSession.WriteStatus("SoulMateSqlAdapter.UpdateSoulMate() : 연결을 종료합니다");
            }
        }
    }
}
