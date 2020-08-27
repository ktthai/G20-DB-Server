using Mabinogi;

namespace XMLDB3
{
	public class WineReadCommand : BasicCommand
	{
		private long m_CharID;

		private Wine m_Wine;

		private REPLY_RESULT m_Result;

		public WineReadCommand()
			: base(NETWORKMSG.NET_DB_WINE_AGING_READ)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_CharID = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("WineReadCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Wine.Read(m_CharID, out m_Wine);
			if (m_Result != 0)
			{
				WorkSession.WriteStatus("WineReadCommand.DoProcess() : 와인 데이터를 읽는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("WineReadCommand.DoProcess() : 와인 데이터를 읽는데 실패했습니다.");
			}
			return m_Result != REPLY_RESULT.FAIL;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("WineReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WineSerializer.Deserialize(m_Wine, message);
			}
			return message;
		}
	}
}
