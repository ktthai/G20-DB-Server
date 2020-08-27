using System;
using Mabinogi.SQL;

using Tables = Mabinogi.SQL.Tables;
using Columns = Mabinogi.SQL.Columns;
using System.Text.Json;

namespace Authenticator
{
    public class Coupon
    {
        public enum RESULT
        {
            FAIL,
            SUCCESS,
            NOT_VALID_STATE,
            EXPIRED,
            NOT_VALID_USER,
            USED,
            NOT_EXIST,
            SERVER_MISMATCH
        }

        protected static SimpleConnection Connection
        {
            get
            {
                if (ServerConfiguration.IsLocalTestMode)
                {
                    return new SQLiteSimpleConnection(ServerConfiguration.CouponConnectionString);
                }
                else
                {
                    return new MySqlSimpleConnection(ServerConfiguration.CouponConnectionString);
                }
            }
        }

        public static RESULT Begin_UsingTransaction(string _couponid, string _account, long _characterid, string _charactername, string _server, ref long _itemid, ref int _coupontype)
        {
            using (var conn = Connection)
            using (var trans = conn.BeginTransaction())
            {
                try
                {
                    RESULT result = RESULT.FAIL;
                    short state = -1;
                    int type = -1;
                    string[] validServers = null;
                    DateTime startValid = DateTime.MinValue;
                    DateTime endValid = DateTime.MinValue;

                    using (var stateCmd = conn.GetDefaultSelectCommand(Tables.Shop.Coupons, trans))
                    {
                        stateCmd.Where(Columns.Coupons.Code, _couponid);

                        stateCmd.Set(Columns.Coupons.State, 0);


                        using (var reader = stateCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader.GetInt16Safe(Columns.Coupons.State, out state) == false)
                                {
                                    state = -1;
                                }
                                else
                                {

                                    if (reader.GetInt32Safe(Columns.Coupons.Type, out type) == false)
                                        type = -1;
                                    string servers;
                                    if (reader.GetStringSafe(Columns.Coupons.ValidServer, out servers))
                                        validServers = JsonSerializer.Deserialize<string[]>(servers);
                                    reader.GetDateTimeSafe(Columns.Coupons.ValidServer, out startValid);
                                    reader.GetDateTimeSafe(Columns.Coupons.ValidServer, out endValid);
                                }
                            }
                        }
                    }

                    switch (state)
                    {
                        case -1:
                            {
                                result = RESULT.NOT_EXIST;
                            }
                            break;
                        case 1:
                            {
                                using (var cmd = conn.GetDefaultSelectCommand(Tables.Shop.Coupons, trans))
                                {
                                    cmd.Where(Columns.Coupons.Code, _couponid);
                                    cmd.Where(Columns.Coupons.Account, _account);
                                    cmd.Where(Columns.Coupons.EntityId, _characterid);
                                    cmd.Where(Columns.Coupons.Server, _server);

                                    using (var reader = cmd.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            _itemid = Convert.ToInt64(reader.GetString(Columns.Coupons.MainData));
                                            result = RESULT.NOT_VALID_STATE;
                                        }
                                        else
                                        {
                                            result = RESULT.USED;
                                        }
                                    }
                                }
                            }
                            break;
                        case 2:
                            {
                                result = RESULT.USED;
                            }
                            break;
                        case 0:
                            {
                                if (type != -1)
                                {
                                    if ((startValid == DateTime.MinValue || startValid < DateTime.Now) && (endValid == DateTime.MinValue || endValid > DateTime.Now))
                                    {
                                        if (validServers != null)
                                        {
                                            int i;
                                            for (i = 0; i < validServers.Length && !(validServers[i].ToLower() == _server.ToLower()); i++)
                                            {
                                            }
                                            if (i == validServers.Length)
                                            {
                                                result = RESULT.SERVER_MISMATCH;
                                                break;
                                            }
                                        }

                                        using (var cmd = conn.GetDefaultUpdateCommand(Tables.Shop.Coupons, trans))
                                        {
                                            cmd.Where(Columns.Coupons.Code, _couponid);

                                            cmd.Set(Columns.Coupons.Account, _account);
                                            cmd.Set(Columns.Coupons.EntityId, _characterid);
                                            cmd.Set(Columns.Coupons.EntityName, _charactername);
                                            cmd.Set(Columns.Coupons.Server, _server);
                                            cmd.Set(Columns.Coupons.MainData, Convert.ToString(_itemid));
                                            cmd.Set(Columns.Coupons.StartTime, DateTime.Now);
                                            cmd.Set(Columns.Coupons.State, 1);

                                            cmd.Execute();
                                            result = RESULT.SUCCESS;
                                            _coupontype = type;
                                        }

                                    }
                                    else
                                    {
                                        result = RESULT.EXPIRED;
                                    }
                                }
                            }
                            break;
                    }

