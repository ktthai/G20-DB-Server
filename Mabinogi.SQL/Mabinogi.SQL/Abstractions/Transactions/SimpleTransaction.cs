using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mabinogi.SQL
{
    // TODO: Auto rollback if exception, unless specified by boolean
    public interface SimpleTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
