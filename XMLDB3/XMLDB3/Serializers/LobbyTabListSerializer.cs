using Mabinogi;

namespace XMLDB3
{
	public class LobbyTabListSerializer
	{
		public static LobbyTabList Serialize(Message _message)
		{
			LobbyTabList lobbyTabList = new LobbyTabList();
			int num = _message.ReadS32();
			if (num > 0)
			{
				lobbyTabList.tabInfo = new LobbyTab[num];
				for (int i = 0; i < num; i++)
				{
					lobbyTabList.tabInfo[i] = new LobbyTab();
					lobbyTabList.tabInfo[i].charID = _message.ReadS64();
					lobbyTabList.tabInfo[i].server = _message.ReadString();
					lobbyTabList.tabInfo[i].tab = (_message.ReadU8() == 1);
				}
			}
			return lobbyTabList;
		}
	}
}
