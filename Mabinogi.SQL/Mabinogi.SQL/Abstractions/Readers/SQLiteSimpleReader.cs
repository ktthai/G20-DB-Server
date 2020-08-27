using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Mabinogi.SQL
{
    public class SQLiteSimpleReader : SimpleReader
    {
        protected SQLiteDataReader _reader;
        protected Dictionary<string, int> _columns;
        public bool IsRead { get { return _IsRead; } }
        protected bool _IsRead;
        public SQLiteSimpleReader(SQLiteDataReader reader)
        {
            _reader = reader;
            _columns = new Dictionary<string, int>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                _columns.Add(reader.GetName(i), i);
            }
        }

        public void Dispose()
        {
            Close();
        }
        public void Close()
        {
            _reader.Close();
        }

        public byte GetByte(string column)
        {
            return _reader.GetByte(_columns[column]);
        }

        public DateTime GetDateTime(string column)
        {
            if (_IsRead == false)
                ReadSafe();
            return _reader.GetDateTime(_columns[column]);
        }

        public float GetFloat(string column)
        {
            if (_IsRead == false)
                ReadSafe();
            return _reader.GetFloat(_columns[column]);
        }

        public short GetInt16(string column)
        {
            if (_IsRead == false)
                ReadSafe();
            return _reader.GetInt16(_columns[column]);
        }

        public int GetInt32(string column)
        {
            if (_IsRead == false)
                ReadSafe();
            return _reader.GetInt32(_columns[column]);
        }

        public long GetInt64(string column)
        {
            if (_IsRead == false)
                ReadSafe();
            return _reader.GetInt64(_columns[column]);
        }

        public string GetString(string column)
        {
            if (_IsRead == false)
                ReadSafe();
            return _reader.GetString(_columns[column]);
        }

        public bool GetBoolean(string column)
        {
            if (_IsRead == false)
                ReadSafe();
            return _reader.GetBoolean(_columns[column]);
        }

        public bool HasRows { get { return _reader.HasRows; } }

        private void ReadSafe()
        {
            _IsRead = true;
            // TODO: Share EventLogger
            //EventLogger.WriteEventLog("Read() was not called before attempting to read values at :" + Environment.StackTrace);
            _reader.Read();
        }

        public bool Read()
        {
            _IsRead = true;
            return _reader.Read();
        }

        public bool GetDateTimeSafe(string column, out DateTime result)
        {
            
            if (_reader.IsDBNull(_columns[column]))
            {
                result = DateTime.MinValue;
                return false;
            }

            result = GetDateTime(column);
            return true;
        }

        public bool GetByteSafe(string column, out byte result)
        {
            
            if (_reader.IsDBNull(_columns[column]))
            {
                result = 0;
                return false;
            }

            result = GetByte(column);
            return true;
        }

        public bool GetInt16Safe(string column, out short result)
        {
            if (_reader.IsDBNull(_columns[column]))
            {
                result = 0;
                return false;
            }

            result = GetInt16(column);
            return true;
        }

        public bool GetInt32Safe(string column, out int result)
        {
            if (_reader.IsDBNull(_columns[column]))
            {
                result = 0;
                return false;
            }

            result = GetInt32(column);
            return true;
        }

        public bool GetInt64Safe(string column, out long result)
        {
            if (_reader.IsDBNull(_columns[column]))
            {
                result = 0;
                return false;
            }

            result = GetInt64(column);
            return true;
        }

        public bool GetStringSafe(string column, out string result)
        {
            if (_reader.IsDBNull(_columns[column]))
            {
                result = string.Empty;
                return false;
            }

            result = GetString(column);
            return true;
        }

        public bool GetFloatSafe(string column, out float result)
        {
            if (_reader.IsDBNull(_columns[column]))
            {
                result = 0;
                return false;
            }

            result = GetFloat(column);
            return true;
        }

        public bool GetBooleanSafe(string column, out bool result)
        {
            if (_reader.IsDBNull(_columns[column]))
            {
                result = false;
                return false;
            }

            result = GetBoolean(column);
            return true;
        }
    }
}
