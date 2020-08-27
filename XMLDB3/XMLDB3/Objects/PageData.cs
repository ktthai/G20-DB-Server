using System;

public class PageData
{
	private long snField;

	private ushort pageField;

	private ushort bgIdField;

	private ushort bgmIdField;

	private ushort portraitIdField;

	private ushort portraitPosField;

	private ushort emotionIdField;

	private ushort soundEffectIdField;

	private ushort effectIdField;

	private string ambassadorField;

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

	public ushort page
	{
		get
		{
			return pageField;
		}
		set
		{
			pageField = value;
		}
	}

	public ushort bgId
	{
		get
		{
			return bgIdField;
		}
		set
		{
			bgIdField = value;
		}
	}

	public ushort bgmId
	{
		get
		{
			return bgmIdField;
		}
		set
		{
			bgmIdField = value;
		}
	}

	public ushort portraitId
	{
		get
		{
			return portraitIdField;
		}
		set
		{
			portraitIdField = value;
		}
	}

	public ushort portraitPos
	{
		get
		{
			return portraitPosField;
		}
		set
		{
			portraitPosField = value;
		}
	}

	public ushort emotionId
	{
		get
		{
			return emotionIdField;
		}
		set
		{
			emotionIdField = value;
		}
	}

	public ushort soundEffectId
	{
		get
		{
			return soundEffectIdField;
		}
		set
		{
			soundEffectIdField = value;
		}
	}

	public ushort effectId
	{
		get
		{
			return effectIdField;
		}
		set
		{
			effectIdField = value;
		}
	}

	public string ambassador
	{
		get
		{
			return ambassadorField;
		}
		set
		{
			ambassadorField = value;
		}
	}
}
