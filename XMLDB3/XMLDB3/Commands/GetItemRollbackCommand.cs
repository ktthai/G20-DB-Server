using Mabinogi;
using System;
using XMLDB3.ItemMarket;

namespace XMLDB3
{
	public class GetItemRollbackCommand : BasicCommand
	{
		private bool m_bReplyEnable = true;

		private string m_CharacterName = string.Empty;

		private long m_TradeID;

		private int m_ItemLocation;

		public override bool ReplyEnable => m_bReplyEnable;

		public GetItemRollbackCommand()
			: base(NETWORKMSG.NET_DB_ITEMMARKET_GETITEM_ROLLBACK)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_CharacterName = _message.ReadString();
			m_TradeID = _message.ReadS64();
			m_ItemLocation = _message.ReadS32();
		}

		public override bool DoProcess()
		{
			try
			{
				ItemMarketCommand command = new IMGetItemRollbackCommand(ConfigManager.ItemMarketServerNo, m_CharacterName, m_TradeID, m_ItemLocation);
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
			WorkSession.WriteStatus("GetItemRollbackCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8(0);
			return message;
		}
	}
}
