using Mabinogi;

namespace XMLDB3
{
	public class PropUpdateCommand : BasicCommand
	{
		private Prop m_Prop;

		private bool m_Result;

		public PropUpdateCommand()
			: base(NETWORKMSG.NET_DB_PROP_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Prop = PropSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PropUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Prop.Write(m_Prop);
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PropUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
