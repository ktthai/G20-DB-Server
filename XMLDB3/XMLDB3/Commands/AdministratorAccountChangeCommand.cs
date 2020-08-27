using Mabinogi;
using System;
using System.Collections;
using XMLDB3.ItemMarket;

namespace XMLDB3
{
	public class AdministratorAccountChangeCommand : BasicCommand
	{
		private bool m_bReplyEnable = true;

		private string m_NewNexonID = string.Empty;

		private ArrayList m_Accounts = new ArrayList();

		public override bool ReplyEnable => m_bReplyEnable;

		public AdministratorAccountChangeCommand()
			: base(NETWORKMSG.NET_DB_ITEMMARKET_ADMINISTRATOR_ACCOUNT_CHANGE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_NewNexonID = _message.ReadString();
			uint num = _message.ReadU32();
			for (int i = 0; i < num; i++)
			{
				m_Accounts.Add(_message.ReadString());
			}
		}

		public override bool DoProcess()
		{
			try
			{
				ItemMarketCommand command = new IMAdministratorAccountChange(ConfigManager.ItemMarketGameNo, ConfigManager.ItemMarketServerNo, m_NewNexonID, m_Accounts);
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
			WorkSession.WriteStatus("AdministratorAccountChangeCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8(0);
			return message;
		}
	}
}
