using Mabinogi;

namespace XMLDB3
{
	public class WorldMetaUpdateCommand : BasicCommand
	{
		private WorldMetaList m_WorldMetaUpdateList;

		private string[] m_removeKeys;

		private REPLY_RESULT m_Result;

		private byte m_errorCode;

		public WorldMetaUpdateCommand()
			: base(NETWORKMSG.NET_DB_WORLDMETA_UPDATE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			WorldMetaUpdateListSerializer.Serialize(_message, out m_WorldMetaUpdateList, out m_removeKeys);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("WorldMetaUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			if ((m_WorldMetaUpdateList == null || m_WorldMetaUpdateList.metas == null || m_WorldMetaUpdateList.metas.Count == 0) && (m_removeKeys == null || m_removeKeys.Length == 0))
			{
				WorkSession.WriteStatus("WorldMetaUpdateCommand.DoProcess() : 월드메타 업데이트할 항목이 없습니다.");
				m_Result = REPLY_RESULT.SUCCESS;
				return true;
			}
			m_Result = QueryManager.WorldMeta.UpdateList(m_WorldMetaUpdateList, m_removeKeys, ref m_errorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("WorldMetaUpdateCommand.DoProcess() : 월드메타 데이터를 성공적으로 업데이트했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("WorldMetaUpdateCommand.DoProcess() : 월드메타 데이터를 업데이트하는데 실패하였습니다.");
			}
			return m_Result == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("WorldMetaUpdateListSerializer.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)m_Result);
			if (m_Result == REPLY_RESULT.FAIL_EX)
			{
				message.WriteU8(m_errorCode);
			}
			return message;
		}
	}
}
