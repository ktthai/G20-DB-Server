using Mabinogi;
using System;
using System.Collections;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class CharacterAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Character;

        public CharacterAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        private CharacterInfo _Read(long _id)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter._Read() : 함수에 진입하였습니다");
            try
            {
                CharacterInfo characterInfo;
                using (var conn = Connection)
                    characterInfo = CharacterObjectBuilder.Build(_id, conn);


                long timestamp = Stopwatch.GetTimestamp();
                CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctCharacterRead, Stopwatch.GetElapsedMilliseconds(timestamp));
                WorkSession.WriteStatus("CharacterSqlAdapter._Read() : DataSet 으로부터 캐릭터 정보를 생성합니다");
                return characterInfo;
            }
            finally
            {
                WorkSession.WriteStatus("CharacterSqlAdapter._Read() : 연결을 종료합니다");
            }
        }

        public CharacterInfo Read(long _id, CharacterInfo _cache)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.Read() : 함수에 진입하였습니다");
            try
            {
                if (_cache == null)
                {
                    return _Read(_id);
                }
                if (!IsValidCache(_cache))
                {
                    return _Read(_id);
                }
                return _cache;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _id);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _id);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
        }

        public bool IsValidCache(CharacterInfo _cache)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 함수에 진입하였습니다");
            if (_cache == null)
            {
                return false;
            }
            try
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 데이터베이스에 연결합니다");
                DateTime updateTime;
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Character))
                {
                    cmd.Where(Mabinogi.SQL.Columns.Character.Id, _cache.id);
                    cmd.Set(Mabinogi.SQL.Columns.Character.UpdateTime, 0);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            updateTime = reader.GetDateTime(Mabinogi.SQL.Columns.Character.UpdateTime);
                        else
                            return false;
                    }
                }


                WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜를 얻어옵니다");

                if (updateTime != null)
                {
                    if (UpdateUtility.CacheMissDate.Ticks >= updateTime.Ticks)
                    {
                        WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 캐쉬가 유효하지 않습니다. - by CacheMissDate");
                        return false;
                    }
                    if (UpdateUtility.CacheMissDate.Ticks <= _cache.updatetime.Ticks)
                    {
                        WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 마지막으로 변경된 날짜가 캐쉬 날짜보다 빠릅니다. 캐쉬의 정보가 최신입니다");
                        return true;
                    }
                    WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 캐쉬가 유효하지 않습니다.");
                    return false;
                }
                return false;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _cache);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _cache);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.IsValidCache() : 데이터베이스에 연결을 종료합니다");
            }
        }

        public bool Write(CharacterInfo _character, CharacterInfo _cache, out Message _outBuildResultMsg)
        {
            return _Write(_character, _cache, _forceLinkUpdate: false, out _outBuildResultMsg);
        }

        private bool _Write(CharacterInfo _character, CharacterInfo _cache, bool _forceLinkUpdate, out Message _outBuildResultMsg)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.Write() : 함수에 진입하였습니다");
            _outBuildResultMsg = new Message();

            WorkSession.WriteStatus("CharacterSqlAdapter.Write() : 저장되어 있는 데이터를 읽어옵니다");
            if (_cache == null)
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.Write() : 저장되어 있는 데이터가 없어 새로 만듭니다.");
                _cache = _Read(_character.id);
            }
            else if (!IsValidCache(_cache))
            {
                _cache = _Read(_character.id);
            }

            using (var conn = Connection)
            {
                SimpleTransaction transaction = null;
                try
                {
                    transaction = conn.BeginTransaction();
                    WorkSession.WriteStatus("CharacterSqlAdapter.Write() : 데이터베이스와 연결합니다");
                    long timestamp = Stopwatch.GetTimestamp();

                    InventoryUpdateBuilder.Build(_character.id, _character.inventory, _cache.inventory, _forceLinkUpdate, conn, transaction, out _character.strToHash);
                    _character.inventoryHash = InventoryHashUtility.ComputeHash(_character.strToHash);

                    CharacterUpdateBuilder.Build(_character, _cache, conn, transaction, out _outBuildResultMsg);
                    UpdateMeta(_character, _cache, conn, transaction);
                    transaction.Commit();
                    CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctCharacterWrite, Stopwatch.GetElapsedMilliseconds(timestamp));
                    return true;

                }
                catch (SimpleSqlException ex)
                {
                    transaction?.Rollback();
                    if (!_forceLinkUpdate && ItemSqlBuilder.ForceUpdateRetry(ex))
                    {
                        WorkSession.WriteStatus("CharacterSqlAdapter.Write() : 아이템 오류로 재시도합니다.");
                        ExceptionMonitor.ExceptionRaised(ex, _character);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return _Write(_character, null, _forceLinkUpdate: true, out _outBuildResultMsg);
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _character);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    transaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex2, _character);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.Write() : 연결을 종료합니다");
                }
            }
        }



        public void UpdateMeta(CharacterInfo _character, CharacterInfo _cache, SimpleConnection con, SimpleTransaction trans)
        {
            Hashtable hashtable = CMetaHelper.MetaStringToMetaRowList(_character.id, _character.data.meta);
            Hashtable hashtable2 = CMetaHelper.MetaStringToMetaRowList(_cache.id, _cache.data.meta);
            foreach (character_meta_row value in hashtable.Values)
            {
                if (!hashtable2.ContainsKey(value.mcode))
                {
                    CDBAPI.UpdateCharacterMeta(con, trans, value);
                }
                else
                {
                    character_meta_row character_meta_row2 = (character_meta_row)hashtable2[value.mcode];
                    if (character_meta_row2.mdata != value.mdata)
                    {
                        CDBAPI.UpdateCharacterMeta(con, trans, value);
                    }
                }
            }
            foreach (string key in hashtable2.Keys)
            {
                if (!hashtable.ContainsKey(key))
                {
                    CDBAPI.DeleteCharacterMeta(con, trans, (character_meta_row)hashtable2[key]);
                }
            }
        }



        public bool CreateEx(string _account, byte _supportRewardState, string _server, byte _race, bool _supportCharacter, CharacterInfo data, AccountRefAdapter accountrefAdapter, BankAdapter bankAdapter, WebSynchAdapter _websynch)
        {
            if (ConfigManager.IsLocalMode)
            {
                return SQLite_CreateEx(_account, _supportRewardState, _server, _race, _supportCharacter, data, accountrefAdapter, bankAdapter, _websynch);
            }
            return MySql_CreateEx( _account,  _supportRewardState, _server, _race,  _supportCharacter, data, accountrefAdapter, bankAdapter, _websynch);
        }

        private bool SQLite_CreateEx(string _account, byte _supportRewardState, string _server, byte _race, bool _supportCharacter, CharacterInfo data, AccountRefAdapter accountrefAdapter, BankAdapter bankAdapter, WebSynchAdapter _websynch)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;

            using (var conn = Connection)
            {
                try
                {

                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 데이터베이스와 연결합니다");

                    SimpleCommand cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.NotUsableGameId);
                    cmd.Where(Mabinogi.SQL.Columns.NotUsableGameId.Name, data.name);

                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 캐릭터 이름 중복 검사를 실행합니다");
                    if (cmd.ExecuteReader().HasRows)
                    {
                        ExceptionMonitor.ExceptionRaised(new Exception("캐릭터 이름이 중복됩니다."), _account, data.name);
                        return false;
                    }

                    transaction = conn.BeginTransaction();

                    CharacterCreateBuilder.Build(data, conn, transaction);

                    WorkSession.WriteStatus("캐릭터 메타 데이터를 추가합니다.");

                    Hashtable hashtable = CMetaHelper.MetaStringToMetaRowList(data.id, data.data.meta);
                    foreach (character_meta_row value in hashtable.Values)
                    {
                        CDBAPI.UpdateCharacterMeta(conn, transaction, value);
                    }
                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 뱅크 생성 명령을 실행합니다");
                    var bankSlot = new BankSlot(data.name, (BankRace)_race);
                    bankSlot.slot.itemHash = InventoryHashUtility.ComputeHash(bankSlot.Name, bankSlot.item);
                    SlotUpdateBuilder.AddBankSlot(bankSlot, _account, conn, transaction);

                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 계정-캐릭터 링크 생성 명령을 실행합니다");
                    var charref = new AccountrefCharacter() { id = data.id, name = data.name, server = _server, race = _race, supportCharacter = _supportCharacter };
                    accountrefAdapter.CreateCharacterRef(_account, charref, conn, transaction);

                    accountrefAdapter.SetAccountRewardSupportState(_account, _supportRewardState, conn, transaction);

                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 웹싱크 명령을 실행합니다");
                    _websynch.AddCharRefSync(_account, _server, data, conn, transaction);

                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 트랜잭션을 커밋합니다");

                    transaction.Commit();

                    return true;

                }
                catch (SimpleSqlException ex)
                {
                    transaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex, _account, data);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 트랜잭션을 롤백합니다");
                    return false;
                }
                catch (Exception ex2)
                {
                    transaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex2, _account, data);
                    WorkSession.WriteStatus(ex2.Message, _account);
                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 트랜잭션을 롤백합니다");
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 연결을 종료합니다");
                    transaction?.Dispose();
                }
            }
        }

        private bool MySql_CreateEx(string _account, byte _supportRewardState, string _server, byte _race, bool _supportCharacter, CharacterInfo data, AccountRefAdapter accountrefAdapter, BankAdapter bankAdapter, WebSynchAdapter _websynch)
        { 
            WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 함수에 진입하였습니다");
            SimpleTransaction charTransaction = null;
            SimpleTransaction accrefTransaction = null;
            SimpleTransaction bankTransaction = null;
            SimpleTransaction websyncTransaction = null;
            using (var websyncConn = _websynch.Connection)
            using (var accrefConn = accountrefAdapter.Connection)
            using (var charConn = Connection)
            using (var bankConn = bankAdapter.Connection)
            {
                try
                {

                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 데이터베이스와 연결합니다");

                    using (var cmd = charConn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.NotUsableGameId))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.NotUsableGameId.Name, data.name);

                        WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 캐릭터 이름 중복 검사를 실행합니다");

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Close();
                                ExceptionMonitor.ExceptionRaised(new Exception("캐릭터 이름이 중복됩니다."), _account, data.name);
                                return false;
                            }
                            reader.Close();
                        }
                    }

                    charTransaction = charConn.BeginTransaction();

                    accrefTransaction = accrefConn.BeginTransaction();

                    bankTransaction = bankConn.BeginTransaction();

                    websyncTransaction = websyncConn.BeginTransaction();

                    CharacterCreateBuilder.Build(data, charConn, charTransaction);

                    WorkSession.WriteStatus("캐릭터 메타 데이터를 추가합니다.");

                    Hashtable hashtable = CMetaHelper.MetaStringToMetaRowList(data.id, data.data.meta);
                    foreach (character_meta_row value in hashtable.Values)
                    {
                        CDBAPI.UpdateCharacterMeta(charConn, charTransaction, value);
                    }

                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 뱅크 생성 명령을 실행합니다");
                    var bankSlot = new BankSlot(data.name, (BankRace)_race);
                    bankSlot.slot.itemHash = InventoryHashUtility.ComputeHash(bankSlot.Name, bankSlot.item);
                    SlotUpdateBuilder.AddBankSlot(bankSlot, _account, bankConn, bankTransaction);

                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 계정-캐릭터 링크 생성 명령을 실행합니다");
                    var charref = new AccountrefCharacter() { id = data.id, name = data.name, server = _server, race = _race, supportCharacter = _supportCharacter };
                    accountrefAdapter.CreateCharacterRef(_account, charref, accrefConn, accrefTransaction);

                    accountrefAdapter.SetAccountRewardSupportState(_account, _supportRewardState, accrefConn, accrefTransaction);

                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 웹싱크 명령을 실행합니다");
                    _websynch.AddCharRefSync(_account, _server, data, websyncConn, websyncTransaction);

                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 트랜잭션을 커밋합니다");

                    charTransaction.Commit();
                    accrefTransaction.Commit();
                    bankTransaction.Commit();
                    websyncTransaction.Commit();
                    return true;

                }
                catch (SimpleSqlException ex)
                {
                    charTransaction?.Rollback();
                    accrefTransaction?.Rollback();
                    bankTransaction?.Rollback();
                    websyncTransaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex, _account, data);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 트랜잭션을 롤백합니다");
                    return false;
                }
                catch (Exception ex2)
                {
                    charTransaction?.Rollback();
                    accrefTransaction?.Rollback();
                    bankTransaction?.Rollback();
                    websyncTransaction?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex2, _account, data);
                    WorkSession.WriteStatus(ex2.Message, _account);
                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 트랜잭션을 롤백합니다");
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.CreateEx() : 연결을 종료합니다");
                    charTransaction?.Dispose();
                    accrefTransaction?.Dispose();
                    bankTransaction?.Dispose();
                    websyncTransaction?.Dispose();
                }
            }
        }

        public bool DeleteEx(string account, byte _supportRace, byte _supportRewardState, int _supportLastChangeTime, string serverName, long charID, AccountRefAdapter accrefAdapter, BankAdapter bankAdapter, GuildAdapter guildAdapter, WebSynchAdapter websyncAdapter)
        {
            if (ConfigManager.IsLocalMode)
            {
                return SQLite_DeleteEx(account, _supportRace, _supportRewardState, _supportLastChangeTime, serverName, charID, accrefAdapter, bankAdapter, guildAdapter, websyncAdapter);
            }
            return MySql_DeleteEx(account, _supportRace, _supportRewardState, _supportLastChangeTime, serverName, charID, accrefAdapter, bankAdapter, guildAdapter, websyncAdapter);
        }

        private bool SQLite_DeleteEx(string account, byte _supportRace, byte _supportRewardState, int _supportLastChangeTime, string serverName, long charID, AccountRefAdapter accrefAdapter, BankAdapter bankAdapter, GuildAdapter guildAdapter, WebSynchAdapter websyncAdapter)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 함수에 진입하였습니다");
            SimpleTransaction trans = null;

            using (var conn = this.Connection)
            {
                try
                {

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 데이터베이스와 연결합니다");

                    trans = conn.BeginTransaction();

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 뱅크 삭제 명령을 실행합니다");
                    string charName = this.GetCharName(charID, conn);
                    SlotUpdateBuilder.RemoveBankSlot(account, charName, conn, trans);

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 계정-캐릭터 링크를 제거명령을 실행합니다");
                    accrefAdapter.DeleteAccountrefCharacter(account, charID, serverName, conn, trans);
                    accrefAdapter.WriteAccountSupportState(account, _supportRace, _supportRewardState, _supportLastChangeTime, conn, trans);

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 길드 멤버 제거명령을 실행합니다");
                    guildAdapter.RemoveGuildMemberEx(charID, serverName, conn, trans);

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 웹 싱크 명령을 실행합니다");
                    websyncAdapter.RemoveCharacter(account, charID, serverName, conn, trans);

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 캐릭터 제거 명령을 실행합니다");
                    DeleteCharacter(charID, conn, trans);

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 트랜잭션을 커밋합니다");
                    trans.Commit();
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 트랜잭션을 롤백합니다");
                    trans?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex, account, charID);
                    WorkSession.WriteStatus(ex.Message, account);
                    return false;
                }
                catch (Exception ex2)
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 트랜잭션을 롤백합니다");
                    trans?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex2, account, charID);
                    WorkSession.WriteStatus(ex2.Message, account);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 연결을 종료합니다");
                    trans?.Dispose();
                }
            }
        }
        private bool MySql_DeleteEx(string account, byte _supportRace, byte _supportRewardState, int _supportLastChangeTime, string serverName, long charID, AccountRefAdapter accrefAdapter, BankAdapter bankAdapter, GuildAdapter guildAdapter, WebSynchAdapter websyncAdapter)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 함수에 진입하였습니다");
            SimpleTransaction charTrans = null;
            SimpleTransaction accrefTrans = null;
            SimpleTransaction bankTrans = null;
            SimpleTransaction guildTrans = null;
            SimpleTransaction websyncTrans = null;

            using (var charConn = this.Connection)
            using (var accrefConn = accrefAdapter.Connection)
            using (var bankConn = bankAdapter.Connection)
            using (var guildConn = guildAdapter.Connection)
            using (var websyncConn = websyncAdapter.Connection)
            {
                try
                {

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 데이터베이스와 연결합니다");

                    charTrans = charConn.BeginTransaction();
                    accrefTrans = accrefConn.BeginTransaction();
                    bankTrans = bankConn.BeginTransaction();
                    guildTrans = guildConn.BeginTransaction();

                    websyncTrans = websyncConn.BeginTransaction();

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 뱅크 삭제 명령을 실행합니다");
                    string charName = this.GetCharName(charID, charConn);
                    SlotUpdateBuilder.RemoveBankSlot(account, charName, bankConn, bankTrans);

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 계정-캐릭터 링크를 제거명령을 실행합니다");
                    accrefAdapter.DeleteAccountrefCharacter(account, charID, serverName, accrefConn, accrefTrans);
                    accrefAdapter.WriteAccountSupportState(account, _supportRace, _supportRewardState, _supportLastChangeTime, accrefConn, accrefTrans);

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 길드 멤버 제거명령을 실행합니다");
                    guildAdapter.RemoveGuildMemberEx(charID, serverName, guildConn, guildTrans);

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 웹 싱크 명령을 실행합니다");
                    websyncAdapter.RemoveCharacter(account, charID, serverName, websyncConn, websyncTrans);

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 캐릭터 제거 명령을 실행합니다");
                    DeleteCharacter(charID, charConn, charTrans);

                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 트랜잭션을 커밋합니다");
                    charTrans.Commit();
                    accrefTrans.Commit();
                    bankTrans.Commit();
                    guildTrans.Commit();
                    websyncTrans.Commit();
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 트랜잭션을 롤백합니다");
                    charTrans?.Rollback();
                    accrefTrans?.Rollback();
                    bankTrans?.Rollback();
                    guildTrans?.Rollback();
                    websyncTrans?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex, account, charID);
                    WorkSession.WriteStatus(ex.Message, account);
                    return false;
                }
                catch (Exception ex2)
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 트랜잭션을 롤백합니다");
                    charTrans?.Rollback();
                    accrefTrans?.Rollback();
                    bankTrans?.Rollback();
                    guildTrans?.Rollback();
                    websyncTrans?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex2, account, charID);
                    WorkSession.WriteStatus(ex2.Message, account);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteEx() : 연결을 종료합니다");
                    charTrans?.Dispose();
                    accrefTrans?.Dispose();
                    bankTrans?.Dispose();
                    guildTrans?.Dispose();
                    websyncTrans?.Dispose();
                }
            }
        }

        public void DeleteCharacter(long charID, SimpleConnection conn, SimpleTransaction trans)
        {
            byte charFlag;
                string gameIdName;
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.GameId))
            {
                cmd.Where(Mabinogi.SQL.Columns.GameId.Id, charID);

                
                using (var reader = cmd.ExecuteReader())
                {
                    gameIdName = reader.GetString(Mabinogi.SQL.Columns.GameId.Name);
                    charFlag = reader.GetByte(Mabinogi.SQL.Columns.GameId.Flag);
                }
            }

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.NotUsableGameId))
            {
                cmd.Where(Mabinogi.SQL.Columns.NotUsableGameId.Name, gameIdName);
                if (!cmd.ExecuteReader().HasRows)
                {

                    using (var cmd2 = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.NotUsableGameId, trans))
                    {
                        cmd2.Set(Mabinogi.SQL.Columns.NotUsableGameId.Id, charID);
                        cmd2.Set(Mabinogi.SQL.Columns.NotUsableGameId.Name, gameIdName);
                        cmd2.Set(Mabinogi.SQL.Columns.NotUsableGameId.Flag, charFlag);
                        cmd2.Set(Mabinogi.SQL.Columns.NotUsableGameId.InsertDate, DateTime.Now);

                        cmd2.Execute();
                    }
                }
            }

            string charName;
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Character))
            {
                cmd.Where(Mabinogi.SQL.Columns.Character.Id, charID);
                cmd.Set(Mabinogi.SQL.Columns.Character.Name, 0);


                using (var reader = cmd.ExecuteReader())
                    charName = reader.GetString(Mabinogi.SQL.Columns.Character.Name);
            }

            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Character, trans))
            {
                cmd.Set(Mabinogi.SQL.Columns.Character.Name, charName + charID);
                cmd.Set(Mabinogi.SQL.Columns.Character.DeleteTime, DateTime.Now);
                cmd.Where(Mabinogi.SQL.Columns.Character.Id, charID);
                cmd.Execute();
            }

            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.GameId, trans))
            {
                cmd.Set(Mabinogi.SQL.Columns.GameId.Name, gameIdName + charID);
                cmd.Where(Mabinogi.SQL.Columns.GameId.Id, charID);
                cmd.Execute();
            }

            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.DeleteChar, trans))
            {
                cmd.Set(Mabinogi.SQL.Columns.DeleteChar.Id, charID);
                cmd.Execute();
            }

            using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.RoyalAlchemist, trans))
            {
                cmd.Set(Mabinogi.SQL.Columns.RoyalAlchemist.CharId, charID);
                cmd.Execute();
            }

            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.PrivateFarm, trans))
            { 
                cmd.Set(Mabinogi.SQL.Columns.PrivateFarm.DeleteFlag, 1);
                cmd.Where(Mabinogi.SQL.Columns.PrivateFarm.OwnerId, charID);
                cmd.Execute();
            }

            // Clean up from SoulMate table now, rather than waste time making a run through the table each time on requesting a list (SelectSoulMate procedure)
            using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.SoulMate, trans))
            {
                cmd.Where(Mabinogi.SQL.Columns.SoulMate.MainCharId, charID);
                cmd.Execute();
            }

            using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.SoulMate, trans))
            {
                cmd.Where(Mabinogi.SQL.Columns.SoulMate.SubCharId, charID);
                cmd.Execute();
            }
        }

        public bool IsUsableName(string _name, string _account)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.IsUsableName() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.IsUsableName() : 데이터베이스와 연결합니다");

                bool result = false;
                if (_account.Length > 0)
                {
                    // Stored procedure was "CheckGameReservedName". Seems game names can be reserved.
                    // TODO: implement. no example. not priority
                    WorkSession.WriteStatus("CheckGameReservedName procedure was called with account: " + _account + " and name: " + _name);
                    return result;
                }
                else
                {
                    // PROCEDURE: CheckUsableName
                    using (var conn = Connection)
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.NotUsableGameId))
                    {

                        cmd.Where(Mabinogi.SQL.Columns.NotUsableGameId.Name, _name);

                        using (var reader = cmd.ExecuteReader())
                            if (reader.HasRows)
                                return false;

                        using (var cmd1 = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.GameId))
                        {
                            cmd1.Where(Mabinogi.SQL.Columns.GameId.Name, _name);

                            using (var reader = cmd1.ExecuteReader())
                                if (reader.HasRows)
                                    return false;
                        }
                    }
                    return true;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _name);
                WorkSession.WriteStatus(ex.Message, _name);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _name);
                WorkSession.WriteStatus(ex2.Message, _name);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.IsUsableName() : 연결을 종료합니다");
            }
        }

        public bool RemoveReservedCharName(string _name, string _account)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.RemoveReservedCharName() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("CharacterSqlAdapter.RemoveReservedCharName() : Called with Account: " + _account + " and Name: " + _name);
            // TODO: implement. no example. not priority
            return false;

            /*sqlconnection sqlconnection = new sqlconnection(base.connectionstring);
            sqltransaction sqltransaction = null;
            try
            {
                worksession.writestatus("charactersqladapter.removereservedcharname() : 데이터베이스와 연결합니다");
                sqlconnection.open();
                sqltransaction = sqlconnection.begintransaction("remove_reserved_char_name");
                sqlcommand sqlcommand = new sqlcommand("dbo.updategamereservednameused", sqlconnection, sqltransaction);
                sqlcommand.commandtype = commandtype.storedprocedure;
                sqlparameter sqlparameter = sqlcommand.parameters.add("@account", sqldbtype.varchar, 64);
                sqlparameter sqlparameter2 = sqlcommand.parameters.add("@charactername", sqldbtype.varchar, 64);
                sqlparameter.value = _account;
                sqlparameter2.value = _name;
                worksession.writestatus("charactersqladapter.removereservedcharname() : 명령을 실행합니다.");
                sqlcommand.executenonquery();
                worksession.writestatus("charactersqladapter.removereservedcharname() : 트랜잭션을 커밋합니다.");
                sqltransaction.commit();
                return true;
            }
            catch (simplesqlexception ex)
            {
                if (sqltransaction != null)
                {
                    worksession.writestatus("charactersqladapter.removereservedcharname() : 트랜잭션을 롤백합니다");
                    sqltransaction.rollback("remove_reserved_char_name");
                }
                exceptionmonitor.exceptionraised(ex, _name);
                worksession.writestatus(ex.message, _name);
                return false;
            }
            catch (exception ex2)
            {
                if (sqltransaction != null)
                {
                    worksession.writestatus("charactersqladapter.removereservedcharname() : 트랜잭션을 롤백합니다");
                    sqltransaction.rollback("remove_reserved_char_name");
                }
                exceptionmonitor.exceptionraised(ex2, _name);
                worksession.writestatus(ex2.message, _name);
                return false;
            }
            finally
            {
                worksession.writestatus("charactersqladapter.removereservedcharname() : 연결을 종료합니다");
                sqlconnection.close();
            }*/
        }

        public bool DeleteItem(long _id, ItemList[] _list, ref CharacterInfo _cache)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.DeleteItem() : 함수에 진입하였습니다");
            try
            {

                SimpleTransaction trans = null;

                if (_cache == null)
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.DeleteItem() : 저장되어 있는 데이터가 없어 새로 만듭니다.");
                    _cache = _Read(_id);
                }
                else if (!IsValidCache(_cache))
                {
                    _cache = _Read(_id);
                }
                InventoryHash inventoryHash = new InventoryHash(_id);
                if (_cache != null && _cache.inventory != null)
                {
                    inventoryHash.Parse(_cache.strToHash);
                }
                if (_list != null && _list.Length > 0)
                {

                    using (var conn = Connection)
                    {
                        try
                        {

                            trans = conn.BeginTransaction();
                            foreach (ItemList itemList in _list)
                            {
                                ItemSqlBuilder.DeleteItem(_id, itemList.itemID, itemList.storedtype, conn, trans);
                                if (_cache != null && _cache.inventory != null && _cache.inventory.ContainsKey(itemList.itemID))
                                {
                                    Item item = (Item)_cache.inventory[itemList.itemID];
                                    inventoryHash.Remove(item);
                                    _cache.inventory.Remove(itemList.itemID);
                                }
                            }


                            WorkSession.WriteStatus("CharacterSqlAdapter.DeleteItem() : 데이터베이스와 연결합니다");
                            long timestamp = Stopwatch.GetTimestamp();

                            WorkSession.WriteStatus("CharacterSqlAdapter.DeleteItem() : 명령을 수행합니다.");
                            trans.Commit();
                            CommandStatistics.RegisterCommandTime(CommandStatistics.CommandType.cctCharItemDelete, Stopwatch.GetElapsedMilliseconds(timestamp));
                            return true;
                        }
                        catch (SimpleSqlException ex)
                        {
                            ExceptionMonitor.ExceptionRaised(ex, _list);
                            WorkSession.WriteStatus(ex.Message, ex.Number);
                            trans?.Rollback();
                            return false;
                        }
                        catch (Exception ex2)
                        {
                            ExceptionMonitor.ExceptionRaised(ex2, _list);
                            WorkSession.WriteStatus(ex2.Message);
                            trans?.Rollback();
                            return false;
                        }
                        finally
                        {
                            WorkSession.WriteStatus("CharacterSqlAdapter.DeleteItem() : 연결을 종료합니다");
                            trans?.Dispose();
                        }
                    }
                }
                WorkSession.WriteStatus("CharacterSqlAdapter.DeleteItem() : 삭제할 아이템이 없습니다.");
                return true;

            }
            catch (Exception ex3)
            {
                WorkSession.WriteStatus(ex3.Message);
                ExceptionMonitor.ExceptionRaised(ex3);
                return false;
            }
        }

        public bool GetWriteCounter(long _id, out byte _counter)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.GetWriteCounter() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.GetWriteCounter() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                {
                    var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Character);
                    cmd.Where(Mabinogi.SQL.Columns.Character.Id, _id);
                    cmd.Set(Mabinogi.SQL.Columns.Character.WriteCounter, 0);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            _counter = 0;
                            return false;
                        }
                        _counter = reader.GetByte(Mabinogi.SQL.Columns.Character.WriteCounter);
                    }
                    WorkSession.WriteStatus("CharacterSqlAdapter.GetWriteCounter() : 저장 카운터를 읽었습니다.");
                    return true;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _id);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                _counter = 0;
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _id);
                WorkSession.WriteStatus(ex2.Message, _id);
                _counter = 0;
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("CharacterSqlAdapter.GetWriteCounter() : 연결을 종료합니다");
            }
        }

        public uint GetAccumLevel(string _account, string _name)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.GetAccumLevel() : 함수에 진입하였습니다");
            if (_account.Length > 0 && _name.Length > 0)
            {

                try
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.GetAccumLevel() : 데이터베이스와 연결합니다");
                    using (var conn = Connection)
                    {
                        var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Character);
                        cmd.Where(Mabinogi.SQL.Columns.Character.Name, _name);
                        cmd.Set(Mabinogi.SQL.Columns.Character.Level, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Character.CumulatedLevel, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                return 0;

                            return (uint)reader.GetInt32(Mabinogi.SQL.Columns.Character.Level) + (uint)reader.GetInt32(Mabinogi.SQL.Columns.Character.CumulatedLevel);
                        }
                    }
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _account + " / " + _name);
                    WorkSession.WriteStatus(ex.Message, _account + " / " + _name);
                    return 0u;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _account + " / " + _name);
                    WorkSession.WriteStatus(ex2.Message, _account + " / " + _name);
                    return 0u;
                }
                finally
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.GetAccumLevel() : 연결을 종료합니다");
                }
            }
            return 0u;
        }

        public string GetCharName(long id)
        {
            using (var conn = Connection)
                return GetCharName(id, conn);
        }

        public string GetCharName(long id, SimpleConnection conn)
        {
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.Character))
            {
                cmd.Set(Mabinogi.SQL.Columns.Character.Name, 0);
                cmd.Where(Mabinogi.SQL.Columns.Character.Id, id);

                using (var reader = cmd.ExecuteReader())
                    return reader.GetString(Mabinogi.SQL.Columns.Character.Name);
            }
        }

        public bool InsertCharacterMeta(string _account, character_meta_row _meta)
        {
            WorkSession.WriteStatus("CharacterSqlAdapter.InsertCharacterMeta() : 함수에 진입하였습니다");
            if (_meta == null || _account == null)
            {
                return false;
            }
            if (_account.Length == 0 || _meta.charID == 0)
            {
                return false;
            }

            SimpleTransaction trans = null;
            using (var conn = Connection)
            {
                try
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.InsertCharacterMeta() : 데이터베이스와 연결합니다");

                    var dictionary = CDBAPI.GetCharacterMeta(conn, _meta.charID);

                    int num = -1;
                    for (int i = 0; i < 100; i++)
                    {
                        if (!dictionary.ContainsKey(_meta.mcode + "_" + i))
                        {
                            num = i;
                            break;
                        }
                    }
                    if (num < 0)
                    {
                        throw new Exception("CharacterSqlAdapter.InsertCharacterMeta() :: 메타의 인덱스가 99가 넘었습니다.");
                    }
                    _meta.mcode = _meta.mcode + "_" + num;
                    trans = conn.BeginTransaction();
                    CDBAPI.UpdateCharacterMeta(conn, trans, _meta);
                    trans.Commit();
                    return true;

                }
                catch (SimpleSqlException ex)
                {
                    trans?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex, _account + "/" + _meta.charID);
                    WorkSession.WriteStatus(ex.Message, _account + "/" + _meta.charID);
                    return false;
                }
                catch (Exception ex2)
                {
                    trans?.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex2, _account + "/" + _meta.charID);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("CharacterSqlAdapter.InsertCharacterMeta() : 연결을 종료합니다");
                    trans?.Dispose();
                }
            }
        }

        public static void UpdateCharacterTime(long id, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.Character, transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.Character.UpdateTime, DateTime.Now);
                cmd.Where(Mabinogi.SQL.Columns.Character.Id, id);
                cmd.Execute();
            }
        }
    }
}
