using System.Collections.Generic;
public class CommerceCriminals
{
	public Dictionary<int, CommerceCriminal> criminalTable;


	public CommerceCriminal[] _criminalTable
	{
		get
		{
			if (criminalTable != null)
			{
				CommerceCriminal[] array = new CommerceCriminal[criminalTable.Values.Count];
				criminalTable.Values.CopyTo(array, 0);
				return array;
			}
			return null;
		}
		set
		{
			criminalTable = new Dictionary<int, CommerceCriminal>(value.Length);
			foreach (CommerceCriminal commerceCriminal in value)
			{
				criminalTable.Add(commerceCriminal.id, commerceCriminal);
			}
		}
	}
}
