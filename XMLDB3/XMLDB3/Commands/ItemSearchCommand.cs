using Mabinogi;
using System;
using XMLDB3.ItemMarket;

namespace XMLDB3
{
	public class ItemSearchCommand : BasicCommand
	{
		private bool m_bReplyEnable = true;

		private string m_CharacterName = string.Empty;

		private int m_PageNo;

		private int m_PageItemCount;

		private string m_ItemName = string.Empty;

		private IMSortingType m_SortingType = IMSortingType.ExpireDate;

		private bool m_SortingAsc;

		private int m_ItemGroup = -1;

		public override bool ReplyEnable => m_bReplyEnable;

		public ItemSearchCommand()
			: base(NETWORKMSG.NET_DB_ITEMMARKET_ITEMSEARCH)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_CharacterName = _message.ReadString();
			m_PageNo = _message.ReadS32();
			m_PageItemCount = _message.ReadS32();
			m_ItemName = _message.ReadString();
			int sortType = _message.ReadS32();
			m_SortingType = SortTypeHelper.GetSortingType(sortType);
			m_SortingAsc = SortTypeHelper.GetAscendingType(sortType);
			int num = _message.ReadS32();
			switch (num)
			{
			case 0:
				m_ItemGroup = -1;
				break;
			case 10:
				m_ItemGroup = 0;
				break;
			default:
				m_ItemGroup = num;
				break;
			}
		}

		public override bool DoProcess()
		{
			try
			{
				ItemMarketCommand command = new IMItemSearchCommand(ConfigManager.ItemMarketServerNo, m_CharacterName, m_PageNo, m_PageItemCount, m_ItemName, m_SortingType, m_SortingAsc, m_ItemGroup);
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
			WorkSession.WriteStatus("ItemSearchCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8(0);
			return message;
		}
	}
}
