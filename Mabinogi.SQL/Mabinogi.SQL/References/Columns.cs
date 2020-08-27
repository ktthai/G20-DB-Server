namespace Mabinogi.SQL.Columns
{

    #region Reference
    public static class Reference
    {
        public const string TotalCount = "totalCount";
        public const string Count = "count";
        public const string StartDate = "startDate";
        public const string EndDate = "endDate";
    }
    #endregion Reference

    #region MabinogiDB
    public static class AcAuth
    {
        public const string Id = "Id";
        public const string Authority = "Authority";
        public const string AuthDesc = "AuthDesc";
    }

    public static class Account
    {
        public const string Id = "Id";
        public const string Password = "Password";
        public const string SecondPassword = "SecondPassword";
        public const string SecondPassMissCount = "SecondPassMissCount";
        public const string Name = "Name";
        public const string Email = "Email";
        public const string Flag = "Flag";
        public const string BlockingDate = "BlockingDate";
        public const string BlockingDuration = "BlockingDuration";
        public const string Authority = "Authority";
        public const string ProviderCode = "ProviderCode";
        public const string MachineIDs = "MachineIds";
    }

    public static class AccountCache
    {
        public const string Account = "Account";
        public const string Key = "Key";
    }

    public static class AccountCharacterLinkedAP
    {
        public const string ServerId = "ServerId";
        public const string CharId = "CharId";
        public const string SavedAp = "SavedAp";
        public const string TermAp = "TermAp";
        public const string ResetTime = "ResetTime";
    }

    public static class AccountCharacterRef
    {
        public const string Id = "Id";
        public const string CharacterId = "CharacterId";
        public const string CharacterName = "CharacterName";
        public const string Server = "Server";
        public const string Deleted = "Deleted";
        public const string GroupId = "GroupId";
        public const string Race = "Race";
        public const string SupportCharacter = "SupportCharacter";
        public const string Tab = "Tab";
    }

    public static class AccountLog
    {
        public const string Account = "Account";
        public const string Address = "Address";
        public const string IntISPCode = "IntISPCode";
        public const string RegDate = "RegDate";
    }

    public static class AccountMeta
    {
        public const string Id = "Id";
        public const string MCode = "MCode";
        public const string MType = "MType";
        public const string MData = "MData";
    }

    public static class AccountPawnCoin
    {
        public const string IdAccount = "IdAccount";
        public const string PawnCoin = "PawnCoin";
        public const string UpdateDate = "UpdateDate";
    }

    public static class AccountPetRef
    {
        public const string Id = "Id";
        public const string PetId = "PetId";
        public const string PetName = "PetName";
        public const string Server = "Server";
        public const string Deleted = "Deleted";
        public const string RemainTime = "RemainTime";
        public const string LastTime = "LastTime";
        public const string GroupId = "GroupId";
        public const string Tab = "Tab";
        public const string ExpireTime = "ExpireTime";
    }

    public static class AccountSession
    {
        public const string Account = "Account";
        public const string Session = "Session";
    }

    public static class AccountLoginHistory
    {
        public const string Id = "Id";
        public const string LoginCount = "LoginCount";
        public const string LoginTime = "LoginTime";
        public const string LastLoginTime = "LastLoginTime";
    }

    public static class AccountRef
    {
        public const string Id = "Id";
        public const string Flag = "Flag";
        public const string MaxSlot = "MaxSlot";
        public const string In = "In";
        public const string Out = "Out";
        public const string PlayableTime = "PlayableTime";
        public const string SupportRace = "SupportRace";
        public const string SupportRewardState = "SupportRewardState";
        public const string LobbyOption = "LobbyOption";
        public const string SupportLastChangeTime = "SupportLastChangeTime";
        public const string MacroCheckFailure = "MacroCheckFailure";
        public const string BeginnerFlag = "BeginnerFlag";
        public const string MacroCheckSuccess = "MacroCheckSuccess";
        public const string Ip = "Ip";
        public const string MachineId = "MachineId";
    }

    public static class AssetRank
    {
        public const string Num = "Num";
        public const string RegDate = "RegDate";
        public const string Id = "Id";
        public const string ChWealth = "ChWealth";
        public const string BankDeposit = "BankDeposit";
        public const string BankWealth = "BankWealth";
        public const string TotalAsset = "TotalAsset";
        public const string Rank = "Rank";
    }

    public static class Bank
    {
        public const string Account = "Account";
        public const string Deposit = "Deposit";
        public const string UpdateTime = "UpdateTime";
        public const string Password = "Password";
        public const string HumanWealth = "HumanWealth";
        public const string ElfWealth = "ElfWealth";
        public const string GiantWealth = "GiantWealth";
        public const string CouponCode = "CouponCode";
    }

    public static class BankSlot
    {
        public const string Account = "Account";
        public const string Name = "Name";
        public const string Valid = "Valid";
        public const string Race = "Race";
        public const string CouponCode = "CouponCode";
        public const string UpdateTime = "UpdateTime";
        public const string ReceiveTime = "ReceiveTime";
    }

    public static class BankItem
    {
        public const string Account = "Account";
        public const string SlotName = "SlotName";
        public const string Location = "Location";
        public const string ExtraTime = "ExtraTime";
        public const string Time = "Time";
        public const string Race = "Race";
    }

    public static class BankItemHuge
    {
        public static string Account => BankItem.Account;
        public static string SlotName => BankItem.SlotName;
        public static string Location => BankItem.Location;
        public static string ExtraTime => BankItem.ExtraTime;
        public static string Time => BankItem.Time;
        public static string ItemId => Item.ItemId;
        public static string ItemLoc => Item.ItemLoc;
        public static string Class => Item.Class;
        public static string PosX => Item.PosX;
        public static string PosY => Item.PosY;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string DurabilityMax => Item.DurabilityMax;
        public static string OriginalDurabilityMax => Item.OriginalDurabilityMax;
        public static string Data => Item.Data;
        public static string SellingPrice => Item.SellingPrice;
        public static string Expiration => Item.Expiration;
        public static string UpdateTime => Item.UpdateTime;
        public static string Race => BankItem.Race;
    }

    public static class BankItemLarge
    {
        public static string Account => BankItem.Account;
        public static string SlotName => BankItem.SlotName;
        public static string Location => BankItem.Location;
        public static string ExtraTime => BankItem.ExtraTime;
        public static string Time => BankItem.Time;
        public static string ItemId => Item.ItemId;
        public static string ItemLoc => Item.ItemLoc;
        public static string Class => Item.Class;
        public static string PosX => Item.PosX;
        public static string PosY => Item.PosY;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string DurabilityMax => Item.DurabilityMax;
        public static string OriginalDurabilityMax => Item.OriginalDurabilityMax;
        public static string AttackMin => Item.AttackMin;
        public static string AttackMax => Item.AttackMax;
        public static string WAttackMin => Item.WAttackMin;
        public static string WAttackMax => Item.WAttackMax;
        public static string Balance => Item.Balance;
        public static string Critical => Item.Critical;
        public static string Defence => Item.Defence;
        public static string Protect => Item.Protect;
        public static string EffectiveRange => Item.EffectiveRange;
        public static string AttackSpeed => Item.AttackSpeed;
        public static string DownHitCount => Item.DownHitCount;
        public static string Experience => Item.Experience;
        public static string ExpPoint => Item.ExpPoint;
        public static string Upgraded => Item.Upgraded;
        public static string UpgradeMax => Item.UpgradeMax;
        public static string Grade => Item.Grade;
        public static string Prefix => Item.Prefix;
        public static string Suffix => Item.Suffix;
        public static string Data => Item.Data;
        public static string Option => Item.Option;
        public static string SellingPrice => Item.SellingPrice;
        public static string Expiration => Item.Expiration;
        public static string UpdateTime => Item.UpdateTime;
        public static string Race => BankItem.Race;
    }

    public static class BankItemQuest
    {
        public static string Account => BankItem.Account;
        public static string SlotName => BankItem.SlotName;
        public static string Location => BankItem.Location;
        public static string ExtraTime => BankItem.ExtraTime;
        public static string Time => BankItem.Time;
        public static string ItemId => Item.ItemId;
        public static string ItemLoc => Item.ItemLoc;
        public static string Quest => Item.Quest;
        public static string Class => Item.Class;
        public static string PosX => Item.PosX;
        public static string PosY => Item.PosY;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string TemplateId => Item.TemplateId;
        public static string Complete => Item.Complete;
        public static string StartTime => Item.StartTime;
        public static string Data => Item.Data;
        public static string Objective => Item.Objective;
        public static string SellingPrice => Item.SellingPrice;
        public static string Expiration => Item.Expiration;
        public static string UpdateTime => Item.UpdateTime;
        public static string Race => BankItem.Race;
    }

    public static class BankItemSmall
    {
        public static string Account => BankItem.Account;
        public static string SlotName => BankItem.SlotName;
        public static string Location => BankItem.Location;
        public static string ExtraTime => BankItem.ExtraTime;
        public static string Time => BankItem.Time;
        public static string ItemId => Item.ItemId;
        public static string ItemLoc => Item.ItemLoc;
        public static string Class => Item.Class;
        public static string PosX => Item.PosX;
        public static string PosY => Item.PosY;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string SellingPrice => Item.SellingPrice;
        public static string Expiration => Item.Expiration;
        public static string UpdateTime => Item.UpdateTime;
        public static string Race => BankItem.Race;
    }

    public static class Bid
    {
        public const string BidId = "BidId";
        public const string CharId = "CharId";
        public const string CharName = "CharName";
        public const string AuctionItemId = "AuctionItemId";
        public const string Price = "Price";
        public const string Time = "Time";
        public const string BidState = "BidState";
    }

    public static class BidIdPool
    {
        public const string Count = "Count";
    }

    public static class Castle
    {
        public const string CastleId = "CastleId";
        public const string GuildId = "GuildId";
        public const string Constructed = "Constructed";
        public const string CastleMoney = "CastleMoney";
        public const string WeeklyIncome = "WeeklyIncome";
        public const string TaxRate = "TaxRate";
        public const string UpdateTime = "UpdateTime";
        public const string Durability = "Durability";
        public const string MaxDurability = "MaxDurability";
        public const string BuildState = "BuildState";
        public const string BuildNextTime = "BuildNextTime";
        public const string BuildStep = "BuildStep";
        public const string Flag = "Flag";
        public const string SellDungeonPass = "SellDungeonPass";
        public const string DungeonPassPrice = "DungeonPassPrice";
    }

    public static class CastleBlock
    {
        public const string CastleId = "CastleId";
        public const string GameName = "GameName";
        public const string Flag = "Flag";
        public const string Entry = "Entry";
    }

    public static class CastleBuildResource
    {
        public const string CastleId = "CastleId";
        public const string ClassId = "ClassId";
        public const string CurrentAmount = "CurrentAmount";
        public const string MaxAmount = "MaxAmount";
    }

    public static class CastleBid
    {
        public const string CastleId = "CastleId";
        public const string BidStartTime = "BidStartTime";
        public const string BidEndTime = "BidEndTime";
        public const string MinBidPrice = "MinBidPrice";
    }

    public static class CastleBidder
    {
        public const string CastleId = "CastleId";
        public const string BidGuildId = "BidGuildId";
        public const string BidGuildName = "BidGuildName";
        public const string BidPrice = "BidPrice";
        public const string BidOrder = "BidOrder";
        public const string BidCharacterID = "BidCharacterId";
        public const string BidCharName = "BidCharName";
        public const string BidTime = "BidTime";
        public const string BidUpdateTime = "BidUpdateTime";
    }

    public static class CastleBidderHistory
    {
        public const string CastleId = "CastleId";
        public const string BidGuildId = "BidGuildId";
        public const string BidGuildName = "BidGuildName";
        public const string BidPrice = "BidPrice";
        public const string BidOrder = "BidOrder";
        public const string BidCharacterID = "BidCharacterId";
        public const string BidCharName = "BidCharName";
        public const string BidTime = "BidTime";
        public const string BidUpdateTime = "BidUpdateTime";
        public const string RefundTime = "RefundTime";
        public const string Flag = "Flag";
    }

    public static class ChannelingKeyPool
    {
        public const string ProviderCode = "ProviderCode";
        public const string InsertDate = "InsertDate";
        public const string KeyString = "KeyString";
    }

    public static class CharDeletedRefSync
    {
        public const string Id = "Id";
        public const string CharacterId = "CharacterId";
        public const string CharacterName = "CharacterName";
        public const string Server = "Server";
        public const string DeletedTime = "DeletedTime";
    }

    public static class CharRefSync
    {
        public const string Id = "Id";
        public const string CharacterId = "CharacterId";
        public const string CharacterName = "CharacterName";
        public const string Server = "Server";
    }


    public static class Character
    {
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Type = "Type";
        public const string SkinColor = "SkinColor";
        public const string EyeType = "EyeType";
        public const string EyeColor = "EyeColor";
        public const string MouthType = "MouthType";
        public const string Status = "Status";
        public const string Height = "Height";
        public const string Fatness = "Fatness";
        public const string Upper = "Upper";
        public const string Lower = "Lower";
        public const string Region = "Region";
        public const string X = "X";
        public const string Y = "Y";
        public const string Direction = "Direction";
        public const string BattleState = "BattleState";
        public const string WeaponSet = "WeaponSet";
        public const string Life = "Life";
        public const string LifeDamage = "LifeDamage";
        public const string LifeMax = "LifeMax";
        public const string Mana = "Mana";
        public const string ManaMax = "ManaMax";
        public const string Stamina = "Stamina";
        public const string StaminaMax = "StaminaMax";
        public const string Food = "Food";
        public const string Level = "Level";
        public const string CumulatedLevel = "CumulatedLevel";
        public const string Experience = "Experience";
        public const string Age = "Age";
        public const string Strength = "Strength";
        public const string Dexterity = "Dexterity";
        public const string Intelligence = "Intelligence";
        public const string Will = "Will";
        public const string Luck = "Luck";
        public const string AbilityRemain = "AbilityRemain";
        public const string AttackMin = "AttackMin";
        public const string AttackMax = "AttackMax";
        public const string WAttackMin = "WAttackMin";
        public const string WAttackMax = "WAttackMax";
        public const string Critical = "Critical";
        public const string Protect = "Protect";
        public const string Defense = "Defense";
        public const string Rate = "Rate";
        public const string StrengthBoost = "StrengthBoost";
        public const string DexterityBoost = "DexterityBoost";
        public const string IntelligenceBoost = "IntelligenceBoost";
        public const string WillBoost = "WillBoost";
        public const string LuckBoost = "LuckBoost";
        public const string HeightBoost = "HeightBoost";
        public const string FatnessBoost = "FatnessBoost";
        public const string UpperBoost = "UpperBoost";
        public const string LowerBoost = "LowerBoost";
        public const string LifeBoost = "LifeBoost";
        public const string ManaBoost = "ManaBoost";
        public const string StaminaBoost = "StaminaBoost";
        public const string Toxic = "Toxic";
        public const string ToxicDrunkenTime = "ToxicDrunkenTime";
        public const string ToxicStrength = "ToxicStrength";
        public const string ToxicIntelligence = "ToxicIntelligence";
        public const string ToxicDexterity = "ToxicDexterity";
        public const string ToxicWill = "ToxicWill";
        public const string ToxicLuck = "ToxicLuck";
        public const string LastTown = "LastTown";
        public const string LastDungeon = "LastDungeon";
        public const string NaoMemory = "NaoMemory";
        public const string NaoFavor = "NaoFavor";
        public const string Birthday = "Birthday";
        public const string Playtime = "Playtime";
        public const string Wealth = "Wealth";
        public const string Condition = "Condition";
        public const string Collection = "Collection";
        public const string History = "History";
        public const string Memory = "Memory";
        public const string Title = "Title";
        public const string Reserved = "Reserved";
        public const string Book = "Book";
        public const string UpdateTime = "UpdateTime";
        public const string DeleteTime = "DeleteTime";
        public const string MaxLevel = "MaxLevel";
        public const string RebirthCount = "RebirthCount";
        public const string LifetimeSkill = "LifetimeSkill";
        public const string NsRespawnCount = "NsRespawnCount";
        public const string NsLastRespawnDay = "NsLastRespawnDay";
        public const string NsGiftReceiveDay = "NsGiftReceiveDay";
        public const string ApGiftReceiveDay = "ApGiftReceiveDay";
        public const string Rank1 = "Rank1";
        public const string Rank2 = "Rank2";
        public const string RebirthDay = "RebirthDay";
        public const string RebirthAge = "RebirthAge";
        public const string NaoStyle = "NaoStyle";
        public const string Score = "Score";
        public const string MateId = "MateId";
        public const string MateName = "MateName";
        public const string MarriageTime = "MarriageTime";
        public const string MarriageCount = "MarriageCount";
        public const string WriteCounter = "WriteCounter";
        public const string ExploLevel = "ExploLevel";
        public const string ExploCumLevel = "ExploCumLevel";
        public const string ExploExp = "ExploExp";
        public const string DiscoverCount = "DiscoverCount";
        public const string NsBombCount = "NsBombCount";
        public const string NsBombDay = "NsBombDay";
        public const string LifeMaxByFood = "LifeMaxByFood";
        public const string ManaMaxByFood = "ManaMaxByFood";
        public const string StaminaMaxByFood = "StaminaMaxByFood";
        public const string StrengthByFood = "StrengthByFood";
        public const string DexterityByFood = "DexterityByFood";
        public const string IntelligenceByFood = "IntelligenceByFood";
        public const string WillByFood = "WillByFood";
        public const string LuckByFood = "LuckByFood";
        public const string FarmId = "FarmId";
        public const string HeartUpdateTime = "HeartUpdateTime";
        public const string HeartPoint = "HeartPoint";
        public const string HeartTotalPoint = "HeartTotalPoint";
        public const string JoustPoint = "JoustPoint";
        public const string JoustLastWinYear = "JoustLastWinYear";
        public const string JoustLastWinWeek = "JoustLastWinWeek";
        public const string JoustWeekWinCount = "JoustWeekWinCount";
        public const string JoustDailyWinCount = "JoustDailyWinCount";
        public const string JoustDailyLoseCount = "JoustDailyLoseCount";
        public const string JoustServerWinCount = "JoustServerWinCount";
        public const string JoustServerLoseCount = "JoustServerLoseCount";
        public const string ExploMaxKeyLevel = "ExploMaxKeyLevel";
        public const string DonationValue = "DonationValue";
        public const string DonationUpdate = "DonationUpdate";
        public const string MacroPoint = "MacroPoint";
        public const string JobId = "JobId";
        public const string CouponCode = "CouponCode";
    }

    public static class CharacterDeed
    {
        public const string Id = "Id";
        public static readonly string[] DayCount = { "DayCount1", "DayCount2", "DayCount3", "DayCount4", "DayCount5", "DayCount6", "DayCount7", "DayCount8", "DayCount9", "DayCount10" };
        public static readonly string[] Flag = { "Flag1", "Flag2", "Flag3", "Flag4", "Flag5", "Flag6", "Flag7", "Flag8", "Flag9", "Flag10" };
    }

    public static class CharacterDivineKnight
    {
        public const string Id = "Id";
        public const string Experience = "Experience";
        public const string GroupLimit = "GroupLimit";
        public const string GroupSelected = "GroupSelected";
        public const string UpdateDate = "UpdateDate";
    }

    public static class CharacterKeyword
    {
        public const string CharId = "CharId";
        public const string KeywordId = "KeywordId";
    }

    public static class CharacterMeta
    {
        public const string CharId = "CharId";
        public const string MCode = "MCode";
        public const string MType = "MType";
        public const string MData = "MData";
    }

    public static class CharacterMyKnights
    {
        public const string CharId = "CharId";
        public const string Name = "Name";
        public const string Level = "Level";
        public const string Experience = "Experience";
        public const string Point = "Point";
        public const string CreateDate = "CreateDate";
        public const string DateBuffMember = "DateBuffMember";
        public const string AddedSlotCount = "AddedSlotCount";
        public const string UpdateTime = "UpdateTime";
    }

    public static class CharacterMyKnightsMember
    {
        public const string CharId = "CharId";
        public const string KnightId = "KnightId";
        public const string IsMine = "IsMine";
        public const string Holy = "Holy";
        public const string Strength = "Strength";
        public const string Intelligence = "Intelligence";
        public const string Dexterity = "Dexterity";
        public const string Will = "Will";
        public const string Luck = "Luck";
        public const string FavorLvl = "FavorLvl";
        public const string Favor = "Favor";
        public const string Stress = "Stress";
        public const string WoundTime = "WoundTime";
        public const string IsSelfCured = "IsSelfCured";
        public const string CurrentTraining = "CurrentTraining";
        public const string StartTrainingTime = "StartTrainingTime";
        public const string CurrentCommand = "CurrentCommand";
        public const string CurrentTask = "CurrentTask";
        public const string RestStartTime = "RestStartTime";
        public const string DateList = "DateList";
        public const string LastDateTime = "LastDateTime";
        public const string FirstScoutTime = "FirstScoutTime";
        public const string LastScoutTime = "LastScoutTime";
        public const string ReleaseCount = "ReleaseCount";
        public const string CommandTryCount = "CommandTryCount";
        public const string CommandSuccessCount = "CommandSuccessCount";
        public const string FavorTalkCount = "FavorTalkCount";
        public const string CurrentCommandTemplate = "CurrentCommandTemplate";
        public const string CommandEndTime = "CommandEndTime";
        public const string LastReleaseTime = "LastReleaseTime";
    }

    public static class CharacterPvP
    {
        public const string Id = "Id";
        public const string WinCount = "WinCount";
        public const string LoseCount = "LoseCount";
        public const string PenaltyPoint = "PenaltyPoint";
    }

    public static class CharacterShape
    {
        public const string Id = "Id";
        public const string ShapeId = "ShapeId";
        public const string Count = "Count";
        public const string Flag = "Flag";
        public const string UpdateTime = "UpdateTime";
    }

    public static class CharacterSkill
    {
        public const string Id = "Id";
        public const string Skill = "Skill";
        public const string Version = "Version";
        public const string Level = "Level";
        public const string MaxLevel = "MaxLevel";
        public const string Experience = "Experience";
        public const string Count = "Count";
        public const string Flag = "Flag";
        public const string SubFlag1 = "SubFlag1";
        public const string SubFlag2 = "SubFlag2";
        public const string SubFlag3 = "SubFlag3";
        public const string SubFlag4 = "SubFlag4";
        public const string SubFlag5 = "SubFlag5";
        public const string SubFlag6 = "SubFlag6";
        public const string SubFlag7 = "SubFlag7";
        public const string SubFlag8 = "SubFlag8";
        public const string SubFlag9 = "SubFlag9";
        public const string LastPromotionTime = "LastPromotionTime";
        public const string PromotionConditionCount = "PromotionConditionCount";
        public const string PromotionExperience = "PromotionExperience";
    }

    public static class CharacterSubskill
    {
        public const string Id = "Id";
        public const string Subskill = "Subskill";
        public const string Level = "Level";
        public const string Experience = "Experience";
    }

    public static class CharacterAchievement
    {
        public const string Id = "Id";
        public const string TotalScore = "TotalScore";
        public const string Achievement = "Achievement";
    }

    public static class CharacterQuest
    {
        public const string Id = "Id";
        public const string QuestId = "QuestId";
        public const string Start = "Start";
        public const string End = "End";
        public const string Extra = "Extra";
    }

    public static class CharIdPool
    {
        public const string Count = "Count";
    }


    public static class CharItem
    {
        public const string Id = "Id";
        public const string VarInt = "VarInt";
    }

    public static class CharItemEgo
    {
        public static string Id => CharItem.Id;
        public static string ItemId => Item.ItemId;
        public static string ItemLoc => Item.ItemLoc;
        public static string PocketId => Item.PocketId;
        public static string PosX => Item.PosX;
        public static string PosY => Item.PosY;
        public static string VarInt => CharItem.VarInt;
        public static string Class => Item.Class;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Figure => Item.Figure;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string DurabilityMax => Item.DurabilityMax;
        public static string OriginalDurabilityMax => Item.OriginalDurabilityMax;
        public static string AttackMin => Item.AttackMin;
        public static string AttackMax => Item.AttackMax;
        public static string WAttackMin => Item.WAttackMin;
        public static string WAttackMax => Item.WAttackMax;
        public static string Balance => Item.Balance;
        public static string Critical => Item.Critical;
        public static string Defence => Item.Defence;
        public static string Protect => Item.Protect;
        public static string EffectiveRange => Item.EffectiveRange;
        public static string AttackSpeed => Item.AttackSpeed;
        public static string DownHitCount => Item.DownHitCount;
        public static string Experience => Item.Experience;
        public static string ExpPoint => Item.ExpPoint;
        public static string Upgraded => Item.Upgraded;
        public static string UpgradeMax => Item.UpgradeMax;
        public static string Grade => Item.Grade;
        public static string Prefix => Item.Prefix;
        public static string Suffix => Item.Suffix;
        public static string Data => Item.Data;
        public static string Option => Item.Option;
        public static string SellingPrice => Item.SellingPrice;
        public static string Expiration => Item.Expiration;
        public static string UpdateTime => Item.UpdateTime;
        public static string EgoName => Item.EgoName;
        public static string EgoType => Item.EgoType;
        public static string EgoDesire => Item.EgoDesire;
        public static string EgoSocialLevel => Item.EgoSocialLevel;
        public static string EgoSocialExp => Item.EgoSocialExp;
        public static string EgoStrengthLevel => Item.EgoStrengthLevel;
        public static string EgoStrengthExp => Item.EgoStrengthExp;
        public static string EgoIntelligenceLevel => Item.EgoIntelligenceLevel;
        public static string EgoIntelligenceExp => Item.EgoIntelligenceExp;
        public static string EgoDexterityLevel => Item.EgoDexterityLevel;
        public static string EgoDexterityExp => Item.EgoDexterityExp;
        public static string EgoWillLevel => Item.EgoWillLevel;
        public static string EgoWillExp => Item.EgoWillExp;
        public static string EgoLuckLevel => Item.EgoLuckLevel;
        public static string EgoLuckExp => Item.EgoLuckExp;
        public static string EgoSkillCount => Item.EgoSkillCount;
        public static string EgoSkillGauge => Item.EgoSkillGauge;
        public static string EgoSkillCooldown => Item.EgoSkillCooldown;
    }

    public static class CharItemHuge
    {
        public static string Id => CharItem.Id;
        public static string ItemId => Item.ItemId;
        public static string ItemLoc => Item.ItemLoc;
        public static string PocketId => Item.PocketId;
        public static string Class => Item.Class;
        public static string PosX => Item.PosX;
        public static string PosY => Item.PosY;
        public static string VarInt => CharItem.VarInt;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string DurabilityMax => Item.DurabilityMax;
        public static string OriginalDurabilityMax => Item.OriginalDurabilityMax;
        public static string Data => Item.Data;
        public static string SellingPrice => Item.SellingPrice;
        public static string Expiration => Item.Expiration;
        public static string UpdateTime => Item.UpdateTime;
    }

    public static class CharItemLarge
    {
        public static string Id => CharItem.Id;
        public static string ItemId => Item.ItemId;
        public static string ItemLoc => Item.ItemLoc;
        public static string PocketId => Item.PocketId;
        public static string Class => Item.Class;
        public static string PosX => Item.PosX;
        public static string PosY => Item.PosY;
        public static string VarInt => CharItem.VarInt;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string DurabilityMax => Item.DurabilityMax;
        public static string OriginalDurabilityMax => Item.OriginalDurabilityMax;
        public static string AttackMin => Item.AttackMin;
        public static string AttackMax => Item.AttackMax;
        public static string WAttackMin => Item.WAttackMin;
        public static string WAttackMax => Item.WAttackMax;
        public static string Balance => Item.Balance;
        public static string Critical => Item.Critical;
        public static string Defence => Item.Defence;
        public static string Protect => Item.Protect;
        public static string EffectiveRange => Item.EffectiveRange;
        public static string AttackSpeed => Item.AttackSpeed;
        public static string DownHitCount => Item.DownHitCount;
        public static string Experience => Item.Experience;
        public static string ExpPoint => Item.ExpPoint;
        public static string Upgraded => Item.Upgraded;
        public static string UpgradeMax => Item.UpgradeMax;
        public static string Grade => Item.Grade;
        public static string Prefix => Item.Prefix;
        public static string Suffix => Item.Suffix;
        public static string Data => Item.Data;
        public static string Option => Item.Option;
        public static string SellingPrice => Item.SellingPrice;
        public static string Expiration => Item.Expiration;
        public static string UpdateTime => Item.UpdateTime;
    }

    public static class CharItemQuest
    {
        public static string Id => CharItem.Id;
        public static string ItemId => Item.ItemId;
        public static string ItemLoc => Item.ItemLoc;
        public static string PocketId => Item.PocketId;
        public static string Quest => Item.Quest;
        public static string Class => Item.Class;
        public static string PosX => Item.PosX;
        public static string PosY => Item.PosY;
        public static string VarInt => CharItem.VarInt;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string TemplateId => Item.TemplateId;
        public static string Complete => Item.Complete;
        public static string StartTime => Item.StartTime;
        public static string Data => Item.Data;
        public static string Objective => Item.Objective;
        public static string SellingPrice => Item.SellingPrice;
        public static string Expiration => Item.Expiration;
        public static string UpdateTime => Item.UpdateTime;
    }

    public static class CharItemSmall
    {
        public static string Id => CharItem.Id;
        public static string ItemId => Item.ItemId;
        public static string ItemLoc => Item.ItemLoc;
        public static string PocketId => Item.PocketId;
        public static string Class => Item.Class;
        public static string PosX => Item.PosX;
        public static string PosY => Item.PosY;
        public static string VarInt => CharItem.VarInt;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string SellingPrice => Item.SellingPrice;
        public static string Expiration => Item.Expiration;
        public static string UpdateTime => Item.UpdateTime;
    }

    public static class Item
    {
        public const string ItemId = "ItemId";
        public const string PocketId = "PocketId";
        public const string ItemLoc = "ItemLoc";
        public const string PosX = "PosX";
        public const string PosY = "PosY";
        public const string Class = "Class";
        public const string Color1 = "Color1";
        public const string Color2 = "Color2";
        public const string Color3 = "Color3";
        public const string Price = "Price";
        public const string Bundle = "Bundle";
        public const string LinkedPocket = "LinkedPocket";
        public const string Figure = "Figure";
        public const string Flag = "Flag";
        public const string Durability = "Durability";
        public const string DurabilityMax = "DurabilityMax";
        public const string OriginalDurabilityMax = "OriginalDurabilityMax";
        public const string AttackMin = "AttackMin";
        public const string AttackMax = "AttackMax";
        public const string WAttackMin = "WAttackMin";
        public const string WAttackMax = "WAttackMax";
        public const string Balance = "Balance";
        public const string Critical = "Critical";
        public const string Defence = "Defence";
        public const string Protect = "Protect";
        public const string EffectiveRange = "EffectiveRange";
        public const string AttackSpeed = "AttackSpeed";
        public const string DownHitCount = "DownHitCount";
        public const string Experience = "Experience";
        public const string ExpPoint = "ExpPoint";
        public const string Upgraded = "Upgraded";
        public const string UpgradeMax = "UpgradeMax";
        public const string Grade = "Grade";
        public const string Prefix = "Prefix";
        public const string Suffix = "Suffix";
        public const string Data = "Data";
        public const string Option = "Option";
        public const string SellingPrice = "SellingPrice";
        public const string Expiration = "Expiration";
        public const string UpdateTime = "UpdateTime";

        public const string Quest = "Quest";
        public const string TemplateId = "TemplateId";
        public const string Complete = "Complete";
        public const string StartTime = "StartTime";
        public const string Objective = "Objective";


        public const string EgoName = "EgoName";
        public const string EgoType = "EgoType";
        public const string EgoDesire = "EgoDesire";
        public const string EgoSocialLevel = "EgoSocialLevel";
        public const string EgoSocialExp = "EgoSocialExp";
        public const string EgoStrengthLevel = "EgoStrengthLevel";
        public const string EgoStrengthExp = "EgoStrengthExp";
        public const string EgoIntelligenceLevel = "EgoIntelligenceLevel";
        public const string EgoIntelligenceExp = "EgoIntelligenceExp";
        public const string EgoDexterityLevel = "EgoDexterityLevel";
        public const string EgoDexterityExp = "EgoDexterityExp";
        public const string EgoWillLevel = "EgoWillLevel";
        public const string EgoWillExp = "EgoWillExp";
        public const string EgoLuckLevel = "EgoLuckLevel";
        public const string EgoLuckExp = "EgoLuckExp";
        public const string EgoSkillCount = "EgoSkillCount";
        public const string EgoSkillGauge = "EgoSkillGauge";
        public const string EgoSkillCooldown = "EgoSkillCooldown";

    }

    public static class Commerce
    {
        public const string CharId = "CharId";
        public const string Ducat = "Ducat";
        public const string CurrentTransportId = "CurrentTransportId";
        public const string UnlockTransport = "UnlockTransport";
        public const string LostPercent = "LostPercent";
        public const string Post1Credit = "Post1Credit";
        public const string Post2Credit = "Post2Credit";
        public const string Post3Credit = "Post3Credit";
        public const string Post4Credit = "Post4Credit";
        public const string Post5Credit = "Post5Credit";
        public const string Post6Credit = "Post6Credit";
        public const string Post7Credit = "Post7Credit";
        public const string Post8Credit = "Post8Credit";
        public const string UpdateDate = "UpdateDate";
        public static readonly string[] Post = { Post1Credit, Post2Credit, Post3Credit, Post4Credit, Post5Credit, Post6Credit, Post7Credit, Post8Credit };
    }

    public static class CommerceCriminal
    {
        public const string CriminalId = "CriminalId";
        public const string CharName = "CharName";
        public const string Ducat = "Ducat";
    }
    public static class CommerceCriminalReward
    {
        public const string CriminalId = "CriminalId";
        public const string Reward = "Reward";
    }

    public static class CommercePost
    {
        public const string PostId = "PostId";
        public const string PostInvestment = "PostInvestment";
        public const string PostCommission = "PostCommission";
    }

    public static class CommerceProduct
    {
        public const string ProductId = "ProductId";
        public const string ProductPrice = "ProductPrice";
        public const string ProductCount = "ProductCount";
    }

    public static class CommerceProductStock
    {
        public const string ProductId = "ProductId";
        public const string ProductSellPostId = "ProductSellPostId";
        public const string ProductStock = "ProductStock";
        public const string ProductStockPrice = "ProductStockPrice";
    }

    public static class CommercePurchasedProduct
    {
        public const string CharId = "CharId";
        public const string ClassId = "ClassId";
        public const string Bundle = "Bundle";
        public const string Price = "Price";
    }

    public static class CumulativeLevelRank
    {
        public const string Num = "Num";
        public const string RegDate = "RegDate";
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Level = "Level";
        public const string PlayTime = "PlayTime";
        public const string Experience = "Experience";
        public const string Age = "Age";
        public const string CumulatedLevel = "CumulatedLevel";
        public const string Race = "Race";
    }

    public static class DeleteChar
    {
        public const string Id = "Id";
        public const string DeleteTime = "DeleteTime";
    }
    public static class EquipCollect
    {
        public const string Account = "Account";
        public const string UpdateTime = "UpdateTime";
    }

    public static class EquipCollectItem
    {
        public const string Account = "Account";
        public static string ItemLoc => Item.ItemLoc;
        public const string UpdateTime = "UpdateTime";
        public const string LockTime = "LockTime";
    }

    public static class Family
    {
        public const string Id = "Id";
        public const string Name = "Name";
        public const string HeadMemberId = "HeadMemberId";
        public const string State = "State";
        public const string Tradition = "Tradition";
        public const string Meta = "Meta";
    }

    public static class FamilyMember
    {
        public const string CharId = "CharId";
        public const string FamilyId = "FamilyId";
        public const string Name = "Name";
        public const string Class = "Class";
    }

    public static class Farm
    {
        public const string FarmId = "FarmId";
        public const string OwnerAccount = "OwnerAccount";
        public const string OwnerCharId = "OwnerCharId";
        public const string OwnerCharName = "OwnerCharName";
        public const string ExpireTime = "ExpireTime";
        public const string Crop = "Crop";
        public const string PlantTime = "PlantTime";
        public const string WaterWork = "WaterWork";
        public const string NutrientWork = "NutrientWork";
        public const string InsectWork = "InsectWork";
        public const string Water = "Water";
        public const string Nutrient = "Nutrient";
        public const string Insect = "Insect";
        public const string Growth = "Growth";
        public const string CurrentWork = "CurrentWork";
        public const string WorkCompleteTime = "WorkCompleteTime";
        public const string TodayWorkCount = "TodayWorkCount";
        public const string LastWorkTime = "LastWorkTime";
    }

    public static class FavoritePrivateFarm
    {
        public const string CharId = "CharId";
        public const string PrivateFarmId = "PrivateFarmId";
        public const string WorldPosX = "WorldPosX";
        public const string WorldPosY = "WorldPosY";
        public const string OwnerName = "OwnerName";
        public const string FarmName = "FarmName";
        public const string ThemeId = "ThemeId";
    }

    public static class GameId
    {
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Flag = "Flag";
    }

    public static class GuestBook
    {
        public const string Id = "Id";
        public const string Server = "Server";
        public const string Account = "Account";
        public const string Message = "Message";
        public const string Valid = "Valid";
    }

    public static class GuestBookComment
    {
        public const string SerialNumber = "SerialNumber";
        public const string Id = "Id";
        public const string Server = "Server";
        public const string Account = "Account";
        public const string Name = "Name";
        public const string Thread = "Thread";
        public const string Depth = "Depth";
        public const string WriteDate = "WriteDate";
        public const string Type = "Type";
        public const string Message = "Message";
        public const string ReplyNum = "ReplyNum";
        public const string RealDate = "RealDate";
    }

    public static class HelpPointRanking
    {
        public const string CharId = "CharId";
        public const string Score1 = "Score1";
        public const string Score2 = "Score2";
        public const string UpdateTime = "UpdateTime";
    }

    public static class House
    {
        public const string HouseId = "HouseId";
        public const string Constructed = "Constructed";
        public const string UpdateTime = "UpdateTime";
        public const string CharName = "CharName";
        public const string HouseName = "HouseName";
        public const string HouseClass = "HouseClass";
        public const string RoofSkin = "RoofSkin";
        public const string RoofColor1 = "RoofColor1";
        public const string RoofColor2 = "RoofColor2";
        public const string RoofColor3 = "RoofColor3";
        public const string WallSkin = "WallSkin";
        public const string WallColor1 = "WallColor1";
        public const string WallColor2 = "WallColor2";
        public const string WallColor3 = "WallColor3";
        public const string InnerSkin = "InnerSkin";
        public const string InnerColor1 = "InnerColor1";
        public const string InnerColor2 = "InnerColor2";
        public const string InnerColor3 = "InnerColor3";
        public const string Width = "Width";
        public const string Height = "Height";
        public const string BidSuccessDate = "BidSuccessDate";
        public const string TaxPrevDate = "TaxPrevDate";
        public const string TaxNextDate = "TaxNextDate";
        public const string TaxPrice = "TaxPrice";
        public const string TaxAutopay = "TaxAutopay";
        public const string HouseMoney = "HouseMoney";
        public const string Deposit = "Deposit";
        public const string Flag = "Flag";
    }

    public static class HouseBlock
    {
        public const string HouseId = "HouseId";
        public const string GameName = "GameName";
        public const string Flag = "Flag";
    }

    public static class HouseBid
    {
        public const string HouseId = "HouseId";
        public const string BidStartTime = "BidStartTime";
        public const string BidEndTime = "BidEndTime";
        public const string BidRepayEndTime = "BidRepayEndTime";
        public const string MinBidPrice = "MinBidPrice";
    }

    public static class HouseBidder
    {
        public const string HouseId = "HouseId";
        public const string BidAccount = "BidAccount";
        public const string BidPrice = "BidPrice";
        public const string BidOrder = "BidOrder";
        public const string BidCharacter = "BidCharacter";
        public const string BidCharName = "BidCharName";
        public const string BidTime = "BidTime";
        public const string IsWinner = "IsWinner";
    }

    public static class HouseBidderHistory
    {
        public const string HouseId = "HouseId";
        public const string BidAccount = "BidAccount";
        public const string BidPrice = "BidPrice";
        public const string BidOrder = "BidOrder";
        public const string BidCharacter = "BidCharacter";
        public const string BidCharName = "BidCharName";
        public const string BidTime = "BidTime";
        public const string RefundTime = "RefundTime";
        public const string Flag = "Flag";
    }

    public static class HouseItem
    {
        public const string Account = "Account";
        public const string Direction = "Direction";
        public const string UserPrice = "UserPrice";
        public const string Pocket = "Pocket";
    }

    public static class HouseItemHuge
    {
        public static string Account => HouseItem.Account;
        public static string Direction => HouseItem.Direction;
        public static string UserPrice => HouseItem.UserPrice;
        public static string Pocket => HouseItem.Pocket;
        public static string ItemId => Item.ItemId;
        public static string ItemLoc => Item.ItemLoc;
        public static string Class => Item.Class;
        public static string PosX => Item.PosX;
        public static string PosY => Item.PosY;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string DurabilityMax => Item.DurabilityMax;
        public static string OriginalDurabilityMax => Item.OriginalDurabilityMax;
        public static string Data => Item.Data;
        public static string SellingPrice => Item.SellingPrice;
        public static string Expiration => Item.Expiration;
        public static string UpdateTime => Item.UpdateTime;
    }

    public static class HouseItemLarge
    {
        public static string Account => HouseItem.Account;
        public static string Direction => HouseItem.Direction;
        public static string UserPrice => HouseItem.UserPrice;
        public static string Pocket => HouseItem.Pocket;
        public static string ItemId => Item.ItemId;
        public static string ItemLoc => Item.ItemLoc;
        public static string Class => Item.Class;
        public static string PosX => Item.PosX;
        public static string PosY => Item.PosY;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Figure => Item.Figure;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string DurabilityMax => Item.DurabilityMax;
        public static string OriginalDurabilityMax => Item.OriginalDurabilityMax;
        public static string AttackMin => Item.AttackMin;
        public static string AttackMax => Item.AttackMax;
        public static string WAttackMin => Item.WAttackMin;
        public static string WAttackMax => Item.WAttackMax;
        public static string Balance => Item.Balance;
        public static string Critical => Item.Critical;
        public static string Defence => Item.Defence;
        public static string Protect => Item.Protect;
        public static string EffectiveRange => Item.EffectiveRange;
        public static string AttackSpeed => Item.AttackSpeed;
        public static string DownHitCount => Item.DownHitCount;
        public static string Experience => Item.Experience;
        public static string ExpPoint => Item.ExpPoint;
        public static string Upgraded => Item.Upgraded;
        public static string UpgradeMax => Item.UpgradeMax;
        public static string Grade => Item.Grade;
        public static string Prefix => Item.Prefix;
        public static string Suffix => Item.Suffix;
        public static string Data => Item.Data;
        public static string Option => Item.Option;
        public static string SellingPrice => Item.SellingPrice;
        public static string Expiration => Item.Expiration;
        public static string UpdateTime => Item.UpdateTime;
    }

    public static class HouseItemQuest
    {
        public static string Account => HouseItem.Account;
        public static string Direction => HouseItem.Direction;
        public static string UserPrice => HouseItem.UserPrice;
        public static string Pocket => HouseItem.Pocket;
        public static string ItemId => Item.ItemId;
        public static string ItemLoc => Item.ItemLoc;
        public static string Quest => Item.Quest;
        public static string Class => Item.Class;
        public static string PosX => Item.PosX;
        public static string PosY => Item.PosY;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string TemplateId => Item.TemplateId;
        public static string Complete => Item.Complete;
        public static string StartTime => Item.StartTime;
        public static string Data => Item.Data;
        public static string Objective => Item.Objective;
        public static string SellingPrice => Item.SellingPrice;
        public static string Expiration => Item.Expiration;
        public static string UpdateTime => Item.UpdateTime;
    }

    public static class HouseItemSmall
    {
        public static string Account => HouseItem.Account;
        public static string Direction => HouseItem.Direction;
        public static string UserPrice => HouseItem.UserPrice;
        public static string Pocket => HouseItem.Pocket;
        public static string ItemId => Item.ItemId;
        public static string ItemLoc => Item.ItemLoc;
        public static string Class => Item.Class;
        public static string PosX => Item.PosX;
        public static string PosY => Item.PosY;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string SellingPrice => Item.SellingPrice;
        public static string Expiration => Item.Expiration;
        public static string UpdateTime => Item.UpdateTime;
    }

    public static class HouseOwner
    {
        public const string HouseId = "HouseId";
        public const string Account = "Account";
    }

    public static class InviteEvent
    {
        public const string IdX = "IdX";
        public const string Id = "Id";
        public const string Server = "Server";
        public const string InviteCharacterId = "InviteCharacterId";
        public const string InviteCharacterName = "InviteCharacterName";
        public const string SendDate = "SendDate";
    }

    public static class ItemHistory
    {
        public static string Id => Item.ItemId;
        public static string PocketId => Item.PocketId;
        public static string Class => Item.Class;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Figure => Item.Figure;
        public static string Flag => Item.Flag;
        public static string Durability => Item.Durability;
        public static string DurabilityMax => Item.DurabilityMax;
        public static string OriginalDurabilityMax => Item.OriginalDurabilityMax;
        public static string AttackMin => Item.AttackMin;
        public static string AttackMax => Item.AttackMax;
        public static string WAttackMin => Item.WAttackMin;
        public static string WAttackMax => Item.WAttackMax;
        public static string Balance => Item.Balance;
        public static string Critical => Item.Critical;
        public static string Defence => Item.Defence;
        public static string Protect => Item.Protect;
        public static string EffectiveRange => Item.EffectiveRange;
        public static string AttackSpeed => Item.AttackSpeed;
        public static string DownHitCount => Item.DownHitCount;
        public static string Experience => Item.Experience;
        public static string ExpPoint => Item.ExpPoint;
        public static string Upgraded => Item.Upgraded;
        public static string UpgradeMax => Item.UpgradeMax;
        public static string Grade => Item.Grade;
        public static string Prefix => Item.Prefix;
        public static string Suffix => Item.Suffix;
        public static string Data => Item.Data;
        public static string Option => Item.Option;
        public static string SellingPrice => Item.SellingPrice;
        public const string DeleteTime = "DeleteTime";
        public static string StoredType = "StoredType";
        public static string Expiration => Item.Expiration;
    }

    public static class ItemHistoryEgo
    {
        public static string Id => Item.ItemId;
        public static string Pocket => Item.PocketId;
        public static string Class => Item.Class;
        public static string Color1 => Item.Color1;
        public static string Color2 => Item.Color2;
        public static string Color3 => Item.Color3;
        public static string Price => Item.Price;
        public static string Bundle => Item.Bundle;
        public static string LinkedPocket => Item.LinkedPocket;
        public static string Figure => Item.Figure;
        public static string Flag => Item.Flag;



        public static string Durability => Item.Durability;
        public static string DurabilityMax => Item.DurabilityMax;
        public static string OriginalDurabilityMax => Item.OriginalDurabilityMax;
        public static string AttackMin => Item.AttackMin;
        public static string AttackMax => Item.AttackMax;
        public static string WAttackMin => Item.WAttackMin;
        public static string WAttackMax => Item.WAttackMax;
        public static string Balance => Item.Balance;
        public static string Critical => Item.Critical;
        public static string Defence => Item.Defence;
        public static string Protect => Item.Protect;
        public static string EffectiveRange => Item.EffectiveRange;
        public static string AttackSpeed => Item.AttackSpeed;
        public static string DownHitCount => Item.DownHitCount;
        public static string Experience => Item.Experience;
        public static string ExpPoint => Item.ExpPoint;
        public static string Upgraded => Item.Upgraded;
        public static string UpgradeMax => Item.UpgradeMax;
        public static string Grade => Item.Grade;
        public static string Prefix => Item.Prefix;
        public static string Suffix => Item.Suffix;
        public static string Data => Item.Data;
        public static string Option => Item.Option;
        public static string SellingPrice => Item.SellingPrice;
        public const string DeleteTime = "DeleteTime";
        public static string Expiration => Item.Expiration;

        public const string EgoName = "EgoName";
        public const string EgoType = "EgoType";
        public const string EgoDesire = "EgoDesire";
        public const string EgoSocialLevel = "EgoSocialLevel";
        public const string EgoSocialExp = "EgoSocialExp";
        public const string EgoStrengthLevel = "EgoStrengthLevel";
        public const string EgoStrengthExp = "EgoStrengthExp";
        public const string EgoIntelligenceLevel = "EgoIntelligenceLevel";
        public const string EgoIntelligenceExp = "EgoIntelligenceExp";
        public const string EgoDexterityLevel = "EgoDexterityLevel";
        public const string EgoDexterityExp = "EgoDexterityExp";
        public const string EgoWillLevel = "EgoWillLevel";
        public const string EgoWillExp = "EgoWillExp";
        public const string EgoLuckLevel = "EgoLuckLevel";
        public const string EgoLuckExp = "EgoLuckExp";
        public const string EgoSkillCount = "EgoSkillCount";
        public const string EgoSkillGauge = "EgoSkillGauge";
        public const string EgoSkillCooldown = "EgoSkillCooldown";
    }

    public static class ItemIdPool
    {
        public const string Count = "Count";
    }

    public static class ItemId
    {
        public const string Id = "Id";
        public static string ItemLoc => Item.ItemLoc;
    }

    public static class LevelRank
    {
        public const string Num = "Num";
        public const string RegDate = "RegDate";
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Level = "Level";
        public const string PlayTime = "PlayTime";
        public const string Experience = "Experience";
        public const string Age = "Age";
        public const string CumulatedLevel = "CumulatedLevel";
        public const string Race = "Race";
    }

    public static class LogDucat
    {
        public const string CharId = "CharId";
        public const string Ducat = "Ducat";
        public const string LogDate = "LogDate";
    }

    public static class LoginIdPool
    {
        public const string Count = "Count";
    }

    public static class MailBoxItem
    {
        public const string ReceiverCharID = "ReceiverCharID";
        public const string SenderCharID = "SenderCharID";
        public const string StoredType = "StoredType";
    }

    public static class MailBoxReceive
    {
        public const string PostId = "PostId";
        public const string ReceiverCharId = "ReceiverCharId";
        public const string ReceiverCharName = "ReceiverCharName";
        public const string SenderCharId = "SenderCharId";
        public const string SenderCharName = "SenderCharName";
        public const string ItemId = "ItemId";
        public const string ItemCharge = "ItemCharge";
        public const string SenderMessage = "SenderMessage";
        public const string SendDate = "SendDate";
        public const string PostType = "PostType";
        public const string Location = "Location";
        public const string Status = "Status";
    }

    public static class NotUsableGameId
    {
        public const string IdX = "IdX";
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Flag = "Flag";
        public const string InsertDate = "InsertDate";
    }

    public static class PersonalRanking
    {
        public const string RankingId = "RankingId";
        public const string CharId = "CharId";
        public const string Score = "Score";
        public const string LastUpdate = "LastUpdate";
    }

    public static class Pet
    {
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Type = "Type";
        public const string SkinColor = "SkinColor";
        public const string EyeType = "EyeType";
        public const string EyeColor = "EyeColor";
        public const string MouthType = "MouthType";
        public const string Status = "Status";
        public const string Height = "Height";
        public const string Fatness = "Fatness";
        public const string Upper = "Upper";
        public const string Lower = "Lower";
        public const string Region = "Region";
        public const string X = "X";
        public const string Y = "Y";
        public const string Direction = "Direction";
        public const string BattleState = "BattleState";
        public const string Extra1 = "Extra1";
        public const string Extra2 = "Extra2";
        public const string Extra3 = "Extra3";
        public const string Life = "Life";
        public const string LifeDamage = "LifeDamage";
        public const string LifeMax = "LifeMax";
        public const string Mana = "Mana";
        public const string ManaMax = "ManaMax";
        public const string Stamina = "Stamina";
        public const string StaminaMax = "StaminaMax";
        public const string Food = "Food";
        public const string Level = "Level";
        public const string Experience = "Experience";
        public const string Age = "Age";
        public const string Strength = "Strength";
        public const string Dexterity = "Dexterity";
        public const string Intelligence = "Intelligence";
        public const string Will = "Will";
        public const string Luck = "Luck";
        public const string AttackMin = "AttackMin";
        public const string AttackMax = "AttackMax";
        public const string WAttackMin = "WAttackMin";
        public const string WAttackMax = "WAttackMax";
        public const string Critical = "Critical";
        public const string Protect = "Protect";
        public const string Defense = "Defense";
        public const string Rate = "Rate";
        public const string StrengthBoost = "StrengthBoost";
        public const string DexterityBoost = "DexterityBoost";
        public const string IntelligenceBoost = "IntelligenceBoost";
        public const string WillBoost = "WillBoost";
        public const string LuckBoost = "LuckBoost";
        public const string HeightBoost = "HeightBoost";
        public const string FatnessBoost = "FatnessBoost";
        public const string UpperBoost = "UpperBoost";
        public const string LowerBoost = "LowerBoost";
        public const string LifeBoost = "LifeBoost";
        public const string ManaBoost = "ManaBoost";
        public const string StaminaBoost = "StaminaBoost";
        public const string Toxic = "Toxic";
        public const string ToxicDrunkenTime = "ToxicDrunkenTime";
        public const string ToxicStrength = "ToxicStrength";
        public const string ToxicIntelligence = "ToxicIntelligence";
        public const string ToxicDexterity = "ToxicDexterity";
        public const string ToxicWill = "ToxicWill";
        public const string ToxicLuck = "ToxicLuck";
        public const string LastTown = "LastTown";
        public const string LastDungeon = "LastDungeon";
        public const string UI = "UI";
        public const string Meta = "Meta";
        public const string Birthday = "Birthday";
        public const string PlayTime = "PlayTime";
        public const string Wealth = "Wealth";
        public const string Condition = "Condition";
        public const string Memory = "Memory";
        public const string Reserved = "Reserved";
        public const string Registered = "Registered";
        public const string Loyalty = "Loyalty";
        public const string Favor = "Favor";
        public const string UpdateTime = "UpdateTime";
        public const string DeleteTime = "DeleteTime";
        public const string CumulatedLevel = "CumulatedLevel";
        public const string MaxLevel = "MaxLevel";
        public const string RebirthCount = "RebirthCount";
        public const string RebirthDay = "RebirthDay";
        public const string RebirthAge = "RebirthAge";
        public const string WriteCounter = "WriteCounter";
        public const string MacroPoint = "MacroPoint";
        public const string CouponCode = "CouponCode";
    }

    public static class PetSkill
    {
        public const string Id = "Id";
        public const string Skill = "Skill";
        public const string Level = "Level";
        public const string Flag = "Flag";
    }

    public static class PetAssetRank
    {
        public const string Num = "Num";
        public const string RegDate = "RegDate";
        public const string PetId = "PetId";
        public const string PetName = "PetName";
        public const string TotalAsset = "TotalAsset";
        public const string Rank = "Rank";
    }

    public static class PlaytimeRank
    {
        public const string Num = "Num";
        public const string RegDate = "RegDate";
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Level = "Level";
        public const string PlayTime = "PlayTime";
        public const string Experience = "Experience";
        public const string Age = "Age";
        public const string CumulatedLevel = "CumulatedLevel";
    }

    public static class PrivateFarm
    {
        public const string Id = "Id";
        public const string OwnerId = "OwnerId";
        public const string ClassId = "ClassId";
        public const string Level = "Level";
        public const string Exp = "Exp";
        public const string UpdateTime = "UpdateTime";
        public const string Name = "Name";
        public const string WorldPosX = "WorldPosX";
        public const string WorldPosY = "WorldPosY";
        public const string CreateDate = "CreateDate";
        public const string DeleteFlag = "DeleteFlag";
        public const string OwnerName = "OwnerName";
        public const string BindedChannel = "BindedChannel";
        public const string NextBindableTime = "NextBindableTime";
    }

    public static class PrivateFarmFacility
    {
        public const string PrivateFarmId = "PrivateFarmId";
        public const string FacilityId = "FacilityId";
        public const string ClassId = "ClassId";
        public const string X = "X";
        public const string Y = "Y";
        public const string Dir = "Dir";
        public const string Color1 = "Color1";
        public const string Color2 = "Color2";
        public const string Color3 = "Color3";
        public const string Color4 = "Color4";
        public const string Color5 = "Color5";
        public const string Color6 = "Color6";
        public const string Color7 = "Color7";
        public const string Color8 = "Color8";
        public const string Color9 = "Color9";
        public static readonly string[] Color = { Color1, Color2, Color3, Color4, Color5, Color6, Color7, Color8, Color9 };
        public const string FinishTime = "FinishTime";
        public const string LastProcessingTime = "LastProcessingTime";
        public const string Meta = "Meta";
        public const string CustomName = "CustomName";
        public const string LinkedFacilityId = "LinkedFacilityId";
        public const string PermissionFlag = "PermissionFlag";
    }

    public static class PrivateFarmFacilityIdPool
    {
        public const string Count = "Count";
    }

    public static class PrivateFarmRecommend
    {
        public const string FarmId = "FarmId";
        public const string OwnerName = "OwnerName";
    }

    public static class PrivateFarmVisitor
    {
        public const string PrivateFarmId = "PrivateFarmId";
        public const string CharName = "CharName";
        public const string Account = "Account";
        public const string CharId = "CharId";
        public const string Status = "Status";
    }

    public static class PromotionRank
    {
        public const string Server = "Server";
        public const string SkillId = "SkillId";
        public const string SkillCategory = "SkillCategory";
        public const string SkillName = "SkillName";
        public const string CharacterId = "CharacterId";
        public const string CharacterName = "CharacterName";
        public const string Race = "Race";
        public const string Level = "Level";
        public const string Point = "Point";
        public const string RegDate = "RegDate";
        public const string Rank = "Rank";
    }

    public static class PromotionRecord
    {
        public const string Server = "Server";
        public const string SkillId = "SkillId";
        public const string SkillCategory = "SkillCategory";
        public const string SkillName = "SkillName";
        public const string CharacterId = "CharacterId";
        public const string CharacterName = "CharacterName";
        public const string Race = "Race";
        public const string Level = "Level";
        public const string Point = "Point";
        public const string RegDate = "RegDate";
        public const string Channel = "Channel";
    }

    public static class Prop
    {
        public const string Id = "Id";
        public const string ClassId = "ClassId";
        public const string Region = "Region";
        public const string X = "X";
        public const string Y = "Y";
        public const string Z = "Z";
        public const string Direction = "Direction";
        public const string Scale = "Scale";
        public const string Color1 = "Color1";
        public const string Color2 = "Color2";
        public const string Color3 = "Color3";
        public const string Color4 = "Color4";
        public const string Color5 = "Color5";
        public const string Color6 = "Color6";
        public const string Color7 = "Color7";
        public const string Color8 = "Color8";
        public const string Color9 = "Color9";
        public const string Name = "Name";
        public const string State = "State";
        public const string EnterTime = "EnterTime";
        public const string Extra = "Extra";
    }

    public static class PropEvent
    {
        public const string Id = "Id";
        public const string Default = "Default";
        public const string Signal = "Signal";
        public const string Type = "Type";
        public const string Extra = "Extra";
    }
    public static class PropIdPool
    {
        public const string Count = "Count";
    }

    public static class GoldLog
    {
        public const string IdX = "IdX";
        public const string Id = "Id";
        public const string Quest = "Quest";
        public const string Field = "Field";
        public const string Commerce = "Commerce";
        public const string Mail = "Mail";
        public const string Bank = "Bank";
        public const string ItemBuySell = "ItemBuySell";
        public const string ItemRepair = "ItemRepair";
        public const string ItemUpgrade = "ItemUpgrade";
        public const string ItemSpecialUpgrade = "ItemSpecialUpgrade";
        public const string Mint = "Mint";
        public const string Guild = "Guild";
        public const string PrivateShop = "PrivateShop";
        public const string Housing = "Housing";
        public const string Etc = "Etc";
        public const string LogDate = "LogDate";
        public const string DynamicRegion = "DynamicRegion";
    }

    public static class Recommend
    {
        public const string OldbieCharName = "OldbieCharName";
        public const string OldbieServerId = "OldbieServerId";
        public const string NewbieCharName = "NewbieCharName";
        public const string NewbieServerId = "NewbieServerId";
        public const string RecommendTime = "RecommendTime";
        public const string FlagTime1 = "FlagTime1";
        public const string FlagTime2 = "FlagTime2";
        public const string FlagTime3 = "FlagTime3";
        public const string FlagTime4 = "FlagTime4";
        public const string FlagTime5 = "FlagTime5";
        public const string FlagTime6 = "FlagTime6";
        public const string FlagTime7 = "FlagTime7";
        public const string FlagTime8 = "FlagTime8";
        public const string FlagTime9 = "FlagTime9";
        public const string FlagTime10 = "FlagTime10";
        public const string FlagTime11 = "FlagTime11";
        public const string FlagTime12 = "FlagTime12";
        public const string FlagTime13 = "FlagTime13";
        public const string FlagTime14 = "FlagTime14";
        public const string FlagTime15 = "FlagTime15";
        public const string FlagTime16 = "FlagTime16";
        public const string FlagTime17 = "FlagTime17";
        public const string FlagTime18 = "FlagTime18";
        public const string FlagTime19 = "FlagTime19";
        public const string FlagTime20 = "FlagTime20";
        public const string FlagTime21 = "FlagTime21";
        public const string FlagTime22 = "FlagTime22";
        public const string FlagTime23 = "FlagTime23";
        public const string FlagTime24 = "FlagTime24";
        public const string FlagTime25 = "FlagTime25";
        public const string FlagTime26 = "FlagTime26";
        public const string FlagTime27 = "FlagTime27";
        public const string FlagTime28 = "FlagTime28";
        public const string FlagTime29 = "FlagTime29";

        public static readonly string[] FlagTime =  {   FlagTime1, FlagTime2, FlagTime3, FlagTime4, FlagTime5, FlagTime6,
            FlagTime7, FlagTime8, FlagTime9, FlagTime10, FlagTime11, FlagTime12, FlagTime13, FlagTime14, FlagTime15,
            FlagTime16, FlagTime17, FlagTime18, FlagTime19, FlagTime20, FlagTime21, FlagTime22, FlagTime23, FlagTime24,
            FlagTime25, FlagTime26, FlagTime27, FlagTime28, FlagTime29 };
    }

    public static class Relic
    {
        public const string RuinId = "RuinId";
        public const string State = "State";
        public const string Position = "Position";
        public const string LastTime = "LastTime";
        public const string ExploCharId = "ExploCharId";
        public const string ExploCharName = "ExploCharName";
        public const string ExploTime = "ExploTime";
    }

    public static class RenewalQuestList
    {
        public const string IdX = "IdX";
        public const string QuestId = "QuestId";
        public const string EditQuestId = "EditQuestId";
        public const string Flag = "Flag";
    }

    public static class ReportCharacterLevel
    {
        public const string RegDate = "RegDate";
        public const string Race = "Race";
        public const string Level = "Level";
        public const string Count = "Count";
    }

    public static class ReportCharacterSkill
    {
        public const string RegDate = "RegDate";
        public const string Race = "Race";
        public const string SkillId = "SkillId";
        public const string Level = "Level";
        public const string Count = "Count";
    }

    public static class ReportDateList
    {
        public const string IdX = "IdX";
        public const string RegDate = "RegDate";
    }

    public static class RoyalAlchemist
    {
        public const string CharId = "CharId";
        public const string CharName = "CharName";
        public const string RegistrationFlag = "RegistrationFlag";
        public const string Rank = "Rank";
        public const string Meta = "Meta";
    }

    public static class Ruin
    {
        public const string RuinId = "RuinId";
        public const string State = "State";
        public const string Position = "Position";
        public const string LastTime = "LastTime";
        public const string ExploCharId = "ExploCharId";
        public const string ExploCharName = "ExploCharName";
        public const string ExploTime = "ExploTime";
    }
    public static class ScrapBook
    {
        public const string CharId = "CharId";
        public const string ScrapType = "ScrapType";
        public const string ClassId = "ClassId";
        public const string ScrapData = "ScrapData";
        public const string RegionId = "RegionId";
        public const string UpdateTime = "UpdateTime";
    }
    public static class ScrapBookBestCook
    {
        public const string ClassId = "ClassId";
        public const string CharId = "CharId";
        public const string Name = "Name";
        public const string Quality = "Quality";
        public const string Comment = "Comment";
        public const string UpdateTime = "UpdateTime";
    }

    public static class ServerAssetInfo
    {
        public const string Num = "Num";
        public const string RegDate = "RegDate";
        public const string TotalAsset = "TotalAsset";
    }
    public static class SoulMate
    {
        public const string MainCharId = "MainCharId";
        public const string SubCharId = "SubCharId";
        public const string MatePoint = "MatePoint";
        public const string StartTime = "StartTime";
        public const string UpdateTime = "UpdateTime";
    }

    public static class Wine
    {
        public const string CharId = "CharId";
        public const string WineType = "WineType";
        public const string AgingCount = "AgingCount";
        public const string AgingStartTime = "AgingStartTime";
        public const string LastRackingTime = "LastRackingTime";
        public const string Acidity = "Acidity";
        public const string Purity = "Purity";
        public const string Freshness = "Freshness";
    }

    public static class WorldMeta
    {
        public const string MetaKey = "MetaKey";
        public const string MetaType = "MetaType";
        public const string MetaValue = "MetaValue";
    }
    #endregion MabinogiDB

    #region MabiGuildDB
    public static class Guild
    {
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Server = "Server";
        public const string GuildPoint = "GuildPoint";
        public const string GuildMoney = "GuildMoney";
        public const string GuildType = "GuildType";
        public const string JoinType = "JoinType";
        public const string MaxMember = "MaxMember";
        public const string MemberCount = "MemberCount";
        public const string GuildAbility = "GuildAbility";
        public const string CreateTime = "CreateTime";
        public const string UpdateTime = "UpdateTime";
        public const string GuildMasterId = "GuildMasterId";
        public const string WebMemberCount = "WebMemberCount";
        public const string Expiration = "Expiration";
        public const string Enable = "Enable";
        public const string MasterChangeTime = "MasterChangeTime";
        public const string DrawableMoney = "DrawableMoney";
        public const string DrawableDate = "DrawableDate";
        public const string BattleGroundType = "BattleGroundType";
        public const string BattleGroundWinnerType = "BattleGroundWinnerType";
        public const string GuildStatusFlag = "GuildStatusFlag";
        public const string GuildTitle = "GuildTitle";
    }

    public static class GuildMember
    {
        public const string GuildId = "GuildId";
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Account = "Account";
        public const string Class = "Class";
        public const string Point = "Point";
        public const string JoinTime = "JoinTime";
        public const string Text = "Text";
        public const string JoinMsg = "JoinMsg";
    }

    public static class AwayGuildMember
    {
        public const string MemberId = "MemberId";
        public const string Server = "Server";
        public const string GuildId = "GuildId";
        public const string JoinTime = "JoinTime";
        public const string Name = "Name";
        public const string Account = "Account";
        public const string AwayTime = "AwayTime";
    }


    public static class GuildIdPool
    {
        public const string Count = "Count";
    }

    public static class GuildStone
    {
        public const string GuildId = "GuildId";
        public const string Server = "Server";
        public const string PositionId = "PositionId";
        public const string Type = "Type";
        public const string Region = "Region";
        public const string X = "X";
        public const string Y = "Y";
        public const string Direction = "Direction";
    }

    public static class GuildRobe
    {
        public const string GuildId = "GuildId";
        public const string EmblemChestIcon = "EmblemChestIcon";
        public const string EmblemChestDeco = "EmblemChestDeco";
        public const string EmblemBeltDeco = "EmblemBeltDeco";
        public const string Color1 = "Color1";
        public const string Color2Index = "Color2Index";
        public const string Color3Index = "Color3Index";
        public const string Color4Index = "Color4Index";
        public const string Color5Index = "Color5Index";
    }

    public static class GuildDeleted
    {
        public const string Id = "Id";
        public const string Name = "Name";
        public const string Server = "Server";
        public const string DeleteTime = "DeleteTime";
    }

    public static class GuildMenu
    {
        public const string GuildId = "GuildId";
        public const string MenuId = "MenuId";
        public const string MenuName = "MenuName";
        public const string Level1 = "Level1";
        public const string Level2 = "Level2";
        public const string Level3 = "Level3";
        public const string Level4 = "Level4";
    }

    // I don't have a use for this table, but I added it from the SQL.
    //public static class GuildPollCheck
    //{
    //    public const string PollID = "";
    //    public const string Server = "";
    //    public const string CharacterName = "";
    //}

    public static class GuildText
    {
        public const string GuildId = "GuildId";
        public const string Profile = "Profile";
        public const string Greeting = "Greeting";
        public const string Leaving = "Leaving";
        public const string Refuse = "Refuse";
        public const string Emblem = "Emblem";
    }
    #endregion MabiGuildDB

    #region Chronicle
    public static class Chronicle
    {
        public const string CharId = "CharId";
        public const string ServerName = "ServerName";
        public const string QuestId = "QuestId";
        public const string CreateTime = "CreateTime";
        public const string Meta = "Meta";
    }

    public static class ChronicleEventRank
    {
        public const string ServerName = "ServerName";
        public const string CharId = "CharId";
        public const string QuestId = "QuestId";
        public const string CharName = "CharName";
        public const string Rank = "Rank";
        public const string EventCount = "EventCount";
        public const string CountRank = "CountRank";
        public const string UpdateTime = "UpdateTime";
    }

    public static class ChronicleLatestRank
    {
        public const string Id = "Id";
        public const string ServerName = "ServerName";
        public const string QuestId = "QuestId";
        public const string CharId = "CharId";
        public const string CharName = "CharName";
        public const string RankTime = "RankTime";
        public const string Rank = "Rank";
    }

    public static class ChronicleFirstRank
    {
        public const string ServerName = "ServerName";
        public const string QuestId = "QuestId";
        public const string CharId = "CharId";
        public const string CharName = "CharName";
        public const string RankTime = "RankTime";
    }

    public static class ChronicleInfo
    {
        public const string QuestId = "QuestId";
        public const string QuestName = "QuestName";
        public const string Keyword = "Keyword";
        public const string LocalText = "LocalText";
        public const string Source = "Source";
        public const string Width = "Width";
        public const string Height = "Height";
        public const string Sort = "Sort";
        public const string Group = "Group";
    }
    #endregion Chronicle

    #region DungeonRank
    public static class DungeonScoreRankInfo
    {
        public const string Server = "Server";
        public const string DungeonName = "DungeonName";
        public const string Race = "Race";
        public const string ScoreRow = "ScoreRow";
    }
    public static class DungeonTimeRankInfo
    {
        public const string Server = "Server";
        public const string DungeonName = "DungeonName";
        public const string Race = "Race";
        public const string TimeRow = "TimeRow";
    }
    public static class DungeonScoreBoard
    {
        public const string Idx = "Idx";
        public const string Server = "Server";
        public const string DungeonName = "DungeonName";
        public const string Race = "Race";
        public const string CharacterId = "CharacterId";
        public const string CharacterName = "CharacterName";
        public const string Score = "Score";
        public const string RegDate = "RegDate";
    }
    public static class DungeonTimeBoard
    {
        public const string Idx = "Idx";
        public const string Server = "Server";
        public const string DungeonName = "DungeonName";
        public const string Race = "Race";
        public const string CharacterId = "CharacterId";
        public const string CharacterName = "CharacterName";
        public const string LapTime = "LapTime";
        public const string RegDate = "RegDate";
    }
    #endregion DungeonRank

    #region Shop
    public static class FantasyLifeClub
    {
        public const string Id = "Id";
        public const string NaoSupportExpiration = "NaoSupportExpiration";
        public const string StorageExpiration = "StorageExpiration";
        public const string AdvancedPlayExpiration = "AdvancedPlayExpiration";
        public const string Updated = "Updated";
        public const string Checked = "Checked";
    }
    public static class Cards
    {
        public const string Id = "Id";
        public const string CardId = "CardId";
        public const string Type = "Type";
        //public const string TypeId = "TypeId";
        public const string Status = "Status";
        public const string EntityId = "EntityId";
        public const string EntityName = "EntityName";
        public const string RebirthCount = "RebirthCount";
        public const string Server = "Server";
        public const string Reserved = "Reserved";
        public const string Created = "Created";
        public const string Used = "Used";
        public const string Ended = "Ended";
        public const string ValidServer = "ValidServer";
    }

    public static class PremiumPack
    {
        public const string Id = "Id";
        public const string InventoryPlus = "InventoryPlus";
        public const string PremPack = "PremPack";
        public const string Updated = "Updated";
        public const string Checked = "Checked";
        public const string VIP = "VIP";
        public const string PremiumVIP = "PremiumVIP";
        public const string GuildPack = "GuildPack";
    }

    public static class Coupons
    {
        public const string Id = "Id";
        public const string Code = "Code";
        public const string Type = "Type";
        public const string State = "State";
        public const string Account = "Account";
        public const string EntityId = "EntityId";
        public const string EntityName = "EntityName";
        public const string Server = "Server";
        public const string StartTime = "StartTime";
        public const string EndTime = "EndTime";
        public const string MainData = "MainData";
        public const string SubData = "SubData";
        public const string RollbackCount = "RollbackCount";
        public const string StartValidityTerm = "StartValidityTerm";
        public const string EndValidityTerm = "EndValidityTerm";
        public const string ValidServer = "ValidServer";
    }

    public static class Gifts
    {
        public const string CardId = "CardId";
        public const string CardType = "CardType";
        public const string TypeId = "TypeId";
        public const string Type = "Type";
        public const string Status = "Status";
        public const string SenderId = "SenderId";
        public const string SenderCharId = "SenderCharId";
        public const string SenderCharName = "SenderCharName";
        public const string SenderServer = "SenderServer";
        public const string ReceiverId = "ReceiverId";
        public const string ReceiverCharId = "ReceiverCharId";
        public const string ReceiverCharName = "ReceiverCharName";
        public const string ReceiverServer = "ReceiverServer";
        public const string SenderMessage = "SenderMessage";
        public const string SendDate = "SendDate";
        public const string RejectDate = "RejectDate";
    }

    public static class GiftHistory
    {
        public static string CardId => Gifts.CardId;
        public static string CardType => Gifts.CardType;
        public static string TypeId => Gifts.TypeId;
        public static string Type => Gifts.Type;
        public static string Status => Gifts.Status;
        public static string SenderId => Gifts.SenderId;
        public static string SenderCharId => Gifts.SenderCharId;
        public static string SenderCharName => Gifts.SenderCharName;
        public static string SenderServer => Gifts.SenderServer;
        public static string ReceiverId => Gifts.ReceiverId;
        public static string ReceiverCharId => Gifts.ReceiverCharId;
        public static string ReceiverCharName => Gifts.ReceiverCharName;
        public static string ReceiverServer => Gifts.ReceiverServer;
        public static string SenderMessage => Gifts.SenderMessage;
        public static string SendDate => Gifts.SendDate;
        public static string RejectDate => Gifts.RejectDate;

        public const string GiftId = "GiftId";
        public const string RegDate = "RegDate";
    }

    public static class FreeServiceAccount
    {
        public const string Account = "Account";
        public const string UpdateTime = "UpdateTime";
    }
    #endregion Shop

    #region MabiNovel
    public static class MabiNovelBoard
    {
        public const string BoardSn = "BoardSn";
        public const string Server = "Server";
        public const string CharId = "CharId";
        public const string Title = "Title";
        public const string TransCount = "TransCount";
        public const string EndDate = "EndDate";
        public const string BlockCount = "BlockCount";
        public const string Flag = "Flag";
        public const string ReadCount = "ReadCount";
        public const string BlockDate = "BlockDate";
        public const string UpdateTime = "UpdateTime";
    }

    public static class MabiNovel
    {
        public const string BoardSn = "BoardSn";
        public const string Page = "Page";
        public const string BackgroundId = "BackgroundId";
        public const string BgmId = "BgmId";
        public const string PortraitId = "PortraitId";
        public const string PortraitPos = "PortraitPos";
        public const string EmotionId = "EmotionId";
        public const string SoundEffectId = "SoundEffectId";
        public const string EffectId = "EffectId";
        public const string Ambassador = "Ambassador";
    }
    #endregion MabiNovel

    #region MabiMemo
    public static class MemoBlacklist
    {
        public const string Id = "Id";
        public const string FromId = "FromId";
        public const string ToId = "ToId";
        public const string FromName = "FromName";
        public const string ToName = "ToName";
        public const string ToServer = "ToServer";
        public const string ToLevel = "ToLevel";
    }

    public static class Memo
    {
        public const string Id = "Id";
        public const string FromName = "FromName";
        public const string FromId = "FromId";
        public const string ToName = "ToName";
        public const string ToId = "ToId";
        public const string FromDate = "FromDate";
        public const string ToDate = "ToDate";
        public const string Content = "Content";
        public const string ToCheck = "ToCheck";
        public const string FromCheck = "FromCheck";
        public const string FromServer = "FromServer";
        public const string FromLevel = "FromLevel";
        public const string ToServer = "ToServer";
        public const string ToLevel = "ToLevel";
    }

    #endregion MabiMemo

    #region ShopAdvertise
    public static class ShopAdvertiseItem
    {
        public const string Account = "Account";

        public const string Server = "Server";

        public const string ItemID = "ItemID";

        public const string StoredType = "StoredType";

        public const string ItemName = "ItemName";

        public const string Price = "Price";

        public const string Class = "Class";

        public const string Color1 = "Color1";

        public const string Color2 = "Color2";

        public const string Color3 = "Color3";
    }

    public static class ShopAdvertise
    {
        public const string Account = "Account";

        public const string Server = "Server";

        public const string ShopName = "ShopName";

        public const string Area = "Area";

        public const string CharacterName = "CharacterName";

        public const string Comment = "Comment";

        public const string StartTime = "StartTime";

        public const string Region = "Region";

        public const string X = "X";

        public const string Y = "Y";

        public const string LeafletCount = "LeafletCount";
    }
    #endregion ShopAdvertise
}

