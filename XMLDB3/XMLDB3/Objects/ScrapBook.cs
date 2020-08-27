using System.Collections.Generic;

public class ScrapBook
{
	public long characterId;

	public Dictionary<long, ScrapBookData> scrapBookTable;

	public ScrapBookData[] _scrapBookTable
	{
		get
		{
			if (scrapBookTable != null)
			{
				ScrapBookData[] array = new ScrapBookData[scrapBookTable.Values.Count];
				scrapBookTable.Values.CopyTo(array, 0);
				return array;
			}
			return null;
		}
		set
		{
			scrapBookTable = new Dictionary<long, ScrapBookData>(value.Length);
			foreach (ScrapBookData scrapBookData in value)
			{
				scrapBookTable.Add(scrapBookData.Key, scrapBookData);
			}
		}
	}
}
