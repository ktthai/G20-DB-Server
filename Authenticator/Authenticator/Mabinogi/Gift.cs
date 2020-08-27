using Mabinogi;
using Mabinogi.SQL;
using System;
using System.Collections.Generic;

using Columns = Mabinogi.SQL.Columns;
using Tables = Mabinogi.SQL.Tables;

namespace Authenticator
{
    public class Gift
    {
        private long m_CardId;

        private ECardType m_CardType;

        private string m_Type = string.Empty;

        private EGiftStatus m_GiftStatus = EGiftStatus.egsInvalid;

        private string m_SenderId = string.Empty;

        private long m_SenderCharId;

        private string m_SenderCharName = string.Empty;

        private string m_SenderServer = string.Empty;

        private string m_SenderMsg = string.Empty;

        private string m_ReceiverId = string.Empty;

        private long m_ReceiverCharId;

        private string m_ReceiverCharName = string.Empty;

        private string m_ReceiverServer = string.Empty;

        private DateTime m_SendDate = DateTime.MinValue;


        protected static SimpleConnection Connection
        {
            get
            {
                if (ServerConfiguration.IsLocalTestMode)
                {
                    return new SQLiteSimpleConnection(ServerConfiguration.GiftConnectionString);
                }
                else
                {
                    return new MySqlSimpleConnection(ServerConfiguration.GiftConnectionString);
                }
            }
        }

        protected static SimpleConnection CharacterCardConnection
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
        protected static SimpleConnection PetCardConnection
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

        public Message ToMessage()
        {
            WorkSession.WriteStatus("Gift.ToMessage() : enter");
            Message message = new Message(0u, 0uL);
            message.WriteS64(m_CardId);
            message.WriteU8((byte)m_CardType);
            message.WriteString(m_Type);
            message.WriteS32((int)m_GiftStatus);
            message.WriteString(m_SenderId);
            message.WriteS64(m_SenderCharId);
            message.WriteString(m_SenderCharName);
            message.WriteString(m_SenderServer);
            message.WriteString(m_SenderMsg);
            message.WriteString(m_ReceiverId);
            message.WriteS64(m_ReceiverCharId);
            message.WriteString(m_ReceiverCharName);
            message.WriteString(m_ReceiverServer);
            message.WriteS64(m_SendDate.Ticks);
            WorkSession.WriteStatus("Gift.ToMessage() : complete, return msg");
            return message;
        }

        public static Gift[] Load(string _account)
        {
            try
            {
                WorkSession.WriteStatus("Gift.Load(\"" + _account + "\") : open db connection");
                using (var conn = Connection)
                using (var cmd = conn.GetDefaultSelectCommand(Tables.Shop.Gifts))
                {
                    cmd.Where(Columns.Gifts.ReceiverId, _account);
                    cmd.Where(Columns.Gifts.Status, 0);

                    using (var reader = cmd.ExecuteReader())
                    {
                        List<Gift> gifts = new List<Gift>();
                        Gift gift;
                        while (reader.Read())
                        {
                            gift = new Gift();

                            WorkSession.WriteStatus("Gift.Load(\"" + _account + "\") : making gift info ");
                            gift.LoadFromReader(reader);

                            gifts.Add(gift);
                        }

                        return gifts.ToArray();
                    }
                }
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2);
                return new Gift[0];
            }
            finally
            {
                WorkSession.WriteStatus("Gift.Load(\"" + _account + "\") : close db connection");
            }
        }

