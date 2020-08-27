using Mabinogi;
using System.Reflection;

namespace XMLDB3
{
	public class HelpPointRankListLoadCommand : BasicCommand
	{
		private HelpPointRankList RankList_;

		public HelpPointRankListLoadCommand()
			: base(NETWORKMSG.NET_DB_HELP_POINT_LIST_LOAD)
		{
		}

		protected override void ReceiveData(Message _message)
		{
		}

		public override bool DoProcess()
		{
			MethodBase currentMethod = MethodBase.GetCurrentMethod();
			string str = currentMethod.ReflectedType.Name + "." + currentMethod.Name + "()";
			RankList_ = QueryManager.HelpPointRank.ReadList();
			WorkSession.WriteStatus(str + " : 함수에 진입하였습니다");
			return true;
		}

		public override Message MakeMessage()
		{
			string name = MethodBase.GetCurrentMethod().Name;
			WorkSession.WriteStatus(name + " : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (RankList_ != null)
			{
				message.WriteU8(1);
				HelpPointRankListSerializer.Deserialize(RankList_, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
