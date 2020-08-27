using System;
using System.Collections.Generic;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class ChronicleAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.MabiChronicle.Chronicle;

        public ChronicleAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public bool Create(string _characterName, Chronicle chronicle)
        {
            WorkSession.WriteStatus("ChronicleSqlAdapter.Create() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            SimpleCommand cmd = null;
            using (var conn = Connection)
            {
                try
                {
                    ChronicleCache chronicleCache = ObjectCache.Chronicle;
                    bool flag = false;
                    int num = 0;
                    if (chronicleCache.Exists(chronicle.questID))
                    {
                        flag = true;
                        num = GetChronicleCount(chronicle);
                    }

                    WorkSession.WriteStatus("ChronicleSqlAdapter.Create() : 명령을 생성합니다.");
                    int questRank = 0;
                    DateTime _createTime;
                    if (flag && num == 0)
                    {
                        questRank = chronicleCache.GetNextCount(chronicle.serverName, chronicle.questID, out _createTime);
                    }
                    else
                    {
                        _createTime = chronicleCache.GetNextTime();
                    }

                    transaction = conn.BeginTransaction();

                    cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiChronicle.Chronicle, transaction);
                    cmd.Set(Mabinogi.SQL.Columns.Chronicle.ServerName, chronicle.serverName);
                    cmd.Set(Mabinogi.SQL.Columns.Chronicle.CharId, chronicle.charID);
                    cmd.Set(Mabinogi.SQL.Columns.Chronicle.QuestId, chronicle.questID);
                    cmd.Set(Mabinogi.SQL.Columns.Chronicle.CreateTime, _createTime);
                    cmd.Set(Mabinogi.SQL.Columns.Chronicle.Meta, chronicle.meta);
                    cmd.Execute();

                    if (flag)
                    {
                        if (num > 0)
                        {
                            cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleEventRank);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleEventRank.EventCount, "");
                            cmd.Where(Mabinogi.SQL.Columns.ChronicleEventRank.CharId, chronicle.charID);
                            cmd.Where(Mabinogi.SQL.Columns.ChronicleEventRank.ServerName, chronicle.serverName);
                            cmd.Where(Mabinogi.SQL.Columns.ChronicleEventRank.QuestId, chronicle.questID);

                            int eventCnt;
                            using (var reader = cmd.ExecuteReader())
                            {
                                reader.Read();
                                eventCnt = reader.GetInt32(Mabinogi.SQL.Columns.ChronicleEventRank.EventCount);
                            }



                            cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleEventRank, transaction);

                            cmd.Set(Mabinogi.SQL.Columns.ChronicleEventRank.EventCount, eventCnt + 1);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleEventRank.UpdateTime, _createTime);

                            cmd.Where(Mabinogi.SQL.Columns.ChronicleEventRank.CharId, chronicle.charID);
                            cmd.Where(Mabinogi.SQL.Columns.ChronicleEventRank.ServerName, chronicle.serverName);
                            cmd.Where(Mabinogi.SQL.Columns.ChronicleEventRank.QuestId, chronicle.questID);
                            cmd.Execute();
                        }
                        else
                        {
                            cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleEventRank, transaction);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleEventRank.ServerName, chronicle.serverName);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleEventRank.CharId, chronicle.charID);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleEventRank.CharName, _characterName);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleEventRank.QuestId, chronicle.questID);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleEventRank.Rank, questRank);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleEventRank.EventCount, 1);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleEventRank.CountRank, 0);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleEventRank.UpdateTime, _createTime);
                            cmd.Execute();

                            cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleLatestRank, transaction);
                            cmd.ComplexWhere(string.Format("{0} = ( SELECT {0} FROM {1} WHERE {2} = @{2} AND {3} = @{3} ORDER BY {4} ASC)", Mabinogi.SQL.Columns.ChronicleLatestRank.Id, Mabinogi.SQL.Tables.MabiChronicle.ChronicleLatestRank, Mabinogi.SQL.Columns.ChronicleLatestRank.ServerName, Mabinogi.SQL.Columns.ChronicleLatestRank.QuestId, Mabinogi.SQL.Columns.ChronicleLatestRank.RankTime));
                            cmd.AddParameter(Mabinogi.SQL.Columns.ChronicleLatestRank.ServerName, chronicle.serverName);
                            cmd.AddParameter(Mabinogi.SQL.Columns.ChronicleLatestRank.QuestId, chronicle.questID);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleLatestRank.CharId, chronicle.charID);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleLatestRank.CharName, _characterName);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleLatestRank.RankTime, _createTime);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleLatestRank.Rank, questRank);
                            cmd.Execute();

                            if (questRank <= ConfigManager.MaxChronicleFirst)
                            {
                                cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleFirstRank, transaction);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleFirstRank.ServerName, chronicle.serverName);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleFirstRank.CharId, chronicle.charID);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleFirstRank.CharName, _characterName);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleFirstRank.QuestId, chronicle.questID);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleFirstRank.RankTime, _createTime);
                                cmd.Execute();
                            }
                        }
                    }
                    WorkSession.WriteStatus("ChronicleSqlAdapter.Create() : 데이터 베이스에 연결합니다.");

                    WorkSession.WriteStatus("ChronicleSqlAdapter.Write() : 명령을 실행합니다");
                    transaction.Commit();
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, chronicle.serverName, chronicle.charID, chronicle.questID);
                    WorkSession.WriteStatus(ex.Message);
                    transaction?.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, chronicle.serverName, chronicle.charID, chronicle.questID);
                    WorkSession.WriteStatus(ex2.Message);
                    transaction?.Rollback();
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("ChronicleSqlAdapter.Create() : 연결을 종료합니다");
                }
            }
        }

        private int GetChronicleCount(Chronicle _chronicle)
        {
            WorkSession.WriteStatus("ChronicleSqlAdapter.IsChronicleExist() : 탐사연표 존재를 검사합니다.");
            try
            {
                WorkSession.WriteStatus("ChronicleSqlAdapter.IsChronicleExist() : DB에 연결합니다.");
                using (var conn = this.Connection)
                {

                    var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleEventRank);
                    cmd.Where(Mabinogi.SQL.Columns.ChronicleEventRank.CharId, _chronicle.charID);
                    cmd.Where(Mabinogi.SQL.Columns.ChronicleEventRank.ServerName, _chronicle.serverName);
                    cmd.Where(Mabinogi.SQL.Columns.ChronicleEventRank.QuestId, _chronicle.questID);
                    return cmd.ExecuteReader().GetInt32(Mabinogi.SQL.Columns.ChronicleEventRank.EventCount);
                }
            }
            finally
            {
            }
        }

        private void UpdateChronicleCache(Dictionary<int, int> dictionary, ChronicleInfoList _list, SimpleConnection conn, SimpleTransaction transaction)
        {

            ObjectCache.InitChronicleCache(_list.serverName, dictionary);

            SimpleCommand cmd;
            DateTime past = DateTime.Now.AddYears(-60);

            foreach (int key in dictionary.Keys)
            {
                cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleLatestRank);
                cmd.Where(Mabinogi.SQL.Columns.ChronicleLatestRank.ServerName, _list.serverName);
                cmd.Where(Mabinogi.SQL.Columns.ChronicleLatestRank.QuestId, key);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleLatestRank, transaction);
                        cmd.Set(Mabinogi.SQL.Columns.ChronicleLatestRank.ServerName, _list.serverName);
                        cmd.Set(Mabinogi.SQL.Columns.ChronicleLatestRank.QuestId, key);
                        cmd.Set(Mabinogi.SQL.Columns.ChronicleLatestRank.RankTime, past);
                        cmd.Set(Mabinogi.SQL.Columns.ChronicleLatestRank.CharId, 0);
                        cmd.Set(Mabinogi.SQL.Columns.ChronicleLatestRank.CharName, string.Empty);
                        cmd.Set(Mabinogi.SQL.Columns.ChronicleLatestRank.Rank, 0);
                        cmd.Execute();
                    }
                }
            }
        }

        private void UpdateChronicleInfo(Dictionary<int, ChronicleInfo> dictionary, ChronicleInfo[] _infos, SimpleConnection conn, SimpleTransaction transaction)
        {

            SimpleCommand cmd;
            ChronicleInfo info;

            foreach (ChronicleInfo chronicleInfo in _infos)
            {

                if (dictionary.TryGetValue(chronicleInfo.questID, out info))
                {
                    if (!chronicleInfo.ContentEquals(info))
                    {
                        cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleInfo);
                        cmd.Where(Mabinogi.SQL.Columns.ChronicleInfo.QuestId, chronicleInfo.questID);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleInfo, transaction);
                                cmd.Where(Mabinogi.SQL.Columns.ChronicleInfo.QuestId, chronicleInfo.questID);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.QuestName, reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.QuestName));
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Keyword, reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.Keyword));
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.LocalText, reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.LocalText));
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Sort, reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.Sort));
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Group, reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.Group));
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Source, reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.Source));
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Width, reader.GetInt16(Mabinogi.SQL.Columns.ChronicleInfo.Width));
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Height, reader.GetInt16(Mabinogi.SQL.Columns.ChronicleInfo.Height));
                            }
                            else
                            {
                                cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleInfo, transaction);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.QuestId, chronicleInfo.questID);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.QuestName, chronicleInfo.questName);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Keyword, chronicleInfo.keyword);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.LocalText, chronicleInfo.localtext);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Sort, chronicleInfo.sort);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Group, chronicleInfo.group);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Source, chronicleInfo.source);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Width, chronicleInfo.width);
                                cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Height, chronicleInfo.height);
                            }
                        }
                    }
                }
                else
                {
                    cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleInfo);
                    cmd.Where(Mabinogi.SQL.Columns.ChronicleInfo.QuestId, chronicleInfo.questID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleInfo, transaction);
                            cmd.Where(Mabinogi.SQL.Columns.ChronicleInfo.QuestId, chronicleInfo.questID);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.QuestName, reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.QuestName));
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Keyword, reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.Keyword));
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.LocalText, reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.LocalText));
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Sort, reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.Sort));
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Group, reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.Group));
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Source, reader.GetString(Mabinogi.SQL.Columns.ChronicleInfo.Source));
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Width, reader.GetInt16(Mabinogi.SQL.Columns.ChronicleInfo.Width));
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Height, reader.GetInt16(Mabinogi.SQL.Columns.ChronicleInfo.Height));
                        }
                        else
                        {
                            cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleInfo, transaction);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.QuestId, chronicleInfo.questID);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.QuestName, chronicleInfo.questName);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Keyword, chronicleInfo.keyword);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.LocalText, chronicleInfo.localtext);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Sort, chronicleInfo.sort);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Group, chronicleInfo.group);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Source, chronicleInfo.source);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Width, chronicleInfo.width);
                            cmd.Set(Mabinogi.SQL.Columns.ChronicleInfo.Height, chronicleInfo.height);
                        }
                    }
                }
            }
        }
        public bool UpdateChronicleInfoList(ChronicleInfoList _list)
        {
            WorkSession.WriteStatus("ChronicleSqlAdapter.UpdateChronicleInfoList() : 함수에 진입하였습니다");
            if (_list != null && _list.infos != null && _list.infos.Length > 0)
            {
                SimpleTransaction transaction = null;
                using (var conn = Connection)
                {
                    try
                    {

                        SimpleCommand cmd = conn.GetSelectCommand(string.Format("SELECT `{0}`, MAX(`{1}`) AS {2} FROM `{3}` WHERE {4} GROUP BY {0};",
                            Mabinogi.SQL.Columns.ChronicleEventRank.QuestId, Mabinogi.SQL.Columns.ChronicleEventRank.Rank, Mabinogi.SQL.Columns.Reference.TotalCount, Mabinogi.SQL.Tables.MabiChronicle.ChronicleEventRank, Mabinogi.SQL.Columns.ChronicleEventRank.ServerName));
                        transaction = conn.BeginTransaction();

                        Dictionary<int, int> hashTable;
                        using (var reader = cmd.ExecuteReader())
                        {
                            hashTable = ChronicleCacheBuilder.Build(reader, _list.infos);
                        }

                        UpdateChronicleCache(hashTable, _list, conn, transaction);

                        WorkSession.WriteStatus("ChronicleSqlAdapter.UpdateChronicleInfoList() : 데이터베이스에 연결합니다");

                        WorkSession.WriteStatus("ChronicleSqlAdapter.UpdateChronicleInfoList() : 데이터베이스로부터 이미지를 읽어옵니다.");
                        cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiChronicle.ChronicleInfo);

                        Dictionary<int, ChronicleInfo> dict;
                        using (var reader = cmd.ExecuteReader())
                        {
                            dict = ChronicleInfoBuilder.Build(reader);
                        }
                        UpdateChronicleInfo(dict, _list.infos, conn, transaction);

                        transaction.Commit();


                        return true;

                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        transaction?.Rollback();
                        ObjectCache.DeleteChronicleCache();
                        return false;
                    }
                    catch (Exception ex2)
                    {
                        ExceptionMonitor.ExceptionRaised(ex2);
                        WorkSession.WriteStatus(ex2.Message);
                        transaction?.Rollback();
                        ObjectCache.DeleteChronicleCache();
                        return false;
                    }
                    finally
                    {
                    }
                }
            }
            WorkSession.WriteStatus("ChronicleSqlAdapter.UpdateChronicleInfoList() : 이미지가 없습니다.");
            return true;
        }
    }
}
