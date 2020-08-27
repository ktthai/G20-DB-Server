using Mabinogi;
using Mabinogi.SQL;
using System;
using System.Text.Json;

using Columns = Mabinogi.SQL.Columns;
using Tables = Mabinogi.SQL.Tables;

namespace Authenticator
{
    public abstract class CardInterface
    {
        protected string m_Id = string.Empty;

        protected long m_CardId;

        protected string m_Type = string.Empty;

        protected int m_Status;

        protected long m_EntityId;

        protected string m_EntityName = string.Empty;

        protected int m_RebirthCount;

        protected string m_Server = string.Empty;

        protected string m_Reserved = string.Empty;

        protected DateTime m_Created = DateTime.MinValue;

        protected DateTime m_Used = DateTime.MinValue;

        protected DateTime m_Ended = DateTime.MinValue;

        protected string[] m_ValidServers;

        

        public void LoadFromReader(SimpleReader reader)
        {
            m_Id = reader.GetString(Columns.Cards.Id);
            m_CardId = reader.GetInt64(Columns.Cards.CardId);
            m_Type = reader.GetString(Columns.Cards.Type);
            m_Status = reader.GetInt32(Columns.Cards.Status);

            reader.GetInt64Safe(Columns.Cards.EntityId, out m_EntityId);
            reader.GetStringSafe(Columns.Cards.EntityName, out m_EntityName);
            reader.GetInt32Safe(Columns.Cards.RebirthCount, out m_RebirthCount);
            reader.GetStringSafe(Columns.Cards.Server, out m_Server);
            reader.GetStringSafe(Columns.Cards.Reserved, out m_Reserved);

            reader.GetDateTimeSafe(Columns.Cards.Created, out m_Created);
            reader.GetDateTimeSafe(Columns.Cards.Used, out m_Used);
            reader.GetDateTimeSafe(Columns.Cards.Ended, out m_Ended);

            string text;
            if (reader.GetStringSafe(Columns.Cards.ValidServer, out text))
            {
                m_ValidServers = JsonSerializer.Deserialize<string[]>(text);
            }
        }

        public abstract Message ToMessage();


        
    }
}
