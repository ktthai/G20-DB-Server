using Mabinogi;
using System;

namespace XMLDB3
{
	public class BankUpdateCommand : SerializedCommand
	{
		private string m_CharName = string.Empty;

		private Bank m_Bank;

		private bool m_Result;

		public BankUpdateCommand()
			: base(NETWORKMSG.NET_DB_BANK_ACCOUNT_UPDATE)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_CharName = _Msg.ReadString();
			m_Bank = BankSerializer.Serialize(_Msg);
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.StringIDRegistant(m_Bank.account);
			if (m_Bank.slot != null)
			{
				foreach (BankSlot item2 in m_Bank.slot)
				{
					if (item2.item != null)
					{
						foreach (BankItem bankItem in item2.item)
						{
							_helper.ObjectIDRegistant(bankItem.item.id);
						}
					}
				}
			}
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("BankUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("BankUpdateCommand.DoProcess() : [" + m_Bank.account + "] 의 데이터를 캐쉬에서 읽습니다");
			BankCache bankCache = (BankCache)ObjectCache.Bank.Extract(m_Bank.account);
			if (bankCache == null)
			{
				bankCache = new BankCache();
			}
			WorkSession.WriteStatus("BankUpdateCommand.DoProcess() : [" + m_Bank.account + "] 의 데이터를 저장합니다");
			m_Result = QueryManager.Bank.Write(m_CharName, m_Bank, bankCache);
			if (!m_Result)
			{
				WorkSession.WriteStatus("BankUpdateCommand.DoProcess() : [" + m_Bank.account + "] 의 데이터 저장에 실패하였습니다. 다시 시도합니다");
				bankCache = new BankCache();
				m_Result = QueryManager.Bank.Write(m_CharName, m_Bank, bankCache);
			}
			if (m_Result)
			{
				WorkSession.WriteStatus("BankUpdateCommand.DoProcess() : [" + m_Bank.account + "] 의 데이터를 업데이트 하였습니다");
				ObjectCache.Bank.Push(m_Bank.account, bankCache);
			}
			else
			{
				WorkSession.WriteStatus("BankUpdateCommand.DoProcess() : [" + m_Bank.account + "] 의 데이터 저장에 실패하였습니다");
				ExceptionMonitor.ExceptionRaised(new Exception("[" + m_Bank.account + "] 의 데이터 저장에 실패하였습니다"));
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("BankUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
