using Mabinogi;

namespace XMLDB3
{
	public class CastleUpdateCommand : SerializedCommand
	{
		private Castle m_Castle;

		private bool m_Result;

		public CastleUpdateCommand()
			: base(NETWORKMSG.NET_DB_CASTLE_UPDATE)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_Castle = CastleSerializer.Serialize(_Msg);
			m_Castle.build = null;
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.ObjectIDRegistant(m_Castle.castleID);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("CastleUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CastleUpdateCommand.DoProcess() : 성 정보를 업데이트 합니다.");
			m_Result = QueryManager.Castle.Write(m_Castle);
			if (m_Result)
			{
				WorkSession.WriteStatus("CastleUpdateCommand.DoProcess() : 성 정보를 업데이트하였습니다..");
			}
			else
			{
				WorkSession.WriteStatus("CastleUpdateCommand.DoProcess() : 성 정보를 업데이트하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CastleUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
