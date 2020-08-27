using Mabinogi;

namespace XMLDB3
{
	public class HouseUpdateCommand : SerializedCommand
	{
		private House m_House;

		private bool m_Result;

		public HouseUpdateCommand()
			: base(NETWORKMSG.NET_DB_HOUSE_UPDATE)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_House = HouseAppearSerializer.Serialize(_Msg);
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.ObjectIDRegistant(m_House.houseID);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("HouseUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("HouseUpdateCommand.DoProcess() : 집 상태를 업데이트합니다.");
			m_Result = QueryManager.House.Write(m_House);
			if (m_Result)
			{
				WorkSession.WriteStatus("HouseUpdateCommand.DoProcess() : 집 상태를 업데이트 하였습니다.");
			}
			else
			{
				WorkSession.WriteStatus("HouseUpdateCommand.DoProcess() : 집 상태를 업데이트 하는데 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("HouseUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
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
