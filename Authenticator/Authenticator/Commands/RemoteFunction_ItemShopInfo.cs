using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_ItemShopInfo : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			string serverName = base.Input.ReadString();
			string characterName = base.Input.ReadString();
			ItemShop itemShop = ItemShop.Load(serverName, characterName);
			if (itemShop != null)
			{
				newReply.WriteU8(1);
				itemShop.ToMessage(newReply);
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
			string text2 = _input.ReadString();
			return "Item Shop Info (target:" + text2 + " at " + text + ")";
		}
	}
}
