using Mabinogi;

namespace XMLDB3
{
	public class FamilyReadCommand : BasicCommand
	{
		private long m_familyID;

		private FamilyListFamily m_family;

		public FamilyReadCommand()
			: base(NETWORKMSG.NET_DB_FAMILY_READ)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_familyID = _message.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("FamilyReadCommand.DoProcess() : 함수에 진입하였습니다");
			m_family = QueryManager.Family.Read(m_familyID);
			if (m_family != null)
			{
				WorkSession.WriteStatus("FamilyReadCommand.DoProcess() : 가문 데이터를 성공적으로 읽었습니다");
			}
			else
			{
				WorkSession.WriteStatus("FamilyReadCommand.DoProcess() : 가문 데이터를 읽는데 실패하였습니다.");
			}
			return m_family != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("FamilyReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_family != null)
			{
				message.WriteU8(1);
				FamilySerializer.Deserialize(m_family, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
