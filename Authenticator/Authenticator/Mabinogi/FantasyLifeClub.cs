using Mabinogi;
using Mabinogi.SQL;
using System;

using Tables = Mabinogi.SQL.Tables;
using Columns = Mabinogi.SQL.Columns;

namespace Authenticator
{
	public class FantasyLifeClub
	{
		protected class FLInfo
		{
			public bool canUseNaoSupport;

			public DateTime naoSupportExpiration = DateTime.MinValue;

			public bool canUseStorage;

			public DateTime storageExpiration = DateTime.MinValue;

			public bool canUseAdvancedPlay;

			public DateTime advancedPlayExpiration = DateTime.MinValue;

			public bool payCheck;
		}

		protected string account;

		protected FLInfo flInfo;

		protected FantasyLifeClub(string _account)
		{
			account = _account;
			flInfo = null;
		}

		public static FantasyLifeClub GetFantasyLifeClub(string _account, bool _bTestMode)
		{
			return new FantasyLifeClub(_account);
		}

		public bool Process()
		{
			flInfo = Load();
			flInfo = ApplyFantasylifeclubEvent(flInfo, ServerConfiguration.FantasyLifeClubEvent);
			return flInfo != null;
		}

		private FLInfo ApplyFantasylifeclubEvent(FLInfo _info, EventFantasylifeclub _event)
		{
			DateTime now = DateTime.Now;
			if (_event != null && now >= _event.eventStart && now <= _event.eventEnd)
			{
				if (_info == null)
				{
					_info = new FLInfo();
				}
				if (!_info.canUseAdvancedPlay || _info.advancedPlayExpiration < _event.eventEnd)
				{
					_info.canUseAdvancedPlay = true;
					_info.advancedPlayExpiration = _event.eventEnd;
				}
				if (!_info.canUseNaoSupport || _info.naoSupportExpiration < _event.eventEnd)
				{
					_info.canUseNaoSupport = true;
					_info.naoSupportExpiration = _event.eventEnd;
				}
				if (!_info.canUseStorage || _info.storageExpiration < _event.eventEnd)
				{
					_info.canUseStorage = true;
					_info.storageExpiration = _event.eventEnd;
				}
				return _info;
			}
			return _info;
		}

        protected FLInfo Load()
        {
            try
            {
                WorkSession.WriteStatus("FantasyLifeClub.Load(\"" + account + "\") : open db connection");

                if (ServerConfiguration.IsLocalTestMode)
                {
                    using (var conn = new SQLiteSimpleConnection(ServerConfiguration.FantasyLifeClubConnectionString))
                    using (var cmd = conn.GetDefaultSelectCommand(Tables.Shop.FantasyLifeClub))
                    {
                        cmd.Where(Columns.FantasyLifeClub.Id, account);

                        return Load(cmd.ExecuteReader());
                    }
                }
                else
                {
                    using (var conn = new MySqlSimpleConnection(ServerConfiguration.FantasyLifeClubConnectionString))
                    using (var cmd = conn.GetDefaultSelectCommand(Tables.Shop.FantasyLifeClub))
                    {
                        cmd.Where(Columns.FantasyLifeClub.Id, account);

                        return Load(cmd.ExecuteReader());
                    }
                }
            }
            catch (Exception ex2)
            {

                WorkSession.WriteStatus("FantasyLifeClub.Load(\"" + account + "\") : rollback query");


                throw ex2;
            }
        }

        protected FLInfo Load(SimpleReader reader)
        {
			FLInfo result = null;
			if (reader.Read())
            {
				result = new FLInfo();

				
				if (reader.GetDateTimeSafe(Columns.FantasyLifeClub.NaoSupportExpiration, out result.naoSupportExpiration))
                {
					result.canUseNaoSupport = true;
                }
                if (reader.GetDateTimeSafe(Columns.FantasyLifeClub.StorageExpiration, out result.storageExpiration))
                {
                    result.canUseStorage = true;
                }
                if (reader.GetDateTimeSafe(Columns.FantasyLifeClub.AdvancedPlayExpiration, out result.advancedPlayExpiration))
                {
                    result.canUseAdvancedPlay = true;
                }

                if (result.canUseNaoSupport || result.canUseStorage || result.canUseAdvancedPlay)
                {
					DateTime updated;
					DateTime check;
					if (reader.GetDateTimeSafe(Columns.FantasyLifeClub.Updated, out updated) == false)
                    {
                        result.payCheck = true;
                    }
                    else if (reader.GetDateTimeSafe(Columns.FantasyLifeClub.Checked, out check) == false )
                    {
                        result.payCheck = true;
                    }
                    else
                    {
                        if (updated > check)
                        {
                            result.payCheck = true;
                        }
                        else
                        {
                            result.payCheck = false;
                        }
                    }
                }
            }

			return result;
        }

		public Message ToMessage()
		{
			WorkSession.WriteStatus("FantasyLifeClub.ToMessage() : enter");
			if (flInfo == null)
			{
				throw new Exception("Invalid fantasylifeclub information");
			}
			Message message = new Message(0u, 0uL);
			message.WriteString(account);
			if (flInfo.canUseNaoSupport)
			{
				message.WriteU64((ulong)flInfo.naoSupportExpiration.Ticks);
			}
			else
			{
				message.WriteU64(0uL);
			}
			if (flInfo.canUseStorage)
			{
				message.WriteU64((ulong)flInfo.storageExpiration.Ticks);
			}
			else
			{
				message.WriteU64(0uL);
			}
			if (flInfo.canUseAdvancedPlay)
			{
				message.WriteU64((ulong)flInfo.advancedPlayExpiration.Ticks);
			}
			else
			{
				message.WriteU64(0uL);
			}
			message.WriteU8((byte)(flInfo.payCheck ? 1 : 0));
			WorkSession.WriteStatus("FantasyLifeClub.ToMessage() : complete, return msg");
			return message;
		}
	}
}
