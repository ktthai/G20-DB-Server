public class ConfigurationSqlConnections
{
	public Connection Account { get; set; }

	public Connection Accountref { get; set; }

	public Connection Character { get; set; }

	public Connection Prop { get; set; }

	public Connection Bank { get; set; }

	public Connection ItemIdPool { get; set; }

	public Connection CharIdPool { get; set; }

	public Connection PropIdPool { get; set; }

	public Connection Guild { get; set; }

	public Connection GuildIdPool { get; set; }

	public Connection LoginIdPool { get; set; }

	public Connection WebSync { get; set; }

	public Connection Castle { get; set; }

	public Connection House { get; set; }

	public Connection Memo { get; set; }

	public Connection Chronicle { get; set; }

	public Connection Ruin { get; set; }

	public Connection ShopAdvertise { get; set; }

	public Connection HouseGuestBook { get; set; }

	public Connection DungeonRank { get; set; }

	public Connection ChannelingKeyPool { get; set; }

	public Connection PromotionRank { get; set; }

	public Connection Mailbox { get; set; }

	public Connection Farm { get; set; }

	public Connection BidIdPool { get; set; }

	public Connection Bid { get; set; }

	public Connection Event { get; set; }

	public Connection WorldMeta { get; set; }

	public Connection Wine { get; set; }

	public Connection CountryReport { get; set; }

	public Connection LoginOutReport { get; set; }

	public Connection Husky { get; set; }

	public Connection PrivateFarm { get; set; }

	public Connection PrivateFarmRecommend { get; set; }

	public Connection FacilityIdPool { get; set; }

	public Connection ScrapBook { get; set; }

	public Connection Commerce { get; set; }

	public Connection CommerceSystem { get; set; }

	public Connection CommerceCriminal { get; set; }

	public Connection Recommend { get; set; }

	public Connection GoldLog { get; set; }

	public Connection LinkedApCharacter { get; set; }

	public Connection EquipmentCollection { get; set; }

	public Connection Soulmate { get; set; }

	public Connection SetInfo { get; set; }

	public Connection SnapshotData { get; set; }

	public Connection PersonalRanking { get; set; }

	public Connection DefaultConnection { get; set; }

	public Connection MabiNovel { get; set; }

	public Connection MabiNovelBoard { get; set; }

	public Connection HelpPointRank { get; set; }

	public Connection InviteEvent { get; set; }

	public ConfigurationSqlConnections()
	{
		Account = new Connection();
		Accountref = new Connection();
		Character = new Connection();
		Prop = new Connection();
		Bank = new Connection();
		ItemIdPool = new Connection();
		CharIdPool = new Connection();
		PropIdPool = new Connection();
		Guild = new Connection();
		GuildIdPool = new Connection();
		LoginIdPool = new Connection();
		WebSync = new Connection();
		Castle = new Connection();
		House = new Connection();
		Memo = new Connection();
		Chronicle = new Connection();
		Ruin = new Connection();
		ShopAdvertise = new Connection();
		HouseGuestBook = new Connection();
		DungeonRank = new Connection();
		ChannelingKeyPool = new Connection();
		PromotionRank = new Connection();
		Mailbox = new Connection();
		Farm = new Connection();
		BidIdPool = new Connection();
		Bid = new Connection();
		Event = new Connection();
		WorldMeta = new Connection();
		Wine = new Connection();
		CountryReport = new Connection();
		LoginOutReport = new Connection();
		Husky = new Connection();
		PrivateFarm = new Connection();
		PrivateFarmRecommend = new Connection();
		FacilityIdPool = new Connection();
		ScrapBook = new Connection();
		Commerce = new Connection();
		CommerceSystem = new Connection();
		CommerceCriminal = new Connection();
		Recommend = new Connection();
		GoldLog = new Connection();
		LinkedApCharacter = new Connection();
		EquipmentCollection = new Connection();
		Soulmate = new Connection();
		SetInfo = new Connection();
		SnapshotData = new Connection();
		PersonalRanking = new Connection();
		DefaultConnection = new Connection();
		MabiNovel = new Connection();
		MabiNovelBoard = new Connection();
		HelpPointRank = new Connection();
		InviteEvent = new Connection();
	}
}
