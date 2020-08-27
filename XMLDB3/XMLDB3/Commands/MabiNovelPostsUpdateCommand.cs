using Mabinogi;

namespace XMLDB3
{
	internal class MabiNovelPostsUpdateCommand : BasicCommand
	{
		private REPLY_RESULT result_ = REPLY_RESULT.ERROR;

		private PostsList postsList_;

		public MabiNovelPostsUpdateCommand()
			: base(NETWORKMSG.NET_DB_MABINOVEL_NOVEL_POSTS_UPDATE)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			postsList_ = MabiNovelPostsListSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("MabiNovelPostsUpdateCommand.DoProcess() : 함수에 진입하였습니다");
			result_ = QueryManager.MabiNovelBoard.UpdataPosts(postsList_);
			if (result_ == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("MabiNovelPostsUpdateCommand.DoProcess() : 게시물 업데이트가 성공 하였습니다..");
			}
			else
			{
				WorkSession.WriteStatus("MabiNovelPostsUpdateCommand.DoProcess() : 게시물 업데이트가 실패 하였습니다.");
			}
			return result_ == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MabiNovelPostsUpdateCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)result_);
			if (result_ == REPLY_RESULT.SUCCESS)
			{
				MabiNovelPostsListSerializer.Deserialize(postsList_, message);
			}
			return message;
		}
	}
}
