using Mabinogi;
using System.Reflection;

namespace XMLDB3
{
	public class PersonalRankingClearCacheCommand : BasicCommand
	{
		private PersonalRanking.EId rankingId_;

		public PersonalRankingClearCacheCommand()
			: base(NETWORKMSG.NET_DB_PERSONAL_RANKING_CLEAR_CACHE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			rankingId_ = (PersonalRanking.EId)_message.ReadU32();
		}

		public override bool DoProcess()
		{
			MethodBase currentMethod = MethodBase.GetCurrentMethod();
			string str = currentMethod.ReflectedType.Name + "." + currentMethod.Name + "()";
			WorkSession.WriteStatus(str + " : 함수에 진입하였습니다");
			PersonalRankingTable.ClearCache(rankingId_);
			WorkSession.WriteStatus(str + " : " + rankingId_.ToString() + " PersonalRanking 캐시가 제거되었습니다");
			return true;
		}

		public override Message MakeMessage()
		{
			string name = MethodBase.GetCurrentMethod().Name;
			WorkSession.WriteStatus(name + " : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8(1);
			return message;
		}
	}
}
