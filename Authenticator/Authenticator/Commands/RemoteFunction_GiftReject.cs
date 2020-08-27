using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_GiftReject : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			long cardid = base.Input.ReadS64();
			ECardType cardType = (ECardType)base.Input.ReadU8();
			if (Gift.RejectGift(account, cardid, cardType))
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
			return "Gift Accept (target:" + str + ")";
		}
	}
}
