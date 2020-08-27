using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_PetCardTranCommit : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			long cardid = base.Input.ReadS64();
			long petid = base.Input.ReadS64();
			if (PetCard.Commit_UsingTransaction(account, cardid, petid))
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
			string text = _input.ReadString();
			long num = _input.ReadS64();
			long num2 = _input.ReadS64();
			return "Pet Card Using Transaction Commit (card:" + num + "@" + text + ", target:<" + num2 + ">)";
		}
	}
}
