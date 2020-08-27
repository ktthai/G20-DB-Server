using System;

public class HelpPointRank
{
	private ulong charIdField;

	private string charNameField;

	private int normalHelpPointField;

	private int accumulatedHelpPointField;

	private DateTime lastUpdateField;


	public ulong charId
	{
		get
		{
			return charIdField;
		}
		set
		{
			charIdField = value;
		}
	}

	public string charName
	{
		get
		{
			return charNameField;
		}
		set
		{
			charNameField = value;
		}
	}

	public int NormalHelpPoint
	{
		get
		{
			return normalHelpPointField;
		}
		set
		{
			normalHelpPointField = value;
		}
	}

	public int AccumulatedHelpPoint
	{
		get
		{
			return accumulatedHelpPointField;
		}
		set
		{
			accumulatedHelpPointField = value;
		}
	}

	public DateTime lastUpdate
	{
		get
		{
			return lastUpdateField;
		}
		set
		{
			lastUpdateField = value;
		}
	}
}
