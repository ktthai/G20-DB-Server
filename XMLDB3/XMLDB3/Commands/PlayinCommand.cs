using Mabinogi;

namespace XMLDB3
{
	public class PlayinCommand : BasicCommand
	{
		private string m_Account;

		private int m_RemainTime;

		private bool m_bResult;

		public PlayinCommand()
			: base(NETWORKMSG.NET_DB_SIGNAL_PLAYIN)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_Account = _Msg.ReadString();
			m_RemainTime = _Msg.ReadS32();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PlayinCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PlayinCommand.DoProcess() : [" + m_Account + "] 가 게임접속을 기록합니다");
			m_bResult = QueryManager.Accountref.PlayIn(m_Account, m_RemainTime);
			if (m_bResult)
			{
				WorkSession.WriteStatus("PlayinCommand.DoProcess() : [" + m_Account + "] 가 게임접속을 기록합니다");
				return true;
			}
			WorkSession.WriteStatus("PlayinCommand.DoProcess() : [" + m_Account + "] 가 게임접속을 기록하지 못하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PlayinCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_bResult)
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
