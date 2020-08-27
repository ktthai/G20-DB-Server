using Mabinogi;

namespace XMLDB3
{
	public class PlayoutCommand : BasicCommand
	{
		private string m_Account;

		private int m_RemainTime;

		private string m_Server;

		private GroupIDList m_CharGroupID;

		private GroupIDList m_PetGroupID;

		private byte m_supportRace;

		private byte m_supportRewardState;

		private int m_supportLastChangeTime;

		private byte m_macroCheckFailure;

		private byte m_macroCheckSuccess;

		private bool m_beginnerFlag;

		private bool m_bResult;

		public PlayoutCommand()
			: base(NETWORKMSG.NET_DB_SIGNAL_PLAYOUT)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			m_Account = _message.ReadString();
			m_RemainTime = _message.ReadS32();
			m_Server = _message.ReadString();
			m_supportLastChangeTime = _message.ReadS32();
			m_supportRace = _message.ReadU8();
			m_supportRewardState = _message.ReadU8();
			m_CharGroupID = GroupIDListSerializer.Serialize(_message);
			m_PetGroupID = GroupIDListSerializer.Serialize(_message);
			m_macroCheckFailure = _message.ReadU8();
			m_macroCheckSuccess = _message.ReadU8();
			m_beginnerFlag = ((_message.ReadU8() != 0) ? true : false);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("PlayoutCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("PlayoutCommand.DoProcess() : [" + m_Account + "] 가 게임접속종료를 기록합니다");
			m_bResult = QueryManager.Accountref.PlayOut(m_Account, m_RemainTime, m_Server, m_CharGroupID, m_PetGroupID, m_supportRace, m_supportRewardState, m_supportLastChangeTime, m_macroCheckFailure, m_macroCheckSuccess, m_beginnerFlag);
			if (m_bResult)
			{
				WorkSession.WriteStatus("PlayoutCommand.DoProcess() : [" + m_Account + "] 가 게임접속종료를 기록합니다");
				return true;
			}
			WorkSession.WriteStatus("PlayoutCommand.DoProcess() : [" + m_Account + "] 가 게임접속종료를 기록하지 못하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PlayoutCommand.MakeMessage() : 함수에 진입하였습니다");
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
