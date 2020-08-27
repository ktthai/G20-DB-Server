using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	internal class MabiNovelPostsListSerializer
	{
		public static Message Deserialize(PostsList _postsList, Message _msg)
		{
			if (_postsList.list != null && _postsList.list.Count > 0)
			{
				_msg.WriteS32(_postsList.list.Count);

				foreach (Posts posts in _postsList.list)
				{
					MabiNovelPostsSerializer.Deserialize(posts, _msg);
				}
			}
			else
			{
				_msg.WriteS32(0);
			}
			return _msg;
		}

		public static PostsList Serialize(Message _msg)
		{
			PostsList postsList = new PostsList();
			int num = _msg.ReadS32();
			if (num > 0)
			{
				postsList.list = new List<Posts>(num);
				for (int i = 0; i < num; i++)
				{
					postsList.list[i] = MabiNovelPostsSerializer.Serialize(_msg);
				}
			}
			else
			{
				postsList.list = null;
			}
			return postsList;
		}
	}
}
