using System;

namespace XMLDB3
{
	public interface ISection : ICacheItem
	{
		int Count
		{
			get;
		}

		ILinkItem First
		{
			get;
		}

		object[] ToArray();

		Array ToArray(Type type);
	}
}