                    trans.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    ExceptionMonitor.ExceptionRaised(ex);
                    trans.Rollback();
                    return RESULT.FAIL;
                }
            }
        }

        public static RESULT Commit_UsingTransaction(string _couponid, string _account, long _characterid, string _server)
        {
            WorkSession.WriteStatus("Coupon.Commit_UsingTransaction(\"" + _account + "\") : enter");

            WorkSession.WriteStatus("Coupon.Commit_UsingTransaction(\"" + _account + "\") : open db connection");
            RESULT result = RESULT.FAIL;

            using (var conn = Connection)
            using (var trans = conn.BeginTransaction())
            {
                try
                {
                    short state = -1;
                    string account = string.Empty;
                    long charId = -1;
                    string server = string.Empty;

                    using (var cmd = conn.GetDefaultSelectCommand(Tables.Shop.Coupons, trans))
                    {
                        cmd.Where(Columns.Coupons.Code, _couponid);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                reader.GetStringSafe(Columns.Coupons.Account, out account);
                                reader.GetStringSafe(Columns.Coupons.Server, out server);
                                reader.GetInt64Safe(Columns.Coupons.EntityId, out charId);
                                reader.GetInt16Safe(Columns.Coupons.State, out state);
                            }
                            else
                            {
                                result = RESULT.NOT_EXIST;
                            }
                        }
                    }

                    switch (state)
                    {
                        case 0:
                            result = RESULT.NOT_VALID_STATE;
                            break;
                        case 2:
                            result = RESULT.USED;
                            break;
                        case 1:
                            {
                                if ((account.ToLower() == _account.ToLower()) && (charId == _characterid) && (server.ToLower() == _server.ToLower()))
                                {
                                    using (var cmd = conn.GetDefaultUpdateCommand(Tables.Shop.Coupons, trans))
                                    {
                                        cmd.Where(Columns.Coupons.Code, _couponid);

                                        cmd.Set(Columns.Coupons.EndTime, DateTime.Now);
                                        cmd.Set(Columns.Coupons.State, 2);
                                        cmd.Execute();
                                    }
                                    result = RESULT.SUCCESS;
                                }
                                else
                                {
                                    result = RESULT.NOT_VALID_USER;
                                }
                            }
                            break;
                    }

                    trans.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex);
                    return RESULT.FAIL;
                }
            }
        }

        public static RESULT Rollback_UsingTransaction(string _couponid, string _account, long _characterid, string _server)
        {
            WorkSession.WriteStatus("Coupon.Rollback_UsingTransaction(\"" + _account + "\") : enter");


            RESULT result = RESULT.FAIL;

            using (var conn = Connection)
            using (var trans = conn.BeginTransaction())
            {
                try
                {
                    short state = -1;
                    short rollCount = 0;
                    string account = string.Empty;
                    long charId = -1;
                    string server = string.Empty;

                    using (var cmd = conn.GetDefaultSelectCommand(Tables.Shop.Coupons, trans))
                    {
                        cmd.Where(Columns.Coupons.Code, _couponid);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                reader.GetStringSafe(Columns.Coupons.Account, out account);
                                reader.GetStringSafe(Columns.Coupons.Server, out server);
                                reader.GetInt64Safe(Columns.Coupons.EntityId, out charId);
                                reader.GetInt16Safe(Columns.Coupons.State, out state);
                            }
                            else
                            {
                                result = RESULT.NOT_EXIST;
                            }
                        }
                    }

                    switch (state)
                    {
                        case 0:
                            result = RESULT.NOT_VALID_STATE;
                            break;
                        case 2:
                            result = RESULT.USED;
                            break;
                        case 1:
                            {
                                if ((account.ToLower() == _account.ToLower()) && (charId == _characterid) && (server.ToLower() == _server.ToLower()))
                                {
                                    using (var cmd = conn.GetDefaultUpdateCommand(Tables.Shop.Coupons, trans))
                                    {
                                        cmd.Where(Columns.Coupons.Code, _couponid);

                                        cmd.Set(Columns.Coupons.RollbackCount, rollCount + 1);
                                        cmd.Set(Columns.Coupons.State, 0);
                                        cmd.Execute();
                                    }
                                    result = RESULT.SUCCESS;
                                }
                                else
                                {
                                    result = RESULT.NOT_VALID_USER;
                                }
                            }
                            break;
                    }

                    trans.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    ExceptionMonitor.ExceptionRaised(ex);
                    return RESULT.FAIL;
                }
            }
        }
    }
}
