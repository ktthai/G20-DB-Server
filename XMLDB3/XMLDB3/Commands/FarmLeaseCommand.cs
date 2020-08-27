using Mabinogi;

namespace XMLDB3
{
	public class FarmLeaseCommand : BasicCommand
	{
		private long m_FarmId;

		private string m_OwnerAccount;

		private long m_OwnerCharId;

		private string m_OwnerCharName;

		private long m_ExpireTime;

		private REPLY_RESULT m_Result;

		private byte m_ErrorCode;

		public FarmLeaseCommand()
			: base(NETWORKMSG.NET_DB_FARM_LEASE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_FarmId = _Msg.ReadS64();
			m_OwnerAccount = _Msg.ReadString();
			m_OwnerCharId = _Msg.ReadS64();
			m_OwnerCharName = _Msg.ReadString();
			m_ExpireTime = _Msg.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("FarmLeaseCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("FarmLeaseCommand.DoProcess() : 농장을 임대합니다.");
			m_Result = QueryManager.Farm.Lease(m_FarmId, m_OwnerAccount, m_OwnerCharId, m_OwnerCharName, m_ExpireTime, ref m_ErrorCode);
			if (m_Result == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("FarmLeaseCommand.DoProcess() :  농장을 임대했습니다");
				return true;
			}
			WorkSession.WriteStatus("FarmLeaseCommand.DoProcess() : 농장을 임대하는데 실패하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("FarmLeaseCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result != REPLY_RESULT.FAIL_EX)
			{
				message.WriteU8((byte)m_Result);
			}
			else
			{
				message.WriteU8((byte)m_Result);
				message.WriteU8(m_ErrorCode);
			}
			return message;
		}
	}
}
