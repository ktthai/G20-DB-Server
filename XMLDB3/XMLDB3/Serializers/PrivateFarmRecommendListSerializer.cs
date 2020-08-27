using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmRecommendListSerializer
	{
		public static void Deserialize(PrivateFarmRecommendList _list, Message _message)
		{
			if (_list == null || _list.recommendList == null || _list.recommendList.Count == 0)
			{
				_message.WriteS32(0);
				return;
			}
			_message.WriteS32(_list.recommendList.Count);

			foreach (PrivateFarmRecommend privateFarmRecommend in _list.recommendList)
			{
				PrivateFarmRecommendSerializer.Deserialize(privateFarmRecommend, _message);
			}
		}
	}
}
