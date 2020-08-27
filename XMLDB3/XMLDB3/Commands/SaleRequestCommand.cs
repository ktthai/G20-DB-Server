using Mabinogi;
using System;
using XMLDB3.ItemMarket;

namespace XMLDB3
{
	public class SaleRequestCommand : BasicCommand
	{
		private bool m_bReplyEnable = true;

		private string m_CharacterName = string.Empty;

		private Item m_Item;

		private string m_ItemName = string.Empty;

		private int m_Price;

		private int m_ItemFee;

		private int m_ItemRegistFee;

		private byte m_SalePeriod;

		public override bool ReplyEnable => m_bReplyEnable;

		public SaleRequestCommand()
			: base(NETWORKMSG.NET_DB_ITEMMARKET_SALE_REQUEST)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_CharacterName = _message.ReadString();
			m_Item = ItemSerializer.Serialize(_message);
			m_ItemName = _message.ReadString();
			m_Price = _message.ReadS32();
			m_ItemFee = _message.ReadS32();
			m_ItemRegistFee = _message.ReadS32();
			m_SalePeriod = _message.ReadU8();
		}

		public override bool DoProcess()
		{
			try
			{
				ItemMarketCommand command = new IMSaleRequestCommand(ConfigManager.ItemMarketServerNo, m_CharacterName, m_Item, m_ItemName, m_Price, m_ItemFee, m_ItemRegistFee, m_SalePeriod);
				ItemMarketHandler handler = ItemMarketManager.GetHandler();
				if (handler != null && handler.Send(command, base.ID, base.QueryID, 0u, base.Target))
				{
					m_bReplyEnable = false;
				}
			}
			catch (Exception ex)
			{
				ExceptionMonitor.ExceptionRaised(ex);
				m_bReplyEnable = true;
			}
			return true;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("SaleRequestCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8(0);
			return message;
		}
	}
}
