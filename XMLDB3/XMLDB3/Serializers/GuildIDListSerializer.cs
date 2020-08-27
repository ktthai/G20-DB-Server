using Mabinogi;

namespace XMLDB3
{
	public class GuildIDListSerializer
	{
		public static void Deserialize(GuildIDList _list, Message _messsage)
		{
			if (_list == null)
			{
				_list = new GuildIDList();
			}
			if (_list.guildID != null)
			{
				_messsage.WriteS32(_list.guildID.Count);

				foreach (long data in _list.guildID)
				{
					_messsage.WriteS64(data);
				}
			}
			else
			{
				_messsage.WriteU32(0u);
			}
		}
	}
}
