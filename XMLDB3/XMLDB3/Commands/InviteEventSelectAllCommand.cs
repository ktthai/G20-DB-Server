using Mabinogi;
using System.Reflection;

namespace XMLDB3
{
	public class InviteEventSelectAllCommand : BasicCommand
	{
		private InviteEventList InviteEventList_;

		public InviteEventSelectAllCommand()
			: base(NETWORKMSG.NET_DB_LOAD_ENTRY_COUPON_LIST)
		{
		}

		protected override void ReceiveData(Message _message)
		{
		}

		public override bool DoProcess()
		{
			MethodBase currentMethod = MethodBase.GetCurrentMethod();
			string str = currentMethod.ReflectedType.Name + "." + currentMethod.Name + "()";
			InviteEventList_ = QueryManager.InviteEvent.SelectAllInviteEventList();
			WorkSession.WriteStatus(str + " : 함수에 진입하였습니다");
			return true;
		}

		public override Message MakeMessage()
		{
			string name = MethodBase.GetCurrentMethod().Name;
			WorkSession.WriteStatus(name + " : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (InviteEventList_ != null)
			{
				message.WriteU8(1);
				InviteEventListSerializer.Deserialize(InviteEventList_, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
