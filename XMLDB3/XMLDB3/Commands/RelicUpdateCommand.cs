using Mabinogi;

namespace XMLDB3
{
	public class RelicUpdateCommand : BasicCommand
	{
		private RuinList m_RuinList;

		private bool m_Result;

		public RelicUpdateCommand()
			: base(NETWORKMSG.NET_DB_RELIC_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_RuinList = RuinListSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("RelicUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Ruin.Write(m_RuinList, RuinType.rtRelic);
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("RelicUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
