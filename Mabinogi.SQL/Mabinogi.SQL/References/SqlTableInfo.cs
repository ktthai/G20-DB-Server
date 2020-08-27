using System;
using System.Collections.Generic;
using System.Diagnostics;
using Col = Mabinogi.SQL.Columns;

namespace Mabinogi.SQL
{
    public class ColumnInfo
    {
        public string Name;
        public Type Type;
        public bool IsNullable;
        public int Size;
        public string DefaultValue;
        public int Options;


        public string TypeToString(bool local)
        {
            string result = null;
            if (Type == typeof(int))
            {
                result = "int";
            }

            if (Type == typeof(long))
            {
                result = "bigint";
            }

            if (Type == typeof(short))
            {
                result = "smallint";
            }

            if (Type == typeof(byte))
            {
                result = "tinyint UNSIGNED";
            }

            if (Type == typeof(char))
            {
                result = string.Format("varchar({0})", Size);
            }

            if (Type == typeof(string))
            {
                result = "text";
            }

            if (Type == typeof(float))
            {
                result = "float";
            }

            if (Type == typeof(double))
            {
                result = "double";
            }
            if (Type == typeof(bool))
            {
                result = "boolean";
            }

            if (Type == typeof(DateTime))
            {
                result = "datetime";
            }

            if(result == null)
                throw new Exception(string.Format("Unexpected type {0} in ColumnInfo!", Type.ToString()));

            if (Options == 1)
                result += " AUTO_INCREMENT";

            return result;
            
        }

        internal string DefaultToString(bool local = false)
        {
            string result;

            if (!IsNullable)
                result = "NOT NULL";
            else
                result = "NULL";

            if (DefaultValue != null && DefaultValue != string.Empty)
            {
                if (local == false && DefaultValue == "CURRENT_TIMESTAMP")
                {
                    result += string.Format(" DEFAULT {0}()", DefaultValue);
                }
                else
                {
                    result += string.Format(" DEFAULT '{0}'", DefaultValue);
                }
            }

            return result;
        }
    }

    public class ForeignKey
    {
        public string Table;
        public string[] ForeignColumns;
        public string[] LocalColumns;
        public bool Delete;
        public bool Update;
        public bool Check;

        public ForeignKey(string table, string[] columns, string[] localColumns, bool check, bool delete = false, bool update = false)
        {
            Table = table;
            ForeignColumns = columns;
            LocalColumns = localColumns;
            Check = check;
            Update = update;
            Delete = delete;
        }
    }

    public abstract class BaseTable
    {
        public string DBName => _dbName;
        public string TableName => _tableName;
        public List<ColumnInfo> Columns => _columns;
        public string[] PrimaryKey => _primaryKey;
        public List<string[]> Keys => _keys;
        public ForeignKey[] ForeignKeys => _foreignKeys;


        protected string _tableName;
        protected string _dbName;
        protected List<ColumnInfo> _columns;
        protected string[] _primaryKey;
        protected List<string[]> _keys;
        protected ForeignKey[] _foreignKeys;
    }


