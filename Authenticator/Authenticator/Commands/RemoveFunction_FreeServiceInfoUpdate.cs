using Mabinogi;

namespace Authenticator
{
	public class RemoveFunction_FreeServiceInfoUpdate : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			bool free = base.Input.ReadU8() > 0;
			newReply.WriteU8((byte)(FreeService.Update(account, free) ? 1 : 0));
			Reply(newReply);
			return newReply;
		}

		protected override string BuildName(Message _input)
		{
			string text = _input.ReadString();
			byte b = _input.ReadU8();
			return "Free Service Information Update (account:" + text + ",info:" + b + ")";
		}
	}
}
