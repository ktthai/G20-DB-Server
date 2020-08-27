using Mabinogi;

namespace XMLDB3
{
	public class PetDeleteCommand : BasicCommand
	{
		private long m_Id;

		private string m_Account;

		private string m_Server;

		private bool m_Result;

		public PetDeleteCommand()
			: base(NETWORKMSG.NET_DB_PET_DELETE)
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
			WorkSession.WriteStatus("PetDeleteCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PetDeleteCommand.DoProcess() : [" + m_Id + "/알수없음@" + m_Server + "] 펫을 삭제합니다");
			m_Result = QueryManager.Pet.DeleteEx(m_Account, m_Server, m_Id, QueryManager.Accountref, QueryManager.WebSynch);
			if (m_Result)
			{
				WorkSession.WriteStatus("PetDeleteCommand.DoProcess() : [" + m_Id + "/알수없음@" + m_Server + "] 펫을 삭제하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("PetDeleteCommand.DoProcess() : [" + m_Id + "/알수없음@" + m_Server + "] 펫 삭제에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PetDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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
