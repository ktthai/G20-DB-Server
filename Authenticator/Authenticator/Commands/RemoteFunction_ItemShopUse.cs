using Mabinogi;

namespace Authenticator
{
	public class RemoteFunction_ItemShopUse : RemoteFunction
	{
		public override Message Process()
		{
			Message newReply = GetNewReply();
			int orderNumber = base.Input.ReadS32();
			int productNumber = base.Input.ReadS32();
			if (ItemShop.Use(orderNumber, productNumber))
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
			int num = _input.ReadS32();
			int num2 = _input.ReadS32();
			return "Item Shop Use (target:" + num + ":" + num2 + ")";
		}
	}
}
