using System;

public class ScrapBookData
{
	public byte scrapType;

	public int classId;

	public int scrapData;

	public int regionId;

	public DateTime updatetime;

	public long Key
	{
		get
		{
			long num = (long)(scrapType + 1) << 32;
			long num2 = classId;
			return num | num2;
		}
	}

	public static long MakeKey(byte _scrapType, int _classId)
	{
		long num = (long)(_scrapType + 1) << 32;
		long num2 = _classId;
		return num | num2;
	}
}
