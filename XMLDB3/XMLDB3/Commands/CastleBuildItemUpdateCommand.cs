using Mabinogi;

namespace XMLDB3
{
	public class CastleBuildItemUpdateCommand : BasicCommand
	{
		private long m_CastleID;

		private CastleBuildResource m_Resource;

		private bool m_Result;

		public CastleBuildItemUpdateCommand()
			: base(NETWORKMSG.NET_DB_CASTLE_BUILD_ITEM_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_CastleID = _Msg.ReadS64();
			m_Resource = CastleBuildResourceSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CastleBuildItemUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CastleBuildItemUpdateCommand.DoProcess() : 성 건설 아이템 정보를 업데이트 합니다.");
			m_Result = QueryManager.Castle.UpdateBuildResource(m_CastleID, m_Resource);
			if (m_Result)
			{
				WorkSession.WriteStatus("CastleBuildItemUpdateCommand.DoProcess() : 성 건설 아이템 정보를 업데이트하였습니다..");
			}
			else
			{
				WorkSession.WriteStatus("CastleBuildItemUpdateCommand.DoProcess() : 성 건설 아이템 정보를 업데이트하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CastleBuildItemUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
