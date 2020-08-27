using Mabinogi;
using Mabinogi.SQL;
using System;
using System.Collections;

namespace XMLDB3
{
    public class AccountRefAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.AccountRef;

        public AccountRefAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }
        public bool Create(AccountRef data)
        {
            // PROCEDURE: CreateAccountref2
            using (var conn = this.Connection)
            using (var transaction = conn.BeginTransaction())
            {
                using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.AccountRef, transaction))
                {
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.Id, data.account);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.Flag, data.flag);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.MaxSlot, data.maxslot);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.In, data.In);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.Out, data.Out);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.PlayableTime, data.playabletime);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.SupportRace, data.supportRace);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.SupportRewardState, data.supportRewardState);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.LobbyOption, data.lobbyOption);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.SupportLastChangeTime, data.supportLastChangeTime);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.MacroCheckFailure, data.macroCheckFailure);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.BeginnerFlag, data.beginnerFlag);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.MacroCheckSuccess, data.macroCheckSuccess);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.Ip, "");
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.MachineId, "");

                    cmd.Execute();
                }

                CreateCharacterRefs(data, conn, transaction);
                CreatePetRefs(data, conn, transaction);
                transaction.Commit();
            }
            return true;
        }

        private void CreateCharacterRefs(AccountRef data, SimpleConnection conn, SimpleTransaction transaction)
        {
            foreach (AccountrefCharacter charRef in data.Character)
                CreateCharacterRef(data.account, charRef, conn, transaction);
        }

        public void CreateCharacterRef(string accountName, AccountrefCharacter data, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCharacterRef, transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.AccountCharacterRef.Id, accountName);
                cmd.Set(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterId, data.id);
                cmd.Set(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterName, data.name);
                cmd.Set(Mabinogi.SQL.Columns.AccountCharacterRef.Server, data.server);
                cmd.Set(Mabinogi.SQL.Columns.AccountCharacterRef.Deleted, data.deleted);
                cmd.Set(Mabinogi.SQL.Columns.AccountCharacterRef.GroupId, data.groupID);
                cmd.Set(Mabinogi.SQL.Columns.AccountCharacterRef.Race, data.race);
                cmd.Set(Mabinogi.SQL.Columns.AccountCharacterRef.SupportCharacter, data.supportCharacter);
                cmd.Set(Mabinogi.SQL.Columns.AccountCharacterRef.Tab, data.tab);

                cmd.Execute();
            }
        }

        private void CreatePetRefs(AccountRef data, SimpleConnection conn, SimpleTransaction transaction)
        {
            foreach (AccountrefPet petRef in data.Pet)
                CreatePetRef(data.account, petRef, conn, transaction);
        }

        private void CreatePetRef(string accountName, AccountrefPet data, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.AccountPetRef, transaction))
            {
                cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.Id, accountName);
                cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.PetId, data.id);
                cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.PetName, data.name);
                cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.Server, data.server);
                cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.Deleted, data.deleted);
                cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.RemainTime, data.remaintime);
                cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.LastTime, data.lasttime);
                cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.GroupId, data.groupID);
                cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.Tab, data.tab);
                cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.ExpireTime, data.expiretime);


                cmd.Execute();
            }
        }

        private void GetCharacterRefs(string accountId, ref AccountRef result, SimpleConnection conn)
        {
            using (var mc = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCharacterRef))
            {
                mc.Where(Mabinogi.SQL.Columns.AccountCharacterRef.Id, accountId);

                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var character = new AccountrefCharacter();
                        character.id = reader.GetInt64(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterId);
                        character.name = reader.GetString(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterName);
                        character.server = reader.GetString(Mabinogi.SQL.Columns.AccountCharacterRef.Server);
                        character.deleted = reader.GetInt64(Mabinogi.SQL.Columns.AccountCharacterRef.Deleted);
                        character.groupID = reader.GetByte(Mabinogi.SQL.Columns.AccountCharacterRef.GroupId);
                        character.race = reader.GetByte(Mabinogi.SQL.Columns.AccountCharacterRef.Race);
                        character.supportCharacter = reader.GetBoolean(Mabinogi.SQL.Columns.AccountCharacterRef.SupportCharacter);
                        character.tab = reader.GetBoolean(Mabinogi.SQL.Columns.AccountCharacterRef.Tab);

                        result.Character.Add(character);
                    }
                }
            }
        }

        private void GetPetRefs(string accountId, ref AccountRef result, SimpleConnection conn)
        {
            using (var mc = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.AccountPetRef))
            {
                mc.Where("id", accountId);

                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pet = new AccountrefPet();
                        pet.id = reader.GetInt64(Mabinogi.SQL.Columns.AccountPetRef.PetId);
                        pet.name = reader.GetString(Mabinogi.SQL.Columns.AccountPetRef.PetName);
                        pet.server = reader.GetString(Mabinogi.SQL.Columns.AccountPetRef.Server);
                        pet.deleted = reader.GetInt64(Mabinogi.SQL.Columns.AccountPetRef.Deleted);
                        pet.remaintime = reader.GetInt32(Mabinogi.SQL.Columns.AccountPetRef.RemainTime);
                        pet.lasttime = reader.GetInt64(Mabinogi.SQL.Columns.AccountPetRef.LastTime);
                        pet.groupID = reader.GetByte(Mabinogi.SQL.Columns.AccountPetRef.GroupId);
                        pet.tab = reader.GetBoolean(Mabinogi.SQL.Columns.AccountPetRef.Tab);
                        pet.expiretime = reader.GetInt64(Mabinogi.SQL.Columns.AccountPetRef.ExpireTime);

                        result.Pet.Add(pet);
                    }
                }
            }
        }

        public AccountRef Read(string _id)
        {
            AccountRef result;

            // PROCEDURE: SelectAccountref4
            using (var conn = this.Connection)
            using (var mc = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.AccountRef))
            {
                mc.Where(Mabinogi.SQL.Columns.AccountRef.Id, _id);

                using (var reader = mc.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    result = AccountRef.Build(reader);
                }

                GetCharacterRefs(_id, ref result, conn);
                GetPetRefs(_id, ref result, conn);
            }


            return result;
        }

        public bool SetSlotFlag(string _account, long _id, string _server, long _flag)
        {
            WorkSession.WriteStatus("AccountrefSqlAdapter.SetSlotFlag() : 함수에 진입하였습니다");
            try
            {
                using (var mc = Connection.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCharacterRef))
                {
                    mc.Set(Mabinogi.SQL.Columns.AccountCharacterRef.Deleted, _flag);

                    mc.Where(Mabinogi.SQL.Columns.AccountCharacterRef.Id, _account);
                    mc.Where(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterId, _id);
                    mc.Where(Mabinogi.SQL.Columns.AccountCharacterRef.Server, _server);

                    mc.Execute();
                }
            }
            catch (SimpleSqlException ex)

            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }

            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _account);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            return true;
        }

        public bool SetPetSlotFlag(string _account, long _id, string _server, long _flag)
        {

            try
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetPetSlotFlag() : 함수에 진입하였습니다");



                using (var mc = Connection.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountPetRef))
                {
                    mc.Set(Mabinogi.SQL.Columns.AccountPetRef.Deleted, _flag);

                    mc.Where(Mabinogi.SQL.Columns.AccountPetRef.Id, _account);
                    mc.Where(Mabinogi.SQL.Columns.AccountPetRef.PetId, _id);
                    mc.Where(Mabinogi.SQL.Columns.AccountPetRef.Server, _server);

                    mc.Execute();
                }

                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _account);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetPetSlotFlag() : 연결을 종료합니다");
            }
        }

        public bool PlayIn(string _account, int _remainTime)
        {
            WorkSession.WriteStatus("AccountrefSqlAdapter.PlayIn() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.PlayIn() : 데이터베이스와 연결합니다");
                using (var mc = Connection.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountRef))
                {
                    mc.Set(Mabinogi.SQL.Columns.AccountRef.In, DateTime.Now);
                    mc.Set(Mabinogi.SQL.Columns.AccountRef.PlayableTime, _remainTime);

                    mc.Where(Mabinogi.SQL.Columns.AccountRef.Id, _account);

                    WorkSession.WriteStatus("AccountrefSqlAdapter.PlayIn() : 명령을 실행합니다");
                    mc.Execute();
                }
                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _account);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.PlayIn() : 연결을 종료합니다");
            }
        }

        public bool PlayOut(string _account, int _remainTime, string _server, GroupIDList _charGroupID, GroupIDList _petGroupID, byte _supportRace, byte _supportRewardState, int _supportLastChangeTime, byte _macroCheckFailure, byte _macroCheckSuccess, bool _beginnerFlag)
        {
            WorkSession.WriteStatus("AccountrefSqlAdapter.PlayOut() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.PlayOut() : 데이터베이스와 연결합니다");
                using (var conn = this.Connection)
                using (var transaction = conn.BeginTransaction())
                {

                    var mc = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountRef, transaction);

                    mc.Where(Mabinogi.SQL.Columns.AccountRef.Id, _account);

                    mc.Set(Mabinogi.SQL.Columns.AccountRef.PlayableTime, _remainTime);
                    mc.Set(Mabinogi.SQL.Columns.AccountRef.SupportRace, _supportRace);
                    mc.Set(Mabinogi.SQL.Columns.AccountRef.SupportRewardState, _supportRewardState);
                    mc.Set(Mabinogi.SQL.Columns.AccountRef.SupportLastChangeTime, _supportLastChangeTime);
                    mc.Set(Mabinogi.SQL.Columns.AccountRef.MacroCheckFailure, _macroCheckFailure);
                    mc.Set(Mabinogi.SQL.Columns.AccountRef.BeginnerFlag, _beginnerFlag);
                    mc.Set(Mabinogi.SQL.Columns.AccountRef.MacroCheckSuccess, _macroCheckSuccess);

                    mc.Execute();

                    if (_charGroupID.group != null)
                    {
                        foreach (GroupID groupID in _charGroupID.group)
                        {
                            mc = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCharacterRef, transaction);

                            mc.Set(Mabinogi.SQL.Columns.AccountCharacterRef.GroupId, groupID.groupID);

                            mc.Where(Mabinogi.SQL.Columns.AccountCharacterRef.Id, _account);
                            mc.Where(Mabinogi.SQL.Columns.AccountCharacterRef.Server, _server);
                            mc.Where(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterId, groupID.charID);

                            mc.Execute();
                        }
                    }
                    if (_petGroupID.group != null)
                    {
                        foreach (GroupID groupID in _petGroupID.group)
                        {
                            mc = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountPetRef, transaction);

                            mc.Set(Mabinogi.SQL.Columns.AccountPetRef.GroupId, groupID.groupID);

                            mc.Where(Mabinogi.SQL.Columns.AccountPetRef.Id, _account);
                            mc.Where(Mabinogi.SQL.Columns.AccountPetRef.Server, _server);
                            mc.Where(Mabinogi.SQL.Columns.AccountPetRef.PetId, groupID.charID);

                            mc.Execute();
                        }
                    }
                    WorkSession.WriteStatus("AccountrefSqlAdapter.PlayOut() : 명령을 실행합니다");
                    transaction.Commit();
                    return true;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message, ex.Number);

                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _account);
                WorkSession.WriteStatus(ex2.Message);

                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.PlayOut() : 연결을 종료합니다");
            }
        }

        public bool SetFlag(string _account, int _flag)
        {
            WorkSession.WriteStatus("AccountrefSqlAdapter.SetFlag() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetFlag() : 데이터베이스와 연결합니다");
                using (var cmd = Connection.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountRef))
                {
                    cmd.Where(Mabinogi.SQL.Columns.AccountRef.Id, _account);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.Flag, _flag);

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
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetFlag() : 연결을 종료합니다");
            }

        }

        public bool SetLobbyOption(string _account, int _lobbyOption, LobbyTabList _charLobbyTabList, LobbyTabList _petLobbyTabList)
        {
            WorkSession.WriteStatus("AccountrefSqlAdapter.SetLobbyOption() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetLobbyOption() : 데이터베이스와 연결합니다");
                bool result = true;

                while (result)
                {
                    using (var conn = this.Connection)
                    using (var transaction = conn.BeginTransaction())
                    {

                        using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountRef, transaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.AccountRef.Id, _account);

                            cmd.Set(Mabinogi.SQL.Columns.AccountRef.LobbyOption, _lobbyOption);

                            result = (cmd.Execute() > 0);
                        }

                        if (_charLobbyTabList.tabInfo != null && _charLobbyTabList.tabInfo.Length <= 80)
                        {
                            foreach (LobbyTab lobbyTab in _charLobbyTabList.tabInfo)
                            {
                                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCharacterRef, transaction))
                                {
                                    cmd.Where(Mabinogi.SQL.Columns.AccountCharacterRef.Id, _account);
                                    cmd.Where(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterId, lobbyTab.charID);
                                    cmd.Where(Mabinogi.SQL.Columns.AccountCharacterRef.Server, lobbyTab.server);

                                    cmd.Set(Mabinogi.SQL.Columns.AccountCharacterRef.Tab, lobbyTab.tab);

                                    result = (cmd.Execute() > 0);
                                }
                            }
                        }

                        if (_petLobbyTabList.tabInfo != null && _petLobbyTabList.tabInfo.Length <= 80)
                        {
                            foreach (LobbyTab lobbyTab in _petLobbyTabList.tabInfo)
                            {
                                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountPetRef, transaction))
                                {
                                    cmd.Where(Mabinogi.SQL.Columns.AccountPetRef.Id, _account);
                                    cmd.Where(Mabinogi.SQL.Columns.AccountPetRef.PetId, lobbyTab.charID);
                                    cmd.Where(Mabinogi.SQL.Columns.AccountPetRef.Server, lobbyTab.server);

                                    cmd.Set(Mabinogi.SQL.Columns.AccountPetRef.Tab, lobbyTab.tab);

                                    result = (cmd.Execute() > 0);
                                }
                            }
                        }
                    }
                    WorkSession.WriteStatus("AccountrefSqlAdapter.SetLobbyOption() : 명령을 실행합니다");
                    return result;
                }
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetLobbyOption() : 명령을 실행합니다");
                return result;

            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;

            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _account);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.SetLobbyOption() : 연결을 종료합니다");
            }
        }

        public bool CacheKeyCheck(string _account, int _cacheKey, out int _oldCacheKey)
        {
            WorkSession.WriteStatus("AccountrefSqlAdapter.CacheKeyCheck() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.CacheKeyCheck() : 데이터베이스와 연결합니다");
                _oldCacheKey = 0;

                using (var conn = this.Connection)
                {
                    int rows = 0;

                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCache))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.AccountCache.Account, _account);
                        cmd.Set(Mabinogi.SQL.Columns.AccountCache.Key, _cacheKey);

                        _oldCacheKey = _cacheKey;
                        rows = cmd.Execute();

                        WorkSession.WriteStatus("AccountrefSqlAdapter.CacheKeyCheck() : 명령을 실행합니다");
                    }

                    if (rows == 0)
                    {
                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCache))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.AccountCache.Account, _account);
                            cmd.Set(Mabinogi.SQL.Columns.AccountCache.Key, _cacheKey);


                            cmd.Execute();
                        }
                    }
                }
                return true;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                _oldCacheKey = 0;
                return false;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message, _account);
                ExceptionMonitor.ExceptionRaised(ex2);
                _oldCacheKey = 0;
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.CacheKeyCheck() : 연결을 종료합니다");
            }
        }

        public bool ModifyPawnCoin(string _account, int _modifyCoin, ref int _resultCoin)
        {
            WorkSession.WriteStatus("AccountrefSqlAdapter.ModifyPawnCoin() : 함수에 진입하였습니다");
            try
            {

                try
                {
                    WorkSession.WriteStatus("AccountrefSqlAdapter.ModifyPawnCoin() : 데이터베이스와 연결합니다");
                    bool result = false;
                    using (var conn = this.Connection)
                    {
                        using (var mc = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.AccountPawnCoin))
                        {
                            mc.Where(Mabinogi.SQL.Columns.AccountPawnCoin.IdAccount, _account);

                            using (var reader = mc.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    reader.Close();
                                    return SetPawnCoin(true, _account, _modifyCoin, conn, ref _resultCoin);
                                }
                                else
                                {
                                    _resultCoin = reader.GetInt32(Mabinogi.SQL.Columns.AccountPawnCoin.PawnCoin);
                                    result = true;
                                }
                            }
                            if (_modifyCoin == 0)
                                return result;
                        }

                        return SetPawnCoin(false, _account, _modifyCoin, conn, ref _resultCoin);
                    }
                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("AccountrefSqlAdapter.ModifyPawnCoin() : 연결을 종료합니다");
                }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
        }

        private bool SetPawnCoin(bool create, string _account, int _modifyCoin, SimpleConnection conn, ref int _resultCoin)
        {
            bool result = false;
            if (create)
            {
                using (var mc = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.AccountPawnCoin))
                {
                    mc.Set(Mabinogi.SQL.Columns.AccountPawnCoin.IdAccount, _account);
                    mc.Set(Mabinogi.SQL.Columns.AccountPawnCoin.PawnCoin, _modifyCoin);
                    mc.Set(Mabinogi.SQL.Columns.AccountPawnCoin.UpdateDate, DateTime.Now);

                    WorkSession.WriteStatus("AccountrefSqlAdapter.ModifyPawnCoin() : 명령을 실행합니다");
                    result = (mc.Execute() > 0);
                }
            }
            else
            {
                using (var mc = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountPawnCoin))
                {
                    mc.Where(Mabinogi.SQL.Columns.AccountPawnCoin.IdAccount, _account);

                    mc.Set(Mabinogi.SQL.Columns.AccountPawnCoin.PawnCoin, _modifyCoin);
                    mc.Set(Mabinogi.SQL.Columns.AccountPawnCoin.UpdateDate, DateTime.Now);

                    WorkSession.WriteStatus("AccountrefSqlAdapter.ModifyPawnCoin() : 명령을 실행합니다");
                    result = (mc.Execute() > 0);
                }
            }

            if (result)
                _resultCoin += _modifyCoin;

            return result;
        }

        public bool InsertPawnCoinLog(Message _message)
        {
            return true;
        }

        public bool UpdateAccountMeta(string account, Hashtable _newMetaData)
        {
            WorkSession.WriteStatus("AccountrefSqlAdapter.UpdateAccountMeta() : 함수에 진입하였습니다");
            if (account == null || string.Empty == account)
            {
                return false;
            }
            if (account.Length == 0)
            {
                return false;
            }
            using (var conn = Connection)
            using (var transaction = conn.BeginTransaction())
            {
                // PROCEDURE: UpdateAccountMeta
                try
                {
                    WorkSession.WriteStatus("AccountrefSqlAdapter.UpdateAccountMeta() : 데이터베이스와 연결합니다");

                    Hashtable hashtable = _newMetaData;
                    if (hashtable == null)
                    {
                        hashtable = new Hashtable();
                    }
                    Hashtable hashtable2 = GetAccountMeta(conn, account);
                    if (hashtable2 == null)
                    {
                        hashtable2 = new Hashtable();
                    }

                    foreach (AccountrefMeta value in hashtable.Values)
                    {
                        if (!hashtable2.ContainsKey(value.mcode))
                        {
                            UpdateAccountMeta(conn, transaction, account, value);
                        }
                        else
                        {
                            AccountrefMeta accountrefMeta2 = (AccountrefMeta)hashtable2[value.mcode];
                            if (accountrefMeta2.mdata != value.mdata)
                            {
                                UpdateAccountMeta(conn, transaction, account, value);
                            }
                        }
                    }
                    foreach (string key in hashtable2.Keys)
                    {
                        if (!hashtable.ContainsKey(key))
                        {
                            DeleteAccountMeta(conn, transaction, account, (AccountrefMeta)hashtable2[key]);
                        }
                    }
                    transaction.Commit();

                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, account);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, account);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("AccountrefSqlAdapter.UpdateAccountMeta() : 연결을 종료합니다");
                }
                return true;
            }
        }

        public bool UpdateAccountRefIPMID(string _account, string _connectedIp, string _connectedMachineId)
        {
            WorkSession.WriteStatus("AccountrefSqlAdapter.UpdateAccountRefIPMID() : 함수에 진입하였습니다");

            try
            {
                using (var cmd = Connection.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountRef))
                {
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.Ip, _connectedIp);
                    cmd.Set(Mabinogi.SQL.Columns.AccountRef.MachineId, _connectedMachineId);

                    cmd.Where(Mabinogi.SQL.Columns.AccountRef.Id, _account);
                    cmd.Execute();
                    WorkSession.WriteStatus("AccountrefSqlAdapter.UpdateAccountRefIPMID() : 명령을 실행합니다");


                    return true;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _account);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _account);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.UpdateAccountRefIPMID() : 연결을 종료합니다");
            }
        }

        public bool VerifyMembershipFromBilling(string _account, string _curMonth, ref byte _errorCode, ref int _resultValue)
        {
            WorkSession.WriteStatus("MembershipBillingSqlAdapter.VerifyMembershipFromBilling() : 함수에 진입하였습니다.");

            //TODO: Work this out. I guess it returns how many months or days are left of premium service?
            // Maybe related to AccountActivationAdapter line 42?
            return false;

            /*SqlConnection sqlConnection = new SqlConnection(base.ConnectionString);
            SqlCommand sqlCommand = new SqlCommand("dbo.SelectAccountVip", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "RetVal";
            sqlParameter.SqlDbType = SqlDbType.Int;
            sqlParameter.Direction = ParameterDirection.ReturnValue;
            sqlCommand.Parameters.Add(sqlParameter);
            SqlParameter sqlParameter2 = sqlCommand.Parameters.Add("@setDate", SqlDbType.Char, 6);
            SqlParameter sqlParameter3 = sqlCommand.Parameters.Add("@idAccount", SqlDbType.NVarChar, 50);
            sqlParameter2.Value = _curMonth;
            sqlParameter3.Value = _account;
            try
            {
                WorkSession.WriteStatus("MembershipBillingSqlAdapter.VerifyMembershipFromBilling() : 데이터 베이스에 연결합니다.");
                //using (var cmd = GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.SomethingToDoWithVipServices))
                //{
                //	cmd.AddWhere(Mabinogi.SQL.Columns.SomethingToDoWithVipServices.)
                //}
                _resultValue = 0;
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
                WorkSession.WriteStatus("MembershipBillingSqlAdapter.VerifyMembershipFromBilling() : 데이터 베이스에 연결을 종료합니다.");
            }*/
        }

        public string SelectMabinogiId(string _servername, string _charname)
        {
            WorkSession.WriteStatus("AccountrefSqlAdapter.SelectMabinogiId() : 함수에 진입하였습니다");
            try
            {
                using (var cmd = Connection.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCharacterRef))
                {
                    cmd.Where(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterName, _charname);
                    cmd.Where(Mabinogi.SQL.Columns.AccountCharacterRef.Server, _servername);

                    WorkSession.WriteStatus("AccountrefSqlAdapter.SelectMabinogiId() : 명령을 실행합니다");
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return reader.GetString(Mabinogi.SQL.Columns.AccountCharacterRef.Id);

                    }
                }
                return null;
            }
            catch (SimpleSqlException ex)
            {
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("AccountrefSqlAdapter.SelectMabinogiId() : 연결을 종료합니다");
            }
        }

        public void SetAccountRewardSupportState(string account, byte supportRewardState, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountRef, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.AccountRef.Id, account);
                cmd.Set(Mabinogi.SQL.Columns.AccountRef.SupportRewardState, supportRewardState);

                cmd.Execute();
            }

        }

        public void DeleteAccountrefCharacter(string account, long charID, string server, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCharacterRef, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterId, charID);
                cmd.Where(Mabinogi.SQL.Columns.AccountCharacterRef.Id, account);
                cmd.Where(Mabinogi.SQL.Columns.AccountCharacterRef.Server, server);

                cmd.Execute();
            }
        }

        public void WriteAccountSupportState(string account, byte supportRace, byte supportRewardState, int supportLastChangeTime, SimpleConnection conn, SimpleTransaction transaction)
        {
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountRef, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.AccountRef.Id, account);
                cmd.Where(Mabinogi.SQL.Columns.AccountRef.SupportRace, supportRace);
                cmd.Where(Mabinogi.SQL.Columns.AccountRef.SupportRewardState, supportRewardState);
                cmd.Where(Mabinogi.SQL.Columns.AccountRef.SupportLastChangeTime, supportLastChangeTime);

                cmd.Execute();
            }
        }

        public static string GetAccountIdFromCharId(long charID, SimpleConnection conn)
        {
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.AccountCharacterRef))
            {
                cmd.Where(Mabinogi.SQL.Columns.AccountCharacterRef.CharacterId, charID);
                cmd.Set(Mabinogi.SQL.Columns.AccountCharacterRef.Id, 0);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetString(Mabinogi.SQL.Columns.AccountCharacterRef.Id);
                    }
                }

                return string.Empty;
            }
        }

        private void DeleteAccountMeta(SimpleConnection conn, SimpleTransaction transaction, string account, AccountrefMeta row)
        {
            if (account != null && !(string.Empty == account) && row != null)
            {
                using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.Mabinogi.AccountMeta, transaction))
                {
                    cmd.Where(Mabinogi.SQL.Columns.AccountMeta.Id, account);
                    cmd.Where(Mabinogi.SQL.Columns.AccountMeta.MCode, row.mcode);

                    cmd.Execute();
                }
            }


        }

        private void UpdateAccountMeta(SimpleConnection conn, SimpleTransaction transaction, string account, AccountrefMeta row)
        {
            if (account != null && !(string.Empty == account) && row != null)
            {
                int rows = 0;
                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.Mabinogi.AccountMeta, transaction))
                {
                    cmd.Where(Mabinogi.SQL.Columns.AccountMeta.Id, account);
                    cmd.Where(Mabinogi.SQL.Columns.AccountMeta.MCode, row.mcode);

                    cmd.Set(Mabinogi.SQL.Columns.AccountMeta.MData, row.mdata);

                    rows = cmd.Execute();
                }

                if (rows == 0)
                {
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.AccountMeta, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.AccountMeta.Id, account);
                        cmd.Set(Mabinogi.SQL.Columns.AccountMeta.MCode, row.mcode);
                        cmd.Set(Mabinogi.SQL.Columns.AccountMeta.MData, row.mdata);
                        cmd.Set(Mabinogi.SQL.Columns.AccountMeta.MType, row.mtype);

                        cmd.Execute();
                    }
                }
            }
        }


        private Hashtable GetAccountMeta(SimpleConnection conn, string account)
        {
            if (account == null || string.Empty == account)
            {
                return null;
            }

            Hashtable hashtable = new Hashtable();

            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Mabinogi.AccountMeta))
            {
                cmd.Where(Mabinogi.SQL.Columns.AccountMeta.Id, account);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AccountrefMeta accountrefMeta = new AccountrefMeta();
                        accountrefMeta.mcode = reader.GetString(Mabinogi.SQL.Columns.AccountMeta.MCode);
                        accountrefMeta.mtype = reader.GetString(Mabinogi.SQL.Columns.AccountMeta.MType);
                        accountrefMeta.mdata = reader.GetString(Mabinogi.SQL.Columns.AccountMeta.MData);
                        hashtable.Add(accountrefMeta.mcode, accountrefMeta);
                    }
                    return hashtable;
                }
            }
        }
    }
}
