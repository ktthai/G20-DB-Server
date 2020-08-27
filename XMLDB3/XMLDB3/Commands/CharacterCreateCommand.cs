using Mabinogi;

namespace XMLDB3
{
	public class CharacterCreateCommand : BasicCommand
	{
		private CharacterInfo m_WriteCharacter;

		private string m_Account = string.Empty;

		private string m_Server = string.Empty;

		private byte m_Race;

		private bool m_SupportCharacter;

		private byte m_SupportRewardState;

		private string desc = string.Empty;

		private bool m_Result;

		public CharacterCreateCommand()
			: base(NETWORKMSG.MC_DB_CHARACTER_CREATE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Account = _message.ReadString();
			m_SupportRewardState = _message.ReadU8();
			m_Server = _message.ReadString();
			m_Race = _message.ReadU8();
			m_SupportCharacter = (_message.ReadU8() != 0);
			m_WriteCharacter = CharacterSerializer.Serialize(_message);
			desc = m_WriteCharacter.id + "/" + m_WriteCharacter.name + "@" + m_Server;
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CharacterCreateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CharacterCreateCommand.DoProcess() : [" + desc + "] 캐릭터를 생성합니다");
			m_Result = QueryManager.Character.CreateEx(m_Account, m_SupportRewardState, m_Server, m_Race, m_SupportCharacter, m_WriteCharacter, QueryManager.Accountref, QueryManager.Bank, QueryManager.WebSynch);
			if (m_Result)
			{
				WorkSession.WriteStatus("CharacterCreateCommand.DoProcess() : [" + desc + "] 캐릭터를 생성하였습니다");
				return true;
			}
			WorkSession.WriteStatus("CharacterCreateCommand.DoProcess() : [" + desc + "] 캐릭터를 생성하는데 실패하였습니다");
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CharacterCreateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				message.WriteU64((ulong)m_WriteCharacter.id);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
