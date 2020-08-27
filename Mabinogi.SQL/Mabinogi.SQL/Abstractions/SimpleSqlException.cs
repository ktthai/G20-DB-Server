using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mabinogi.SQL
{
    [Serializable]
    public class SimpleSqlException : Exception
    {
        public int Number { get { return _number; } }
        private int _number;

        public SimpleSqlException()
        {
        }

        public SimpleSqlException(string message) : base(message)
        {
        }

        public SimpleSqlException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public SimpleSqlException(string message, int code, Exception innerException) : base(message, innerException)
        {
            _number = code;
        }

        protected SimpleSqlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
