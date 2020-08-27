using Mabinogi;

namespace XMLDB3
{
	public class ShopAdvertiseItemAddCommand : BasicCommand
	{
		private string m_Account = string.Empty;

		private string m_Server = string.Empty;

		private ShopAdvertiseItem m_Item;

		private bool m_Result;

		public ShopAdvertiseItemAddCommand()
			: base(NETWORKMSG.NET_DB_ADVERTISE_ITEM_ADD)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Account = _message.ReadString();
			m_Server = _message.ReadString();
			m_Item = ShopAdvertiseItemSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ShopAdvertiseItemAddCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("ShopAdvertiseItemAddCommand.DoProcess() : 상점 광고에 아이템을 더합니다.");
			m_Result = QueryManager.ShopAdvertise.AddItem(m_Account, m_Server, m_Item);
			if (m_Result)
			{
				WorkSession.WriteStatus("ShopAdvertiseItemAddCommand.DoProcess() : 상점 광고에 아이템을 더하는데 성공하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("ShopAdvertiseItemAddCommand.DoProcess() : 점 광고에 아이템을 더하는데 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ShopAdvertiseItemAddCommand.MakeMessage() : 함수에 진입하였습니다");
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
