using Mabinogi;

namespace XMLDB3
{
	public class RemoveReservedCharNameCommand : BasicCommand
	{
		private string m_Name;

		private string m_Account;

		private bool m_Result;

		public RemoveReservedCharNameCommand()
			: base(NETWORKMSG.NET_DB_REMOVE_RESERVED_CHARNAME_INFO)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Name = _Msg.ReadString();
			m_Account = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("RemoveReservedCharNameCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("RemoveReservedCharNameCommand.DoProcess() : 캐릭터 이름 [" + m_Name + "] 을 중복체크합니다");
			m_Result = QueryManager.Character.RemoveReservedCharName(m_Name, m_Account);
			if (m_Result)
			{
				WorkSession.WriteStatus("RemoveReservedCharNameCommand.DoProcess() : 캐릭터 이름 [" + m_Name + "] 가 사용가능합니다");
			}
			else
			{
				WorkSession.WriteStatus("RemoveReservedCharNameCommand.DoProcess() : 캐릭터 이름 [" + m_Name + "] 가 사용불가합니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("RemoveReservedCharNameCommand.MakeMessage() : 함수에 진입하였습니다");
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
