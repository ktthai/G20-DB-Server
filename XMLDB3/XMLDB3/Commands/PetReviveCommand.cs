using Mabinogi;

namespace XMLDB3
{
	public class PetReviveCommand : BasicCommand
	{
		private long m_Id;

		private string m_Account;

		private string m_Server;

		private bool m_Result;

		public PetReviveCommand()
			: base(NETWORKMSG.NET_DB_PET_REVIVE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_Id = (long)_Msg.ReadU64();
			m_Server = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PetReviveCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PetReviveCommand.DoProcess() : [" + m_Id + "-" + m_Account + "@" + m_Server + "] 펫을 삭제 취소합니다");
			m_Result = QueryManager.Accountref.SetPetSlotFlag(m_Account, m_Id, m_Server, 0L);
			if (m_Result)
			{
				WorkSession.WriteStatus("PetReviveCommand.DoProcess() : [" + m_Id + "-" + m_Account + "@" + m_Server + "] 펫을 삭제 취소했습니다");
			}
			else
			{
				WorkSession.WriteStatus("PetReviveCommand.DoProcess() : [" + m_Id + "-" + m_Account + "@" + m_Server + "] 펫을 삭제 취소에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PetReviveCommand.MakeMessage() : 함수에 진입하였습니다");
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
