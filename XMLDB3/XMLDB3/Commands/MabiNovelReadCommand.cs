using Mabinogi;

namespace XMLDB3
{
	internal class MabiNovelReadCommand : BasicCommand
	{
		private long sn_;

		private PageList pageList_;

		public MabiNovelReadCommand()
			: base(NETWORKMSG.NET_DB_MABINOVEL_NOVEL_PAGE_DATA_READ)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			sn_ = (long)_message.ReadU64();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("MabiNovelReadCommand.DoProcess() : 함수에 진입하였습니다");
			pageList_ = QueryManager.MabiNovel.Read(sn_);
			if (pageList_ != null)
			{
				WorkSession.WriteStatus("MabiNovelReadCommand.DoProcess() : 마비노벨 데이터를 성공적으로 읽었습니다.");
			}
			else
			{
				WorkSession.WriteStatus("MabiNovelReadCommand.DoProcess() : 마비노벨 데이터를 읽는데 실패하였습니다.");
			}
			return pageList_ != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MabiNovelReadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (pageList_ != null)
			{
				message.WriteU8(1);
				MabiNovelListSerializer.Deserialize(pageList_, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
