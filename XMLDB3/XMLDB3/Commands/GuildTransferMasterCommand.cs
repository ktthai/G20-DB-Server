using Mabinogi;

namespace XMLDB3
{
	public class GuildTransferMasterCommand : BasicCommand
	{
		private long m_Id;

		private long m_OldMaster;

		private long m_NewMaster;

		private bool m_Result;

		public GuildTransferMasterCommand()
			: base(NETWORKMSG.NET_DB_GUILD_TRANSFER_MASTER)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = (long)_Msg.ReadU64();
			m_OldMaster = (long)_Msg.ReadU64();
			m_NewMaster = (long)_Msg.ReadU64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildTransferMasterCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.TransferGuildMaster(m_Id, m_OldMaster, m_NewMaster);
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildTransferMasterCommand.MakeMessage() : 함수에 진입하였습니다");
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
