using Mabinogi;
using System;

namespace XMLDB3
{
	internal class MabiNovelInsertCommand : BasicCommand
	{
		private REPLY_RESULT result_ = REPLY_RESULT.ERROR;

		private Posts posts_;

		private PageList novelPageList_;

		private string serverName_ = "";

		public MabiNovelInsertCommand()
			: base(NETWORKMSG.NET_DB_MABINOVEL_INSERT_NOVEL)
		{
		}

		protected override void ReceiveData(Message _message)
		{
			serverName_ = _message.ReadString();
			posts_ = new Posts();
			posts_.sn = 0L;
			posts_.authorId = _message.ReadS64();
			posts_.author = _message.ReadString();
			posts_.title = _message.ReadString();
			posts_.transcriptionCount = 0u;
			posts_.blockCount = 0u;
			posts_.option = 0u;
			posts_.readingCount = 0u;
			DateTime dateTime = DateTime.Now.AddMonths(1);
			posts_.endDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
			posts_.endDate.ToFileTime();
			novelPageList_ = MabiNovelListSerializer.Serialize(_message);
		}

		public override bool DoProcess()
		{
			WorkSession.WriteStatus("MabiNovelInsertCommand.DoProcess() : 함수에 진입하였습니다");
			result_ = QueryManager.MabiNovel.Insert(serverName_, posts_, novelPageList_);
			if (result_ == REPLY_RESULT.SUCCESS)
			{
				WorkSession.WriteStatus("MabiNovelInsertCommand.DoProcess() : 마비노벨 데이터를 성공적으로 읽었습니다");
			}
			else
			{
				WorkSession.WriteStatus("MabiNovelInsertCommand.DoProcess() : 마비노벨 데이터를 읽는데 실패하였습니다.");
			}
			return result_ == REPLY_RESULT.SUCCESS;
		}

		public override Message MakeMessage()
		{
			WorkSession.WriteStatus("MabiNovelInsertCommand.MakeMessage() : 함수에 진입하였습니다");
			Message message = new Message(base.ID, 0uL);
			message.WriteU32(base.QueryID);
			message.WriteU8((byte)result_);
			if (result_ == REPLY_RESULT.SUCCESS)
			{
				MabiNovelPostsSerializer.Deserialize(posts_, message);
				MabiNovelListSerializer.Deserialize(novelPageList_, message);
			}
			return message;
		}
	}
}
