using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mabinogi.SQL
{
    public interface SimpleConnection : IDisposable
    {
        SimpleTransaction BeginTransaction();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        SimpleCommand GetInsertCommand(string command);

        SimpleCommand GetInsertCommand(string command, SimpleTransaction transaction = null);

        SimpleCommand GetDefaultDeleteCommand(string table);

        SimpleCommand GetDefaultDeleteCommand(string table, SimpleTransaction transaction = null);

        SimpleCommand GetDeleteCommand(string command);

        SimpleCommand GetDeleteCommand(string command, SimpleTransaction transaction = null);

        SimpleCommand GetDefaultInsertCommand(string table);

        SimpleCommand GetDefaultInsertCommand(string table, SimpleTransaction transaction = null);

        SimpleCommand GetUpdateCommand(string command);

        SimpleCommand GetUpdateCommand(string command, SimpleTransaction transaction = null);

        SimpleCommand GetDefaultUpdateCommand(string table);

        SimpleCommand GetDefaultUpdateCommand(string table, SimpleTransaction transaction = null);

        SimpleCommand GetSelectCommand(string command);

        SimpleCommand GetSelectCommand(string command, SimpleTransaction transaction = null);

        SimpleCommand GetDefaultSelectCommand(string table);

        SimpleCommand GetDefaultSelectCommand(string table, SimpleTransaction transaction = null);
    }
}
