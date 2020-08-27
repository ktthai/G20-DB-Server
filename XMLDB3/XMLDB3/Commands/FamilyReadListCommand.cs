using Mabinogi;

namespace XMLDB3
{
	public class FamilyReadListCommand : BasicCommand
	{
		private FamilyList m_familyList;

		public FamilyReadListCommand()
			: base(NETWORKMSG.NET_DB_FAMILY_LIST)
		{
		}

		protected override void ReceiveData(Message _message)
		{
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("FamilyReadListCommand.DoProcess() : 함수에 진입하였습니다");
			m_familyList = QueryManager.Family.ReadList();
			if (m_familyList != null)
			{
				WorkSession.WriteStatus("FamilyReadListCommand.DoProcess() : 가문 데이터를 성공적으로 읽었습니다");
			}
			else
			{
				WorkSession.WriteStatus("FamilyReadListCommand.DoProcess() : 가문 데이터를 읽는데 실패하였습니다.");
			}
			return m_familyList != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("FamilyReadListCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_familyList != null)
			{
				message.WriteU8(1);
				FamilyListSerializer.Deserialize(m_familyList, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
