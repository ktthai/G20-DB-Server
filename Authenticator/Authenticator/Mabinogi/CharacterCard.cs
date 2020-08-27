using Mabinogi;
using Mabinogi.SQL;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Columns = Mabinogi.SQL.Columns;
using Tables = Mabinogi.SQL.Tables;

namespace Authenticator
{
    public class CharacterCard : CardInterface
    {
        public override Message ToMessage()
        {
            WorkSession.WriteStatus("CharacterCard.ToMessage() : enter");
            Message message = new Message(0u, 0uL);
            message.WriteString(m_Id);
            message.WriteS64(m_CardId);
            message.WriteString(m_Type);
            message.WriteS32(m_Status);
            message.WriteS64(m_EntityId);
            message.WriteString(m_EntityName);
            message.WriteS32(m_RebirthCount);
            message.WriteString(m_Server);
            message.WriteString(m_Reserved);
            if (m_Created == DateTime.MinValue)
            {
                message.WriteS64(0L);
            }
            else
            {
                message.WriteS64(m_Created.Ticks);
            }
            if (m_Used == DateTime.MinValue)
            {
                message.WriteS64(0L);
            }
            else
            {
                message.WriteS64(m_Used.Ticks);
            }
            if (m_Ended == DateTime.MinValue)
            {
                message.WriteU64(0uL);
            }
            else
            {
                message.WriteS64(m_Ended.Ticks);
            }
            if (m_ValidServers != null)
            {
                message.WriteS32(m_ValidServers.Length);

                foreach (string characterCardServer in m_ValidServers)
                {
                    message.WriteString(characterCardServer);
                }
            }
            else
            {
                message.WriteS32(0);
            }
            WorkSession.WriteStatus("CharacterCard.ToMessage() : complete, return msg");
            return message;
        }

        protected static SimpleConnection Connection
        {
            get
            {
                if (ServerConfiguration.IsLocalTestMode)
                {
                    return new SQLiteSimpleConnection(ServerConfiguration.CharacterCardConnectionString);
                }
                else
                {
                    return new MySqlSimpleConnection(ServerConfiguration.CharacterCardConnectionString);
                }
            }
        }

