using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_SystemInfo : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			WorkSession.WriteStatus("RemoteFunction_SystemInfo.Process() : 프로세스 사용량을 체크합니다");
			newReply.WriteS32(SystemMonitor.GetProcessorUsage());
			WorkSession.WriteStatus("RemoteFunction_SystemInfo.Process() : 메모리 사용량을 체크합니다");
			newReply.WriteS32(SystemMonitor.GetMemoryUsage());
			WorkSession.WriteStatus("RemoteFunction_SystemInfo.Process() : 시스템 정보 요청을 완료했습니다");
			Reply(newReply);
			return null;
		}

		protected override string BuildName(Message _input)
		{
			return "System Information";
		}
	}
}