        public void LoadFromReader(SimpleReader reader)
        {
            m_ReceiverId = reader.GetString(Columns.Gifts.ReceiverId);

            m_CardId = reader.GetInt64(Columns.Gifts.CardId);
            m_CardType = (ECardType)reader.GetByte(Columns.Gifts.CardType);
            m_Type = reader.GetString(Columns.Gifts.Type);
            m_GiftStatus = (EGiftStatus)reader.GetInt64(Columns.Gifts.Status);
            if (m_CardType == ECardType.ectInvalid || m_GiftStatus == EGiftStatus.egsInvalid)
            {
                ExceptionMonitor.ExceptionRaised(new Exception("invalid gift from " + m_ReceiverId));
            }
            else
            {
                m_SenderId = reader.GetString(Columns.Gifts.SenderId);
                reader.GetInt64Safe(Columns.Gifts.SenderCharId, out m_SenderCharId);
                reader.GetStringSafe(Columns.Gifts.SenderCharName, out m_SenderCharName);
                reader.GetStringSafe(Columns.Gifts.SenderServer, out m_SenderServer);
                reader.GetStringSafe(Columns.Gifts.SenderMessage, out m_SenderMsg);

                reader.GetInt64Safe(Columns.Gifts.ReceiverCharId, out m_ReceiverCharId);
                reader.GetStringSafe(Columns.Gifts.ReceiverCharName, out m_ReceiverCharName);
                reader.GetStringSafe(Columns.Gifts.ReceiverServer, out m_ReceiverServer);
                m_SendDate = reader.GetDateTime(Columns.Gifts.SendDate);
                WorkSession.WriteStatus("Gift.Load(\"" + m_ReceiverId + "\") : complete making gift info");
            }
        }

