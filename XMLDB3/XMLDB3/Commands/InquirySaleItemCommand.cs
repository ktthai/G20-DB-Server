using Mabinogi;
using System;
using XMLDB3.ItemMarket;

namespace XMLDB3
{
	public class InquirySaleItemCommand : BasicCommand
	{
		private bool m_bReplyEnable = true;

		private string m_CharacterName = string.Empty;

		private int m_PageNo;

		private int m_PageItemCount;

		public override bool ReplyEnable => m_bReplyEnable;

		public InquirySaleItemCommand()
			: base(NETWORKMSG.NET_DB_ITEMMARKET_INQUIRY_SALEITEM)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_CharacterName = _message.ReadString();
			m_PageNo = _message.ReadS32();
			m_PageItemCount = _message.ReadS32();
		}

		public override bool DoProcess()
		{
			try
			{
				ItemMarketCommand command = new IMInquirySaleItemCommand(ConfigManager.ItemMarketServerNo, m_CharacterName, m_PageNo, m_PageItemCount);
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
			WorkSession.WriteStatus("InquirySaleItemCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8(0);
			return message;
		}
	}
}
