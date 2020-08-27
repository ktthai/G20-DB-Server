using Mabinogi;

namespace XMLDB3
{
	public class BankQueryPasswordCommand : SerializedCommand
	{
		private Bank m_Bank;

		private string m_Account = string.Empty;

		private string m_CharName = string.Empty;

		public BankQueryPasswordCommand()
			: base(NETWORKMSG.NET_DB_QUERY_BANK_PASSWORD)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_CharName = _Msg.ReadString();
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.StringIDRegistant(m_Account);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("BankQueryPasswordCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("BankQueryPasswordCommand.DoProcess() : [" + m_Account + "] 를 캐쉬에서 읽도록 시도합니다.");
			BankCache bankCache = (BankCache)ObjectCache.Bank.Extract(m_Account);
			if (bankCache == null)
			{
				bankCache = new BankCache();
			}
			m_Bank = QueryManager.Bank.Read(m_Account, m_CharName, BankRace.None, bankCache);
			if (m_Bank != null)
			{
				WorkSession.WriteStatus("BankQueryPasswordCommand.DoProcess() : [" + m_Account + "] 의 정보를 데이터베이스에서 읽었습니다");
				ObjectCache.Bank.Push(m_Account, bankCache);
			}
			else
			{
				WorkSession.WriteStatus("BankQueryPasswordCommand.DoProcess() : [" + m_Account + "] 의 정보를 데이터베이스에 쿼리하는데 실패하였습니다");
			}
			return m_Bank != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("BankQueryPasswordCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Bank != null)
			{
				message.WriteString(m_Bank.data.password);
			}
			else
			{
				string data = "";
				message.WriteString(data);
			}
			return message;
		}
	}
}
