using Mabinogi;

namespace XMLDB3
{
	public class CharacterInsertMetaCommand : BasicCommand
	{
		private bool result;

		private string account = string.Empty;

		private long charId;

		private string metaKey = string.Empty;

		private string metaType = string.Empty;

		private string metaValue = string.Empty;

		private string desc = string.Empty;

		public CharacterInsertMetaCommand()
			: base(NETWORKMSG.NET_DB_QUERY_INSERT_CHARACTER_META)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			account = _message.ReadString();
			charId = _message.ReadS64();
			metaKey = _message.ReadString();
			metaType = _message.ReadString();
			metaValue = _message.ReadString();
			desc = charId + "/[" + metaKey + ":" + metaType + ":" + metaValue + "]";
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CharacterInsertMetaCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CharacterInsertMetaCommand.DoProcess() : [" + desc + "] 메타데이터를 추가합니다.");
			character_meta_row character_meta_row = new character_meta_row();
			if (character_meta_row != null)
			{
				character_meta_row.charID = charId;
				character_meta_row.mcode = metaKey;
				character_meta_row.mtype = metaType;
				character_meta_row.mdata = metaValue;
				result = QueryManager.Character.InsertCharacterMeta(account, character_meta_row);
				if (result)
				{
					WorkSession.WriteStatus("CharacterInsertMetaCommand.DoProcess() : [" + desc + "] 메타데이터를 추가하였습니다.");
					return true;
				}
				WorkSession.WriteStatus("CharacterInsertMetaCommand.DoProcess() : [" + desc + "] 메타데이터 삽입에 실패했습니다.");
			}
			return result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CharacterInsertMetaCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (result)
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
