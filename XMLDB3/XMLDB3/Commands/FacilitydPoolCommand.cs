using Mabinogi;

namespace XMLDB3
{
	public class FacilitydPoolCommand : BasicCommand
	{
		private long m_IdOffset;

		public FacilitydPoolCommand()
			: base(NETWORKMSG.NET_DB_PRIVATEFARM_FACILITY_ID_POOL)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("FacilityIdPoolCommand.DoProcess() : 함수에 진입하였습니다");
			m_IdOffset = QueryManager.FacilityIdPool.GetIdPool();
			return true;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("FacilityIdPoolCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU64((ulong)m_IdOffset);
			return message;
		}
	}
}
