using Mabinogi;
using System.Reflection;

namespace XMLDB3
{
	public class HelpPointRankDecreasePointCommand : BasicCommand
	{
		private ulong charId_;

		private int DecreaseHelpPoint_;

		public HelpPointRankDecreasePointCommand()
			: base(NETWORKMSG.NET_DB_HELP_POINT_DECREASE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			charId_ = _message.ReadU64();
			DecreaseHelpPoint_ = _message.ReadS32();
		}

		public override bool DoProcess()
		{
			MethodBase currentMethod = MethodBase.GetCurrentMethod();
			string str = currentMethod.ReflectedType.Name + "." + currentMethod.Name + "()";
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
