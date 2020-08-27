using Mabinogi;

namespace XMLDB3
{
	public class ShopAdvertiseSerializer
	{
		public static ShopAdvertise Serialize(Message _message)
		{
			ShopAdvertise shopAdvertise = new ShopAdvertise();
			shopAdvertise.shopInfo = ShopAdvertisebaseSerializer.Serialize(_message);
			int num = _message.ReadS32();
			if (num == 0)
			{
				shopAdvertise.items = null;
			}
			else
			{
				shopAdvertise.items = new ShopAdvertiseItem[num];
				for (int i = 0; i < num; i++)
				{
					shopAdvertise.items[i] = ShopAdvertiseItemSerializer.Serialize(_message);
				}
			}
			return shopAdvertise;
		}
	}
}
