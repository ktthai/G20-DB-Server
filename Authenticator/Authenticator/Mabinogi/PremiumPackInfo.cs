using Mabinogi;

namespace Authenticator
{
	public class PremiumPackInfo : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			PremiumPack premiumPack = PremiumPack.GetPremiumPack(account, ServerConfiguration.IsLocalTestMode);
			if (premiumPack.Process())
			{
				newReply.WriteU8(1);
				newReply += premiumPack.ToMessage();
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
			return "PremiumPack Information (target:" + str + ")";
		}
	}
}
