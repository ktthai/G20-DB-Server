using Mabinogi.SQL.Columns;
using System;
using System.Collections.Generic;

public class GS_CommerceInfo
{
	public long ducat;

	public Dictionary<int, CommerceCredibility> credibilityTable;

	public long unlockTransport;

	public int currentTransport;

	public int lost_percent;

	public DateTime updateTime;

	public CommerceCredibility[] _credibilityTable
	{
		get
		{
			if (credibilityTable != null)
			{
				CommerceCredibility[] array = new CommerceCredibility[credibilityTable.Values.Count];
				credibilityTable.Values.CopyTo(array, 0);
				return array;
			}
			return null;
		}
		set
		{
			credibilityTable = new Dictionary<int, CommerceCredibility>(value.Length);
			foreach (CommerceCredibility commerceCredibility in value)
			{
				credibilityTable.Add(commerceCredibility.postId, commerceCredibility);
			}
		}
	}
}
