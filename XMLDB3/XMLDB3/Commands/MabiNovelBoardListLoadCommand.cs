using Mabinogi;

namespace XMLDB3
{
	internal class MabiNovelBoardListLoadCommand : BasicCommand
	{
		private string serverName_ = "";

		private PostsList postsList_;

		public MabiNovelBoardListLoadCommand()
			: base(NETWORKMSG.NET_DB_MABINOVEL_NOVEL_BOARD_LIST_LOAD)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			serverName_ = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("MabiNovelBoardListLoadCommand.DoProcess() : 함수에 진입하였습니다");
			postsList_ = QueryManager.MabiNovelBoard.ReadPostsList(serverName_);
			if (postsList_ != null)
			{
				WorkSession.WriteStatus("MabiNovelBoardListLoadCommand.DoProcess() : 마비노벨 데이터를 성공적으로 읽었습니다");
			}
			else
			{
				WorkSession.WriteStatus("MabiNovelBoardListLoadCommand.DoProcess() : 마비노벨 데이터를 읽는데 실패하였습니다.");
			}
			return postsList_ != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MabiNovelBoardListLoadCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			if (postsList_ != null)
			{
				message.WriteU8(1);
				MabiNovelPostsListSerializer.Deserialize(postsList_, message);
			}
			else
			{
				message.WriteU8(0);
			}
			return message;
		}
	}
}
