using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_GiftInfo : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			Gift[] array = Gift.Load(account);
			if (array != null)
			{
				newReply.WriteU8(1);
				newReply.WriteS32(array.Length);
				Gift[] array2 = array;
				foreach (Gift gift in array2)
				{
					newReply += gift.ToMessage();
				}
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
			return "Gift Info (target:" + str + ")";
		}
	}
}
