using Mabinogi;

namespace XMLDB3
{
	public class PawnCoinModifyCommand : BasicCommand
	{
		private string m_Account;

		private int m_ModifyAmount;

		private bool m_Result;

		private int m_ResultPawnCoin;

		public PawnCoinModifyCommand()
			: base(NETWORKMSG.NET_DB_PAWN_COIN_MODIFY)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_ModifyAmount = _Msg.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PawnCoinModifyCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PawnCoinModifyCommand.DoProcess() : [" + m_ModifyAmount + "] 변경 요청 합니다.");
			m_Result = QueryManager.Accountref.ModifyPawnCoin(m_Account, m_ModifyAmount, ref m_ResultPawnCoin);
			if (m_Result)
			{
				WorkSession.WriteStatus("PawnCoinModifyCommand.DoProcess() [" + m_ResultPawnCoin + "] 로 변경 되었습니다.");
				return true;
			}
			WorkSession.WriteStatus("PawnCoinModifyCommand.DoProcess() : 변경에 실패 하였습니다.");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PawnCoinModifyCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				message.WriteS32(m_ResultPawnCoin);
			}
			else
			{
				message.WriteU8(0);
				message.WriteS32(m_ResultPawnCoin);
			}
			return message;
		}
	}
}
