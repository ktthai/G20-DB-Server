using Mabinogi;
using System;

namespace XMLDB3
{
	public class BankUpdateExCommand : SerializedCommand
	{
		private CharacterInfo m_Character;

		private Bank m_Bank;

		private bool m_Result;

		private Message m_BuildResultMsg;

		private string desc;

		public BankUpdateExCommand()
			: base(NETWORKMSG.NET_DB_BANK_ACCOUNT_UPDATE_EX)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_Character = CharacterSerializer.Serialize(_Msg);
			m_Bank = BankSerializer.Serialize(_Msg);
			desc = m_Character.id + "/" + m_Character.name;
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.StringIDRegistant(m_Bank.account);
			if (m_Bank.slot != null)
			{
				foreach (BankSlot item3 in m_Bank.slot)
				{
					if (item3.item != null)
					{
						foreach (BankItem bankItem in item3.item)
						{
							_helper.ObjectIDRegistant(bankItem.item.id);
						}
					}
				}
			}
			_helper.ObjectIDRegistant(m_Character.id);
			if (m_Character.inventory != null)
			{
				foreach (Item value in m_Character.inventory.Values)
				{
					_helper.ObjectIDRegistant(value.id);
				}
			}
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : [" + m_Bank.account + "] 의 데이터를 캐쉬에서 읽습니다");
			BankCache bankCache = (BankCache)ObjectCache.Bank.Extract(m_Bank.account);
			if (bankCache == null)
			{
				bankCache = new BankCache();
			}
			WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : [" + desc + "] 의 데이터를 캐쉬에서 읽습니다");
			CharacterInfo charCache = (CharacterInfo)ObjectCache.Character.Extract(m_Character.id);
			WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : [" + m_Bank.account + "] 의 데이터를 저장합니다");
			m_Result = QueryManager.Bank.WriteEx(m_Bank, m_Character, bankCache, charCache, QueryManager.Character, out m_BuildResultMsg);
			if (!m_Result)
			{
				WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : [" + m_Bank.account + "] 의 데이터 저장에 실패하였습니다. 다시 시도합니다");
				bankCache = new BankCache();
				m_Result = QueryManager.Bank.WriteEx(m_Bank, m_Character, bankCache, null, QueryManager.Character, out m_BuildResultMsg);
			}
			if (m_Result)
			{
				WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : [" + m_Bank.account + "] 의 데이터를 업데이트 하였습니다");
				ObjectCache.Bank.Push(m_Bank.account, bankCache);
				ObjectCache.Character.Push(m_Character.id, m_Character);
			}
			else
			{
				WorkSession.WriteStatus("BankUpdateExCommand.DoProcess() : [" + m_Bank.account + "] 의 데이터 저장에 실패하였습니다");
				ExceptionMonitor.ExceptionRaised(new Exception("[" + m_Bank.account + "] 의 데이터 저장에 실패하였습니다"));
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("BankUpdateExCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8(1);
			return message + m_BuildResultMsg;
		}
	}
}
