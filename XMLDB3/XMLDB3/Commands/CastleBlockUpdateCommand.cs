using Mabinogi;

namespace XMLDB3
{
	public class CastleBlockUpdateCommand : BasicCommand
	{
		private long m_CastleID;

		private CastleBlock[] m_AddedBlock;

		private CastleBlock[] m_DeletedBlock;

		private bool m_Result;

		public CastleBlockUpdateCommand()
			: base(NETWORKMSG.NET_DB_CASTLE_BLOCK_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_CastleID = _Msg.ReadS64();
			m_AddedBlock = CastleBlockSerializer.Serialize(_Msg);
			m_DeletedBlock = CastleBlockSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("CastleBlockUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("CastleBlockUpdateCommand.DoProcess() : 성 집 출입 제한 리스트를 업데이트 합니다.");
			m_Result = QueryManager.Castle.UpdateBlock(m_CastleID, m_AddedBlock, m_DeletedBlock);
			if (m_Result)
			{
				WorkSession.WriteStatus("CastleBlockUpdateCommand.DoProcess() : 성 집 출입 제한 리스트를 업데이트했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("CastleBlockUpdateCommand.DoProcess() : 성 집 출입 제한 리스트를 업데이트하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("CastleBlockUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
