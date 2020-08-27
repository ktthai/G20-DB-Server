using Mabinogi;

namespace XMLDB3
{
	public class ChronicleCreateCommand : BasicCommand
	{
		private string m_CharacterName = string.Empty;

		private Chronicle m_Chronicle;

		private bool m_Result;

		public ChronicleCreateCommand()
			: base(NETWORKMSG.NET_DB_CHRONICLE_INSERT_LOG)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Chronicle = ChronicleSerializer.Serialize(_Msg);
			m_CharacterName = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ChronicleCreateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("ChronicleCreateCommand.DoProcess() : 탐사연표를 생성합니다");
			m_Result = QueryManager.Chronicle.Create(m_CharacterName, m_Chronicle);
			if (m_Result)
			{
				WorkSession.WriteStatus("ChronicleCreateCommand.DoProcess() :탐사연표를 생성하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("ChronicleCreateCommand.DoProcess() : 탐사연표 생성에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ChronicleCreateCommand.MakeMessage() : 함수에 진입하였습니다");
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
