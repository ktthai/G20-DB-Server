using Mabinogi;

namespace Authenticator
{
	public class RemoveFunction_FreeServiceInfo : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			newReply.WriteU8((byte)(FreeService.FindInfo(account) ? 1 : 0));
			Reply(newReply);
			return newReply;
		}

		protected override string BuildName(Message _input)
		{
			string str = _input.ReadString();
			return "Free Service Information (target:" + str + ")";
		}
	}
}
