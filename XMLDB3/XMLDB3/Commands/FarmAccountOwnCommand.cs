using Mabinogi;

namespace XMLDB3
{
	public class FarmAccountOwnCommand : BasicCommand
	{
		private string m_Account = "";

		private long m_FarmId;

		private long m_OwnerCharID;

		private string m_OwnerCharName = "";

		private bool m_Result;

		public FarmAccountOwnCommand()
			: base(NETWORKMSG.NET_DB_FARM_ACCOUNT_OWN)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("FarmAccountOwnCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("FarmAccountOwnCommand.DoProcess() : 소유한 농장을 읽어옵니다.");
			m_Result = QueryManager.Farm.GetOwnerInfo(m_Account, ref m_FarmId, ref m_OwnerCharID, ref m_OwnerCharName);
			if (m_Result)
			{
				WorkSession.WriteStatus("FarmAccountOwnCommand.DoProcess() :  농장을 읽어왔습니다");
				return true;
			}
			WorkSession.WriteStatus("FarmAccountOwnCommand.DoProcess() : 농장을 얻는데 실패하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("FarmAccountOwnCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				message.WriteS64(m_FarmId);
				message.WriteS64(m_OwnerCharID);
				message.WriteString(m_OwnerCharName);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
