using Mabinogi;

namespace XMLDB3
{
	public class MemoSendCommand : BasicCommand
	{
		private Memo m_Memo;

		private bool m_Result;

		public MemoSendCommand()
			: base(NETWORKMSG.NET_DB_MEMO_SEND)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Memo = MemoSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("MemoSendCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("MemoSendCommand.DoProcess() : 쪽지를 보냅니다");
			m_Result = QueryManager.Memo.SendMemo(m_Memo);
			if (m_Result)
			{
				WorkSession.WriteStatus("MemoSendCommand.DoProcess() : 쪽지를 보냈습니다.");
			}
			else
			{
				WorkSession.WriteStatus("MemoSendCommand.DoProcess() : 쪽지를 보내는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MemoSendCommand.MakeMessage() : 함수에 진입하였습니다");
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
