using Mabinogi;

namespace XMLDB3
{
	public class CastleListReadCommand : BasicCommand
	{
		private CastleList m_CastleList;

		public CastleListReadCommand()
			: base(NETWORKMSG.NET_DB_CASTLE_LIST_READ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CastleListReadCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CastleListReadCommand.DoProcess() : 성 전체 리스트를 얻어옵니다");
			m_CastleList = QueryManager.Castle.ReadList();
			if (m_CastleList != null)
			{
				WorkSession.WriteStatus("CastleListReadCommand.DoProcess() : 성 전체 리스트를 얻어왔습니다");
				return true;
			}
			WorkSession.WriteStatus("CastleListReadCommand.DoProcess() : 성 전체 리스트를 얻는데 실패하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CastleListReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_CastleList != null)
			{
				message.WriteU8(1);
				CastleListSerializer.Deserialize(m_CastleList, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
