using Mabinogi;

namespace XMLDB3
{
	public class ShopAdvertiseUnregisterCommand : BasicCommand
	{
		private string m_Account = string.Empty;

		private string m_Server = string.Empty;

		private bool m_Result;

		public ShopAdvertiseUnregisterCommand()
			: base(NETWORKMSG.NET_DB_ADVERTISE_UNREGISTER)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Account = _message.ReadString();
			m_Server = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ShopAdvertiseUnregisterCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("ShopAdvertiseUnregisterCommand.DoProcess() : 상점 광고를 삭제합니다.");
			m_Result = QueryManager.ShopAdvertise.Unregister(m_Account, m_Server);
			if (m_Result)
			{
				WorkSession.WriteStatus("ShopAdvertiseUnregisterCommand.DoProcess() : 상점 광고를 삭제하는데 성공하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("ShopAdvertiseUnregisterCommand.DoProcess() : 상점 광고를 삭제하는데 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ShopAdvertiseUnregisterCommand.MakeMessage() : 함수에 진입하였습니다");
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
