using Mabinogi;

namespace XMLDB3
{
	public class ShopAdvertiseItemPriceUpdateCommand : BasicCommand
	{
		private string m_Account = string.Empty;

		private string m_Server = string.Empty;

		private long m_ItemID;

		private int m_Price;

		private bool m_Result;

		public ShopAdvertiseItemPriceUpdateCommand()
			: base(NETWORKMSG.NET_DB_ADVERTISE_ITEM_SET_PRICE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Account = _message.ReadString();
			m_Server = _message.ReadString();
			m_ItemID = _message.ReadS64();
			m_Price = _message.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ShopAdvertiseItemPriceUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("ShopAdvertiseItemPriceUpdateCommand.DoProcess() : 상점 광고에 아이템 가격을 업데이트합니다.");
			m_Result = QueryManager.ShopAdvertise.SetItemPrice(m_Account, m_Server, m_ItemID, m_Price);
			if (m_Result)
			{
				WorkSession.WriteStatus("ShopAdvertiseItemPriceUpdateCommand.DoProcess() : 상점 광고에 아이템 가격을 업데이트하는데 성공하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("ShopAdvertiseItemPriceUpdateCommand.DoProcess() : 점 광고에 아이템 가격을 업데이트하는데 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ShopAdvertiseItemPriceUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
