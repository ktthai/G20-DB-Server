using Mabinogi;
using System;

namespace Authenticator
{
	public class RemoteFunction_NetworkForceClose : RemoteFunction
	{
		public override Message Process()
		{
			int num = base.Input.ReadS32();
			Message newReply = GetNewReply();
			try
			{
				WorkSession.WriteStatus("RemoteFunction_NetworkForceClose.Process() : 컨넥션 아이디 " + num + " 의 종료를 시도합니다");
				MainProcedure.CloseConnection(num);
				WorkSession.WriteStatus("RemoteFunction_NetworkForceClose.Process() : 컨넥션 아이디 " + num + " 의 강제 종료 요청을 완료했습니다");
				newReply.WriteU8(1);
			}
			catch (Exception ex)
			{
				WorkSession.WriteStatus("RemoteFunction_NetworkForceClose.Process() : 컨넥션 아이디 " + num + " 의 강제 종료 요청 처리중에 예외가 발생하였습니다");
				newReply.WriteU8(0);
				ExceptionMonitor.ExceptionRaised(ex);
			}
			Reply(newReply);
			return newReply;
		}

		protected override string BuildName(Message _input)
		{
			return "Network Connectoin Force Closing";
		}
	}
}
