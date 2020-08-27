using Mabinogi;

namespace XMLDB3
{
	internal class MabiNovelPostsRoyaltiesListCommand : BasicCommand
	{
		private PostsList postsList_;

		private string serverName_ = "";

		public MabiNovelPostsRoyaltiesListCommand()
			: base(NETWORKMSG.NET_DB_MABINOVEL_NOVEL_ROTALTIES_LIST_LOAD)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			serverName_ = _message.ReadString();
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("MabiNovelPostsRoyaltiesListCommand.DoProcess() : 함수에 진입하였습니다");
			postsList_ = QueryManager.MabiNovelBoard.ReadPostsRoyaltiesList(serverName_);
			if (postsList_ != null)
			{
				WorkSession.WriteStatus("MabiNovelPostsRoyaltiesListCommand.DoProcess() : 인세를 지불해야하는 리스트를 받아 왔습니다.");
			}
			else
			{
				WorkSession.WriteStatus("MabiNovelPostsRoyaltiesListCommand.DoProcess() : 인세를 지불해야하는 리스트를 받아 오는데 실패 하였습니다.");
			}
			return postsList_ != null;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MabiNovelPostsRoyaltiesListCommand.MakeMessage() : 함수에 진입하였습니다");
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
