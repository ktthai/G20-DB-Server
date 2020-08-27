using Mabinogi;

namespace XMLDB3
{
	public class PersonalRankingListSerializer
	{
		public static void Deserialize(PersonalRankingList _list, Message _message)
		{
			if (_list == null || _list.Items == null || _list.Items.Length == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_list.Items.Length);
			PersonalRanking[] items = _list.Items;
			foreach (PersonalRanking data in items)
			{
				PersonalRankingSerializer.Deserialize(data, _message);
			}
		}
	}
}
