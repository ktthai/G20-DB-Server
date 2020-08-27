using Mabinogi;

namespace XMLDB3
{
	public class PrivateFarmRecommendSerializer
	{
		public static void Deserialize(PrivateFarmRecommend _privateFarmRecommend, Message _message)
		{
			_message.WriteS64(_privateFarmRecommend.FarmZoneID);
			_message.WriteString(_privateFarmRecommend.OwnerCharName);
		}
	}
}
