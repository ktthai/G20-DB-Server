namespace Mabinogi.SQL.Tables
{
    // Commented out values are tables I didn't need to implement
    public static class Mabinogi
    {
        public const string AcAuth = "AcAuth";
        public const string Account = "Account";
        public const string AccountCache = "AccountCache";
        public const string AccountCharacterLinkedAP = "AccountCharacterLinkedAP";
        public const string AccountCharacterRef = "AccountCharacterRef";
        public const string AccountLog = "AccountLog";
        public const string AccountMeta = "AccountMeta";
        public const string AccountPawnCoin = "AccountPawnCoin";
        public const string AccountPetRef = "AccountPetRef";
        public const string AccountSession = "AccountSession";
        public const string AccountLoginHistory = "AccountLoginHistory";
        public const string AccountRef = "AccountRef";
        public const string AssetRank = "AssetRank";
        public const string Bank = "Bank";
        public const string BankSlot = "BankSlot";
        public const string BankItemHuge = "BankItemHuge";
        public const string BankItemLarge = "BankItemLarge";
        public const string BankItemQuest = "BankItemQuest";
        public const string BankItemSmall = "BankItemSmall";
        public const string Bid = "Bid";
        public const string BidIdPool = "BidIdPool";
        public const string Castle = "Castle";
        public const string CastleBlock = "CastleBlock";
        public const string CastleBuildResource = "CastleBuildResource";
        public const string CastleBid = "CastleBid";
        public const string CastleBidder = "CastleBidder";
        public const string CastleBidderHistory = "CastleBidderHistory";
        public const string ChannelingKeyPool = "ChannelingKeyPool";
        public const string CharDeletedRefSync = "CharDeletedRefSync";
        public const string CharRefSync = "CharRefSync";
        public const string Character = "Character";
        public const string CharacterDeed = "CharacterDeed";
        public const string CharacterDivineKnight = "CharacterDivineKnight";
        public const string CharacterKeyword = "CharacterKeyword";
        public const string CharacterMeta = "CharacterMeta";
        public const string CharacterMyKnights = "CharacterMyKnights";
        public const string CharacterMyKnightsMember = "CharacterMyKnightsMember";
        public const string CharacterPvP = "CharacterPvP";
        public const string CharacterShape = "CharacterShape";
        public const string CharacterSkill = "CharacterSkill";
        public const string CharacterSubskill = "CharacterSubskill";
        public const string CharacterAchievement = "CharacterAchievement";
        public const string CharacterQuest = "CharacterQuest";
        public const string CharIdPool = "CharIdPool";
        public const string CharItemEgo = "CharItemEgo";
        public const string CharItemHuge = "CharItemHuge";
        public const string CharItemLarge = "CharItemLarge";
        public const string CharItemQuest = "CharItemQuest";
        public const string CharItemSmall = "CharItemSmall";
        public const string Commerce = "Commerce";
        public const string CommerceCriminal = "CommerceCriminal";
        public const string CommerceCriminalReward = "CommerceCriminalReward";
        public const string CommercePost = "CommercePost";
        public const string CommerceProduct = "CommerceProduct";
        public const string CommerceProductStock = "CommerceProductStock";
        public const string CommercePurchasedProduct = "CommercePurchasedProduct";
        public const string CumulativeLevelRank = "CumulativeLevelRank";
        public const string DeleteChar = "DeleteChar";
        public const string EquipCollect = "EquipCollect";
        public const string EquipCollectItemLarge = "EquipCollectItemLarge";
        public const string Family = "Family";
        public const string FamilyMember = "FamilyMember";
        public const string Farm = "Farm";
        public const string FavoritePrivateFarm = "FavoritePrivateFarm";
        public const string GameId = "GameId";
        public const string GoldLog = "GoldLog";
        public const string GuestBook = "GuestBook";
        public const string GuestBookComment = "GuestBookComment";
        public const string HelpPointRanking = "HelpPointRanking";
        public const string House = "House";
        public const string HouseBlock = "HouseBlock";
        public const string HouseBid = "HouseBid";
        public const string HouseBidder = "HouseBidder";
        public const string HouseBidderHistory = "HouseBidderHistory";
        public const string HouseItemHuge = "HouseItemHuge";
        public const string HouseItemLarge = "HouseItemLarge";
        public const string HouseItemQuest = "HouseItemQuest";
        public const string HouseItemSmall = "HouseItemSmall";
        public const string HouseOwner = "HouseOwner";
        public const string InviteEvent = "InviteEvent";
        public const string ItemHistory1 = "ItemHistory1";
        public const string ItemHistory2 = "ItemHistory2";
        public const string ItemEgoHistory = "ItemEgoHistory";
        public const string ItemEgoId = "ItemEgoId";
        public const string ItemHugeId = "ItemHugeId";
        public const string ItemIdPool = "ItemIdPool";
        public const string ItemLargeId = "ItemLargeId";
        public const string ItemQuestId = "ItemQuestId";
        public const string ItemSmallId = "ItemSmallId";
        public const string LevelRank = "LevelRank";
        public const string LogDucat = "LogDucat";
        public const string LoginIdPool = "LoginIdPool";
        public const string MailBoxItem = "MailBoxItem";
        public const string MailBoxReceive = "MailBoxReceive";
        public const string NotUsableGameId = "NotUsableGameId";
        public const string PersonalRanking = "PersonalRanking";
        public const string Pet = "Pet";
        public const string PetSkill = "PetSkill";
        public const string PetAssetRank = "PetAssetRank";
        public const string PlaytimeRank = "PlaytimeRank";
        public const string PrivateFarm = "PrivateFarm";
        public const string PrivateFarmFacility = "PrivateFarmFacility";
        public const string PrivateFarmFacilityIdPool = "PrivateFarmFacilityIdPool";
        public const string PrivateFarmRecommend = "PrivateFarmRecommend";
        public const string PrivateFarmVisitor = "PrivateFarmVisitor";
        public const string PromotionRank = "PromotionRank";
        public const string PromotionRecord = "PromotionRecord";
        public const string Prop = "Prop";
        public const string PropEvent = "PropEvent";
        public const string PropIdPool = "PropIdPool";
        public const string Recommend = "Recommend";
        public const string Relic = "Relic";
        public const string RenewalQuestList = "RenewalQuestList";
        public const string ReportCharacterLevel = "ReportCharacterLevel";
        public const string ReportCharacterSkill = "ReportCharacterSkill";
        public const string ReportDateList = "ReportDateList";
        public const string RoyalAlchemist = "RoyalAlchemist";
        public const string Ruin = "Ruin";
        public const string ScrapBook = "ScrapBook";
        public const string ScrapBookBestCook = "ScrapBookBestCook";
        public const string ServerAssetInfo = "ServerAssetInfo";
        public const string SoulMate = "SoulMate";
        public const string Wine = "Wine";
        public const string WorldMeta = "WorldMeta";
        public const string Husky = "Husky";
    }

    public static class MabiGuild
    {
        public const string AwayGuildMember = "AwayGuildMember";
        public const string Guild = "Guild";
        public const string GuildDeleted = "GuildDeleted";
        public const string GuildMenu = "GuildMenu";
        //public const string GuildPollCheck = "";
        //public const string GuildPollItem = "";
        //public const string GuildPollQuest = "";
        public const string GuildRobe = "GuildRobe";
        public const string GuildStone = "GuildStone";
        public const string GuildText = "GuildText";
        //public const string GuildBudget = "";
        //public const string GuildBudgetCancel = "";
        //public const string GuildBudgetCheck = "";
        public const string GuildIdPool = "GuildIdPool";
        public const string GuildMember = "GuildMember";
    }

    public static class MabiChronicle
    {
        public const string Chronicle = "Chronicle";
        //public const string ChronicleUpdate = "";
        //public const string ChronicleSwitch = "";
        //public const string ChronicleRankUpdateTime = "";
        public const string ChronicleLatestRank = "ChronicleLatestRank";
        public const string ChronicleInfo = "ChronicleInfo";
        public const string ChronicleFirstRank = "ChronicleFirstRank";
        public const string ChronicleEventRank = "ChronicleEventRank";
        //public const string TempEventCountRank = "";
    }

    public static class Mabi_Novel
    {
        public const string MabiNovelBoard = "MabiNovelBoard";
        public const string MabiNovel = "MabiNovel";
        //public const string NovelInsert = "";
    }

    public static class MabiMemo
    {
        public const string Memo = "Memo";
        public const string MemoBlacklist = "MemoBlacklist";
        //public const string MessengerMove = "";
    }

    public static class DungeonRank
    {
        public const string DungeonScoreRankInfo = "DungeonScoreRankInfo";
        public const string DungeonScoreBoard = "DungeonScoreBoard";
        public const string DungeonTimeBoard = "DungeonTimeBoard";
        public const string DungeonTimeRankInfo = "DungeonTimeRankInfo";
    }

    public static class Shop
    {
        public const string FantasyLifeClub = "FantasyLifeClub";
        public const string CharacterCards = "CharacterCards";
        public const string PetCards = "PetCards";
        public const string Gifts = "Gifts";
        public const string Coupons = "Coupons";
        public const string PremiumPack = "PremiumPack";
        public const string GiftHistory = "GiftHistory";
        public const string FreeServiceAccount = "FreeServiceAccount";
    }

    public static class ShopAdvertise
    {
        public const string Item = "Item";
        public const string Advertise = "Advertise";
    }
}