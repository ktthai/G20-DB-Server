using Mabinogi;

namespace XMLDB3
{
	public class CharacterNameUsableCommand : BasicCommand
	{
		private string m_Name;

		private string m_Account;

		private bool m_Result;

		public CharacterNameUsableCommand()
			: base(NETWORKMSG.NET_DB_CHARACTER_NAME_USABLE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Name = _Msg.ReadString();
			m_Account = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CharacterNameUsableCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CharacterNameUsableCommand.DoProcess() : 캐릭터 이름 [" + m_Name + "] 을 중복체크합니다");
			m_Result = QueryManager.Character.IsUsableName(m_Name, m_Account);
			if (m_Result)
			{
				WorkSession.WriteStatus("CharacterNameUsableCommand.DoProcess() : 캐릭터 이름 [" + m_Name + "] 가 사용가능합니다");
			}
			else
			{
				WorkSession.WriteStatus("CharacterNameUsableCommand.DoProcess() : 캐릭터 이름 [" + m_Name + "] 가 사용불가합니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CharacterNameUsableCommand.MakeMessage() : 함수에 진입하였습니다");
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
