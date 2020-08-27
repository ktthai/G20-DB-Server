using Mabinogi;

namespace XMLDB3
{
	public class HouseBidderDeleteCommand : SerializedCommand
	{
		private long m_HouseID;

		private string m_Account = string.Empty;

		private string m_CharName = string.Empty;

		private int m_RepayMoney;

		private int m_RemainMoney;

		private int m_MaxRemainMoney;

		private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

		public HouseBidderDeleteCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_BIDDER_DELETE)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_HouseID = _Msg.ReadS64();
			m_Account = _Msg.ReadString();
			m_CharName = _Msg.ReadString();
			m_RepayMoney = _Msg.ReadS32();
			m_MaxRemainMoney = _Msg.ReadS32();
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.StringIDRegistant(m_Account);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("HouseBidderDeleteCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseBidderDeleteCommand.DoProcess() : 입찰자를 삭제합니다.");
			m_Result = QueryManager.House.DeleteBidder(m_HouseID, m_Account, m_CharName, m_RepayMoney, QueryManager.Bank, m_MaxRemainMoney, out m_RemainMoney, out string _strHash);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("HouseBidderDeleteCommand.DoProcess() : 입찰자를 삭제하였습니다.");
				BankCache bankCache = (BankCache)ObjectCache.Bank.Find(m_Account);
				if (bankCache != null && bankCache.IsValid())
				{
					bankCache.bank.deposit = m_RemainMoney;
					bankCache.bank.hash = _strHash;
				}
				return true;
			}
			WorkSession.WriteStatus("HouseBidderDeleteCommand.DoProcess() : 입찰자를 삭제하는데 실패하였습니다.");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseBidEndCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				message.WriteS32(m_RemainMoney);
			}
			return message;
		}
	}
}
