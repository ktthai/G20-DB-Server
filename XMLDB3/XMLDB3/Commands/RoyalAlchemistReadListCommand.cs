using Mabinogi;

namespace XMLDB3
{
	public class RoyalAlchemistReadListCommand : BasicCommand
	{
		private RoyalAlchemistList m_royalAlchemistList;

		public override bool IsPrimeCommand => true;

		public RoyalAlchemistReadListCommand()
			: base(NETWORKMSG.NET_DB_ROYALALCHEMIST_LIST)
		{
		}

		protected override void ReceiveData(Message _message)
		{
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("RoyalAlchemistReadListCommand.DoProcess() : 함수에 진입하였습니다");
			m_royalAlchemistList = QueryManager.RoyalAlchemist.ReadList();
			if (m_royalAlchemistList != null)
			{
				WorkSession.WriteStatus("RoyalAlchemistReadListCommand.DoProcess() : 왕성 연금술사 데이터를 성공적으로 읽었습니다");
			}
			else
			{
				WorkSession.WriteStatus("RoyalAlchemistReadListCommand.DoProcess() : 왕성 연금술사 데이터를 읽는데 실패하였습니다.");
			}
			return m_royalAlchemistList != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("RoyalAlchemistReadListCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_royalAlchemistList != null)
			{
				message.WriteU8(1);
				RoyalAlchemistListSerializer.Deserialize(m_royalAlchemistList, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
