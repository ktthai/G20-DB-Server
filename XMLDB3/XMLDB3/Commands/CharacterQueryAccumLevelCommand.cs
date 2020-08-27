using Mabinogi;

namespace XMLDB3
{
	public class CharacterQueryAccumLevelCommand : BasicCommand
	{
		private string m_Name;

		private string m_Account;

		private uint m_Result;

		public CharacterQueryAccumLevelCommand()
			: base(NETWORKMSG.NET_DB_QUERY_ACCUM_LEVEL)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_Name = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CharacterQueryAccumLevelCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CharacterQueryAccumLevelCommand.DoProcess() : 캐릭터 [" + m_Account + " / " + m_Name + "] 의 누적레벨을 쿼리합니다");
			m_Result = QueryManager.Character.GetAccumLevel(m_Account, m_Name);
			WorkSession.WriteStatus("CharacterQueryAccumLevelCommand.DoProcess() : 캐릭터 [" + m_Account + " / " + m_Name + "] 의 누적레벨은 " + m_Result + " 입니다");
			return m_Result != 0;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CharacterNameUsableCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU32(m_Result);
			return message;
		}
	}
}
