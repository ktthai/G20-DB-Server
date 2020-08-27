using Mabinogi;
using System.Reflection;

namespace XMLDB3
{
	public class PersonalRankingClearCommand : BasicCommand
	{
		private PersonalRanking.EId rankingId_;

		private REPLY_RESULT result_;

		public PersonalRankingClearCommand()
			: base(NETWORKMSG.NET_DB_PERSONAL_RANKING_CLEAR)
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
			result_ = QueryManager.PersonalRanking.Clear(rankingId_);
			if (result_ == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus(str + " : 랭킹 기록을 성공적으로 지웠습니다.");
			}
			else
			{
				WorkSession.WriteStatus(str + " : 랭킹 기록을 지우는데 실패하였습니다.");
			}
			return result_ == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			MethodBase currentMethod = MethodBase.GetCurrentMethod();
			string str = currentMethod.ReflectedType.Name + "." + currentMethod.Name + "()";
			WorkSession.WriteStatus(str + " : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)result_);
			return message;
		}
	}
}
