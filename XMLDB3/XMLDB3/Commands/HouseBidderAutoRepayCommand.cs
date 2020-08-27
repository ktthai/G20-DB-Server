using Mabinogi;

namespace XMLDB3
{
	public class HouseBidderAutoRepayCommand : BasicCommand
	{
		private long m_HouseID;

		private string m_Account = string.Empty;

		private HouseInventory m_Inventory;

		private bool m_Result;

		public HouseBidderAutoRepayCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_BIDDER_AUTOREPAY)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_HouseID = _Msg.ReadS64();
			m_Account = _Msg.ReadString();
			m_Inventory = HouseInventorySerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("HouseBidderAutoRepayCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseBidderAutoRepayCommand.DoProcess() : 입찰자를 삭제합니다.");
			m_Result = QueryManager.House.AutoRepay(m_HouseID, m_Account, m_Inventory);
			if (m_Result)
			{
				WorkSession.WriteStatus("HouseBidderAutoRepayCommand.DoProcess() : 입찰자를 삭제하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("HouseBidderAutoRepayCommand.DoProcess() : 입찰자를 삭제하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseBidderAutoRepayCommand.MakeMessage() : 함수에 진입하였습니다");
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
