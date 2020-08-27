using Mabinogi;

namespace XMLDB3
{
	public class WineDeleteCommand : BasicCommand
	{
		private bool m_Result;

		private long m_CharID;

		public WineDeleteCommand()
			: base(NETWORKMSG.NET_DB_WINE_AGING_REMOVE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_CharID = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("WineDeleteCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Wine.Delete(m_CharID);
			if (m_Result)
			{
				WorkSession.WriteStatus("WineDeleteCommand.DoProcess() : 와인 데이터를 성공적으로 삭제했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("WineDeleteCommand.DoProcess() : 와인 데이터를 삭제하는데 실패했습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("WineDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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
