using Mabinogi;
using System;

namespace XMLDB3
{
	internal class MabiNovelPostsSerializer
	{
		public static Posts Serialize(Message _msg)
		{
			Posts posts = new Posts();
			posts.sn = _msg.ReadS64();
			posts.authorId = _msg.ReadS64();
			posts.author = _msg.ReadString();
			posts.title = _msg.ReadString();
			posts.transcriptionCount = _msg.ReadU32();
			posts.endDate = new DateTime((long)(_msg.ReadU64() * 10000));
			posts.blockCount = _msg.ReadU32();
			posts.option = _msg.ReadU32();
			posts.readingCount = _msg.ReadU32();
			return posts;
		}

		public static Message Deserialize(Posts _posts, Message _msg)
		{
			_msg.WriteS64(_posts.sn);
			_msg.WriteS64(_posts.authorId);
			_msg.WriteString(_posts.author);
			_msg.WriteString(_posts.title);
			_msg.WriteU32(_posts.transcriptionCount);
			_msg.WriteS64(_posts.endDate.Ticks);
			_msg.WriteU32(_posts.blockCount);
			_msg.WriteU32(_posts.option);
			_msg.WriteU32(_posts.readingCount);
			return _msg;
		}
	}
}
