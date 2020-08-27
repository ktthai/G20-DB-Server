using Mabinogi;
using System;
using Mabinogi.SQL;

using Tables = Mabinogi.SQL.Tables;
using Columns = Mabinogi.SQL.Columns;


namespace Authenticator
{
	public class PremiumPack
	{
		protected class PPInfo
		{
			public bool canUseInventoryPlus;

			public DateTime inventoryPlusExpiration = DateTime.MinValue;

			public bool canUsePremiumPack;

			public DateTime premiumPackExpiration = DateTime.MinValue;

			public bool canUseVip;

			public DateTime vipExpiration = DateTime.MinValue;

			public bool canUsePremiumVip;

			public DateTime primiumVipExpiration = DateTime.MinValue;

			public bool canUseguildpack;

			public DateTime guildpackExpiration = DateTime.MinValue;

			public bool payCheck;
		}

		protected string account;

		protected PPInfo ppInfo;

		protected PremiumPack(string _account)
		{
			account = _account;
			ppInfo = null;
		}

		public static PremiumPack GetPremiumPack(string _account, bool _bTestMode)
		{
			return new PremiumPack(_account);
		}

		public bool Process()
		{
			ppInfo = Load();
			ppInfo = ApplyPremiumPackEvent(ppInfo, ServerConfiguration.PremiumPackEvent);
			return ppInfo != null;
		}

		private PPInfo ApplyPremiumPackEvent(PPInfo _info, EventPremiumPack _event)
		{
			DateTime now = DateTime.Now;
			if (_event != null && now >= _event.eventStart && now <= _event.eventEnd)
			{
				if (_info == null)
				{
					_info = new PPInfo();
				}
				_info.canUseInventoryPlus = true;
				_info.inventoryPlusExpiration = _event.eventEnd;
				_info.canUsePremiumPack = true;
				_info.premiumPackExpiration = _event.eventEnd;
				_info.canUseVip = true;
				_info.vipExpiration = _event.eventEnd;
				_info.canUsePremiumVip = true;
				_info.primiumVipExpiration = _event.eventEnd;
				_info.canUseguildpack = true;
				_info.guildpackExpiration = _event.eventEnd;
				return _info;
			}
			return _info;
		}

        protected PPInfo Load()
        {
            WorkSession.WriteStatus("PremiumPack.Load(\"" + account + "\") : enter");

            try
            {
                WorkSession.WriteStatus("PremiumPack.Load(\"" + account + "\") : open db connection");

                if (ServerConfiguration.IsLocalTestMode)
                {
                    using (var conn = new SQLiteSimpleConnection(ServerConfiguration.PremiumPackConnectionString))
                    using (var cmd = conn.GetDefaultSelectCommand(Tables.Shop.PremiumPack))
                    {
                        cmd.Where(Columns.PremiumPack.Id, account);

                        using (var reader = cmd.ExecuteReader())
                        {
                            return Load(reader);
                        }
                    }
                }
                else
                {
                    using (var conn = new MySqlSimpleConnection(ServerConfiguration.PremiumPackConnectionString))
                    using (var cmd = conn.GetDefaultSelectCommand(Tables.Shop.PremiumPack))
                    {
                        cmd.Where(Columns.PremiumPack.Id, account);

                        using (var reader = cmd.ExecuteReader())
                        {
                            return Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex2)
            {
                throw ex2;
            }
        }

        protected PPInfo Load(SimpleReader reader)
        {
            if (reader.Read() == false)
                return null;

            PPInfo pPInfo = new PPInfo();

            if (reader.GetDateTimeSafe(Columns.PremiumPack.InventoryPlus, out pPInfo.inventoryPlusExpiration))
            {
                pPInfo.canUseInventoryPlus = true;
            }
            if (reader.GetDateTimeSafe(Columns.PremiumPack.PremPack, out pPInfo.premiumPackExpiration))
            {
                pPInfo.canUsePremiumPack = true;
            }
            if (reader.GetDateTimeSafe(Columns.PremiumPack.VIP, out pPInfo.vipExpiration))
            {
                pPInfo.canUseVip = true;
            }
            if (reader.GetDateTimeSafe(Columns.PremiumPack.PremiumVIP, out pPInfo.primiumVipExpiration))
            {
                pPInfo.canUsePremiumVip = true;
            }
            if (reader.GetDateTimeSafe(Columns.PremiumPack.GuildPack, out pPInfo.guildpackExpiration))
            {
                pPInfo.canUseguildpack = true;
            }
            if (pPInfo.canUseInventoryPlus || pPInfo.canUsePremiumPack || pPInfo.canUseVip)
            {
                DateTime updated, check;
                if (reader.GetDateTimeSafe(Columns.PremiumPack.Updated, out updated))
                {
                    pPInfo.payCheck = true;
                }
                else if (reader.GetDateTimeSafe(Columns.PremiumPack.Checked, out check))
                {
                    pPInfo.payCheck = true;
                }
                else
                {
                    if (updated > check)
                    {
                        pPInfo.payCheck = true;
                    }
                    else
                    {
                        pPInfo.payCheck = false;
                    }
                }
            }
            return pPInfo;
        }

        public Message ToMessage()
		{
			WorkSession.WriteStatus("PremiumPack.ToMessage() : enter");
			if (ppInfo == null)
			{
				throw new Exception("Invalid premiumpack information");
			}
			Message message = new Message(0u, 0uL);
			message.WriteString(account);
			if (ppInfo.canUseInventoryPlus)
			{
				message.WriteU64((ulong)ppInfo.inventoryPlusExpiration.Ticks);
			}
			else
			{
				message.WriteU64(0uL);
			}
			if (ppInfo.canUsePremiumPack)
			{
				message.WriteU64((ulong)ppInfo.premiumPackExpiration.Ticks);
			}
			else
			{
				message.WriteU64(0uL);
			}
			if (ppInfo.canUseVip)
			{
				message.WriteU64((ulong)ppInfo.vipExpiration.Ticks);
			}
			else
			{
				message.WriteU64(0uL);
			}
			if (ppInfo.canUsePremiumVip)
			{
				message.WriteU64((ulong)ppInfo.primiumVipExpiration.Ticks);
			}
			else
			{
				message.WriteU64(0uL);
			}
			if (ppInfo.canUseguildpack)
			{
				message.WriteU64((ulong)ppInfo.guildpackExpiration.Ticks);
			}
			else
			{
				message.WriteU64(0uL);
			}
			message.WriteU8((byte)(ppInfo.payCheck ? 1 : 0));
			WorkSession.WriteStatus("PremiumPack.ToMessage() : complete, return msg");
			return message;
		}
	}
}
