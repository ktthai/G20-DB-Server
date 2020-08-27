
public class ShopAdvertise
{
	public ShopAdvertisebase shopInfo;

	public ShopAdvertiseItem[] items;

	protected bool IsValid()
	{
		if (shopInfo != null && shopInfo.account != null)
		{
			return shopInfo.server != null;
		}
		return false;
	}

	public override string ToString()
	{
		return base.ToString() + ":" + (IsValid() ? (shopInfo.server + ":" + shopInfo.account) : "invalid");
	}
}
