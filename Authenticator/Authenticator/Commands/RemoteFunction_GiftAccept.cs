using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_GiftAccept : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string receiver = base.Input.ReadString();
			string sender = base.Input.ReadString();
			long cardid = base.Input.ReadS64();
			ECardType cardType = (ECardType)base.Input.ReadU8();
			string userIP = base.Input.ReadString();
			CardInterface cardInterface = Gift.AcceptGift(receiver, sender, cardid, cardType, userIP);
			if (cardInterface != null)
			{
				newReply.WriteU8(1);
				newReply += cardInterface.ToMessage();
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
