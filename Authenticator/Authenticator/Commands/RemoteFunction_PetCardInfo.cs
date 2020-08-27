using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_PetCardInfo : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			PetCard[] array = PetCard.Load(account);
			if (array != null)
			{
				newReply.WriteU8(1);
				newReply.WriteS32(array.Length);
				PetCard[] array2 = array;
				foreach (PetCard petCard in array2)
				{
					newReply += petCard.ToMessage();
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
			return "Pet Card Info (target:" + str + ")";
		}
	}
}
