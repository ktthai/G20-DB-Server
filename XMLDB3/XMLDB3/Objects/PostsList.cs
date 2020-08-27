using System.Collections.Generic;
public class PostsList
{
	private List<Posts> listField;
	public List<Posts> list
	{
		get
		{
			return listField;
		}
		set
		{
			listField = value;
		}
	}
}
