using System.Collections.Generic;

public class BestCook
{
	public Dictionary<int, BestCookData> bestCookTable;

	public BestCookData[] _bestCookTable
	{
		get
		{
			if (bestCookTable != null)
			{
				BestCookData[] array = new BestCookData[bestCookTable.Values.Count];
				bestCookTable.Values.CopyTo(array, 0);
				return array;
			}
			return null;
		}
		set
		{
			bestCookTable = new Dictionary<int, BestCookData>(value.Length);
			foreach (BestCookData bestCookData in value)
			{
				bestCookTable.Add(bestCookData.classId, bestCookData);
			}
		}
	}
}
