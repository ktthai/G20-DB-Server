using Mabinogi;

namespace Authenticator
{
	public class FantasyLifeClubInfo : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			FantasyLifeClub fantasyLifeClub = FantasyLifeClub.GetFantasyLifeClub(account, ServerConfiguration.IsLocalTestMode);
			if (fantasyLifeClub.Process())
			{
				newReply.WriteU8(1);
				newReply += fantasyLifeClub.ToMessage();
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
			return "Fantasy Life Club Information (target:" + str + ")";
		}
	}
}
