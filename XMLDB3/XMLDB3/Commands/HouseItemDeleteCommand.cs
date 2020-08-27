using Mabinogi;

namespace XMLDB3
{
	public class HouseItemDeleteCommand : SerializedCommand
	{
		private long m_HouseID;

		private string m_Account = string.Empty;

		private Item m_Item;

		private int m_HouseMoney;

		private bool m_Result;

		public HouseItemDeleteCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_ITEM_DELETE)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_HouseID = _Msg.ReadS64();
			m_Account = _Msg.ReadString();
			m_HouseMoney = _Msg.ReadS32();
			m_Item = ItemSerializer.Serialize(_Msg);
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			if (m_HouseID != 0)
			{
				_helper.ObjectIDRegistant(m_HouseID);
			}
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("HouseItemDeleteCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseItemDeleteCommand.DoProcess() : 집 아이템을 삭제합니다.");
			m_Result = QueryManager.House.DeleteItem(m_HouseID, m_Account, m_Item, m_HouseMoney);
			if (m_Result)
			{
				WorkSession.WriteStatus("HouseItemDeleteCommand.DoProcess() : 집 아이템을 삭제하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("HouseItemDeleteCommand.DoProcess() : 집 아이템을 삭제하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseItemDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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
