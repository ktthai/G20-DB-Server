using Mabinogi;

namespace XMLDB3
{
	public class RuinUpdateCommand : BasicCommand
	{
		private RuinList m_RuinList;

		private bool m_Result;

		public RuinUpdateCommand()
			: base(NETWORKMSG.NET_DB_RUIN_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_RuinList = RuinListSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("RuinUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Ruin.Write(m_RuinList, RuinType.rtRuin);
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("RuinUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
