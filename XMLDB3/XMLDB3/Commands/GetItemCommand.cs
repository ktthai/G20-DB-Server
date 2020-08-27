using Mabinogi;
using System;
using XMLDB3.ItemMarket;

namespace XMLDB3
{
	public class GetItemCommand : BasicCommand
	{
		private bool m_bReplyEnable = true;

		private string m_CharacterName = string.Empty;

		private long m_TradeID;

		public override bool ReplyEnable => m_bReplyEnable;

		public GetItemCommand()
			: base(NETWORKMSG.NET_DB_ITEMMARKET_GETITEM)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_CharacterName = _message.ReadString();
			m_TradeID = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			try
			{
				ItemMarketCommand command = new IMGetItemCommand(ConfigManager.ItemMarketServerNo, m_CharacterName, m_TradeID);
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
			WorkSession.WriteStatus("GetItemCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8(0);
			return message;
		}
	}
}
