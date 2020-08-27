using Mabinogi;

namespace XMLDB3
{
	public class WineUpdateCommand : BasicCommand
	{
		private Wine m_Wine;

		private bool m_Result;

		public WineUpdateCommand()
			: base(NETWORKMSG.NET_DB_WINE_AGING_UPDATE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Wine = WineSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("WineUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Wine.Update(m_Wine);
			if (m_Result)
			{
				WorkSession.WriteStatus("WineUpdateCommand.DoProcess() : 와인 데이터를 업데이트하는데 성공했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("WineUpdateCommand.DoProcess() : 와인 데이터를 업데이트하는데 실패했습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("WineUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
