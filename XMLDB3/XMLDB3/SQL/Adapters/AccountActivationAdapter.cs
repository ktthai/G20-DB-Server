using System;
using Mabinogi.SQL;

namespace XMLDB3
{
    public class AccountActivationAdapter : SqlAdapter
    {
        protected override string ConfigRef => Mabinogi.SQL.Tables.Mabinogi.Account;

        public AccountActivationAdapter()
        {
            SetConnection(ConfigManager.GetConnectionString(ConfigRef), ConfigManager.IsLocalMode);
        }

        public bool Create(AccountActivation data)
        {
            try
            {
                using (var cmd = Connection.GetDefaultInsertCommand(Mabinogi.SQL.Tables.Mabinogi.Account))
                {
                    cmd.Set(Mabinogi.SQL.Columns.Account.Id, data.id);
                    cmd.Set(Mabinogi.SQL.Columns.Account.Password, data.password);
                    cmd.Set(Mabinogi.SQL.Columns.Account.Name, data.name);
                    cmd.Set(Mabinogi.SQL.Columns.Account.Email, data.email);
                    cmd.Set(Mabinogi.SQL.Columns.Account.Flag, data.flag);
                    cmd.Set(Mabinogi.SQL.Columns.Account.BlockingDate, data.blocking_date);
                    cmd.Set(Mabinogi.SQL.Columns.Account.BlockingDuration, data.blocking_duration);
                    cmd.Set(Mabinogi.SQL.Columns.Account.Authority, data.authority);
                    cmd.Set(Mabinogi.SQL.Columns.Account.ProviderCode, data.provider_code);

                    return cmd.Execute() > 0;
                }
            }
            catch (SimpleSqlException ex)
            {
                ExceptionMonitor.ExceptionRaised(ex, data);
                WorkSession.WriteStatus(ex.Message, ex.Number);
                return false;
            }
            catch (Exception ex2)
            {
                ExceptionMonitor.ExceptionRaised(ex2, data);
                WorkSession.WriteStatus(ex2.Message, data);
                return false;
            }

            /* TODO: Create shop functionality, and implement "INSERT INTO `mabi_shop.dbo.shop_fantasylifeclub`
             * ([id],[naosupportexpiration],[storageexpiration],[updated],[checked])
             * Values( @id,@activedatetime,@activedatetime,getdate(),getdate() ) "
             @activedatetime = today + free premium trial length */
        }
    }
}
