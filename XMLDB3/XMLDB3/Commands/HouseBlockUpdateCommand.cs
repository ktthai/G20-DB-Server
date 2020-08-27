using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class HouseBlockUpdateCommand : BasicCommand
	{
		private long m_HouseID;

		private List<HouseBlock> m_AddedBlock;

		private List<HouseBlock> m_DeletedBlock;

		private bool m_Result;

		public HouseBlockUpdateCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_BLOCK_UPDATE)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_HouseID = _Msg.ReadS64();
			m_AddedBlock = HouseBlockSerializer.Serialize(_Msg);
			m_DeletedBlock = HouseBlockSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("HouseBlockUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseBlockUpdateCommand.DoProcess() : 집 집 출입 제한 리스트를 업데이트 합니다.");
			m_Result = QueryManager.House.UpdateBlock(m_HouseID, m_AddedBlock, m_DeletedBlock);
			if (m_Result)
			{
				WorkSession.WriteStatus("HouseBlockUpdateCommand.DoProcess() : 집 집 출입 제한 리스트를 업데이트했습니다.");
			}
			else
			{
				WorkSession.WriteStatus("HouseBlockUpdateCommand.DoProcess() : 집 집 출입 제한 리스트를 업데이트하는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseBlockUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
