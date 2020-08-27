using Mabinogi;

namespace XMLDB3
{
	public class RemoteFunction_ExceptionInfo : RemoteFunction
	{
		public override Message Process()
		{
			int startIdx = base.Input.ReadS32();
			int count = base.Input.ReadS32();
			Message newReply = GetNewReply();
			WorkSession.WriteStatus("RemoteFunction_ExceptionInfo.Process() : 처리된 예외 정보를 얻습니다");
			newReply += ExceptionMonitor.ToMessage(startIdx, count);
			WorkSession.WriteStatus("RemoteFunction_ExceptionInfo.Process() : 처리된 예외 정보 요청을 완료했습니다 ");
			Reply(newReply);
			return newReply;
		}

		protected override string BuildName(Message _input)
		{
			return "Exception Information";
		}
	}
}
