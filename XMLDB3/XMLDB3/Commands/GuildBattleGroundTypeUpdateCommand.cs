using Mabinogi;

namespace XMLDB3
{
	public class GuildBattleGroundTypeUpdateCommand : BasicCommand
	{
		private long m_Id;

		private int m_GuildPoint;

		private int m_GuildMoney;

		private byte m_BattleGroundType;

		private int m_Result = -1;

		public GuildBattleGroundTypeUpdateCommand()
			: base(NETWORKMSG.NET_DB_GUILD_UPDATE_BATTLEGROUND_TYPE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Id = (long)_message.ReadU64();
			m_GuildPoint = _message.ReadS32();
			m_GuildMoney = _message.ReadS32();
			m_BattleGroundType = _message.ReadU8();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("GuildBattleGroundTypeUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Guild.UpdateBattleGroundType(m_Id, m_GuildPoint, m_GuildMoney, m_BattleGroundType);
			if (m_Result == 0)
			{
				WorkSession.WriteStatus("GuildBattleGroundTypeUpdateCommand.DoProcess() : [" + m_Id + "] 길드를 길드전 [" + m_BattleGroundType + "] 타입으로 설정하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("GuildBattleGroundTypeUpdateCommand.DoProcess() : [" + m_Id + "] 길드를 길드전 [" + m_BattleGroundType + "] 타입 설정을 실패하였습니다");
			}
			return m_Result == 0;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("GuildBattleGroundTypeUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result == 0)
			{
				message.WriteU8(1);
			}
			else
			{
				message.WriteU8(0);
				message.WriteS32(m_Result);
			}
			return message;
		}
	}
}