        public static CardInterface AcceptGift(string _receiver, string _sender, long _cardid, ECardType _cardType, string _userIP)
        {
            WorkSession.WriteStatus("Gift.AcceptGift(\"" + _receiver + "\") : enter");
            string table;
            CardInterface cardInterface = null;
            switch (_cardType)
            {
                case ECardType.ectCharacterCard:
                    cardInterface = new CharacterCard();
                    table = Tables.Shop.CharacterCards;
                    break;
                case ECardType.ectPetCard:
                    cardInterface = new PetCard();
                    table = Tables.Shop.PetCards;
                    break;
                default:
                    throw new Exception("[" + _cardid + "] 카드 타입이 유효하지 않습니다.");
            }


            using (var cardConn = (_cardType == ECardType.ectCharacterCard ? CharacterCardConnection : PetCardConnection))
            using (var trans = cardConn.BeginTransaction())
            {
                try
                {
                    int status = -1;
                    using (var cmd = cardConn.GetDefaultSelectCommand(table, trans))
                    {
                        cmd.Where(Columns.Cards.CardId, _cardid);
                        cmd.Where(Columns.Cards.Id, _sender);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                status = reader.GetInt32(Columns.Cards.Status);

                                if (status == 3)
                                    cardInterface.LoadFromReader(reader);
                            }
                        }
                    }

                    if (status == 3)
                    {
                        using (var cmd = cardConn.GetDefaultUpdateCommand(table, trans))
                        {
                            cmd.Where(Columns.Cards.CardId, _cardid);
                            cmd.Where(Columns.Cards.Id, _sender);

                            cmd.Set(Columns.Cards.Status, 0);
                        }
                    }
                    else if (status == -1)
                    {
                        throw new Exception("Gift.AcceptGift() Card does not exist!");
                    }
                    else
                    {
                        throw new Exception("Gift.AcceptGift() Card is not gift!");
                    }
                }
                catch (SimpleSqlException ex)
                {
                    trans.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex);
                    return null;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex);
                    return null;
                }
            }

            using (var giftConn = Connection)
            using (var trans = giftConn.BeginTransaction())
            {
                try
                {
                    Gift gift = null;
                    string tid = string.Empty;
                    using (var cmd = giftConn.GetDefaultSelectCommand(Tables.Shop.Gifts, trans))
                    {
                        cmd.Where(Columns.Gifts.ReceiverId, _receiver);
                        cmd.Where(Columns.Gifts.CardId, _cardid);
                        cmd.Where(Columns.Gifts.CardType, _cardType);



                        using (var reader = cmd.ExecuteReader())
                        {
                            int status = -1;
                            if (reader.Read())
                            {
                                status = reader.GetInt32(Columns.Gifts.Status);
                                if (status == 0)
                                {
                                    gift = new Gift();
                                    gift.LoadFromReader(reader);
                                    tid = reader.GetString(Columns.Gifts.TypeId);
                                }
                                else
                                {
                                    throw new Exception("Gift.AcceptGift() Gift not valid state!");
                                }
                            }
                            else
                            {
                                throw new Exception("Gift.AcceptGift() Gift does not exist!");
                            }
                        }
                    }
                    if (gift != null)
                    {
                        using (var cmd = giftConn.GetDefaultInsertCommand(Tables.Shop.GiftHistory, trans))
                        {
                            cmd.Set(Columns.GiftHistory.CardId, gift.m_CardId);
                            cmd.Set(Columns.GiftHistory.CardType, gift.m_CardType);
                            cmd.Set(Columns.GiftHistory.TypeId, tid);
                            cmd.Set(Columns.GiftHistory.Type, gift.m_Type);
                            cmd.Set(Columns.GiftHistory.Status, 1);

                            cmd.Set(Columns.GiftHistory.RegDate, DateTime.Now);
                            cmd.Set(Columns.GiftHistory.SenderId, gift.m_SenderId);
                            cmd.Set(Columns.GiftHistory.SenderCharId, gift.m_SenderCharId);
                            cmd.Set(Columns.GiftHistory.SenderCharName, gift.m_SenderCharName);
                            cmd.Set(Columns.GiftHistory.SenderServer, gift.m_SenderServer);

                            cmd.Set(Columns.GiftHistory.ReceiverId, gift.m_ReceiverId);
                            cmd.Set(Columns.GiftHistory.ReceiverCharId, gift.m_ReceiverCharId);
                            cmd.Set(Columns.GiftHistory.ReceiverCharName, gift.m_ReceiverCharName);
                            cmd.Set(Columns.GiftHistory.ReceiverServer, gift.m_ReceiverServer);
                            cmd.Set(Columns.GiftHistory.SenderMessage, gift.m_SenderMsg);

                            cmd.Set(Columns.GiftHistory.SendDate, gift.m_SendDate);

                            cmd.Execute();
                        }

                        using (var cmd = giftConn.GetDefaultDeleteCommand(Tables.Shop.Gifts, trans))
                        {
                            cmd.Where(Columns.Gifts.ReceiverId, _receiver);
                            cmd.Where(Columns.Gifts.CardId, _cardid);
                            cmd.Where(Columns.Gifts.CardType, _cardType);

                            cmd.Execute();
                        }
                    }
                }
                catch (SimpleSqlException ex)
                {
                    trans.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex);
                    return null;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex);
                    return null;
                }
                finally
                {
                    WorkSession.WriteStatus("Gift.AcceptGift(\"" + _receiver + "\") : close db connection");
                }
            }
            return cardInterface;
        }

        public static bool RejectGift(string _account, long _cardid, ECardType _cardType)
        {
            WorkSession.WriteStatus("Gift.RejectGift(\"" + _account + "\") : enter");

            using (var conn = Connection)
            using (var trans = conn.BeginTransaction())
            {
                try
                {
                    int status = -1;

                    using (var cmd = conn.GetDefaultSelectCommand(Tables.Shop.Gifts, trans))
                    {
                        cmd.Where(Columns.Gifts.ReceiverId, _account);
                        cmd.Where(Columns.Gifts.CardId, _cardid);
                        cmd.Where(Columns.Gifts.CardType, _cardType);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                status = reader.GetInt32(Columns.Gifts.Status);
                            }
                            else
                            {
                                throw new Exception("Gift.RejectGift() Gift does not exist!");
                            }
                        }
                    }

                    if (status == 0)
                    {
                        using (var cmd = conn.GetDefaultUpdateCommand(Tables.Shop.Gifts, trans))
                        {
                            cmd.Where(Columns.Gifts.ReceiverId, _account);
                            cmd.Where(Columns.Gifts.CardId, _cardid);
                            cmd.Where(Columns.Gifts.CardType, _cardType);

                            cmd.Set(Columns.Gifts.RejectDate, DateTime.Now);
                            cmd.Set(Columns.Gifts.Status, 2);

                            cmd.Execute();
                        }
                    }
                    else
                    {
                        throw new Exception("Gift.RejectGift() Gift not valid state!");
                    }

                    trans.Commit();
                    return true;
                }
                catch (SimpleSqlException ex)
                {
                    trans.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex);
                    return false;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex);
                    return false;
                }
            }
        }
    }
}
