using Mabinogi;

namespace XMLDB3
{
	public class DungeonRankUpdateCommand : BasicCommand
	{
		private bool m_Result;

		private DungeonRank m_dungeonRank;

		public DungeonRankUpdateCommand()
			: base(NETWORKMSG.NET_DB_DUNGEON_RANKING_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_dungeonRank = DungeonRankSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("DungeonRankUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("DungeonRankUpdateCommand.DoProcess() : 던전 랭킹을 기록합니다");
			m_Result = QueryManager.DungeonRank.Update(m_dungeonRank);
			if (m_Result)
			{
				WorkSession.WriteStatus("DungeonRankUpdateCommand.DoProcess() : 던전 랭킹을 기록하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("DungeonRankUpdateCommand.DoProcess() : 던전 랭킹 기록에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("DungeonRankUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
