using Mabinogi;

namespace XMLDB3
{
	public class GroupIDListSerializer
	{
		public static GroupIDList Serialize(Message _message)
		{
			GroupIDList groupIDList = new GroupIDList();
			int num = _message.ReadS32();
			if (num > 0)
			{
				groupIDList.group = new GroupID[num];
				for (int i = 0; i < num; i++)
				{
					groupIDList.group[i] = new GroupID();
					groupIDList.group[i].charID = _message.ReadS64();
					groupIDList.group[i].groupID = _message.ReadU8();
				}
			}
			else
			{
				groupIDList.group = null;
			}
			return groupIDList;
		}
	}
}
