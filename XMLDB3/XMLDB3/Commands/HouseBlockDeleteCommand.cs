using Mabinogi;

namespace XMLDB3
{
	public class HouseBlockDeleteCommand : BasicCommand
	{
		private long m_HouseID;

		private bool m_Result;

		public HouseBlockDeleteCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_BLOCK_DELETE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_HouseID = _Msg.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("HouseBlockDeleteCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseBlockDeleteCommand.DoProcess() : 집 집 출입 제한 리스트를 삭제합니다.");
			m_Result = QueryManager.House.DeleteBlock(m_HouseID);
			if (m_Result)
			{
				WorkSession.WriteStatus("HouseBlockDeleteCommand.DoProcess() : 집 집 출입 제한 리스트를 삭제하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("HouseBlockDeleteCommand.DoProcess() : 집 집 출입 제한 리스트를 삭제하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseBlockDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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
