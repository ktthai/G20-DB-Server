using System.Collections.Generic;

public class CommerceCriminalInfo
{
	public Dictionary<string, CCommerceCriminalLost>  criminalTable;

	public CCommerceCriminalLost[] _criminalTable
	{
		get
		{
			if (criminalTable != null)
			{
				CCommerceCriminalLost[] array = new CCommerceCriminalLost[criminalTable.Values.Count];
				criminalTable.Values.CopyTo(array, 0);
				return array;
			}
			return null;
		}
		set
		{
			criminalTable = new Dictionary<string, CCommerceCriminalLost>(value.Length);
			foreach (CCommerceCriminalLost cCommerceCriminalLost in value)
			{
				criminalTable.Add(cCommerceCriminalLost.charName, cCommerceCriminalLost);
			}
		}
	}
}
