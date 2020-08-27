using Mabinogi;

namespace XMLDB3
{
	public class PawnCoinModifyLogCommand : BasicCommand
	{
		private bool m_Result;

		private Message m_Message;

		public PawnCoinModifyLogCommand()
			: base(NETWORKMSG.NET_DB_PAWN_COIN_LOG_INSERT)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Message = _Msg;
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PawnCoinModifyLogCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PawnCoinModifyLogCommand.DoProcess() : 저장 요청 합니다.");
			m_Result = QueryManager.Accountref.InsertPawnCoinLog(m_Message);
			if (m_Result)
			{
				WorkSession.WriteStatus("PawnCoinModifyLogCommand.DoProcess() 저장 완료 되었습니다.");
				return true;
			}
			WorkSession.WriteStatus("PawnCoinModifyLogCommand.DoProcess() : 변경에 실패 하였습니다.");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PawnCoinModifyLogCommand.MakeMessage() : 함수에 진입하였습니다");
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
