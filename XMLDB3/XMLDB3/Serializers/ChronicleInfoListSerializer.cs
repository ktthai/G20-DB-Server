using Mabinogi;

namespace XMLDB3
{
	public class ChronicleInfoListSerializer
	{
		public static ChronicleInfoList Serialize(Message _message)
		{
			ChronicleInfoList chronicleInfoList = new ChronicleInfoList();
			chronicleInfoList.serverName = _message.ReadString();
			int num = _message.ReadS32();
			if (num > 0)
			{
				chronicleInfoList.infos = new ChronicleInfo[num];
				for (int i = 0; i < num; i++)
				{
					chronicleInfoList.infos[i] = new ChronicleInfo();
					chronicleInfoList.infos[i].questID = _message.ReadS32();
					chronicleInfoList.infos[i].questName = _message.ReadString();
					chronicleInfoList.infos[i].keyword = _message.ReadString();
					chronicleInfoList.infos[i].localtext = _message.ReadString();
					chronicleInfoList.infos[i].sort = _message.ReadString();
					chronicleInfoList.infos[i].group = _message.ReadString();
					chronicleInfoList.infos[i].source = _message.ReadString();
					chronicleInfoList.infos[i].width = _message.ReadS16();
					chronicleInfoList.infos[i].height = _message.ReadS16();
				}
			}
			return chronicleInfoList;
		}
	}
}
