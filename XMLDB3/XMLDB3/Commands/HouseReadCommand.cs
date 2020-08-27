using Mabinogi;

namespace XMLDB3
{
	public class HouseReadCommand : BasicCommand
	{
		private long m_HouseID;

		private bool m_Result;

		private House m_House;

		public HouseReadCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_READ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_HouseID = _Msg.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("HouseListReadCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseListReadCommand.DoProcess() : 집을 읽습니다.");
			m_Result = QueryManager.House.Read(m_HouseID, out m_House);
			if (m_Result)
			{
				WorkSession.WriteStatus("HouseListReadCommand.DoProcess() : 집을 얻어왔습니다");
			}
			else
			{
				WorkSession.WriteStatus("HouseListReadCommand.DoProcess() : 집을 얻는데 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseListReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				HouseSerializer.Deserialize(m_House, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
