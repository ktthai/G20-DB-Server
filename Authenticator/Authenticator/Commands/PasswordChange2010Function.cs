using Mabinogi;

namespace Authenticator
{
	public class PasswordChange2010Function : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			PasswordChange2010 passwordChange = new PasswordChange2010(account, ServerConfiguration.IsLocalTestMode);
			if (passwordChange.QueryShowInformation())
			{
				newReply.WriteU8(1);
			}
			else
			{
				newReply.WriteU8(0);
			}
			Reply(newReply);
			return newReply;
		}

		protected override string BuildName(Message _input)
		{
			string str = _input.ReadString();
			return "SecurityInformation Information (target:" + str + ")";
		}
	}
}
