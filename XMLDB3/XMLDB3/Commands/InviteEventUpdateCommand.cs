using Mabinogi;
using System.Reflection;

namespace XMLDB3
{
	public class InviteEventUpdateCommand : BasicCommand
	{
		private InviteEvent InviteEventObj_;

		public InviteEventUpdateCommand()
			: base(NETWORKMSG.NET_DB_UPDATE_ENTRY_COUPON)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			InviteEventObj_ = InviteEventSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			MethodBase currentMethod = MethodBase.GetCurrentMethod();
			string str = currentMethod.ReflectedType.Name + "." + currentMethod.Name + "()";
			QueryManager.InviteEvent.UpdateInviteEvent(InviteEventObj_);
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
