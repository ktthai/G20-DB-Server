using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_CharacterCardDelete : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			long cardID = base.Input.ReadS64();
			if (CharacterCard.DeleteCard(account, cardID))
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
			return "Character Card Delete (target:" + str + ")";
		}
	}
}
