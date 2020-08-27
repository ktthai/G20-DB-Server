using Mabinogi;

namespace XMLDB3
{
	public class QuerySimpleAccountCommand : BasicCommand
	{
		private string m_strAccount;

		private bool m_Result;

		public QuerySimpleAccountCommand()
			: base(NETWORKMSG.NET_DB_QUERY_SIMPLE_ACCOUNT)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_strAccount = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("QuerySimpleAccountCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("QuerySimpleAccountCommand.DoProcess() : [" + m_strAccount + "] 계정 정보 읽기를 쿼리합니다");
			m_Result = QueryManager.Account.QuerySimpleAccount(m_strAccount);
			return true;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("QuerySimpleAccountCommand.MakeMessage() : 함수에 진입하였습니다");
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
