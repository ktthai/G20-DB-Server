using Mabinogi;

namespace XMLDB3
{
	internal class MabiNovelFindTitleCommand : BasicCommand
	{
		private REPLY_RESULT result_ = REPLY_RESULT.ERROR;

		private string author_ = "";

		private string title_ = "";

		private string serverName_ = "";

		private long authorId_;

		public MabiNovelFindTitleCommand()
			: base(NETWORKMSG.NET_DB_MABINOVEL_NOVEL_BOARD_FIND_TITLE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			serverName_ = _message.ReadString();
			authorId_ = _message.ReadS64();
			author_ = _message.ReadString();
			title_ = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("MabiNovelFindTitle.DoProcess() : 함수에 진입하였습니다");
			result_ = QueryManager.MabiNovelBoard.FindTitleByAuthor(serverName_, authorId_, author_, title_);
			if (result_ == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("MabiNovelFindTitle.DoProcess() : 이미 있는 타이틀 입니다.");
			}
			else
			{
				WorkSession.WriteStatus("MabiNovelFindTitle.DoProcess() : 해당 작가가 쓴 해당 타이틀이 없습니다.");
			}
			return result_ == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MabiNovelFindTitle.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)result_);
			return message;
		}
	}
}