    #region MabinogiDB
    public class AccountTable : BaseTable
    {
        public AccountTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.Account;
            _dbName = "Mabinogi";

            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.Account.Id, IsNullable = false, Type = typeof(char), Size = 50},
                new ColumnInfo() { Name = Col.Account.Password, IsNullable = false, Type = typeof(char), Size = 50},
                new ColumnInfo() { Name = Col.Account.Name, IsNullable = false, Type = typeof(char), Size = 50},
                new ColumnInfo() { Name = Col.Account.Email, IsNullable = false, Type = typeof(char), Size = 50},
                new ColumnInfo() { Name = Col.Account.Flag, IsNullable = false, Type = typeof(short)},
                new ColumnInfo() { Name = Col.Account.BlockingDate, IsNullable = false, Type = typeof(DateTime)},
                new ColumnInfo() { Name = Col.Account.BlockingDuration, IsNullable = false, Type = typeof(short)},
                new ColumnInfo() { Name = Col.Account.Authority, IsNullable = false, Type = typeof(byte), DefaultValue = "0"},
                new ColumnInfo() { Name = Col.Account.ProviderCode, IsNullable = true, Type = typeof(short)},
                new ColumnInfo() { Name = Col.Account.MachineIDs, IsNullable = false, Type = typeof(char), Size = 104, DefaultValue = ""},
            };

            _primaryKey = new string[] { Col.Account.Id };
        }
    }

    public class AccountAuthTable : BaseTable
    {

        public AccountAuthTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.AcAuth;
            _dbName = "Mabinogi";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.AcAuth.Id, IsNullable = false, Type = typeof(char), Size = 255},
                new ColumnInfo() { Name = Col.AcAuth.Authority, IsNullable = false, Type = typeof(byte), Size = 50},
                new ColumnInfo() { Name = Col.AcAuth.AuthDesc, IsNullable = false, Type = typeof(char), Size = 255},
            };

            _primaryKey = new string[] { Col.AcAuth.Id };
        }
    }

    public class AccountCacheTable : BaseTable
    {

        public AccountCacheTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.AccountCache;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.AccountCache.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.AccountCache.Key, IsNullable = false, Type = typeof(int) },
                };

            _primaryKey = new string[] { Col.AccountCache.Account };
        }
    }

    public class AccountCharacterLinkedApTable : BaseTable
    {
        public AccountCharacterLinkedApTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.AccountCharacterLinkedAP;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.AccountCharacterLinkedAP.ServerId, IsNullable = false, Type = typeof(char), Size = 20 },
                new ColumnInfo() { Name = Col.AccountCharacterLinkedAP.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.AccountCharacterLinkedAP.SavedAp, IsNullable = true, Type = typeof(int) },
                new ColumnInfo() { Name = Col.AccountCharacterLinkedAP.TermAp, IsNullable = true, Type = typeof(int) },
                new ColumnInfo() { Name = Col.AccountCharacterLinkedAP.ResetTime, IsNullable = true, Type = typeof(long) },
                };

            _primaryKey = new string[] { Col.AccountCharacterLinkedAP.ServerId, Col.AccountCharacterLinkedAP.CharId };
        }
    }

    public class AccountCharacterRefTable : BaseTable
    {

        public AccountCharacterRefTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.AccountCharacterRef;
            _dbName = "Mabinogi";
            _columns =
        new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.AccountCharacterRef.Id, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.AccountCharacterRef.CharacterId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.AccountCharacterRef.CharacterName, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.AccountCharacterRef.Server, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.AccountCharacterRef.Deleted, IsNullable = false, Type = typeof(long), DefaultValue = "0"  },
                new ColumnInfo() { Name = Col.AccountCharacterRef.GroupId, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.AccountCharacterRef.Race, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.AccountCharacterRef.SupportCharacter, IsNullable = false, Type = typeof(bool), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.AccountCharacterRef.Tab, IsNullable = false, Type = typeof(bool), DefaultValue = "0" },
            };

            _keys = new List<string[]>()
            {
                new string[] { Col.AccountCharacterRef.Id },
                new string[]{ Col.AccountCharacterRef.Id, Col.AccountCharacterRef.CharacterId, Col.AccountCharacterRef.CharacterName, Col.AccountCharacterRef.Server, Col.AccountCharacterRef.Deleted },
        new string[] { Col.AccountCharacterRef.Id, Col.AccountCharacterRef.Server }
            };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.AccountRef, new string[] { Col.AccountRef.Id }, new string[] { Col.AccountCharacterRef.Id }, true, true) };
        }
    }

    public class AccountLogTable : BaseTable
    {
        public AccountLogTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.AccountLog;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.AccountLog.Account, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.AccountLog.Address, IsNullable = true, Type = typeof(char), Size = 16 },
                new ColumnInfo() { Name = Col.AccountLog.IntISPCode, IsNullable = true, Type = typeof(int) },
                new ColumnInfo() { Name = Col.AccountLog.RegDate, IsNullable = true, Type = typeof(DateTime) },
                };

            _primaryKey = new string[] { Col.AccountLog.Account };
        }
    }

    public class AccountMetaTable : BaseTable
    {
        public AccountMetaTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.AccountMeta;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.AccountMeta.Id, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.AccountMeta.MCode, IsNullable = false, Type = typeof(char), Size = 30 },
                new ColumnInfo() { Name = Col.AccountMeta.MType, IsNullable = false, Type = typeof(char), Size = 10 },
                new ColumnInfo() { Name = Col.AccountMeta.MData, IsNullable = false, Type = typeof(char), Size = 30 },
                };

            _primaryKey = new string[] { Col.AccountMeta.Id, Col.AccountMeta.MCode };
        }
    }

    public class AccountPawnTable : BaseTable
    {

        public AccountPawnTable()
        {

            _tableName = Mabinogi.SQL.Tables.Mabinogi.AccountPawnCoin;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.AccountPawnCoin.IdAccount, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.AccountPawnCoin.PawnCoin, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.AccountPawnCoin.UpdateDate, IsNullable = false, Type = typeof(DateTime) },
                };


            _primaryKey = new string[] { Col.AccountPawnCoin.IdAccount };
        }
    }

    public class AccountPetRefTable : BaseTable
    {

        public AccountPetRefTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.AccountPetRef;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.AccountPetRef.Id, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.AccountPetRef.PetId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.AccountPetRef.PetName, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.AccountPetRef.Server, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.AccountPetRef.Deleted, IsNullable = false, Type = typeof(long), DefaultValue = "0"  },

                new ColumnInfo() { Name = Col.AccountPetRef.RemainTime, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.AccountPetRef.LastTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.AccountPetRef.GroupId, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.AccountPetRef.Tab, IsNullable = false, Type = typeof(bool), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.AccountPetRef.ExpireTime, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
                };

            _keys = new List<string[]>() {
                new string[] { Col.AccountPetRef.Id },
                new string[]{ Col.AccountPetRef.Id, Col.AccountPetRef.PetId, Col.AccountPetRef.PetName, Col.AccountPetRef.Server, Col.AccountPetRef.Deleted },
        new string[] { Col.AccountPetRef.Id, Col.AccountPetRef.Server } };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.AccountRef, new string[] { Col.AccountRef.Id }, new string[] { Col.AccountPetRef.Id }, true, true) };
        }
    }

    public class AccountSessionTable : BaseTable
    {
        public AccountSessionTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.AccountSession;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.AccountSession.Account, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.AccountSession.Session, IsNullable = false, Type = typeof(long) },
                };


            _primaryKey = new string[] { Col.AccountSession.Account };
        }
    }

    public class AccountLoginHistoryTable : BaseTable
    {
        public AccountLoginHistoryTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.AccountLoginHistory;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.AccountLoginHistory.Id, IsNullable = true, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.AccountLoginHistory.LoginCount, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.AccountLoginHistory.LoginTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
                new ColumnInfo() { Name = Col.AccountLoginHistory.LastLoginTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
                };
        }
    }

    public class AccountRefTable : BaseTable
    {

        public AccountRefTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.AccountRef;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.AccountRef.Id, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.AccountRef.Flag, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.AccountRef.MaxSlot, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.AccountRef.In, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.AccountRef.Out, IsNullable = false, Type = typeof(DateTime) },

                new ColumnInfo() { Name = Col.AccountRef.PlayableTime, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.AccountRef.SupportRace, IsNullable = false, Type = typeof(byte), DefaultValue = "0"  },
                new ColumnInfo() { Name = Col.AccountRef.SupportRewardState, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.AccountRef.LobbyOption, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.AccountRef.SupportLastChangeTime, IsNullable = false, Type = typeof(int), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.AccountRef.MacroCheckFailure, IsNullable = false, Type = typeof(byte), DefaultValue = "0"  },
                new ColumnInfo() { Name = Col.AccountRef.BeginnerFlag, IsNullable = false, Type = typeof(bool), DefaultValue = "1"  },
                new ColumnInfo() { Name = Col.AccountRef.MacroCheckSuccess, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.AccountRef.Ip, IsNullable = false, Type = typeof(char), Size = 15 },
                new ColumnInfo() { Name = Col.AccountRef.MachineId, IsNullable = false, Type = typeof(char), Size = 32 },
                };


            _primaryKey = new string[] { Col.AccountRef.Id };
        }
    }

    public class AssetRankTable : BaseTable
    {

        public AssetRankTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.AssetRank;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.AssetRank.Num, IsNullable = false, Type = typeof(int), Options = 1 }, // AUTO_INCREMENT
                new ColumnInfo() { Name = Col.AssetRank.RegDate, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.AssetRank.Id, IsNullable = true, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.AssetRank.ChWealth, IsNullable = true, Type = typeof(int) },
                new ColumnInfo() { Name = Col.AssetRank.BankDeposit, IsNullable = true, Type = typeof(int) },

                new ColumnInfo() { Name = Col.AssetRank.BankWealth, IsNullable = true, Type = typeof(int) },
                new ColumnInfo() { Name = Col.AssetRank.TotalAsset, IsNullable = true, Type = typeof(int) },
                new ColumnInfo() { Name = Col.AssetRank.Rank, IsNullable = true, Type = typeof(byte) },
                };


            _primaryKey = new string[] { Col.AssetRank.Num };
            _keys = new List<string[]>() { new string[] { Col.AssetRank.RegDate } };
        }
    }

    public class BankTable : BaseTable
    {

        public BankTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.Bank;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.Bank.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.Bank.Deposit, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Bank.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Bank.Password, IsNullable = false, Type = typeof(char), Size = 50, DefaultValue = "" },
                new ColumnInfo() { Name = Col.Bank.HumanWealth, IsNullable = false, Type = typeof(int), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Bank.ElfWealth, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Bank.GiantWealth, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Bank.CouponCode, IsNullable = false, Type = typeof(char), Size = 40, DefaultValue = "" },
                };


            _primaryKey = new string[] { Col.Bank.Account };
        }
    }

    public class BankSlotTable : BaseTable
    {
        public BankSlotTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.BankSlot;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.BankSlot.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.BankSlot.Name, IsNullable = false, Type = typeof(char), Size = 100 },
                new ColumnInfo() { Name = Col.BankSlot.Valid, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.BankSlot.Race, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.BankSlot.CouponCode, IsNullable = false, Type = typeof(char), Size = 40, DefaultValue = "" },

                new ColumnInfo() { Name = Col.BankSlot.UpdateTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "1999-01-01 00:00:01" },
                new ColumnInfo() { Name = Col.BankSlot.ReceiveTime, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                };

            _keys = new List<string[]>() { new string[] { Col.BankSlot.Name }, new string[] { Col.BankSlot.Account, Col.BankSlot.Name }, new string[] { Col.BankSlot.Account, Col.BankSlot.Race } };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.Bank, new string[] { Col.Bank.Account }, new string[] { Col.BankSlot.Account }, true, true) };
        }
    }

    public class BankItemHugeTable : BaseTable
    {

        public BankItemHugeTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.BankItemHuge;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.BankItem.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.BankItem.SlotName, IsNullable = false, Type = typeof(char), Size = 100 },
                new ColumnInfo() { Name = Col.BankItem.Location, IsNullable = false, Type = typeof(char), Size = 250 },
                new ColumnInfo() { Name = Col.BankItem.ExtraTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.BankItem.Time, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.Item.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "1" },
                new ColumnInfo() { Name = Col.Item.Class, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosX, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosY, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Bundle, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Item.LinkedPocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.DurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.OriginalDurabilityMax, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Data, IsNullable = false, Type = typeof(char), Size = 3000 },
                new ColumnInfo() { Name = Col.Item.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.Expiration, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.BankItem.Race, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                };


            _primaryKey = new string[] { Col.Item.ItemId, Col.Item.ItemLoc };
            _keys = new List<string[]>() {
                new string[] { Col.BankItem.Account, Col.BankItem.Race }
            };

            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.BankSlot, new string[] { Col.BankSlot.Account, Col.BankSlot.Name }, new string[] { Col.BankItem.Account, Col.BankItem.SlotName },  true, true),
            };
        }
    }

    public class BankItemLargeTable : BaseTable
    {
        public BankItemLargeTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.BankItemLarge;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.BankItem.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.BankItem.SlotName, IsNullable = false, Type = typeof(char), Size = 100 },
                new ColumnInfo() { Name = Col.BankItem.Location, IsNullable = false, Type = typeof(char), Size = 250 },
                new ColumnInfo() { Name = Col.BankItem.ExtraTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.BankItem.Time, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.Item.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "1" },
                new ColumnInfo() { Name = Col.Item.Class, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosX, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosY, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Bundle, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Item.LinkedPocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Figure, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.DurabilityMax, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.OriginalDurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.AttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.AttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.WAttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.WAttackMax, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Item.Balance, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Critical, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Defence, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Protect, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.EffectiveRange, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Item.AttackSpeed, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.DownHitCount, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Experience, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.ExpPoint, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Upgraded, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.UpgradeMax, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Grade, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Prefix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.Suffix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.Data, IsNullable = false, Type = typeof(char), Size = 600 },

                new ColumnInfo() { Name = Col.Item.Option, IsNullable = false, Type = typeof(char), Size = 4000 },
                new ColumnInfo() { Name = Col.Item.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.Expiration, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.BankItem.Race, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                };


            _primaryKey = new string[] { Col.Item.ItemId, Col.Item.ItemLoc };
            _keys = new List<string[]>() {
                new string[] { Col.BankItem.Account, Col.BankItem.Race }
            };

            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey( Mabinogi.SQL.Tables.Mabinogi.BankSlot, new string[] { Col.BankSlot.Account, Col.BankSlot.Name }, new string[] { Col.BankItem.Account, Col.BankItem.SlotName }, true, true),
            };
        }
    }

    public class BankItemQuestTable : BaseTable
    {
        public BankItemQuestTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.BankItemQuest;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.BankItem.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.BankItem.SlotName, IsNullable = false, Type = typeof(char), Size = 100 },
                new ColumnInfo() { Name = Col.BankItem.Location, IsNullable = false, Type = typeof(char), Size = 250 },
                new ColumnInfo() { Name = Col.BankItem.ExtraTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.BankItem.Time, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.Item.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "1" },
                new ColumnInfo() { Name = Col.Item.Quest, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.Class, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosX, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.PosY, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Price, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Bundle, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.LinkedPocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.TemplateId, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Complete, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.StartTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.Data, IsNullable = false, Type = typeof(char), Size = 200 },
                new ColumnInfo() { Name = Col.Item.Objective, IsNullable = false, Type = typeof(char), Size = 1500 },
                new ColumnInfo() { Name = Col.Item.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Item.Expiration, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.BankItem.Race, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                };


            _primaryKey = new string[] { Col.Item.ItemId, Col.Item.ItemLoc };
            _keys = new List<string[]>() {
                new string[] { Col.BankItem.Account, Col.BankItem.Race }
            };

            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey( Mabinogi.SQL.Tables.Mabinogi.BankSlot,  new string[] { Col.BankSlot.Account, Col.BankSlot.Name }, new string[] { Col.BankItem.Account, Col.BankItem.SlotName }, true, true),
            };
        }
    }

    public class BankItemSmallTable : BaseTable
    {
        public BankItemSmallTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.BankItemSmall;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.BankItem.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.BankItem.SlotName, IsNullable = false, Type = typeof(char), Size = 100 },
                new ColumnInfo() { Name = Col.BankItem.Location, IsNullable = false, Type = typeof(char), Size = 250 },
                new ColumnInfo() { Name = Col.BankItem.ExtraTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.BankItem.Time, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.Item.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "1" },
                new ColumnInfo() { Name = Col.Item.Class, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosX, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosY, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Bundle, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Item.LinkedPocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.Expiration, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.BankItem.Race, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                };


            _primaryKey = new string[] { Col.Item.ItemId, Col.Item.ItemLoc };
            _keys = new List<string[]>() {
                new string[] { Col.BankItem.Account, Col.BankItem.Race }
            };

            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey( Mabinogi.SQL.Tables.Mabinogi.BankSlot,  new string[] { Col.BankSlot.Account, Col.BankSlot.Name }, new string[] { Col.BankItem.Account, Col.BankItem.SlotName },  true, true),
            };
        }
    }

    public class BidTable : BaseTable
    {
        public BidTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.Bid;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.Bid.BidId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Bid.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Bid.CharName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.Bid.AuctionItemId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Bid.Price, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Bid.Time, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Bid.BidState, IsNullable = false, Type = typeof(byte)},
                };


            _primaryKey = new string[] { Col.Bid.BidId };
        }
    }

    public class BidIdPoolTable : BaseTable
    {
        public BidIdPoolTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.BidIdPool;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.BidIdPool.Count, IsNullable = false, Type = typeof(long) }
                };
        }
    }

    public class CastleTable : BaseTable
    {
        public CastleTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.Castle;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.Castle.CastleId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Castle.GuildId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Castle.Constructed, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Castle.CastleMoney, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Castle.WeeklyIncome, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Castle.TaxRate, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Castle.UpdateTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Castle.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Castle.MaxDurability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Castle.BuildState, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Castle.BuildNextTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Castle.BuildStep, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Castle.Flag, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Castle.SellDungeonPass, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Castle.DungeonPassPrice, IsNullable = false, Type = typeof(int), DefaultValue = "1000" },
                };


            _primaryKey = new string[] { Col.Castle.CastleId };
        }
    }

    public class CastleBlockTable : BaseTable
    {
        public CastleBlockTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CastleBlock;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CastleBlock.CastleId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CastleBlock.GameName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.CastleBlock.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CastleBlock.Entry, IsNullable = false, Type = typeof(byte) },
                };

            _keys = new List<string[]>() { new string[] { Col.CastleBlock.CastleId }, new string[] { Col.CastleBlock.GameName } };
        }
    }

    public class CastleBuildResourceTable : BaseTable
    {
        public CastleBuildResourceTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CastleBuildResource;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CastleBuildResource.CastleId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CastleBuildResource.ClassId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CastleBuildResource.CurrentAmount, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CastleBuildResource.MaxAmount, IsNullable = false, Type = typeof(int) },
                };


            _keys = new List<string[]>() { new string[] { Col.CastleBuildResource.CastleId } };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.Castle, new string[] { Col.Castle.CastleId }, new string[] { Col.CastleBuildResource.CastleId }, true, true), };
        }
    }

    public class CastleBidTable : BaseTable
    {
        public CastleBidTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CastleBid;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CastleBid.CastleId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CastleBid.BidStartTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
                new ColumnInfo() { Name = Col.CastleBid.BidEndTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.CastleBid.MinBidPrice, IsNullable = false, Type = typeof(int) },
                };


            _primaryKey = new string[] { Col.CastleBid.CastleId };
        }
    }

    public class CastleBidderTable : BaseTable
    {
        public CastleBidderTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CastleBidder;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CastleBidder.CastleId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CastleBidder.BidGuildId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CastleBidder.BidGuildName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.CastleBidder.BidPrice, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CastleBidder.BidOrder, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.CastleBidder.BidCharacterID, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CastleBidder.BidCharName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.CastleBidder.BidTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.CastleBidder.BidUpdateTime, IsNullable = true, Type = typeof(DateTime) },
                };


            _primaryKey = new string[] { Col.CastleBidder.BidGuildId };
            _keys = new List<string[]>() { new string[] { Col.CastleBidder.CastleId } };
        }
    }

    public class CastleBidderHistoryTable : BaseTable
    {
        public CastleBidderHistoryTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CastleBidderHistory;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CastleBidderHistory.CastleId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CastleBidderHistory.BidGuildId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CastleBidderHistory.BidGuildName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.CastleBidderHistory.BidPrice, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CastleBidderHistory.BidOrder, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.CastleBidderHistory.BidCharacterID, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CastleBidderHistory.BidCharName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.CastleBidderHistory.BidTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.CastleBidderHistory.BidUpdateTime, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.CastleBidderHistory.RefundTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP"  },

                new ColumnInfo() { Name = Col.CastleBidderHistory.Flag, IsNullable = true, Type = typeof(byte) },
                };


            _keys = new List<string[]>() { new string[] { Col.CastleBidderHistory.CastleId } };
        }
    }

    public class ChannelingKeyPoolTable : BaseTable
    {
        public ChannelingKeyPoolTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ChannelingKeyPool;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.ChannelingKeyPool.ProviderCode, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ChannelingKeyPool.InsertDate, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.ChannelingKeyPool.KeyString, IsNullable = false, Type = typeof(char), Size = 128 },
                };

            _primaryKey = new string[] { Col.ChannelingKeyPool.ProviderCode, Col.ChannelingKeyPool.KeyString };
            _keys = new List<string[]>() { new string[] { Col.ChannelingKeyPool.InsertDate } };
        }
    }

    public class CharDeletedRefSyncTable : BaseTable
    {
        public CharDeletedRefSyncTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharDeletedRefSync;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CharDeletedRefSync.Id, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.CharDeletedRefSync.CharacterId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharDeletedRefSync.CharacterName, IsNullable = false, Type = typeof(char), Size = 64 },
                new ColumnInfo() { Name = Col.CharDeletedRefSync.Server, IsNullable = false, Type = typeof(char), Size = 16 },
                new ColumnInfo() { Name = Col.CharDeletedRefSync.DeletedTime, IsNullable = false, Type = typeof(DateTime), },
                };

            _keys = new List<string[]>() { new string[] { Col.CharDeletedRefSync.DeletedTime }, new string[] { Col.CharDeletedRefSync.Id } };
        }
    }

    public class CharRefSyncTable : BaseTable
    {
        public CharRefSyncTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharRefSync;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CharRefSync.Id, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.CharRefSync.CharacterId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharRefSync.CharacterName, IsNullable = false, Type = typeof(char), Size = 64 },
                new ColumnInfo() { Name = Col.CharRefSync.Server, IsNullable = false, Type = typeof(char), Size = 16 },
                };

            _keys = new List<string[]>() { new string[] { Col.CharRefSync.Id }, new string[] { Col.CharRefSync.CharacterId, Col.CharRefSync.Server }, new string[] { Col.CharRefSync.CharacterName, Col.CharRefSync.Server } };
        }
    }

    public class ChararacterTable : BaseTable
    {
        public ChararacterTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.Character;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.Character.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Character.Name, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.Character.Type, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Character.SkinColor, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.EyeType, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Character.EyeColor, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.MouthType, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.Status, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Character.Height, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.Fatness, IsNullable = false, Type = typeof(double) },

                new ColumnInfo() { Name = Col.Character.Upper, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.Lower, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.Region, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Character.X, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Character.Y, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Character.Direction, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.BattleState, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Character.WeaponSet, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.Life, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.LifeDamage, IsNullable = false, Type = typeof(double) },

                new ColumnInfo() { Name = Col.Character.LifeMax, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.Mana, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.ManaMax, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.Stamina, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.StaminaMax, IsNullable = false, Type = typeof(double) },

                new ColumnInfo() { Name = Col.Character.Food, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.Level, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Character.CumulatedLevel, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Character.Experience, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Character.Age, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Character.Strength, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.Dexterity, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.Intelligence, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.Will, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.Luck, IsNullable = false, Type = typeof(double) },

                new ColumnInfo() { Name = Col.Character.AbilityRemain, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Character.AttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Character.AttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Character.WAttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Character.WAttackMax, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Character.Critical, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.Protect, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.Defense, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Character.Rate, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Character.StrengthBoost, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Character.DexterityBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.IntelligenceBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.WillBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.LuckBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.HeightBoost, IsNullable = false, Type = typeof(byte) },


                new ColumnInfo() { Name = Col.Character.FatnessBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.UpperBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.LowerBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.LifeBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.ManaBoost, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Character.StaminaBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Character.Toxic, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.ToxicDrunkenTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Character.ToxicStrength, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.ToxicIntelligence, IsNullable = false, Type = typeof(double) },

                new ColumnInfo() { Name = Col.Character.ToxicDexterity, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.ToxicWill, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.ToxicLuck, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Character.LastTown, IsNullable = false, Type = typeof(char), Size = 100 },
                new ColumnInfo() { Name = Col.Character.LastDungeon, IsNullable = false, Type = typeof(char), Size = 100 },

                new ColumnInfo() { Name = Col.Character.NaoMemory, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Character.NaoFavor, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Character.Birthday, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Character.Playtime, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Character.Wealth, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Character.Condition, IsNullable = false, Type = typeof(char), Size = 1000 },
                new ColumnInfo() { Name = Col.Character.Collection, IsNullable = false, Type = typeof(string) },
                new ColumnInfo() { Name = Col.Character.History, IsNullable = false, Type = typeof(char), Size = 500 },
                new ColumnInfo() { Name = Col.Character.Memory, IsNullable = false, Type = typeof(char), Size = 1100 },
                new ColumnInfo() { Name = Col.Character.Title, IsNullable = false, Type = typeof(string) },

                new ColumnInfo() { Name = Col.Character.Reserved, IsNullable = false, Type = typeof(string) },
                new ColumnInfo() { Name = Col.Character.Book, IsNullable = false, Type = typeof(string) },
                new ColumnInfo() { Name = Col.Character.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Character.DeleteTime, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Character.MaxLevel, IsNullable = false, Type = typeof(short), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Character.RebirthCount, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.LifetimeSkill, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.NsRespawnCount, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.NsLastRespawnDay, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.NsGiftReceiveDay, IsNullable = false, Type = typeof(int), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Character.ApGiftReceiveDay, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.Rank1, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.Rank2, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.RebirthDay, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Character.RebirthAge, IsNullable = false, Type = typeof(short), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Character.NaoStyle, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.Score, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.MateId, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.MateName, IsNullable = false, Type = typeof(char), Size = 20, DefaultValue = "" },
                new ColumnInfo() { Name = Col.Character.MarriageTime, IsNullable = false, Type = typeof(int), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Character.MarriageCount, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.WriteCounter, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.ExploLevel, IsNullable = false, Type = typeof(short), DefaultValue = "1" },
                new ColumnInfo() { Name = Col.Character.ExploCumLevel, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.ExploExp, IsNullable = false, Type = typeof(long), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Character.DiscoverCount, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.NsBombCount, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.NsBombDay, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.LifeMaxByFood, IsNullable = false, Type = typeof(double), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.ManaMaxByFood, IsNullable = false, Type = typeof(double), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Character.StaminaMaxByFood, IsNullable = false, Type = typeof(double), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.StrengthByFood, IsNullable = false, Type = typeof(double), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.DexterityByFood, IsNullable = false, Type = typeof(double), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.IntelligenceByFood, IsNullable = false, Type = typeof(double), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.WillByFood, IsNullable = false, Type = typeof(double), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Character.LuckByFood, IsNullable = false, Type = typeof(double), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.FarmId, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.HeartUpdateTime, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.HeartPoint, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.HeartTotalPoint, IsNullable = false, Type = typeof(short), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Character.JoustPoint, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.JoustLastWinYear, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.JoustLastWinWeek, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.JoustWeekWinCount, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.JoustDailyWinCount, IsNullable = false, Type = typeof(short), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Character.JoustDailyLoseCount, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.JoustServerWinCount, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.JoustServerLoseCount, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.ExploMaxKeyLevel, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.DonationValue, IsNullable = false, Type = typeof(int), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Character.DonationUpdate, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Character.MacroPoint, IsNullable = false, Type = typeof(int), DefaultValue = "2000" },
                new ColumnInfo() { Name = Col.Character.JobId, IsNullable = false, Type = typeof(byte) , DefaultValue = "0"},
                new ColumnInfo() { Name = Col.Character.CouponCode, IsNullable = false, Type = typeof(char), Size = 40, DefaultValue = "" },


                };

            _primaryKey = new string[] { Col.Character.Id };
            _keys = new List<string[]>() { new string[] { Col.Character.Name }, new string[] { Col.Character.Birthday } };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.GameId, new string[] { Col.GameId.Id }, new string[] { Col.Character.Id }, false), };
        }
    }

    public class ChararacterDeedTable : BaseTable
    {
        public ChararacterDeedTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharacterDeed;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.CharacterDeed.Id, IsNullable = false, Type = typeof(long) },
            };

            for (int i = 0; i < Col.CharacterDeed.DayCount.Length; i++)
            {
                _columns.Add(new ColumnInfo() { Name = Col.CharacterDeed.DayCount[i], IsNullable = false, Type = typeof(int), DefaultValue = "0" });
                _columns.Add(new ColumnInfo() { Name = Col.CharacterDeed.Flag[i], IsNullable = false, Type = typeof(long), DefaultValue = "0" });
            }

            _primaryKey = new string[] { Col.CharacterDeed.Id };
        }
    }

    public class CharacterDivineKnightTable : BaseTable
    {
        public CharacterDivineKnightTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharacterDivineKnight;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CharacterDivineKnight.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterDivineKnight.Experience, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterDivineKnight.GroupLimit, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterDivineKnight.GroupSelected, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterDivineKnight.UpdateDate, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP"  },
                };

            _primaryKey = new string[] { Col.CharacterDivineKnight.Id };
        }
    }

    public class CharacterKeywordTable : BaseTable
    {
        public CharacterKeywordTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharacterKeyword;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CharacterKeyword.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterKeyword.KeywordId, IsNullable = false, Type = typeof(int) },
                };

            _primaryKey = new string[] { Col.CharacterKeyword.CharId, Col.CharacterKeyword.KeywordId };
        }
    }

    public class CharacterMetaTable : BaseTable
    {
        public CharacterMetaTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharacterMeta;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CharacterMeta.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterMeta.MCode, IsNullable = false, Type = typeof(char), Size = 30 },
                new ColumnInfo() { Name = Col.CharacterMeta.MType, IsNullable = false, Type = typeof(char), Size = 10 },
                new ColumnInfo() { Name = Col.CharacterMeta.MData, IsNullable = false, Type = typeof(char), Size = 30 },
                };

            _primaryKey = new string[] { Col.CharacterMeta.CharId, Col.CharacterMeta.MCode };
        }
    }

    public class CharacterMyKnightsTable : BaseTable
    {
        public CharacterMyKnightsTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharacterMyKnights;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CharacterMyKnights.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterMyKnights.Name, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.CharacterMyKnights.Level, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterMyKnights.Experience, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterMyKnights.Point, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.CharacterMyKnights.CreateDate, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.CharacterMyKnights.DateBuffMember, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterMyKnights.AddedSlotCount, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterMyKnights.UpdateTime, IsNullable = false, Type = typeof(DateTime) },
                };

            _primaryKey = new string[] { Col.CharacterMyKnights.CharId };
        }
    }

    public class CharacterMyKnightsMemberTable : BaseTable
    {
        public CharacterMyKnightsMemberTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharacterMyKnightsMember;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.KnightId, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.IsMine, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.Holy, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.Strength, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.Intelligence, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.Dexterity, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.Will, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.Luck, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.FavorLvl, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.Favor, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.Stress, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.WoundTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.IsSelfCured, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.CurrentTraining, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.StartTrainingTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.CurrentCommand, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.RestStartTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.DateList, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.LastDateTime, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.FirstScoutTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.LastScoutTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.LastReleaseTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.ReleaseCount, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.CommandTryCount, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.CommandSuccessCount, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.FavorTalkCount, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.CurrentCommandTemplate, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterMyKnightsMember.CommandEndTime, IsNullable = false, Type = typeof(long) },
                };

            _primaryKey = new string[] { Col.CharacterMyKnightsMember.CharId, Col.CharacterMyKnightsMember.KnightId };
        }
    }

    public class CharacterPvPTable : BaseTable
    {
        public CharacterPvPTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharacterPvP;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.CharacterPvP.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterPvP.WinCount, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterPvP.LoseCount, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterPvP.PenaltyPoint, IsNullable = false, Type = typeof(int) },
            };

            _primaryKey = new string[] { Col.CharacterPvP.Id };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.Character, new string[] { Col.Character.Id }, new string[] { Col.CharacterPvP.Id }, true, true) };
        }
    }

    public class CharacterShapeTable : BaseTable
    {
        public CharacterShapeTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharacterShape;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.CharacterShape.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterShape.ShapeId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterShape.Count, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CharacterShape.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CharacterShape.UpdateTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
            };

            _primaryKey = new string[] { Col.CharacterShape.Id, Col.CharacterShape.ShapeId };
        }
    }

    public class CharacterSkillTable : BaseTable
    {
        public CharacterSkillTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharacterSkill;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.CharacterSkill.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterSkill.Skill, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterSkill.Version, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterSkill.Level, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CharacterSkill.MaxLevel, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.CharacterSkill.Experience, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterSkill.Count, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterSkill.Flag, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterSkill.SubFlag1, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterSkill.SubFlag2, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.CharacterSkill.SubFlag3, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterSkill.SubFlag4, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterSkill.SubFlag5, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterSkill.SubFlag6, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterSkill.SubFlag7, IsNullable = false, Type = typeof(short) },


                new ColumnInfo() { Name = Col.CharacterSkill.SubFlag8, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterSkill.SubFlag9, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.CharacterSkill.LastPromotionTime, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.CharacterSkill.PromotionConditionCount, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.CharacterSkill.PromotionExperience, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
            };

            _primaryKey = new string[] { Col.CharacterSkill.Id, Col.CharacterSkill.Skill };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.Character, new string[] { Col.Character.Id }, new string[] { Col.CharacterSkill.Id }, true, true) };
        }
    }

    public class CharacterSubskillTable : BaseTable
    {
        public CharacterSubskillTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharacterSubskill;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.CharacterSubskill.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterSubskill.Subskill, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterSubskill.Level, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterSubskill.Experience, IsNullable = false, Type = typeof(int) },
            };

            _primaryKey = new string[] { Col.CharacterSubskill.Id, Col.CharacterSubskill.Subskill };
        }
    }

    public class CharacterAchievementTable : BaseTable
    {
        public CharacterAchievementTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharacterAchievement;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.CharacterAchievement.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterAchievement.TotalScore, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterAchievement.Achievement, IsNullable = false, Type = typeof(char), Size = 256 },
            };

            _primaryKey = new string[] { Col.CharacterAchievement.Id };
            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.Character, new string[] { Col.Character.Id }, new string[] { Col.CharacterAchievement.Id }, true, true) };
        }
    }

    public class CharacterQuestTable : BaseTable
    {
        public CharacterQuestTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharacterQuest;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.CharacterQuest.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterQuest.QuestId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharacterQuest.Start, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterQuest.End, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CharacterQuest.Extra, IsNullable = true, Type = typeof(int) },
            };

            _primaryKey = new string[] { Col.CharacterQuest.Id, Col.CharacterQuest.QuestId };
            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.Character, new string[] { Col.Character.Id }, new string[] { Col.CharacterQuest.Id }, true, true) };
        }
    }

    public class CharIdPoolTable : BaseTable
    {
        public CharIdPoolTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharIdPool;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.CharIdPool.Count, IsNullable = false, Type = typeof(long) },
            };
        }
    }

    public class CharItemEgoTable : BaseTable
    {
        public CharItemEgoTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharItemEgo;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CharItemEgo.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.PocketId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Class, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.PosX, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosY, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharItemEgo.VarInt, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color2, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Bundle, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.LinkedPocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Figure, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.DurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.OriginalDurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.AttackMin, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Item.AttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.WAttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.WAttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.Balance, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Critical, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.Defence, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Protect, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.EffectiveRange, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.AttackSpeed, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.DownHitCount, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.Experience, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.ExpPoint, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Upgraded, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.UpgradeMax, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Grade, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.Prefix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.Suffix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.Data, IsNullable = false, Type = typeof(char), Size = 150 },
                new ColumnInfo() { Name = Col.Item.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.Expiration, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.CharItemEgo.EgoName, IsNullable = false, Type = typeof(char), Size = 12 },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoType, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoDesire, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoSocialLevel, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoSocialExp, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.CharItemEgo.EgoStrengthLevel, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoStrengthExp, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoIntelligenceLevel, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoIntelligenceExp, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoDexterityLevel, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.CharItemEgo.EgoDexterityExp, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoWillLevel, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoWillExp, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoLuckLevel, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoLuckExp, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoSkillCount, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoSkillGauge, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.CharItemEgo.EgoSkillCooldown, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.Option, IsNullable = false, Type = typeof(char), Size = 4000, DefaultValue = "" },
                };


            _primaryKey = new string[] { Col.CharItemEgo.ItemId, Col.CharItemEgo.ItemLoc };

            _keys = new List<string[]>() {
                new string[] { Col.CharItemEgo.Id }
            };

            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey( Mabinogi.SQL.Tables.Mabinogi.GameId, new string[] { Col.GameId.Id }, new string[] { Col.CharItem.Id }, true )
            };
        }
    }

    public class CharItemHugeTable : BaseTable
    {
        public CharItemHugeTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharItemHuge;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CharItemHuge.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.PocketId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Class, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.PosX, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosY, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharItemHuge.VarInt, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color2, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Bundle, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.LinkedPocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Flag, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.DurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.OriginalDurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Data, IsNullable = false, Type = typeof(char), Size = 3000 },
                new ColumnInfo() { Name = Col.Item.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Item.Expiration, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                };


            _primaryKey = new string[] { Col.CharItemHuge.ItemId, Col.CharItemHuge.ItemLoc };
            _keys = new List<string[]>() {
                new string[] { Col.CharItemHuge.Id }
            };

            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey( Mabinogi.SQL.Tables.Mabinogi.GameId, new string[] { Col.GameId.Id }, new string[] { Col.CharItem.Id }, true ),
            };
        }
    }

    public class CharItemLargeTable : BaseTable
    {
        public CharItemLargeTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharItemLarge;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CharItemLarge.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.PocketId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Class, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.PosX, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosY, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharItemLarge.VarInt, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color2, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Bundle, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.LinkedPocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Figure, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.DurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.OriginalDurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.AttackMin, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Item.AttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.WAttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.WAttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.Balance, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Critical, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.Defence, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Protect, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.EffectiveRange, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.AttackSpeed, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.DownHitCount, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.Experience, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.ExpPoint, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Upgraded, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.UpgradeMax, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Grade, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.Prefix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.Suffix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.Data, IsNullable = false, Type = typeof(char), Size = 600 },
                new ColumnInfo() { Name = Col.Item.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.Expiration, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Item.Option, IsNullable = false, Type = typeof(char), Size = 4000, DefaultValue = "" },
                };


            _primaryKey = new string[] { Col.CharItemLarge.ItemId, Col.CharItemLarge.ItemLoc };
            _keys = new List<string[]>() {
                new string[] { Col.CharItemLarge.Id }
            };

            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey( Mabinogi.SQL.Tables.Mabinogi.GameId, new string[] { Col.GameId.Id }, new string[] { Col.CharItem.Id }, true )
            };
        }
    }

    public class CharItemQuestTable : BaseTable
    {
        public CharItemQuestTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharItemQuest;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CharItemQuest.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.PocketId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharItemQuest.Quest, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.Item.Class, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosX, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosY, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharItemQuest.VarInt, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color1, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Color2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Bundle, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.LinkedPocket, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharItemQuest.TemplateId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharItemQuest.Complete, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.CharItemQuest.StartTime, IsNullable = false, Type = typeof(long) },


                new ColumnInfo() { Name = Col.Item.Data, IsNullable = false, Type = typeof(char), Size = 200 },
                new ColumnInfo() { Name = Col.CharItemQuest.Objective, IsNullable = false, Type = typeof(char), Size = 1500 },
                new ColumnInfo() { Name = Col.Item.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.Expiration, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                };


            _primaryKey = new string[] { Col.CharItemQuest.ItemId, Col.CharItemQuest.ItemLoc };
            _keys = new List<string[]>() {
                new string[] { Col.CharItemQuest.Id }
            };

            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey( Mabinogi.SQL.Tables.Mabinogi.GameId, new string[] { Col.GameId.Id }, new string[] { Col.CharItem.Id },true )
            };
        }
    }

    public class CharItemSmallTable : BaseTable
    {
        public CharItemSmallTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CharItemSmall;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CharItemSmall.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.PocketId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Class, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.PosX, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosY, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharItemSmall.VarInt, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color2, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Bundle, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.LinkedPocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Flag, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.Expiration, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                };


            _primaryKey = new string[] { Col.CharItemSmall.ItemId, Col.CharItemSmall.ItemLoc };
            _keys = new List<string[]>() {
                new string[] { Col.CharItemSmall.Id }
            };

            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey( Mabinogi.SQL.Tables.Mabinogi.GameId,  new string[] { Col.GameId.Id }, new string[] { Col.CharItem.Id }, true )
            };
        }
    }

    public class CommerceTable : BaseTable
    {
        public CommerceTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.Commerce;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.Commerce.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Commerce.Ducat, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Commerce.CurrentTransportId, IsNullable = false, Type = typeof(int), DefaultValue = "1" },
                new ColumnInfo() { Name = Col.Commerce.UnlockTransport, IsNullable = false, Type = typeof(long), DefaultValue = "1" },
                new ColumnInfo() { Name = Col.Commerce.LostPercent, IsNullable = false, Type = typeof(int), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Commerce.Post1Credit, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Commerce.Post2Credit, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Commerce.Post3Credit, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Commerce.Post4Credit, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Commerce.Post5Credit, IsNullable = false, Type = typeof(int), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Commerce.Post6Credit, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Commerce.Post7Credit, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Commerce.Post8Credit, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Commerce.UpdateDate, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
                };


            _primaryKey = new string[] { Col.Commerce.CharId };
        }
    }

    public class CommerceCriminalTable : BaseTable
    {
        public CommerceCriminalTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CommerceCriminal;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.CommerceCriminal.CriminalId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CommerceCriminal.CharName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.CommerceCriminal.Ducat, IsNullable = false, Type = typeof(int) },
            };

            _primaryKey = new string[] { Col.CommerceCriminal.CriminalId, Col.CommerceCriminal.CharName };
            _keys = new List<string[]>() {
                new string[] { Col.CommerceCriminal.CharName }
            };
        }
    }

    public class CommerceCriminalRewardTable : BaseTable
    {
        public CommerceCriminalRewardTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CommerceCriminalReward;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.CommerceCriminalReward.CriminalId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CommerceCriminalReward.Reward, IsNullable = false, Type = typeof(int) },
            };

            _primaryKey = new string[] { Col.CommerceCriminalReward.CriminalId };
        }
    }

    public class CommercePostTable : BaseTable
    {
        public CommercePostTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CommercePost;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.CommercePost.PostId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CommercePost.PostInvestment, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CommercePost.PostCommission, IsNullable = false, Type = typeof(int) },
            };

            _primaryKey = new string[] { Col.CommercePost.PostId };
        }
    }

    public class CommerceProductTable : BaseTable
    {
        public CommerceProductTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CommerceProduct;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.CommerceProduct.ProductId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CommerceProduct.ProductPrice, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CommerceProduct.ProductCount, IsNullable = false, Type = typeof(int) },
            };

            _primaryKey = new string[] { Col.CommerceProduct.ProductId };
        }
    }

    public class CommerceProductStockTable : BaseTable
    {
        public CommerceProductStockTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CommerceProductStock;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CommerceProductStock.ProductId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CommerceProductStock.ProductSellPostId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CommerceProductStock.ProductStock, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CommerceProductStock.ProductStockPrice, IsNullable = false, Type = typeof(int) },
                };


            _primaryKey = new string[] { Col.CommerceProductStock.ProductId, Col.CommerceProductStock.ProductSellPostId };

            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey( Mabinogi.SQL.Tables.Mabinogi.CommercePost,  new string[] { Col.CommercePost.PostId}, new string[] { Col.CommerceProductStock.ProductSellPostId }, true ),
                new ForeignKey( Mabinogi.SQL.Tables.Mabinogi.CommerceProduct,  new string[] { Col.CommerceProduct.ProductId }, new string[] { Col.CommerceProductStock.ProductId }, true )
            };
        }
    }

    public class CommercePurchasedProductTable : BaseTable
    {
        public CommercePurchasedProductTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CommercePurchasedProduct;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CommercePurchasedProduct.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.CommercePurchasedProduct.ClassId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CommercePurchasedProduct.Bundle, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CommercePurchasedProduct.Price, IsNullable = false, Type = typeof(int) },
                };


            _primaryKey = new string[] { Col.CommercePurchasedProduct.CharId, Col.CommercePurchasedProduct.ClassId };
        }
    }

    public class CumulativeLevelRankTable : BaseTable
    {
        public CumulativeLevelRankTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.CumulativeLevelRank;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.CumulativeLevelRank.Num, IsNullable = false, Type = typeof(int), Options = 1 },
                new ColumnInfo() { Name = Col.CumulativeLevelRank.RegDate, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.CumulativeLevelRank.Id, IsNullable = true, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.CumulativeLevelRank.Name, IsNullable = true, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.CumulativeLevelRank.Level, IsNullable = true, Type = typeof(short) },

                new ColumnInfo() { Name = Col.CumulativeLevelRank.PlayTime, IsNullable = true, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.CumulativeLevelRank.Experience, IsNullable = true, Type = typeof(long), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.CumulativeLevelRank.Age, IsNullable = true, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.CumulativeLevelRank.CumulatedLevel, IsNullable = true, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.CumulativeLevelRank.Race, IsNullable = true, Type = typeof(byte) },

                };


            _primaryKey = new string[] { Col.CumulativeLevelRank.Num };

            _keys = new List<string[]>() {
                new string[] { Col.CumulativeLevelRank.RegDate }
            };
        }
    }

    public class DeleteCharTable : BaseTable
    {
        public DeleteCharTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.DeleteChar;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.DeleteChar.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.DeleteChar.DeleteTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
                };


            _primaryKey = new string[] { Col.DeleteChar.Id };

            _keys = new List<string[]>() {
                new string[] { Col.DeleteChar.DeleteTime }
            };
        }
    }
    public class EquipCollectTable : BaseTable
    {
        public EquipCollectTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.EquipCollect;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.EquipCollect.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.EquipCollect.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                };


            _primaryKey = new string[] { Col.EquipCollect.Account };
        }
    }

    public class EquipCollectItemLargeTable : BaseTable
    {
        public EquipCollectItemLargeTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.EquipCollectItemLarge;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.EquipCollectItem.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.Item.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.PocketId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Class, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.PosX, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.PosY, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.CharItemLarge.VarInt, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color2, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Bundle, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.LinkedPocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Figure, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.DurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.OriginalDurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.AttackMin, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Item.AttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.WAttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.WAttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.Balance, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Critical, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.Defence, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Protect, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.EffectiveRange, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.AttackSpeed, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.DownHitCount, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.Experience, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.ExpPoint, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Upgraded, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.UpgradeMax, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Grade, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.Prefix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.Suffix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.Data, IsNullable = false, Type = typeof(char), Size = 600 },
                new ColumnInfo() { Name = Col.Item.Option, IsNullable = false, Type = typeof(char), Size = 4000 },
                new ColumnInfo() { Name = Col.Item.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Item.Expiration, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.EquipCollectItem.LockTime, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
                };


            _primaryKey = new string[] { Col.CharItemLarge.ItemId, Col.CharItemLarge.ItemLoc };

            _keys = new List<string[]>() {
                new string[] { Col.Item.ItemId, Col.Item.ItemLoc },
                new string[] { Col.Item.Class, Col.Item.PosX },
                new string[] { Col.Item.PosX, Col.Item.PosY }
            };
        }
    }

    public class FamilyTable : BaseTable
    {
        public FamilyTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.Family;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.Family.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Family.Name, IsNullable = false, Type = typeof(char), Size = 50  },
                new ColumnInfo() { Name = Col.Family.HeadMemberId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Family.State, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Family.Tradition, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Family.Meta, IsNullable = false, Type = typeof(char), Size = 200 },
                };


            _primaryKey = new string[] { Col.Family.Id };

            _keys = new List<string[]>() {
                new string[] { Col.Family.Name }
            };
        }
    }

    public class FamilyMemberTable : BaseTable
    {
        public FamilyMemberTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.FamilyMember;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.FamilyMember.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.FamilyMember.FamilyId, IsNullable = false, Type = typeof(long)  },
                new ColumnInfo() { Name = Col.FamilyMember.Name, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.FamilyMember.Class, IsNullable = false, Type = typeof(short) },
                };


            _primaryKey = new string[] { Col.FamilyMember.CharId };

            _keys = new List<string[]>() {
                new string[] { Col.FamilyMember.FamilyId }
            };

            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey( Mabinogi.SQL.Tables.Mabinogi.Family, new string[] { Col.Family.Id }, new string[] { Col.FamilyMember.FamilyId }, true ),
            };
        }
    }

    public class FarmTable : BaseTable
    {
        public FarmTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.Farm;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.Farm.FarmId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Farm.OwnerAccount, IsNullable = false, Type = typeof(char), Size = 32  },
                new ColumnInfo() { Name = Col.Farm.OwnerCharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Farm.OwnerCharName, IsNullable = false, Type = typeof(char), Size = 64 },
                new ColumnInfo() { Name = Col.Farm.ExpireTime, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.Farm.Crop, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Farm.PlantTime, IsNullable = false, Type = typeof(long)  },
                new ColumnInfo() { Name = Col.Farm.WaterWork, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Farm.NutrientWork, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Farm.InsectWork, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Farm.Water, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Farm.Nutrient, IsNullable = false, Type = typeof(short)  },
                new ColumnInfo() { Name = Col.Farm.Insect, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Farm.Growth, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Farm.CurrentWork, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Farm.WorkCompleteTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Farm.TodayWorkCount, IsNullable = false, Type = typeof(byte)  },
                new ColumnInfo() { Name = Col.Farm.LastWorkTime, IsNullable = false, Type = typeof(long) },
                };


            _primaryKey = new string[] { Col.Farm.FarmId };

            _keys = new List<string[]>() {
                new string[] { Col.Farm.OwnerAccount}
            };
        }
    }

    public class FavoritePrivateFarmTable : BaseTable
    {
        public FavoritePrivateFarmTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.FavoritePrivateFarm;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.FavoritePrivateFarm.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.FavoritePrivateFarm.PrivateFarmId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.FavoritePrivateFarm.WorldPosX, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.FavoritePrivateFarm.WorldPosY, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.FavoritePrivateFarm.OwnerName, IsNullable = false, Type = typeof(char), Size = 50 },

                new ColumnInfo() { Name = Col.FavoritePrivateFarm.FarmName, IsNullable = false, Type = typeof(char), Size = 20 },
                new ColumnInfo() { Name = Col.FavoritePrivateFarm.ThemeId, IsNullable = false, Type = typeof(int)  },
                };


            _primaryKey = new string[] { Col.FavoritePrivateFarm.CharId, Col.FavoritePrivateFarm.PrivateFarmId };

            _keys = new List<string[]>() {
                new string[] { Col.FavoritePrivateFarm.PrivateFarmId }
            };

            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey( Mabinogi.SQL.Tables.Mabinogi.Character, new string[] { Col.Character.Id}, new string[] { Col.FavoritePrivateFarm.CharId }, true, true ),
            };
        }
    }

    public class GameIdTable : BaseTable
    {
        public GameIdTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.GameId;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.GameId.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.GameId.Name, IsNullable = false, Type = typeof(char), Size = 50  },
                new ColumnInfo() { Name = Col.GameId.Flag, IsNullable = false, Type = typeof(byte) },
                };


            _primaryKey = new string[] { Col.GameId.Id };

            _keys = new List<string[]>() {
                new string[] { Col.GameId.Name }
            };
        }
    }

    public class GuestBookTable : BaseTable
    {
        public GuestBookTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.GuestBook;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {

                new ColumnInfo() { Name = Col.GuestBook.Id, IsNullable = false, Type = typeof(int), Options = 1  },

                new ColumnInfo() { Name = Col.GuestBook.Server, IsNullable = false, Type = typeof(char), Size = 128  },
                new ColumnInfo() { Name = Col.GuestBook.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.GuestBook.Message, IsNullable = true, Type = typeof(char), Size = 1000  },
                new ColumnInfo() { Name = Col.GuestBook.Valid, IsNullable = false, Type = typeof(bool), DefaultValue = "1" },
                };


            _primaryKey = new string[] { Col.GuestBook.Id };

            _keys = new List<string[]>() {
                new string[] { Col.GuestBook.Server, Col.GuestBook.Account }
            };
        }
    }

    public class GuestBookCommentTable : BaseTable
    {
        public GuestBookCommentTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.GuestBookComment;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.GuestBookComment.SerialNumber, IsNullable = false, Type = typeof(long), Options = 1  },


                new ColumnInfo() { Name = Col.GuestBookComment.Id, IsNullable = false, Type = typeof(int)  },
                new ColumnInfo() { Name = Col.GuestBookComment.Server, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.GuestBookComment.Account, IsNullable = true, Type = typeof(char), Size = 50  },
                new ColumnInfo() { Name = Col.GuestBookComment.Name, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.GuestBookComment.Thread, IsNullable = false, Type = typeof(long), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.GuestBookComment.Depth, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.GuestBookComment.WriteDate, IsNullable = true, Type = typeof(long) },
                new ColumnInfo() { Name = Col.GuestBookComment.Type, IsNullable = false, Type = typeof(bool), DefaultValue = "1" },
                new ColumnInfo() { Name = Col.GuestBookComment.Message, IsNullable = false, Type = typeof(char), Size = 1000  },
                new ColumnInfo() { Name = Col.GuestBookComment.ReplyNum, IsNullable = false, Type = typeof(int), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.GuestBookComment.RealDate, IsNullable = true, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
                };


            _primaryKey = new string[] { Col.GuestBookComment.SerialNumber };

            _keys = new List<string[]>() {
                new string[] { Col.GuestBookComment.Thread },
                new string[] { Col.GuestBookComment.Id },
                new string[] { Col.GuestBookComment.Account }
            };


            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey( Mabinogi.SQL.Tables.Mabinogi.GuestBook, new string[] { Col.GuestBook.Id}, new string[] { Col.GuestBookComment.Id }, true, true, true ),
            };
        }
    }

    public class HelpPointRankingTable : BaseTable
    {
        public HelpPointRankingTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.HelpPointRanking;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.HelpPointRanking.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.HelpPointRanking.Score1, IsNullable = false, Type = typeof(int)  },
                new ColumnInfo() { Name = Col.HelpPointRanking.Score2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HelpPointRanking.UpdateTime, IsNullable = false, Type = typeof(DateTime)  },
                };


            _primaryKey = new string[] { Col.HelpPointRanking.CharId };
        }
    }

    public class HouseTable : BaseTable
    {
        public HouseTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.House;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.House.HouseId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.House.Constructed, IsNullable = false, Type = typeof(byte)  },
                new ColumnInfo() { Name = Col.House.UpdateTime, IsNullable = false, Type = typeof(DateTime)  },
                new ColumnInfo() { Name = Col.House.CharName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.House.HouseName, IsNullable = false, Type = typeof(char), Size = 16  },

                new ColumnInfo() { Name = Col.House.HouseClass, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.House.RoofSkin, IsNullable = false, Type = typeof(byte)  },
                new ColumnInfo() { Name = Col.House.RoofColor1, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.House.RoofColor2, IsNullable = false, Type = typeof(byte)  },
                new ColumnInfo() { Name = Col.House.RoofColor3, IsNullable = false, Type = typeof(byte)  },

                new ColumnInfo() { Name = Col.House.WallSkin, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.House.WallColor1, IsNullable = false, Type = typeof(byte)  },
                new ColumnInfo() { Name = Col.House.WallColor2, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.House.WallColor3, IsNullable = false, Type = typeof(byte)  },
                new ColumnInfo() { Name = Col.House.InnerSkin, IsNullable = false, Type = typeof(byte)  },

                new ColumnInfo() { Name = Col.House.InnerColor1, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.House.InnerColor2, IsNullable = false, Type = typeof(byte)  },
                new ColumnInfo() { Name = Col.House.InnerColor3, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.House.Width, IsNullable = false, Type = typeof(int)  },
                new ColumnInfo() { Name = Col.House.Height, IsNullable = false, Type = typeof(int)  },

                new ColumnInfo() { Name = Col.House.BidSuccessDate, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.House.TaxPrevDate, IsNullable = true, Type = typeof(DateTime)  },
                new ColumnInfo() { Name = Col.House.TaxNextDate, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.House.TaxPrice, IsNullable = false, Type = typeof(int)  },
                new ColumnInfo() { Name = Col.House.TaxAutopay, IsNullable = false, Type = typeof(byte)  },

                new ColumnInfo() { Name = Col.House.HouseMoney, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.House.Deposit, IsNullable = false, Type = typeof(int)  },
                new ColumnInfo() { Name = Col.House.Flag, IsNullable = false, Type = typeof(long) },
                };


            _primaryKey = new string[] { Col.House.HouseId };
        }
    }

    public class HouseBlockTable : BaseTable
    {
        public HouseBlockTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.HouseBlock;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.HouseBlock.HouseId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.HouseBlock.GameName, IsNullable = false, Type = typeof(char), Size = 50  },
                new ColumnInfo() { Name = Col.HouseBlock.Flag, IsNullable = false, Type = typeof(byte)  },
                };


            _keys = new List<string[]>() { new string[] { Col.HouseBlock.HouseId }, new string[] { Col.HouseBlock.GameName } };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.House, new string[] { Col.House.HouseId }, new string[] { Col.HouseBlock.HouseId }, true, true) };
        }
    }

    public class HouseBidTable : BaseTable
    {
        public HouseBidTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.HouseBid;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.HouseBid.HouseId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.HouseBid.BidStartTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.HouseBid.BidEndTime, IsNullable = false, Type = typeof(DateTime)  },
                new ColumnInfo() { Name = Col.HouseBid.BidRepayEndTime, IsNullable = false, Type = typeof(DateTime)  },
                new ColumnInfo() { Name = Col.HouseBid.MinBidPrice, IsNullable = false, Type = typeof(int)  },
                };


            _primaryKey = new string[] { Col.HouseBid.HouseId };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.House, new string[] { Col.House.HouseId }, new string[] { Col.HouseBid.HouseId }, true, true) };
        }
    }

    public class HouseBidderTable : BaseTable
    {
        public HouseBidderTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.HouseBidder;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.HouseBidder.HouseId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.HouseBidder.BidAccount, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.HouseBidder.BidPrice, IsNullable = false, Type = typeof(int)  },
                new ColumnInfo() { Name = Col.HouseBidder.BidOrder, IsNullable = false, Type = typeof(int)  },
                new ColumnInfo() { Name = Col.HouseBidder.BidCharacter, IsNullable = false, Type = typeof(long)  },

                new ColumnInfo() { Name = Col.HouseBidder.BidCharName, IsNullable = false, Type = typeof(char), Size = 50  },
                new ColumnInfo() { Name = Col.HouseBidder.BidTime, IsNullable = false, Type = typeof(DateTime)  },
                new ColumnInfo() { Name = Col.HouseBidder.IsWinner, IsNullable = false, Type = typeof(byte) },
                };


            _primaryKey = new string[] { Col.HouseBidder.BidAccount };

            _keys = new List<string[]>() { new string[] { Col.HouseBidder.HouseId } };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.House, new string[] { Col.House.HouseId }, new string[] { Col.HouseBidder.HouseId }, true, true) };
        }
    }

    public class HouseBidderHistoryTable : BaseTable
    {
        public HouseBidderHistoryTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.HouseBidderHistory;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.HouseBidderHistory.HouseId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.HouseBidderHistory.BidAccount, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.HouseBidderHistory.BidPrice, IsNullable = false, Type = typeof(int)  },
                new ColumnInfo() { Name = Col.HouseBidderHistory.BidOrder, IsNullable = false, Type = typeof(int)  },
                new ColumnInfo() { Name = Col.HouseBidderHistory.BidCharacter, IsNullable = false, Type = typeof(long)  },

                new ColumnInfo() { Name = Col.HouseBidderHistory.BidCharName, IsNullable = false, Type = typeof(char), Size = 50  },
                new ColumnInfo() { Name = Col.HouseBidderHistory.BidTime, IsNullable = false, Type = typeof(DateTime)  },
                new ColumnInfo() { Name = Col.HouseBidderHistory.RefundTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.HouseBidderHistory.Flag, IsNullable = false, Type = typeof(byte) },
                };

            _keys = new List<string[]>() { new string[] { Col.HouseBidderHistory.HouseId } };

        }
    }

    public class HouseItemHugeTable : BaseTable
    {

        public HouseItemHugeTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.HouseItemHuge;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.HouseItemHuge.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.HouseItemHuge.PosX, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemHuge.PosY, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemHuge.Direction, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemHuge.UserPrice, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.HouseItemHuge.Pocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemHuge.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.HouseItemHuge.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "2" },
                new ColumnInfo() { Name = Col.HouseItemHuge.Class, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemHuge.Color1, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.HouseItemHuge.Color2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemHuge.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemHuge.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemHuge.Bundle, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.HouseItemHuge.LinkedPocket, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.HouseItemHuge.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemHuge.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemHuge.DurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemHuge.OriginalDurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemHuge.Data, IsNullable = false, Type = typeof(char), Size = 3000 },

                new ColumnInfo() { Name = Col.HouseItemHuge.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.HouseItemHuge.Expiration, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemHuge.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                };


            _primaryKey = new string[] { Col.HouseItemHuge.ItemId, Col.HouseItemHuge.ItemLoc };
            _keys = new List<string[]>() {
                new string[] { Col.HouseItemHuge.Account }
            };
        }
    }

    public class HouseItemLargeTable : BaseTable
    {
        public HouseItemLargeTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.HouseItemLarge;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.HouseItemLarge.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.HouseItemLarge.PosX, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemLarge.PosY, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Direction, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemLarge.UserPrice, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.HouseItemLarge.Pocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemLarge.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.HouseItemLarge.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "2" },
                new ColumnInfo() { Name = Col.HouseItemLarge.Class, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Color1, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.HouseItemLarge.Color2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Bundle, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.HouseItemLarge.LinkedPocket, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.HouseItemLarge.Figure, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemLarge.DurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemLarge.OriginalDurabilityMax, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.HouseItemLarge.AttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.HouseItemLarge.AttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.HouseItemLarge.WAttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.HouseItemLarge.WAttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Balance, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.HouseItemLarge.Critical, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Defence, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Protect, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.HouseItemLarge.EffectiveRange, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.HouseItemLarge.AttackSpeed, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.HouseItemLarge.DownHitCount, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Experience, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.HouseItemLarge.ExpPoint, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Upgraded, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemLarge.UpgradeMax, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.HouseItemLarge.Grade, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Prefix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Suffix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.HouseItemLarge.Data, IsNullable = false, Type = typeof(char), Size = 600 },
                new ColumnInfo() { Name = Col.HouseItemLarge.Option, IsNullable = false, Type = typeof(char), Size = 4000 },

                new ColumnInfo() { Name = Col.HouseItemLarge.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.HouseItemLarge.Expiration, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemLarge.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                };


            _primaryKey = new string[] { Col.HouseItemLarge.ItemId, Col.HouseItemLarge.ItemLoc };
            _keys = new List<string[]>() {
                new string[] { Col.HouseItemLarge.Account }
            };
        }
    }

    public class HouseItemQuestTable : BaseTable
    {
        public HouseItemQuestTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.HouseItemQuest;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.HouseItemQuest.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.HouseItemQuest.PosX, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemQuest.PosY, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemQuest.Direction, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemQuest.UserPrice, IsNullable = false, Type = typeof(int) },


                new ColumnInfo() { Name = Col.HouseItemQuest.Pocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemQuest.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.HouseItemQuest.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "2" },
                new ColumnInfo() { Name = Col.HouseItemQuest.Quest, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.HouseItemQuest.Class, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.HouseItemQuest.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemQuest.Color2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemQuest.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemQuest.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemQuest.Bundle, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.HouseItemQuest.LinkedPocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemQuest.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemQuest.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemQuest.Data, IsNullable = false, Type = typeof(char), Size = 200 },
                new ColumnInfo() { Name = Col.HouseItemQuest.TemplateId, IsNullable = false, Type = typeof(int) },


                new ColumnInfo() { Name = Col.HouseItemQuest.Complete, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemQuest.StartTime, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemQuest.Objective, IsNullable = false, Type = typeof(char), Size = 1500 },
                new ColumnInfo() { Name = Col.HouseItemQuest.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.HouseItemQuest.Expiration, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.HouseItemQuest.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                };


            _primaryKey = new string[] { Col.HouseItemQuest.ItemId, Col.HouseItemQuest.ItemLoc };
            _keys = new List<string[]>() {
                new string[] { Col.HouseItemQuest.Account }
            };
        }
    }

    public class HouseItemSmallTable : BaseTable
    {
        public HouseItemSmallTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.HouseItemSmall;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.HouseItemSmall.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.HouseItemSmall.PosX, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemSmall.PosY, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemSmall.Direction, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemSmall.UserPrice, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.HouseItemSmall.Pocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemSmall.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.HouseItemSmall.ItemLoc, IsNullable = false, Type = typeof(byte), DefaultValue = "2" },
                new ColumnInfo() { Name = Col.HouseItemSmall.Class, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemSmall.Color1, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.HouseItemSmall.Color2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemSmall.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemSmall.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemSmall.Bundle, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.HouseItemSmall.LinkedPocket, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.HouseItemSmall.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.HouseItemSmall.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemSmall.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.HouseItemSmall.Expiration, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.HouseItemSmall.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                };


            _primaryKey = new string[] { Col.HouseItemSmall.ItemId, Col.HouseItemSmall.ItemLoc };
            _keys = new List<string[]>() {
                new string[] { Col.HouseItemSmall.Account }
            };
        }
    }

    public class HouseOwnerTable : BaseTable
    {
        public HouseOwnerTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.HouseOwner;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.HouseOwner.HouseId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.HouseOwner.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                };


            _primaryKey = new string[] { Col.HouseOwner.HouseId };

            _keys = new List<string[]>() { new string[] { Col.HouseOwner.Account } };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.House, new string[] { Col.House.HouseId }, new string[] { Col.HouseOwner.HouseId }, true, true) };
        }
    }

    public class InviteEventTable : BaseTable
    {
        public InviteEventTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.InviteEvent;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.InviteEvent.IdX, IsNullable = false, Type = typeof(int), Options = 1 },
                new ColumnInfo() { Name = Col.InviteEvent.Id, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.InviteEvent.Server, IsNullable = false, Type = typeof(char), Size = 20 },
                new ColumnInfo() { Name = Col.InviteEvent.InviteCharacterId, IsNullable = true, Type = typeof(long) },
                new ColumnInfo() { Name = Col.InviteEvent.InviteCharacterName, IsNullable = false, Type = typeof(char), Size = 32 },

                new ColumnInfo() { Name = Col.InviteEvent.SendDate, IsNullable = false, Type = typeof(DateTime) },

                };


            _primaryKey = new string[] { Col.InviteEvent.Id, Col.InviteEvent.Server, Col.InviteEvent.InviteCharacterName, Col.InviteEvent.SendDate, Col.InviteEvent.IdX };
            _keys = new List<string[]>() { new string[] { Col.InviteEvent.IdX } };
        }
    }

    public abstract class ItemHistoryTable : BaseTable
    {
        public ItemHistoryTable()
        {
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.ItemHistory.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.ItemHistory.PocketId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistory.Class, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistory.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistory.Color2, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.ItemHistory.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistory.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistory.Bundle, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistory.LinkedPocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistory.Figure, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.ItemHistory.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistory.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistory.DurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistory.OriginalDurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistory.AttackMin, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.ItemHistory.AttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistory.WAttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistory.WAttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistory.Balance, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistory.Critical, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.ItemHistory.Defence, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistory.Protect, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistory.EffectiveRange, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistory.AttackSpeed, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistory.DownHitCount, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.ItemHistory.Experience, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistory.ExpPoint, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistory.Upgraded, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistory.UpgradeMax, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistory.Grade, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.ItemHistory.Prefix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistory.Suffix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistory.Data, IsNullable = false, Type = typeof(char), Size = 3000 },
                new ColumnInfo() { Name = Col.ItemHistory.Option, IsNullable = false, Type = typeof(char), Size = 4000 },
                new ColumnInfo() { Name = Col.ItemHistory.SellingPrice, IsNullable = false, Type = typeof(int) },


                new ColumnInfo() { Name = Col.ItemHistory.DeleteTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.ItemHistory.StoredType, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistory.Expiration, IsNullable = false, Type = typeof(int) },
                };


            _keys = new List<string[]>() {
                new string[] { Col.ItemHistory.DeleteTime }, new string[] { Col.ItemHistory.Id }
            };

        }
    }

    public class ItemHistoryTable1 : ItemHistoryTable
    {
        public ItemHistoryTable1()
            : base()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ItemHistory1;
        }
    }

    public class ItemHistoryTable2 : ItemHistoryTable
    {
        public ItemHistoryTable2()
            : base()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ItemHistory2;
        }
    }

    public class ItemEgoHistoryTable : BaseTable
    {
        public ItemEgoHistoryTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ItemEgoHistory;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Pocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Class, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Color2, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.ItemHistoryEgo.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Bundle, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.LinkedPocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Figure, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.ItemHistoryEgo.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Durability, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.DurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.OriginalDurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.AttackMin, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.ItemHistoryEgo.AttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.WAttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.WAttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Balance, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Critical, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.ItemHistoryEgo.Defence, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Protect, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EffectiveRange, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.AttackSpeed, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.DownHitCount, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.ItemHistoryEgo.Experience, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.ExpPoint, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Upgraded, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.UpgradeMax, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Grade, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.ItemHistoryEgo.Prefix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Suffix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Data, IsNullable = false, Type = typeof(char), Size = 150 },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.SellingPrice, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Expiration, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoName, IsNullable = false, Type = typeof(char), Size = 12 },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoType, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoDesire, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoSocialLevel, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoSocialExp, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoStrengthLevel, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoStrengthExp, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoIntelligenceLevel, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoIntelligenceExp, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoDexterityLevel, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoDexterityExp, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoWillLevel, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoWillExp, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoLuckLevel, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoLuckExp, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.ItemHistoryEgo.DeleteTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoSkillCount, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoSkillGauge, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.EgoSkillCooldown, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.ItemHistoryEgo.Option, IsNullable = false, Type = typeof(char), Size = 4000, DefaultValue = "" },
                };



            _keys = new List<string[]>() {
                new string[] { Col.ItemHistoryEgo.DeleteTime },
                new string[] { Col.ItemHistoryEgo.Id }
            };

        }
    }

    public abstract class ItemIdTable : BaseTable
    {
        public ItemIdTable()
        {
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.ItemId.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.ItemId.ItemLoc, IsNullable = false, Type = typeof(byte) },
                };


            _primaryKey = new string[] { Col.ItemId.Id };
        }
    }

    public class ItemEgoIdTable : ItemIdTable
    {
        public ItemEgoIdTable()
            : base()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ItemEgoId;
        }
    }

    public class ItemHugeIdTable : ItemIdTable
    {
        public ItemHugeIdTable()
            : base()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ItemHugeId;
        }
    }

    public class ItemIdPoolTable : BaseTable
    {
        public ItemIdPoolTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ItemIdPool;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.ItemIdPool.Count, IsNullable = false, Type = typeof(long) },
            };
        }
    }

    public class ItemLargeIdTable : ItemIdTable
    {
        public ItemLargeIdTable()
            : base()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ItemLargeId;
        }
    }

    public class ItemQuestIdTable : ItemIdTable
    {
        public ItemQuestIdTable()
            : base()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ItemQuestId;
        }
    }

    public class ItemSmallIdTable : ItemIdTable
    {
        public ItemSmallIdTable()
            : base()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ItemSmallId;
        }
    }

    public class LevelRankTable : BaseTable
    {
        public LevelRankTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.LevelRank;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.LevelRank.Num, IsNullable = false, Type = typeof(int), Options = 1 },
                new ColumnInfo() { Name = Col.LevelRank.RegDate, IsNullable = true, Type = typeof(DateTime)},
                new ColumnInfo() { Name = Col.LevelRank.Id, IsNullable = true, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.LevelRank.Name, IsNullable = true, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.LevelRank.Level, IsNullable = true, Type = typeof(short) },

                new ColumnInfo() { Name = Col.LevelRank.PlayTime, IsNullable = true, Type = typeof(DateTime), DefaultValue = "0000-00-00" },
                new ColumnInfo() { Name = Col.LevelRank.Experience, IsNullable = true, Type = typeof(long), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.LevelRank.Age, IsNullable = true, Type = typeof(short), Size = 20, DefaultValue = "0" },
                new ColumnInfo() { Name = Col.LevelRank.CumulatedLevel, IsNullable = true, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.LevelRank.Race, IsNullable = true, Type = typeof(byte) },

                };


            _primaryKey = new string[] { Col.LevelRank.Num };

            _keys = new List<string[]>() {
                new string[] { Col.LevelRank.RegDate }
            };
        }
    }

    public class LogDucatTable : BaseTable
    {
        public LogDucatTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.LogDucat;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.LogDucat.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.LogDucat.Ducat, IsNullable = true, Type = typeof(long) },
                new ColumnInfo() { Name = Col.LogDucat.LogDate, IsNullable = true, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
            };
        }
    }

    public class LoginIdPoolTable : BaseTable
    {
        public LoginIdPoolTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.LoginIdPool;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.LoginIdPool.Count, IsNullable = true, Type = typeof(long) },
            };
        }
    }

    public class MailBoxItemTable : BaseTable
    {
        public MailBoxItemTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.MailBoxItem;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.MailBoxItem.ReceiverCharID, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.MailBoxItem.SenderCharID, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Item.ItemLoc, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.MailBoxItem.StoredType, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.Class, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Price, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.Bundle, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.LinkedPocket, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Figure, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Flag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Durability, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Item.DurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.OriginalDurabilityMax, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.AttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.AttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.WAttackMin, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Item.WAttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.Balance, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Critical, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Defence, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.Protect, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Item.EffectiveRange, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.AttackSpeed, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.DownHitCount, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Experience, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.ExpPoint, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Item.Upgraded, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.UpgradeMax, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Grade, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Item.Prefix, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Item.Suffix, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Item.Data, IsNullable = false, Type = typeof(char), Size = 600 },
                new ColumnInfo() { Name = Col.Item.Option, IsNullable = false, Type = typeof(char), Size = 4000 },
                new ColumnInfo() { Name = Col.Item.SellingPrice, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Item.Expiration, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Item.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
            };

            _primaryKey = new string[] { Col.Item.ItemId };

            _keys = new List<string[]>() {
                new string[] { Col.MailBoxItem.ReceiverCharID },
                new string[] { Col.MailBoxItem.SenderCharID },
            };
        }
    }

    public class MailBoxReceiveTable : BaseTable
    {
        public MailBoxReceiveTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.MailBoxReceive;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.MailBoxReceive.PostId, IsNullable = false, Type = typeof(long), Options = 1 },
                new ColumnInfo() { Name = Col.MailBoxReceive.ReceiverCharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.MailBoxReceive.ReceiverCharName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.MailBoxReceive.SenderCharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.MailBoxReceive.SenderCharName, IsNullable = false, Type = typeof(char), Size = 50 },

                new ColumnInfo() { Name = Col.MailBoxReceive.ItemId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.MailBoxReceive.ItemCharge, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.MailBoxReceive.SenderMessage, IsNullable = false, Type = typeof(char), Size = 160 },
                new ColumnInfo() { Name = Col.MailBoxReceive.SendDate, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.MailBoxReceive.PostType, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.MailBoxReceive.Location, IsNullable = false, Type = typeof(char), Size = 250 },
                new ColumnInfo() { Name = Col.MailBoxReceive.Status, IsNullable = false, Type = typeof(byte) },
            };

            _primaryKey = new string[] { Col.MailBoxReceive.PostId };

            _keys = new List<string[]>() {
                new string[] { Col.MailBoxReceive.ReceiverCharId },
                new string[] { Col.MailBoxReceive.SenderCharId },
            };
        }
    }

    public class NotUsableGameIdTable : BaseTable
    {
        public NotUsableGameIdTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.NotUsableGameId;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.NotUsableGameId.IdX, IsNullable = false, Type = typeof(int), Options = 1 },
                new ColumnInfo() { Name = Col.NotUsableGameId.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.NotUsableGameId.Name, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.NotUsableGameId.Flag, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.NotUsableGameId.InsertDate, IsNullable = false, Type = typeof(DateTime) },
            };

            _primaryKey = new string[] { Col.NotUsableGameId.IdX };

            _keys = new List<string[]>() {
                new string[] { Col.NotUsableGameId.InsertDate },
                new string[] { Col.NotUsableGameId.Id },
                new string[] { Col.NotUsableGameId.Name },
            };
        }
    }

    public class PersonalRankingTable : BaseTable
    {
        public PersonalRankingTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.PersonalRanking;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.PersonalRanking.RankingId, IsNullable = false, Type = typeof(int), Options = 1 },
                new ColumnInfo() { Name = Col.PersonalRanking.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.PersonalRanking.Score, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.PersonalRanking.LastUpdate, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
            };

            _primaryKey = new string[] { Col.PersonalRanking.RankingId, Col.PersonalRanking.CharId, };
        }
    }

    public class PetTable : BaseTable
    {
        public PetTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.Pet;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.Pet.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Pet.Name, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.Pet.Type, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Pet.SkinColor, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.EyeType, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Pet.EyeColor, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.MouthType, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.Status, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Pet.Height, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.Fatness, IsNullable = false, Type = typeof(double) },

                new ColumnInfo() { Name = Col.Pet.Upper, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.Lower, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.Region, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Pet.X, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Pet.Y, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Pet.Direction, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.BattleState, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Pet.Extra1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Pet.Extra2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Pet.Extra3, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Pet.Life, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.LifeDamage, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.LifeMax, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.Mana, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.ManaMax, IsNullable = false, Type = typeof(double) },

                new ColumnInfo() { Name = Col.Pet.Stamina, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.StaminaMax, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.Food, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.Level, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Pet.Experience, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.Pet.Age, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Pet.Strength, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.Dexterity, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.Intelligence, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.Will, IsNullable = false, Type = typeof(double) },

                new ColumnInfo() { Name = Col.Pet.Luck, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.AttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Pet.AttackMax, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Pet.WAttackMin, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Pet.WAttackMax, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Pet.Critical, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.Protect, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.Defense, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Pet.Rate, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Pet.StrengthBoost, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Pet.DexterityBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.IntelligenceBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.WillBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.LuckBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.HeightBoost, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Pet.FatnessBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.UpperBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.LowerBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.LifeBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.ManaBoost, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Pet.StaminaBoost, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.Toxic, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.ToxicDrunkenTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Pet.ToxicStrength, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.ToxicIntelligence, IsNullable = false, Type = typeof(double) },

                new ColumnInfo() { Name = Col.Pet.ToxicDexterity, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.ToxicWill, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.ToxicLuck, IsNullable = false, Type = typeof(double) },
                new ColumnInfo() { Name = Col.Pet.LastTown, IsNullable = false, Type = typeof(char), Size = 100 },
                new ColumnInfo() { Name = Col.Pet.LastDungeon, IsNullable = false, Type = typeof(char), Size = 100 },

                new ColumnInfo() { Name = Col.Pet.UI, IsNullable = false, Type = typeof(string) },
                new ColumnInfo() { Name = Col.Pet.Meta, IsNullable = false, Type = typeof(char), Size = 1024 },
                new ColumnInfo() { Name = Col.Pet.Birthday, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Pet.PlayTime, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Pet.Wealth, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Pet.Condition, IsNullable = false, Type = typeof(char), Size = 1000 },
                new ColumnInfo() { Name = Col.Pet.Memory, IsNullable = false, Type = typeof(char), Size = 1100 },
                new ColumnInfo() { Name = Col.Pet.Reserved, IsNullable = false, Type = typeof(string) },
                new ColumnInfo() { Name = Col.Pet.Registered, IsNullable = false, Type = typeof(string) },
                new ColumnInfo() { Name = Col.Pet.Loyalty, IsNullable = false, Type = typeof(byte) },

                new ColumnInfo() { Name = Col.Pet.Favor, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Pet.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Pet.DeleteTime, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Pet.CumulatedLevel, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Pet.MaxLevel, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.Pet.RebirthCount, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Pet.RebirthDay, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Pet.RebirthAge, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Pet.WriteCounter, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Pet.MacroPoint, IsNullable = false, Type = typeof(int), DefaultValue = "2000" },

                new ColumnInfo() { Name = Col.Pet.CouponCode, IsNullable = false, Type = typeof(char), Size = 40, DefaultValue = "" },
                };

            _primaryKey = new string[] { Col.Pet.Id };
            _keys = new List<string[]>() { new string[] { Col.Pet.Name } };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.GameId, new string[] { Col.GameId.Id }, new string[] { Col.Pet.Id }, true, true) };
        }
    }


    public class PetSkillTable : BaseTable
    {
        public PetSkillTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.PetSkill;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.PetSkill.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.PetSkill.Skill, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.PetSkill.Level, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.PetSkill.Flag, IsNullable = false, Type = typeof(short) },
            };

            _primaryKey = new string[] { Col.PetSkill.Id, Col.PetSkill.Skill };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.Pet, new string[] { Col.Pet.Id }, new string[] { Col.PetSkill.Id }, true, true) };
        }
    }

    public class PetAssetTable : BaseTable
    {
        public PetAssetTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.PetAssetRank;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.PetAssetRank.Num, IsNullable = false, Type = typeof(long), Options = 1 },
                new ColumnInfo() { Name = Col.PetAssetRank.RegDate, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.PetAssetRank.PetId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.PetAssetRank.PetName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.PetAssetRank.TotalAsset, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.PetAssetRank.Rank, IsNullable = false, Type = typeof(byte) },
            };

            _primaryKey = new string[] { Col.PetAssetRank.Num };
        }
    }

    public class PlaytimeRankTable : BaseTable
    {
        public PlaytimeRankTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.PlaytimeRank;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.PlaytimeRank.Num, IsNullable = false, Type = typeof(long), Options = 1 },
                new ColumnInfo() { Name = Col.PlaytimeRank.RegDate, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.PlaytimeRank.Id, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.PlaytimeRank.Name, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.PlaytimeRank.Level, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.PlaytimeRank.PlayTime, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.PlaytimeRank.Experience, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.PlaytimeRank.Age, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.PlaytimeRank.CumulatedLevel, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
            };

            _primaryKey = new string[] { Col.PlaytimeRank.Num };

            _keys = new List<string[]>() { new string[] { Col.PlaytimeRank.RegDate } };
        }
    }

    public class PrivateFarmTable : BaseTable
    {
        public PrivateFarmTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.PrivateFarm;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.PrivateFarm.Id, IsNullable = false, Type = typeof(long), Options = 1 },
                new ColumnInfo() { Name = Col.PrivateFarm.OwnerId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.PrivateFarm.ClassId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PrivateFarm.Level, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PrivateFarm.Exp, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.PrivateFarm.UpdateTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.PrivateFarm.Name, IsNullable = false, Type = typeof(char), Size = 20 },
                new ColumnInfo() { Name = Col.PrivateFarm.WorldPosX, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.PrivateFarm.WorldPosY, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.PrivateFarm.CreateDate, IsNullable = true, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },

                new ColumnInfo() { Name = Col.PrivateFarm.DeleteFlag, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.PrivateFarm.OwnerName, IsNullable = false, Type = typeof(char), Size = 50, DefaultValue = "" },
                new ColumnInfo() { Name = Col.PrivateFarm.BindedChannel, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.PrivateFarm.NextBindableTime, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
            };

            _primaryKey = new string[] { Col.PrivateFarm.Id };

            _keys = new List<string[]>() { new string[] { Col.PrivateFarm.Name }, new string[] { Col.PrivateFarm.OwnerId } };
        }
    }

    public class PrivateFarmFacilityTable : BaseTable
    {
        public PrivateFarmFacilityTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.PrivateFarmFacility;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.PrivateFarmFacility.PrivateFarmId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.FacilityId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.ClassId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.X, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.Y, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.PrivateFarmFacility.Dir, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.Color2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.Color4, IsNullable = true, Type = typeof(int) },

                new ColumnInfo() { Name = Col.PrivateFarmFacility.Color5, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.Color6, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.Color7, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.Color8, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.Color9, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.PrivateFarmFacility.FinishTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.LastProcessingTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.Meta, IsNullable = false, Type = typeof(char), Size = 500 },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.CustomName, IsNullable = false, Type = typeof(char), Size = 16 },
                new ColumnInfo() { Name = Col.PrivateFarmFacility.LinkedFacilityId, IsNullable = false, Type = typeof(long), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.PrivateFarmFacility.PermissionFlag, IsNullable = false, Type = typeof(long), DefaultValue = "0" },
            };

            _primaryKey = new string[] { Col.PrivateFarmFacility.FacilityId };

            _keys = new List<string[]>() { new string[] { Col.PrivateFarmFacility.PrivateFarmId } };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.PrivateFarm, new string[] { Col.PrivateFarm.Id }, new string[] { Col.PrivateFarmFacility.PrivateFarmId }, true, true) };
        }
    }

    public class PrivateFarmFacilityIdPoolTable : BaseTable
    {
        public PrivateFarmFacilityIdPoolTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.PrivateFarmFacilityIdPool;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.PrivateFarmFacilityIdPool.Count, IsNullable = true, Type = typeof(long) },
            };
        }
    }

    public class PrivateFarmVisitorTable : BaseTable
    {
        public PrivateFarmVisitorTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.PrivateFarmVisitor;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.PrivateFarmVisitor.PrivateFarmId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.PrivateFarmVisitor.CharName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.PrivateFarmVisitor.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.PrivateFarmVisitor.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.PrivateFarmVisitor.Status, IsNullable = false, Type = typeof(byte) },
            };

            _primaryKey = new string[] { Col.PrivateFarmVisitor.PrivateFarmId, Col.PrivateFarmVisitor.CharId };

            _keys = new List<string[]>() { new string[] { Col.PrivateFarmVisitor.CharId } };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.PrivateFarm, new string[] { Col.PrivateFarm.Id }, new string[] { Col.PrivateFarmVisitor.PrivateFarmId }, true, true) };
        }
    }

    public class PromotionRankTable : BaseTable
    {
        public PromotionRankTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.PromotionRank;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.PromotionRank.Server, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.PromotionRank.SkillId, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.PromotionRank.SkillCategory, IsNullable = false, Type = typeof(char), Size = 20 },
                new ColumnInfo() { Name = Col.PromotionRank.SkillName, IsNullable = false, Type = typeof(char), Size = 20 },
                new ColumnInfo() { Name = Col.PromotionRank.CharacterId, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.PromotionRank.CharacterName, IsNullable = false, Type = typeof(char), Size = 50},
                new ColumnInfo() { Name = Col.PromotionRank.Race, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.PromotionRank.Level, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.PromotionRank.Point, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PromotionRank.RegDate, IsNullable = false, Type = typeof(DateTime) },

                new ColumnInfo() { Name = Col.PromotionRank.Rank, IsNullable = false, Type = typeof(int) },
            };

            _primaryKey = new string[] { Col.PromotionRank.CharacterId, Col.PromotionRank.Server, Col.PromotionRank.SkillId };

            _keys = new List<string[]>() { new string[] { Col.PromotionRank.Server, Col.PromotionRank.SkillId } };
        }
    }

    public class PromotionRecordTable : BaseTable
    {
        public PromotionRecordTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.PromotionRecord;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.PromotionRecord.Server, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.PromotionRecord.SkillId, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.PromotionRecord.SkillCategory, IsNullable = false, Type = typeof(char), Size = 20 },
                new ColumnInfo() { Name = Col.PromotionRecord.SkillName, IsNullable = false, Type = typeof(char), Size = 20 },
                new ColumnInfo() { Name = Col.PromotionRecord.CharacterId, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.PromotionRecord.CharacterName, IsNullable = false, Type = typeof(char), Size = 50},
                new ColumnInfo() { Name = Col.PromotionRecord.Race, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.PromotionRecord.Level, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.PromotionRecord.Point, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PromotionRecord.RegDate, IsNullable = false, Type = typeof(DateTime) },

                new ColumnInfo() { Name = Col.PromotionRecord.Channel, IsNullable = false, Type = typeof(char), Size = 10, DefaultValue = "" },
            };

            _primaryKey = new string[] { Col.PromotionRecord.CharacterId, Col.PromotionRecord.Server, Col.PromotionRecord.SkillId };

            _keys = new List<string[]>() { new string[] { Col.PromotionRecord.Server, Col.PromotionRecord.SkillId } };
        }
    }

    public class PropTable : BaseTable
    {
        public PropTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.Prop;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.Prop.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Prop.ClassId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Prop.Region, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Prop.X, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Prop.Y, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Prop.Z, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Prop.Direction, IsNullable = false, Type = typeof(float) },
                new ColumnInfo() { Name = Col.Prop.Scale, IsNullable = false, Type = typeof(float) },
                new ColumnInfo() { Name = Col.Prop.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Prop.Color2, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Prop.Color3, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Prop.Color4, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Prop.Color5, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Prop.Color6, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Prop.Color7, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Prop.Color8, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Prop.Color9, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Prop.Name, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.Prop.State, IsNullable = false, Type = typeof(char), Size = 512 },
                new ColumnInfo() { Name = Col.Prop.EnterTime, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.Prop.Extra, IsNullable = false, Type = typeof(char), Size = 2000 },
            };

            _primaryKey = new string[] { Col.Prop.Id };
        }
    }

    public class PropEventTable : BaseTable
    {
        public PropEventTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.PropEvent;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.PropEvent.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.PropEvent.Default, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.PropEvent.Signal, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PropEvent.Type, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.PropEvent.Extra, IsNullable = false, Type = typeof(char), Size = 2000 },
            };

            _keys = new List<string[]>() { new string[] { Col.PropEvent.Id } };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.Prop, new string[] { Col.Prop.Id }, new string[] { Col.PropEvent.Id }, true, true) };
        }
    }

    public class PropIdPoolTable : BaseTable
    {
        public PropIdPoolTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.PropIdPool;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.PropIdPool.Count, IsNullable = true, Type = typeof(long) },
            };
        }
    }

    public class GoldLogTable : BaseTable
    {
        public GoldLogTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.GoldLog;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.GoldLog.IdX, IsNullable = false, Type = typeof(int), Options = 1 },
                new ColumnInfo() { Name = Col.GoldLog.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.GoldLog.Quest, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GoldLog.Field, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GoldLog.Commerce, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.GoldLog.Mail, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GoldLog.Bank, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GoldLog.ItemBuySell, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GoldLog.ItemRepair, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GoldLog.ItemUpgrade, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.GoldLog.ItemSpecialUpgrade, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GoldLog.Mint, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GoldLog.Guild, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GoldLog.PrivateShop, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GoldLog.Housing, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.GoldLog.Etc, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GoldLog.LogDate, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.GoldLog.DynamicRegion, IsNullable = false, Type = typeof(int) },
            };

            _primaryKey = new string[] { Col.GoldLog.IdX };

            _keys = new List<string[]>() { new string[] { Col.GoldLog.LogDate, Col.GoldLog.Id } };
        }
    }

    public class RelicTable : BaseTable
    {
        public RelicTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.Relic;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.Relic.RuinId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Relic.State, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Relic.Position, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Relic.LastTime, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Relic.ExploCharId, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.Relic.ExploCharName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.Relic.ExploTime, IsNullable = true, Type = typeof(DateTime) },
            };

            _primaryKey = new string[] { Col.Relic.RuinId };
        }
    }

    public class RenewalQuestListTable : BaseTable
    {
        public RenewalQuestListTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.RenewalQuestList;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.RenewalQuestList.IdX, IsNullable = false, Type = typeof(int), Options = 1 },
                new ColumnInfo() { Name = Col.RenewalQuestList.QuestId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.RenewalQuestList.EditQuestId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.RenewalQuestList.Flag, IsNullable = false, Type = typeof(int) },
            };

            _keys = new List<string[]>() { new string[] { Col.RenewalQuestList.IdX } };
        }
    }

    public class ReportCharacterLevelTable : BaseTable
    {
        public ReportCharacterLevelTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ReportCharacterLevel;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.ReportCharacterLevel.RegDate, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.ReportCharacterLevel.Race, IsNullable = true, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ReportCharacterLevel.Level, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ReportCharacterLevel.Count, IsNullable = true, Type = typeof(int) },
            };

            _keys = new List<string[]>() { new string[] { Col.ReportCharacterLevel.RegDate } };
        }
    }

    public class ReportCharacterSkillTable : BaseTable
    {
        public ReportCharacterSkillTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ReportCharacterSkill;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.ReportCharacterSkill.RegDate, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.ReportCharacterSkill.Race, IsNullable = true, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ReportCharacterSkill.SkillId, IsNullable = true, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ReportCharacterSkill.Level, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ReportCharacterSkill.Count, IsNullable = true, Type = typeof(int) },
            };

            _keys = new List<string[]>() { new string[] { Col.ReportCharacterSkill.RegDate } };
        }
    }

    public class ReportDateListTable : BaseTable
    {
        public ReportDateListTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ReportDateList;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.ReportDateList.IdX, IsNullable = false, Type = typeof(int), Options = 1 },
                new ColumnInfo() { Name = Col.ReportDateList.RegDate, IsNullable = false, Type = typeof(DateTime) },
            };

            _keys = new List<string[]>() { new string[] { Col.ReportDateList.IdX } };
        }
    }

    public class RoyalAlchemistTable : BaseTable
    {
        public RoyalAlchemistTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.RoyalAlchemist;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.RoyalAlchemist.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.RoyalAlchemist.CharName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.RoyalAlchemist.RegistrationFlag, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.RoyalAlchemist.Rank, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.RoyalAlchemist.Meta, IsNullable = false, Type = typeof(char), Size = 200 },
            };

            _primaryKey = new string[] { Col.RoyalAlchemist.CharId };
        }
    }

    public class RuinTable : BaseTable
    {
        public RuinTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.Ruin;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.Ruin.RuinId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Ruin.State, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Ruin.Position, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Ruin.LastTime, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Ruin.ExploCharId, IsNullable = false, Type = typeof(long) },


                new ColumnInfo() { Name = Col.Ruin.ExploCharName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.Ruin.ExploTime, IsNullable = true, Type = typeof(DateTime) },
            };

            _primaryKey = new string[] { Col.Ruin.RuinId };
        }
    }

    public class ScrapBookTable : BaseTable
    {
        public ScrapBookTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ScrapBook;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.ScrapBook.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.ScrapBook.ScrapType, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ScrapBook.ClassId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ScrapBook.ScrapData, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ScrapBook.RegionId, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.ScrapBook.UpdateTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
            };

            _primaryKey = new string[] { Col.ScrapBook.CharId, Col.ScrapBook.ScrapType, Col.ScrapBook.ClassId, Col.ScrapBook.ScrapData };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.Mabinogi.Character, new string[] { Col.Character.Id }, new string[] { Col.ScrapBook.CharId }, true, true) };
        }
    }

    public class ScrapBookBestCookTable : BaseTable
    {
        public ScrapBookBestCookTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ScrapBookBestCook;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.ScrapBookBestCook.ClassId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ScrapBookBestCook.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.ScrapBookBestCook.Name, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.ScrapBookBestCook.Quality, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ScrapBookBestCook.Comment, IsNullable = false, Type = typeof(char), Size = 100, DefaultValue = "" },


                new ColumnInfo() { Name = Col.ScrapBookBestCook.UpdateTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
            };

            _primaryKey = new string[] { Col.ScrapBookBestCook.ClassId };
        }
    }

    public class ServerAssetInfoTable : BaseTable
    {
        public ServerAssetInfoTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.ServerAssetInfo;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.ServerAssetInfo.Num, IsNullable = false, Type = typeof(int), Options = 1 },
                new ColumnInfo() { Name = Col.ServerAssetInfo.RegDate, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.ServerAssetInfo.TotalAsset, IsNullable = true, Type = typeof(long) },
            };

            _primaryKey = new string[] { Col.ServerAssetInfo.Num };

            _keys = new List<string[]>() { new string[] { Col.ServerAssetInfo.RegDate } };
        }
    }

    public class SoulMateTable : BaseTable
    {
        public SoulMateTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.SoulMate;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.SoulMate.MainCharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.SoulMate.SubCharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.SoulMate.MatePoint, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.SoulMate.StartTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.SoulMate.UpdateTime, IsNullable = false, Type = typeof(DateTime) },

            };

            _primaryKey = new string[] { Col.SoulMate.MainCharId };


            _keys = new List<string[]>() { new string[] { Col.SoulMate.SubCharId } };
        }
    }

    public class WineTable : BaseTable
    {
        public WineTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.Wine;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.Wine.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Wine.WineType, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Wine.AgingCount, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Wine.AgingStartTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Wine.LastRackingTime, IsNullable = false, Type = typeof(DateTime) },


                new ColumnInfo() { Name = Col.Wine.Acidity, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Wine.Purity, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Wine.Freshness, IsNullable = false, Type = typeof(int) },
            };

            _primaryKey = new string[] { Col.Wine.CharId };
        }
    }

    public class WorldMetaTable : BaseTable
    {
        public WorldMetaTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabinogi.WorldMeta;
            _dbName = "Mabinogi";
            _columns =
            new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.WorldMeta.MetaKey, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.WorldMeta.MetaType, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.WorldMeta.MetaValue, IsNullable = false, Type = typeof(char), Size = 512 },
            };

            _primaryKey = new string[] { Col.WorldMeta.MetaKey };
        }
    }
    #endregion MabinogiDB

    #region MabiGuildDB
    public class AwayGuildMemberTable : BaseTable
    {
        public AwayGuildMemberTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiGuild.AwayGuildMember;
            _dbName = "MabiGuild";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.AwayGuildMember.MemberId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.AwayGuildMember.Server, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.AwayGuildMember.GuildId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.AwayGuildMember.JoinTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.AwayGuildMember.Name, IsNullable = false, Type = typeof(char), Size = 32 },

                new ColumnInfo() { Name = Col.AwayGuildMember.Account, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.AwayGuildMember.AwayTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
            };

            _primaryKey = new string[] { Col.AwayGuildMember.MemberId, Col.AwayGuildMember.Server };
        }
    }

    public class GuildTable : BaseTable
    {
        public GuildTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiGuild.Guild;
            _dbName = "MabiGuild";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.Guild.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Guild.Name, IsNullable = false, Type = typeof(char), Size = 128},
                new ColumnInfo() { Name = Col.Guild.Server, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.Guild.GuildPoint, IsNullable = false, Type = typeof(int)},
                new ColumnInfo() { Name = Col.Guild.GuildMoney, IsNullable = false, Type = typeof(char), Size = 32},

                new ColumnInfo() { Name = Col.Guild.GuildType, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Guild.JoinType, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Guild.MaxMember, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Guild.MemberCount, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Guild.GuildAbility, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.Guild.CreateTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
                new ColumnInfo() { Name = Col.Guild.UpdateTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Guild.GuildMasterId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Guild.WebMemberCount, IsNullable = false, Type = typeof(int)},
                new ColumnInfo() { Name = Col.Guild.Expiration, IsNullable = false, Type = typeof(DateTime) },

                new ColumnInfo() { Name = Col.Guild.Enable, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Guild.MasterChangeTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
                new ColumnInfo() { Name = Col.Guild.DrawableMoney, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Guild.DrawableDate, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
                new ColumnInfo() { Name = Col.Guild.BattleGroundType, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Guild.BattleGroundWinnerType, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Guild.GuildStatusFlag, IsNullable = false, Type = typeof(byte), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Guild.GuildTitle, IsNullable = false, Type = typeof(char), Size = 20, DefaultValue = "" },
            };

            _primaryKey = new string[] { Col.Guild.Id };

            _keys = new List<string[]>()
            {
                new string[] { Col.Guild.Name },
                new string[] { Col.Guild.Server, Col.Guild.Id },
                new string[] { Col.Guild.Server, Col.Guild.GuildType, Col.Guild.GuildMasterId, Col.Guild.MemberCount, Col.Guild.Id },
                new string[] { Col.Guild.Server, Col.Guild.MaxMember, Col.Guild.GuildMasterId, Col.Guild.MemberCount, Col.Guild.Id },
                new string[] { Col.Guild.Server, Col.Guild.MemberCount, Col.Guild.GuildMasterId, Col.Guild.Id },
            };
        }
    }

    public class GuildDeletedTable : BaseTable
    {
        public GuildDeletedTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiGuild.GuildDeleted;
            _dbName = "MabiGuild";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.GuildDeleted.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.GuildDeleted.Name, IsNullable = false, Type = typeof(char), Size = 64 },
                new ColumnInfo() { Name = Col.GuildDeleted.Server, IsNullable = false, Type = typeof(char), Size = 64 },
                new ColumnInfo() { Name = Col.GuildDeleted.DeleteTime, IsNullable = false, Type = typeof(DateTime) },
            };
        }
    }

    public class GuildMenuTable : BaseTable
    {
        public GuildMenuTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiGuild.GuildMenu;
            _dbName = "MabiGuild";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.GuildMenu.GuildId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.GuildMenu.MenuId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.GuildMenu.MenuName, IsNullable = false, Type = typeof(char), Size = 25 },
                new ColumnInfo() { Name = Col.GuildMenu.Level1, IsNullable = false, Type = typeof(char), Size = 10 },
                new ColumnInfo() { Name = Col.GuildMenu.Level2, IsNullable = false, Type = typeof(char), Size = 10 },

                new ColumnInfo() { Name = Col.GuildMenu.Level3, IsNullable = false, Type = typeof(char), Size = 10 },
                new ColumnInfo() { Name = Col.GuildMenu.Level4, IsNullable = false, Type = typeof(char), Size = 10 },
            };

            _keys = new List<string[]>()
            {
                new string[] { Col.GuildMenu.GuildId },
                new string[] { Col.GuildMenu.GuildId, Col.GuildMenu.MenuId },
            };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.MabiGuild.Guild, new string[] { Col.Guild.Id }, new string[] { Col.GuildMenu.GuildId }, true, true) };
        }
    }

    public class GuildRobeTable : BaseTable
    {
        public GuildRobeTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiGuild.GuildRobe;
            _dbName = "MabiGuild";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.GuildRobe.GuildId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.GuildRobe.EmblemChestIcon, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.GuildRobe.EmblemChestDeco, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.GuildRobe.EmblemBeltDeco, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.GuildRobe.Color1, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.GuildRobe.Color2Index, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.GuildRobe.Color3Index, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.GuildRobe.Color4Index, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.GuildRobe.Color5Index, IsNullable = false, Type = typeof(byte) },
            };
            _primaryKey = new string[] { Col.GuildRobe.GuildId };


            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.MabiGuild.Guild, new string[] { Col.Guild.Id }, new string[] { Col.GuildRobe.GuildId }, true, true) };
        }
    }

    public class GuildStoneTable : BaseTable
    {
        public GuildStoneTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiGuild.GuildStone;
            _dbName = "MabiGuild";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.GuildStone.GuildId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.GuildStone.Server, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.GuildStone.PositionId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.GuildStone.Type, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GuildStone.Region, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.GuildStone.X, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GuildStone.Y, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GuildStone.Direction, IsNullable = false, Type = typeof(float) },
            };
            _primaryKey = new string[] { Col.GuildStone.GuildId };

            _keys = new List<string[]>()
            {
                new string[] { Col.GuildStone.Server, Col.GuildStone.PositionId },
            };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.MabiGuild.Guild, new string[] { Col.Guild.Id }, new string[] { Col.GuildStone.GuildId }, true, true) };
        }
    }

    public class GuildTextTable : BaseTable
    {
        public GuildTextTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiGuild.GuildText;
            _dbName = "MabiGuild";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.GuildText.GuildId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.GuildText.Profile, IsNullable = false, Type = typeof(char), Size = 512 },
                new ColumnInfo() { Name = Col.GuildText.Greeting, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.GuildText.Leaving, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.GuildText.Refuse, IsNullable = false, Type = typeof(char), Size = 128 },

                new ColumnInfo() { Name = Col.GuildText.Emblem, IsNullable = false, Type = typeof(char), Size = 32 },
            };

            _keys = new List<string[]>() { new string[] { Col.GuildText.GuildId } };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.MabiGuild.Guild, new string[] { Col.Guild.Id }, new string[] { Col.GuildText.GuildId }, true, true) };
        }
    }

    public class GuildIdPoolTable : BaseTable
    {
        public GuildIdPoolTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiGuild.GuildIdPool;
            _dbName = "MabiGuild";
            _columns =
            new List<ColumnInfo>()
                {
                new ColumnInfo() { Name = Col.GuildIdPool.Count, IsNullable = false, Type = typeof(long) }
                };
        }
    }

    public class GuildMemberTable : BaseTable
    {
        public GuildMemberTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiGuild.GuildMember;
            _dbName = "MabiGuild";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.GuildMember.GuildId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.GuildMember.Id, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.GuildMember.Name, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.GuildMember.Account, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.GuildMember.Class, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.GuildMember.Point, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.GuildMember.JoinTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.GuildMember.Text, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.GuildMember.JoinMsg, IsNullable = false, Type = typeof(char), Size = 100, DefaultValue = "" },
            };

            _keys = new List<string[]>()
            {
                new string[] { Col.GuildMember.Id, Col.GuildMember.GuildId },
                new string[] { Col.GuildMember.GuildId },
                new string[] { Col.GuildMember.Name },
            };

            _foreignKeys = new ForeignKey[] { new ForeignKey(Mabinogi.SQL.Tables.MabiGuild.Guild, new string[] { Col.Guild.Id }, new string[] { Col.GuildMember.GuildId }, true, true) };
        }
    }

    #endregion MabiGuildDB

    #region MabiChronicle
    public class ChronicleTable : BaseTable
    {
        public ChronicleTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiChronicle.Chronicle;
            _dbName = "MabiChronicle";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.Chronicle.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Chronicle.ServerName, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.Chronicle.QuestId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Chronicle.CreateTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Chronicle.Meta, IsNullable = false, Type = typeof(char), Size = 60 },
            };
            _keys = new List<string[]>() { new string[] { Col.Chronicle.CharId, Col.Chronicle.ServerName } };

        }
    }

    public class ChronicleEventRankTable : BaseTable
    {
        public ChronicleEventRankTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiChronicle.ChronicleEventRank;
            _dbName = "MabiChronicle";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.ChronicleEventRank.ServerName, IsNullable = false, Type = typeof(char), Size = 50},
                new ColumnInfo() { Name = Col.ChronicleEventRank.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.ChronicleEventRank.QuestId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ChronicleEventRank.CharName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.ChronicleEventRank.Rank, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.ChronicleEventRank.EventCount, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ChronicleEventRank.CountRank, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.ChronicleEventRank.UpdateTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
            };

            _primaryKey = new string[] { Col.ChronicleEventRank.ServerName, Col.ChronicleEventRank.CharId, Col.ChronicleEventRank.QuestId, };
        }
    }

    public class ChronicleFirstRankTable : BaseTable
    {
        public ChronicleFirstRankTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiChronicle.ChronicleFirstRank;
            _dbName = "MabiChronicle";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.ChronicleFirstRank.ServerName, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.ChronicleFirstRank.QuestId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ChronicleFirstRank.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.ChronicleFirstRank.CharName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.ChronicleFirstRank.RankTime, IsNullable = false, Type = typeof(DateTime) },
            };

            _keys = new List<string[]>() { new string[] { Col.ChronicleEventRank.ServerName, Col.ChronicleEventRank.QuestId, } };
        }
    }

    public class ChronicleInfoTable : BaseTable
    {
        public ChronicleInfoTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiChronicle.ChronicleInfo;
            _dbName = "MabiChronicle";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.ChronicleInfo.QuestId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ChronicleInfo.QuestName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.ChronicleInfo.Keyword, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.ChronicleInfo.LocalText, IsNullable = false, Type = typeof(char), Size = 100 },
                new ColumnInfo() { Name = Col.ChronicleInfo.Source, IsNullable = false, Type = typeof(char), Size = 50 },

                new ColumnInfo() { Name = Col.ChronicleInfo.Width, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ChronicleInfo.Height, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.ChronicleInfo.Sort, IsNullable = false, Type = typeof(char), Size = 10 },
                new ColumnInfo() { Name = Col.ChronicleInfo.Group, IsNullable = false, Type = typeof(char), Size = 10 },
            };

            _primaryKey = new string[] { Col.ChronicleInfo.QuestId };
        }
    }

    public class ChronicleLatestRankTable : BaseTable
    {
        public ChronicleLatestRankTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiChronicle.ChronicleLatestRank;
            _dbName = "MabiChronicle";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.ChronicleLatestRank.Id, IsNullable = false, Type = typeof(int), Options = 1 },
                new ColumnInfo() { Name = Col.ChronicleLatestRank.ServerName, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.ChronicleLatestRank.QuestId, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ChronicleLatestRank.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.ChronicleLatestRank.CharName, IsNullable = false, Type = typeof(char), Size = 50 },

                new ColumnInfo() { Name = Col.ChronicleLatestRank.RankTime, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.ChronicleLatestRank.Rank, IsNullable = false, Type = typeof(int) }
            };

            _primaryKey = new string[] { Col.ChronicleLatestRank.Id };

            _keys = new List<string[]>()
            {
                new string[] { Col.ChronicleLatestRank.ServerName,  Col.ChronicleLatestRank.QuestId },
            };
        }
    }
    #endregion MabiChronicle

    #region MabiNovel
    public class MabiNovelTable : BaseTable
    {
        public MabiNovelTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabi_Novel.MabiNovel;
            _dbName = "MabiNovel";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.MabiNovel.BoardSn, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.MabiNovel.Page, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.MabiNovel.BackgroundId, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.MabiNovel.BgmId, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.MabiNovel.PortraitId, IsNullable = false, Type = typeof(short) },

                new ColumnInfo() { Name = Col.MabiNovel.PortraitPos, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.MabiNovel.EmotionId, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.MabiNovel.SoundEffectId, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.MabiNovel.EffectId, IsNullable = false, Type = typeof(short) },
                new ColumnInfo() { Name = Col.MabiNovel.Ambassador, IsNullable = false, Type = typeof(char), Size = 200 },
            };
            _primaryKey = new string[] { Col.MabiNovel.BoardSn, Col.MabiNovel.Page };
        }
    }

    public class MabiNovelBoardTable : BaseTable
    {
        public MabiNovelBoardTable()
        {
            _tableName = Mabinogi.SQL.Tables.Mabi_Novel.MabiNovelBoard;
            _dbName = "MabiNovel";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.MabiNovelBoard.BoardSn, IsNullable = false, Type = typeof(long), Options = 1, Size = 1000 },
                new ColumnInfo() { Name = Col.MabiNovelBoard.Server, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.MabiNovelBoard.CharId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.MabiNovelBoard.Title, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.MabiNovelBoard.TransCount, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.MabiNovelBoard.EndDate, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.MabiNovelBoard.BlockCount, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.MabiNovelBoard.Flag, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.MabiNovelBoard.ReadCount, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.MabiNovelBoard.BlockDate, IsNullable = true, Type = typeof(DateTime) },

                new ColumnInfo() { Name = Col.MabiNovelBoard.UpdateTime, IsNullable = true, Type = typeof(DateTime) },
            };
            _primaryKey = new string[] { Col.MabiNovelBoard.Server, Col.MabiNovelBoard.CharId, Col.MabiNovelBoard.Title };

            _keys = new List<string[]>() { new string[] { Col.MabiNovelBoard.BoardSn } };
        }
    }
    #endregion MabiNovel

    #region MabiMemo
    public class MemoTable : BaseTable
    {
        public MemoTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiMemo.Memo;
            _dbName = "MabiMemo";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.Memo.Id, IsNullable = false, Type = typeof(long), Options = 1, Size = 1 },
                new ColumnInfo() { Name = Col.Memo.FromName, IsNullable = false, Type = typeof(char), Size = 30 },
                new ColumnInfo() { Name = Col.Memo.FromId, IsNullable = false, Type = typeof(char), Size = 30 },
                new ColumnInfo() { Name = Col.Memo.ToName, IsNullable = false, Type = typeof(char), Size = 30 },
                new ColumnInfo() { Name = Col.Memo.ToId, IsNullable = false, Type = typeof(char), Size = 30 },

                new ColumnInfo() { Name = Col.Memo.FromDate, IsNullable = false, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Memo.ToDate, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Memo.Content, IsNullable = false, Type = typeof(string) },
                new ColumnInfo() { Name = Col.Memo.ToCheck, IsNullable = false, Type = typeof(short), DefaultValue = "0" },
                new ColumnInfo() { Name = Col.Memo.FromCheck, IsNullable = true, Type = typeof(short), DefaultValue = "0" },


                new ColumnInfo() { Name = Col.Memo.FromServer, IsNullable = false, Type = typeof(char), Size = 20, DefaultValue = "test" },
                new ColumnInfo() { Name = Col.Memo.FromLevel, IsNullable = false, Type = typeof(short), DefaultValue = "5" },
                new ColumnInfo() { Name = Col.Memo.ToServer, IsNullable = false, Type = typeof(char), Size = 20, DefaultValue = "test" },
                new ColumnInfo() { Name = Col.Memo.ToLevel, IsNullable = true, Type = typeof(short) },
            };
            _primaryKey = new string[] { Col.Memo.Id };

            _keys = new List<string[]>()
            {
                new string[] { Col.Memo.ToId },
                new string[] { Col.Memo.FromId }
            };
        }
    }

    public class MemoBlacklistTable : BaseTable
    {
        public MemoBlacklistTable()
        {
            _tableName = Mabinogi.SQL.Tables.MabiMemo.MemoBlacklist;
            _dbName = "MabiMemo";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.MemoBlacklist.Id, IsNullable = false, Type = typeof(long), Options = 1, Size = 1 },
                new ColumnInfo() { Name = Col.MemoBlacklist.FromId, IsNullable = false, Type = typeof(char), Size = 30 },
                new ColumnInfo() { Name = Col.MemoBlacklist.ToId, IsNullable = false, Type = typeof(char), Size = 30 },
                new ColumnInfo() { Name = Col.MemoBlacklist.FromName, IsNullable = false, Type = typeof(char), Size = 30 },
                new ColumnInfo() { Name = Col.MemoBlacklist.ToName, IsNullable = false, Type = typeof(char), Size = 30 },

                new ColumnInfo() { Name = Col.MemoBlacklist.ToServer, IsNullable = true, Type = typeof(char), Size = 30 },
                new ColumnInfo() { Name = Col.MemoBlacklist.ToLevel, IsNullable = true, Type = typeof(short) },
            };
            _primaryKey = new string[] { Col.MemoBlacklist.Id };

            _keys = new List<string[]>()
            {
                new string[] { Col.MemoBlacklist.FromId }
            };
        }
    }
    #endregion MabiMemo

    #region DungeonRank
    public class DungeonScoreBoardTable : BaseTable
    {
        public DungeonScoreBoardTable()
        {
            _tableName = Mabinogi.SQL.Tables.DungeonRank.DungeonScoreBoard;
            _dbName = "MabiDungeonRank";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.DungeonScoreBoard.Idx, IsNullable = false, Type = typeof(int), Options = 1, Size = 1 },
                new ColumnInfo() { Name = Col.DungeonScoreBoard.Server, IsNullable = false, Type = typeof(char), Size = 20 },
                new ColumnInfo() { Name = Col.DungeonScoreBoard.DungeonName, IsNullable = false, Type = typeof(char), Size = 80 },
                new ColumnInfo() { Name = Col.DungeonScoreBoard.Race, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.DungeonScoreBoard.CharacterId, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.DungeonScoreBoard.CharacterName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.DungeonScoreBoard.Score, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.DungeonScoreBoard.RegDate, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
            };
            _primaryKey = new string[] { Col.DungeonScoreBoard.Idx };

            _keys = new List<string[]>()
            {
                new string[] { Col.DungeonScoreBoard.Server, Col.DungeonScoreBoard.DungeonName, Col.DungeonScoreBoard.Score, Col.DungeonScoreBoard.Race },
                new string[] { Col.DungeonScoreBoard.CharacterId, Col.DungeonScoreBoard.Server },
            };
        }
    }

    public class DungeonScoreRankInfoTable : BaseTable
    {
        public DungeonScoreRankInfoTable()
        {
            _tableName = Mabinogi.SQL.Tables.DungeonRank.DungeonScoreRankInfo;
            _dbName = "MabiDungeonRank";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.DungeonScoreRankInfo.Server, IsNullable = false, Type = typeof(char), Size = 20 },
                new ColumnInfo() { Name = Col.DungeonScoreRankInfo.DungeonName, IsNullable = false, Type = typeof(char), Size = 80 },
                new ColumnInfo() { Name = Col.DungeonScoreRankInfo.Race, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.DungeonScoreRankInfo.ScoreRow, IsNullable = false, Type = typeof(int) },
            };

            _primaryKey = new string[] { Col.DungeonScoreRankInfo.Server, Col.DungeonScoreRankInfo.DungeonName, Col.DungeonScoreRankInfo.Race };
        }
    }

    public class DungeonTimeBoardTable : BaseTable
    {
        public DungeonTimeBoardTable()
        {
            _tableName = Mabinogi.SQL.Tables.DungeonRank.DungeonTimeBoard;
            _dbName = "MabiDungeonRank";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.DungeonTimeBoard.Idx, IsNullable = false, Type = typeof(int), Options = 1, Size = 1 },
                new ColumnInfo() { Name = Col.DungeonTimeBoard.Server, IsNullable = false, Type = typeof(char), Size = 20 },
                new ColumnInfo() { Name = Col.DungeonTimeBoard.DungeonName, IsNullable = false, Type = typeof(char), Size = 80 },
                new ColumnInfo() { Name = Col.DungeonTimeBoard.Race, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.DungeonTimeBoard.CharacterId, IsNullable = false, Type = typeof(long) },

                new ColumnInfo() { Name = Col.DungeonTimeBoard.CharacterName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.DungeonTimeBoard.LapTime, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.DungeonTimeBoard.RegDate, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
            };
            _primaryKey = new string[] { Col.DungeonTimeBoard.Idx };

            _keys = new List<string[]>()
            {
                new string[] { Col.DungeonTimeBoard.Server, Col.DungeonTimeBoard.DungeonName, Col.DungeonTimeBoard.LapTime, Col.DungeonTimeBoard.Race },
                new string[] { Col.DungeonTimeBoard.CharacterId, Col.DungeonTimeBoard.Server },
            };
        }
    }

    public class DungeonTimeRankInfoTable : BaseTable
    {
        public DungeonTimeRankInfoTable()
        {
            _tableName = Mabinogi.SQL.Tables.DungeonRank.DungeonTimeRankInfo;
            _dbName = "MabiDungeonRank";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.DungeonTimeRankInfo.Server, IsNullable = false, Type = typeof(char), Size = 20 },
                new ColumnInfo() { Name = Col.DungeonTimeRankInfo.DungeonName, IsNullable = false, Type = typeof(char), Size = 80 },
                new ColumnInfo() { Name = Col.DungeonTimeRankInfo.Race, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.DungeonTimeRankInfo.TimeRow, IsNullable = false, Type = typeof(int) },
            };

            _primaryKey = new string[] { Col.DungeonTimeRankInfo.Server, Col.DungeonTimeRankInfo.DungeonName, Col.DungeonTimeRankInfo.Race };
        }
    }
    #endregion DungeonRank

    #region MabiShop
    public class FantasyLifeClubTable : BaseTable
    {
        public FantasyLifeClubTable()
        {
            _tableName = Tables.Shop.FantasyLifeClub;
            _dbName = "MabiShop";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.FantasyLifeClub.Id, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.FantasyLifeClub.NaoSupportExpiration, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.FantasyLifeClub.StorageExpiration, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.FantasyLifeClub.AdvancedPlayExpiration, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.FantasyLifeClub.Updated, IsNullable = true, Type = typeof(DateTime) },

                new ColumnInfo() { Name = Col.FantasyLifeClub.Checked, IsNullable = true, Type = typeof(DateTime) },
            };

            _primaryKey = new string[] { Col.FantasyLifeClub.Id };
        }
    }

    public class PremiumPackTable : BaseTable
    {
        public PremiumPackTable()
        {
            _tableName = Tables.Shop.PremiumPack;
            _dbName = "MabiShop";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.PremiumPack.Id, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.PremiumPack.InventoryPlus, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.PremiumPack.PremPack, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.PremiumPack.VIP, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.PremiumPack.PremiumVIP, IsNullable = true, Type = typeof(DateTime) },

                new ColumnInfo() { Name = Col.PremiumPack.GuildPack, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.PremiumPack.Updated, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.PremiumPack.Checked, IsNullable = true, Type = typeof(DateTime) },
            };

            _primaryKey = new string[] { Col.PremiumPack.Id };
        }
    }

    public class CharacterCardsTable : BaseTable
    {
        public CharacterCardsTable()
        {
            _tableName = Tables.Shop.CharacterCards;
            _dbName = "MabiShop";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.Cards.Id, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.Cards.CardId, IsNullable = false, Type = typeof(long), Options = 1, Size = 1 },
                //new ColumnInfo() { Name = Col.Cards.TypeId, IsNullable = false, Type = typeof(char), Size = 36 },
                new ColumnInfo() { Name = Col.Cards.Type, IsNullable = false, Type = typeof(char), Size = 16 },
                new ColumnInfo() { Name = Col.Cards.Status, IsNullable = false, Type = typeof(int), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Cards.EntityId, IsNullable = true, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Cards.EntityName, IsNullable = true, Type = typeof(char), Size = 64 },
                new ColumnInfo() { Name = Col.Cards.RebirthCount, IsNullable = true, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Cards.Server, IsNullable = true, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.Cards.Reserved, IsNullable = true, Type = typeof(char), Size = 128 },

                new ColumnInfo() { Name = Col.Cards.Created, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Cards.Used, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Cards.Ended, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Cards.ValidServer, IsNullable = true, Type = typeof(char), Size = 150 },
            };

            _primaryKey = new string[] { Col.Cards.CardId };
        }
    }

    public class PetCardsTable : BaseTable
    {
        public PetCardsTable()
        {
            _tableName = Tables.Shop.PetCards;
            _dbName = "MabiShop";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.Cards.Id, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.Cards.CardId, IsNullable = false, Type = typeof(long), Options = 1, Size = 1 },
                //new ColumnInfo() { Name = Col.Cards.TypeId, IsNullable = false, Type = typeof(char), Size = 36 },
                new ColumnInfo() { Name = Col.Cards.Type, IsNullable = false, Type = typeof(char), Size = 16 },
                new ColumnInfo() { Name = Col.Cards.Status, IsNullable = false, Type = typeof(int), DefaultValue = "0" },

                new ColumnInfo() { Name = Col.Cards.EntityId, IsNullable = true, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Cards.EntityName, IsNullable = true, Type = typeof(char), Size = 64 },
                new ColumnInfo() { Name = Col.Cards.RebirthCount, IsNullable = true, Type = typeof(int) },
                new ColumnInfo() { Name = Col.Cards.Server, IsNullable = true, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.Cards.Reserved, IsNullable = true, Type = typeof(char), Size = 128 },

                new ColumnInfo() { Name = Col.Cards.Created, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Cards.Used, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Cards.Ended, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.Cards.ValidServer, IsNullable = true, Type = typeof(char), Size = 150 },
            };

            _primaryKey = new string[] { Col.Cards.CardId };
        }
    }

    public class CouponsTable : BaseTable
    {
        public CouponsTable()
        {
            _tableName = Tables.Shop.Coupons;
            _dbName = "MabiShop";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.Coupons.Id, IsNullable = false, Type = typeof(int), Options = 1, Size = 1 },
                new ColumnInfo() { Name = Col.Coupons.Code, IsNullable = false, Type = typeof(char), Size = 18 },
                new ColumnInfo() { Name = Col.Coupons.Type, IsNullable = true, Type = typeof(int), },
                new ColumnInfo() { Name = Col.Coupons.State, IsNullable = true, Type = typeof(short), },
                new ColumnInfo() { Name = Col.Coupons.Account, IsNullable = true, Type = typeof(char), Size = 32 },

                new ColumnInfo() { Name = Col.Coupons.EntityId, IsNullable = true, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Coupons.EntityName, IsNullable = true, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.Coupons.Server, IsNullable = true, Type = typeof(char), Size = 12 },
                new ColumnInfo() { Name = Col.Coupons.StartTime, IsNullable = true, Type = typeof(DateTime), },
                new ColumnInfo() { Name = Col.Coupons.EndTime, IsNullable = true, Type = typeof(DateTime), },

                new ColumnInfo() { Name = Col.Coupons.MainData, IsNullable = true, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.Coupons.SubData, IsNullable = true, Type = typeof(char), Size = 64 },
                new ColumnInfo() { Name = Col.Coupons.RollbackCount, IsNullable = true, Type = typeof(short) },
                new ColumnInfo() { Name = Col.Coupons.StartValidityTerm, IsNullable = true, Type = typeof(DateTime), },
                new ColumnInfo() { Name = Col.Coupons.EndValidityTerm, IsNullable = true, Type = typeof(DateTime), },

                new ColumnInfo() { Name = Col.Coupons.ValidServer, IsNullable = true, Type = typeof(char), Size = 150 },
            };

            _primaryKey = new string[] { Col.Coupons.Code };
            _keys = new List<string[]>()
            {
                new string[] { Col.Coupons.Account },
                new string[] { Col.Coupons.EndTime },
                new string[] { Col.Coupons.Id },
                new string[] { Col.Coupons.SubData },
            };
        }
    }

    public class GiftsTable : BaseTable
    {
        public GiftsTable()
        {
            _tableName = Tables.Shop.Gifts;
            _dbName = "MabiShop";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.Gifts.CardId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.Gifts.CardType, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.Gifts.TypeId, IsNullable = false, Type = typeof(char), Size=36, },
                new ColumnInfo() { Name = Col.Gifts.Type, IsNullable = false, Type = typeof(char), Size=16, },
                new ColumnInfo() { Name = Col.Gifts.Status, IsNullable = false, Type = typeof(int), },

                new ColumnInfo() { Name = Col.Gifts.SenderId, IsNullable = false, Type = typeof(char), Size=32 },
                new ColumnInfo() { Name = Col.Gifts.SenderCharId, IsNullable = true, Type = typeof(long), },
                new ColumnInfo() { Name = Col.Gifts.SenderCharName, IsNullable = true, Type = typeof(char), Size = 64},
                new ColumnInfo() { Name = Col.Gifts.SenderServer, IsNullable = true, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.Gifts.ReceiverId, IsNullable = false, Type = typeof(char), Size = 32 },

                new ColumnInfo() { Name = Col.Gifts.ReceiverCharId, IsNullable = true, Type = typeof(long), },
                new ColumnInfo() { Name = Col.Gifts.ReceiverCharName, IsNullable = true, Type = typeof(char), Size = 64 },
                new ColumnInfo() { Name = Col.Gifts.ReceiverServer, IsNullable = true, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.Gifts.SenderMessage, IsNullable = true, Type = typeof(char), Size = 100 },
                new ColumnInfo() { Name = Col.Gifts.SendDate, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },

                new ColumnInfo() { Name = Col.Gifts.RejectDate, IsNullable = true, Type = typeof(DateTime) },
            };

            _primaryKey = new string[] { Col.Gifts.CardId, Col.Gifts.CardType };
            _keys = new List<string[]>()
            {
                new string[] { Col.Gifts.ReceiverId },
                new string[] { Col.Gifts.SenderId },
            };
        }
    }

    public class GiftHistoryTable : BaseTable
    {
        public GiftHistoryTable()
        {
            _tableName = Tables.Shop.GiftHistory;
            _dbName = "MabiShop";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.GiftHistory.GiftId, IsNullable = false, Type = typeof(long), Options = 1, Size = 1 },
                new ColumnInfo() { Name = Col.GiftHistory.CardId, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.GiftHistory.CardType, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.GiftHistory.TypeId, IsNullable = false, Type = typeof(char), Size=36, },
                new ColumnInfo() { Name = Col.GiftHistory.Type, IsNullable = false, Type = typeof(char), Size=16, },

                new ColumnInfo() { Name = Col.GiftHistory.Status, IsNullable = false, Type = typeof(int), },
                new ColumnInfo() { Name = Col.GiftHistory.SenderId, IsNullable = false, Type = typeof(char), Size=32 },
                new ColumnInfo() { Name = Col.GiftHistory.SenderCharId, IsNullable = true, Type = typeof(long), },
                new ColumnInfo() { Name = Col.GiftHistory.SenderCharName, IsNullable = true, Type = typeof(char), Size = 64},
                new ColumnInfo() { Name = Col.GiftHistory.SenderServer, IsNullable = true, Type = typeof(char), Size = 32 },

                new ColumnInfo() { Name = Col.GiftHistory.ReceiverId, IsNullable = false, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.GiftHistory.ReceiverCharId, IsNullable = true, Type = typeof(long), },
                new ColumnInfo() { Name = Col.GiftHistory.ReceiverCharName, IsNullable = true, Type = typeof(char), Size = 64 },
                new ColumnInfo() { Name = Col.GiftHistory.ReceiverServer, IsNullable = true, Type = typeof(char), Size = 32 },
                new ColumnInfo() { Name = Col.GiftHistory.SenderMessage, IsNullable = true, Type = typeof(char), Size = 100 },

                new ColumnInfo() { Name = Col.GiftHistory.SendDate, IsNullable = true, Type = typeof(DateTime)},
                new ColumnInfo() { Name = Col.GiftHistory.RejectDate, IsNullable = true, Type = typeof(DateTime) },
                new ColumnInfo() { Name = Col.GiftHistory.RegDate, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
            };

            _primaryKey = new string[] { Col.GiftHistory.GiftId };
        }
    }

    public class FreeServiceAccountTable : BaseTable
    {
        public FreeServiceAccountTable()
        {
            _tableName = Tables.Shop.FreeServiceAccount;
            _dbName = "MabiShop";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.FreeServiceAccount.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.FreeServiceAccount.UpdateTime, IsNullable = false, Type = typeof(DateTime), DefaultValue = "CURRENT_TIMESTAMP" },
            };

            _primaryKey = new string[] { Col.FreeServiceAccount.Account };
        }
    }
    #endregion MabiShop

    #region ShopAdvertise
    public class ShopAdvertiseTable : BaseTable
    {
        public ShopAdvertiseTable()
        {
            _tableName = Mabinogi.SQL.Tables.ShopAdvertise.Advertise;
            _dbName = "MabiShopAdvertise";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.ShopAdvertise.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.ShopAdvertise.Server, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.ShopAdvertise.ShopName, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.ShopAdvertise.Area, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.ShopAdvertise.CharacterName, IsNullable = false, Type = typeof(char), Size = 32 },

                new ColumnInfo() { Name = Col.ShopAdvertise.Comment, IsNullable = false, Type = typeof(char), Size = 256 },
                new ColumnInfo() { Name = Col.ShopAdvertise.StartTime, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.ShopAdvertise.Region, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ShopAdvertise.X, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ShopAdvertise.Y, IsNullable = false, Type = typeof(int) },

                new ColumnInfo() { Name = Col.ShopAdvertise.LeafletCount, IsNullable = false, Type = typeof(int), DefaultValue = "0" },
            };
            _primaryKey = new string[] { Col.ShopAdvertise.Server, Col.ShopAdvertise.Account };
        }
    }

    public class ShopAdvertiseItemTable : BaseTable
    {
        public ShopAdvertiseItemTable()
        {
            _tableName = Mabinogi.SQL.Tables.ShopAdvertise.Item;
            _dbName = "MabiShopAdvertise";
            _columns = new List<ColumnInfo>()
            {
                new ColumnInfo() { Name = Col.ShopAdvertiseItem.Account, IsNullable = false, Type = typeof(char), Size = 50 },
                new ColumnInfo() { Name = Col.ShopAdvertiseItem.Server, IsNullable = false, Type = typeof(char), Size = 128 },
                new ColumnInfo() { Name = Col.ShopAdvertiseItem.ItemID, IsNullable = false, Type = typeof(long) },
                new ColumnInfo() { Name = Col.ShopAdvertiseItem.StoredType, IsNullable = false, Type = typeof(byte) },
                new ColumnInfo() { Name = Col.ShopAdvertiseItem.ItemName, IsNullable = false, Type = typeof(char), Size = 128 },

                new ColumnInfo() { Name = Col.ShopAdvertiseItem.Price, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ShopAdvertiseItem.Class, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ShopAdvertiseItem.Color1, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ShopAdvertiseItem.Color2, IsNullable = false, Type = typeof(int) },
                new ColumnInfo() { Name = Col.ShopAdvertiseItem.Color3, IsNullable = false, Type = typeof(int) },
            };
            _primaryKey = new string[] { Col.ShopAdvertiseItem.ItemID, Col.ShopAdvertiseItem.Server };

            _foreignKeys = new ForeignKey[]
            {
                new ForeignKey(Mabinogi.SQL.Tables.ShopAdvertise.Advertise, new string[] { Col.ShopAdvertise.Server, Col.ShopAdvertise.Account }, new string[] { Col.ShopAdvertiseItem.Server, Col.ShopAdvertiseItem.Account }, true),
            };
        }
    }
    #endregion ShopAdvertise
}