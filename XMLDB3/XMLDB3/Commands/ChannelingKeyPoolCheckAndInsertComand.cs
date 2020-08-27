using Mabinogi;

namespace XMLDB3
{
	public class ChannelingKeyPoolCheckAndInsertComand : BasicCommand
	{
		private bool m_Result;

		private ChannelingKey m_chKey;

		public ChannelingKeyPoolCheckAndInsertComand()
			: base(NETWORKMSG.NET_DB_CHANNELING_CHECK_AND_INSERT)
		{
		}

		protected override void ReceiveData(Message _Msg)
		{
			m_chKey = ChannelingKeyPoolSerializer.Serialize(_Msg);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("ChannelingKeyPoolCheckAndInsertComand.DoProcess() : 함수에 진입하였습니다");
			WorkSession.WriteStatus("ChannelingKeyPoolCheckAndInsertComand.DoProcess() : 채널링 키풀 프로시저를 실행합니다");
			m_Result = QueryManager.ChannelingKeyPool.Do(m_chKey);
			if (m_Result)
			{
				WorkSession.WriteStatus("ChannelingKeyPoolCheckAndInsertComand.DoProcess() : 채널링 키풀 프로시저 실행 성공");
			}
			else
			{
				WorkSession.WriteStatus("ChannelingKeyPoolCheckAndInsertComand.DoProcess() : 채널링 키풀 프로시저 실행에 실패하였습니다");
			}
			return m_Result;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("ChannelingKeyPoolCheckAndInsertComand.MakeMessage() : 함수에 진입하였습니다");
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
