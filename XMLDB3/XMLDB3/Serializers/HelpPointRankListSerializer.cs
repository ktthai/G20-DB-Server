using Mabinogi;

namespace XMLDB3
{
	public class HelpPointRankListSerializer
	{
		public static void Deserialize(HelpPointRankList _list, Message _message)
		{
			if (_list == null || _list.HelpPointRanks == null || _list.HelpPointRanks.Length == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_list.HelpPointRanks.Length);
			HelpPointRank[] helpPointRanks = _list.HelpPointRanks;
			foreach (HelpPointRank data in helpPointRanks)
			{
				HelpPointRankSerializer.Deserialize(data, _message);
			}
		}

		public static HelpPointRankList Serialize(Message _message)
		{
			HelpPointRankList helpPointRankList = new HelpPointRankList();
			int num = _message.ReadS32();
			if (num > 0)
			{
				helpPointRankList.HelpPointRanks = new HelpPointRank[num];
				for (int i = 0; i < num; i++)
				{
					helpPointRankList.HelpPointRanks[i] = HelpPointRankSerializer.Serialize(_message);
				}
			}
			else
			{
				helpPointRankList.HelpPointRanks = null;
			}
			return helpPointRankList;
		}
	}
}
