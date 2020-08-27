using Mabinogi;

namespace XMLDB3
{
	public class PropReadCommand : BasicCommand
	{
		private Prop m_Prop;

		private long m_Id;

		public PropReadCommand()
			: base(NETWORKMSG.NET_DB_PROP_READ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = (long)_Msg.ReadU64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PropReadCommand.DoProcess() : 함수에 진입하였습니다");
			m_Prop = QueryManager.Prop.Read(m_Id);
			if (m_Prop == null)
			{
				return false;
			}
			return true;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PropReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Prop != null)
			{
				message.WriteU8(1);
				message = PropSerializer.Deserialize(m_Prop, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