        public static CharacterCard[] Load(string _account)
        {
            WorkSession.WriteStatus("CharacterCard.Load(\"" + _account + "\") : enter");
            try
            {
                WorkSession.WriteStatus("CharacterCard.Load(\"" + _account + "\") : open db connection");

                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Tables.Shop.CharacterCards))
                {
                    cmd.Where(Columns.Cards.Id, _account);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<CharacterCard> cards = new List<CharacterCard>();
                        CharacterCard card;
                        while (reader.Read())
                        {
                            card = new CharacterCard();
                            card.LoadFromReader(reader);
                            cards.Add(card);
                        }
                        return cards.ToArray();
                    }
                }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
            }
            finally
            {
                WorkSession.WriteStatus("CharacterCard.Load(\"" + _account + "\") : close db connection");
            }
            return new CharacterCard[0];
        }

        public static bool Begin_UsingTransaction(string _account, long _cardid, long _entityId, string _entityName, int _rebirthcount, string _server, string _comment)
        {
            WorkSession.WriteStatus("Card.Begin_UsingTransaction(\"" + _account + "\") : enter");

            WorkSession.WriteStatus("Card.Begin_UsingTransaction(\"" + _account + "\") : open db connection");
            using (var conn = Connection)
            using (var trans = conn.BeginTransaction())
            using (var cmd = conn.GetDefaultUpdateCommand(Tables.Shop.CharacterCards, trans))
            {

                try
                {
                    cmd.Where(Columns.Cards.Id, _account);
                    cmd.Where(Columns.Cards.CardId, _cardid);

                    cmd.Set(Columns.Cards.EntityId, _entityId);
                    cmd.Set(Columns.Cards.EntityName, _entityName);
                    cmd.Set(Columns.Cards.RebirthCount, _rebirthcount);
                    cmd.Set(Columns.Cards.Reserved, _comment);
                    cmd.Set(Columns.Cards.Server, _server);
                    cmd.Set(Columns.Cards.Status, 1);
                    cmd.Set(Columns.Cards.Used, DateTime.Now);

                    var result = cmd.Execute() == 1;

                    trans.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    trans.Rollback();
                    return false;
                }
            }

        }

        public static bool Commit_UsingTransaction(string _account, long _cardid, long _entityid)
        {
            WorkSession.WriteStatus("Card.Commit_UsingTransaction(\"" + _account + "\") : enter");

            WorkSession.WriteStatus("Card.Commit_UsingTransaction(\"" + _account + "\") : open db connection");

            using (var conn = Connection)
            using (var trans = conn.BeginTransaction())
            using (var cmd = conn.GetUpdateCommand($"UPDATE `{Tables.Shop.CharacterCards}` SET `{Columns.Cards.Status}` = @{Columns.Cards.Status} WHERE `{Columns.Cards.Id}` = @{Columns.Cards.Id} " +
                $"AND `{Columns.Cards.Status}` = @{Columns.Cards.Status}1 AND `{Columns.Cards.CardId}` = @{Columns.Cards.CardId} " +
                $"AND `{Columns.Cards.EntityId}` = @{Columns.Cards.EntityId}", trans))
            {
                try
                {
                    cmd.AddParameter(Columns.Cards.Id, _account);
                    cmd.AddParameter(Columns.Cards.CardId, _cardid);
                    cmd.AddParameter(Columns.Cards.Status, 2);
                    cmd.AddParameter(Columns.Cards.EntityId, _entityid);

                    cmd.AddParameter(Columns.Cards.Status + "1", 1);
                    var result = cmd.Execute() == 1;

                    trans.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    trans.Rollback();
                    return false;
                }
            }
        }

        public static bool Rollback_UsingTransaction(string _account, long _cardid, long _petid)
        {
            WorkSession.WriteStatus("Card.Rollback_UsingTransaction(\"" + _account + "\") : enter");

            using (var conn = Connection)
            using (var trans = conn.BeginTransaction())
            using (var readCmd = conn.GetDefaultSelectCommand(Tables.Shop.CharacterCards, trans))
            {
                try
                {
                    readCmd.Where(Columns.Cards.Id, _account);
                    readCmd.Where(Columns.Cards.CardId, _cardid);

                    bool check = false;
                    using (var reader = readCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            check = reader.GetInt32(Columns.Cards.Status) == 1;
                        }
                    }

                    if (check)
                    {
                        using (var cmd = conn.GetDefaultUpdateCommand(Tables.Shop.CharacterCards, trans))
                        {
                            cmd.Where(Columns.Cards.Id, _account);
                            cmd.Where(Columns.Cards.CardId, _cardid);

                            cmd.Set(Columns.Cards.Status, 0);
                            cmd.Set(Columns.Cards.EntityName, null);
                            cmd.Set(Columns.Cards.Reserved, null);
                            cmd.Set(Columns.Cards.Server, null);
                            cmd.Set(Columns.Cards.Used, null);

                            cmd.Execute();

                            trans.Commit();
                            return true;
                        }
                    }

                    trans.Rollback();
                    return false;
                }
                catch (Exception ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    trans.Rollback();
                    return false;
                }
            }
        }

        public static bool CreateCardWithServer(string _account, string _type, DateTime _ended, string[] _validServer)
        {
            WorkSession.WriteStatus("Card.CreateCard(\"" + _account + "\") : enter");

            WorkSession.WriteStatus("Card.CreateCard(\"" + _account + "\") : open db connection");

            using (var conn = Connection)
            using (var trans = conn.BeginTransaction())
            {
                if (ServerConfiguration.IsLocalTestMode)
                {
                    using (var cmd = conn.GetInsertCommand(string.Format("INSERT INTO `{0}`(`{1}`,`{2}`,`{3}`,`{4}`,`{5}`) VALUES(@{1},@{2},@{3},@{4},(SELECT COUNT(*) FROM `{0}`))",
                        /*0*/ Tables.Shop.CharacterCards, /*1*/ Columns.Cards.Id, /*2*/ Columns.Cards.Created, /*3*/ Columns.Cards.Type, /*4*/ Columns.Cards.ValidServer, /*5*/ Columns.Cards.CardId), trans))
                    {

                        try
                        {
                            cmd.AddParameter(Columns.Cards.Id, _account);
                            cmd.AddParameter(Columns.Cards.Created, DateTime.Now);
                            cmd.AddParameter(Columns.Cards.Type, _type);
                            cmd.AddParameter(Columns.Cards.ValidServer, JsonSerializer.Serialize(_validServer));

                            if (_ended == DateTime.MinValue)
                                cmd.Set(Columns.Cards.Ended, null);
                            else
                                cmd.Set(Columns.Cards.Ended, _ended);

                            var result = cmd.Execute() == 1;

                            trans.Commit();
                            return result;
                        }
                        catch (Exception ex)
                        {
                            ExceptionMonitor.ExceptionRaised(ex);
                            trans.Rollback();
                            return false;
                        }
                    }
                }
                else
                {
                    using (var cmd = conn.GetDefaultInsertCommand(Tables.Shop.CharacterCards, trans))
                    {

                        try
                        {
                            cmd.Set(Columns.Cards.Id, _account);
                            cmd.Set(Columns.Cards.Created, DateTime.Now);
                            cmd.Set(Columns.Cards.Type, _type);
                            cmd.Set(Columns.Cards.ValidServer, JsonSerializer.Serialize(_validServer));

                            if (_ended == DateTime.MinValue)
                                cmd.Set(Columns.Cards.Ended, null);
                            else
                                cmd.Set(Columns.Cards.Ended, _ended);

                            var result = cmd.Execute() == 1;

                            trans.Commit();
                            return result;
                        }
                        catch (Exception ex)
                        {
                            ExceptionMonitor.ExceptionRaised(ex);
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }
        }

        public static bool CreateCard(string _account, string _type)
        {
            WorkSession.WriteStatus("Card.CreateDefaultCard(\"" + _account + "\") : enter");
            try
            {
                WorkSession.WriteStatus("Card.CreateDefaultCard(\"" + _account + "\") : open db connection");
                using (var conn = Connection)
                {
                    if (ServerConfiguration.IsLocalTestMode)
                    {
                        using (var cmd = conn.GetInsertCommand(string.Format("INSERT INTO `{0}`(`{1}`,`{2}`,`{3}`) VALUES(@{1},@{2},(SELECT COUNT(*) FROM `{0}`))",
                        /*0*/ Tables.Shop.CharacterCards, /*1*/ Columns.Cards.Id, /*2*/ Columns.Cards.Type, /*3*/ Columns.Cards.CardId)))
                        {
                            cmd.AddParameter(Columns.Cards.Id, _account);
                            cmd.AddParameter(Columns.Cards.Type, _type);

                            return cmd.Execute() == 1;
                        }
                    }
                    else
                    {
                        using (var cmd = conn.GetDefaultInsertCommand(Tables.Shop.CharacterCards))
                        {
                            cmd.Set(Columns.Cards.Id, _account);
                            cmd.Set(Columns.Cards.Type, _type);

                            return cmd.Execute() == 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
                return false;
            }
            finally
            {
                WorkSession.WriteStatus("Card.CreateDefaultCard(\"" + _account + "\") : close db connection");
            }
        }

        public static bool DeleteCard(string _account, long _cardID)
        {
            WorkSession.WriteStatus("Card.Commit_UsingTransaction(\"" + _account + "\") : enter");

            WorkSession.WriteStatus("Card.Commit_UsingTransaction(\"" + _account + "\") : open db connection");

            using (var conn = Connection)
            using (var trans = conn.BeginTransaction())
            using (var cmd = conn.GetUpdateCommand($"UPDATE `{Tables.Shop.CharacterCards}` SET `{Columns.Cards.Status}` = @{Columns.Cards.Status} WHERE `{Columns.Cards.Id}` = @{Columns.Cards.Id} " +
                $"AND `{Columns.Cards.Status}` = @{Columns.Cards.Status}1 AND `{Columns.Cards.CardId}` = @{Columns.Cards.CardId} ", trans))
            {
                try
                {
                    cmd.AddParameter(Columns.Cards.Id, _account);
                    cmd.AddParameter(Columns.Cards.CardId, _cardID);
                    cmd.AddParameter(Columns.Cards.Status, 2);

                    cmd.AddParameter(Columns.Cards.Status + "1", 0);
                    var result = cmd.Execute() == 1;

                    trans.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    trans.Rollback();
                    return false;
                }
            }
        }
    }
}
