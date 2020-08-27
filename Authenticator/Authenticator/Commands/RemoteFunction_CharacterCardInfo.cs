using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_CharacterCardInfo : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			CharacterCard[] array = CharacterCard.Load(account);
			if (array != null)
			{
				newReply.WriteU8(1);
				newReply.WriteS32(array.Length);
				CharacterCard[] array2 = array;
				foreach (CharacterCard characterCard in array2)
				{
					newReply += characterCard.ToMessage();
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
			return "Character Card Info (target:" + str + ")";
		}
	}
}
