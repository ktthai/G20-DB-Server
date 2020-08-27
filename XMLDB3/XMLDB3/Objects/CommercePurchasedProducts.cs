using System.Collections.Generic;

public class CommercePurchasedProducts
{
	public Dictionary<int, CommercePurchasedProduct> productTable;

	public CommercePurchasedProduct[] _productTable
	{
		get
		{
			if (productTable != null)
			{
				CommercePurchasedProduct[] array = new CommercePurchasedProduct[productTable.Values.Count];
				productTable.Values.CopyTo(array, 0);
				return array;
			}
			return null;
		}
		set
		{
			productTable = new Dictionary<int, CommercePurchasedProduct>(value.Length);
			foreach (CommercePurchasedProduct commercePurchasedProduct in value)
			{
				productTable.Add(commercePurchasedProduct.id, commercePurchasedProduct);
			}
		}
	}
}
