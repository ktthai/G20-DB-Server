using Mabinogi;

namespace XMLDB3
{
	public class CharacterUpdateCommand : SerializedCommand
	{
		private CharacterInfo m_WriteCharacter;

		private bool m_Result;

		private string desc = string.Empty;

		private Message m_BuildResultMsg;

		public CharacterUpdateCommand()
			: base(NETWORKMSG.MC_DB_CHARACTER_UPDATE)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_WriteCharacter = new CharacterInfo();
			m_WriteCharacter = CharacterSerializer.Serialize(_Msg);
			desc = m_WriteCharacter.id + "/" + m_WriteCharacter.name;
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.ObjectIDRegistant(m_WriteCharacter.id);
			if (m_WriteCharacter.inventory != null)
			{
				foreach (Item value in m_WriteCharacter.inventory.Values)
				{
					_helper.ObjectIDRegistant(value.id);
				}
			}
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("CharacterUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CharacterUpdateCommand.DoProcess() : [" + desc + "] 의 데이터를 캐쉬에서 읽습니다");
			CharacterInfo cache = (CharacterInfo)ObjectCache.Character.Extract(m_WriteCharacter.id);
			WorkSession.WriteStatus("CharacterUpdateCommand.DoProcess() : [" + desc + "] 의 데이터를 업데이트합니다");
			m_Result = QueryManager.Character.Write(m_WriteCharacter, cache, out m_BuildResultMsg);
			if (!m_Result)
			{
				WorkSession.WriteStatus("CharacterUpdateCommand.DoProcess() : [" + desc + "] 의 데이터 저장에 실패하였습니다. 다시 시도합니다");
				m_Result = QueryManager.Character.Write(m_WriteCharacter, null, out m_BuildResultMsg);
			}
			if (m_Result)
			{
				WorkSession.WriteStatus("CharacterUpdateCommand.DoProcess() : [" + desc + "] 의 데이터를 업데이트 하였습니다");
				ObjectCache.Character.Push(m_WriteCharacter.id, m_WriteCharacter);
			}
			else
			{
				WorkSession.WriteStatus("CharacterUpdateCommand.DoProcess() : [" + desc + "] 의 데이터 저장에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CharacterUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				message.WriteU64((ulong)m_WriteCharacter.id);
				message += m_BuildResultMsg;
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
