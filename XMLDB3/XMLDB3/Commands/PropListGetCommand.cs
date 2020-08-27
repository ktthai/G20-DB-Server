using Mabinogi;

namespace XMLDB3
{
	public class PropListGetCommand : BasicCommand
	{
		private PropIDList m_PropList;

		public PropListGetCommand()
			: base(NETWORKMSG.NET_DB_PROPLIST_READ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PropListGetCommand.DoProcess() : 함수에 진입하였습니다");
			m_PropList = QueryManager.Prop.LoadPropList();
			if (m_PropList == null)
			{
				return false;
			}
			return true;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PropListGetCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_PropList != null)
			{
				message.WriteU8(1);
				PropListSerializer.Deserialize(m_PropList, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
