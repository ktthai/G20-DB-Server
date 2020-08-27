using Mabinogi;

namespace XMLDB3
{
	public class ChronicleInfoInitCommand : BasicCommand
	{
		private ChronicleInfoList m_InfoList;

		private bool m_Result;

		public ChronicleInfoInitCommand()
			: base(NETWORKMSG.NET_DB_CHRONICLE_INFO_LIST_INIT)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_InfoList = ChronicleInfoListSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ChronicleInfoInitCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("ChronicleInfoInitCommand.DoProcess() : 탐사연표 이미지를 초기화합니다.");
			m_Result = QueryManager.Chronicle.UpdateChronicleInfoList(m_InfoList);
			if (m_Result)
			{
				WorkSession.WriteStatus("ChronicleInfoInitCommand.DoProcess() :탐사연표 이미지를 초기화하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("ChronicleInfoInitCommand.DoProcess() : 탐사연표 이미지 초기화에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ChronicleInfoInitCommand.MakeMessage() : 함수에 진입하였습니다");
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
