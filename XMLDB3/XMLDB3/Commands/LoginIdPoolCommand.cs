using Mabinogi;

namespace XMLDB3
{
	public class LoginIdPoolCommand : BasicCommand
	{
		private int m_Size;

		private long m_IdOffset;

		public override bool IsPrimeCommand => true;

		public LoginIdPoolCommand()
			: base(NETWORKMSG.NET_DB_LOGIN_ID_POOL)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Size = _Msg.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("LoginIdPoolCommand.DoProcess() : 함수에 진입하였습니다");
			m_IdOffset = QueryManager.LoginIdPool.GetIdPool(m_Size);
			return true;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("LoginIdPoolCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU64((ulong)m_IdOffset);
			return message;
		}
	}
}
