using System;

namespace XMLDB3
{
	public class QueryManager
	{
		private static QueryManager m_QueryManager = new QueryManager();

		private AccountAdapter m_AccountAdapter;

		private AccountActivationAdapter m_AccountActivationAdapter;

		private AccountRefAdapter m_AccountRefAdapter;

		private CharacterAdapter m_CharacterAdapter;

		private PetAdapter m_PetAdapter;

		private BankAdapter m_BankAdapter;

		private GuildAdapter m_GuildAdapter;

		private PropAdapter m_PropAdapter;

		private WebSynchAdapter m_WebSynchAdapter;

		private CastleAdapter m_CastleAdapter;

		private HouseAdapter m_HouseAdapter;

		private MemoAdapter m_MemoAdapter;

		private CharIdPoolAdapter m_CharacterIdPool;

		private GuildIdPoolAdapter m_GuildIdPool;

		private ItemIdPoolAdapter m_ItemIdPool;

		private PropIdPoolAdapter m_PropIdPool;

		private BidIdPoolAdapter m_BidIdPool;

		private ChannelingKeyPoolAdapter m_ChannelingKeyPool;

		private LoginIdPoolAdapter m_LoginIdPool;

		private FacilityIdPoolAdapter m_FacilityIdPool;

		private ChronicleAdapter m_ChronicleAdapter;

		private PromotionAdapter m_PromotionAdapter;

		private RuinAdapter m_RuinAdapter;

		private ShopAdvertiseAdapter m_ShopAdvertiseAdapter;

		private DungeonRankAdapter m_DungeonRankAdapter;

		private MailBoxAdapter m_MailBoxAdapter;

		private FarmAdapter m_FarmAdapter;

		private BidAdapter m_BidAdapter;

		private EventAdapter m_EventAdapter;

		private WorldMetaAdapter m_WorldMetaAdapter;

		private WineAdapter m_WineAdapter;

		private RoyalAlchemistAdapter m_RoyalAlchemistAdapter;

		private FamilyAdapter m_FamilyAdapter;

		private CountryReportAdapter m_CountryReportAdapter;

		private LogInOutReportAdapter m_LogInOutReportAdapter;

		private HuskyAdapter m_HuskyAdapter;

		private PrivateFarmAdapter m_PrivateFarmAdapter;

		private PrivateFarmRecommendAdapter m_PrivateFarmRecommendAdapter;

		private ScrapBookAdapter m_ScrapBookAdapter;

		private GS_CommerceAdapter m_GS_CommerceAdapter;

        private CommerceSystemAdapter m_CommerceSystemAdapter;

		private CommerceCriminalAdapter m_CommerceCriminalAdapter;

		private RecommendAdapter m_RecommendAdapter;

		private GoldLogAdapter m_GoldLogAdapter;

		private LinkedApCharacterAdapter m_LinkedApCharacterAdapter;

		private EquipmentCollectionAdapter m_EquipmentCollectionAdapter;

		private SoulMateAdapter m_SoulMateAdapter;

		private PersonalRankingAdapter m_PersonalRankingAdapter;

		private SetInfoAdapter m_SetInfoAdapter;

		private MabiNovelAdapter m_MabiNovelAdapter;

		private MabiNovelBoardAdapter m_MabiNovelBoardAdapter;

		private HelpPointRankAdapter m_HelpPointRankAdapter;

		private InviteEventAdapter m_InviteEventAdapter;

		public static AccountAdapter Account => m_QueryManager.m_AccountAdapter;

		public static AccountActivationAdapter AccountActivation => m_QueryManager.m_AccountActivationAdapter;

		public static AccountRefAdapter Accountref => m_QueryManager.m_AccountRefAdapter;

		public static CharacterAdapter Character => m_QueryManager.m_CharacterAdapter;

		public static PetAdapter Pet => m_QueryManager.m_PetAdapter;

		public static BankAdapter Bank => m_QueryManager.m_BankAdapter;

		public static GuildAdapter Guild => m_QueryManager.m_GuildAdapter;

		public static CastleAdapter Castle => m_QueryManager.m_CastleAdapter;

		public static HouseAdapter House => m_QueryManager.m_HouseAdapter;

		public static ChannelingKeyPoolAdapter ChannelingKeyPool => m_QueryManager.m_ChannelingKeyPool;

		public static PropAdapter Prop => m_QueryManager.m_PropAdapter;

		public static WebSynchAdapter WebSynch => m_QueryManager.m_WebSynchAdapter;

