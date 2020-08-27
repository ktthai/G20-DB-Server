using Mabinogi;

namespace XMLDB3
{
	public class ShopAdvertiseUpdateCommand : BasicCommand
	{
		private ShopAdvertisebase m_Advertise;

		private bool m_Result;

		public ShopAdvertiseUpdateCommand()
			: base(NETWORKMSG.NET_DB_ADVERTISE_UPDATE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Advertise = ShopAdvertisebaseSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ShopAdvertiseUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("ShopAdvertiseUpdateCommand.DoProcess() : 상점 광고를 업데이트합니다.");
			m_Result = QueryManager.ShopAdvertise.UpdateShopAdvertise(m_Advertise);
			if (m_Result)
			{
				WorkSession.WriteStatus("ShopAdvertiseUpdateCommand.DoProcess() : 상점 광고를 업데이트하는데 성공하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("ShopAdvertiseUpdateCommand.DoProcess() : 상점 광고를 업데이트하는데 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ShopAdvertiseUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
