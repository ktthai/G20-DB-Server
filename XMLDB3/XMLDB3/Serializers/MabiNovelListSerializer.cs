using Mabinogi;
using System.Collections.Generic;

namespace XMLDB3
{
	public class MabiNovelListSerializer
	{
		public static Message Deserialize(PageList _list, Message _msg)
		{
			if (_list.novel != null && _list.novel.Count > 0)
			{
				_msg.WriteS32(_list.novel.Count);

				foreach (PageData page in _list.novel)
				{
					MabiNovelSerializer.Deserialize(page, _msg);
				}
			}
			else
			{
				_msg.WriteS32(0);
			}
			return _msg;
		}

		public static PageList Serialize(Message _msg)
		{
			PageList pageList = new PageList();
			int num = _msg.ReadS32();
			if (num > 0)
			{
				pageList.novel = new List<PageData>(num);
				for (int i = 0; i < num; i++)
				{
					pageList.novel[i] = MabiNovelSerializer.Serialize(_msg);
				}
			}
			else
			{
				pageList.novel = null;
			}
			return pageList;
		}
	}
}