		public static CharIdPoolAdapter CharacterIdPool => m_QueryManager.m_CharacterIdPool;

		public static ItemIdPoolAdapter ItemIDPool => m_QueryManager.m_ItemIdPool;

		public static GuildIdPoolAdapter GuildIdPool => m_QueryManager.m_GuildIdPool;

		public static PropIdPoolAdapter PropIdPool => m_QueryManager.m_PropIdPool;

		public static BidIdPoolAdapter BidIdPool => m_QueryManager.m_BidIdPool;

		public static LoginIdPoolAdapter LoginIdPool => m_QueryManager.m_LoginIdPool;

		public static FacilityIdPoolAdapter FacilityIdPool => m_QueryManager.m_FacilityIdPool;

		public static MemoAdapter Memo => m_QueryManager.m_MemoAdapter;

		public static ChronicleAdapter Chronicle => m_QueryManager.m_ChronicleAdapter;

		public static RuinAdapter Ruin => m_QueryManager.m_RuinAdapter;

		public static ShopAdvertiseAdapter ShopAdvertise => m_QueryManager.m_ShopAdvertiseAdapter;

		public static DungeonRankAdapter DungeonRank => m_QueryManager.m_DungeonRankAdapter;

		public static PromotionAdapter PromotionRank => m_QueryManager.m_PromotionAdapter;

		public static MailBoxAdapter MailBox => m_QueryManager.m_MailBoxAdapter;

		public static FarmAdapter Farm => m_QueryManager.m_FarmAdapter;

		public static BidAdapter Bid => m_QueryManager.m_BidAdapter;

		public static EventAdapter Event => m_QueryManager.m_EventAdapter;

		public static WorldMetaAdapter WorldMeta => m_QueryManager.m_WorldMetaAdapter;

		public static WineAdapter Wine => m_QueryManager.m_WineAdapter;

		public static RoyalAlchemistAdapter RoyalAlchemist => m_QueryManager.m_RoyalAlchemistAdapter;

		public static FamilyAdapter Family => m_QueryManager.m_FamilyAdapter;

		public static CountryReportAdapter CountryReport => m_QueryManager.m_CountryReportAdapter;

		public static LogInOutReportAdapter LogInOutReport => m_QueryManager.m_LogInOutReportAdapter;

		public static HuskyAdapter HuskyEvent => m_QueryManager.m_HuskyAdapter;

		public static PrivateFarmAdapter PrivateFarm => m_QueryManager.m_PrivateFarmAdapter;

		public static PrivateFarmRecommendAdapter PrivateFarmRecommend => m_QueryManager.m_PrivateFarmRecommendAdapter;

		public static ScrapBookAdapter ScrapBook => m_QueryManager.m_ScrapBookAdapter;

		public static GS_CommerceAdapter GSCommerce => m_QueryManager.m_GS_CommerceAdapter;

		public static CommerceSystemAdapter COCommerce => m_QueryManager.m_CommerceSystemAdapter;

		public static CommerceCriminalAdapter CCCommerce => m_QueryManager.m_CommerceCriminalAdapter;

		public static RecommendAdapter Recommend => m_QueryManager.m_RecommendAdapter;

		public static GoldLogAdapter GoldLog => m_QueryManager.m_GoldLogAdapter;

		public static LinkedApCharacterAdapter LinkedApCharacter => m_QueryManager.m_LinkedApCharacterAdapter;

		public static EquipmentCollectionAdapter EquipmentCollection => m_QueryManager.m_EquipmentCollectionAdapter;

		public static SoulMateAdapter SoulMate => m_QueryManager.m_SoulMateAdapter;

		public static SetInfoAdapter SetInfo => m_QueryManager.m_SetInfoAdapter;

		public static PersonalRankingAdapter PersonalRanking => m_QueryManager.m_PersonalRankingAdapter;

		public static MabiNovelAdapter MabiNovel => m_QueryManager.m_MabiNovelAdapter;

		public static MabiNovelBoardAdapter MabiNovelBoard => m_QueryManager.m_MabiNovelBoardAdapter;

		public static HelpPointRankAdapter HelpPointRank => m_QueryManager.m_HelpPointRankAdapter;

		public static InviteEventAdapter InviteEvent => m_QueryManager.m_InviteEventAdapter;

