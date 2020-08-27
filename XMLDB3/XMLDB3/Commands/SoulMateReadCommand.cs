using Mabinogi;

namespace XMLDB3
{
	public class SoulMateReadCommand : BasicCommand
	{
		private SoulMateList m_soulMateList;

		public SoulMateReadCommand()
			: base(NETWORKMSG.NET_DB_SOULMATE_READ_LIST)
		{
		}

		protected override void ReceiveData(Message _message)
		{
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("SoulMateReadCommand.DoProcess() : 함수에 진입하였습니다");
			m_soulMateList = QueryManager.SoulMate.ReadList();
			if (m_soulMateList != null)
			{
				WorkSession.WriteStatus("SoulMateReadCommand.DoProcess() : 소울메이트 데이터를 성공적으로 읽었습니다");
			}
			else
			{
				WorkSession.WriteStatus("SoulMateReadCommand.DoProcess() : 소울메이트 데이터를 읽는데 실패하였습니다.");
			}
			return m_soulMateList != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("SoulMateReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_soulMateList != null)
			{
				message.WriteU8(1);
				SoulMateListSerializer.Deserialize(m_soulMateList, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
