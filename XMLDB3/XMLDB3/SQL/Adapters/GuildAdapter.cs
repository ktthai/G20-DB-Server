using Mabinogi.SQL;
using System;
using System.Collections.Generic;


namespace XMLDB3
{
    public class GuildAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.MabiGuild.Guild;
        public GuildAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        private GuildIDList _loadGuildList(string _server)
        {
            WorkSession.WriteStatus("GuildSqlAdapter._loadGuildList() : 함수에 진입하였습니다");

            try
            {
                WorkSession.WriteStatus("GuildSqlAdapter._loadGuildList() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                {
                    // PROCEDURE: SelectGuildList3
                    WorkSession.WriteStatus("GuildSqlAdapter._loadGuildList() : 명령을 실행합니다");
                    cmd.Where(Mabinogi.SQL.Columns.Guild.Server, _server);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader == null)
                        {
                            throw new Exception("쿼리된 데이터가 없습니다");
                        }
                        WorkSession.WriteStatus("GuildSqlAdapter._loadGuildList() : 쿼리 결과를 길드리스트로 변환합니다");
                        return ReadGuildIdList(reader);
                    }
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter._loadGuildList() : 연결을 종료합니다");
            }
        }

        public GuildIDList LoadGuildList(string _server, DateTime _overTime)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.LoadGuildList() : 함수에 진입하였습니다");
            try
            {
                if (!(_overTime == DateTime.MinValue))
                {
                    try
                    {
                        using (var conn = Connection)
                        using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                        {
                            // PROCEDURE: SelectGuildListDiffer3
                            cmd.Where(Mabinogi.SQL.Columns.Guild.Server, _server);
                            cmd.Where(Mabinogi.SQL.Columns.Guild.UpdateTime, _overTime);
                            cmd.Set(Mabinogi.SQL.Columns.Guild.Id, 0);

                            using (var reader = cmd.ExecuteReader())
                                return ReadGuildIdList(reader);
                        }
                    }
                    catch (SimpleSqlException ex)
                    {
                        ExceptionMonitor.ExceptionRaised(ex);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return null;
                    }
                    finally
                    {
                    }
                }
                return _loadGuildList(_server);
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
        }

        public GuildIDList LoadDeletedGuildList(string _server, DateTime _overTime)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.LoadDeletedGuildList() : 함수에 진입하였습니다");
            try
            {

                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.GuildDeleted))
                {
                    //PROCEDURE: SelectDeletedGuildListDiffer3
                    cmd.Where(Mabinogi.SQL.Columns.Guild.Server, _server);
                    cmd.Where(Mabinogi.SQL.Columns.Guild.UpdateTime, _overTime);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.Id, 0);

                    using (var reader = cmd.ExecuteReader())
                        return ReadGuildIdList(reader);
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return null;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return null;
            }
            finally
            {
            }
        }

        public bool AddMember(long _id, GuildMember _member, string _joinmsg)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    //PROCEDURE: AddGuildMember
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMember))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.GuildMember.GuildId, _id);
                        cmd.Where(Mabinogi.SQL.Columns.GuildMember.Id, _member.memberid);
                        if (cmd.ExecuteReader().HasRows)
                        {
                            //WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : Already member of this guild");
                            return false;
                        }
                    }
                    int memberMax;
                    int memberCount;
                    long masterId;
                    string serverName;

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _id);

                        cmd.Set(Mabinogi.SQL.Columns.Guild.MaxMember, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.MemberCount, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.Server, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMasterId, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                memberMax = reader.GetInt32(Mabinogi.SQL.Columns.Guild.MaxMember);
                                memberCount = reader.GetInt32(Mabinogi.SQL.Columns.Guild.MemberCount);
                                serverName = reader.GetString(Mabinogi.SQL.Columns.Guild.Server);
                                masterId = reader.GetInt64(Mabinogi.SQL.Columns.Guild.GuildMasterId);
                            }
                            else
                            {
                                WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : Somehow, this guild doesn't exist: " + _id);
                                return false;
                            }
                        }
                    }

                    if (!(memberMax > memberCount))
                        return false;

                    if (!CheckGuildMemberJointime(_member.memberid, serverName, conn))
                        return false;

                    transaction = conn.BeginTransaction();
                    DateTime now = DateTime.Now;
                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMember, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.GuildId, _id);
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.Id, _member.memberid);
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.Name, _member.name);
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.Account, _member.account);
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.Class, _member.@class);
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.Point, _member.point);
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.JoinTime, now);
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.Text, string.Empty);
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.JoinMsg, _joinmsg);

                        WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 명령을 실행합니다");
                        cmd.Execute();
                    }

                    switch (_member.@class)
                    {
                        case 0:
                            {
                                if (masterId == 0)
                                {
                                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                                    {
                                        cmd.Set(Mabinogi.SQL.Columns.Guild.MemberCount, memberCount + 1);
                                        cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, now);
                                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMasterId, _member.memberid);

                                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _id);
                                        cmd.Execute();
                                    }
                                }
                                else
                                {
                                    WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : Guild already has master");
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                            break;
                        case -1:
                            {
                                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                                {
                                    cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, now);

                                    cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _id);
                                    cmd.Execute();
                                }

                            }
                            break;
                        default:
                            {
                                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                                {
                                    cmd.Set(Mabinogi.SQL.Columns.Guild.MemberCount, memberCount + 1);
                                    cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, now);

                                    cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _id);
                                    cmd.Execute();
                                }
                            }
                            break;
                    }

                    WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return true;

                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    if (ex.Number == 50000)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 멤버수 초과로 인해서 실패하였습니다");
                        return false;
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _id, _member);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _id, _member);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.AddMember() : 연결을 종료합니다");
                }
            }
        }

        public bool AddPoint(long _id, int _iAddedPoint)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 함수에 진입하였습니다");



            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    int curPoints;
                    transaction = conn.BeginTransaction();
                    // PROCEDURE: AddGuildPoint
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _id);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() != true)
                            {
                                return false;
                            }

                            curPoints = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildPoint);
                        }
                    }


                    WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 명령을 실행합니다");
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _id);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, curPoints + _iAddedPoint);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, DateTime.Now);

                        cmd.Execute();
                    }
                    WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 트랜잭션을 커밋합니다");
                    transaction.Commit();

                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _id, _iAddedPoint);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _id, _iAddedPoint);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.AddPoint() : 연결을 종료합니다");
                }
            }
        }


        public bool AddMoney(long _id, int _iAddedMoney, SimpleConnection conn, SimpleTransaction transaction)
        {
            int curMoney;
            // PROCEDURE: AddGuildMoney
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _id);
                cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, 0);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read() != true)
                    {
                        return false;
                    }
                    curMoney = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildMoney);
                }
            }
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _id);
                cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, _iAddedMoney + curMoney);
                cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, DateTime.Now);

                WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 명령을 실행합니다");
                cmd.Execute();

            }
            return true;
        }

        public bool AddMoney(long _id, int _iAddedMoney)
        {

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 함수에 진입하였습니다");
            using (var conn = Connection)
            {
                try
                {

                    WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 데이터베이스와 연결합니다");
                    transaction = conn.BeginTransaction();
                    bool result = AddMoney(_id, _iAddedMoney, conn, transaction);
                    if (result)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 트랜잭션을 커밋합니다");
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                    return result;

                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _id, _iAddedMoney);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _id, _iAddedMoney);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.AddMoney() : 연결을 종료합니다");
                }
            }
        }

        public bool SetGuildStone(long _idGuild, GuildStone _guildStone)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 함수에 진입하였습니다");


            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 데이터베이스와 연결합니다");

            using (var conn = Connection)
            {
                try
                {
                    // PROCEDURE: SetGuildStone2
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.Id, 0);
                        if (cmd.ExecuteReader().HasRows != true)
                            return false;
                    }

                    transaction = conn.BeginTransaction();

                    WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 명령을 실행합니다");
                    using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.GuildStone, transaction))
                    {
                        upCmd.Where(Mabinogi.SQL.Columns.GuildStone.GuildId, _idGuild);

                        upCmd.Set(Mabinogi.SQL.Columns.GuildStone.Server, _guildStone.server);
                        upCmd.Set(Mabinogi.SQL.Columns.GuildStone.PositionId, _guildStone.position_id);
                        upCmd.Set(Mabinogi.SQL.Columns.GuildStone.Type, _guildStone.type);
                        upCmd.Set(Mabinogi.SQL.Columns.GuildStone.Region, _guildStone.region);
                        upCmd.Set(Mabinogi.SQL.Columns.GuildStone.X, _guildStone.x);
                        upCmd.Set(Mabinogi.SQL.Columns.GuildStone.Y, _guildStone.y);
                        upCmd.Set(Mabinogi.SQL.Columns.GuildStone.Direction, _guildStone.direction);

                        if (upCmd.Execute() < 1)
                        {
                            using (var inCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiGuild.GuildStone, transaction))
                            {
                                inCmd.Set(Mabinogi.SQL.Columns.GuildStone.GuildId, _idGuild);
                                inCmd.Set(Mabinogi.SQL.Columns.GuildStone.Server, _guildStone.server);
                                inCmd.Set(Mabinogi.SQL.Columns.GuildStone.PositionId, _guildStone.position_id);
                                inCmd.Set(Mabinogi.SQL.Columns.GuildStone.Type, _guildStone.type);
                                inCmd.Set(Mabinogi.SQL.Columns.GuildStone.Region, _guildStone.region);
                                inCmd.Set(Mabinogi.SQL.Columns.GuildStone.X, _guildStone.x);
                                inCmd.Set(Mabinogi.SQL.Columns.GuildStone.Y, _guildStone.y);
                                inCmd.Set(Mabinogi.SQL.Columns.GuildStone.Direction, _guildStone.direction);

                                if (inCmd.Execute() < 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                        }
                    }

                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, DateTime.Now);
                        cmd.Execute();
                    }
                    WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 트랜잭션을 커밋합니다");
                    transaction.Commit();

                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _idGuild, _guildStone);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _idGuild, _guildStone);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.SetGuildStone() : 연결을 종료합니다");
                }
            }
        }

        public bool DeleteGuildStone(long _idGuild)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    // PROCEDURE: DeleteGuildStone
                    transaction = conn.BeginTransaction();
                    using (var delCmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.MabiGuild.GuildStone, transaction))
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 명령을 실행합니다");
                        delCmd.Where(Mabinogi.SQL.Columns.GuildStone.GuildId, _idGuild);

                        if (delCmd.Execute() > 0)
                        {
                            using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                            {
                                upCmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                                upCmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, DateTime.Now);
                                upCmd.Execute();
                            }
                        }
                        else
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }



                    WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 트랜잭션을 커밋합니다");
                    transaction.Commit();

                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _idGuild);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _idGuild);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildStone() : 연결을 종료합니다");
                }
            }
        }

        public bool TransferGuildMaster(long _idGuild, long _idOldMaster, long _idNewMaster)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 함수에 진입하였습니다");

            if (_idNewMaster == _idOldMaster)
                return false;

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    // PROCEDURE: TransferGuildMaster
                    string newMasterAcc = AccountRefAdapter.GetAccountIdFromCharId(_idNewMaster, conn);
                    DateTime storageExpiration;
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Where(Mabinogi.SQL.Columns.Guild.GuildMasterId, _idOldMaster);
                        // original procedure got this value, for some unknown reason
                        //cmd.Set(Mabinogi.SQL.Columns.Guild.Server, 0);

                        // We don't check this? I guess this is checked elsewhere..?
                        //cmd.Set(Mabinogi.SQL.Columns.Guild.MasterChangeTime, 0);

                        if (cmd.ExecuteReader().HasRows != true)
                            // Either guild doesn't exist, or specified ID isn't leader
                            return false;
                    }
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.Shop.FantasyLifeClub))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.FantasyLifeClub.Id, newMasterAcc);
                        cmd.Set(Mabinogi.SQL.Columns.FantasyLifeClub.StorageExpiration, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                // Apparently we don't check to see if the storage service is even valid before just assigning leader later.
                                storageExpiration = reader.GetDateTime(Mabinogi.SQL.Columns.FantasyLifeClub.StorageExpiration);
                            else
                                return false;
                        }
                    }
                    transaction = conn.BeginTransaction();
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMember, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.Class, 5);
                        cmd.Where(Mabinogi.SQL.Columns.GuildMember.GuildId, _idGuild);
                        cmd.Where(Mabinogi.SQL.Columns.GuildMember.Id, _idOldMaster);
                        cmd.Where(Mabinogi.SQL.Columns.GuildMember.Class, 0);
                        if (cmd.Execute() < 1)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMember, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.Class, 0);
                        cmd.Where(Mabinogi.SQL.Columns.GuildMember.GuildId, _idGuild);
                        cmd.Where(Mabinogi.SQL.Columns.GuildMember.Id, _idNewMaster);
                        cmd.Where(Mabinogi.SQL.Columns.GuildMember.Class, 5);
                        if (cmd.Execute() < 1)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.Guild.Expiration, storageExpiration);
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Where(Mabinogi.SQL.Columns.Guild.GuildMasterId, _idNewMaster);
                        cmd.Where(Mabinogi.SQL.Columns.Guild.MasterChangeTime, DateTime.Now);
                        if (cmd.Execute() < 1)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 명령을 실행합니다");

                    WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 트랜잭션을 커밋합니다");
                    transaction.Commit();


                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _idGuild, _idOldMaster);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _idGuild, _idOldMaster);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.TransferGuildMaster() : 연결을 종료합니다");
                }
            }
        }



        public DateTime GetDBCurrentTime()
        {
            WorkSession.WriteStatus("GuildSqlAdapter.GetDBCurrentTime() : 함수에 진입하였습니다");

            WorkSession.WriteStatus("GuildSqlAdapter.GetDBCurrentTime() : 데이터베이스와 연결합니다");

            try
            {
                return DateTime.Now;
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return DateTime.MinValue;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                WorkSession.WriteStatus(ex2.Message);
                return DateTime.MinValue;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.GetDBCurrentTime() : 연결을 종료합니다");
            }
        }

        public void RemoveGuildMemberEx(long charID, string serverName, SimpleConnection conn, SimpleTransaction trans)
        {
            using (var chkCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMember))
            {
                chkCmd.Where(Mabinogi.SQL.Columns.GuildMember.Id, charID);

                long guildID;
                using (var reader = chkCmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return;

                    guildID = reader.GetInt64(Mabinogi.SQL.Columns.GuildMember.GuildId);
                }

                using (var guildCmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                {
                    guildCmd.Where(Mabinogi.SQL.Columns.Guild.Server, serverName);
                    guildCmd.Where(Mabinogi.SQL.Columns.Guild.Id, guildID);

                    if (!guildCmd.ExecuteReader().HasRows)
                        return;
                }
                RemoveGuildMember(guildID, charID, conn, trans);
            }
        }

        public void RemoveGuildMember(long guildID, long charID, SimpleConnection conn, SimpleTransaction trans)
        {
            var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMember);
            cmd.Where(Mabinogi.SQL.Columns.GuildMember.GuildId, guildID);
            cmd.Where(Mabinogi.SQL.Columns.GuildMember.Id, charID);

            DateTime joinTime;
            int memberClass;
            string name, account;
            using (var reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                    throw new SimpleSqlException(string.Format("Member {0} doesn't exist in guild {1}", guildID, charID));

                memberClass = reader.GetInt32(Mabinogi.SQL.Columns.GuildMember.Class);
                joinTime = reader.GetDateTime(Mabinogi.SQL.Columns.GuildMember.JoinTime);
                name = reader.GetString(Mabinogi.SQL.Columns.GuildMember.Name);
                account = reader.GetString(Mabinogi.SQL.Columns.GuildMember.Account);
            }


            cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild);
            cmd.Where(Mabinogi.SQL.Columns.Guild.Id, guildID);

            using (var guildReader = cmd.ExecuteReader())
            {
                var guildServer = guildReader.GetString(Mabinogi.SQL.Columns.Guild.Server);


                if (memberClass >= 0)
                {
                    cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.AwayGuildMember);
                    cmd.Where(Mabinogi.SQL.Columns.AwayGuildMember.MemberId, charID);
                    cmd.Where(Mabinogi.SQL.Columns.AwayGuildMember.Server, guildServer);

                    // TODO: remove these useless checks
                    if (cmd.ExecuteReader().HasRows)
                    {
                        cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.AwayGuildMember, trans);
                        cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.GuildId, guildID);
                        cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.JoinTime, joinTime);
                        cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.AwayTime, DateTime.Now);

                        cmd.Where(Mabinogi.SQL.Columns.AwayGuildMember.MemberId, charID);
                        cmd.Where(Mabinogi.SQL.Columns.AwayGuildMember.Server, guildServer);

                        cmd.Execute();
                    }
                    else
                    {
                        cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiGuild.AwayGuildMember, trans);

                        cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.MemberId, charID);
                        cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.JoinTime, joinTime);
                        cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.Name, name);

                        cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.Account, account);
                        cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.GuildId, guildID);
                        cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.Server, guildServer);
                    }
                }

                cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMember, trans);
                cmd.Where(Mabinogi.SQL.Columns.GuildMember.Id, charID);
                cmd.Where(Mabinogi.SQL.Columns.GuildMember.GuildId, guildID);
                cmd.Execute();


                if (guildReader.GetInt64(Mabinogi.SQL.Columns.Guild.GuildMasterId) == charID)
                {
                    cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiGuild.GuildDeleted, trans);
                    cmd.Set(Mabinogi.SQL.Columns.GuildDeleted.Id, guildID);
                    cmd.Set(Mabinogi.SQL.Columns.GuildDeleted.Name, guildReader.GetString(Mabinogi.SQL.Columns.Guild.Name));
                    cmd.Set(Mabinogi.SQL.Columns.GuildDeleted.Server, guildServer);
                    cmd.Set(Mabinogi.SQL.Columns.GuildDeleted.DeleteTime, DateTime.Now);

                    cmd.Execute();

                    cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, trans);
                    cmd.Where(Mabinogi.SQL.Columns.Guild.Id, guildID);
                    cmd.Execute();
                }
                else if (memberClass == -1)
                {
                    cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, trans);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, DateTime.Now);
                    cmd.Where(Mabinogi.SQL.Columns.Guild.Id, guildID);
                }
                else
                {
                    cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, trans);
                    // verify this
                    cmd.Set(Mabinogi.SQL.Columns.Guild.MemberCount, guildReader.GetInt32(Mabinogi.SQL.Columns.Guild.MemberCount) - 1);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, DateTime.Now);
                    cmd.Where(Mabinogi.SQL.Columns.Guild.Id, guildID);
                }
            }

        }

        public bool IsUsableName(string _name)
        {

            try
            {
                WorkSession.WriteStatus("GuildSqlAdapter.IsUsableName() : 함수에 진입하였습니다");
                WorkSession.WriteStatus("GuildSqlAdapter.IsUsableName() : 데이터베이스와 연결합니다");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                {
                    cmd.Where(Mabinogi.SQL.Columns.Guild.Name, _name);
                    WorkSession.WriteStatus("GuildSqlAdapter.IsUsableName() : DataSet 을 채웁니다");
                    return !cmd.ExecuteReader().HasRows;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, _name);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _name);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.IsUsableName() : 연결을 종료합니다");
            }
        }

        public bool CheckMemberJointime(long _idMember, string _server)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.CheckMemberJointime() : 함수에 진입하였습니다");
            WorkSession.WriteStatus("GuildSqlAdapter.CheckMemberJointime() : 데이터베이스와 연결합니다");
            try
            {
                using (var conn = Connection)
                    return GuildAdapter.CheckGuildMemberJointime(_idMember, _server, conn);
            }
            catch (SimpleSqlException ex)
            {
                if (ex.Number >= 50000)
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.CheckMemberJointime() : 가입 불가능합니다.");
                    return false;
                }
                ExceptionMonitor.ExceptionRaised(ex, _idMember, _server);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, _idMember, _server);
                WorkSession.WriteStatus(ex2.Message);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("GuildSqlAdapter.CheckMemberJointime() : 연결을 종료합니다");
            }
        }

        public bool Create(Guild _data)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.Create() : 함수에 진입하였습니다");
            try
            {
                WorkSession.WriteStatus("GuildSqlAdapter.Write() : SQL 명령문을 생성합니다");


                SimpleTransaction transaction = null;
                WorkSession.WriteStatus("GuildSqlAdapter.Create() : 데이터 베이스에 연결합니다.");
                using (var conn = Connection)
                {
                    try
                    {

                        transaction = conn.BeginTransaction();

                        WorkSession.WriteStatus("GuildSqlAdapter.Write() : 명령을 실행합니다");
                        GuildCreateBuilder.Build(_data, conn, transaction);


                        WorkSession.WriteStatus("GuildSqlAdapter.Write() : 트랜잭션을 커밋합니다.");
                        transaction.Commit();
                    }
                    catch (SimpleSqlException ex)
                    {
                        if (transaction != null)
                        {
                            WorkSession.WriteStatus("GuildSqlAdapter.Create() : 트랜잭션을 롤백합니다");
                            transaction.Rollback();
                        }
                        ExceptionMonitor.ExceptionRaised(ex, _data);
                        WorkSession.WriteStatus(ex.Message, ex.Number);
                        return false;
                    }
                    catch (Exception ex2)
                    {
                        if (transaction != null)
                        {
                            WorkSession.WriteStatus("GuildSqlAdapter.Create() : 트랜잭션을 롤백합니다");
                            transaction.Rollback();
                        }
                        ExceptionMonitor.ExceptionRaised(ex2, _data);
                        WorkSession.WriteStatus(ex2.Message);
                        return false;
                    }
                    finally
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.Create() : 연결을 종료합니다");
                    }
                    return true;
                }
            }
            catch (Exception ex3)
            {
                ExceptionMonitor.ExceptionRaised(ex3, _data);
                WorkSession.WriteStatus(ex3.Message);
                return false;
            }
        }

        public Guild Read(long _id)
        {
            Guild guild = null;

            using (var conn = Connection)
            {
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                {
                    cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _id);


                    cmd.Set(Mabinogi.SQL.Columns.Guild.Name, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.Server, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.GuildType, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.JoinType, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.MaxMember, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.DrawableMoney, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.DrawableDate, 0);

                    cmd.Set(Mabinogi.SQL.Columns.Guild.Expiration, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.Enable, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.BattleGroundType, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.BattleGroundWinnerType, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.GuildStatusFlag, 0);
                    cmd.Set(Mabinogi.SQL.Columns.Guild.GuildTitle, 0);

                    using (var reader = cmd.ExecuteReader())
                    {

                        if (reader.Read() != true)
                            return guild;

                        guild = new Guild();
                        guild.id = _id;
                        guild.name = reader.GetString(Mabinogi.SQL.Columns.Guild.Name);
                        guild.server = reader.GetString(Mabinogi.SQL.Columns.Guild.Server);
                        guild.guildpoint = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildPoint);
                        guild.guildmoney = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildMoney);
                        guild.guildtype = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildType);
                        guild.jointype = reader.GetInt32(Mabinogi.SQL.Columns.Guild.JoinType);
                        guild.maxmember = reader.GetInt32(Mabinogi.SQL.Columns.Guild.MaxMember);
                        guild.drawablemoney = reader.GetInt32(Mabinogi.SQL.Columns.Guild.DrawableMoney);
                        guild.drawabledate = reader.GetDateTime(Mabinogi.SQL.Columns.Guild.DrawableDate);

                        guild.expiration = reader.GetDateTime(Mabinogi.SQL.Columns.Guild.Expiration);
                        guild.enable = reader.GetByte(Mabinogi.SQL.Columns.Guild.Enable);
                        guild.battlegroundtype = reader.GetByte(Mabinogi.SQL.Columns.Guild.BattleGroundType);
                        guild.BattleGroundWinnerType = reader.GetByte(Mabinogi.SQL.Columns.Guild.BattleGroundWinnerType);
                        guild.guildstatusflag = reader.GetByte(Mabinogi.SQL.Columns.Guild.GuildStatusFlag);
                        guild.guildtitle = reader.GetString(Mabinogi.SQL.Columns.Guild.GuildTitle);
                    }
                }
                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.GuildText))
                {
                    cmd.Where(Mabinogi.SQL.Columns.GuildText.GuildId, _id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            guild.profile = reader.GetString(Mabinogi.SQL.Columns.GuildText.Profile);
                            guild.greeting = reader.GetString(Mabinogi.SQL.Columns.GuildText.Greeting);
                            guild.leaving = reader.GetString(Mabinogi.SQL.Columns.GuildText.Leaving);
                            guild.refuse = reader.GetString(Mabinogi.SQL.Columns.GuildText.Refuse);
                        }
                    }
                }

                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMember))
                {
                    cmd.Where(Mabinogi.SQL.Columns.GuildMember.GuildId, _id);

                    cmd.Set(Mabinogi.SQL.Columns.GuildMember.Id, 0);
                    cmd.Set(Mabinogi.SQL.Columns.GuildMember.Name, 0);
                    cmd.Set(Mabinogi.SQL.Columns.GuildMember.Account, 0);
                    cmd.Set(Mabinogi.SQL.Columns.GuildMember.Class, 0);
                    cmd.Set(Mabinogi.SQL.Columns.GuildMember.Point, 0);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<GuildMember> memberList = new List<GuildMember>();
                            GuildMember member;

                            while (reader.Read())
                            {
                                member = new GuildMember();
                                member.memberid = reader.GetInt64(Mabinogi.SQL.Columns.GuildMember.Id);
                                member.name = reader.GetString(Mabinogi.SQL.Columns.GuildMember.Name);
                                member.account = reader.GetString(Mabinogi.SQL.Columns.GuildMember.Account);
                                member.@class = reader.GetInt32(Mabinogi.SQL.Columns.GuildMember.Class);
                                member.point = reader.GetInt32(Mabinogi.SQL.Columns.GuildMember.Point);
                                memberList.Add(member);
                            }

                            guild.members = memberList;
                        }
                    }
                }

                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.GuildStone))
                {
                    cmd.Where(Mabinogi.SQL.Columns.GuildStone.GuildId, _id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            guild.stone = new GuildStone();
                            guild.stone.region = reader.GetInt16(Mabinogi.SQL.Columns.GuildStone.Region);
                            guild.stone.server = reader.GetString(Mabinogi.SQL.Columns.GuildStone.Server);
                            guild.stone.position_id = reader.GetInt64(Mabinogi.SQL.Columns.GuildStone.PositionId);
                            guild.stone.type = reader.GetInt32(Mabinogi.SQL.Columns.GuildStone.Type);
                            guild.stone.x = reader.GetInt32(Mabinogi.SQL.Columns.GuildStone.X);
                            guild.stone.y = reader.GetInt32(Mabinogi.SQL.Columns.GuildStone.Y);
                            guild.stone.direction = reader.GetFloat(Mabinogi.SQL.Columns.GuildStone.Direction);
                        }
                    }
                }

                using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.GuildRobe))
                {
                    cmd.Where(Mabinogi.SQL.Columns.GuildRobe.GuildId, _id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            guild.robe = new GuildRobe();
                            guild.robe.emblemChestIcon = reader.GetByte(Mabinogi.SQL.Columns.GuildRobe.EmblemChestIcon);
                            guild.robe.emblemChestDeco = reader.GetByte(Mabinogi.SQL.Columns.GuildRobe.EmblemChestDeco);
                            guild.robe.emblemBeltDeco = reader.GetByte(Mabinogi.SQL.Columns.GuildRobe.EmblemBeltDeco);
                            guild.robe.color1 = reader.GetInt32(Mabinogi.SQL.Columns.GuildRobe.Color1);
                            guild.robe.color2Index = reader.GetByte(Mabinogi.SQL.Columns.GuildRobe.Color2Index);
                            guild.robe.color3Index = reader.GetByte(Mabinogi.SQL.Columns.GuildRobe.Color3Index);
                            guild.robe.color4Index = reader.GetByte(Mabinogi.SQL.Columns.GuildRobe.Color4Index);
                            guild.robe.color5Index = reader.GetByte(Mabinogi.SQL.Columns.GuildRobe.Color5Index);
                        }
                    }
                }
            }
            return guild;
        }

        public bool Delete(long _id)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    // PROCEDURE: DeleteGuild
                    string server;
                    string name;
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _id);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.Server, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.Name, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() != true)
                                return false;
                            server = reader.GetString(Mabinogi.SQL.Columns.Guild.Server);
                            name = reader.GetString(Mabinogi.SQL.Columns.Guild.Name);
                        }
                    }

                    List<long> leftMemberIDs = new List<long>();

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.AwayGuildMember))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.AwayGuildMember.GuildId, _id);
                        cmd.Where(Mabinogi.SQL.Columns.AwayGuildMember.Server, server);
                        cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.MemberId, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                leftMemberIDs.Add(reader.GetInt64(Mabinogi.SQL.Columns.AwayGuildMember.MemberId));
                            }
                        }

                    }

                    transaction = conn.BeginTransaction();

                    // TODO: take another go at this

                    foreach (long memberId in leftMemberIDs)
                    {
                        using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMember, transaction))
                        {
                            cmd.Where(Mabinogi.SQL.Columns.GuildMember.GuildId, _id);
                            cmd.Where(Mabinogi.SQL.Columns.GuildMember.Id, memberId);
                            cmd.Execute();
                        }
                    }


                    List<GuildMember> memberList = new List<GuildMember>();
                    List<DateTime> joinTimes = new List<DateTime>();

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.GuildMember, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.GuildMember.GuildId, _id);

                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.Id, 0);
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.Name, 0);
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.Account, 0);
                        cmd.Set(Mabinogi.SQL.Columns.GuildMember.JoinTime, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            GuildMember member;

                            while (reader.Read())
                            {
                                member = new GuildMember();
                                member.memberid = reader.GetInt64(Mabinogi.SQL.Columns.GuildMember.Id);
                                member.name = reader.GetString(Mabinogi.SQL.Columns.GuildMember.Name);
                                member.account = reader.GetString(Mabinogi.SQL.Columns.GuildMember.Account);

                                joinTimes.Add(reader.GetDateTime(Mabinogi.SQL.Columns.GuildMember.JoinTime));

                                memberList.Add(member);
                            }
                        }
                    }

                    DateTime now = DateTime.Now;
                    for (int i = 0; i < memberList.Count; i++)
                    {
                        using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiGuild.AwayGuildMember, transaction))
                        {
                            cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.GuildId, _id);
                            cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.Server, server);
                            cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.MemberId, memberList[i].memberid);
                            cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.Name, memberList[i].name);
                            cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.Account, memberList[i].account);
                            cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.JoinTime, joinTimes[i]);
                            cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.AwayTime, now);
                            cmd.Execute();
                        }
                    }


                    using (var cmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiGuild.GuildDeleted, transaction))
                    {
                        cmd.Set(Mabinogi.SQL.Columns.GuildDeleted.Id, _id);
                        cmd.Set(Mabinogi.SQL.Columns.GuildDeleted.Server, server);
                        cmd.Set(Mabinogi.SQL.Columns.GuildDeleted.Name, name);
                        cmd.Set(Mabinogi.SQL.Columns.GuildDeleted.DeleteTime, DateTime.Now);
                        cmd.Execute();
                    }



                    WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 명령을 실행합니다");
                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _id);
                        cmd.Execute();
                    }



                    WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 트랜잭션을 커밋합니다");
                    transaction.Commit();
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _id);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _id);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.Delete() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT WithdrawDrawableMoney(long _idGuild, int _money, out int _remainMoney, out int _remainDrawableMoney)
        {


            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.WithdrawDrawableMoney() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    // PROCEDURE: WithdrawDrawableMoney
                    WorkSession.WriteStatus("GuildSqlAdapter.WithdrawDrawableMoney() : 함수에 진입하였습니다");

                    transaction = conn.BeginTransaction();
                    if (WithdrawMoney(_idGuild, _money, out _remainMoney, conn, transaction) && WithdrawDrawableMoney(_idGuild, _money, out _remainDrawableMoney, conn, transaction))
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.WithdrawDrawableMoney() : 명령을 실행합니다");
                        transaction.Commit();
                        return REPLY_RESULT.SUCCESS;
                    }


                    _remainDrawableMoney = _remainMoney = 0;
                    transaction.Rollback();
                    return REPLY_RESULT.FAIL;
                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.WithdrawDrawableMoney() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _idGuild, _money);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    _remainDrawableMoney = 0;
                    _remainMoney = 0;
                    return REPLY_RESULT.ERROR;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.WithdrawDrawableMoney() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _idGuild, _money);
                    WorkSession.WriteStatus(ex2.Message);
                    _remainDrawableMoney = 0;
                    _remainMoney = 0;
                    return REPLY_RESULT.ERROR;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.WithdrawDrawableMoney() : 연결을 종료합니다");
                }
            }
        }

        public int ChangeGuildStone(long _idGuild, int _iType, int _iGold, int _iGP)
        {

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 함수에 진입하였습니다");

                    // PROCEDURE: ChangeGuildStone
                    int curGp, curMoney;
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, 0);

                        using (var reader = cmd.ExecuteReader())
                        {

                            if (reader.Read() != true)
                                return -1;

                            curMoney = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildMoney);
                            curGp = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildPoint);
                        }
                    }

                    if (curMoney < _iGold)
                        return 1;
                    if (curGp < _iGP)
                        return 2;

                    transaction = conn.BeginTransaction();

                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, curMoney - _iGold);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, curGp - _iGP);
                        cmd.Execute();
                    }

                    WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 명령을 실행합니다");

                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.GuildStone, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.GuildStone.GuildId, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.GuildStone.Type, _iType);
                        cmd.Execute();
                    }

                    WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 트랜잭션을 커밋합니다");
                    transaction.Commit();

                    return 0;
                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _idGuild);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return -1;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _idGuild);
                    WorkSession.WriteStatus(ex2.Message);
                    return -1;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.ChangeGuildStone() : 연결을 종료합니다");
                }
            }
        }

        public bool UpdateGuildProperties(long _idGuild, int _iGP, int _iMoney, int _iLevel)
        {
            SimpleTransaction transaction = null;

            WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildProperties() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildProperties() : 함수에 진입하였습니다");

                    // PROCEDURE: UpdateGuild
                    int result = 0;
                    int maxMembers, guildAbility, guildPoints, guildMoney;

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.MaxMember, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildAbility, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() != true)
                                return false;

                            maxMembers = reader.GetInt32(Mabinogi.SQL.Columns.Guild.MaxMember);
                            guildAbility = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildAbility);
                            guildPoints = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildPoint);
                            guildMoney = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildMoney);
                        }
                    }


                    transaction = conn.BeginTransaction();
                    switch (_iLevel)
                    {
                        case 0:
                            {
                                using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                                {
                                    cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                                    cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, _iGP + guildPoints);
                                    cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, _iMoney + guildMoney);
                                    cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, DateTime.Now);
                                    result = cmd.Execute();
                                }
                            }
                            break;
                        case 1:
                            {
                                {
                                    switch (maxMembers)
                                    {
                                        case 5:
                                            {
                                                guildAbility += GuildFlagBit.BasicUpkeep;
                                                maxMembers = 10;
                                            }
                                            break;
                                        case 10:
                                            {
                                                guildAbility += GuildFlagBit.AdvUpkeep;
                                                maxMembers = 20;
                                            }
                                            break;
                                        case 20:
                                            {
                                                guildAbility += GuildFlagBit.GreatUpkeep;
                                                maxMembers = 50;
                                            }
                                            break;
                                        case 50:
                                            {
                                                guildAbility += GuildFlagBit.GrandUpkeep;
                                                maxMembers = 250;
                                            }
                                            break;
                                    }

                                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                                    {
                                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                                        cmd.Set(Mabinogi.SQL.Columns.Guild.MaxMember, maxMembers);
                                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildAbility, guildAbility);
                                        cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, DateTime.Now);
                                        result = cmd.Execute();
                                    }
                                }
                            }
                            break;
                        case -1:
                            {
                                {
                                    switch (maxMembers)
                                    {
                                        case 10:
                                            {
                                                guildAbility -= GuildFlagBit.BasicUpkeep;
                                                maxMembers = 5;
                                            }
                                            break;
                                        case 20:
                                            {
                                                guildAbility -= GuildFlagBit.AdvUpkeep;
                                                maxMembers = 10;
                                            }
                                            break;
                                        case 50:
                                            {
                                                guildAbility -= GuildFlagBit.GreatUpkeep;
                                                maxMembers = 20;
                                            }
                                            break;
                                        case 250:
                                            {
                                                guildAbility -= GuildFlagBit.GrandUpkeep;
                                                maxMembers = 50;
                                            }
                                            break;
                                    }

                                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                                    {
                                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);

                                        cmd.Set(Mabinogi.SQL.Columns.Guild.MaxMember, maxMembers);
                                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildAbility, guildAbility);
                                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, _iGP);
                                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, _iMoney);
                                        cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, DateTime.Now);

                                        result = cmd.Execute();
                                    }
                                }
                            }
                            break;
                    }


                    if (result > 0)
                    {
                        transaction.Commit();
                        return true;
                    }
                    transaction?.Rollback();
                    return false;

                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildProperties() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _idGuild);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildProperties() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _idGuild);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildProperties() : 연결을 종료합니다");
                }
            }
        }

        public REPLY_RESULT UpdateGuildRobe(long _idGuild, int _guildPoint, int _guildMoney, GuildRobe _guildRobe, out byte _errorCode)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 함수에 진입하였습니다");
            _errorCode = 0;

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    int curGp, curMoney;
                    // PROCEDURE: UpdateGuildRobe

                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() == false)
                            {
                                _errorCode = 1;
                                return REPLY_RESULT.FAIL;
                            }

                            curGp = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildPoint);
                            curMoney = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildMoney);
                        }
                    }

                    if (curGp < _guildPoint)
                    {
                        _errorCode = 1;
                        return REPLY_RESULT.FAIL;
                    }

                    if (curMoney < _guildMoney)
                    {
                        _errorCode = 2;
                        return REPLY_RESULT.FAIL;
                    }

                    transaction = conn.BeginTransaction();

                    using (var upCmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.GuildRobe, transaction))
                    {
                        upCmd.Where(Mabinogi.SQL.Columns.GuildRobe.GuildId, _idGuild);

                        upCmd.Set(Mabinogi.SQL.Columns.GuildRobe.EmblemChestIcon, _guildRobe.emblemChestIcon);
                        upCmd.Set(Mabinogi.SQL.Columns.GuildRobe.EmblemChestDeco, _guildRobe.emblemChestDeco);
                        upCmd.Set(Mabinogi.SQL.Columns.GuildRobe.EmblemBeltDeco, _guildRobe.emblemBeltDeco);
                        upCmd.Set(Mabinogi.SQL.Columns.GuildRobe.Color1, _guildRobe.color1);
                        upCmd.Set(Mabinogi.SQL.Columns.GuildRobe.Color2Index, _guildRobe.color2Index);
                        upCmd.Set(Mabinogi.SQL.Columns.GuildRobe.Color3Index, _guildRobe.color3Index);
                        upCmd.Set(Mabinogi.SQL.Columns.GuildRobe.Color4Index, _guildRobe.color4Index);
                        upCmd.Set(Mabinogi.SQL.Columns.GuildRobe.Color5Index, _guildRobe.color5Index);

                        if (upCmd.Execute() < 1)
                        {
                            using (var insertCmd = conn.GetDefaultInsertCommand(Mabinogi.SQL.Tables.MabiGuild.GuildRobe, transaction))
                            {
                                insertCmd.Set(Mabinogi.SQL.Columns.GuildRobe.GuildId, _idGuild);
                                insertCmd.Set(Mabinogi.SQL.Columns.GuildRobe.EmblemChestIcon, _guildRobe.emblemChestIcon);
                                insertCmd.Set(Mabinogi.SQL.Columns.GuildRobe.EmblemChestDeco, _guildRobe.emblemChestDeco);
                                insertCmd.Set(Mabinogi.SQL.Columns.GuildRobe.EmblemBeltDeco, _guildRobe.emblemBeltDeco);
                                insertCmd.Set(Mabinogi.SQL.Columns.GuildRobe.Color1, _guildRobe.color1);
                                insertCmd.Set(Mabinogi.SQL.Columns.GuildRobe.Color2Index, _guildRobe.color2Index);
                                insertCmd.Set(Mabinogi.SQL.Columns.GuildRobe.Color3Index, _guildRobe.color3Index);
                                insertCmd.Set(Mabinogi.SQL.Columns.GuildRobe.Color4Index, _guildRobe.color4Index);
                                insertCmd.Set(Mabinogi.SQL.Columns.GuildRobe.Color5Index, _guildRobe.color5Index);

                                insertCmd.Execute();
                            }
                        }
                    }

                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 명령을 실행합니다.");
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, curGp - _guildPoint);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, curMoney - _guildMoney);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, DateTime.Now);
                        cmd.Execute();
                    }


                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 트랜잭션을 커밋합니다.");
                    transaction.Commit();
                    return REPLY_RESULT.SUCCESS;

                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _idGuild, _guildRobe);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return REPLY_RESULT.ERROR;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _idGuild, _guildRobe);
                    WorkSession.WriteStatus(ex2.Message);
                    return REPLY_RESULT.ERROR;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildRobe() : 연결을 종료합니다");
                }
            }
        }

        public bool DeleteGuildRobe(long _idGuild)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    // PROCEDURE: DeleteGuildRobe
                    transaction = conn.BeginTransaction();
                    using (var cmd = conn.GetDefaultDeleteCommand(Mabinogi.SQL.Tables.MabiGuild.GuildRobe, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.GuildRobe.GuildId, _idGuild);

                        if (cmd.Execute() < 1)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 명령을 실행합니다.");
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, DateTime.Now);
                        cmd.Execute();
                    }
                    WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 트랜잭션을 커밋합니다.");
                    transaction.Commit();
                    return true;

                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _idGuild);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _idGuild);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.DeleteGuildRobe() : 연결을 종료합니다");
                }
            }
        }

        public int UpdateBattleGroundType(long _idGuild, int _guildPoint, int _guildMoney, byte _battleGroundType)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 함수에 진입하였습니다");
            SimpleTransaction transaction = null;

            WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {


                    // PROCEDURE: UpdateGuildBattleGroundType
                    int curGp, curMoney;
                    byte curBattleType;
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.BattleGroundType, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() == false)
                            {
                                return 1;
                            }

                            curGp = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildPoint);
                            curMoney = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildMoney);
                            curBattleType = reader.GetByte(Mabinogi.SQL.Columns.Guild.BattleGroundType);
                        }
                    }

                    if (curGp < _guildPoint)
                    {
                        return 1;
                    }

                    if (curMoney < _guildMoney)
                    {
                        return 2;
                    }

                    if (_battleGroundType == curBattleType)
                    {
                        return 3;
                    }

                    transaction = conn.BeginTransaction();
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, curGp - _guildPoint);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, curMoney - _guildMoney);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.BattleGroundType, curBattleType);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.UpdateTime, DateTime.Now);

                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 명령을 실행합니다.");
                        if (cmd.Execute() > 0)
                        {
                            WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 트랜잭션을 커밋합니다.");
                            transaction.Commit();
                            return 0;
                        }
                    }

                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 트랜잭션을 롤백합니다.");
                    transaction?.Rollback();
                    return -1;

                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _idGuild);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return -1;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _idGuild);
                    WorkSession.WriteStatus(ex2.Message);
                    return -1;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundType() : 연결을 종료합니다");
                }
            }
        }

        public bool ClearBattleGroundType(string _server /* this is just genuinely unused. */, List<long> _GuildList)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 함수에 진입하였습니다");
            if (_GuildList.Count <= 0)
            {
                WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 업데이트 내역이 없습니다.");
                return true;
            }


            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    SimpleCommand cmd;
                    transaction = conn.BeginTransaction();
                    WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 명령을 실행합니다.");
                    foreach (long id in _GuildList)
                    {
                        cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction);
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, id);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.BattleGroundType, 0);
                        cmd.Execute();
                    }
                    transaction.Commit();

                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.ClearBattleGroundType() : 연결을 종료합니다");
                }
            }
        }

        public bool UpdateBattleGroundWinnerType(long _idGuild, byte _BattleGroundWinnerType)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 함수에 진입하였습니다");

            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    // PROCEDURE: UpdateGuildBattleGroundWinnerType
                    transaction = conn.BeginTransaction();
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.BattleGroundType, _BattleGroundWinnerType);
                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 명령을 실행합니다.");
                        if (cmd.Execute() > 0)
                        {
                            WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 트랜잭션을 커밋합니다.");
                            transaction.Commit();
                            return true;
                        }
                    }


                    transaction?.Rollback();
                    return false;
                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _idGuild);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _idGuild);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateBattleGroundWinnerType() : 연결을 종료합니다");
                }
            }
        }

        public bool UpdateGuildStatus(long _idGuild, byte _statusFlag, bool _set, int _guildPointRequired)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 함수에 진입하였습니다");


            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    // PROCEDURE: UpdateGuildStatus
                    int curPoint = -1;
                    byte curStatusFlag = 0;
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                    {

                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, 0);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildStatusFlag, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() == false)
                                return false;

                            curStatusFlag = reader.GetByte(Mabinogi.SQL.Columns.Guild.GuildStatusFlag);
                            curPoint = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildPoint);
                        }
                    }

                    if (curPoint < _guildPointRequired)
                        return false;

                    transaction = conn.BeginTransaction();
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildPoint, curPoint - _guildPointRequired);

                        if (_set)
                            cmd.Set(Mabinogi.SQL.Columns.Guild.GuildStatusFlag, curStatusFlag | _statusFlag);
                        else
                            cmd.Set(Mabinogi.SQL.Columns.Guild.GuildStatusFlag, curStatusFlag & ~_statusFlag);

                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 명령을 실행합니다.");
                        cmd.Execute();
                    }

                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 트랜잭션을 커밋합니다.");
                    transaction.Commit();
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _idGuild);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _idGuild);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateGuildStatus() : 연결을 종료합니다");
                }
            }
        }

        public bool UpdateTitle(long _idGuild, string _strGuildTitle, bool _bUsable)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 함수에 진입하였습니다");


            SimpleTransaction transaction = null;
            WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    // PROCEDURE: UpdateGuildTitle
                    byte curStatusFlag = 0;
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                    {

                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildStatusFlag, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() == false)
                                return false;

                            curStatusFlag = reader.GetByte(Mabinogi.SQL.Columns.Guild.GuildStatusFlag);
                        }
                    }

                    transaction = conn.BeginTransaction();
                    using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.GuildTitle, _strGuildTitle);


                        if (_bUsable)
                            cmd.Set(Mabinogi.SQL.Columns.Guild.GuildStatusFlag, curStatusFlag | 0x08);
                        else
                            cmd.Set(Mabinogi.SQL.Columns.Guild.GuildStatusFlag, curStatusFlag & ~0x08);

                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 명령을 실행합니다.");
                        cmd.Execute();
                    }

                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 트랜잭션을 커밋합니다.");
                    transaction.Commit();
                    return true;

                }
                catch (SimpleSqlException ex)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex, _idGuild);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    if (transaction != null)
                    {
                        WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 트랜잭션을 롤백합니다");
                        transaction.Rollback();
                    }
                    ExceptionMonitor.ExceptionRaised(ex2, _idGuild);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.UpdateTitle() : 연결을 종료합니다");
                }
            }
        }

        public bool GetJoinedMemberCount(long _idGuild, DateTime _startTime, DateTime _endTime, out int _count)
        {
            WorkSession.WriteStatus("GuildSqlAdapter.GetJoinedMemberCount() : 함수에 진입하였습니다");
            _count = 0;

            WorkSession.WriteStatus("GuildSqlAdapter.GetJoinedMemberCount() : 데이터베이스와 연결합니다");
            using (var conn = Connection)
            {
                try
                {

                    // PROCEDURE: GetGuildJoinedMemberByPeriod
                    DateTime created;
                    using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild))
                    {
                        cmd.Where(Mabinogi.SQL.Columns.Guild.Id, _idGuild);
                        cmd.Set(Mabinogi.SQL.Columns.Guild.CreateTime, 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() == false)
                                return false;
                            created = reader.GetDateTime(Mabinogi.SQL.Columns.Guild.CreateTime);
                        }
                    }

                    string cmdStr = string.Format("SELECT COUNT(DISTINCT {0}) {1} FROM {2} WHERE {3} = @{3} AND {4} >= {5} AND {4} <= {6} AND {7} ",
                        /*0*/ Mabinogi.SQL.Columns.GuildMember.Account, /*1*/ Mabinogi.SQL.Columns.Reference.Count, /*2*/ Mabinogi.SQL.Tables.MabiGuild.GuildMember, /*3*/ Mabinogi.SQL.Columns.GuildMember.GuildId, /*4*/ Mabinogi.SQL.Columns.GuildMember.JoinTime, /*5*/ Mabinogi.SQL.Columns.Reference.StartDate, /*6*/ Mabinogi.SQL.Columns.Reference.EndDate, /*7*/ Mabinogi.SQL.Columns.GuildMember.Class);

                    if (created < _startTime)
                        cmdStr += "> @" + Mabinogi.SQL.Columns.GuildMember.Class;
                    else
                        cmdStr += ">= @" + Mabinogi.SQL.Columns.GuildMember.Class;

                    using (var cmd = conn.GetSelectCommand(cmdStr))
                    {
                        cmd.AddParameter(Mabinogi.SQL.Columns.GuildMember.GuildId, _idGuild);
                        cmd.AddParameter(Mabinogi.SQL.Columns.GuildMember.Class, 0);
                        cmd.AddParameter(Mabinogi.SQL.Columns.Reference.StartDate, _startTime);
                        cmd.AddParameter(Mabinogi.SQL.Columns.Reference.EndDate, _endTime);

                        WorkSession.WriteStatus("GetJoinedMemberCount.UpdateTitle() : 명령을 실행합니다.");
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() == false)
                                return false;

                            _count = reader.GetInt32(Mabinogi.SQL.Columns.Reference.Count);
                            return true;
                        }
                    }

                }
                catch (SimpleSqlException ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex, _idGuild);
                    WorkSession.WriteStatus(ex.Message, ex.Number);
                    return false;
                }
                catch (Exception ex2)
                {
                    ExceptionMonitor.ExceptionRaised(ex2, _idGuild);
                    WorkSession.WriteStatus(ex2.Message);
                    return false;
                }
                finally
                {
                    WorkSession.WriteStatus("GuildSqlAdapter.GetJoinedMemberCount() : 연결을 종료합니다");
                }
            }
        }

        public int GetMoney(long guildID)
        {
            using (var conn = Connection)
            {
                var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild);
                cmd.Where(Mabinogi.SQL.Columns.Guild.Id, guildID);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildMoney);
                    else
                        return 0;
                }

            }
        }

        internal bool WithdrawMoney(long guildID, int price, out int remainMoney)
        {
            using (var conn = Connection)
                return WithdrawMoney(guildID, price, out remainMoney, Connection, null);
        }

        internal bool WithdrawMoney(long guildID, int price, out int remainMoney, SimpleConnection conn, SimpleTransaction transaction)
        {
            //PROCEDURE: WithdrawGuildMoney
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.Guild.Id, guildID);
                cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, 0);

                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        remainMoney = reader.GetInt32(Mabinogi.SQL.Columns.Guild.GuildMoney);
                    }
                    else
                    {
                        remainMoney = 0;
                        return false;
                    }
                }
            }

            if (remainMoney < price)
            {
                return false;
            }

            remainMoney -= price;
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.Guild.Id, guildID);
                cmd.Set(Mabinogi.SQL.Columns.Guild.GuildMoney, remainMoney);
                cmd.Execute();
            }
            return true;
        }

        internal bool WithdrawDrawableMoney(long guildID, int price, out int remainDrawableMoney, SimpleConnection conn, SimpleTransaction transaction)
        {
            //PROCEDURE: WithdrawDrawableGuildMoney
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.Guild.Id, guildID);
                cmd.Set(Mabinogi.SQL.Columns.Guild.DrawableMoney, 0);

                using (var reader = cmd.ExecuteReader())
                {

                    if (reader.Read())
                    {
                        remainDrawableMoney = reader.GetInt32(Mabinogi.SQL.Columns.Guild.DrawableMoney);
                    }
                    else
                    {
                        remainDrawableMoney = 0;
                        return false;
                    }
                }
            }

            if (remainDrawableMoney < price)
            {
                return false;
            }

            remainDrawableMoney -= price;
            using (var cmd = conn.GetDefaultUpdateCommand(Mabinogi.SQL.Tables.MabiGuild.Guild, transaction))
            {
                cmd.Where(Mabinogi.SQL.Columns.Guild.Id, guildID);
                cmd.Set(Mabinogi.SQL.Columns.Guild.DrawableMoney, remainDrawableMoney);
                cmd.Execute();
            }
            return true;
        }

        private GuildIDList ReadGuildIdList(SimpleReader reader)
        {
            if (reader.HasRows == false)
                return null;

            GuildIDList result = new GuildIDList();

            while (reader.Read())
            {
                result.guildID.Add(reader.GetInt64(Mabinogi.SQL.Columns.Guild.Id));
            }

            return result;
        }

        public static bool CheckGuildMemberJointime(long memberId, string serverName, SimpleConnection conn)
        {
            using (var cmd = conn.GetDefaultSelectCommand(Mabinogi.SQL.Tables.MabiGuild.AwayGuildMember))
            {
                cmd.Set(Mabinogi.SQL.Columns.AwayGuildMember.JoinTime, 0);
                cmd.Where(Mabinogi.SQL.Columns.AwayGuildMember.Server, serverName);
                cmd.Where(Mabinogi.SQL.Columns.AwayGuildMember.MemberId, memberId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var joinTime = reader.GetDateTime(Mabinogi.SQL.Columns.AwayGuildMember.JoinTime);
                        if (joinTime > DateTime.Now.AddDays(-7))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }

    public static class GuildFlagBit
    {
        internal const int BasicUpkeep = 0x00000001;
        internal const int AdvUpkeep = 0x00000002;
        internal const int GreatUpkeep = 0x00000004;
        internal const int GrandUpkeep = 0x00000008;
        internal const int StoneMark = 0x00000010;
        internal const int StoneBind = 0x00000020;
        internal const int GuildTitleF = 0x00000040;
        internal const int GuildTitleE = 0x00000080;
        internal const int GuildTitleD = 0x00000100;
        internal const int GuildTitleC = 0x00000200;
        internal const int GuildTitleB = 0x00000400;
        internal const int GuildTitleA = 0x00000800;
        internal const int GuildTitle9 = 0x00001000;
        internal const int GuildTitle8 = 0x00002000;
        internal const int GuildTitle7 = 0x00004000;
        internal const int GuildTitle6 = 0x00008000;
        internal const int GuildTitle5 = 0x00010000;
        internal const int GuildTitle4 = 0x00020000;
        internal const int GuildTitle3 = 0x00040000;
        internal const int GuildTitle2 = 0x00080000;
        internal const int GuildTitle1 = 0x00100000;

        internal const int GuildBoard = 0x00200000;
        internal const int RankingBoard = 0x00400000;
        internal const int ScreenShotBoard = 0x00800000;
        internal const int GuildEmblem = 0x01000000;
    }
}
