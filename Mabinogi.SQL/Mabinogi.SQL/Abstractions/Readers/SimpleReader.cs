using System;

namespace Mabinogi.SQL
{ 
    public interface SimpleReader : IDisposable
    {
        bool HasRows { get; }
        bool IsRead { get; }
        byte GetByte(string column);
        short GetInt16(string column);
        int GetInt32(string column);
        long GetInt64(string column);
        DateTime GetDateTime(string column);
        string GetString(string column);
        float GetFloat(string column);
        bool GetBoolean(string column);

        bool Read();

        void Close();
        
        bool GetByteSafe(string column, out byte result);
        bool GetInt16Safe(string column, out short result);
        bool GetInt32Safe(string column, out int result);
        bool GetInt64Safe(string column, out long result);
        bool GetDateTimeSafe(string column, out DateTime result);
        bool GetStringSafe(string column, out string result);
        bool GetFloatSafe(string column, out float result);
        bool GetBooleanSafe(string column, out bool result);
    }
}
