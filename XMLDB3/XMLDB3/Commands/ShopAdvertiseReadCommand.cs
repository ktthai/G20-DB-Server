using Mabinogi;

namespace XMLDB3
{
	public class ShopAdvertiseReadCommand : BasicCommand
	{
		private string m_Server = string.Empty;

		private ShopAdvertiseList m_List;

		public ShopAdvertiseReadCommand()
			: base(NETWORKMSG.NET_DB_ADVERTISE_QUERY)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Server = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ShopAdvertiseReadCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("ShopAdvertiseReadCommand.DoProcess() : 상점 광고를 읽습니다.");
			m_List = QueryManager.ShopAdvertise.Read(m_Server, QueryManager.House);
			if (m_List != null)
			{
				WorkSession.WriteStatus("ShopAdvertiseReadCommand.DoProcess() : 상점 광고를 읽는데 성공했습니다.");
				return true;
			}
			WorkSession.WriteStatus("ShopAdvertiseReadCommand.DoProcess() : 상점 광고를 읽는데 실패하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ShopAdvertiseReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_List != null)
			{
				message.WriteU8(1);
				ShopAdvertiseListSerializer.Deserialize(m_List, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
