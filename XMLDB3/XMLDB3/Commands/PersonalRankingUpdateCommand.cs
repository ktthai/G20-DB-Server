using Mabinogi;
using System;
using System.Reflection;

namespace XMLDB3
{
	public class PersonalRankingUpdateCommand : BasicCommand
	{
		private PersonalRanking.EId rankingId_;

		private PersonalRanking.EScoreSortType sortType_;

		private DateTime rankingCycleStartTime_ = DateTime.MinValue;

		private DateTime currentTime_ = DateTime.Now;

		private ulong charId_;

		private int score_;

		private REPLY_RESULT result_;

		private byte errorCode_;

		public PersonalRankingUpdateCommand()
			: base(NETWORKMSG.NET_DB_PERSONAL_RANKING_UPDATE_SOCRE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			rankingId_ = (PersonalRanking.EId)_message.ReadU32();
			sortType_ = (PersonalRanking.EScoreSortType)_message.ReadU32();
			rankingCycleStartTime_ = new DateTime((long)(_message.ReadU64() * 10000));
			currentTime_ = new DateTime((long)(_message.ReadU64() * 10000));
			charId_ = _message.ReadU64();
			score_ = _message.ReadS32();
		}

		public override bool DoProcess()
		{
			MethodBase currentMethod = MethodBase.GetCurrentMethod();
			string str = currentMethod.ReflectedType.Name + "." + currentMethod.Name + "()";
			WorkSession.WriteStatus(str + " : 함수에 진입하였습니다");
			result_ = QueryManager.PersonalRanking.UpdateScore(rankingId_, sortType_, rankingCycleStartTime_, currentTime_, charId_, score_, ref errorCode_);
			if (result_ == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus(str + " : 랭킹 데이터를 성공적으로 추가했습니다.");
			}
			else if (errorCode_ == 200)
			{
				WorkSession.WriteStatus(str + " : 이전 기록이 더 좋아서 업데이트를 하지 않습니다.");
				result_ = REPLY_RESULT.SUCCESS;
			}
			else
			{
				WorkSession.WriteStatus(str + " : 랭킹 데이터를 추가하는데 실패하였습니다.");
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
			if (result_ == REPLY_RESULT.FAIL_EX)
			{
				message.WriteU8(errorCode_);
			}
			return message;
		}
	}
}
