using System;

public class Posts
{
	private long snField;

	private string authorField;

	private long authorIdField;

	private string titleField;

	private uint transcriptionCountField;

	private DateTime endDateField;

	private uint blockCountField;

	private uint optionField;

	private uint readingCountField;

	private DateTime updateTimeField;

	public long sn
	{
		get
		{
			return snField;
		}
		set
		{
			snField = value;
		}
	}

	public string author
	{
		get
		{
			return authorField;
		}
		set
		{
			authorField = value;
		}
	}

	public long authorId
	{
		get
		{
			return authorIdField;
		}
		set
		{
			authorIdField = value;
		}
	}

	public string title
	{
		get
		{
			return titleField;
		}
		set
		{
			titleField = value;
		}
	}

	public uint transcriptionCount
	{
		get
		{
			return transcriptionCountField;
		}
		set
		{
			transcriptionCountField = value;
		}
	}

	public DateTime endDate
	{
		get
		{
			return endDateField;
		}
		set
		{
			endDateField = value;
		}
	}

	public uint blockCount
	{
		get
		{
			return blockCountField;
		}
		set
		{
			blockCountField = value;
		}
	}

	public uint option
	{
		get
		{
			return optionField;
		}
		set
		{
			optionField = value;
		}
	}

	public uint readingCount
	{
		get
		{
			return readingCountField;
		}
		set
		{
			readingCountField = value;
		}
	}

	public DateTime updateTime
	{
		get
		{
			return updateTimeField;
		}
		set
		{
			updateTimeField = value;
		}
	}
}
