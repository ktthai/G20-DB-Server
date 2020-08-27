using Mabinogi;

namespace XMLDB3
{
	public class WorldMetaReadCommand : BasicCommand
	{
		private WorldMetaList m_WorldMetaList;

		public override bool IsPrimeCommand => true;

		public WorldMetaReadCommand()
			: base(NETWORKMSG.NET_DB_WORLDMETA_READ)
		{
		}

		protected override void ReceiveData(Message _message)
		{
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("WorldMetaReadCommand.DoProcess() : 함수에 진입하였습니다");
			m_WorldMetaList = QueryManager.WorldMeta.Read();
			if (m_WorldMetaList != null)
			{
				WorkSession.WriteStatus("WorldMetaReadCommand.DoProcess() : 월드메타 데이터를 성공적으로 읽었습니다");
			}
			else
			{
				WorkSession.WriteStatus("WorldMetaReadCommand.DoProcess() : 월드메타 데이터를 읽는데 실패하였습니다.");
			}
			return m_WorldMetaList != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("WorldMetaReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_WorldMetaList != null)
			{
				message.WriteU8(1);
				WorldMetaListSerializer.Deserialize(m_WorldMetaList, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
