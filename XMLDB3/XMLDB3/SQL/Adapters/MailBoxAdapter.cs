using System;
using System.Collections;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class MailBoxAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.MailBoxReceive;
        public MailBoxAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public long CheckCharacterName(string _name, ref string _outname, ref byte _errorCode)
        {
            WorkSession.WriteStatus("MailBoxSqlAdapter.CheckCharacterName() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.CheckCharacterName() : 데이터베이스와 연결합니다");
                //PROCEDURE: CheckCharacterName
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.GameId))
                {
                    WorkSession.WriteStatus("MailBoxSqlAdapter.CheckCharacterName() : 프로시져 명령 객체를 작성합니다");
                    cmd.Where(Mabinogi.SQL.Columns.GameId.Name, _name);

                    using (var reader = cmd.ExecuteReader())
                    {
                        WorkSession.WriteStatus("MailBoxSqlAdapter.CheckCharacterName() : DataSet 을 채웁니다");

                        long result;
                        if (reader != null && reader.Read())
                        {
                            if (reader.GetByte(Mabinogi.SQL.Columns.GameId.Flag) == 1)
                            {
                                result = reader.GetInt64(Mabinogi.SQL.Columns.GameId.Id);
                                _outname = reader.GetString(Mabinogi.SQL.Columns.GameId.Name);
                            }
                            else
                            {
                                result = 0L;
                                _errorCode = 5;
                            }
                        }
                        else
                        {
                            result = 0L;
                            _errorCode = 4;
                        }
                        return result;
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _name);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                _errorCode = 0;
                return 0L;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message);
                ExceptionMonitor.ExceptionRaised(ex2);
                _errorCode = 0;
                return 0L;
            }
            finally
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.CheckCharacterName() : 연결을 종료합니다");
            }
        }

        private bool CheckMailBox(long _charID, ref byte _errorCode)
        {
            WorkSession.WriteStatus("MailBoxSqlAdapter.CheckMailBox() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.CheckMailBox() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetSelectCommand(string.Format("SELECT count(*) {0} FROM {1} WHERE {2} = @{2} AND ({3} = 1 OR {3} = 2)",
                    /* 0 */ Mabinogi.SQL.Columns.Reference.Count, /* 1 */ Mabinogi.SQL.Tables.Mabinogi.MailBoxReceive, /* 2 */ Mabinogi.SQL.Columns.MailBoxReceive.ReceiverCharId, /* 3 */ Mabinogi.SQL.Columns.MailBoxReceive.PostType)))
                {
                    cmd.AddParameter(Mabinogi.SQL.Columns.MailBoxReceive.ReceiverCharId, _charID);


                    WorkSession.WriteStatus("MailBoxSqlAdapter.CheckMailBox() : 명령을 실행합니다");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && reader.GetInt32(Mabinogi.SQL.Columns.Reference.Count) > 29)
                        {
                            _errorCode = 0;
                            return true;
                        }
                    }


                    _errorCode = 1;
                    return false;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _charID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                _errorCode = 0;
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _charID);
                WorkSession.WriteStatus(ex2.Message, _charID);
                _errorCode = 0;
                return false;
            }
        }
        private static int postID = 2;
        public long SendMail(MailItem _mail, ref byte _errorCode)
        {
            WorkSession.WriteStatus("MailBoxSqlAdapter.SendMail() : 함수에 진입하였습니다");
            if ((_mail.postType == 1 || _mail.postType == 2) && !CheckMailBox(_mail.receiverCharID, ref _errorCode))
            {
                return 0L;
            }

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("MailBoxSqlAdapter.SendMail() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();
                    var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.MailBoxReceive, transaction);
                    {
                        cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.PostId, postID);
                        cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.ReceiverCharId, _mail.receiverCharID);
                        cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.ReceiverCharName, _mail.receiverCharName);
                        cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.SenderCharId, _mail.senderCharID);
                        cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.SenderCharName, _mail.senderCharName);
                        if (_mail.item != null)
                            cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.ItemId, _mail.item.id);
                        else
                            cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.ItemId, 0);
                        cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.ItemCharge, _mail.itemCharge);
                        cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.SenderMessage, _mail.senderMsg);
                        cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.SendDate, _mail.sendDate);
                        cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.PostType, _mail.postType);
                        cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.Location, _mail.location);
                        cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.Status, _mail.status);
                        cmd.Execute();
                    }

                    long id = postID;
                    postID++;
                    /*
                    cmd = conn.GetSelectCommand("SELECT LAST_VALUE() Identity", transaction);
                    {
                        var reader = cmd.ExecuteReader();
                        id = reader.GetInt64("Identity");
                    }*/
                    string cmdStr = string.Empty;

                    if (_mail.item != null)
                    {
                        cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.MailBoxItem, transaction);
                        {
                            cmd.Set(Mabinogi.SQL.Columns.MailBoxItem.ReceiverCharID, _mail.receiverCharID);
                            cmd.Set(Mabinogi.SQL.Columns.MailBoxItem.SenderCharID, _mail.senderCharID);
                            cmd.Set(Mabinogi.SQL.Columns.MailBoxItem.StoredType, _mail.item.storedtype);
                            cmd.Set(Mabinogi.SQL.Columns.Item.UpdateTime, DateTime.Now);
                            cmd.Set(Mabinogi.SQL.Columns.Item.ItemId, _mail.item.id);
                            cmd.Set(Mabinogi.SQL.Columns.Item.ItemLoc, 3);

                            switch (_mail.item.storedtype)
                            {
                                case 1:
                                    {
                                        cmdStr = Mabinogi.SQL.Tables.Mabinogi.ItemLargeId;
                                        ItemParameterBuilder.BuildLargeItem(_mail.item, cmd);
                                    }
                                    break;
                                case 2:
                                    {
                                        cmdStr = Mabinogi.SQL.Tables.Mabinogi.ItemSmallId;
                                        ItemParameterBuilder.BuildSmallItem(_mail.item, cmd);
                                    }
                                    break;
                                case 3:
                                    {
                                        cmdStr = Mabinogi.SQL.Tables.Mabinogi.ItemHugeId;
                                        ItemParameterBuilder.BuildHugeItem(_mail.item, cmd);
                                    }
                                    break;
                                case 4:
                                    {
                                        cmdStr = Mabinogi.SQL.Tables.Mabinogi.ItemQuestId;
                                        ItemParameterBuilder.BuildQuestItem(_mail.item, cmd);
                                    }
                                    break;
                            }
                            cmd.Execute();
                        }


                        cmd = conn.GetDefaultInsertCommand(cmdStr, transaction);
                        {
                            cmd.Set(Mabinogi.SQL.Columns.ItemId.Id, id);
                            cmd.Set(Mabinogi.SQL.Columns.ItemId.ItemLoc, 3);
                            cmd.Execute();
                        }
                    }
                    transaction.Commit();
                    return id;

                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _mail.senderCharName);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    transaction?.Rollback();
                    return 0L;
                }
                catch (Exception ex2)
                {
                    WorkSession.WriteStatus(ex2.Message);
                    ExceptionMonitor.ExceptionRaised(ex2);
                    transaction?.Rollback();
                    return 0L;
                }
                finally
                {
                    WorkSession.WriteStatus("MailBoxSqlAdapter.SendMail() : 연결을 종료합니다");
                }
            }
        }

        private Hashtable BuildMailBoxItems(SimpleReader _itemTable)
        {
            if (_itemTable == null)
            {
                return null;
            }

            Hashtable hashtable = new Hashtable();
            if (_itemTable.HasRows)
            {
                Item charItem;
                while (_itemTable.Read())
                {
                    charItem = ItemSqlBuilder.GetItem(_itemTable.GetByte(Mabinogi.SQL.Columns.MailBoxItem.StoredType), _itemTable);
                    if (charItem != null)
                    {
                        hashtable.Add(charItem.id, charItem);
                    }
                    else
                    {
                        ExceptionMonitor.ExceptionRaised(new Exception("아이템을 빌드하는데 실패했습니다"), _itemTable.GetInt64(Mabinogi.SQL.Columns.Item.ItemId));
                    }
                }
            }
            return hashtable;
        }
        private MailBox BuildMailBox(SimpleReader _boxTable, Hashtable hashtable)
        {
            ArrayList arrayList = new ArrayList();

            if (_boxTable.HasRows)
            {
                MailItem mailItem;
                while (_boxTable.Read())
                {
                    mailItem = new MailItem();
                    mailItem.postID = _boxTable.GetInt64(Mabinogi.SQL.Columns.MailBoxReceive.PostId);
                    mailItem.receiverCharID = _boxTable.GetInt64(Mabinogi.SQL.Columns.MailBoxReceive.ReceiverCharId);
                    mailItem.receiverCharName = _boxTable.GetString(Mabinogi.SQL.Columns.MailBoxReceive.ReceiverCharName);
                    mailItem.senderCharID = _boxTable.GetInt64(Mabinogi.SQL.Columns.MailBoxReceive.SenderCharId);
                    mailItem.senderCharName = _boxTable.GetString(Mabinogi.SQL.Columns.MailBoxReceive.SenderCharName);
                    mailItem.itemCharge = _boxTable.GetInt32(Mabinogi.SQL.Columns.MailBoxReceive.ItemCharge);
                    mailItem.senderMsg = _boxTable.GetString(Mabinogi.SQL.Columns.MailBoxReceive.SenderMessage);
                    mailItem.sendDate = _boxTable.GetDateTime(Mabinogi.SQL.Columns.MailBoxReceive.SendDate);
                    mailItem.postType = _boxTable.GetByte(Mabinogi.SQL.Columns.MailBoxReceive.PostType);
                    mailItem.location = _boxTable.GetString(Mabinogi.SQL.Columns.MailBoxReceive.Location);
                    mailItem.status = _boxTable.GetByte(Mabinogi.SQL.Columns.MailBoxReceive.Status);
                    long num = _boxTable.GetInt64(Mabinogi.SQL.Columns.MailBoxReceive.ItemId);
                    if (num != 0)
                    {
                        if (!hashtable.Contains(num))
                        {
                            throw new Exception("메일에 아이템이 존재하지 않습니다. PostID:" + mailItem.postID);
                        }
                        mailItem.item = (Item)hashtable[num];
                    }
                    else
                    {
                        mailItem.item = null;
                    }
                    arrayList.Add(mailItem);
                }
            }
            MailBox mailBox = new MailBox();
            mailBox.mailItem = (MailItem[])arrayList.ToArray(typeof(MailItem));
            return mailBox;
        }

        public bool ReadMail(long _charID, MailBox _recvBox, MailBox _sendBox)
        {
            WorkSession.WriteStatus("MailBoxSqlAdapter.ReadMail() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.ReadMail() : 데이터베이스와 연결합니다");
                // PROCEDURE: MailBoxSelect
                using (var conn = Connection)
                {
                    WorkSession.WriteStatus("MailBoxSqlAdapter.ReadMail() : 프로시져 명령 객체를 작성합니다");


                    Hashtable hashtable;

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.MailBoxItem))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.MailBoxItem.ReceiverCharID, _charID);
                        using (var recvItem = cmd.ExecuteReader())
                            hashtable = BuildMailBoxItems(recvItem);
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.MailBoxReceive))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.MailBoxReceive.ReceiverCharId, _charID);
                        cmd.OrderBy(Mabinogi.SQL.Columns.MailBoxReceive.PostId, false);
                        using (var recvBox = cmd.ExecuteReader())
                            _recvBox.mailItem = BuildMailBox(recvBox, hashtable).mailItem;
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.MailBoxItem))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.MailBoxItem.SenderCharID, _charID);
                        using (var sendItem = cmd.ExecuteReader())
                            hashtable = BuildMailBoxItems(sendItem);
                    }

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.MailBoxReceive))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.MailBoxReceive.SenderCharId, _charID);
                        cmd.OrderBy(Mabinogi.SQL.Columns.MailBoxReceive.PostId, false);
                        using (var sendBox = cmd.ExecuteReader())
                            _sendBox.mailItem = BuildMailBox(sendBox, hashtable).mailItem;
                    }

                    WorkSession.WriteStatus("MailBoxSqlAdapter.ReadMail() : DataSet 에 우편함 정보를 채웁니다");

                    return true;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _charID);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message);
                ExceptionMonitor.ExceptionRaised(ex2);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.ReadMail() : 연결을 종료합니다");
            }
        }

        public bool UpdateMail(long _postID, byte _status, long _receiverCharID, long _senderCharID)
        {
            WorkSession.WriteStatus("MailBoxSqlAdapter.UpdateMail() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("MailBoxSqlAdapter.UpdateMail() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {
                    // PROCEDURE: MailUpdate
                    transaction = conn.BeginTransaction();
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.MailBoxReceive, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.MailBoxReceive.PostId, _postID);
                        cmd.Set(Mabinogi.SQL.Columns.MailBoxReceive.Status, _status);

                        WorkSession.WriteStatus("MailBoxSqlAdapter.UpdateMail() : 명령을 실행합니다");
                        if (cmd.Execute() > 0)
                        {
                            transaction.Commit();
                            return true;
                        }
                    }

                    transaction?.Rollback();
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _postID);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    transaction?.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    WorkSession.WriteStatus(ex2.Message, _postID);
                    ExceptionMonitor.ExceptionRaised(ex2);
                    transaction?.Rollback();
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("MailBoxSqlAdapter.UpdateMail() : 연결을 종료합니다");
                }
            }
        }

        public bool DeleteMail(long _postID, long _itemID, byte _storedType, long _receiverCharID, long _senderCharID, ref byte _errorCode)
        {
            WorkSession.WriteStatus("MailBoxSqlAdapter.DeleteMail() : 함수에 진입하였습니다");
            _errorCode = 0;

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("MailBoxSqlAdapter.DeleteMail() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    transaction = conn.BeginTransaction();
                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.MailBoxReceive, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.MailBoxReceive.PostId, _postID);

                        WorkSession.WriteStatus("MailBoxSqlAdapter.DeleteMail() : 명령을 실행합니다");
                        if (cmd.Execute() > 0)
                        {
                            if (_itemID == 0)
                            {
                                transaction.Commit();
                                return true;
                            }
                        }
                        else
                        {
                            transaction.Commit();
                            _errorCode = 3;
                            return false;
                        }
                    }
                    string cmdStr = string.Empty;

                    switch (_storedType)
                    {
                        case 1:
                            {
                                cmdStr = Mabinogi.SQL.Tables.Mabinogi.ItemLargeId;
                            }
                            break;
                        case 2:
                            {
                                cmdStr = Mabinogi.SQL.Tables.Mabinogi.ItemSmallId;
                            }
                            break;
                        case 3:
                            {
                                cmdStr = Mabinogi.SQL.Tables.Mabinogi.ItemHugeId;
                            }
                            break;
                        case 4:
                            {
                                cmdStr = Mabinogi.SQL.Tables.Mabinogi.ItemQuestId;
                            }
                            break;
                    }

                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.MailBoxItem, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Item.ItemId, _itemID);
                        cmd.Execute();
                    }

                    using (var cmd = conn.GetDefaultDeleteCommand(cmdStr, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.ItemId.Id, _itemID);
                        cmd.Where(Mabinogi.SQL.Columns.ItemId.ItemLoc, 3);
                        cmd.Execute();
                    }

                    transaction.Commit();
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _postID);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    transaction?.Rollback();
                    return false;
                }
                catch (Exception ex2)
                {
                    WorkSession.WriteStatus(ex2.Message);
                    ExceptionMonitor.ExceptionRaised(ex2);
                    transaction?.Rollback();
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("MailBoxSqlAdapter.DeleteMail() : 연결을 종료합니다");
                }
            }
        }

        public bool GetUnreadCount(long _receiverID, out int _unreadCount)
        {
            WorkSession.WriteStatus("MailBoxSqlAdapter.GetUnreadCount() : 함수에 진입하였습니다");
            _unreadCount = 0;

            try
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.GetUnreadCount() : 데이터베이스와 연결합니다");
                // PROCEDURE: MailGetUnreadCount
                using (var conn = Connection)
                using (var cmd = conn.GetSelectCommand(string.Format("SELECT count(*) {0} FROM {1} WHERE {2} = @{2} AND {3} = @{3}",
                    /* 0 */Mabinogi.SQL.Columns.Reference.Count, /* 1 */ Mabinogi.SQL.Tables.Mabinogi.MailBoxReceive, /* 2 */Mabinogi.SQL.Columns.MailBoxReceive.ReceiverCharId, /* 3 */Mabinogi.SQL.Columns.MailBoxReceive.Status)))
                {
                    cmd.AddParameter(Mabinogi.SQL.Columns.MailBoxReceive.ReceiverCharId, _receiverID);
                    cmd.AddParameter(Mabinogi.SQL.Columns.MailBoxReceive.Status, 3);

                    WorkSession.WriteStatus("MailBoxSqlAdapter.GetUnreadCount() : 명령을 실행합니다");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            _unreadCount = reader.GetInt32(Mabinogi.SQL.Columns.Reference.Count);
                    }
                }
                return true;
            }
            catch (SimpleSqlException ex)
            {
                WorkSession.WriteStatus(ex.Message);
                ExceptionMonitor.ExceptionRaised(ex);
                return false;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message);
                ExceptionMonitor.ExceptionRaised(ex2);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("MailBoxSqlAdapter.GetUnreadCount() : 연결을 종료합니다");
            }
        }
    }
}
