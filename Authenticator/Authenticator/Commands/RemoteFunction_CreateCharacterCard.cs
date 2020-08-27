using Mabinogi;
using System;

namespace Authenticator
{
	public class RemoteFunction_CreateCharacterCard : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string account = base.Input.ReadString();
			string type = base.Input.ReadString();
			ulong ticks = base.Input.ReadU64();
			uint num = base.Input.ReadU32();
			string[] array = new string[num];
			for (uint num2 = 0u; num2 < num; num2++)
			{
				array[num2] = base.Input.ReadString();
			}
			string validServer = string.Empty;
			if (num != 0)
			{
				validServer = "<servers>";
				for (uint num3 = 0u; num3 < num; num3++)
				{
					validServer = validServer + "<server name=\"" + array[num3] + "\" />";
				}
				validServer += "</servers>";
			}
			if (CharacterCard.CreateCardWithServer(account, type, new DateTime((long)ticks), array))
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
