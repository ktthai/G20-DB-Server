using System;
using Mabinogi.SQL;

using Tables = Mabinogi.SQL.Tables;
using Columns = Mabinogi.SQL.Columns;

namespace Authenticator
{
	public class FreeService
	{
		private string m_account = string.Empty;

        protected static SimpleConnection Connection
        {
            get
            {
                if (ServerConfiguration.IsLocalTestMode)
                {
                    return new SQLiteSimpleConnection(ServerConfiguration.FreeServiceConnectionString);
                }
                else
                {
                    return new MySqlSimpleConnection(ServerConfiguration.FreeServiceConnectionString);
                }
            }
        }

        public static bool FindInfo(string _account)
		{
			WorkSession.WriteStatus("FreeService.FindInfo(\"" + _account + "\") : enter");

			try
			{
				using (var conn = Connection)
				using (var cmd = conn.GetDefaultSelectCommand(Tables.Shop.FreeServiceAccount))
				{
					cmd.Where(Columns.FreeServiceAccount.Account, _account);

					using (var reader = cmd.ExecuteReader())
						return reader.Read();
				}
			}
            catch (Exception ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
				return false;
            }
		}

		public static bool Update(string _account, bool _free)
		{
			WorkSession.WriteStatus("FreeService.Update(\"" + _account + "\") : enter");

			try
			{
				using (var conn = Connection)
				{
					if (_free)
					{
						using (var cmd = conn.GetDefaultInsertCommand(Tables.Shop.FreeServiceAccount))
						{
							cmd.Set(Columns.FreeServiceAccount.Account, _account);
							cmd.Execute();
						}
					}
					else
					{
						using (var cmd = conn.GetDefaultDeleteCommand(Tables.Shop.FreeServiceAccount))
						{
							cmd.Where(Columns.FreeServiceAccount.Account, _account);
							cmd.Execute();
						}
					}
				}
				return true;
			}
            catch (Exception ex)
            {
                ExceptionMonitor.ExceptionRaised(ex);
				return false;
            }
		}
	}
}
