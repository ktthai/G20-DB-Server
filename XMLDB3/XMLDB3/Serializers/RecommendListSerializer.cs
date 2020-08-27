using Mabinogi;

namespace XMLDB3
{
	public class RecommendListSerializer
	{
		public static void Deserialize(RecommendList _list, Message _message)
		{
			if (_list == null || _list.recommendList == null || _list.recommendList.Count == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_list.recommendList.Count);

			foreach (Recommend data in _list.recommendList)
			{
				RecommendSerializer.DeserializeNewbie(data, _message);
			}
		}
	}
}
