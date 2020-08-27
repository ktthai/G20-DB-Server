using Mabinogi;

namespace XMLDB3
{
	public class RoyalAlchemistReadCommand : BasicCommand
	{
		private long m_charID;

		private RoyalAlchemist m_royalAlchemist;

		public override bool IsPrimeCommand => true;

		public RoyalAlchemistReadCommand()
			: base(NETWORKMSG.NET_DB_ROYALALCHEMIST_READ)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_charID = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("RoyalAlchemistReadCommand.DoProcess() : 함수에 진입하였습니다");
			m_royalAlchemist = QueryManager.RoyalAlchemist.Read(m_charID);
			if (m_royalAlchemist != null)
			{
				WorkSession.WriteStatus("RoyalAlchemistReadCommand.DoProcess() : 왕성 연금술사 데이터를 성공적으로 읽었습니다");
			}
			else
			{
				WorkSession.WriteStatus("RoyalAlchemistReadCommand.DoProcess() : 왕성 연금술사 데이터를 읽는데 실패하였습니다.");
			}
			return m_royalAlchemist != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("RoyalAlchemistReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_royalAlchemist != null)
			{
				message.WriteU8(1);
				RoyalAlchemistSerializer.Deserialize(m_royalAlchemist, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
