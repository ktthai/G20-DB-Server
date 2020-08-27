using Mabinogi;

namespace XMLDB3
{
	public class PetGetWriteCounterCommand : SerializedCommand
	{
		private long m_Id;

		private byte m_counter;

		private bool m_Result;

		public PetGetWriteCounterCommand()
			: base(NETWORKMSG.NET_DB_PET_READ_WRITE_COUNTER)
		{
		}

		protected override void _ReceiveData(Message _Msg)
		{
			m_Id = _Msg.ReadS64();
		}

		public override void OnSerialize(IObjLockRegistHelper _helper, bool bBegin)
		{
			_helper.ObjectIDRegistant(m_Id);
		}

		protected override bool _DoProces()
		{
			WorkSession.WriteStatus("PetGetWriteCounterCommand.DoProcess() : 함수에 진입하였습니다");
			m_Result = QueryManager.Pet.GetWriteCounter(m_Id, out m_counter);
			if (m_Result)
			{
				WorkSession.WriteStatus("PetGetWriteCounterCommand.DoProcess() : 저장 카운터를 읽었습니다.");
			}
			else
			{
				WorkSession.WriteStatus("PetGetWriteCounterCommand.DoProcess() : 저장 카운터를 읽는데 실패하였습니다.");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("PetGetWriteCounterCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Result)
			{
				message.WriteU8(1);
				message.WriteU8(m_counter);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
