using Mabinogi;

namespace XMLDB3
{
	public class CharacterDeleteRsvCommand : BasicCommand
	{
		private long m_Id;

		private string m_Account;

		private string m_Server;

		private long m_Time;

		private bool m_Result;

		public CharacterDeleteRsvCommand()
			: base(NETWORKMSG.MC_DB_CHARACTER_DELETE_RESERVE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_Id = (long)_Msg.ReadU64();
			m_Time = (long)_Msg.ReadU64();
			m_Server = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CharacterDeleteRsvCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CharacterDeleteRsvCommand.DoProcess() : [" + m_Id + "-" + m_Account + "@" + m_Server + "] 캐릭터를 삭제 예약합니다");
			m_Result = QueryManager.Accountref.SetSlotFlag(m_Account, m_Id, m_Server, m_Time);
			if (m_Result)
			{
				WorkSession.WriteStatus("CharacterDeleteRsvCommand.DoProcess() : [" + m_Id + "-" + m_Account + "@" + m_Server + "] 캐릭터를 삭제 예약하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("CharacterDeleteRsvCommand.DoProcess() : [" + m_Id + "-" + m_Account + "@" + m_Server + "] 캐릭터를 삭제 예약에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CharacterDeleteRsvCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				message.WriteU64((ulong)m_Id);
			}
			else
			{
				message.WriteU8(0);
				message.WriteU64((ulong)m_Id);
			}
			return message;
		}
	}
}
