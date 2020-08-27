using Mabinogi;
using System;

namespace Authenticator
{
	public class RemoteFunction_Test01 : RemoteFunction
	{
		public override Message Process()
		{
			GetNewReply();
			WorkSession.WriteStatus("RemoteFunction_Test01.Process() : 강제로 예외상황을 만듭니다");
			throw new Exception("테스트를 위해 만든 예외입니다 : " + GetHashCode());
		}

		protected override string BuildName(Message _input)
		{
			return "User Function 1";
		}
	}
}
