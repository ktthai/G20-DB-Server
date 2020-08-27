using Mabinogi;
using System;
using XMLDB3.ItemMarket;

namespace XMLDB3
{
	public class CheckEntranceCommand : BasicCommand
	{
		private bool m_bReplyEnable = true;

		private string m_NexonID = string.Empty;

		private string m_CharacterName = string.Empty;

		public override bool ReplyEnable => m_bReplyEnable;

		public CheckEntranceCommand()
			: base(NETWORKMSG.NET_DB_ITEMMARKET_CHECKENTRANCE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_NexonID = _message.ReadString();
			m_CharacterName = _message.ReadString();
		}

		public override bool DoProcess()
		{
			try
			{
				ItemMarketCommand command = new IMCheckEnteranceCommand(ConfigManager.ItemMarketGameNo, ConfigManager.ItemMarketServerNo, m_NexonID, m_CharacterName, m_CharacterName);
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
			WorkSession.WriteStatus("CheckEntranceCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8(0);
			return message;
		}
	}
}
