using System.Collections.Generic;
public class PageList
{
	private List<PageData> novelField;
	public List<PageData> novel
	{
		get
		{
			return novelField;
		}
		set
		{
			novelField = value;
		}
	}
}
