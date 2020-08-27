using Mabinogi;
using System;
using System.Reflection;

namespace XMLDB3
{
	public class PersonalRankingRead2Command : BasicCommand
	{
		private PersonalRankingTable rankingTable_;

		private PersonalRanking.EId rankingId_;

		private PersonalRanking.EScoreSortType sortType_;

		private DateTime rankingCycleStartTime_ = DateTime.MaxValue;

		private DateTime rankingCycleEndTime_ = DateTime.MaxValue;

		private DateTime currentTime_ = DateTime.Now;

		public PersonalRankingRead2Command()
			: base(NETWORKMSG.NET_DB_PERSONAL_RANKING_READ2)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			rankingId_ = (PersonalRanking.EId)_message.ReadU32();
			sortType_ = (PersonalRanking.EScoreSortType)_message.ReadU32();
			rankingCycleStartTime_ = new DateTime((long)(_message.ReadU64() * 10000));
			rankingCycleEndTime_ = new DateTime((long)(_message.ReadU64() * 10000));
			currentTime_ = new DateTime((long)(_message.ReadU64() * 10000));
		}

		public override bool DoProcess()
		{
			MethodBase currentMethod = MethodBase.GetCurrentMethod();
			string str = currentMethod.ReflectedType.Name + "." + currentMethod.Name + "()";
			WorkSession.WriteStatus(str + " : 함수에 진입하였습니다");
			rankingTable_ = PersonalRankingTable.GetTable(QueryManager.PersonalRanking, rankingId_, sortType_, rankingCycleStartTime_, rankingCycleEndTime_, currentTime_);
			if (rankingTable_ != null)
			{
				WorkSession.WriteStatus(str + " : PersonalRanking 데이터를 성공적으로 읽었습니다");
			}
			else
			{
				WorkSession.WriteStatus(str + " : PersonalRanking 데이터를 읽는데 실패하였습니다.");
			}
			return rankingTable_ != null;
		}

		public override Message MakeMessage()
		{
			string name = MethodBase.GetCurrentMethod().Name;
			WorkSession.WriteStatus(name + " : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (rankingTable_ != null)
			{
				message.WriteU8(1);
				PersonalRankingTableSerializer.Deserialize(rankingTable_, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
