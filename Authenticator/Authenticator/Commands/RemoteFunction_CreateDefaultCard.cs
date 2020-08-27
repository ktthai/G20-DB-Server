using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_CreateDefaultCard : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			string type = base.Input.ReadString();
			int num = base.Input.ReadS32();
			bool flag = true;
			for (int i = 0; i < num; i++)
			{
				flag &= CharacterCard.CreateCard(account, type);
			}
			if (flag)
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
			return "Create Default Card(account : " + str + ")";
		}
	}
}
