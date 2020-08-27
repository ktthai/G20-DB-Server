using Mabinogi;

namespace XMLDB3
{
	public class CharacterReadCommand : SerializedCommand
	{
		private CharacterInfo m_ReadCharacter;

		private long m_Id;

		public CharacterReadCommand()
			: base(NETWORKMSG.MC_DB_CHARACTER_READ)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_Id = (long)_Msg.ReadU64();
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.ObjectIDRegistant(m_Id);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("CharacterReadCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CharacterReadCommand.DoProcess() : [" + m_Id + "] 를 캐쉬에서 읽기를 시도합니다");
			CharacterInfo cache = (CharacterInfo)ObjectCache.Character.Extract(m_Id);
			m_ReadCharacter = QueryManager.Character.Read(m_Id, cache);
			if (m_ReadCharacter != null)
			{
				WorkSession.WriteStatus("CharacterReadCommand.DoProcess() : [" + m_Id + "] 를 데이터베이스에서 읽었습니다");
				ObjectCache.Character.Push(m_Id, m_ReadCharacter);
				if (!InventoryHashUtility.CheckHash(m_ReadCharacter.inventoryHash, m_ReadCharacter.strToHash, m_ReadCharacter.id, m_ReadCharacter.updatetime))
				{
					MailSender.Send("hashMissmatch - ReadCharacter", "Account: " + m_ReadCharacter.name + "(" + m_ReadCharacter.id + ")" + "\tHash: " + m_ReadCharacter.inventoryHash + " " + m_ReadCharacter.strToHash);
					if (ConfigManager.DoesCheckHash)
					{
						m_ReadCharacter = null;
					}
				}
			}
			else
			{
				WorkSession.WriteStatus("CharacterReadCommand.DoProcess() : [" + m_Id + "] 를 데이터베이스에 쿼리하는데 실패하였습니다");
			}
			return m_ReadCharacter != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CharacterReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_ReadCharacter != null)
			{
				message.WriteU8(1);
				CharacterSerializer.Deserialize(m_ReadCharacter, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
