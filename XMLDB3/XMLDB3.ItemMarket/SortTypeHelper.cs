namespace XMLDB3.ItemMarket
{
	public class SortTypeHelper
	{
		public static IMSortingType GetSortingType(int sortType)
		{
			switch (sortType)
			{
			case 1:
			case 2:
				return IMSortingType.ItemName;
			case 3:
			case 4:
				return IMSortingType.Price;
			case 5:
			case 6:
				return IMSortingType.Saler;
			default:
				return IMSortingType.ExpireDate;
			}
		}

		public static bool GetAscendingType(int sortType)
		{
			return sortType % 2 == 1;
		}
	}
}
