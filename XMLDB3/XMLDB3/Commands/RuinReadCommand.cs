using Mabinogi;

namespace XMLDB3
{
	public class RuinReadCommand : BasicCommand
	{
		private RuinList m_RuinList;

		public RuinReadCommand()
			: base(NETWORKMSG.NET_DB_RUIN_READ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("RuinReadCommand.DoProcess() : 함수에 진입하였습니다");
			m_RuinList = QueryManager.Ruin.Read(RuinType.rtRuin);
			if (m_RuinList == null)
			{
				return false;
			}
			return true;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("RuinReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_RuinList != null)
			{
				message.WriteU8(1);
				message = RuinListSerializer.Deserialize(m_RuinList, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
