using Mabinogi;
using System;

namespace Authenticator
{
	public class RemoteFunction_SessionForceClose : RemoteFunction
	{
		public override Message Process()
		{
			string text = base.Input.ReadString();
			Message newReply = GetNewReply();
			try
			{
				WorkSession.WriteStatus("RemoteFunction_SessionForceClose.Process() : 세션 아이디 " + text + " 의 종료를 시도합니다");
				if (WorkSession.Abort(text))
				{
					WorkSession.WriteStatus("RemoteFunction_SessionForceClose.Process() : 세션 아이디 " + text + " 의 강제 종료 요청을 완료했습니다");
					newReply.WriteU8(1);
				}
				else
				{
					WorkSession.WriteStatus("RemoteFunction_SessionForceClose.Process() : 세션 아이디 " + text + " 의 강제 종료 요청이 실패하였습니다");
					newReply.WriteU8(0);
				}
			}
			catch (Exception ex)
			{
				WorkSession.WriteStatus("RemoteFunction_SessionForceClose.Process() : 세션 아이디 " + text + " 의 강제 종료 요청처리중에 예외가 발생하였습니다.");
				newReply.WriteU8(0);
				ExceptionMonitor.ExceptionRaised(ex);
			}
			Reply(newReply);
			return newReply;
		}

		protected override string BuildName(Message _input)
		{
			return "Work Session Force Closing";
		}
	}
}
