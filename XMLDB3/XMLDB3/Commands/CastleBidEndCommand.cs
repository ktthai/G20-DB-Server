using Mabinogi;

namespace XMLDB3
{
	public class CastleBidEndCommand : SerializedCommand
	{
		private Castle m_Castle;

		private bool m_Result;

		public CastleBidEndCommand()
			: base(NETWORKMSG.NET_DB_CASTLE_BID_END)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_Castle = CastleSerializer.Serialize(_Msg);
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.ObjectIDRegistant(m_Castle.castleID);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("CastleBidEndCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CastleBidEndCommand.DoProcess() : 성 경매를 종료합니다.");
			m_Result = QueryManager.Castle.EndBid(m_Castle, QueryManager.Guild);
			if (m_Result)
			{
				WorkSession.WriteStatus("CastleBidEndCommand.DoProcess() : 성 경매를 종료하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CastleBidEndCommand.DoProcess() : 성 경매를 종료하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CastleBidderCreateCommand.MakeMessage() : 함수에 진입하였습니다");
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
