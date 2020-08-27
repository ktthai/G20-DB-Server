using Mabinogi;

namespace XMLDB3
{
	public class HouseBidderCreateCommand : SerializedCommand
	{
		private long m_HouseID;

		private HouseBidder m_HouseBidder;

		private int m_RemainMoney;

		private byte m_ErrorCode;

		private REPLY_RESULT m_Result = REPLY_RESULT.ERROR;

		public HouseBidderCreateCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_BIDDER_CREATE)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_HouseID = _Msg.ReadS64();
			m_HouseBidder = HouseBidderSerializer.Serialize(_Msg);
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.StringIDRegistant(m_HouseBidder.bidAccount);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("HouseBidderCreateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseBidderCreateCommand.DoProcess() : 집 입찰자를 생성합니다.");
			m_Result = QueryManager.House.CreateBidder(m_HouseID, m_HouseBidder, QueryManager.Bank, out m_ErrorCode, out m_RemainMoney, out string _strHash);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("HouseBidderCreateCommand.DoProcess() : 집 입찰자를 생성하였습니다.");
				BankCache bankCache = (BankCache)ObjectCache.Bank.Find(m_HouseBidder.bidAccount);
				if (bankCache != null && bankCache.IsValid())
				{
					bankCache.bank.deposit = m_RemainMoney;
					bankCache.bank.hash = _strHash;
				}
				return true;
			}
			WorkSession.WriteStatus("HouseBidderCreateCommnad.DoProcess() : 집 입찰자를 생성하는데 실패하였습니다.");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseBidderCreateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				message.WriteS32(m_RemainMoney);
			}
			else if (m_Result == REPLY_RESULT.FAIL_EX)
			{
				message.WriteU8(m_ErrorCode);
				if (m_ErrorCode == 0)
				{
					message.WriteS32(m_RemainMoney);
				}
			}
			return message;
		}
	}
}
