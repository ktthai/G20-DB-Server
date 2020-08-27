using Mabinogi;

namespace XMLDB3
{
	public class PropDeleteCommand : BasicCommand
	{
		private long m_Id;

		private bool m_Result;

		public PropDeleteCommand()
			: base(NETWORKMSG.NET_DB_PROP_DELETE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Id = (long)_Msg.ReadU64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PropDeleteCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Prop.Delete(m_Id);
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PropDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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
