using Mabinogi;

namespace XMLDB3
{
	public class GuildBattleGroundWinnerTypeUpdateCommand : BasicCommand
	{
		private long m_Id;

		private byte m_BattleGroundWinnerType;

		private bool m_Result;

		public GuildBattleGroundWinnerTypeUpdateCommand()
			: base(NETWORKMSG.NET_DB_GUILD_UPDATE_BATTLEGROUND_WINNER_TYPE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Id = (long)_message.ReadU64();
			m_BattleGroundWinnerType = _message.ReadU8();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildBattleGroundWinnerTypeUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.UpdateBattleGroundWinnerType(m_Id, m_BattleGroundWinnerType);
			if (m_Result)
			{
				WorkSession.WriteStatus("GuildBattleGroundWinnerTypeUpdateCommand.DoProcess() : [" + m_Id + "] 길드의 길드전 순위 [" + m_BattleGroundWinnerType + "] 로 변경했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("GuildBattleGroundWinnerTypeUpdateCommand.DoProcess() : [" + m_Id + "] 길드의 길드전 순위 [" + m_BattleGroundWinnerType + "] 로 변경 실패했습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildBattleGroundWinnerTypeUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
