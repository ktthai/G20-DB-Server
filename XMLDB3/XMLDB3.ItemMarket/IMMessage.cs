namespace XMLDB3.ItemMarket
{
	public enum IMMessage : byte
	{
		None = 0,
		Initialize = 1,
		Heartbeat = byte.MaxValue,
		CheckEnterance = 17,
		CheckBalance = 18,
		InquirySaleItem = 33,
		InquiryStorage = 34,
		InquiryMyPage = 36,
		ItemList = 49,
		ItemSearch = 50,
		SaleRequest = 65,
		SaleRequestCommit = 66,
		SaleRequestRollback = 69,
		SaleCancel = 67,
		Purchase = 68,
		GetItem = 129,
		GetItemCommit = 130,
		GetItemRollback = 131,
		AdministratorAccountChange = 164
	}
}
