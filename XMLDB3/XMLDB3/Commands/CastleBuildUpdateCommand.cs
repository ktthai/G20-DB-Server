using Mabinogi;

namespace XMLDB3
{
	public class CastleBuildUpdateCommand : BasicCommand
	{
		private long m_CastleID;

		private CastleBuild m_Build;

		private bool m_Result;

		public CastleBuildUpdateCommand()
			: base(NETWORKMSG.NET_DB_CASTLE_BUILD_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_CastleID = _Msg.ReadS64();
			m_Build = CastleBuildSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CastleBuildUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CastleBuildUpdateCommand.DoProcess() : 성 건설 정보를 업데이트 합니다.");
			m_Result = QueryManager.Castle.UpdateBuild(m_CastleID, m_Build);
			if (m_Result)
			{
				WorkSession.WriteStatus("CastleBuildUpdateCommand.DoProcess() : 성 건설 정보를 업데이트하였습니다..");
			}
			else
			{
				WorkSession.WriteStatus("CastleBuildUpdateCommand.DoProcess() : 성 건설 정보를 업데이트하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CastleBuildUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
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
