using Mabinogi;

namespace XMLDB3
{
	public class RemoteFunction_CacheInfo : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			WorkSession.WriteStatus("RemoteFunction_CacheInfo.Process() : 서버의 캐쉬 정보를 얻습니다");
			newReply += ProcessManager.CacheStatisticsToMessage();
			WorkSession.WriteStatus("RemoteFunction_CacheInfo.Process() : 서버의 캐쉬 정보 요청을 완료했습니다");
			Reply(newReply);
			return newReply;
		}

		protected override string BuildName(Message _input)
		{
			return "Cache Statistics";
		}
	}
}
