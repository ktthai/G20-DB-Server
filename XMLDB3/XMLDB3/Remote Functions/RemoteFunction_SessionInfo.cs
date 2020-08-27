using Mabinogi;

namespace XMLDB3
{
	public class RemoteFunction_SessionInfo : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			WorkSession.WriteStatus("RemoteFunction_SessionInfo.Process() : 세션 정보를 얻습니다");
			SessionStatistics[] statistics = WorkSession.Statistics;
			WorkSession.WriteStatus("RemoteFunction_SessionInfo.Process() : 총 " + statistics.Length + " 개의 세션이 있습니다");
			newReply.WriteS32(statistics.Length);
			SessionStatistics[] array = statistics;
			foreach (SessionStatistics sessionStatistics in array)
			{
				WorkSession.WriteStatus("RemoteFunction_SessionInfo.Process() : " + sessionStatistics.Name + "세션정보를 메시지에 적습니다");
				newReply += sessionStatistics.ToMessage();
			}
			WorkSession.WriteStatus("RemoteFunction_SessionInfo.Process() : 세션정보를 요청을 완료했습니다");
			Reply(newReply);
			return newReply;
		}

		protected override string BuildName(Message _input)
		{
			return "Session Information & Statistics";
		}
	}
}