		public static void Initialize()
		{
				m_QueryManager.m_AccountAdapter = new AccountAdapter();
				m_QueryManager.m_AccountActivationAdapter = new AccountActivationAdapter();
				m_QueryManager.m_AccountRefAdapter = new AccountRefAdapter();
				m_QueryManager.m_CharacterAdapter = new CharacterAdapter();
				m_QueryManager.m_PetAdapter = new PetAdapter();
				m_QueryManager.m_BankAdapter = new BankAdapter();
				m_QueryManager.m_GuildAdapter = new GuildAdapter();
				m_QueryManager.m_CastleAdapter = new CastleAdapter();
				m_QueryManager.m_HouseAdapter = new HouseAdapter();
				m_QueryManager.m_PropAdapter = new PropAdapter();
				m_QueryManager.m_WebSynchAdapter = new WebSynchAdapter();
				m_QueryManager.m_ChannelingKeyPool = new ChannelingKeyPoolAdapter();
				m_QueryManager.m_CharacterIdPool = new CharIdPoolAdapter();
				m_QueryManager.m_ItemIdPool = new ItemIdPoolAdapter();
				m_QueryManager.m_PropIdPool = new PropIdPoolAdapter();
				m_QueryManager.m_GuildIdPool = new GuildIdPoolAdapter();
				m_QueryManager.m_LoginIdPool = new LoginIdPoolAdapter();
				m_QueryManager.m_FacilityIdPool = new FacilityIdPoolAdapter();
				m_QueryManager.m_BidIdPool = new BidIdPoolAdapter();
				m_QueryManager.m_MemoAdapter = new MemoAdapter();
				m_QueryManager.m_ChronicleAdapter = new ChronicleAdapter();
				m_QueryManager.m_RuinAdapter = new RuinAdapter();
				m_QueryManager.m_ShopAdvertiseAdapter = new ShopAdvertiseAdapter();
				m_QueryManager.m_DungeonRankAdapter = new DungeonRankAdapter();
				m_QueryManager.m_PromotionAdapter = new PromotionAdapter();
				m_QueryManager.m_MailBoxAdapter = new MailBoxAdapter();
				m_QueryManager.m_FarmAdapter = new FarmAdapter();
				m_QueryManager.m_BidAdapter = new BidAdapter();
				m_QueryManager.m_EventAdapter = new EventAdapter();
				m_QueryManager.m_WorldMetaAdapter = new WorldMetaAdapter();
				m_QueryManager.m_WineAdapter = new WineAdapter();
				m_QueryManager.m_RoyalAlchemistAdapter = new RoyalAlchemistAdapter();
				m_QueryManager.m_FamilyAdapter = new FamilyAdapter();
				m_QueryManager.m_CountryReportAdapter = new CountryReportAdapter();
				m_QueryManager.m_LogInOutReportAdapter = new LogInOutReportAdapter();
				m_QueryManager.m_HuskyAdapter = new HuskyAdapter();
				m_QueryManager.m_PrivateFarmAdapter = new PrivateFarmAdapter();
				m_QueryManager.m_PrivateFarmRecommendAdapter = new PrivateFarmRecommendAdapter();
				m_QueryManager.m_ScrapBookAdapter = new ScrapBookAdapter();
				m_QueryManager.m_GS_CommerceAdapter = new GS_CommerceAdapter();
				m_QueryManager.m_CommerceSystemAdapter = new CommerceSystemAdapter();
				m_QueryManager.m_CommerceCriminalAdapter = new CommerceCriminalAdapter();
				m_QueryManager.m_RecommendAdapter = new RecommendAdapter();
				m_QueryManager.m_GoldLogAdapter = new GoldLogAdapter();
				m_QueryManager.m_LinkedApCharacterAdapter = new LinkedApCharacterAdapter();
				m_QueryManager.m_EquipmentCollectionAdapter = new EquipmentCollectionAdapter();
				m_QueryManager.m_SoulMateAdapter = new SoulMateAdapter();
				m_QueryManager.m_SetInfoAdapter = new SetInfoAdapter();
				m_QueryManager.m_PersonalRankingAdapter = new PersonalRankingAdapter();
				m_QueryManager.m_MabiNovelAdapter = new MabiNovelAdapter();
				m_QueryManager.m_MabiNovelBoardAdapter = new MabiNovelBoardAdapter();
				m_QueryManager.m_HelpPointRankAdapter = new HelpPointRankAdapter();
				m_QueryManager.m_InviteEventAdapter = new InviteEventAdapter();
		}

        public static void CreateGMAccount(string account)
        {
            Account.CreateGMAccount(account);
        }
    }
}
