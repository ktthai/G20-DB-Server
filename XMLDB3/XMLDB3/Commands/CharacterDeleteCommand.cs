using Mabinogi;

namespace XMLDB3
{
	public class CharacterDeleteCommand : BasicCommand
	{
		private long m_Id;

		private byte m_SupportRace;

		private byte m_SupportRewardState;

		private int m_SupportLastChangeTime;

		private string m_Account;

		private string m_Server;

		private bool m_Result;

		public CharacterDeleteCommand()
			: base(NETWORKMSG.MC_DB_CHARACTER_DELETE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Account = _message.ReadString();
			m_SupportLastChangeTime = _message.ReadS32();
			m_SupportRace = _message.ReadU8();
			m_SupportRewardState = _message.ReadU8();
			m_Server = _message.ReadString();
			m_Id = (long)_message.ReadU64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CharacterDeleteCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CharacterDeleteCommand.DoProcess() : [" + m_Id + "/알수없음@" + m_Server + "] 캐릭터를 삭제합니다");
			m_Result = QueryManager.Character.DeleteEx(m_Account, m_SupportRace, m_SupportRewardState, m_SupportLastChangeTime, m_Server, m_Id, QueryManager.Accountref, QueryManager.Bank, QueryManager.Guild, QueryManager.WebSynch);
			if (m_Result)
			{
				WorkSession.WriteStatus("CharacterDeleteCommand.DoProcess() : [" + m_Id + "/알수없음@" + m_Server + "] 캐릭터를 삭제하였습니다");
			}
			else
			{
				WorkSession.WriteStatus("CharacterDeleteCommand.DoProcess() : [" + m_Id + "/알수없음@" + m_Server + "] 캐릭터 삭제에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CharacterDeleteCommand.MakeMessage() : 함수에 진입하였습니다");
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
