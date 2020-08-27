using Mabinogi;
using System.Reflection;

namespace XMLDB3
{
	public class HelpPointRankUpdateCommand : BasicCommand
	{
		private HelpPointRankList RankList_;

		public HelpPointRankUpdateCommand()
			: base(NETWORKMSG.NET_DB_HELP_POINT_UPDATE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			RankList_ = HelpPointRankListSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			MethodBase currentMethod = MethodBase.GetCurrentMethod();
			string str = currentMethod.ReflectedType.Name + "." + currentMethod.Name + "()";
			QueryManager.HelpPointRank.UpdateHelpPoint(RankList_);
			WorkSession.WriteStatus(str + " : 함수에 진입하였습니다");
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
