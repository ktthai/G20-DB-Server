using Mabinogi;

namespace XMLDB3
{
	public class ShopAdvertiseRegisterCommand : BasicCommand
	{
		private ShopAdvertise m_Advertise;

		private bool m_Result;

		public ShopAdvertiseRegisterCommand()
			: base(NETWORKMSG.NET_DB_ADVERTISE_REGISTER)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Advertise = ShopAdvertiseSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ShopAdvertiseRegisterCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("ShopAdvertiseRegisterCommand.DoProcess() : 상점 광고를 등록합니다.");
			m_Result = QueryManager.ShopAdvertise.Register(m_Advertise);
			if (m_Result)
			{
				WorkSession.WriteStatus("ShopAdvertiseRegisterCommand.DoProcess() : 상점 광고를 등록하는데 성공하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("ShopAdvertiseRegisterCommand.DoProcess() : 상점 광고를 등록하는데 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ShopAdvertiseRegisterCommand.MakeMessage() : 함수에 진입하였습니다");
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
