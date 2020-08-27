using Mabinogi;

namespace XMLDB3
{
	public class HuskyCallProcedureCommand : BasicCommand
	{
		private bool m_bResult;

		private long m_charId;

		private string m_accountName;

		private string m_charName;

		public override bool IsPrimeCommand => true;

		public HuskyCallProcedureCommand()
			: base(NETWORKMSG.NET_DB_HUSKY_CALLPROCEDURE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_accountName = _message.ReadString();
			m_charId = _message.ReadS64();
			m_charName = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("HuskyCallProcedureCommand.DoProcess() : 함수에 진입하였습니다");
			HuskyAdapter huskyEvent = QueryManager.HuskyEvent;
			if (huskyEvent != null)
			{
				m_bResult = huskyEvent.Callprocedure(m_accountName, m_charId, m_charName);
			}
			else
			{
				m_bResult = false;
			}
			if (m_bResult)
			{
				WorkSession.WriteStatus("HuskyCallProcedureCommand.DoProcess() : 허스키 프로시져를 성공적으로 실행했습니다");
			}
			else
			{
				WorkSession.WriteStatus("HuskyCallProcedureCommand.DoProcess() : 허스키 프로시져를 실행하는데 실패하였습니다.");
			}
			return m_bResult;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HuskyCallProcedureCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_bResult)
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
