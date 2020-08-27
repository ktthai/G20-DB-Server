using Mabinogi;
using Mabinogi.Network;

namespace Authenticator
{
	public class RemoteFunction_NetworkInfo : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			WorkSession.WriteStatus("RemoteFunction_NetworkInfo.Process() : 서버의 종합적인 정보를 얻습니다");
			newReply += MainProcedure.ServerInfo.ToMessage();
			WorkSession.WriteStatus("RemoteFunction_NetworkInfo.Process() : 서버의 개별 컨넥션 정보를 얻습니다..");
			ServerInstanceInfo[] connectionInfo = MainProcedure.ConnectionInfo;
			WorkSession.WriteStatus("RemoteFunction_NetworkInfo.Process() : 총 " + connectionInfo.Length + " 개의 개별 컨넥션의 정보를 얻었습니다");
			newReply.WriteS32(connectionInfo.Length);
			ServerInstanceInfo[] array = connectionInfo;
			foreach (ServerInstanceInfo serverInstanceInfo in array)
			{
				WorkSession.WriteStatus("RemoteFunction_NetworkInfo.Process() : 컨넥션 " + serverInstanceInfo.ID + " 의 정보를 얻었습니다");
				newReply += serverInstanceInfo.ToMessage();
			}
			WorkSession.WriteStatus("RemoteFunction_NetworkInfo.Process() : 네트웤 정보 요청을 완료했습니다 ");
			Reply(newReply);
			return newReply;
		}

		protected override string BuildName(Message _input)
		{
			return "Network Information & Statistics";
		}
	}
}
