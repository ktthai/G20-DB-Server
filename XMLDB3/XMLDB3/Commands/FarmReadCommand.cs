using Mabinogi;

namespace XMLDB3
{
	public class FarmReadCommand : BasicCommand
	{
		private long m_FarmId;

		private Farm m_Farm;

		public override bool IsPrimeCommand => true;

		public FarmReadCommand()
			: base(NETWORKMSG.NET_DB_FARM_READ)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_FarmId = _Msg.ReadS64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("FarmReadCommand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("FarmReadCommand.DoProcess() : 농장을 읽어옵니다.");
			m_Farm = QueryManager.Farm.Read(m_FarmId);
			if (m_Farm != null)
			{
				WorkSession.WriteStatus("FarmReadCommand.DoProcess() :  농장을 읽어왔습니다");
				return true;
			}
			WorkSession.WriteStatus("FarmReadCommand.DoProcess() : 농장을 얻는데 실패하였습니다");
			return false;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("FarmReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (m_Farm != null)
			{
				message.WriteU8(1);
				FarmSerializer.Deserialize(m_Farm, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
