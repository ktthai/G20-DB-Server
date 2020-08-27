using Mabinogi;

namespace XMLDB3
{
	public class PetCreateCommand : BasicCommand
	{
		private PetInfo m_WritePet;

		private string m_Account = string.Empty;

		private string m_Server = string.Empty;

		private string desc = string.Empty;

		private bool m_Result;

		public PetCreateCommand()
			: base(NETWORKMSG.NET_DB_PET_CREATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_Server = _Msg.ReadString();
			m_WritePet = PetSerializer.Serialize(_Msg);
			desc = m_WritePet.id + "/" + m_WritePet.name + "@" + m_Server;
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PetCreateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PetCreateCommand.DoProcess() : [" + desc + "] 펫을 생성합니다");
			m_Result = QueryManager.Pet.CreateEx(m_Account, m_Server, m_WritePet, QueryManager.Accountref, QueryManager.WebSynch);
			if (m_Result)
			{
				WorkSession.WriteStatus("PetCreateCommand.DoProcess() : [" + desc + "] 캐릭터를 생성하였습니다");
				return true;
			}
			WorkSession.WriteStatus("PetCreateCommand.DoProcess() : [" + desc + "] 캐릭터를 생성하는데 실패하였습니다");
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PetCreateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				message.WriteU64((ulong)m_WritePet.id);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
