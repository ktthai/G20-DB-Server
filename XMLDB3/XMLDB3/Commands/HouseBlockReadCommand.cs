using Mabinogi;

namespace XMLDB3
{
	public class HouseBlockReadCommand : BasicCommand
	{
		private long m_HouseID;

		private HouseBlockList m_HouseBlockList;

		public HouseBlockReadCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_BLOCK_READ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_HouseID = _Msg.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("HouseBlockReadCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseBlockReadCommand.DoProcess() : 집 집 출입 제한 리스트를 읽습니다.");
			m_HouseBlockList = QueryManager.House.ReadBlock(m_HouseID);
			if (m_HouseBlockList != null)
			{
				WorkSession.WriteStatus("HouseBlockReadCommand.DoProcess() : 집 집 출입 제한 리스트를 읽었습니다.");
			}
			else
			{
				WorkSession.WriteStatus("HouseBlockReadCommand.DoProcess() : 집 집 출입 제한 리스트를 읽는데 실패하였습니다.");
			}
			return m_HouseBlockList != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseBlockReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_HouseBlockList != null)
			{
				message.WriteU8(1);
				HouseBlockSerializer.Deserialize(m_HouseBlockList.Blocks, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
