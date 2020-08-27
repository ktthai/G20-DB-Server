using Mabinogi;

namespace XMLDB3
{
	public class BankReadCommand : SerializedCommand
	{
		private Bank m_Bank;

		private string m_Account = string.Empty;

		private string m_CharName = string.Empty;

		private BankRace m_Race = BankRace.None;

		public BankReadCommand()
			: base(NETWORKMSG.NET_DB_BANK_ACCOUNT_READ)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_CharName = _Msg.ReadString();
			m_Race = (BankRace)_Msg.ReadU8();
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.StringIDRegistant(m_Account);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("BankReadCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("BankReadCommand.DoProcess() : [" + m_Account + "] 를 캐쉬에서 읽도록 시도합니다.");
			BankCache bankCache = (BankCache)ObjectCache.Bank.Extract(m_Account);
			if (bankCache == null)
			{
				bankCache = new BankCache();
			}
			m_Bank = QueryManager.Bank.Read(m_Account, m_CharName, m_Race, bankCache);
			if (m_Bank != null)
			{
				WorkSession.WriteStatus("BankReadCommand.DoProcess() : [" + m_Account + "] 의 정보를 데이터베이스에서 읽었습니다");
				ObjectCache.Bank.Push(m_Account, bankCache);
				if (m_Bank.data.deposit != 0 && !InventoryHashUtility.CheckHash(m_Bank.data.hash, m_Bank.account, m_Bank.data.deposit, m_Bank.data.updatetime))
				{
					MailSender.Send("hashMissmatch - Bank", "Account: " + m_CharName + "(" + m_Account + ")" + "\tDeposit: " + m_Bank.data.deposit + "\tHash: " + m_Bank.data.hash);
					if (ConfigManager.DoesCheckHash)
					{
						m_Bank = null;
						return false;
					}
				}
				if (m_Bank.slot != null)
				{
					foreach (BankSlot item in m_Bank.slot)
					{
						if (item.item != null && item.item.Count > 0 && !InventoryHashUtility.CheckHash(item.slot.itemHash, item.slot.strToHash, item.Name, item.slot.updatetime))
						{
							MailSender.Send("hashMissmatch - BankSlot", "Account: " + m_CharName + "(" + m_Account + ")" + "\tSlotName: " + item.Name + "\tHash: " + item.slot.itemHash + " " + item.slot.strToHash);
							if (ConfigManager.DoesCheckHash)
							{
								m_Bank = null;
								return false;
							}
						}
					}
				}
			}
			else
			{
				WorkSession.WriteStatus("BankReadCommand.DoProcess() : [" + m_Account + "] 의 정보를 데이터베이스에 쿼리하는데 실패하였습니다");
			}
			return m_Bank != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("BankReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Bank != null)
			{
				message.WriteU8(1);
				BankSerializer.Deserialize(m_Bank, m_Race, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
